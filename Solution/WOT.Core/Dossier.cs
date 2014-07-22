using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WOTStatistics.SQLite;
using System.Data;


namespace WOTStatistics.Core
{
    public class Dossier : IDisposable
    {
        WOTStats _global = null;
        private int _fileID;
        private string _playerID;
        MessageQueue _message;
        DataTable kills_dt = new DataTable();


        //public Dictionary<string, double> oOverall = new Dictionary<string, double>();

        public Dossier(int fileID, string playerID, MessageQueue message)
        {
            _playerID = playerID;
            //WOTHelper.AddToLog("PL " + _playerID);
            _message = message;
            _global = new WOTStats();
            _fileID = fileID;
            GetStats(true);
        }

        public Dossier(MessageQueue message)
        {
            _message = message;
            _global = new WOTStats();
            _fileID = 0;
            
        }

        internal void GetStats(bool fill)
        {
            if (_fileID > 0)
            {
                FillStats();
            }
        }

        public WOTStats GetStats()
        {
            return _global;
        }

        private void FillStats()
        {

            try
            {
                double RatingWN8_TotalTanks = 0;
                double RatingWN8_TotalTier = 0;
                double RatingWN8_TotalWins = 0;
                double RatingWN8_TotalBattles = 0;
                double RatingWN8_TotalDamage = 0;
                double RatingWN8_TotalFrag = 0;
                double RatingWN8_TotalSpot = 0;
                double RatingWN8_TotalDef = 0;
                double RatingWN8_TotalWinrate = 0;

                double RatingWN8_ExpDamage = 0;
                double RatingWN8_ExpFrag = 0;
                double RatingWN8_ExpSpot = 0;
                double RatingWN8_ExpDef = 0;
                double RatingWN8_ExpWinrate = 0;



                //oOverall.Clear();
                //using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(_playerID), true))
                //{
                //    using (DataTable dt = db.GetDataTable("select * from Overall"))
                //    {
                //        if (dt.Rows.Count == 0)
                //        {

                //            db.ExecuteNonQuery("insert into Overall (ovRatingWN8) VALUES (0)");

                //            foreach (DataColumn oColumn in dt.Columns)
                //            {
                //                WOTHelper.AddToLog("Overall " + oColumn.Caption + ": New");
                //                oOverall.Add(oColumn.Caption, 0);
                //            }
                //        }
                //        else
                //        {
                //            foreach (DataRow oRow in dt.Rows)
                //            {
                //                foreach (DataColumn oColumn in dt.Columns)
                //                {
                //                    WOTHelper.AddToLog("Overall " + oColumn.Caption + ": " + oRow.GetSafeDouble(oColumn.Caption));
                //                    oOverall.Add(oColumn.Caption, oRow.GetSafeDouble(oColumn.Caption));
                //                }
                //            }
                //        }
                //    }
                //}

                
                CountryDescriptions countryDescriptions = new CountryDescriptions(_message);
                TankDescriptions tankDescription = new TankDescriptions(_message);
                string sql = @"   select
                    T1.cmID, T1.cmCountryID, T1.cmTankID, T1.cmUpdated, 
                    sum(T2.bpBattleCount) bpBattleCount, 
                    sum(t2.bpSpotted) bpSpotted, 
                    sum(t2.bpHits) bpHits, 
                    max(t2.bpMaxFrags) bpMaxFrags, 
                    max(t2.bpMaxXP) bpMaxXP, 
                    sum(t2.bpWins) bpWins, 
                    sum(t2.bpCapturePoints) bpCapturePoints, 
                    sum(t2.bpLosses) bpLosses,
                    sum(t2.bpSurvivedBattles) bpSurvivedBattles, 
                    min(T4.foBattleLifeTime) foBattleLifeTime, 
                    sum(t2.bpDefencePoints) bpDefencePoints, 
                    min(t4.foLastBattleTime) foLastBattleTime, 
                    sum(t2.bpDamageReceived) bpDamageReceived, 
                    sum(t2.bpShots) bpShots, 
                    sum(t2.bpWinAndSurvive) bpWinAndSurvive, 
                    sum(t2.bpFrags8P) bpFrags8P,
                    sum(t2.bpDamageDealt) bpDamageDealt, 
                    sum(t2.bpDamageAssistedRadio) bpDamageAssistedRadio,
                    sum(t2.bpDamageAssistedTracks) bpDamageAssistedTracks,
                    sum(t2.bpRatingEffWeight)  bpRatingEffWeight,
                    sum(t2.bpRatingBRWeight)  bpRatingBRWeight,
                    sum(t2.bpRatingWN7Weight)  bpRatingWN7Weight,
                    sum(t2.bpRatingWN8Weight)  bpRatingWN8Weight,
                    sum(t2.bpRatingEff) bpRatingEff,
                    sum(t2.bpRatingBR) bpRatingBR,
                    sum(t2.bpRatingWN7) bpRatingWN7,
                    sum(t2.bpRatingWN8) bpRatingWN8,
                    sum(t2.bpXP) bpXP, 
                    sum(t2.bpMileage) bpMileage, 
                    sum(t2.bpFrags) bpFrags, 
                    t4.foTreesCut, 
                    t3.faMarkOfMastery
                    from File_TankDetails T1
                    inner join File_Battles T2
                    on T1.cmID = T2.bpParentID
                    inner join File_Achievements T3
                    on T1.cmID = T3.faParentID
                    inner join File_Total T4
                    on T1.cmID = T4.foParentID
                    where bpBattleCount>0 and cmFileID =  " + _fileID;

                if (UserSettings.BattleMode == "15")
                    sql += @" and ifnull(bpBattleMode,0) in (15,0)";
                else if(UserSettings.BattleMode == "7")
                    sql += @" and ifnull(bpBattleMode,0) in (7)";




                sql += " group by T1.cmID, T1.cmCountryID, T1.cmTankID, T1.cmUpdated, t4.foTreesCut, t3.faMarkOfMastery";
                WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();
                using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(_playerID), true))
                {
                    //WOTHelper.AddToLog("SQL: " + sql);
                    using (DataTable dt = db.GetDataTable(sql))
                    {
                        var d = (from x in dt.AsEnumerable()
                                 select x["cmId"]).ToArray();

                        foreach (DataRow item in dt.Rows)
                        {
                            int countryID = item.GetSafeInt("cmCountryID");
                            int tankID = item.GetSafeInt("cmTankID");
                            if (tankDescription.Active(countryID, tankID))
                            {

                                WN8ExpValue WN8ExpectedTank = null;
                                try
                                {
                                    WN8ExpectedTank = WN8ExpectedTankList.GetByTankID(countryID, tankID);
                                }
                                catch (Exception ex)
                                {
                                    WOTHelper.AddToLog("ExpTank: "  + ex.Message);
                                }


                                if (WN8ExpectedTank == null)
                                {
                                    WOTHelper.AddToLog("WN8ExpectedTank is null: tankID " + tankID + " countryID " + countryID);
                                }


                                Tank tank = new Tank(_global) { TankID = tankID, 
                                                                  CountryID = countryID, 
                                                                  Updated = item.GetSafeInt("cmUpdated"), 
                                                                  Tier = tankDescription.Tier(countryID, tankID), 
                                                                  Tank_Description = tankDescription.Description(countryID, tankID), 
                                                                  Premium = tankDescription.Premium(countryID, tankID), 
                                                                  Country_Description = countryDescriptions.Description(countryID), 
                                                                    OverallRatingEff = 0,
                                                                    OverallRatingBR = 0,
                                                                    OverallRatingWN7 = 0,
                                                                    OverallRatingWN8 = 0,
                                                                  TankClass = tankDescription.TankType(countryID, tankID) };
                                tank.Data = new TankData(tank) { Spotted = item.GetSafeInt("bpSpotted"), 
                                                                Hits = item.GetSafeInt("bpHits"), 
                                                                MaxFrags = item.GetSafeInt("bpMaxFrags"), 
                                                                BeastFrags = 0, 
                                                                MaxXp = item.GetSafeInt("bpMaxXP"), 
                                                                Victories = item.GetSafeInt("bpWins"), 
                                                                CapturePoints = item.GetSafeInt("bpCapturePoints"), 
                                                                Defeats = item.GetSafeInt("bpLosses"), 
                                                                Survived = item.GetSafeInt("bpSurvivedBattles"), 
                                                                BattleLifeTime = item.GetSafeInt("foBattleLifeTime"), 
                                                                DefencePoints = item.GetSafeInt("bpDefencePoints"), 
                                                                BattlesCount = item.GetSafeInt("bpBattleCount"), 
                                                                LastBattleTime = item.GetSafeInt("foLastBattleTime"), 
                                                                DamageReceived = item.GetSafeInt("bpDamageReceived"), 
                                                                Shots = item.GetSafeInt("bpShots"), 
                                                                VictoryAndSurvived = item.GetSafeInt("bpWinAndSurvive"), 
                                                                Tier8upFrags = item.GetSafeInt("bpFrags8P"), 
                                                                DamageDealt = item.GetSafeInt("bpDamageDealt"),
                                                                DamageAssistedRadio = item.GetSafeInt("bpDamageAssistedRadio"),
                                                                DamageAssistedTracks = item.GetSafeInt("bpDamageAssistedTracks"), 
                                                                Xp = item.GetSafeInt("bpXP"),
                                                                Mileage = item.GetSafeInt("bpMileage"),
                                                                RatingEff = item.GetSafeInt("bpRatingEff"),
                                                                RatingBR = item.GetSafeInt("bpRatingBR"),
                                                                RatingWN7 = item.GetSafeInt("bpRatingWN7"),
                                                                RatingWN8 = item.GetSafeInt("bpRatingWN8"),
                                                                Frags = item.GetSafeInt("bpFrags"), 
                                                                TreesCut = item.GetSafeInt("foTreesCut") };

                                RatingWN8_TotalTier += tankDescription.Tier(countryID, tankID) * item.GetSafeInt("bpBattleCount");
                                RatingWN8_TotalTanks += 1;

                                RatingWN8_TotalBattles += item.GetSafeInt("bpBattleCount");
                                RatingWN8_TotalWins += item.GetSafeInt("bpWins");
                                
                                RatingWN8_TotalDamage += item.GetSafeInt("bpDamageDealt");

                                RatingWN8_ExpDamage += WN8ExpectedTank.expDamage * item.GetSafeInt("bpBattleCount");

                                RatingWN8_TotalFrag += item.GetSafeInt("bpFrags");
                                RatingWN8_ExpFrag += WN8ExpectedTank.expFrag * item.GetSafeInt("bpBattleCount");

                                RatingWN8_TotalSpot += item.GetSafeInt("bpSpotted");
                                RatingWN8_ExpSpot += WN8ExpectedTank.expSpot * item.GetSafeInt("bpBattleCount");

                                RatingWN8_TotalDef += item.GetSafeInt("bpDefencePoints");
                                RatingWN8_ExpDef += WN8ExpectedTank.expDefense * item.GetSafeInt("bpBattleCount");

                                RatingWN8_ExpWinrate += (WN8ExpectedTank.expWin)/100 * item.GetSafeInt("bpBattleCount");
                                

                                tank.Epics = new Epics(tank) { Orlik = 0, Burda = 0, Oskin = 0, Halonen = 0, Billotte = 0, TamadaYoshio = 0, Horoshilov = 0, Lister = 0, HeroesOfRassenai = 0, Kolobanov = 0, Fadin = 0, Erohin = 0, DeLaglanda = 0, Boelter = 0 };

                                //if (UserSettings.BattleMode == "All")
                                //{
                                    if (kills_dt.Rows.Count <= 0)
                                    {
                                        string sqlFrags = @"select fgParentID, fgCountryID, fgTankID, fgValue from File_FragList where fgParentID in (" + string.Join(",", d) + ")";
                                        kills_dt = db.GetDataTable(sqlFrags);
                                    }

                                    foreach (DataRow frag in kills_dt.Select("fgParentID = " + item.GetSafeInt("cmId")))
                                    {
                                        int kCountryID = frag.GetSafeInt("fgCountryID");
                                        int kTankID = frag.GetSafeInt("fgTankID");
                                        FragCount fc = new FragCount(tank) { CountryID = kCountryID, TankID = kTankID, Tank_Description = tankDescription.Description(kCountryID, kTankID), Country_Description = countryDescriptions.Description(kCountryID), TankClass = tankDescription.TankType(kCountryID, kTankID), Tier = tankDescription.Tier(kCountryID, kTankID), frags = frag.GetSafeInt("fgValue") };
                                        tank.FragList.Add(fc);
                                    }
                                //}

                                tank.Battle = new AwardsBattle(tank) { SteelWall = 0, Warrior = 0, BattleHeroes = 0, Scout = 0, Invader = 0, Defender = 0, EvilEye = 0, Supporter = 0, Sniper = 0 };
                                tank.Special = new AwardSpecial(tank) { ArmorPiercer = 0, MouseBane = 0, BeastHunter = 0, MarkOfMastery = item.GetSafeInt("faMarkOfMastery"), Kamikaze = 0, Raider = 0, Invincible = 0, TankExpert = 0, Diehard = 0, HandOfDeath = 0, LumberJack = 0, Sniper = 0 };
                                tank.Major = new AwardsMajor(tank) { Ekins = 0, Abrams = 0, Carius = 0, Poppel = 0, Kay = 0, LeClerc = 0, Knispel = 0, Lavrinenko = 0 };
                                tank.Series = new AwardSeries(tank) { Diehard = 0, Invincible = 0, Killing = 0, Piercing = 0, Sniper = 0, Max_Diehard = 0, Max_Invincible = 0, Max_Killing = 0, Max_Piercing = 0, Max_Sniper = 0 };
                                _global.tanks.Add(tank);
                            }
                            else
                            {
                                //string parkhere = "";
                            }
                        }
                    }
                }
           
