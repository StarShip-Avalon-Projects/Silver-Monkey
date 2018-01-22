using Furcadia.Logging;
using Logging;
using MonkeyCore2.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace MonkeyCore2.Data
{
    /// <summary>
    /// Monkey Systems generic interface to System.Data.Sqlite
    /// </summary>
    public class SQLiteDatabase
    {
        #region Public Fields

        public const string PlayerDbName = "Name";

        #endregion Public Fields

        #region Private Fields

        private const string DefaultFile = "SilverMonkey.db";

        private const string FurreTable = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [Access Level] INTEGER, [date added] TEXT, [date modified] TEXT, [PSBackup] DOUBLE";

        private const string SyncPragma = "PRAGMA encoding = \"UTF-16\"; ";

        private string dbConnection;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default Constructor for SQLiteDatabase Class.
        /// </summary>
        public SQLiteDatabase()
        {
            string inputFile = Path.Combine(Paths.SilverMonkeyBotPath, DefaultFile);
            dbConnection = $"Data Source={ inputFile}";
            CreateTbl("FURRE", FurreTable);
            CreateTbl("BACKUPMASTER", "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [{ PlayerDbName }] TEXT Unique, [date modified] TEXT");
            CreateTbl("BACKUP", "[NameID] INTEGER, [Key] TEXT, [Value] TEXT, PRIMARY KEY ([NameID],[Key])");
            CreateTbl("SettingsTableMaster", "[ID] INTEGER UNIQUE, [SettingsTable] TEXT Unique, [date modified] TEXT, PRIMARY KEY ([ID],[SettingsTable])");
            CreateTbl("SettingsTable", "[ID] INTEGER UNIQUE,[SettingsTableID] INTEGER, [Setting] TEXT, [Value] TEXT");
        }

        /// <summary>
        /// Single Param Constructor for specifying the DB file.
        /// </summary>
        /// <param name="inputFile">
        /// The File containing the DB
        /// </param>
        public SQLiteDatabase(string inputFile)
        {
            if (string.IsNullOrEmpty(inputFile))
            {
                inputFile = Path.Combine(Paths.SilverMonkeyBotPath, DefaultFile);
            }

            string dir = Path.GetDirectoryName(inputFile);
            if (string.IsNullOrEmpty(dir))
            {
                inputFile = Path.Combine(Paths.SilverMonkeyBotPath, inputFile);
            }

            dbConnection = $"Data Source={inputFile};";
            CreateTbl("FURRE", FurreTable);
            CreateTbl("BACKUPMASTER", "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [date modified] TEXT");
            CreateTbl("BACKUP", "[NameID] INTEGER, [Key] TEXT, [Value] TEXT, PRIMARY KEY ([NameID],[Key])");
            CreateTbl("SettingsTableMaster", "[ID] INTEGER UNIQUE, [SettingsTable] TEXT Unique, [date modified] TEXT, PRIMARY KEY ([ID],[SettingsTable])");
            CreateTbl("SettingsTable", "[ID] INTEGER UNIQUE,[SettingsID] INTEGER UNIQUE, [Setting] TEXT, [Value] TEXT");
        }

        /// <summary>
        /// Single Param Constructor for specifying advanced connection options.
        /// </summary>
        /// <param name="connectionOpts">
        /// A dictionary containing all desired options and their values
        /// </param>
        public SQLiteDatabase(Dictionary<string, string> connectionOpts)
        {
            var str = new StringBuilder();
            foreach (KeyValuePair<string, string> row in connectionOpts)
            {
                str.Append($"{row.Key}={row.Value}; ");
            }

            dbConnection = str.ToString();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Allows the user to easily clear all data from a specific table.
        /// </summary>
        /// <param name="table">
        /// The name of the table to clear.
        /// </param>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public bool ClearTable(string table)
        {
            return ExecuteNonQuery($"delete from {table};") > -1;
        }

        ///<Summary>
        ///    Create a Table with Titles
        /// </Summary>
        /// <param name="Table"></param><param name="ColumnNames"></param>
        public void CreateTbl(string Table, string ColumnNames)
        {
            using (var SQLconnect = new SQLiteConnection(dbConnection))
            {
                using (var SQLcommand = SQLconnect.CreateCommand())
                {
                    SQLconnect.Open();
                    // SQL query to Create Table
                    //  [Access Level] INTEGER, [date added] TEXT, [date modified] TEXT,
                    SQLcommand.CommandText = $"{SyncPragma} CREATE TABLE IF NOT EXISTS { Table }( { ColumnNames } );";
                    SQLcommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        public int ExecuteNonQuery(string sql)
        {
            int rowsUpdated;
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    try
                    {
                        mycommand.CommandText = sql;
                        rowsUpdated = mycommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        rowsUpdated = -1;
                    }
                }
                cnn.Close();
            }
            return rowsUpdated;
        }

        /// <summary>
        /// Allows the programmer to retrieve single items from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public object ExecuteScalar(string sql)
        {
            object value;
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = sql;
                    value = mycommand.ExecuteScalar();
                }
            }
            return value;
        }

        /// <summary>
        /// Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        public DataTable GetDataTable(string sql)
        {
            var dt = new DataTable();
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = sql;

                    try
                    {
                        var reader = mycommand.ExecuteReader();
                        dt.Load(reader);
                        reader.Close();
                    }
                    catch
                    {
                        dt = null;
                    }
                }
            }
            return dt;
        }

        ///// <summary>
        ///// Allows the programmer to easily insert into the DB
        ///// </summary>
        ///// <param name="tableName">Name of the table.</param>
        ///// <param name="ID">The identifier.</param>
        ///// <param name="data">The data.</param>
        ///// <returns></returns>
        //public bool InsertMultiRow(string tableName, int ID, Dictionary<string, string> data)
        //{
        //    List<string> values = new List<string>();
        //    int i = 0;
        //    try
        //    {
        //        foreach (var val in data)
        //        {
        //            values.Add($" ( '{ID}', '{val.Key}', '{val.Value}' )");
        //        }

        //        if (values.Count > 0)
        //        {
        //            // INSERT INTO 'table' ('column1', 'col2', 'col3') VALUES (1,2,3),  (1, 2, 3), (etc);
        //            string cmd = $"INSERT into '{tableName}' ([NameID], [Key], [Value]) Values {string.Join(";", values.ToArray())}";
        //            i = ExecuteNonQuery(cmd);
        //        }
        //    }
        //    catch
        //    { }

        //    //  i = -1 if there's an SQLte error
        //    return values.Count != 0 && i > -1;
        //}

        /// <summary>
        /// Adds the column to the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnType">Type of the column.</param>
        public void AddColumn(string tableName, string columnName, string columnType)
        {
            if (IsColumnExist(columnName, tableName))
            {
                return;
            }

            ExecuteNonQuery($"ALTER TABLE { tableName } ADD COLUMN { columnName } { columnType };");
        }

        /// <summary>
        /// Allows the programmer to easily delete all data from the DB.
        /// </summary>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public bool ClearDB()
        {
            using (var tables = GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;"))
            {
                foreach (DataRow table in tables.Rows)
                {
                    ClearTable(table["NAME"].ToString());
                }
            }
            return true;
        }

        /// <summary>
        /// Allows the programmer to easily delete rows from the DB.
        /// </summary>
        /// <param name="tableName">
        /// The table from which to delete.
        /// </param>
        /// <param name="where">
        /// The where clause for the delete.
        /// </param>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public bool Delete(string tableName, string where)
        {
            try
            {
                return ExecuteNonQuery($"delete from {tableName} where {where};") > -1;
            }
            catch (Exception ex)
            {
                Furcadia.Logging.Logger.Error<SQLiteDatabase>(ex);
            }

            return false;
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <returns>
        /// </returns>
        public DataSet ExecuteQuery(string sql)
        {
            var rowsUpdated = new DataSet();
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = SyncPragma + sql;
                    using (var a = new SQLiteDataAdapter(mycommand))
                    {
                        try
                        {
                            a.Fill(rowsUpdated);
                        }
                        catch
                        {
                            rowsUpdated = null;
                        }
                    }
                }
            }
            return rowsUpdated;
        }

        /// <summary>
        /// Get a set of values from the specified table
        /// </summary>
        /// <param name="str">
        /// </param>
        /// <returns>
        /// a dictionary of values
        /// </returns>
        public virtual Dictionary<string, object> GetValueFromTable(string str)
        {
            Dictionary<string, object> test3 = null;
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = str;
                    using (var reader = mycommand.ExecuteReader())
                    {
                        int Size = 0;
                        test3 = new Dictionary<string, object>();
                        while (reader.Read())
                        {
                            Size = reader.VisibleFieldCount;
                            for (int i = 0; i <= Size - 1; i++)
                            {
                                test3.Add(reader.GetName(i), reader.GetValue(i).ToString());
                            }
                        }

                        reader.Close();
                    }
                }
            }
            return test3;
        }

        /// <summary>
        /// Allows the programmer to easily insert into the DB
        /// </summary>
        /// <param name="tableName">
        /// The table into which we insert the data.
        /// </param>
        /// <param name="data">
        /// A dictionary containing the column names and data for the insert.
        /// </param>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public virtual int Insert(string tableName, Dictionary<string, string> data)
        {
            int rowCount = 0;
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            foreach (var val in data)
            {
                columns.Add($"[{val.Key}]");
                values.Add($"'{val.Value}'");
            }

            try
            {
                string cmd = $"INSERT OR IGNORE into {tableName}({string.Join(",", columns.ToArray())}) VALUES ({string.Join(",", values.ToArray())})";
                rowCount = ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                rowCount = -1;
                Furcadia.Logging.Logger.Error(ex);
            }

            return rowCount;
        }

        /// <summary>
        /// Does the Column name exist in the specified table
        /// </summary>
        /// <param name="columnName">
        /// </param>
        /// <param name="tableName">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsColumnExist(string columnName, string tableName)
        {
            var columnNames = GetAllColumnName(tableName);
            return columnNames.Contains($"[{columnName}]");
        }

        /// <summary>
        /// Determines whether [is table exists] [the specified table name].
        /// </summary>
        /// <param name="tableName">
        /// Name of the table.
        /// </param>
        /// <returns>
        /// True if ExecuteNonQurey returns one or more tables
        /// </returns>
        public bool IsTableExists(string tableName)
        {
            return ExecuteNonQuery($"SELECT name FROM sqlite_master WHERE name='{ tableName }'") > 0;
        }

        /// <summary>
        /// removes a column of data from the specified table
        /// </summary>
        /// <param name="tableName">
        /// </param>
        /// <param name="columnName">
        /// </param>
        /// <returns>
        /// </returns>
        public int RemoveColumn(string tableName, string columnName)
        {
            var ColumnNames = GetAllColumnName(tableName);
            if (!ColumnNames.Contains($"[{columnName}]"))
            {
                return -1;
            }
            ColumnNames.Remove($"[{columnName}]");
            var columnNames = string.Join(",", ColumnNames.ToArray());
            // columnNames = columnNames.Replace("[", "").Replace("]", "");
            var sql = new StringBuilder();
            //.Replace("[", "").Replace("]", "")
            sql.Append("CREATE TEMPORARY TABLE ");
            sql.Append($"{tableName}backup(");
            sql.Append($"{columnNames }); INSERT INTO ");
            sql.Append($"{tableName }backup SELECT ");
            sql.Append($"{columnNames} FROM ");
            sql.Append($"{tableName }; DROP TABLE ");
            sql.Append($"{tableName }; CREATE TABLE ");
            sql.Append($"{tableName }({columnNames }); INSERT INTO ");
            sql.Append($"{tableName } SELECT ");
            sql.Append($"{columnNames} FROM ");
            sql.Append($"{tableName }backup; DROP TABLE ");
            sql.Append($"{tableName }backup;");
            return ExecuteNonQuery(sql.ToString());
        }

        /// <summary>
        /// Allows the programmer to easily update rows in the DB.
        /// </summary>
        /// <param name="tableName">
        /// The table to update.
        /// </param>
        /// <param name="data">
        /// A dictionary containing Column names and their new values.
        /// </param>
        /// <param name="where">
        /// The where clause for the update statement.
        /// </param>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public bool Update(string tableName, Dictionary<string, string> data, string where)
        {
            var vals = new List<string>();
            if (data == null || data.Count == 0)
            {
                return false;
            }

            foreach (var val in data)
            {
                vals.Add($"'{val.Key}'='{val.Value}'");
            }

            try
            {
                string cmd = $"update {tableName} set {string.Join(",", vals.ToArray())} where { where};";
                return ExecuteNonQuery(cmd) > 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return false;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// gets all table column names in a string
        /// </summary>
        /// <param name="tableName">
        /// </param>
        /// <returns>
        /// </returns>
        private List<string> GetAllColumnName(string tableName)
        {
            string sql = $"{SyncPragma }SELECT * FROM { tableName}";
            var columnNames = new List<string>();
            using (var SQLconnect = new SQLiteConnection(dbConnection))
            {
                SQLconnect.Open();
                using (var SQLcommand = SQLconnect.CreateCommand())
                {
                    SQLcommand.CommandText = sql;
                    using (var sqlDataReader = SQLcommand.ExecuteReader())
                    {
                        for (int i = 0; i <= sqlDataReader.VisibleFieldCount - 1; i++)
                        {
                            columnNames.Add($"[{ sqlDataReader.GetName(i) }]");
                        }

                        sqlDataReader.Close();
                    }
                }
            }
            return columnNames;
        }

        #endregion Private Methods
    }
}