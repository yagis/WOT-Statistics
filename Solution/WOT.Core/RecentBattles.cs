using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WOTStatistics.SQLite;


namespace WOTStatistics.Core
{
    public class RecentBattles : SortedList<int, RecentBattle>
    {

        private string _playerName;
        private MessageQueue _message;
        private bool _TakeOn;

        public RecentBattles(string playerName, MessageQueue message, bool takeOn = false)
        {
            _playerName = playerName;
            _message = message;
            _TakeOn = takeOn;


            Read();
        }



        public new void Add(int BattleTime, RecentBattle lastBattle)
        {
            if (!base.ContainsKey(BattleTime))
                base.Add(BattleTime, lastBattle);
        }

        public void Save()
        {
            try
            {
                Dictionary<string, string> lastbattle = new Dictionary<string, string>();
                WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();
                foreach (RecentBattle item in this.Values.Where(dbEntry => dbEntry.DBEntry == false))
                {
                    lastbattle.Clear();
                    lastbattle.Add("rbCountryID", item.CountryID.ToString());
                    lastbattle.Add("rbTankID", item.TankID.ToString());
                    lastbattle.Add("rbOriginalBattleCount", (Math.Round(item.OriginalBattleCount, 2) * 100).ToString());
                    lastbattle.Add("rbBattles", (Math.Round(item.Battles, 2) * 100).ToString());
                    lastbattle.Add("rbKills", (Math.Round(item.Kills, 2) * 100).ToString());
                    lastbattle.Add("rbDamageReceived", (Math.Round(item.DamageReceived, 2) * 100).ToString());
                    lastbattle.Add("rbDamageDealt", (Math.Round(item.DamageDealt, 2) * 100).ToString());
                    lastbattle.Add("rbDamageAssistedRadio", (Math.Round(item.DamageAssistedRadio, 2) * 100).ToString());
                    lastbattle.Add("rbDamageAssistedTracks", (Math.Round(item.DamageAssistedTracks, 2) * 100).ToString());
                    lastbattle.Add("rbXPReceived", (Math.Round(item.XPReceived, 2) * 100).ToString());
                    lastbattle.Add("rbSpotted", (Math.Round(item.Spotted, 2) * 100).ToString());
                    lastbattle.Add("rbCapturePoints", (Math.Round(item.CapturePoints, 2) * 100).ToString());
                    lastbattle.Add("rbDefencePoints", (Math.Round(item.DefencePoints, 2) * 100).ToString());
                    lastbattle.Add("rbSurvived", item.Survived.ToString());
                    lastbattle.Add("rbVictory", item.Victory.ToString());
                    lastbattle.Add("rbBattleTime", item.BattleTime.ToString());
                    lastbattle.Add("rbBattleTimeFriendly", new DateTime(item.BattleTime_Friendly.Year, item.BattleTime_Friendly.Month, item.BattleTime_Friendly.Day, item.BattleTime_Friendly.Hour, item.BattleTime_Friendly.Minute, item.BattleTime_Friendly.Second, item.BattleTime_Friendly.Millisecond).ToString("yyyy-MM-dd HH:mm:ss"));

                    lastbattle.Add("rbShot", (Math.Round(item.Shot, 2) * 100).ToString());
                    lastbattle.Add("rbHits", (Math.Round(item.Hits, 2) * 100).ToString());

                    lastbattle.Add("rbTier", item.Tier.ToString());
                    lastbattle.Add("rbBattlesPerTier", item.BattlesPerTier.ToString());

                    lastbattle.Add("rbVictoryCount", item.VictoryCount.ToString());
                    lastbattle.Add("rbDefeatCount", item.DefeatCount.ToString());
                    lastbattle.Add("rbDrawCount", item.DrawCount.ToString());
                    lastbattle.Add("rbSurviveYesCount", item.SurviveYesCount.ToString());
                    lastbattle.Add("rbSurviveNoCount", item.SurviveNoCount.ToString());

                    lastbattle.Add("rbFragList", item.FragList);

                    lastbattle.Add("rbGlobalAvgTier", (Math.Round(item.GlobalAvgTier, 2) * 100).ToString());
                    lastbattle.Add("rbGlobalWinPercentage", (Math.Round(item.GlobalWinPercentage, 2) * 100).ToString());
                    lastbattle.Add("rbGlobalAvgDefPoints", (Math.Round(item.GlobalAvgDefencePoints, 2) * 100).ToString());
                    lastbattle.Add("rbBattleMode", item.BattleMode.ToString());

                    RatingStructure ratingStruct = new RatingStructure();
                    ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
                    ratingStruct.countryID = item.CountryID;
                    ratingStruct.tankID = item.TankID;
                    ratingStruct.tier = item.Tier;
                    ratingStruct.globalTier = item.GlobalAvgTier;

                    ratingStruct.singleTank = true;

                    ratingStruct.battlesCount =Convert.ToInt32(item.Battles);
                    ratingStruct.battlesCount8_8 = Convert.ToInt32(item.Battles88);
                    ratingStruct.capturePoints = item.CapturePoints;
                    ratingStruct.defencePoints = item.DefencePoints;

                    ratingStruct.damageAssistedRadio = item.DamageAssistedRadio;
                    ratingStruct.damageAssistedTracks = item.DamageAssistedTracks;
                    ratingStruct.damageDealt = item.DamageDealt;
                    ratingStruct.frags = item.Kills;
                    ratingStruct.spotted = item.Spotted;

                    ratingStruct.wins = item.VictoryCount;
                    ratingStruct.gWinRate = item.GlobalWinPercentage;




                    lastbattle.Add("rbRatingEff", WOTStatistics.Core.WOTHelper.FormatNumberToString(WOTStatistics.Core.Ratings.GetRatingEff(ratingStruct).Value, 2));
                    lastbattle.Add("rbRatingBR", WOTStatistics.Core.WOTHelper.FormatNumberToString(WOTStatistics.Core.Ratings.GetRatingBR(ratingStruct).Value, 2));
                    lastbattle.Add("rbRatingWN7", WOTStatistics.Core.WOTHelper.FormatNumberToString(WOTStatistics.Core.Ratings.GetRatingWN7(ratingStruct).Value, 2));
                    lastbattle.Add("rbRatingWN8", WOTStatistics.Core.WOTHelper.FormatNumberToString(WOTStatistics.Core.Ratings.GetRatingWN8(ratingStruct).Value, 2));

                    
                    

                    lastbattle.Add("rbSessionID", AutoSessions());

                    using (IDBHelpers sqlLDB = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db")))
                    {
                        sqlLDB.Insert("RecentBattles", lastbattle);
                    }
                }


            }
            catch (Exception ex)
            {
                _message.Add(ex.Message);
            }
        }

        private void DeleteFile()
        {
            if (DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_CLEARRECENTERRORNOTIFY", "DE", "The recent battle log has become unstable. File will be resetted."), "WOT Statistics", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                File.Delete(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "LastBattles.wot"));
            }
        }

        public string NewSession()
        {
            string iGuid = Guid.NewGuid().ToString();

            Dictionary<string, string> tempDataStore = new Dictionary<string, string>();

            using (DBHelpers newSession = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db")))
            {
                string key = newSession.ExecuteScalar("select rsKey from RecentBattles_Session where datetime(rsUEDateTo, 'unixepoch', 'localtime') >= datetime('now', 'localtime')");
                string sql;
                if (!string.IsNullOrEmpty(key))
                {
                    if (!IsEmptySession(key))
                    {

                        sql = "update RecentBattles_Session set rsDateTo = '" + WOTHelper.CurrentDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', rsUEDateTo = strftime('%s','now') where rsKey = '" + key + "'";
                        newSession.ExecuteNonQuery(sql);
                        //Thread.Sleep(1000);
                        sql = "insert into RecentBattles_Session (rsKey, rsDateFrom, rsDateTo, rsUEDateFrom, rsUEDateTo) values ('" + iGuid + "', '" + WOTHelper.CurrentDateTime.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss") + "', '" + new DateTime(2037, 1, 1, 0, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss") + "', strftime('%s','now'), 2114373600)";
                        newSession.ExecuteNonQuery(sql);
                        return iGuid;
                    }
                    else
                    {
                        sql = "update RecentBattles_Session set rsDateFrom = '" + WOTHelper.CurrentDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', rsUEDateFrom = strftime('%s','now') where rsKey = '" + key + "'";
                        newSession.ExecuteNonQuery(sql);
                        return key;
                    }
                }
                else
                {
                    sql = "insert into RecentBattles_Session (rsKey, rsDateFrom, rsDateTo, rsUEDateFrom, rsUEDateTo) values ('" + iGuid + "', '" + WOTHelper.CurrentDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + new DateTime(2037, 1, 1, 0, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss") + "', strftime('%s','now'), 2114373600)";
                    newSession.ExecuteNonQuery(sql);
                    return iGuid;
                }
            }
        }

        private string AutoSessions()
        {
            //we need to build in a fail safe, if auto session is not enables
            if (!UserSettings.AutoCreateSession)
            {
                string failSafe_sql = String.Format("select count(rbCountryID) count from RecentBattles where rbSessionID = '{0}'", GetSession());
                using (IDBHelpers db = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db")))
                {
                    string returnValue = db.ExecuteScalar(failSafe_sql);
                    if (int.Parse(returnValue == "" ? "0" : returnValue) >= ApplicationSettings.MaxNoGamesAllowedRB)
                    {
                        string sessionID = NewSession();
                        UserSettings.ViewSessionID = sessionID;
                        UserSettings.RecentBattlesCurrentSession = 0;
                        _message.Add("Info : New Session has been created. Max Limit of games has been reached for session. Please enable auto session to avoid this in future.");
                        return sessionID;
                       
                    }
                }
            }

            //once failsafe has passed we can carry on
            if (UserSettings.AutoCreateSession)
            {
                if (!UserSettings.AutoSessionXBattles && !UserSettings.AutoSessionXHours)
                    return GetSession();
                else
                {
                    if (UserSettings.AutoSessionXBattles)
                    {
                        string sql = String.Format("select count(rbCountryID) count from RecentBattles where rbSessionID = '{0}'", GetSession());
                        using (IDBHelpers db = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db")))
                        {
                            string returnValue = db.ExecuteScalar(sql);
                            if (int.Parse(returnValue == "" ? "0" : returnValue) >= UserSettings.AutoSessionXBattlesValue)
                            {
                                if (UserSettings.AutoSessionXBattlesMessage)
                                {
                                    if (DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_NEWSESSIONNOTIFY", "DE", "Would you like to create a new recent battles session."), "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        string sessionID = NewSession();
                                        UserSettings.ViewSessionID = sessionID;
                                        UserSettings.RecentBattlesCurrentSession = 0;
                                        return sessionID;
                                    }
                                    else
                                    {
                                        return GetSession();
                                    }
                                }
                                else
                                {
                                    string sessionID = NewSession();
                                    UserSettings.ViewSessionID = sessionID;
                                    UserSettings.RecentBattlesCurrentSession = 0;
                                    return sessionID;
                                }
                            }
                            else
                            {
                                return GetSession();
                            }
                        }
                    }

                    if (UserSettings.AutoSessionXHours)
                    {
                        string sql = @"select (strftime('%s','now', 'localtime') - strftime('%s',datetime(rbBattleTime, 'unixepoch', 'localtime')))/60/60 from recentbattles order by rbID desc limit 1";
                        using (IDBHelpers db = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db")))
                        {
                            string returnValue = db.ExecuteScalar(sql);
                            if (int.Parse(returnValue == "" ? "0" : returnValue) >= UserSettings.AutoSessionXHoursValue)
                            {
                                if (UserSettings.AutoSessionXHoursMessage)
                                {
                                    if (DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_NEWSESSIONNOTIFY", "DE", "Would you like to create a new recent battles session."), "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        string sessionID = NewSession();
                                        UserSettings.ViewSessionID = sessionID;
                                        UserSettings.RecentBattlesCurrentSession = 0;
                                        return sessionID;
                                    }
                                    else
                                    {
                                        return GetSession();
                                    }
                                }
                                else
                                {
                                    string sessionID = NewSession();
                                    UserSettings.ViewSessionID = sessionID;
                                    UserSettings.RecentBattlesCurrentSession = 0;
                                    return sessionID;
                                }
                            }
                            else
                            {
                                return GetSession();
                            }

                        }
                    }

                    return GetSession();
                }



            }
            else
            {
                return GetSession();
            }
        }

        public string GetSession()
        {



            string key = String.Empty;
            using (DBHelpers newSession = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db")))
            {
                key = newSession.ExecuteScalar("select rsKey from RecentBattles_Session where datetime(rsUEDateTo,'unixepoch', 'localtime') >= datetime('now', 'localtime')");
                if (string.IsNullOrEmpty(key))
                {
                    return NewSession();
                }
                else
                {
                    return key;
                }
            }

        }

        public bool IsEmptySession(string sessionID)
        {
            string sql = String.Format(@"select count(rbID) from RecentBattles where rbSessionID = '{0}'", sessionID);
            int value = 0;
            using (DBHelpers db = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db")))
            {
                value = int.Parse(db.ExecuteScalar(sql));
            }

            if (value > 0)
                return false;
            else
                return true;
        }


        private void KeepQuota()
        {

            int number = UserSettings.LastPlayedCompareQuota;
            int counter = 1;
            foreach (RecentBattle item in this.Values.OrderByDescending(x => x.BattleTime))
            {
                if (counter > number)
                {
                    this.Remove(item.BattleTime);
                }
                counter++;
            }
        }


        private void Read()
        {
            if (!(File.Exists(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db"))) || _TakeOn)
            {
                if (File.Exists(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "LastBattles.wot")))
                {
                    try
                    {
                        string[] fileData = File.ReadAllLines(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "LastBattles.wot"));

                        foreach (string item in fileData)
                        {

                            if (item.Contains('|'))
                            {
                                string[] rowData = item.Split('|');
                                int survived = 0;
                                double battleCount = double.Parse(rowData[2]) / 100;

                                if (rowData[10] == "True" || rowData[10] == "1")
                                    survived = 1;
                                else
                                    survived = 0;
                                if (rowData.Length >= 17)
                                {
                                    Add(int.Parse(rowData[12]), new RecentBattle()
                                    {
                                        CountryID = int.Parse(rowData[0]),
                                        TankID = int.Parse(rowData[1]),
                                        Battles = (double.Parse(rowData[2]) / battleCount) / 100,
                                        Kills = (double.Parse(rowData[3]) / battleCount) / 100,
                                        DamageReceived = (double.Parse(rowData[4]) / battleCount) / 100,
                                        DamageDealt = (double.Parse(rowData[5]) / battleCount) / 100,
                                        XPReceived = (double.Parse(rowData[6]) / battleCount) / 100,
                                        Spotted = (double.Parse(rowData[7]) / battleCount) / 100,
                                        CapturePoints = (double.Parse(rowData[8]) / battleCount) / 100,
                                        DefencePoints = (double.Parse(rowData[9]) / battleCount) / 100,
                                        Survived = survived,
                                        Victory = int.Parse(rowData[11]),
                                        BattleTime = int.Parse(rowData[12]),
                                        Shot = (double.Parse(rowData[13]) / battleCount) / 100,
                                        Hits = (double.Parse(rowData[14]) / battleCount) / 100,
                                        OriginalBattleCount = rowData.Length == 18 ? (double.Parse(rowData[15])) / 100 : (battleCount),
                                        FragList = rowData.Length == 18 ? rowData[17] : ""
                                    }
                                      );
                                }
                                else
                                {
                                    Add(int.Parse(rowData[12]), new RecentBattle()
                                    {
                                        CountryID = int.Parse(rowData[0]),
                                        TankID = int.Parse(rowData[1]),
                                        Battles = (double.Parse(rowData[2]) / battleCount),
                                        Kills = (double.Parse(rowData[3]) / battleCount),
                                        DamageReceived = (double.Parse(rowData[4]) / battleCount),
                                        DamageDealt = (double.Parse(rowData[5]) / battleCount),
                                        XPReceived = (double.Parse(rowData[6]) / battleCount),
                                        Spotted = (double.Parse(rowData[7]) / battleCount),
                                        CapturePoints = (double.Parse(rowData[8]) / battleCount),
                                        DefencePoints = (double.Parse(rowData[9]) / battleCount),
                                        Survived = survived,
                                        Victory = int.Parse(rowData[11]),
                                        BattleTime = int.Parse(rowData[12]),
                                        Shot = (double.Parse(rowData[13]) / battleCount),
                                        Hits = (double.Parse(rowData[14]) / battleCount),
                                        OriginalBattleCount = rowData.Length == 18 ? (double.Parse(rowData[15])) : (battleCount),
                                        FragList = rowData.Length == 18 ? rowData[17] : ""
                                    }
                                      );
                                }
                            }
                            else
                            {
                                string[] rowData = item.Split(',');
                                if (rowData.Length > 15)
                                {
                                    //throw away record.
                                }
                                else
                                {
                                    int survived = 0;
                                    double battleCount = double.Parse(rowData[2]);

                                    if (rowData[10] == "True" || rowData[10] == "1")
                                        survived = 1;
                                    else
                                        survived = 0;

                                    Add(int.Parse(rowData[12]), new RecentBattle()
                                    {
                                        CountryID = int.Parse(rowData[0]),
                                        TankID = int.Parse(rowData[1]),
                                        Battles = double.Parse(rowData[2]) / battleCount,
                                        Kills = double.Parse(rowData[3]) / battleCount,
                                        DamageReceived = double.Parse(rowData[4]) / battleCount,
                                        DamageDealt = double.Parse(rowData[5]) / battleCount,
                                        XPReceived = double.Parse(rowData[6]) / battleCount,
                                        Spotted = double.Parse(rowData[7]) / battleCount,
                                        CapturePoints = double.Parse(rowData[8]) / battleCount,
                                        DefencePoints = double.Parse(rowData[9]) / battleCount,
                                        Survived = survived,
                                        Victory = int.Parse(rowData[11]),
                                        BattleTime = int.Parse(rowData[12]),
                                        Shot = double.Parse(rowData[13]) / battleCount,
                                        Hits = double.Parse(rowData[14]) / battleCount,
                                        OriginalBattleCount = rowData.Length == 18 ? double.Parse(rowData[15]) : battleCount,
                                        FragList = rowData.Length == 18 ? rowData[17] : ""
                                    }
                                      );
                                }
                            }
                        }
                    }
                    catch
                    {

                        DeleteFile();
                    }

                    //CreateDatabase.Create(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db"));
                    Save();


                }

            }
            else
            {
                //CreateDatabase.AlterDB(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db"));
                //DateTime datefrom = DateTime.Now.AddHours(UserSettings.TimeAdjustment);
                //new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddHours(UserSettings.TimeAdjustment)
                DateTime datefrom = new DateTime();
                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime.Hour >= Math.Abs(UserSettings.TimeAdjustment))
                {
                    // e.g. Reset At settings is 03:00 AM and current time is 05:00 PM
                    // we need to retrieve today's battles from current day 03:00 AM
                    datefrom = currentDateTime;
                    datefrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day, Convert.ToInt32(Math.Abs(UserSettings.TimeAdjustment)), 0, 0);
                }
                else
                {
                    // e.g. Reset At settings is 03:00 AM and current time is 01:00 AM
                    // we need to retrieve today's battles from yesterday at 03:00 AM
                    datefrom = currentDateTime.AddDays(-1);
                    datefrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day, Convert.ToInt32(Math.Abs(UserSettings.TimeAdjustment)), 0, 0);
                }
                using (IDBHelpers db = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerName, "LastBattle", "WOTSStore.db")))
                {
                    string sql = "";
                    if (UserSettings.RecentBattlesCurrentSession == 0)
                        if (UserSettings.ViewSessionID == null)
                            sql = @"select rbSurvived, rbCountryID, rbTankID, rbBattles, rbKills, rbDamageReceived, rbDamageDealt, rbDamageAssistedRadio, rbDamageAssistedTracks, rbXPReceived, rbSpotted, rbCapturePoints, rbDefencePoints, rbMileage,
                                    rbVictory, rbBattleTime, rbShot, rbHits, rbOriginalBattleCount, rbFragList, rbGlobalAvgTier, rbGlobalWinPercentage, rbGlobalAvgDefPoints , rbRatingBR, rbRatingEff, rbRatingWN7, rbRatingWN8
                                    from RecentBattles_Session T1 join RecentBattles T2 on rbSessionID = rsKey where datetime(rsUEDateTo,'unixepoch', 'localtime') > datetime('now', 'localtime')";
                        else
                            sql = @"select rbSurvived, rbCountryID, rbTankID, rbBattles, rbKills, rbDamageReceived, rbDamageDealt, rbDamageAssistedRadio, rbDamageAssistedTracks, rbXPReceived, rbSpotted, rbCapturePoints, rbDefencePoints, rbMileage,
                                    rbVictory, rbBattleTime, rbShot, rbHits, rbOriginalBattleCount, rbFragList, rbGlobalAvgTier, rbGlobalWinPercentage, rbGlobalAvgDefPoints, rbRatingBR, rbRatingEff, rbRatingWN7, rbRatingWN8 
                                    from RecentBattles_Session T1 join RecentBattles T2 on rbSessionID = rsKey where rsKey = '" + UserSettings.ViewSessionID + "'";
                    else
                        if (UserSettings.RecentBattlesCurrentSession == 1) // Today's battles
                        {

                            sql = String.Format(@"select rbSurvived, rbCountryID, rbTankID, rbBattles, rbKills, rbDamageReceived, rbDamageDealt, rbDamageAssistedRadio, rbDamageAssistedTracks, rbXPReceived, rbSpotted, rbCapturePoints, rbDefencePoints, rbMileage,
                                                    rbVictory, rbBattleTime, rbShot, rbHits, rbOriginalBattleCount, rbFragList, rbGlobalAvgTier, rbGlobalWinPercentage, rbGlobalAvgDefPoints, rbRatingBR, rbRatingEff, rbRatingWN7, rbRatingWN8
                                                    from RecentBattles T2 
                                                    where datetime(rbBattleTime, 'unixepoch') >= datetime({0}, 'unixepoch')", WOTHelper.ConvertToUnixTimestamp(datefrom));
                        }
                        else
                            if (UserSettings.RecentBattlesCurrentSession == 2) // Last 3 days
                            {

                                datefrom = datefrom.AddDays(-2);

                                sql = String.Format(@"select rbSurvived, rbCountryID, rbTankID, rbBattles, rbKills, rbDamageReceived, rbDamageDealt, rbDamageAssistedRadio, rbDamageAssistedTracks, rbXPReceived, rbSpotted, rbCapturePoints, rbDefencePoints, rbVictory, rbBattleTime, rbMileage,
                                                        rbShot, rbHits, rbOriginalBattleCount, rbFragList, rbGlobalAvgTier, rbGlobalWinPercentage, rbGlobalAvgDefPoints, rbRatingBR, rbRatingEff, rbRatingWN7, rbRatingWN8 from RecentBattles T2 
                                                        where datetime(rbBattleTime, 'unixepoch') >= datetime({0}, 'unixepoch')", WOTHelper.ConvertToUnixTimestamp(datefrom));
                            }
                            else
                                if (UserSettings.RecentBattlesCurrentSession == 3) // Last week
                                {

                                    datefrom = datefrom.AddDays(-6);

                                    sql = String.Format(@"select rbSurvived, rbCountryID, rbTankID, rbBattles, rbKills, rbDamageReceived, rbDamageDealt, rbDamageAssistedRadio, rbDamageAssistedTracks, rbXPReceived, rbSpotted, rbCapturePoints, rbDefencePoints, rbVictory, rbBattleTime, rbShot, rbHits, rbMileage,
                                                            rbOriginalBattleCount, rbFragList, rbGlobalAvgTier, rbGlobalWinPercentage, rbGlobalAvgDefPoints, rbRatingBR, rbRatingEff, rbRatingWN7, rbRatingWN8  from RecentBattles T2 
                                                            where datetime(rbBattleTime, 'unixepoch') >= datetime({0}, 'unixepoch')", WOTHelper.ConvertToUnixTimestamp(datefrom));
                                }
                                else
                                    if (UserSettings.RecentBattlesCurrentSession == 4)
                                        sql = "select rbSurvived, rbCountryID, rbTankID, rbBattles, rbKills, rbDamageReceived, rbDamageDealt, rbDamageAssistedRadio, rbDamageAssistedTracks, rbXPReceived, rbSpotted, rbCapturePoints, rbDefencePoints, rbVictory, rbBattleTime, rbShot, rbHits, rbOriginalBattleCount, rbFragList, rbMileage, rbGlobalAvgTier, rbGlobalWinPercentage, rbGlobalAvgDefPoints, rbRatingBR, rbRatingEff, rbRatingWN7, rbRatingWN8 from RecentBattles where 1=1 ";


                    if (UserSettings.BattleMode == "15")
                        sql += " and rbBattleMode = 15 ";
                    else if (UserSettings.BattleMode == "7")
                        sql += " and rbBattleMode = 7";

                   // if (UserSettings.RecentBattlesCurrentSession == 4)
                    sql += @" order by rbID desc ";

                    if (UserSettings.RecentBattlesCurrentSession == 4)
                        sql += @" LIMIT " + UserSettings.LastPlayedCompareQuota;
                    else
                        sql += @" LIMIT " + ApplicationSettings.MaxNoGamesAllowedRB;

                    using (DataTable dt = db.GetDataTable(sql))
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            int survived = 0;
                            double battleCount = double.Parse(item["rbBattles"].ToString()) / 100;
                            if (item["rbSurvived"].ToString() == "True" || item["rbSurvived"].ToString() == "1")
                                survived = 1;
                            else
                                survived = 0;
                            Add(int.Parse(item["rbBattleTime"].ToString()), new RecentBattle()
                            {
                                CountryID = int.Parse(item["rbCountryID"].ToString()),
                                TankID = int.Parse(item["rbTankID"].ToString()),
                                Battles = (double.Parse(item["rbBattles"].ToString()) / battleCount) / 100,
                                Kills = (double.Parse(item["rbKills"].ToString()) / battleCount) / 100,
                                DamageReceived = (double.Parse(item["rbDamageReceived"].ToString()) / battleCount) / 100,
                                DamageDealt = (double.Parse(item["rbDamageDealt"].ToString()) / battleCount) / 100,
                                DamageAssistedRadio = (double.Parse(item["rbDamageAssistedRadio"].ToString()) / battleCount) / 100,
                                DamageAssistedTracks = (double.Parse(item["rbDamageAssistedTracks"].ToString()) / battleCount) / 100,
                                XPReceived = (double.Parse(item["rbXPReceived"].ToString()) / battleCount) / 100,
                                Spotted = (double.Parse(item["rbSpotted"].ToString()) / battleCount) / 100,
                                CapturePoints = (double.Parse(item["rbCapturePoints"].ToString()) / battleCount) / 100,
                                DefencePoints = (double.Parse(item["rbDefencePoints"].ToString()) / battleCount) / 100,
                                Survived = survived,
                                Victory = int.Parse(item["rbVictory"].ToString()),
                                BattleTime = int.Parse(item["rbBattleTime"].ToString()),
                                Mileage = (double.Parse(item["rbMileage"].ToString()) / battleCount) / 100,
                                Shot = (double.Parse(item["rbShot"].ToString()) / battleCount) / 100,
                                Hits = (double.Parse(item["rbHits"].ToString()) / battleCount) / 100,
                                OriginalBattleCount = (double.Parse(item["rbOriginalBattleCount"].ToString())) / 100,
                                FragList = item["rbFragList"].ToString(),
                                GlobalAvgTier = (double.Parse(item["rbGlobalAvgTier"].ToString())) / 100,
                                GlobalWinPercentage = (double.Parse(item["rbGlobalWinPercentage"].ToString())) / 100,
                                GlobalAvgDefencePoints = (double.Parse(item["rbGlobalAvgDefPoints"].ToString())) / 100,
                                RatingBR = (double.Parse(item["rbRatingBR"].ToString())  / battleCount) / 100,
                                RatingEff = (double.Parse(item["rbRatingEff"].ToString())  / battleCount) / 100,
                                RatingWN7 = (double.Parse(item["rbRatingWN7"].ToString()) / battleCount) / 100,
                                RatingWN8 = (double.Parse(item["rbRatingWN8"].ToString()) / battleCount) / 100,
                                DBEntry = true
                            });
                        }
                    }
                }

            }
        }


    }

    public class RecentBattle
    {
        public int CountryID { get; set; }
        public int TankID { get; set; }
        public double OriginalBattleCount { get; set; }
        public double Battles { get; set; }
        public double Battles88 { get; set; }
        public double Kills { get; set; }
        public double DamageReceived { get; set; }
        public double DamageDealt { get; set; }
        public double DamageAssistedRadio { get; set; }
        public double DamageAssistedTracks { get; set; }
        public double XPReceived { get; set; }
        public double Mileage { get; set; }
        public double Spotted { get; set; }
        public double CapturePoints { get; set; }
        public double DefencePoints { get; set; }
        public int Survived { get; set; }
        public int Victory { get; set; }
        public int BattleTime { get; set; }
        public double Shot { get; set; }
        public double Hits { get; set; }
        public int Tier { get; set; }
        public double BattlesPerTier { get; set; }
        public double VictoryCount { get; set; }
        public double DefeatCount { get; set; }
        public double DrawCount { get; set; }
        public double SurviveYesCount { get; set; }
        public double SurviveNoCount { get; set; }
        public string FragList { get; set; }
        public int BattleMode { get; set; }
        public DateTime BattleTime_Friendly
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(BattleTime).AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours);
            }
        }
        public double GlobalAvgTier { get; set; }
        public double GlobalWinPercentage { get; set; }
        public double GlobalAvgDefencePoints { get; set; }
        public double RatingEff { get; set; }
        public double RatingBR { get; set; }
        public double RatingWN7 { get; set; }
        public double RatingWN8 { get; set; }
        public bool DBEntry { get; set; }


    }