                RatingWN8_TotalWinrate = RatingWN8_TotalWins;

                //Console.WriteLine("Total Battle " + RatingWN8_TotalBattles);
                //Console.WriteLine("Total Wins   " + RatingWN8_TotalWins);
                //Console.WriteLine("Total Damage " + RatingWN8_TotalDamage);
                //Console.WriteLine("EXP   Damage " + RatingWN8_ExpDamage);
                //Console.WriteLine("Total Frag   " + RatingWN8_TotalFrag);
                //Console.WriteLine("EXP   Frag   " + RatingWN8_ExpFrag);
                //Console.WriteLine("Total Spot   " + RatingWN8_TotalSpot);
                //Console.WriteLine("EXP   Spot   " + RatingWN8_ExpSpot);
                //Console.WriteLine("Total Def    " + RatingWN8_TotalDef);
                //Console.WriteLine("EXP   Def    " + RatingWN8_ExpDef);
                //Console.WriteLine("Total Win    " + RatingWN8_TotalWinrate);
                //Console.WriteLine("EXP   Win    " + RatingWN8_ExpWinrate);

             

                double rDAMAGE = RatingWN8_TotalDamage / RatingWN8_ExpDamage;
                double rFRAG = RatingWN8_TotalFrag/ RatingWN8_ExpFrag;
                double rSPOT = RatingWN8_TotalSpot / RatingWN8_ExpSpot;
                double rDEF = RatingWN8_TotalDef / RatingWN8_ExpDef;
                double rWIN = RatingWN8_TotalWinrate / RatingWN8_ExpWinrate;

                //Console.WriteLine("r Damage " + rDAMAGE);
                //Console.WriteLine("r Frag   " + rFRAG);
                //Console.WriteLine("r Spot   " + rSPOT);
                //Console.WriteLine("r Def    " + rDEF);
                //Console.WriteLine("r Win    " + rWIN);


                //rDAMAGEc = max(0,                     (rDAMAGE - 0.22) / (1 - 0.22) )    
                //rFRAGc   = max(0, min(rDAMAGEc + 0.2, (rFRAG   - 0.12) / (1 - 0.12)))
                //rSPOTc   = max(0, min(rDAMAGEc + 0.1, (rSPOT   - 0.38) / (1 - 0.38)))
                //rDEFc    = max(0, min(rDAMAGEc + 0.1, (rDEF    - 0.10) / (1 - 0.10)))
                //rWINc    = max(0,                     (rWIN    - 0.71) / (1 - 0.71) )
                double rDAMAGEc = Math.Max(0, (rDAMAGE - 0.22) / (1 - 0.22));
                double rFRAGc = Math.Max(0, Math.Min(rDAMAGEc + 0.2, (rFRAG - 0.12) / (1 - 0.12)));
                double rSPOTc = Math.Max(0, Math.Min(rDAMAGEc + 0.1, (rSPOT - 0.38) / (1 - 0.38)));
                double rDEFc = Math.Max(0, Math.Min(rDAMAGEc + 0.1, (rDEF - 0.10) / (1 - 0.10)));
                double rWINc = Math.Max(0, (rWIN - 0.71) / (1 - 0.71));


