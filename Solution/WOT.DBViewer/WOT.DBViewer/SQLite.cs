using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace WOT.DBViewer
{
        public interface IDBHelpers : IDisposable
        {
            /// <summary>
            /// Allows the programmer to run a query against the Database.
            /// </summary>
            /// <param name="sql">The SQL to run</param>
            /// <returns>A DataTable containing the result set.</returns>
            DataTable GetDataTable(string sql, bool nopwd = false);
            /// <summary>
            /// Allows the programmer to interact with the database for purposes other than a query.
            /// </summary>
            /// <param name="sql">The SQL to be run.</param>
            /// <returns>An Integer containing the number of rows updated.</returns>
            int ExecuteNonQuery(string sql, bool noPWD = false);
            /// <summary>
            /// Allows the programmer to retrieve single items from the DB.
            /// </summary>
            /// <param name="sql">The query to run.</param>
            /// <returns>A string.</returns>
            string ExecuteScalar(string sql);
            /// <summary>
            /// Allows the programmer to easily update rows in the DB.
            /// </summary>
            /// <param name="tableName">The table to update.</param>
            /// <param name="data">A dictionary containing Column names and their new values.</param>
            /// <param name="where">The where clause for the update statement.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            bool Update(String tableName, Dictionary<String, String> data, String where);
            /// <summary>
            /// Allows the programmer to easily delete rows from the DB.
            /// </summary>
            /// <param name="tableName">The table from which to delete.</param>
            /// <param name="where">The where clause for the delete.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            bool Delete(String tableName, String where);
            /// <summary>
            /// Allows the programmer to easily insert into the DB
            /// </summary>
            /// <param name="tableName">The table into which we insert the data.</param>
            /// <param name="data">A dictionary containing the column names and data for the insert.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            bool Insert(String tableName, Dictionary<String, String> data);
            /// <summary>
            /// Allows the programmer to easily delete all data from the DB.
            /// </summary>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            bool ClearDB();
            /// <summary>
            /// Allows the user to easily clear all data from a specific table.
            /// </summary>
            /// <param name="table">The name of the table to clear.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            bool ClearTable(String table);
            bool CreateDatabase(string fileName);
            void RemovePassword();
        }

        public sealed class DBHelpers : IDBHelpers, IDisposable
        {
            String dbConnection;
            String dbConnectionNoPWD;

            /// <summary>
            ///     Default Constructor for SQLiteDatabase Class.
            /// </summary>
            public DBHelpers()
            {
                dbConnection = "Data Source=";
            }

            /// <summary>
            ///     Single Param Constructor for specifying the DB file.
            /// </summary>
            /// <param name="inputFile">The File containing the DB</param>
            public DBHelpers(String inputFile)
            {
                dbConnection = String.Format("Data Source={0}; Version=3; Password=palmboom;", inputFile);
                dbConnectionNoPWD = String.Format("Data Source={0}; Version=3;", inputFile);

            }

            /// <summary>
            ///     Single Param Constructor for specifying advanced connection options.
            /// </summary>
            /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
            public DBHelpers(Dictionary<String, String> connectionOpts)
            {
                String str = "";
                foreach (KeyValuePair<String, String> row in connectionOpts)
                {
                    str += String.Format("{0}={1}; ", row.Key, row.Value);
                }
                str = str.Trim().Substring(0, str.Length - 1);
                dbConnection = str;
            }

            public SQLiteConnection GetConnection(bool noPassword = false)
            {
                if (!noPassword)
                { 
                    SQLiteConnection cnn = new SQLiteConnection(dbConnection);           
                    cnn.Open();
                    return cnn;
                }
                else
                {
                    SQLiteConnection cnn = new SQLiteConnection(dbConnectionNoPWD);
                    cnn.Open();
                    return cnn;
                }
            }
            /// <summary>
            ///     Allows the programmer to run a query against the Database.
            /// </summary>
            /// <param name="sql">The SQL to run</param>
            /// <returns>A DataTable containing the result set.</returns>
            public DataTable GetDataTable(string sql, bool noPWD = false)
            {
                DataTable dt = new DataTable();
                try
                {
                    SQLiteConnection cnn = GetConnection(noPWD);
                    SQLiteCommand mycommand = new SQLiteCommand(cnn);
                    mycommand.CommandText = sql;
                    SQLiteDataReader reader = mycommand.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    cnn.Close();
                    GC.Collect();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return dt;
            }


            public bool CreateDatabase(string fileName)
            {
                try
                {
                    SQLiteConnection.CreateFile(fileName);
                    return true;
                }
                catch
                {
                    return false;
                }

            }

            /// <summary>
            ///     Allows the programmer to interact with the database for purposes other than a query.
            /// </summary>
            /// <param name="sql">The SQL to be run.</param>
            /// <returns>An Integer containing the number of rows updated.</returns>
            public int ExecuteNonQuery(string sql, bool noPWD = false)
            {
                SQLiteConnection cnn = GetConnection(noPWD);
                using (SQLiteCommand mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = sql;
                    int rowsUpdated = mycommand.ExecuteNonQuery();
                    cnn.Close();
                    GC.Collect();
                    return rowsUpdated;
                }
            }


            public void RemovePassword()
            {
                using (SQLiteConnection cn = new SQLiteConnection(dbConnection))
                {
                    cn.Open();
                    cn.ChangePassword("");
                    cn.Close();
                    GC.Collect();
                }
            }
            /// <summary>
            ///     Allows the programmer to retrieve single items from the DB.
            /// </summary>
            /// <param name="sql">The query to run.</param>
            /// <returns>A string.</returns>
            public string ExecuteScalar(string sql)
            {
                SQLiteConnection cnn = GetConnection();
                using (SQLiteCommand mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = sql;
                    object value = mycommand.ExecuteScalar();
                    cnn.Close();
                    GC.Collect();
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }
                return "";
            }

            /// <summary>
            ///     Allows the programmer to easily update rows in the DB.
            /// </summary>
            /// <param name="tableName">The table to update.</param>
            /// <param name="data">A dictionary containing Column names and their new values.</param>
            /// <param name="where">The where clause for the update statement.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool Update(String tableName, Dictionary<String, String> data, String where)
            {
                String vals = "";
                Boolean returnCode = true;
                if (data.Count >= 1)
                {
                    foreach (KeyValuePair<String, String> val in data)
                    {
                        vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                    }
                    vals = vals.Substring(0, vals.Length - 1);
                }
                try
                {
                    this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
                }
                catch
                {
                    returnCode = false;
                }
                return returnCode;
            }

            /// <summary>
            ///     Allows the programmer to easily delete rows from the DB.
            /// </summary>
            /// <param name="tableName">The table from which to delete.</param>
            /// <param name="where">The where clause for the delete.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool Delete(String tableName, String where)
            {
                Boolean returnCode = true;
                try
                {
                    this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
                }
                catch
                {
                    returnCode = false;
                }
                return returnCode;
            }

            /// <summary>
            ///     Allows the programmer to easily insert into the DB
            /// </summary>
            /// <param name="tableName">The table into which we insert the data.</param>
            /// <param name="data">A dictionary containing the column names and data for the insert.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool Insert(String tableName, Dictionary<String, String> data)
            {
                String columns = "";
                String values = "";
                Boolean returnCode = true;
                foreach (KeyValuePair<String, String> val in data)
                {
                    columns += String.Format(" {0},", val.Key.ToString());
                    values += String.Format(" '{0}',", val.Value);
                }
                columns = columns.Substring(0, columns.Length - 1);
                values = values.Substring(0, values.Length - 1);
                try
                {
                    this.ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
                }
                catch
                {
                    returnCode = false;
                }
                return returnCode;
            }

            /// <summary>
            ///     Allows the programmer to easily delete all data from the DB.
            /// </summary>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool ClearDB()
            {
                DataTable tables;
                try
                {
                    tables = this.GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                    foreach (DataRow table in tables.Rows)
                    {
                        this.ClearTable(table["NAME"].ToString());
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            ///     Allows the user to easily clear all data from a specific table.
            /// </summary>
            /// <param name="table">The name of the table to clear.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool ClearTable(String table)
            {
                try
                {

                    this.ExecuteNonQuery(String.Format("delete from {0};", table));
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                }
            }
            ~DBHelpers()
            {
                Dispose(false);
            }
        }

        public static class CreateDatabase
        {
            public static void Create(string fileName)
            {
                using (IDBHelpers dbHelpers = new DBHelpers(fileName))
                {
                    dbHelpers.CreateDatabase(fileName);
                    string sql = @"SELECT name FROM sqlite_master WHERE type='table' AND name='RecentBattles';";
                    using (DataTable tableCheck = dbHelpers.GetDataTable(sql, true))
                    {
                        if (tableCheck.Rows.Count <= 0)
                        {
                            dbHelpers.ExecuteNonQuery(CreateRecentBattlesTable(), true);
                        }
                    }

                    sql = @"SELECT name FROM sqlite_master WHERE type='table' AND name='RecentBattles_Session';";
                    using (DataTable tableCheck = dbHelpers.GetDataTable(sql, true))
                    {
                        if (tableCheck.Rows.Count <= 0)
                        {
                            dbHelpers.ExecuteNonQuery(CreateRecentBattlesSessionTable(), true);
                        }
                    }
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

            private static string CreateRecentBattlesTable()
            {
                return @"CREATE TABLE [RecentBattles] (
                        [rbID] INTEGER PRIMARY KEY NOT NULL,
                        [rbCountryID] INTEGER default (0)  NOT NULL,
                        [rbTankID] INTEGER default (0)  NOT NULL,
                        [rbOriginalBattleCount] REAL default (0)  NULL,
                        [rbBattles] REAL default (0)  NULL,
                        [rbKills] REAL default (0)  NULL,
                        [rbDamageReceived] REAL default (0)  NULL,
                        [rbDamageDealt] REAL default (0)  NULL,
                        [rbXPReceived] REAL default (0)  NULL,
                        [rbSpotted] REAL default (0)  NULL,
                        [rbCapturePoints] REAL default (0)  NULL,
                        [rbDefencePoints] REAL default (0)  NULL,
                        [rbSurvived] INTEGER default (0)  NOT NULL,
                        [rbVictory] INTEGER default (0)  NULL,
                        [rbBattleTime] INTEGER default (0)  NULL,
                        [rbShot] REAL default (0)  NULL,
                        [rbHits] REAL default (0)  NULL,
                        [rbTier] INTEGER default (0)  NULL,
                        [rbBattlesPerTier] REAL default (0)  NULL,
                        [rbVictoryCount] REAL default (0)  NULL,
                        [rbDefeatCount] REAL default (0)  NULL,
                        [rbDrawCount] REAL  default (0) NULL,
                        [rbSurviveYesCount] REAL default (0)  NULL,
                        [rbSurviveNoCount] REAL default (0)  NULL,
                        [rbFragList] TEXT  NULL,
                        [rbBattleTimeFriendly] text  NULL,
                        [rbGlobalAvgTier] REAL default (0)  NULL,
                        [rbGlobalWinPercentage] REAL default (0)  NULL,
                        [rbSessionID] varchar(50)  NULL,
                        [rbGlobalAvgDefPoints] REAL default (0)  NULL
                        );" + Environment.NewLine +

                        @"CREATE UNIQUE INDEX [IDX_RECENTBATTLES_K1_K2_K3_K28] ON [RecentBattles](
                        [rbID]  ASC,
                        [rbCountryID]  ASC,
                        [rbTankID]  ASC,
                        [rbSessionID]  ASC
                        );";
            }

            private static string CreateRecentBattlesSessionTable()
            {
                return @"CREATE TABLE [RecentBattles_Session] (
                        [rsID] INTEGER  NOT NULL PRIMARY KEY,
                        [rsKey] varchar(50)  NULL,
                        [rsDateFrom] text  NULL,
                        [rsDateTo] Text  NULL,
                        [rsUEDateFrom] real NULL,
                        [rsUEDateTo] real NULL
                        );" + Environment.NewLine +

                        @"CREATE INDEX [IDX_RECENTBATTLES_SESSION_K2_K3_K4] ON [RecentBattles_Session](
                        [rsKey]  ASC,
                        [rsUEDateFrom]  ASC,
                        [rsUEDateTo]  ASC
                        );";
            }

            public static void AlterDB(string fileName)
            {
                using (IDBHelpers dbHelpers = new DBHelpers(fileName))
                {
                    using (DataTable dt = dbHelpers.GetDataTable("pragma table_info(RecentBattles)"))
                    {
                        var rbGlobalAvgDefPoints_check = (from x in dt.AsEnumerable()
                                                          where x.Field<string>("name") == "rbGlobalAvgDefPoints"
                                                          select x).DefaultIfEmpty(null).FirstOrDefault();

                        if (rbGlobalAvgDefPoints_check == null)
                        {
                            string sql = "Alter table RecentBattles Add rbGlobalAvgDefPoints REAL default (0)  NULL";
                            dbHelpers.ExecuteNonQuery(sql);
                        }
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
                }
            }

        }

}
