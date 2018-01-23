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
        #region Public Properties

        public string DatabaseFile
        {
            get => inputFile;
        }

        #endregion Public Properties

        #region Private Fields

        private const string DefaultFile = "SilverMonkey.db";
        private const string FurreTable = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [Access Level] INTEGER DEFAULT 0, [date added] DATETIME DEFAULT(datetime('now','localtime')), [date modified] DATETIME DEFAULT(datetime('now','localtime')), [PSBackup] DOUBLE";
        private const string SyncPragma = "PRAGMA encoding = \"UTF-16\"; ";
        private string dbConnection;
        private string inputFile = Path.Combine(Paths.SilverMonkeyBotPath, DefaultFile);

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default Constructor for SQLiteDatabase Class.
        /// </summary>
        public SQLiteDatabase()
        {
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
        public SQLiteDatabase(string databaseFile)
        {
            if (string.IsNullOrWhiteSpace(databaseFile))
            {
                inputFile = Path.Combine(Paths.SilverMonkeyBotPath, DefaultFile);
            }

            string dir = Path.GetDirectoryName(databaseFile);
            if (string.IsNullOrWhiteSpace(dir))
            {
                inputFile = Path.Combine(Paths.SilverMonkeyBotPath, databaseFile);
            }
            else
            {
                inputFile = databaseFile;
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
        /// Adds the column to the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnType">Type of the column.</param>
        public void AddColumn(string tableName, string columnName, string columnType)
        {
            Logger.Debug<SQLiteDatabase>($"Add Collumn {columnName}");
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
            Logger.Debug<SQLiteDatabase>($"ClearDB");
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
            Logger.Debug<SQLiteDatabase>($"Clear Table {table}");
            return ExecuteNonQuery($"delete from {table};") > -1;
        }

        ///<Summary>
        ///    Create a Table with Titles
        /// </Summary>
        /// <param name="Table"></param><param name="ColumnNames"></param>
        public void CreateTbl(string Table, string ColumnNames)
        {
            Logger.Debug<SQLiteDatabase>($"Create Table {ColumnNames}");
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
                    catch (Exception ex)
                    {
                        rowsUpdated = -1;
                        Logger.Error<SQLiteDatabase>(ex);
                    }
                }
                cnn.Close();
            }
            return rowsUpdated;
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
                    mycommand.CommandText = $"{SyncPragma}{sql}";
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
        /// Gets all collumn names with meta data.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetAllCollumnNamesWithMetaData(string table)
        {
            var result = new Dictionary<string, string>();
            var dt = GetDataTable($"PRAGMA Table_Info({table});");
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string nullString = "";
                    string defaultValue = "";
                    if (int.Parse(row["notnull"].ToString()) == 1)
                        nullString = " NOT NULL";
                    if (!string.IsNullOrWhiteSpace(row["dflt_value"].ToString()))
                        defaultValue = $" DEFAULT ({row["dflt_value"].ToString()})";

                    result.Add($"[{row["name"].ToString()}]", $"{row["type"].ToString()}{nullString}{defaultValue}");
                }
            }
            return result;
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
                    mycommand.CommandText = $"{sql}";

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
            Dictionary<string, object> result = null;
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = str;
                    using (var reader = mycommand.ExecuteReader())
                    {
                        int Size = 0;
                        result = new Dictionary<string, object>();
                        while (reader.Read())
                        {
                            Size = reader.VisibleFieldCount;
                            for (int i = 0; i <= Size - 1; i++)
                            {
                                result.Add(reader.GetName(i), reader.GetValue(i).ToString());
                            }
                        }

                        reader.Close();
                    }
                }
            }
            return result;
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
            string PrimaryKeyClause = string.Empty;
            string UniqueKeyClause = string.Empty;
            var Columns = GetAllCollumnNamesWithMetaData(tableName);
            if (!Columns.ContainsKey($"[{columnName}]"))
            {
                return -1;
            }
            Columns.Remove($"[{columnName}]");
            List<string> PrimaryKeys = GetTablePrimaryKeys(tableName);
            List<string> UniqueKeys = GetTableUniqeKeys(tableName);
            PrimaryKeys.Remove($"[{columnName}]");
            UniqueKeys.Remove($"[{columnName}]");
            if (PrimaryKeys != null && PrimaryKeys.Count > 0)
            {
                PrimaryKeyClause = $", PRIMARY KEY ({string.Join(",", PrimaryKeys.ToArray())})";
            }

            if (UniqueKeys != null && UniqueKeys.Count > 0)
            {
                //CONSTRAINT constraint_name UNIQUE (uc_col1, uc_col2, ... uc_col_n)
                UniqueKeyClause = $", CONSTRAINT constraint_{tableName} UNIQUE ({string.Join(",", UniqueKeys.ToArray())})";
            }
            var ColumnNames = new List<string>();
            var ColumnNamesWithOutMetaData = new List<string>();
            foreach (var kvp in Columns)
            {
                ColumnNames.Add($"{kvp.Key} {kvp.Value}");
                ColumnNamesWithOutMetaData.Add($"{kvp.Key}");
            }

            var columnNamesMetaData = string.Join(", ", ColumnNames.ToArray());
            // columnNames = columnNames.Replace("[", "").Replace("]", "");
            var sql = new StringBuilder()
            //.Replace("[", "").Replace("]", "")
            .Append("CREATE TEMPORARY TABLE ")
            .Append($"{tableName}backup(")
            .Append($"{columnNamesMetaData }{PrimaryKeyClause}{UniqueKeyClause}); INSERT INTO ")
            .Append($"{tableName }backup SELECT ")
            .Append($"{string.Join(", ", ColumnNamesWithOutMetaData.ToArray())} FROM ")
            .Append($"{tableName }; DROP TABLE ")
            .Append($"{tableName }; CREATE TABLE ")
            .Append($"{tableName }({columnNamesMetaData }{PrimaryKeyClause}{UniqueKeyClause}); INSERT INTO ")
            .Append($"{tableName } SELECT ")
            .Append($"{string.Join(", ", ColumnNamesWithOutMetaData.ToArray())} FROM ")
            .Append($"{tableName }backup; DROP TABLE ")
            .Append($"{tableName }backup;");
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

        /// <summary>
        /// Gets the table primary keys.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public List<string> GetTablePrimaryKeys(string table)
        {
            var result = new List<string>();
            var dt = GetDataTable($"PRAGMA Table_Info({table});");
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (int.Parse(row["pk"].ToString()) > 0)
                        result.Add($"[{row["name"].ToString()}]");
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the table uniqe keys.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public List<string> GetTableUniqeKeys(string table)
        {
            //PRAGMA index_list(table_name);
            var result = new List<string>();
            var IndexListDataTable = GetDataTable($"PRAGMA INDEX_LIST({table});");
            string test = string.Empty;
            if (IndexListDataTable != null)
            {
                foreach (DataRow row in IndexListDataTable.Rows)
                {
                    if (int.Parse(row["unique"].ToString()) != 0)
                    {
                        var indexXinfoDataTable = GetDataTable($"PRAGMA index_xinfo({row["name"].ToString()});");
                        if (indexXinfoDataTable != null)
                        {
                            foreach (DataRow row2 in indexXinfoDataTable.Rows)
                            {
                                var ColumnName = row2["name"].ToString();
                                if (!string.IsNullOrWhiteSpace(ColumnName))
                                    result.Add($"[{ColumnName}]");
                            }
                        }
                    }
                }
            }

            return result;
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