using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WOTStatistics.SQLite
{
    public static class DatabaseSanityChecker
    {
        public static void Create(string fileName)
        {
            using (IDBHelpers dbHelpers = new DBHelpers(fileName))
            {
                dbHelpers.CreateDatabase(fileName);
                CreateTables(dbHelpers);

            }
        }

        public static void CorrectBrokenData(string fileName)
        {
            using (IDBHelpers db = new DBHelpers(fileName))
            {
                db.ExecuteNonQuery(@"update RecentBattles_Session set rsUEDateTo = rsUEDateFrom + 1  where rsUEDateTo = 2114373600 and rsID <> (select max(rsID) from RecentBattles_Session where rsUEDateTo = 2114373600)");
                db.ExecuteNonQuery(@"delete from RecentBattles where cast(rbDamageReceived as int) < 0");
                db.ExecuteNonQuery(@"delete from File_Battles where ifnull(cast(bpMaxFrags as int),0) > 15");
                db.ExecuteNonQuery(@"delete from RecentBattles where cast(rbXPReceived as int) / 100 > 10000  and cast(rbBattleTime as real) <=  1384113533");   
                db.ExecuteNonQuery(@"delete from RecentBattles where rbBattleTime = 0");
                db.ExecuteNonQuery(@"delete from RecentBattles
                                        where rbID in (
                                        select y.rbID
                                        from RecentBattles y
                                        inner join (
                                        select rbBattleTime, max(rbID) id from RecentBattles b2 group by rbBattleTime) x
                                        on y.rbBattleTime = x.rbBattleTime
                                        where y.rbID <> x.id)");

                
            }
        }


        public static void UpdateLegacyData(string fileName, Dictionary<string, Tuple<int, int, string>> tanks)
        {
            using (IDBHelpers db = new DBHelpers(fileName, true))
            {
                db.BeginTransaction();
                string sql = "select cmCountryID || '_' || cmTankID  TankKey, cmCountryID, cmTankID from File_TankDetails where cmTier is null group by cmCountryID, cmTankID";
                using (DataTable dt = db.GetDataTable(sql))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Tuple<int, int, string> tankDef;
                            if (tanks.TryGetValue(row["TankKey"].ToString(), out tankDef))
                            {
                                string updateSQL = String.Format("Update File_TankDetails set cmType = '{0}', cmTier = '{1}', cmTankTitle = '{2}' where cmCountryID = '{3}' and cmTankID = '{4}' and cmTier is null", 
                                                       tankDef.Item1, 
                                                       tankDef.Item2, 
                                                       tankDef.Item3, 
                                                       row["cmCountryID"], 
                                                       row["cmTankID"]);
                                try
                                {
                                    db.ExecuteNonQuery(updateSQL);
                                }
                                catch
                                {/*very bad practice. never swallow errors*/ }
                            }

                        }
                    }
                }
                db.EndTransaction();
            }
        }


        private static void CheckAndCreateTable(IDBHelpers dbHelpers, string tableName, string tableScript)
        {
            try
            {
                string sql = String.Format(@"SELECT name FROM sqlite_master WHERE type='table' AND name='{0}';", tableName);
                using (DataTable tableCheck = dbHelpers.GetDataTable(sql))
                {
                    if (tableCheck.Rows.Count <= 0)
                    {
                        dbHelpers.ExecuteNonQuery(tableScript);
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static void ReIndexDB(string fileName)
        {
            using (IDBHelpers dbHelpers = new DBHelpers(fileName))
            {
                string sql = @"SELECT name FROM sqlite_master WHERE type='table' AND name='RecentBattles';";
                using (DataTable tableCheck = dbHelpers.GetDataTable(sql))
                {
                    foreach (DataRow row in tableCheck.Rows)
                    {
                        dbHelpers.ExecuteNonQuery(String.Format("reindex '{0}'", row["name"]));
                    }
                }
            }
        }

        public static void ShrinkDB(string fileName)
        {
            using (IDBHelpers dbHelpers = new DBHelpers(fileName))
            {
                dbHelpers.ExecuteNonQuery("vacuum 'WOTSTORE'");
            }
        }

        private static void CreateTables(IDBHelpers dbHelpers)
        {
                CheckAndCreateTable(dbHelpers, "RecentBattles", DatabaseScripts.CreateRecentBattlesTable);
                CheckAndCreateTable(dbHelpers, "RecentBattles_Session", DatabaseScripts.CreateRecentBattlesSessionTable);
                CheckAndCreateTable(dbHelpers, "Files", DatabaseScripts.CreateFilesTable);
                CheckAndCreateTable(dbHelpers, "File_TankDetails", DatabaseScripts.CreateFile_TankDetailsTable);
                CheckAndCreateTable(dbHelpers, "File_Total", DatabaseScripts.CreateFile_TotalTable);
                CheckAndCreateTable(dbHelpers, "File_FragList", DatabaseScripts.CreateFile_FragListTable);
                CheckAndCreateTable(dbHelpers, "File_Company", DatabaseScripts.CreateFile_CompanyTable);
                CheckAndCreateTable(dbHelpers, "File_Clan", DatabaseScripts.CreateFile_ClanTable);
                CheckAndCreateTable(dbHelpers, "File_Historical", DatabaseScripts.CreateFile_HistoricalTable);
                CheckAndCreateTable(dbHelpers, "File_Battles", DatabaseScripts.CreateFile_BattlesTable);
                CheckAndCreateTable(dbHelpers, "File_Achievements", DatabaseScripts.CreateFile_AchievementsTable);
                CheckAndCreateTable(dbHelpers, "Overall", DatabaseScripts.CreateFile_OverallTable);
                CheckAndCreateTable(dbHelpers, "Cache_LastGame", DatabaseScripts.CreateCache_LastGame);
        }

        public static void AlterDB(string fileName)
        {
            using (IDBHelpers dbHelpers = new DBHelpers(fileName))
            {

                Console.WriteLine("Creating Tables...");
                CreateTables(dbHelpers);
                Console.WriteLine("Altering DB...");

                using (DataTable dt = dbHelpers.GetDataTable("pragma table_info(File_Battles)"))
                {

                    CheckAndCreateColumn("File_Battles", "bpMileage", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingEff", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingEffWeight", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingBR", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingBRWeight", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingWN7", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingWN7Weight", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingWN8", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingWN8Weight", "INTEGER null", dbHelpers, dt);
                    CheckAndCreateColumn("File_Battles", "bpRatingVersion", "INTEGER default (0) null", dbHelpers, dt);

                    //CheckAndCreateColumn("File_Battles", "bpRatingExpDamage", "real default (0) null", dbHelpers, dt);
                    //CheckAndCreateColumn("File_Battles", "bpRatingExpFrag", "real default (0) null", dbHelpers, dt);
                    //CheckAndCreateColumn("File_Battles", "bpRatingExpSpot", "real default (0) null", dbHelpers, dt);
                    //CheckAndCreateColumn("File_Battles", "bpRatingExpDef", "real default (0) null", dbHelpers, dt);
                    //CheckAndCreateColumn("File_Battles", "bpRatingExpWin", "real default (0) null", dbHelpers, dt);

      
                  
                }

                using (DataTable dt = dbHelpers.GetDataTable("pragma table_info(RecentBattles)"))
                {

                    CheckAndCreateColumn("RecentBattles", "rbMileage", "real default(0)", dbHelpers, dt);
                    CheckAndCreateColumn("RecentBattles", "rbRatingEff", "real default(0)", dbHelpers, dt);
                    CheckAndCreateColumn("RecentBattles", "rbRatingBR", "real default(0)", dbHelpers, dt);
                    CheckAndCreateColumn("RecentBattles", "rbRatingWN7", "real default(0)", dbHelpers, dt);
                    CheckAndCreateColumn("RecentBattles", "rbRatingWN8", "real default(0)", dbHelpers, dt);
                    CheckAndCreateColumn("RecentBattles", "rbDamageAssistedRadio", "real default(0)", dbHelpers, dt);
                    CheckAndCreateColumn("RecentBattles", "rbDamageAssistedTracks", "real default(0)", dbHelpers, dt);
                    CheckAndCreateColumn("RecentBattles", "rbGlobalAvgDefPoints", "REAL default (0)  NULL", dbHelpers, dt);
                    CheckAndCreateColumn("RecentBattles", "rbBattleMode", "INTEGER default (15)  NULL", dbHelpers, dt);
                }

                using (DataTable dt = dbHelpers.GetDataTable("pragma table_info(RecentBattles_Session)"))
                {
                    var fieldCheck = (from x in dt.AsEnumerable()
                                      where x.Field<string>("name") == "rsUEDateFrom"
                                      select x).DefaultIfEmpty(null).FirstOrDefault();

                    if (fieldCheck == null)
                    {
                        string sql = "Alter table RecentBattles_Session Add rsUEDateFrom real NULL";
                        dbHelpers.ExecuteNonQuery(sql);
                        sql = "Alter table RecentBattles_Session Add rsUEDateTo real NULL";
                        dbHelpers.ExecuteNonQuery(sql);
                        sql = @"UPDATE RecentBattles_Session
                                SET
                                      rsUEDateFrom = (SELECT ifnull(min(RecentBattles.rbBattleTime), datetime('now', 'unixepoch'))
                                                            FROM RecentBattles
                                                            WHERE RecentBattles.rbSessionID = RecentBattles_Session.rsKey )
                                    , rsUEDateTo = (SELECT ifnull(Max(RecentBattles.rbBattleTime), datetime('now', 'unixepoch'))
                                                            FROM RecentBattles
                                                            WHERE RecentBattles.rbSessionID = RecentBattles_Session.rsKey )
                                WHERE
                                    EXISTS (
                                        SELECT *
                                        FROM RecentBattles
                                        WHERE RecentBattles.rbSessionID = RecentBattles_Session.rsKey
                                    )";
                        dbHelpers.ExecuteNonQuery(sql);
                        sql = @"UPDATE RecentBattles_Session
                                SET
                                      rsUEDateFrom =  strftime('%s', '2000-01-01 00:00:00')
                                    , rsUEDateTo = strftime( '%s', '2000-01-01 00:00:01')
                                WHERE
                                  rsUEDateFrom is null";
                        dbHelpers.ExecuteNonQuery(sql);
                        dbHelpers.ExecuteNonQuery(sql);
                        sql = @"update RecentBattles_Session
                                set rsUEDateTo =  strftime('%s', '2037-01-01 00:00:00')
                                where rsID = (select max(rsID) from RecentBattles_Session)";
                        dbHelpers.ExecuteNonQuery(sql);
                   
                    }
                }






                Console.WriteLine("Altering DB done");
                
            }
        }

        public static void CheckAndCreateColumn(string sTable, string sColumn, string sColumnDef, IDBHelpers dbHelpers, DataTable dt)
        {
            var columnCheck = (from x in dt.AsEnumerable()
                    where x.Field<string>("name") == sColumn
            select x).DefaultIfEmpty(null).FirstOrDefault();

            if (columnCheck == null)
            {
                string SQL = "Alter table " + sTable + " Add " + sColumn + " " + sColumnDef;
                Console.WriteLine(SQL);
                dbHelpers.ExecuteNonQuery(SQL);
            }
        }
    }

    


}
