using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOTStatistics.SQLite;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections;

namespace WOT.Stats
{
    internal static class ConvertFlatFileToDB
    {
        public static void DoData(string playerFolder, string dbPath, UpdateGUIProgressBar statusBar)
        {
            using (IDBHelpers db = new DBHelpers(dbPath, true))
            {
                db.BeginTransaction();
                db.ClearTable("Files");
                db.ClearTable("File_Achievements");
                db.ClearTable("File_Battles");
                db.ClearTable("File_Clan");
                db.ClearTable("File_Company");
                db.ClearTable("File_FragList");
                db.ClearTable("File_Total");

                string[] dir = Directory.GetFiles(playerFolder, "*.txt");
                int fileCount = 0;
                statusBar(new Tuple<int, int>(fileCount, dir.Length-1), 2);
                foreach (var historyFile in dir)
                {
                   

                    statusBar(new Tuple<int, int>(fileCount, dir.Length-1), 2);
                    fileCount++;
                    try
                    {
                        string file = File.ReadAllText(historyFile);
                        var file_Serializer = new JavaScriptSerializer();
                        file_Serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                        dynamic dyn_fileObject = file_Serializer.Deserialize(file, typeof(object));


                        //Create File table data.
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        SafeObjectHandler(data, "fiDate", GetSafeValue(dyn_fileObject.date));
                        SafeObjectHandler(data, "fiParser", GetSafeValue(dyn_fileObject.parser));
                        SafeObjectHandler(data, "fiTankCount", GetSafeValue(dyn_fileObject.tankcount));
                        SafeObjectHandler(data, "fiParserVersion", GetSafeValue(dyn_fileObject.parserversion));
                        FileInfo fi = new FileInfo(historyFile);
                        SafeObjectHandler(data, "fiID", fi.Name.Replace(".txt", ""));

                        db.Insert("Files", data);
                        data.Clear();

                        string fileID = fi.Name.Replace(".txt", "");

                        foreach (var tank in dyn_fileObject.tanks)
                        {
                            SafeObjectHandler(data, "cmFileID", fileID);
                            SafeObjectHandler(data, "cmUpdated", GetSafeValue(tank.updated));
                            SafeObjectHandler(data, "cmCountryID", GetSafeValue(tank.countryid));
                            SafeObjectHandler(data, "cmTankID", GetSafeValue(tank.tankid));
                            SafeObjectHandler(data, "cmBaseVersion", GetSafeValue(tank.basedonversion));
                            SafeObjectHandler(data, "cmLastBattleTime", GetSafeValue(tank.tankdata.lastBattleTime));

                            db.Insert("File_TankDetails", data);
                            data.Clear();

                            string tankID = String.Empty;
                            try
                            {
                                tankID = db.ExecuteScalar("select cmID from File_TankDetails where cmFileID = " + fileID + " and cmTankID = " + GetSafeValue(tank.tankid) + " and cmCountryID = " + GetSafeValue(tank.countryid));

                            }
                            catch { }
                            if (ObjectExists(tank.clan))
                            {
                                if (tank.clan.GetType() != typeof(ArrayList))
                                {
                                    SafeObjectHandler(data, "clParentID", tankID);
                                    SafeObjectHandler(data, "clBattlesCount", GetSafeValue(tank.clan.battlesCount));
                                    SafeObjectHandler(data, "clCapturePoints", GetSafeValue(tank.clan.capturePoints));
                                    SafeObjectHandler(data, "clDamageDealt", GetSafeValue(tank.clan.damageDealt));
                                    SafeObjectHandler(data, "clDamageReceived", GetSafeValue(tank.clan.damageReceived));
                                    SafeObjectHandler(data, "clDefencePoints", GetSafeValue(tank.clan.droppedCapturePoints));
                                    SafeObjectHandler(data, "clFrags", GetSafeValue(tank.clan.frags));
                                    SafeObjectHandler(data, "clHits", GetSafeValue(tank.clan.hits));
                                    SafeObjectHandler(data, "clLosses", GetSafeValue(tank.clan.losses));
                                    SafeObjectHandler(data, "clShots", GetSafeValue(tank.clan.shots));
                                    SafeObjectHandler(data, "clSpotted", GetSafeValue(tank.clan.spotted));
                                    SafeObjectHandler(data, "clSurvivedBattles", GetSafeValue(tank.clan.survivedBattles));
                                    SafeObjectHandler(data, "clWins", GetSafeValue(tank.clan.wins));
                                    SafeObjectHandler(data, "clXP", GetSafeValue(tank.clan.xp));
                                    db.Insert("File_Clan", data);
                                    data.Clear();
                                }
                                else
                                {
                                    foreach (var clan in tank.clan)
                                    {
                                        SafeObjectHandler(data, "clParentID", tankID);
                                        SafeObjectHandler(data, "clBattlesCount", GetSafeValue(clan.battlesCount));
                                        SafeObjectHandler(data, "clCapturePoints", GetSafeValue(clan.capturePoints));
                                        SafeObjectHandler(data, "clDamageDealt", GetSafeValue(clan.damageDealt));
                                        SafeObjectHandler(data, "clDamageReceived", GetSafeValue(clan.damageReceived));
                                        SafeObjectHandler(data, "clDefencePoints", GetSafeValue(clan.droppedCapturePoints));
                                        SafeObjectHandler(data, "clFrags", GetSafeValue(clan.frags));
                                        SafeObjectHandler(data, "clHits", GetSafeValue(clan.hits));
                                        SafeObjectHandler(data, "clLosses", GetSafeValue(clan.losses));
                                        SafeObjectHandler(data, "clShots", GetSafeValue(clan.shots));
                                        SafeObjectHandler(data, "clSpotted", GetSafeValue(clan.spotted));
                                        SafeObjectHandler(data, "clSurvivedBattles", GetSafeValue(clan.survivedBattles));
                                        SafeObjectHandler(data, "clWins", GetSafeValue(clan.wins));
                                        SafeObjectHandler(data, "clXP", GetSafeValue(clan.xp));
                                        db.Insert("File_Clan", data);
                                        data.Clear();
                                    }
                                }
                            }

                            if (ObjectExists(tank.company))
                            {
                                if (tank.company.GetType() != typeof(ArrayList))
                                {
                                    SafeObjectHandler(data, "clParentID", tankID);
                                    SafeObjectHandler(data, "fcBattlesCount", GetSafeValue(tank.company.battlesCount));
                                    SafeObjectHandler(data, "fcCapturePoints", GetSafeValue(tank.company.capturePoints));
                                    SafeObjectHandler(data, "fcDamageDealt", GetSafeValue(tank.company.damageDealt));
                                    SafeObjectHandler(data, "fcDamageReceived", GetSafeValue(tank.company.damageReceived));
                                    SafeObjectHandler(data, "fcDefencePoints", GetSafeValue(tank.company.droppedCapturePoints));
                                    SafeObjectHandler(data, "fcFrags", GetSafeValue(tank.company.frags));
                                    SafeObjectHandler(data, "fcHits", GetSafeValue(tank.company.losses));
                                    SafeObjectHandler(data, "fcShots", GetSafeValue(tank.company.shots));
                                    SafeObjectHandler(data, "fcSpotted", GetSafeValue(tank.company.spotted));
                                    SafeObjectHandler(data, "fcSurvivedBattles", GetSafeValue(tank.company.survivedBattles));
                                    SafeObjectHandler(data, "fcWins", GetSafeValue(tank.company.wins));
                                    SafeObjectHandler(data, "fcXP", GetSafeValue(tank.company.xp));
                                    db.Insert("File_Company", data);
                                    data.Clear();
                                }
                                else
                                {
                                    foreach (var clan in tank.company)
                                    {
                                        SafeObjectHandler(data, "fcParentID", tankID);
                                        SafeObjectHandler(data, "fcBattlesCount", GetSafeValue(clan.battlesCount));
                                        SafeObjectHandler(data, "fcCapturePoints", GetSafeValue(clan.capturePoints));
                                        SafeObjectHandler(data, "fcDamageDealt", GetSafeValue(clan.damageDealt));
                                        SafeObjectHandler(data, "fcDamageReceived", GetSafeValue(clan.damageReceived));
                                        SafeObjectHandler(data, "fcDefencePoints", GetSafeValue(clan.droppedCapturePoints));
                                        SafeObjectHandler(data, "fcFrags", GetSafeValue(clan.frags));
                                        SafeObjectHandler(data, "fcHits", GetSafeValue(clan.hits));
                                        SafeObjectHandler(data, "fcLosses", GetSafeValue(clan.losses));
                                        SafeObjectHandler(data, "fcShots", GetSafeValue(clan.shots));
                                        SafeObjectHandler(data, "fcSpotted", GetSafeValue(clan.spotted));
                                        SafeObjectHandler(data, "fcSurvivedBattles", GetSafeValue(clan.survivedBattles));
                                        SafeObjectHandler(data, "fcWins", GetSafeValue(clan.wins));
                                        SafeObjectHandler(data, "fcXP", GetSafeValue(clan.xp));
                                        db.Insert("File_Company", data);
                                        data.Clear();
                                    }
                                }
                            }

                            string insert = "Insert into File_FragList (fgParentID, fgCountryID, fgTankID, fgValue)";
                            List<string> values = new List<string>();
                            foreach (var kills in tank.kills)
                            {
                                values.Add(String.Format("Select '{0}', '{1}', '{2}', '{3}'", tankID, GetSafeValue(kills[0]), GetSafeValue(kills[1]), GetSafeValue(kills[2])) );
                            }
                            if (values.Count > 0)
                            {
                                if (values.Count >= 400)
                                {
                                    foreach (var item in values)
                                    {
                                        db.ExecuteNonQuery(insert + " " + item);
                                    }
                                }
                                else
                                {
                                    db.ExecuteNonQuery(insert + string.Join(" union all ", values));
                                }
                               
                            }
                            values.Clear();

                            SafeObjectHandler(data, "foParentID", tankID);
                            SafeObjectHandler(data, "foTreesCut", GetSafeValue(tank.tankdata.treesCut));
                            SafeObjectHandler(data, "foBattleLifeTime", GetSafeValue(tank.tankdata.battleLifeTime));
                            SafeObjectHandler(data, "foLastBattleTime", GetSafeValue(tank.tankdata.lastBattleTime));
                            db.Insert("File_Total", data);
                            data.Clear();


                            SafeObjectHandler(data, "bpParentID", tankID);
                            SafeObjectHandler(data, "bpBattleCount", GetSafeValue(tank.tankdata.battlesCount));
                            SafeObjectHandler(data, "bpCapturePoints", GetSafeValue(tank.tankdata.capturePoints));
                            SafeObjectHandler(data, "bpDamageDealt", GetSafeValue(tank.tankdata.damageDealt));
                            SafeObjectHandler(data, "bpDamageReceived", GetSafeValue(tank.tankdata.damageReceived));
                            SafeObjectHandler(data, "bpDefencePoints", GetSafeValue(tank.tankdata.droppedCapturePoints));
                            SafeObjectHandler(data, "bpFrags", GetSafeValue(tank.tankdata.frags));
                            SafeObjectHandler(data, "bpFrags8P", GetSafeValue(tank.tankdata.frags8p));
                            SafeObjectHandler(data, "bpHits", GetSafeValue(tank.tankdata.hits));                           
                            SafeObjectHandler(data, "bpLosses", GetSafeValue(tank.tankdata.losses));
                            SafeObjectHandler(data, "bpMaxFrags", GetSafeValue(tank.tankdata.maxFrags));
                            SafeObjectHandler(data, "bpMaxXP", GetSafeValue(tank.tankdata.maxXP));
                            SafeObjectHandler(data, "bpShots", GetSafeValue(tank.tankdata.shots));
                            SafeObjectHandler(data, "bpSpotted", GetSafeValue(tank.tankdata.spotted));
                            SafeObjectHandler(data, "bpSurvivedBattles", GetSafeValue(tank.tankdata.survivedBattles));
                            SafeObjectHandler(data, "bpWinAndSurvive", GetSafeValue(tank.tankdata.winAndSurvived));
                            SafeObjectHandler(data, "bpWins", GetSafeValue(tank.tankdata.wins));
                            SafeObjectHandler(data, "bpXP", GetSafeValue(tank.tankdata.xp));
                            db.Insert("File_Battles", data);
                            data.Clear();

                            SafeObjectHandler(data, "faParentID", tankID);
                            SafeObjectHandler(data, "faDieHardSeries", GetSafeValue(tank.series.diehardSeries));
                            SafeObjectHandler(data, "faInvincibleSeries", GetSafeValue(tank.series.invincibleSeries));
                            SafeObjectHandler(data, "faKillingSeries", GetSafeValue(tank.series.killingSeries));
                            SafeObjectHandler(data, "faMaxDieHardSeries", GetSafeValue(tank.series.maxDiehardSeries));
                            SafeObjectHandler(data, "faMaxInvincibleSeries", GetSafeValue(tank.series.maxInvincibleSeries));
                            SafeObjectHandler(data, "faMaxKillingSeries", GetSafeValue(tank.series.maxKillingSeries));
                            SafeObjectHandler(data, "faMaxPiercingSeries", GetSafeValue(tank.series.maxPiercingSeries));
                            SafeObjectHandler(data, "faMaxSniperSeries", GetSafeValue(tank.series.maxSniperSeries));
                            SafeObjectHandler(data, "faPiercingSeries", GetSafeValue(tank.series.piercingSeries));
                            SafeObjectHandler(data, "faSniperSeries", GetSafeValue(tank.series.sniperSeries));

                            SafeObjectHandler(data, "faMedalAbrams", GetSafeValue(tank.major.Abrams));
                            SafeObjectHandler(data, "faMedalCarius", GetSafeValue(tank.major.Carius));
                            SafeObjectHandler(data, "faMedalEkins", GetSafeValue(tank.major.Ekins));
                            SafeObjectHandler(data, "faMedalKay", GetSafeValue(tank.major.Kay));
                            SafeObjectHandler(data, "faMedalKnispel", GetSafeValue(tank.major.Knispel));
                            SafeObjectHandler(data, "faMedalLavrinenko", GetSafeValue(tank.major.Lavrinenko));
                            SafeObjectHandler(data, "faMedalLeClerc", GetSafeValue(tank.major.LeClerc));
                            SafeObjectHandler(data, "faMedalPoppel", GetSafeValue(tank.major.Poppel));
                            
                            SafeObjectHandler(data, "faFragsBeast", GetSafeValue(tank.tankdata.fragsBeast));
                            SafeObjectHandler(data, "faBattleHeroes", GetSafeValue(tank.battle.battleHeroes));
                            SafeObjectHandler(data, "faDefender", GetSafeValue(tank.battle.defender));
                            SafeObjectHandler(data, "faEveilEye", GetSafeValue(tank.battle.evileye));
                            SafeObjectHandler(data, "faFragsSinai", GetSafeValue(tank.battle.fragsSinai));
                            SafeObjectHandler(data, "faInvader", GetSafeValue(tank.battle.invader));
                            SafeObjectHandler(data, "faScout", GetSafeValue(tank.battle.scout));
                            SafeObjectHandler(data, "faSniper", GetSafeValue(tank.battle.sniper));
                            SafeObjectHandler(data, "faSteelwall", GetSafeValue(tank.battle.steelwall));
                            SafeObjectHandler(data, "faSupporter", GetSafeValue(tank.battle.supporter));
                            SafeObjectHandler(data, "faWarrior", GetSafeValue(tank.battle.warrior));

                            SafeObjectHandler(data, "faMedalBillotte", GetSafeValue(tank.epic.Billotte));
                            SafeObjectHandler(data, "faMedalBurda", GetSafeValue(tank.epic.Burda));
                            SafeObjectHandler(data, "faMedalDeLanglade", GetSafeValue(tank.epic.DeLaglanda));
                            //SafeObjectHandler(data, "faWarrior", tank.epic.Erohin);
                            SafeObjectHandler(data, "faMedalFadin", GetSafeValue(tank.epic.Fadin));
                            SafeObjectHandler(data, "faMedalHalonen", GetSafeValue(tank.epic.Halonen));
                            SafeObjectHandler(data, "faHeroesOfRasseney", GetSafeValue(tank.epic.HeroesOfRassenai));
                            //SafeObjectHandler(data, "faWarrior", tank.epic.Horoshilov);
                            SafeObjectHandler(data, "faMedalKolobanov", GetSafeValue(tank.epic.Kolobanov));
                            //SafeObjectHandler(data, "faWarrior", tank.epic.Lister);
                            SafeObjectHandler(data, "faMedalOrlik", GetSafeValue(tank.epic.Orlik));
                            SafeObjectHandler(data, "faMedalOskin", GetSafeValue(tank.epic.Oskin));
                            SafeObjectHandler(data, "faMedalTamadaYoshio", GetSafeValue(tank.epic.TamadaYoshio));
                            SafeObjectHandler(data, "faMedalWittmann", GetSafeValue(tank.epic.Wittmann));

                            SafeObjectHandler(data, "faArmorPiercer", GetSafeValue(tank.special.armorPiercer));
                            SafeObjectHandler(data, "faBeastHunter", GetSafeValue(tank.special.beasthunter));
                            SafeObjectHandler(data, "faDieHard", GetSafeValue(tank.special.diehard));
                            SafeObjectHandler(data, "faHandOfDeath", GetSafeValue(tank.special.handOfDeath));
                            SafeObjectHandler(data, "faInvincible", GetSafeValue(tank.special.invincible));
                            SafeObjectHandler(data, "faKamikaze", GetSafeValue(tank.special.kamikaze));
                            SafeObjectHandler(data, "faLumberJack", GetSafeValue(tank.special.lumberjack));
                            SafeObjectHandler(data, "faMarkOfMastery", GetSafeValue(tank.special.markOfMastery));
                            SafeObjectHandler(data, "faMousebane", GetSafeValue(tank.special.mousebane));
                            SafeObjectHandler(data, "faRaider", GetSafeValue(tank.special.raider));
                            SafeObjectHandler(data, "faTitleSniper", GetSafeValue(tank.special.sniper));
                            SafeObjectHandler(data, "faTankExpertStrg", GetSafeValue(tank.special.tankExpert));

                            db.Insert("File_Achievements", data);
                            data.Clear();

                        }

                      
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                db.ExecuteNonQuery("delete from File_Battles where bpwins > bpBattleCount");
                db.EndTransaction();
            }

        }

        private static void SafeObjectHandler(Dictionary<string, string> data, string fieldName, object testField)
        {
            if (testField != null)
            {
                data.Add(fieldName, testField.ToString());
            }
        }

        private static bool ObjectExists(object testField)
        {
            if (testField != null)
            {
                return true;
            }
            return false;
        }

        private static string GetSafeValue(object testField)
        {
            if (testField != null)
            {
                return testField.ToString();
            }

            return "";
        }
    }
}
