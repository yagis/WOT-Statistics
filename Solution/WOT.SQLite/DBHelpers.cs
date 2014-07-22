using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace WOTStatistics.SQLite
{
    public interface IDBHelpers : IDisposable
    {
        /// <summary>
        /// Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        DataTable GetDataTable(string sql);
        /// <summary>
        /// Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        int ExecuteNonQuery(string sql);
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
        void ShutDown();
        void BeginTransaction();
        void EndTransaction();
    }

    public sealed class DBHelpers : IDBHelpers, IDisposable
    {
        String dbConnection;
        SQLiteConnection _connection;
        bool _singleConnection;

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
        public DBHelpers(String inputFile, bool singleConnection = false)
        {
            dbConnection = String.Format("Data Source={0}; Version=3; ", inputFile);
            _singleConnection = singleConnection;
            if (singleConnection)
            {
                _connection = GetConnection();
            }
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

        private SQLiteConnection GetConnection()
        {
            SQLiteConnection cnn = new SQLiteConnection(dbConnection);
            cnn.Open();
            return cnn;
        }
        /// <summary>
        ///     Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        public DataTable GetDataTable(string sql)
        {
            using (DataTable dt = new DataTable())
            {
                try
                {
                    //SQLiteConnection cnn = GetConnection();
                    if (_singleConnection)
                    {
                        if (_connection.State == ConnectionState.Closed)
                            _connection.Open();
                    }
                    else
                        _connection = GetConnection();
                    using (SQLiteCommand mycommand = new SQLiteCommand(_connection) { CommandText = sql })
                    {
                        // SQLiteDataReader reader = mycommand.ExecuteReader();
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(mycommand))
                        {
                            da.Fill(dt);
                        }
                        //dt.Load(reader);
                        //reader.Close();
                    }
                    if (!_singleConnection)
                        _connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error executing SQL: " + e.Message);
                    Console.WriteLine("SQL Command: " + sql);
                    throw new Exception(e.Message);
                }
                return dt;
            }
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
        public int ExecuteNonQuery(string sql)
        {
            //SQLiteConnection cnn = GetConnection();
            if (_singleConnection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
            }
            else
                _connection = GetConnection();

            int rowsUpdated = 0;

            using (SQLiteCommand mycommand = new SQLiteCommand(_connection) { CommandText = sql })
            {

                rowsUpdated = mycommand.ExecuteNonQuery();

            }

            if (!_singleConnection)
                _connection.Close();

            return rowsUpdated;
        }

        public int ExecuteNonQueryWithTransaction(string sql)
        {
            //SQLiteConnection cnn = GetConnection();
            if (_singleConnection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
            }
            else
                _connection = GetConnection();

            int rowsUpdated = 0;
            using (SQLiteTransaction trans = _connection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(sql, _connection, trans))
                {
                    rowsUpdated = mycommand.ExecuteNonQuery();
                }
                trans.Commit();
            }

            if (!_singleConnection)
                _connection.Close();

            return rowsUpdated;
        }

        public void ShutDown()
        {
            using (SQLiteConnection cn = new SQLiteConnection(dbConnection))
            {
                cn.Shutdown();
            }
        }

        public void RemovePassword()
        {
            using (SQLiteConnection cn = new SQLiteConnection(dbConnection))
            {
                cn.Open();
                cn.ChangePassword("");
                cn.Close();
            }
        }


        public void  BeginTransaction()
        {
            if (_singleConnection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
            }
            else
                _connection = GetConnection();

            using (SQLiteCommand mycommand = new SQLiteCommand(_connection) { CommandText = "begin" })
            {
                mycommand.ExecuteNonQuery();
            }

            if (!_singleConnection)
                _connection.Close();

        }

        public void EndTransaction()
        {
            if (_singleConnection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
            }
            else
                _connection = GetConnection();

            using (SQLiteCommand mycommand = new SQLiteCommand(_connection) { CommandText = "end" })
            {
                mycommand.ExecuteNonQuery();
            }

            if (!_singleConnection)
                _connection.Close();

        }

        /// <summary>
        ///     Allows the programmer to retrieve single items from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public string ExecuteScalar(string sql)
        {
           if (_singleConnection)
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
            }
            else
                _connection = GetConnection();

           object value = null;
           using (SQLiteCommand mycommand = new SQLiteCommand(_connection) { CommandText = sql })
            {
                value = mycommand.ExecuteScalar();
            }

           if (!_singleConnection)
               _connection.Close();

           if (value != null)
           {
               return value.ToString();
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
                if (_singleConnection)
                {
                    _connection.Close();
                    _connection.Shutdown();
                    _connection.Dispose();
                }
            }
        }
        ~DBHelpers()
        {
            Dispose(false);
        }
    }

}
