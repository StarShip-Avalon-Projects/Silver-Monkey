using Furcadia.Logging;
using IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MonkeyCore.Data
{
    /// <summary>
    /// Monkey Systems generic interface to System.Data.Sqlite
    /// </summary>
    public class SQLiteDatabase
    {
        #region Public Properties

        /// <summary>
        /// Gets the database file.
        /// </summary>
        /// <value>
        /// The database file.
        /// </value>
        public string DatabaseFile
        {
            get => inputFile;
        }

        #endregion Public Properties

        #region Private Fields

        private const string DefaultFile = "SilverMonkey.db";
        private const string FurreTable = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [Access Level] INTEGER DEFAULT 0, [date added] DATETIME DEFAULT(datetime('now','localtime')), [date modified] DATETIME DEFAULT(datetime('now','localtime')), [PSBackup] DOUBLE";
        private const string SettingsTableMasterCreateSQL = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT , [SettingsTable] TEXT UNIQUE, [date modified] DATETIME DEFAULT(datetime('now','localtime'))";
        private const string SettingsTableCreateSQL = "[ID] INTEGER UNIQUE,[SettingsTableID] INTEGER, [Setting] TEXT, [Value] TEXT, PRIMARY KEY ([SettingsTableID], [Setting])";
        private const string BackUpMasterCreateSQL = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [date modified] TEXT";
        private const string BackUpCreateSQL = "[NameID] INTEGER, [Key] TEXT, [Value] TEXT, PRIMARY KEY ([NameID],[Key])";
        private const string SyncPragma = "PRAGMA synchronous = 0;";
        private string dbConnection;
        private string inputFile = Path.Combine(Paths.SilverMonkeyBotPath, DefaultFile);

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default Constructor for SQLiteDatabase Class.
        /// </summary>
        public SQLiteDatabase()
        {
            dbConnection = $"Data Source={ inputFile};";

            CreateTbl("FURRE", FurreTable);
            CreateTbl("BACKUPMASTER", BackUpMasterCreateSQL);
            CreateTbl("BACKUP", BackUpCreateSQL);
            CreateTbl("SettingsTableMaster", SettingsTableMasterCreateSQL);
            CreateTbl("SettingsTable", SettingsTableCreateSQL);
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
            dbConnection = $"Data Source={ inputFile};";

            CreateTbl("FURRE", FurreTable);
            CreateTbl("BACKUPMASTER", BackUpMasterCreateSQL);
            CreateTbl("BACKUP", BackUpCreateSQL);
            CreateTbl("SettingsTableMaster", SettingsTableMasterCreateSQL);
            CreateTbl("SettingsTable", SettingsTableCreateSQL);
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
        /// <param name="table">Name of the table.</param>
        /// <param name="collumn">Name of the column.</param>
        /// <param name="type">Type of the column.</param>
        public int AddColumn(string table, string collumn, string type)
        {
            Logger.Debug<SQLiteDatabase>($"'{collumn} {type}' TO: '{table}'");
            if (IsColumnExist(collumn, table))
            {
                return 0;
            }
            try
            {
                return ExecuteNonQuery($"ALTER TABLE { table } ADD COLUMN { collumn } { type };");
            }
            catch
            {
                Logger.Warn($"Failed to create column [{collumn}]. Most likely it already exists, which is fine.");
            }
            return -1;
        }

        /// <summary>
        /// Allows the programmer to easily delete all data FROM the DB.
        /// </summary>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public bool ClearDB()
        {
            Logger.Debug<SQLiteDatabase>($"ClearDB");
            using (var tables = GetDataTable("select NAME FROM SQLITE_MASTER where type='table' order by NAME;"))
            {
                foreach (DataRow table in tables.Rows)
                {
                    ClearTable(table["NAME"].ToString());
                }
            }
            return true;
        }

        /// <summary>
        /// Allows the user to easily clear all data FROM a specific table.
        /// </summary>
        /// <param name="table">
        /// The name of the table to clear.
        /// </param>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public int ClearTable(string table)
        {
            Logger.Debug<SQLiteDatabase>($"Clear Table {table}");
            try
            {
                return ExecuteNonQuery($"delete FROM {table};");
            }
            catch (Exception ex)
            {
                Logger.Error<SQLiteDatabase>(ex);
            }
            return -1;
        }

        ///<Summary>
        ///    Create a Table with Collumn Headers
        /// </Summary>
        /// <param name="table"></param><param name="columns"></param>
        public void CreateTbl(string table, string columns)
        {
            Logger.Debug<SQLiteDatabase>($"Create Table '{table}' with COLUMNS: '{columns}'");
            try
            {
                ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS { table }( { columns })");
            }
            catch (Exception ex)
            {
                Logger.Error<SQLiteDatabase>(ex);
            }
        }

        /// <summary>
        /// Allows the programmer to easily delete rows FROM the DB.
        /// </summary>
        /// <param name="table">
        /// The table FROM which to delete.
        /// </param>
        /// <param name="where">
        /// The where clause for the delete.
        /// </param>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public int Delete(string table, string where)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}' WHERE: '{where}'");
            try
            {
                return ExecuteNonQuery($"DELETE FROM {table} WHERE {where};");
            }
            catch (Exception ex)
            {
                Logger.Error<SQLiteDatabase>(ex);
            }

            return -1;
        }

        /// <summary>
        /// Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        public int ExecuteNonQuery(string sql)
        {
            var Start = new Stopwatch();
            Start.Start();
            Logger.Debug<SQLiteDatabase>($"'{sql}'");
            int rowsUpdated;
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = $"{SyncPragma} {sql}";
                    rowsUpdated = mycommand.ExecuteNonQuery();
                }
            }
            Logger.Debug<SQLiteDatabase>($"{Start.Elapsed}");
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
            var Start = new Stopwatch();
            Start.Start();
            Logger.Debug<SQLiteDatabase>($"'{sql}'");
            var rowsUpdated = new DataSet();
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = $"{SyncPragma} {sql}";
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
            Logger.Debug<SQLiteDatabase>($"{Start.Elapsed}");
            return rowsUpdated;
        }

        /// <summary>
        /// Allows the programmer to retrieve single items FROM the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public object ExecuteScalar(string sql)
        {
            var Start = new Stopwatch();
            Start.Start();
            Logger.Debug<SQLiteDatabase>($"'{sql}'");
            object value;

            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = $"{SyncPragma} {sql}";
                    value = mycommand.ExecuteScalar();
                }
            }
            Logger.Debug<SQLiteDatabase>($"{Start.Elapsed}");
            return value;
        }

        /// <summary>
        /// Gets all collums with meta data.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetAllCollumnNamesWithMetaData(string table)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}'");
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
            Logger.Debug<SQLiteDatabase>($"'{sql}'");
            var dt = new DataTable();
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = $"{SyncPragma} {sql}";

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
        /// Get a set of values FROM the specified table
        /// </summary>
        /// <param name="SQL">
        /// </param>
        /// <returns>
        /// a dictionary of values
        /// </returns>
        public virtual Dictionary<string, object> GetValueFromTable(string sql)
        {
            Logger.Debug<SQLiteDatabase>($"'{sql}'");
            Dictionary<string, object> result = null;
            using (var cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = $"{SyncPragma} {sql}";
                    using (var reader = mycommand.ExecuteReader())
                    {
                        result = new Dictionary<string, object>();
                        while (reader.Read())
                        {
                            int Size = reader.VisibleFieldCount;
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
        /// <param name="table">
        /// The table into which we insert the data.
        /// </param>
        /// <param name="data">
        /// A dictionary containing the column names and data for the insert.
        /// </param>
        /// <returns>
        /// A boolean true or false to signify success or failure.
        /// </returns>
        public virtual int Insert(string table, Dictionary<string, object> data)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}' data: '{data}'");
            if (data == null || data.Count == 0)
            {
                throw new ArgumentOutOfRangeException("No data to process");
            }
            int rowCount = 0;
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            foreach (var val in data)
            {
                columns.Add($"[{val.Key}]");
                values.Add($"'{val.Value.ToString()}'");
            }

            try
            {
                string cmd = $"INSERT OR IGNORE into {table} ({string.Join(", ", columns.ToArray())}) VALUES ({string.Join(", ", values.ToArray())})";
                rowCount = ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                rowCount = -1;
                Logger.Error(ex);
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
        public bool IsColumnExist(string columnName, string table)
        {
            Logger.Debug<SQLiteDatabase>($"Collumn: '{columnName}' in Table '{table}'");
            var columnNames = GetAllColumnName(table);
            return columnNames.Contains($"[{columnName}]");
        }

        /// <summary>
        /// Determines whether [is table exists] [the specified table name].
        /// </summary>
        /// <param name="table">
        /// Name of the table.
        /// </param>
        /// <returns>
        /// True if ExecuteNonQurey returns one or more tables
        /// </returns>
        public bool TableExists(string table)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}'");
            try
            {
                return ExecuteNonQuery($"SELECT name FROM sqlite_master WHERE name='{ table }'") > 0;
            }
            catch (Exception ex)
            {
                Logger.Error<SQLiteDatabase>(ex);
            }
            return false;
        }

        /// <summary>
        /// removes a column of data FROM the specified table
        /// </summary>
        /// <param name="table">
        /// </param>
        /// <param name="columnName">
        /// </param>
        /// <returns>
        /// </returns>
        public int RemoveColumn(string table, string columnName)
        {
            columnName = columnName.Replace("[", string.Empty).Replace("]", string.Empty);
            Logger.Debug<SQLiteDatabase>($"'{columnName}' FROM '{table}'");
            string PrimaryKeyClause = string.Empty;
            string UniqueKeyClause = string.Empty;
            var Columns = GetAllCollumnNamesWithMetaData(table);
            if (!Columns.ContainsKey($"[{columnName}]"))
            {
                return -1;
            }
            Columns.Remove($"[{columnName}]");
            List<string> PrimaryKeys = GetTablePrimaryKeys(table);
            List<string> UniqueKeys = GetTableUniqeKeys(table);
            PrimaryKeys.Remove($"[{columnName}]");
            UniqueKeys.Remove($"[{columnName}]");
            if (PrimaryKeys != null && PrimaryKeys.Count > 0)
            {
                PrimaryKeyClause = $", PRIMARY KEY ({string.Join(",", PrimaryKeys.ToArray())})";
            }

            if (UniqueKeys != null && UniqueKeys.Count > 0)
            {
                //CONSTRAINT constraint_name UNIQUE (uc_col1, uc_col2, ... uc_col_n)
                UniqueKeyClause = $", CONSTRAINT constraint_{table} UNIQUE ({string.Join(",", UniqueKeys.ToArray())})";
            }
            var ColumnNames = new List<string>();
            var ColumnNamesWithOutMetaData = new List<string>();
            foreach (var kvp in Columns)
            {
                ColumnNames.Add($"{kvp.Key} {kvp.Value}");
                ColumnNamesWithOutMetaData.Add($"{kvp.Key}");
            }

            var columnNamesMetaData = string.Join(", ", ColumnNames.ToArray());

            var sql = new StringBuilder()
                  .Append("ALTER TABLE ")
                  .Append($"{table} RENAME TO {table}backup ;")
                  .Append("CREATE TABLE ")
                  .Append($"{table }({columnNamesMetaData }{PrimaryKeyClause}{UniqueKeyClause}); INSERT INTO ")
                  .Append($"{table } SELECT ")
                  .Append($"{string.Join(", ", ColumnNamesWithOutMetaData.ToArray())} FROM ")
                  .Append($"{table }backup; DROP TABLE ")
                  .Append($"{table }backup;");
            try
            {
                return ExecuteNonQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                Logger.Error<SQLiteDatabase>(ex);
            }
            return -1;
        }

        /// <summary>
        /// Allows the programmer to easily update rows in the DB.
        /// </summary>
        /// <param name="table">
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
        public int Update(string table, Dictionary<string, object> data, string where)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}' Data: '{data}' WHERE '{where}'");
            var vals = new List<string>();
            if (data == null || data.Count == 0)
            {
                throw new ArgumentOutOfRangeException("No data to process");
            }

            foreach (var val in data)
            {
                vals.Add($"[{val.Key}] = '{val.Value.ToString()}'");
            }
            string cmd = $"UPDATE {table} SET {string.Join(", ", vals.ToArray())} WHERE { where};";
            try
            {
                return ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return -1;
            }
        }

        /// <summary>
        /// Gets the table primary keys.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public List<string> GetTablePrimaryKeys(string table)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}'");
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
            Logger.Debug<SQLiteDatabase>($"'{table}'");
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
        /// <param name="table">
        /// </param>
        /// <returns>
        /// </returns>
        private List<string> GetAllColumnName(string table)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}'");
            string sql = $"SELECT * FROM { table}";
            var columnNames = new List<string>();
            using (var SQLconnect = new SQLiteConnection(dbConnection))
            {
                SQLconnect.Open();
                using (var SQLcommand = SQLconnect.CreateCommand())
                {
                    SQLcommand.CommandText = $"{SyncPragma} {sql}";
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