                //980*rDAMAGEc 
                //210*rDAMAGEc*rFRAGc 
                //155*rFRAGc*rSPOTc 
                //75*rDEFc*rFRAGc 
                //145*MIN(1.8,rWINc)
                double eDAMAGE = 980 * rDAMAGEc;
                double eFRAG = 210 * rDAMAGEc * rFRAGc;
                double eSPOT = 155 * rFRAGc * rSPOTc;
                double eDEF = 75 * rDEFc * rFRAGc;
                double eWIN = 145 * Math.Min(1.8, rWINc);

                double WN8 = eDAMAGE +eFRAG + eSPOT + eDEF + eWIN;
                //Console.WriteLine("WN8!: " + WN8);


                double avgTier = RatingWN8_TotalTier / RatingWN8_TotalBattles;
                double avgWinRate = RatingWN8_TotalWins / RatingWN8_TotalBattles;
                double avgFrags= RatingWN8_TotalFrag / RatingWN8_TotalBattles;
                double avgDamage= RatingWN8_TotalDamage / RatingWN8_TotalBattles;
                double avgSpot= RatingWN8_TotalSpot / RatingWN8_TotalBattles;
                double avgDef= RatingWN8_TotalDef / RatingWN8_TotalBattles;


                double WN7_Frags = (1240 - 1040 / Math.Pow(Math.Min(avgTier, 6), 0.164)) * (avgFrags);
                double WN7_Damage = avgDamage * 530 / (184 * Math.Exp(0.24 * avgTier) + 130);

                double WN7_Spot = (avgSpot) * 125 * (Math.Min(avgTier, 3)) / 3;
                double WN7_Def = Math.Min(avgDef, 2.2) * 100;
                double WN7_Winrate = (((185 / (0.17 + Math.Exp(((avgWinRate * 100) - 35) * -0.134))) - 500) * 0.45);
                double WN7_Malus =  (((5 - Math.Min(avgTier, 5)) * 125) / (1 + Math.Exp(avgTier - Math.Pow(RatingWN8_TotalBattles / 220, 3 / avgTier)) * 1.5));

                double WN7 = WN7_Frags + WN7_Damage + WN7_Spot + WN7_Def + WN7_Winrate - WN7_Malus;
//                Console.WriteLine("WN7!: " + WN7);

//                using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(_playerID), true))
//                {

//                    Console.WriteLine(oOverall["ovRatingWN7"]);


//                    string sUpdate = string.Format(@"update Overall set ovRatingWN7='{0}'", 556767);
//                    db.ExecuteNonQuery(sUpdate);

////                    string sUpdate = string.Format(@"update Overall set 
////                                                ovRatingWN7='{0}',
////                                                ovRatingWN7Prev='{1}',
////                                                ovRatingWN8='{2}',
////                                                ovRatingWN8Prev='{3}',
////                                                ovRatingWinRate='{4}'
////                                                ovRatingWinRatePrev='{5}'
////                                                WHERE bpParentID={6}",
////                                                    rDamage,
////                                                    rFrag,
////                                                    rSpot,
////                                                    rDef,
////                                                    rWin,
////                                                    item.GetSafeInt("bpParentID")
////                                                    );
////                    //WOTHelper.AddToLog("Update: " + sUpdate);
////                    db.ExecuteNonQuery(sUpdate);
//                }


                //WOTHelper.AddToLog("Adding Ratings...");
                _global.OverallRatingWN7 = WN7;
                _global.OverallRatingWN8 = WN8;

                //foreach (Tank oTank in _global.tanks)
                //{
                //    oTank.OverallRatingWN7 = WN7;
                //    oTank.OverallRatingWN8 = WN8;