    public static class RecentBattleHelpers
    {



        public static Tuple<DateTime?, DateTime?, int?> GetSessionDates(string playerName)
        {
            using (DBHelpers newSession = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + playerName, "LastBattle", "WOTSStore.db")))
            {
                string sql;
                if (UserSettings.ViewSessionID == null)
                    sql = String.Format("select rsID, rsKey, datetime(rsUEDateFrom, 'unixepoch', 'localtime') as rsDateFrom, datetime(rsUEDateTo, 'unixepoch', 'localtime') as rsDateTo from RecentBattles_Session where datetime(rsUEDateTo, 'unixepoch', 'localtime') >= datetime({0}, 'unixepoch', 'localtime')", WOTHelper.ConvertToUnixTimestamp(WOTHelper.CurrentDateTime));
                else
                    sql = String.Format("select rsID, rsKey, datetime(rsUEDateFrom, 'unixepoch', 'localtime') as rsDateFrom, datetime(rsUEDateTo, 'unixepoch', 'localtime') as rsDateTo from RecentBattles_Session where rsKey = '{0}'", UserSettings.ViewSessionID);
                DataTable dt = newSession.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return new Tuple<DateTime?, DateTime?, int?>(Convert.ToDateTime(dt.Rows[0]["rsDateFrom"]), Convert.ToDateTime(dt.Rows[0]["rsDateTo"]), Convert.ToInt16(dt.Rows[0]["rsID"]));
                }
                else
                {
                    return new Tuple<DateTime?, DateTime?, int?>(null, null, null);
                }
            }
        }


    }
}