                //    oTank.Data.OverallRatingWN7 = WN7;
                //    oTank.Data.OverallRatingWN8 = WN8;
                //}
                //WOTHelper.AddToLog("Adding Ratings... Done!");
                countryDescriptions.Dispose();
                tankDescription.Dispose();
            }
            catch (Exception ex)
            {
                _message.Add("Error : Cannot parse json file. - " + ex.Message);
            }
        }

        public void Dispose()
        {
            _global = null;
        }
     
    }

    /* 
     * Request from Rudi
    - Tank Tier []
    - Tank [Tank_Description]
    - Total Battles [battles]
    - Total Victories [victories]
    - Victory % [victories / battles * 100]
    - Total Survived [survived]
    - Total Survived with Victory [survived_with_victory]
    - Survived with Victory % [survived_with_victory / battles * 100] 
    - Total Losses [losses]
    - Lost % [losses / battles * 100]
    - Total Experience [experience]
    - New Max Experience [max_experience] (only show new value if better than previous)
    - Total Damage Taken [damage_received]
    - New Max Destroyed [max_frags] (only show new value if better than previous)
    - Accuracy % [hits / shots * 100]
    - Playing Time [play_time]

     Efficiency Related
    - Total Destroyed [frags]
    - Ave Destroyed [frags / battles]
    - Total Damage Dealt [damage_dealt]
    - Ave Damage Dealt [damage_dealt / battles]
    - Total Spotted [spotted]
    - Ave Spotted [spotted / battles]
    - Total Capture Points [capture_points]
    - Ave Capture Points [capture_points / battles]
    - Total Defense Points [defence_points]
    - Ave Defense Points [defence_points / battles]
    - Efficiency Rating [Still need equation/formula]
     
      */

    //public class EffRatingParameters
    //{
    //    public string EffRating { get; set; }
    //    public double BattleCount { get; set; }
    //    public double TotalDamageDealt { get; set; }
    //    public double damageAssistedRadio { get; set; }
    //    public double damageAssistedTrack { get; set; }
    //    public double avgTier { get; set; }
    //    public double frags { get; set; }
    //    public double spotted { get; set; }
    //    public double capture { get; set; }
    //    public double defence { get; set; }
    //    public double winRate { get; set; }
    //    public bool singleTank { get; set; }
    //    public double globalAvgTier { get; set; }
    //    public double globalAvgDefPoints { get; set; }
    //    public double globalBattlesCount { get; set; }
    //}


    public class WOTStats
    {
        public double OverallRatingEff { get; set; }
        public double OverallRatingBR { get; set; }
        public double OverallRatingWN7 { get; set; }
        public double OverallRatingWN8 { get; set; }

        public ArrayList records { get; set; }
        public int version { get; set; }
        public int extractor_version { get; set; }
        public List<Tank> tanks { get; set; }
        public int BattlesCount
        {
            get
            {
                int val = (from n in tanks
                           select n.Data.BattlesCount).Sum();
                return val;
            }
        }
        public int Victories
        {
            get
            {
                int val = (from n in tanks
                           select n.Data.Victories).Sum();
                return val;
            }
        }
        public int Survived
        {
            get
            {
                int val = (from n in tanks
                           select n.Data.Survived).Sum();
                return val;
            }
        }
        public int Losses
        {
            get
            {
                int val = (from n in tanks
                           select n.Data.Defeats).Sum();
                return val;
            }
        }
        public int Draws
        {
            get
            {
                int val = (from n in tanks
                           select n.Data.Draws).Sum();
                return val;
            }
        }
        public double Victory_Ratio
        {
            get
            {
                return (((double)Victories / (double)BattlesCount));
            }
        }
        public double Victory_Percentage
        {
            get
            {
                return (((double)Victories / (double)BattlesCount) * 100);
            }
        }
        public double Survived_Percentage
        {
            get
            {
                return (((double)Survived / (double)BattlesCount) * 100);
            }
        }
        public double Losses_Percentage
        {
            get
            {
                return (((double)Losses / (double)BattlesCount) * 100);
            }
        }
        public double Draws_Percentage
        {
            get
            {
                return (((double)Draws / (double)BattlesCount) * 100);
            }
        }
        public double RatingEff
        {
            get
            {
                try
                {
                    double Eff =  (from n in tanks
                            select n.Data.RatingEff).Average();
                    return (int)Eff;
                }
                catch
                {
                    return 0;
                }

            }
        }
        public double RatingBR
        {
            get
            {
                try
                {
                    double Eff = (from n in tanks
                                  select n.Data.RatingBR).Average();
                    return (int)Eff;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public double RatingWN7
        {
            get
            {
                try
                {
                    double Eff = (from n in tanks select n.Data.RatingWN7Weight).Sum() / (from n in tanks select n.Data.BattlesCount).Sum();

                    return (int)Eff;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public RatingStructure ratingStruct
        {
            get
            {
                RatingStructure ratingStructs = new RatingStructure();
                ratingStructs.WN8ExpectedTankList = new WN8ExpValues();
                ratingStructs.countryID = (from n in tanks select n.CountryID).FirstOrDefault();
                ratingStructs.tankID = (from n in tanks select n.TankID).FirstOrDefault();
                ratingStructs.tier = (from n in tanks select n.Tier).FirstOrDefault();
                ratingStructs.globalTier = ratingStructs.tier;

                ratingStructs.singleTank = true;
               
                ratingStructs.battlesCount = (from n in tanks select n.Data.BattlesCount).Sum();
                ratingStructs.battlesCount8_8 = (from n in tanks select n.Data.BattlesCount8_8).Sum();
                ratingStructs.capturePoints = (from n in tanks select n.Data.CapturePoints).Sum();
                ratingStructs.defencePoints = (from n in tanks select n.Data.DefencePoints).Sum();

                ratingStructs.damageAssistedRadio = (from n in tanks select n.Data.DamageAssistedRadio).Sum();
                ratingStructs.damageAssistedTracks = (from n in tanks select n.Data.DamageAssistedTracks).Sum();
                ratingStructs.damageDealt = (from n in tanks select n.Data.DamageDealt).Sum();
                ratingStructs.frags = (from n in tanks select n.Data.Frags).Sum();
                ratingStructs.spotted = (from n in tanks select n.Data.Spotted).Sum();
                ratingStructs.wins = (from n in tanks select n.Data.Victories).Sum();
                


                //ratingStructs.globalWinRate = ratingStructs.winRate;

                return ratingStructs;

            }
        }

        public double RatingWN8
        {
            get
            {
                try
                {

                    WOTStatistics.Core.Ratings.RatingStorage WN8 = WOTStatistics.Core.Ratings.GetRatingWN8(ratingStruct);






                    //double Eff = (from n in tanks select n.Data.RatingWN8Weight).Sum() / (from n in tanks select n.Data.BattlesCount).Sum();
                    //WOTHelper.AddToLog("RWN8: " + (int)Eff + " -- " + BattlesCount + " -- " + ((int)Eff / (int)BattlesCount));
                   //WOTHelper.AddToLog("RWN8: " + WN8.Value + " -- " + BattlesCount);

                    return (int)WN8.Value;
            
                }
                catch
                {
                    return 0;
                }
            }
        }
        //public double OverallRatingEff
        //{
        //    get
        //    {
        //        try
        //        {
        //            double Eff = (from n in tanks select n.Data.OverallRatingEff).Max();

        //            return (int)Eff;
        //        }
        //        catch
        //        {
        //            return 0;
        //        }
        //    }
        //}
        //public double OverallRatingBR
        //{
        //    get
        //    {
        //        try
        //        {
        //            double Eff = (from n in tanks select n.Data.OverallRatingBR).Max();

        //            return (int)Eff;
        //        }
        //        catch
        //        {
        //            return 0;
        //        }
        //    }
        //}
        //public double OverallRatingWN7
        //{
        //    get
        //    {
        //        try
        //        {
        //            double Eff = (from n in tanks select n.Data.OverallRatingWN7).Max();

        //            return (int)Eff;
        //        }
        //        catch
        //        {
        //            return 0;
        //        }
        //    }
        //}

        //public double OverallRatingWN8
        //{
        //    get
        //    {
        //        try
        //        {
        //            double Eff = (from n in tanks select n.Data.OverallRatingWN8).Max();

        //            return (int)Eff;
        //        }
        //        catch
        //        {
        //            return 0;
        //        }
        //    }
        //}

        public double Destroyed
        {
            get
            {
                return (from n in tanks
                        select n.Data.Frags).Sum();
            }
        }
        public double Detected
        {
            get
            {
                return (from n in tanks
                        select n.Data.Spotted).Sum();
            }
        }
        public double HitRatio
        {
            get
            {
                double hits = (from n in tanks
                               select ((double)n.Data.Hits)).Sum();

                double shots = (from n in tanks
                               select ((double)n.Data.Shots)).Sum();

                return (hits / shots) * 100;
            }
        }

        public double AverageFrags
        {
            get
            {
                return Destroyed / BattlesCount;

            }
        }

        public double AverageTier
        {
            get
            {
                double tier = 0;
                for (int i = 1; i <= 10; i++)
                {
                    double var = (from x in tanks
                                  where x.Tier == i
                                  select x.Data.BattlesCount).Sum() * i;
                    tier += var;

                }

                double battles = (from x in tanks
                                  select x.Data.BattlesCount).Sum();

                return tier / battles;
            }
        }

        public double AverageSpotted
        {
            get
            {
                
                return  Spotted / BattlesCount;
            }
        }

        public double DamageDealt
        {
            get
            {
                return (from n in tanks
                        select n.Data.DamageDealt).Sum();
            }
        }

        public double DamageAssistedRadio
        {
            get
            {
                return (from n in tanks
                        select n.Data.DamageAssistedRadio).Sum();
            }
        }

        public double DamageAssistedTracks
        {
            get
            {
                return (from n in tanks
                        select n.Data.DamageAssistedTracks).Sum();
            }
        }
        public double Spotted
        {
            get
            {
                return (from n in tanks
                        select n.Data.Spotted).Sum();
            }
        }

        public double DamageReceived
        {
            get
            {
                return (from n in tanks
                        select n.Data.DamageReceived).Sum();
            }
        }
        public double DamageRatio
        {
            get
            {
                double dCaused = (from n in tanks
                                    select n.Data.DamageDealt).Sum();

                double dReceived = (from n in tanks
                                    select n.Data.DamageReceived).Sum();

                return dCaused / dReceived;
            }
        }
        public double CapturePoints
        {
            get
            {
                return (from n in tanks
                        select n.Data.CapturePoints).Sum();
            }
        }
        public double DefencePoints
        {
            get
            {
                return (from n in tanks
                        select n.Data.DefencePoints).Sum();
            }
        }
        public double Experience
        {
            get
            {
                return (from n in tanks
                        select n.Data.Xp).Sum();
            }
        }

        public double Shots
        {
            get
            {
                return (from n in tanks
                        select n.Data.Shots).Sum();
            }
        }

        public double Kills
        {
            get
            {
                return (from n in tanks
                        select n.Data.Frags).Sum();
            }
        }

        public double Hits
        {
            get
            {
                return (from n in tanks
                        select n.Data.Hits).Sum();
            }
        }

        public double AverageDefencePoints
        {
            get
            {
                return DefencePoints / BattlesCount;
            }
        }

        public double AverageShots
        {
            get
            {
                return Shots / BattlesCount;
            }
        }

        public double AverageHits
        {
            get
            {
                return Hits / BattlesCount;
            }
        }

        public double AverageCapturePoints
        {
            get
            {
                return CapturePoints / BattlesCount;
            }
        }

        public double AverageExperiencePerBattle
        {
            get
            {
                return Experience / BattlesCount;
            }
        }
        public double AverageDamageDealtPerBattle
        {
            get
            {
                return DamageDealt / BattlesCount;
            }
        }

        public double AverageDamageAssistedRadioPerBattle
        {
            get
            {
                return DamageAssistedRadio / BattlesCount;
            }
        }
        public double AverageDamageAssistedTracksPerBattle
        {
            get
            {
                return DamageAssistedTracks / BattlesCount;
            }
        }

        public double AverageDamageReceivedPerBattle
        {
            get
            {
                return DamageReceived / BattlesCount;
            }
        }

        public double MaxExperience
        {
            get
            {

                try
                {
                    return (from n in tanks
                            select n.Data.MaxXp).Max();
                }
                catch 
                {

                    return 0;
                }
            }
        }

        public WOTStats()
        {
            tanks = new List<Tank>();
        }
    }
    public class WOTStatsDelta
    {

        public WOTCompare Parent { get; private set; }
        public ArrayList records { get; set; }
        public int version { get; set; }
        public int extractor_version { get; set; }
        public List<TankDelta> tanks { get; set; }
        public int BattlesCount
        {
            get
            {
                return Parent.WOTCurrent.BattlesCount - Parent.WOTPrevious.BattlesCount;
            }
        }
        public int TotalVictories
        {
            get
            {
                return Parent.WOTCurrent.Victories - Parent.WOTPrevious.Victories;
            }
        }
        public int Survived
        {
            get
            {
                return Parent.WOTCurrent.Survived - Parent.WOTPrevious.Survived;
            }
        }
        public int TotalLosses
        {
            get
            {
                return Parent.WOTCurrent.Losses - Parent.WOTPrevious.Losses;
            }
        }
        public int TotalDraws
        {
            get
            {
                return Parent.WOTCurrent.Draws - Parent.WOTPrevious.Draws;
            }
        }
        public double WinPercentage
        {
            get
            {
                return Parent.WOTCurrent.Victory_Percentage - Parent.WOTPrevious.Victory_Percentage;
            }
        }
        public double Survived_Percentage
        {
            get
            {
                return Parent.WOTCurrent.Survived_Percentage - Parent.WOTPrevious.Survived_Percentage;
            }
        }
        public double LossPercentage
        {
            get
            {
                return Parent.WOTCurrent.Losses_Percentage - Parent.WOTPrevious.Losses_Percentage;
            }
        }
        public double DrawPercentage
        {
            get
            {
                return Parent.WOTCurrent.Draws_Percentage - Parent.WOTPrevious.Draws_Percentage;
            }
        }
        public double RatingEff
        {
            get
            {
                return (Parent.WOTCurrent.RatingEff - Parent.WOTPrevious.RatingEff) / BattlesCount;
            }
        }
        public double RatingBR
        {
            get
            {
                return (Parent.WOTCurrent.RatingBR - Parent.WOTPrevious.RatingBR) / BattlesCount;
            }
        }
        public double RatingWN7
        {
            get
            {
                return (Parent.WOTCurrent.RatingWN7 - Parent.WOTPrevious.RatingWN7) / BattlesCount;
            }
        }
        public double RatingWN8
        {
            get
            {
                return (Parent.WOTCurrent.RatingWN8 - Parent.WOTPrevious.RatingWN8) / BattlesCount;
            }
        }

        public double OverallRatingEff
        {
            get
            {
                return (Parent.WOTCurrent.OverallRatingEff - Parent.WOTPrevious.OverallRatingEff);
            }
        }
        public double OverallRatingBR
        {
            get
            {
                return (Parent.WOTCurrent.OverallRatingBR - Parent.WOTPrevious.OverallRatingBR);
            }
        }
        public double OverallRatingWN7
        {
            get
            {
                return (Parent.WOTCurrent.OverallRatingWN7 - Parent.WOTPrevious.OverallRatingWN7);
            }
        }

        public double OverallRatingWN8
        {
            get
            {
                return (Parent.WOTCurrent.OverallRatingWN8 - Parent.WOTPrevious.OverallRatingWN8);
            }
        }

        public double Destroyed
        {
            get
            {
                return Parent.WOTCurrent.Destroyed - Parent.WOTPrevious.Destroyed;
            }
        }
        public double Detected
        {
            get
            {
                return Parent.WOTCurrent.Detected - Parent.WOTPrevious.Detected;
            }
        }
        public double HitRatio
        {
            get
            {
                return Parent.WOTCurrent.HitRatio - Parent.WOTPrevious.HitRatio;
            }
        }
        public double DamageDealt
        {
            get
            {
                return Parent.WOTCurrent.DamageDealt - Parent.WOTPrevious.DamageDealt;
            }
        }
        public double DamageAssistedRadio
        {
            get
            {
                return Parent.WOTCurrent.DamageAssistedRadio - Parent.WOTPrevious.DamageAssistedRadio;
            }
        }
        public double DamageAssistedTracks
        {
            get
            {
                return Parent.WOTCurrent.DamageAssistedTracks - Parent.WOTPrevious.DamageAssistedTracks;
            }
        }
        public double DamageReceived
        {
            get
            {
                return Parent.WOTCurrent.DamageReceived - Parent.WOTPrevious.DamageReceived;
            }
        }
        public double DamageRatio
        {
            get
            {
                return (Parent.WOTCurrent.DamageRatio - Parent.WOTPrevious.DamageRatio) ;
            }
        }
        public double CapturePoints
        {
            get
            {
                return Parent.WOTCurrent.CapturePoints - Parent.WOTPrevious.CapturePoints;
            }
        }
        public double DefencePoints
        {
            get
            {
                return Parent.WOTCurrent.DefencePoints - Parent.WOTPrevious.DefencePoints;
            }
        }
        public double Experience
        {
            get
            {
                return Parent.WOTCurrent.Experience - Parent.WOTPrevious.Experience;
            }
        }
        public double AverageExperiencePerBattle
        {
            get
            {
                return Parent.WOTCurrent.AverageExperiencePerBattle - Parent.WOTPrevious.AverageExperiencePerBattle;
            }
        }
        public double AverageDamagePerBattle
        {
            get
            {
                return Parent.WOTCurrent.AverageDamageDealtPerBattle - Parent.WOTPrevious.AverageDamageDealtPerBattle;
            }
        }
        public double MaxExperience
        {
            get
            {
                return Parent.WOTCurrent.MaxExperience - Parent.WOTPrevious.MaxExperience;
            }
        }
       
        public WOTStatsDelta(WOTCompare parent)
        {
            tanks = new List<TankDelta>();
            Parent = parent;

            foreach (Tank curTank in parent.WOTCurrent.tanks)
            {
                TankDelta newTank = new TankDelta(this);
                newTank.CountryID = curTank.CountryID;
                newTank.TankID = curTank.TankID;
                newTank.Country_Description = curTank.Country_Description;
                newTank.Tank_Description = curTank.Tank_Description;
                newTank.TankClass = curTank.TankClass;
                newTank.Tier = curTank.Tier;
                newTank.Updated = curTank.Updated;


                foreach (FragCount curFragCount in curTank.FragList)
                {
                    FragCountDelta fc = new FragCountDelta(newTank);
                    fc.CountryID = curFragCount.CountryID;
                    fc.TankID = curFragCount.TankID;
                    fc.TankClass = curFragCount.TankClass;
                    fc.Tier = curFragCount.Tier;
                    newTank.FragList.Add(fc);
                }

                tanks.Add(newTank);
            }
        }
    }

    public class Tank
    {
        public WOTStats Parent { get; private set; }
        
        public List<object> ClanData { get; set; }
        public int Updated { get; set; }
        public DateTime Updated_Friendly
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Updated).AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours);
                
            }
        }
        public double OverallRatingEff { get; set; }
        public double OverallRatingBR { get; set; }
        public double OverallRatingWN7 { get; set; }
        public double OverallRatingWN8 { get; set; }
        
        public int CountryID { get; set; }
        public string Country_Description { get; set; }
        public List<FragCount> FragList { get; set; }
        public int basedonversion { get; set; }
        public AwardSeries Series { get; set; }
        public object Company { get; set; }
        public AwardsMajor Major { get; set; }
        public TankData Data { get; set; }
        public AwardsBattle Battle { get; set; }
        public int TankID { get; set; }
        public string Tank_Description { get; set; }
        public int Tier { get; set; }
        public bool Premium { get; set; }
        public Epics Epics { get; set; }
        public AwardSpecial Special { get; set; }
        public string TankClass { get; set; }

        WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();

        public RatingStructure ratingStruct
        {
            get
            {
                RatingStructure ratingStructs = new RatingStructure();
                ratingStructs.WN8ExpectedTankList = WN8ExpectedTankList;
                ratingStructs.countryID = this.CountryID;
                ratingStructs.tankID = this.TankID;
                ratingStructs.tier = this.Tier;
                ratingStructs.globalTier = ratingStructs.tier;

                ratingStructs.singleTank = true;

                ratingStructs.battlesCount = Data.BattlesCount;
                ratingStructs.battlesCount8_8 = Data.BattlesCount8_8;
                ratingStructs.capturePoints = Data.CapturePoints;
                ratingStructs.defencePoints = Data.DefencePoints;

                ratingStructs.damageAssistedRadio = Data.DamageAssistedRadio;
                ratingStructs.damageAssistedTracks = Data.DamageAssistedTracks;
                ratingStructs.damageDealt = Data.DamageDealt;
                ratingStructs.frags = Data.Frags;
                ratingStructs.spotted = Data.Spotted;
                ratingStructs.wins = Data.Victories;
                ratingStructs.gWinRate = Data.VictoryPercentage;

                return ratingStructs;
       
            }
        }
        
       
        
        public double RatingEff
        {
            get
            {
                //return ((Data.DamageDealt / (double)battles) * (10 / ((double)Tier + 2)) * (0.23 + 2 * (double)Tier / 100)) + ((Data.Frags / battles) * 250) + ((Data.Spotted / battles) * 150) + ((Math.Log((Data.CapturePoints / battles) + 1, 1.732)) * 150) + ((Data.DefencePoints / battles) * 150);
                return WOTStatistics.Core.Ratings.GetRatingEff(ratingStruct).Value;
               

            }
        }

        public double RatingBR
        {
            get
            {


                //return ((Data.DamageDealt / (double)battles) * (10 / ((double)Tier + 2)) * (0.23 + 2 * (double)Tier / 100)) + ((Data.Frags / battles) * 250) + ((Data.Spotted / battles) * 150) + ((Math.Log((Data.CapturePoints / battles) + 1, 1.732)) * 150) + ((Data.DefencePoints / battles) * 150);
                return WOTStatistics.Core.Ratings.GetRatingBR(ratingStruct).Value;


            }
        }

        public double RatingWN7
        {
            get
            {
                //return ((Data.DamageDealt / (double)battles) * (10 / ((double)Tier + 2)) * (0.23 + 2 * (double)Tier / 100)) + ((Data.Frags / battles) * 250) + ((Data.Spotted / battles) * 150) + ((Math.Log((Data.CapturePoints / battles) + 1, 1.732)) * 150) + ((Data.DefencePoints / battles) * 150);
                return WOTStatistics.Core.Ratings.GetRatingWN7(ratingStruct).Value;


            }
        }
         
        public double RatingWN8
        {
            get
            {



                WOTStatistics.Core.Ratings.RatingStorage WN8 = WOTStatistics.Core.Ratings.GetRatingWN8(ratingStruct);
                //WOTHelper.AddToLog ("WN8: " + WN8.Value);
                return WN8.Value;


            }
        }


        public Tank(WOTStats parent)
        {
            FragList = new List<FragCount>();
            Parent = parent;
        }

        public T FieldValue<T>(string fieldName)
        {
                    PropertyInfo pi = this.GetType().GetProperty(fieldName);
                    if (pi != null)
                    {
                        return  ((T)pi.GetValue(this, null));
                    }

                return ((T)new object());
        }

    }
    public class TankDelta
    {
        public WOTStatsDelta Parent { get; private set; }
        public WOTCompare Root { get; private set; }
        public List<object> ClanData { get; set; }
        public int Updated { get; set; }
        public DateTime Updated_Friendly
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Updated).AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours);
            }
        }
        public int CountryID { get; set; }
        public string Country_Description { get; set; }
        public List<FragCountDelta> FragList { get; set; }
        public int basedonversion { get; set; }
        public AwardSeriesDelta Series { get; set; }
        public object Company { get; set; }
        public AwardsMajorDelta Major { get; set; }
        public TankDataDelta Data { get; set; }
        public AwardsBattleDelta Battle { get; set; }
        public int TankID { get; set; }
        public string Tank_Description { get; set; }
        public int Tier { get; set; }
        public int Premium { get; set; }
        public string TankClass { get; set; }
        public EpicsDelta Epics { get; set; }
        public AwardSpecialDelta Special { get; set; }

        public double RatingEff
        {
            get
            {
                
                return (from x in Root.WOTCurrent.tanks
                        join y in Root.WOTPrevious.tanks
                        on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == CountryID && x.TankID == TankID
                        select x.RatingEff - y.RatingEff).FirstOrDefault(); 
            }
        }
        public double RatingBR
        {
            get
            {

                return (from x in Root.WOTCurrent.tanks
                        join y in Root.WOTPrevious.tanks
                        on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == CountryID && x.TankID == TankID
                        select x.RatingBR - y.RatingBR).FirstOrDefault();
            }
        }

        public double RatingWN7
        {
            get
            {

                return (from x in Root.WOTCurrent.tanks
                        join y in Root.WOTPrevious.tanks
                        on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == CountryID && x.TankID == TankID
                        select x.RatingWN7 - y.RatingWN7).FirstOrDefault();
            }
        }

        public double RatingWN8
        {
            get
            {
                
                return (from x in Root.WOTCurrent.tanks
                        join y in Root.WOTPrevious.tanks
                        on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == CountryID && x.TankID == TankID
                        select x.RatingWN8 - y.RatingWN8).FirstOrDefault();
            }
        }


        public TankDelta(WOTStatsDelta parent)
        {
            Parent = parent;
            Root = Parent.Parent;
            FragList = new List<FragCountDelta>();
            Data = new TankDataDelta(this);
            Epics = new EpicsDelta(this);
            Series = new AwardSeriesDelta(this);
            Major = new AwardsMajorDelta(this);
            Battle = new AwardsBattleDelta(this);
            Special = new AwardSpecialDelta(this);
        }
    }

    public class TankData
    {
        public Tank Parent { get; private set; }
        
        
        public int Spotted { get; set; }
        public int Hits { get; set; }
        public int MaxFrags { get; set; }
        public int BeastFrags { get; set; }
        public int MaxXp { get; set; }
        public int Victories { get; set; }
        public int CapturePoints { get; set; }
        public int Defeats { get; set; }
        public int Survived { get; set; }
        public int BattleLifeTime { get; set; }
        public int DefencePoints { get; set; } //droppedCapturePoints
        public int BattlesCount { get; set; }
        public int BattlesCount8_8 { get; set; }
        public int LastBattleTime { get; set; }
        public int DamageReceived { get; set; }
        public int Shots { get; set; }
        public int VictoryAndSurvived { get; set; }
        public int Tier8upFrags { get; set; }
        public int DamageDealt { get; set; }
        public int DamageAssistedRadio { get; set; }
        public int DamageAssistedTracks { get; set; }
        public int Xp { get; set; }
        public int Mileage { get; set; }
        public int Frags { get; set; }
        public int TreesCut { get; set; }
        public int RatingEff { get; set; }
        public int RatingBR { get; set; }
        public int RatingWN7 { get; set; }
        public int RatingWN8 { get; set; }
        public int RatingEffWeight { get; set; }
        public int RatingBRWeight { get; set; }
        public int RatingWN7Weight { get; set; }
        public int RatingWN8Weight { get; set; }

        public double OverallRatingEff { get; set; }
        public double OverallRatingBR { get; set; }
        public double OverallRatingWN7 { get; set; }
        public double OverallRatingWN8 { get; set; }
        

        public int Draws { get { return BattlesCount - (Victories + Defeats); } }
        public double VictoryPercentage { get { return ((double)Victories / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)) * 100; } }
        public double LossesPercentage { get { return ((double)Defeats / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)) * 100; } }
        public double DrawsPercentage { get { return ((double)Draws / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)) * 100; } }
        public double SurvivedPercentage { get { return ((double)Survived / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)) * 100; } }
        public double survived_with_victoryPercentage { get { return ((double)VictoryAndSurvived / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)) * 100; } }
        public double Accuracy { get { return ((double)Hits / ((double)Shots == 0 ? 1 : (double)Shots)) * 100; } }
        public double AverageFrags { get { return ((double)Frags / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageDamageDealt { get { return ((double)DamageDealt / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageDamageAssistedRadio { get { return ((double)DamageAssistedRadio / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageDamageAssistedTracks { get { return ((double)DamageAssistedTracks / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageDamageReceived { get { return ((double)DamageReceived / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double DamageRatio { get { return ((double)DamageDealt / ((double)DamageReceived)); } }
        public double AverageSpotted { get { return ((double)Spotted / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageCapturePoints { get { return ((double)CapturePoints / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageDefencePoints { get { return ((double)DefencePoints / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageXP { get { return ((double)Xp / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageMileage { get { return ((double)Mileage / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageShots { get { return ((double)Shots / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }
        public double AverageHits { get { return ((double)Hits / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount)); } }

        
        public double KillperBattle
        {
            get
            {
                return (double)Frags / ((double)BattlesCount == 0 ? 1 : (double)BattlesCount);
            }

        }

        public double KillperDeaths
        {
            get
            {
                return (double)Frags / (((double)BattlesCount - (double)Survived) == 0 ? 1 : ((double)BattlesCount - (double)Survived));
            }

        }

        public TimeSpan BattleLifeTime_Friendly
        {
            get
            {
                DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
                DateTime endDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(BattleLifeTime);
                TimeSpan timeSpan = endDate - startDate;
                return timeSpan.Duration();
            }
        }
        public DateTime Last_Time_Played_Friendly
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(LastBattleTime);
            }
        }
        public TankData(Tank parent)
        {
            Parent = parent;
        }

    }
    public class TankDataDelta
    {
        public TankDelta Parent { get; private set; }

        public int Spotted
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //         select x.Data.Spotted - y.Data.Spotted).FirstOrDefault();     
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Spotted).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Spotted).FirstOrDefault();

                return currentCount - previousCount;

            }
        }
        public int Hits
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Hits - y.Data.Hits).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Hits).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Hits).FirstOrDefault();

                return currentCount - previousCount;

            }
        }
        public int Shots
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Shots - y.Data.Shots).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Shots).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Shots).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int BattlesCount
        {
            get
            {
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.BattlesCount).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.BattlesCount).FirstOrDefault();

                return currentCount - previousCount;
                    
                    //(from x in Parent.Root.WOTCurrent.tanks
                    //    join y in Parent.Root.WOTPrevious.tanks
                    //     on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                    //    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                    //    select x.Data.BattlesCount - y.Data.BattlesCount).FirstOrDefault();
            }
        }
        public int DefencePoints
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DefencePoints - y.Data.DefencePoints).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.DefencePoints).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.DefencePoints).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int Defeats
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Defeats - y.Data.Defeats).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Defeats).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Defeats).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int DamageReceived
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageReceived - y.Data.DamageReceived).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.DamageReceived).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.DamageReceived).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int CapturePoints
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.CapturePoints - y.Data.CapturePoints).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.CapturePoints).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.CapturePoints).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int Xp
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Xp - y.Data.Xp).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Xp).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Xp).FirstOrDefault();

                return currentCount - previousCount;
            }
        }

        public int BattleLifeTime
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Xp - y.Data.Xp).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.BattleLifeTime).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.BattleLifeTime).FirstOrDefault();

                return currentCount - previousCount;
            }
        }

        public double KillperBattle
        {
            get
            {
                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.KillperBattle).FirstOrDefault();


                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.KillperBattle).FirstOrDefault();

                return currentCount - previousCount;
            }

        }

        public double KillperDeaths
        {
            get
            {
                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.KillperDeaths).FirstOrDefault();


                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.KillperDeaths).FirstOrDefault();

                return currentCount - previousCount;
            }

        }

        public int MaxXp
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.MaxXp - y.Data.MaxXp).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.MaxXp).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.MaxXp).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int RatingEff
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.RatingEff).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.RatingEff).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int RatingEffWeight
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.RatingEffWeight).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.RatingEffWeight).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int RatingBR
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                
                
                
                
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.RatingBR).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.RatingBR).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int RatingBRWeight
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();




                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.RatingBRWeight).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.RatingBRWeight).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int RatingWN7
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.RatingWN7).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.RatingWN7).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int RatingWN7Weight
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.RatingWN7Weight).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.RatingWN7Weight).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int RatingWN8
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.RatingWN8).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.RatingWN8).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int RatingWN8Weight
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.RatingWN8Weight).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.RatingWN8Weight).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int Mileage
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Mileage).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Mileage).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int DamageDealt
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageDealt - y.Data.DamageDealt).FirstOrDefault();
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.DamageDealt).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.DamageDealt).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int DamageAssistedRadio
        {
            get
            {
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.DamageAssistedRadio).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.DamageAssistedRadio).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int DamageAssistedTrack
        {
            get
            {
                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.DamageAssistedTracks).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.DamageAssistedTracks).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int Victories
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Victories - y.Data.Victories).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Victories).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Victories).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int BeastFrags
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.BeastFrags - y.Data.BeastFrags).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.BeastFrags).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.BeastFrags).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int MaxFrags
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.MaxFrags - y.Data.MaxFrags).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.MaxFrags).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.MaxFrags).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int Survived
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Survived - y.Data.Survived).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Survived).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Survived).FirstOrDefault();

                return currentCount - previousCount;
            }
        }

        public int TreesCut
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.TreesCut - y.Data.TreesCut).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.TreesCut).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.TreesCut).FirstOrDefault();

                return currentCount - previousCount;
            }
        }

        public int Tier8upFrags
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Tier8upFrags - y.Data.Tier8upFrags).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Tier8upFrags).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Tier8upFrags).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int Frags
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Frags - y.Data.Frags).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Frags).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Frags).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int VictoryAndSurvived
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.VictoryAndSurvived - y.Data.VictoryAndSurvived).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.VictoryAndSurvived).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.VictoryAndSurvived).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public int Draws
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Draws - y.Data.Draws).FirstOrDefault();

                int currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.Draws).FirstOrDefault();

                int previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.Draws).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double DrawsPercentage
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DrawsPercentage - y.Data.DrawsPercentage).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                    where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                    select x.Data.DrawsPercentage).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                     where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                     select x.Data.DrawsPercentage).FirstOrDefault();

                return currentCount - previousCount;
            }
        }

        public double SurvivedPercentage 
        {
            get
            {
                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.SurvivedPercentage).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.SurvivedPercentage).FirstOrDefault();

                return currentCount - previousCount;
            }
        }

        public double VictoryPercentage
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.VictoryPercentage - y.Data.VictoryPercentage).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.VictoryPercentage).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.VictoryPercentage).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double survived_with_victoryPercentage
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.survived_with_victoryPercentage - y.Data.survived_with_victoryPercentage).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.survived_with_victoryPercentage).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.survived_with_victoryPercentage).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double LossesPercentage
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.LossesPercentage - y.Data.LossesPercentage).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.LossesPercentage).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.LossesPercentage).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double Accuracy
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.Accuracy - y.Data.Accuracy).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.Accuracy).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.Accuracy).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double AverageFrags
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.AverageFrags - y.Data.AverageFrags).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.AverageFrags).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.AverageFrags).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double AverageDamage
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.AverageDamage - y.Data.AverageDamage).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.AverageDamageDealt).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.AverageDamageDealt).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double DamageRatio
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.DamageRatio - y.Data.DamageRatio).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.DamageRatio).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.DamageRatio).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double AverageSpotted
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.AverageSpotted - y.Data.AverageSpotted).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.AverageSpotted).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.AverageSpotted).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double AverageCapturePoints
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.AverageCapturePoints - y.Data.AverageCapturePoints).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.AverageCapturePoints).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.AverageCapturePoints).FirstOrDefault();

                return currentCount - previousCount;
            }
        }
        public double AverageDefencePoints
        {
            get
            {
                //return (from x in Parent.Root.WOTCurrent.tanks
                //        join y in Parent.Root.WOTPrevious.tanks
                //         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                //        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                //        select x.Data.AverageDefencePoints - y.Data.AverageDefencePoints).FirstOrDefault();

                double currentCount = (from x in Parent.Root.WOTCurrent.tanks
                                       where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                       select x.Data.AverageDefencePoints).FirstOrDefault();

                double previousCount = (from x in Parent.Root.WOTPrevious.tanks
                                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                                        select x.Data.AverageDefencePoints).FirstOrDefault();

                return currentCount - previousCount;
            }
        }

        public TankDataDelta(TankDelta parent)
        {
            Parent = parent;
        }
    }
    
    public class Epics
    {
        public Tank Parent { get; private set; }

        public int Orlik { get; set; }
        public int Burda { get; set; }
        public int Oskin { get; set; }
        public int Halonen { get; set; }
        public int Billotte { get; set; }
        public int TamadaYoshio { get; set; }
        public int Horoshilov { get; set; }
        public int Lister { get; set; }
        public int HeroesOfRassenai { get; set; }
        public int Kolobanov { get; set; }
        public int Fadin { get; set; }
        public int Erohin { get; set; }
        public int DeLaglanda { get; set; }
        public int Boelter { get; set; } //Wittmann 

        public Epics(Tank parent)
        {
            Parent = parent;
        }
    }
    public class EpicsDelta
    {
        public TankDelta Parent { get; private set; }

        
        public int Orlik
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Orlik - y.Epics.Orlik).FirstOrDefault();
            }
        }
        public int Burda
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Burda - y.Epics.Burda).FirstOrDefault();
            }
        }
        public int Oskin
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Oskin - y.Epics.Oskin).FirstOrDefault();
            }
        }
        public int Halonen
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Halonen - y.Epics.Halonen).FirstOrDefault();
            }
        }
        public int Billotte
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Billotte - y.Epics.Billotte).FirstOrDefault();
            }
        }
        public int TamadaYoshio
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.TamadaYoshio - y.Epics.TamadaYoshio).FirstOrDefault();
            }
        }
        public int Horoshilov
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Horoshilov - y.Epics.Horoshilov).FirstOrDefault();
            }
        }
        public int Lister
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Lister - y.Epics.Lister).FirstOrDefault();
            }
        }
        public int HeroesOfRassenai
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.HeroesOfRassenai - y.Epics.HeroesOfRassenai).FirstOrDefault();
            }
        }
        public int Kolobanov
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Kolobanov - y.Epics.Kolobanov).FirstOrDefault();
            }
        }
        public int Fadin
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Fadin - y.Epics.Fadin).FirstOrDefault();
            }
        }
        public int Erohin
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Erohin - y.Epics.Erohin).FirstOrDefault();
            }
        }
        public int DeLaglanda
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.DeLaglanda - y.Epics.DeLaglanda).FirstOrDefault();
            }
        }
        public int Boelter
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Epics.Boelter - y.Epics.Boelter).FirstOrDefault();
            }
        }

        public EpicsDelta(TankDelta parent)
        {
            Parent = parent;
        }
    }

    public class AwardSeries
    {
        public Tank Parent { get; private set; }

        public int Max_Piercing { get; set; }
        public int Max_Killing { get; set; }
        public int Max_Invincible { get; set; }
        public int Max_Diehard { get; set; }
        public int Piercing { get; set; }
        public int Sniper { get; set; }
        public int Diehard { get; set; }
        public int Invincible { get; set; }
        public int Killing { get; set; }
        public int Max_Sniper { get; set; }

        public AwardSeries(Tank parent)
        {
            Parent = parent;
        }
    }
    public class AwardSeriesDelta
    {
        public TankDelta Parent { get; private set; }

        public int Max_Piercing 
        {   get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Max_Piercing - y.Series.Max_Piercing).FirstOrDefault();
            } 
        }
        public int Max_Killing
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Max_Killing - y.Series.Max_Killing).FirstOrDefault();
            }
        }
        public int Max_Invincible
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Max_Invincible - y.Series.Max_Invincible).FirstOrDefault();
            }
        }
        public int Max_Diehard
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Max_Diehard - y.Series.Max_Diehard).FirstOrDefault();
            }
        }
        public int Piercing
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Piercing - y.Series.Piercing).FirstOrDefault();
            }
        }
        public int Sniper
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Sniper - y.Series.Sniper).FirstOrDefault();
            }
        }
        public int DieHard
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Diehard - y.Series.Diehard).FirstOrDefault();
            }
        }
        public int Invincible
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Invincible - y.Series.Invincible).FirstOrDefault();
            }
        }
        public int Killing
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Killing - y.Series.Killing).FirstOrDefault();
            }
        }
        public int Max_Sniper
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Series.Max_Sniper - y.Series.Max_Sniper).FirstOrDefault();
            }
        }

        public AwardSeriesDelta(TankDelta parent)
        {
            Parent = parent;
        }
    }

    public class AwardSpecial
    {
        public Tank Parent { get; private set; }

        public int ArmorPiercer { get; set; }
        public int MouseBane { get; set; }
        public int BeastHunter { get; set; }
        public int MarkOfMastery { get; set; }
        public int Kamikaze { get; set; }
        public int Raider { get; set; }
        public int Invincible { get; set; }
        public int TankExpert { get; set; }
        public int Diehard { get; set; }
        public int HandOfDeath { get; set; }
        public int LumberJack { get; set; }
        public int Sniper { get; set; }

        public AwardSpecial(Tank parent)
        {
            Parent = parent;
        }
    }
    public class AwardSpecialDelta
    {
        public TankDelta Parent { get; private set; }

        public int ArmorPiercer
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.ArmorPiercer - y.Special.ArmorPiercer).FirstOrDefault();
            }
        }
        public int MouseBane
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.MouseBane - y.Special.MouseBane).FirstOrDefault();
            }
        }
        public int BeastHunter
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.BeastHunter - y.Special.BeastHunter).FirstOrDefault();
            }
        }
        public int MarkOfMastery
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.MarkOfMastery - y.Special.MarkOfMastery).FirstOrDefault();
            }
        }
        public int Kamikaze
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.Kamikaze - y.Special.Kamikaze).FirstOrDefault();
            }
        }
        public int Raider
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.Raider - y.Special.Raider).FirstOrDefault();
            }
        }
        public int Invincible
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.Invincible - y.Special.Invincible).FirstOrDefault();
            }
        }
        public int TankExpert
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.TankExpert - y.Special.TankExpert).FirstOrDefault();
            }
        }
        public int Diehard
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.Diehard - y.Special.Diehard).FirstOrDefault();
            }
        }
        public int HandOfDeath
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.HandOfDeath - y.Special.HandOfDeath).FirstOrDefault();
            }
        }
        public int LumberJack
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.LumberJack - y.Special.LumberJack).FirstOrDefault();
            }
        }
        public int Sniper
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Special.Sniper - y.Special.Sniper).FirstOrDefault();
            }
        }

        public AwardSpecialDelta(TankDelta parent)
        {
            Parent = parent;
        }
    }

    public class AwardsMajor
    {
        public Tank Parent { get; private set; }

        public int Ekins { get; set; }
        public int Abrams { get; set; }
        public int Carius { get; set; }
        public int Poppel { get; set; }
        public int Kay { get; set; }
        public int LeClerc { get; set; }
        public int Knispel { get; set; }
        public int Lavrinenko { get; set; }

        public AwardsMajor(Tank parent)
        {
            Parent = parent;
        }
    }
    public class AwardsMajorDelta
    {
        public TankDelta Parent { get; private set; }

        public int Ekins
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Major.Ekins - y.Major.Ekins).FirstOrDefault();
            }
        }
        public int Abrams
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Major.Abrams - y.Major.Abrams).FirstOrDefault();
            }
        }
        public int Carius
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Major.Carius - y.Major.Carius).FirstOrDefault();
            }
        }
        public int Poppel
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Major.Poppel - y.Major.Poppel).FirstOrDefault();
            }
        }
        public int Kay
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Major.Kay - y.Major.Kay).FirstOrDefault();
            }
        }
        public int LeClerc
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Major.LeClerc - y.Major.LeClerc).FirstOrDefault();
            }
        }
        public int Knispel
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Major.Knispel - y.Major.Knispel).FirstOrDefault();
            }
        }
        public int Lavrinenko
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Major.Lavrinenko - y.Major.Lavrinenko).FirstOrDefault();
            }
        }

        public AwardsMajorDelta(TankDelta parent)
        {
            Parent = parent;
        }
    }

    public class FragCount
    {
        public Tank Parent { get; private set; }

        public int CountryID { get; set; }
        public string Country_Description { get; set; }
        public int TankID { get; set; }
        public string Tank_Description { get; set; }
        public int frags { get; set; }
        public int Tier { get; set; }
        public string TankClass { get; set; }

        public FragCount(Tank parent)
        {
            Parent = parent;
        }

    }
    public class FragCountDelta
    {
        public TankDelta Parent { get; private set; }

        public int CountryID { get; set; }
        public int TankID { get; set; }
        public string TankClass { get; set; }
        public int Tier { get; set; }
        public int frags
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        from a in x.FragList
                        from b in y.FragList
                        where a.CountryID == b.CountryID && a.TankID == b.TankID
                        where a.CountryID == CountryID && a.TankID == TankID
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select a.frags - b.frags).DefaultIfEmpty(1).FirstOrDefault();
            }
        }


        public FragCountDelta(TankDelta parent)
        {
            Parent = parent;
        }
    }

    public class AwardsBattle
    {
        public Tank Parent { get; private set; }

        public int SteelWall { get; set; }
        public int Warrior { get; set; }
        public int BattleHeroes { get; set; }
        public int Scout { get; set; }
        public int Invader { get; set; }
        public int Defender { get; set; }
        public int EvilEye { get; set; }
        public int Supporter { get; set; }
        public int Sniper { get; set; }

        public AwardsBattle(Tank parent)
        {
            Parent = parent;
        }
    }
    public class AwardsBattleDelta
    {
        public TankDelta Parent { get; private set; }

        public int SteelWall
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.SteelWall - y.Battle.SteelWall).FirstOrDefault();
            }
        }
        public int Warrior
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.Warrior - y.Battle.Warrior).FirstOrDefault();
            }
        }
        public int BattleHeroes
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.BattleHeroes - y.Battle.BattleHeroes).FirstOrDefault();
            }
        }
        public int Scout
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.Scout - y.Battle.Scout).FirstOrDefault();
            }
        }
        public int Invader
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.Invader - y.Battle.Invader).FirstOrDefault();
            }
        }
        public int Defender
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.Defender - y.Battle.Defender).FirstOrDefault();
            }
        }
        public int EvilEye
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.EvilEye - y.Battle.EvilEye).FirstOrDefault();
            }
        }
        public int Supporter
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.Supporter - y.Battle.Supporter).FirstOrDefault();
            }
        }
        public int Sniper
        {
            get
            {
                return (from x in Parent.Root.WOTCurrent.tanks
                        join y in Parent.Root.WOTPrevious.tanks
                         on new { country = x.CountryID, x.TankID } equals new { country = y.CountryID, y.TankID }
                        where x.CountryID == Parent.CountryID && x.TankID == Parent.TankID
                        select x.Battle.Sniper - y.Battle.Sniper).FirstOrDefault();
            }
        }

        public AwardsBattleDelta(TankDelta parent)
        {
            Parent = parent;
        }
    }

    public class WOTCompare
    {
        public WOTStats WOTCurrent { get; private set; }
        public WOTStats WOTPrevious { get; private set; }
        public WOTStatsDelta Delta { get; private set; }

        public WOTCompare(WOTStats currentStats, WOTStats previousStats)
        {
            WOTCurrent = currentStats;
            WOTPrevious = previousStats;
            Delta = new WOTStatsDelta(this);
        }
    }
}