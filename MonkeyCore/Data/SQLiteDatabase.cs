#region Usings

using IO;
using MonkeyCore.Logging;
using Monkeyspeak.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Text;

#endregion Usings

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
        public string DatabaseFile => inputFile;

        #endregion Public Properties

        #region Private Fields

        private const string DefaultFile = "SilverMonkey.db";
        private const string FurreTable = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [Access Level] INTEGER DEFAULT 0, [date added] DATETIME DEFAULT(datetime('now','localtime')), [date modified] DATETIME DEFAULT(datetime('now','localtime')), [PSBackup] DOUBLE";
        private const string SettingsTableMasterCreateSQL = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT , [SettingsTable] TEXT UNIQUE, [date modified] DATETIME DEFAULT(datetime('now','localtime'))";
        private const string SettingsTableCreateSQL = "[SettingsTableID] INTEGER, [Setting] TEXT, [Value] TEXT, PRIMARY KEY ([SettingsTableID], [Setting])";
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
            CreateTbl("SettingsTableMaster", SettingsTableMasterCreateSQL);
            CreateTbl("SettingsTable", SettingsTableCreateSQL);
        }

        /// <summary>
        /// Single Param Constructor for specifying advanced cnnection options.
        /// </summary>
        /// <param name="connectionOpts">
        /// A dictionary containing all desired options and their values
        /// </param>
        public SQLiteDatabase(Dictionary<string, string> cnnectionOpts)
        {
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> row in cnnectionOpts)
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
            if (ColumnExist(collumn, table))
            {
                return 0;
            }

            return ExecuteNonQuery($"ALTER TABLE { table } ADD COLUMN { collumn } { type };");
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
            using (DataTable tables = GetDataTable("select NAME FROM SQLITE_MASTER where type='table';")) // order by NAME
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

            return ExecuteNonQuery($"delete FROM {table};");
        }

        ///<Summary>
        ///    Create a Table with Collumn Headers
        /// </Summary>
        /// <param name="table"></param><param name="columns"></param>
        public void CreateTbl(string table, string columns)
        {
            Logger.Debug<SQLiteDatabase>($"Create Table '{table}' with COLUMNS: '{columns}'");

            ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS { table }( { columns })");
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

            return ExecuteNonQuery($"DELETE FROM {table} WHERE {where};");
        }

        /// <summary>
        /// Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        public int ExecuteNonQuery(string sql)
        {
            Logger.Debug<SQLiteDatabase>($"'{sql}'");
            int rowsUpdated;
            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(cnn))
                {
                    cmd.CommandText = $"{SyncPragma} {sql}";
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
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
            var Start = new Stopwatch();
            Start.Start();
            Logger.Debug<SQLiteDatabase>($"'{sql}'");
            DataSet rowsUpdated = new DataSet();
            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(cnn))
                {
                    cmd.CommandText = $"{SyncPragma} {sql}";
                    using (SQLiteDataAdapter a = new SQLiteDataAdapter(cmd))
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
            Logger.Debug<SQLiteDatabase>($"Elapsed time: '{Start.Elapsed}'");
            return rowsUpdated;
        }

        /// <summary>
        /// Allows the programmer to retrieve single items FROM the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public object ExecuteScalar(string sql)
        {
            Logger.Debug<SQLiteDatabase>($"'{sql}'");
            object value;

            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(cnn))
                {
                    cmd.CommandText = $"{SyncPragma} {sql}";
                    value = cmd.ExecuteScalar();
                }
            }
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
            Dictionary<string, string> result = new Dictionary<string, string>();
            DataTable dt = GetDataTable($"PRAGMA Table_Info({table});");
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string nullString = "";
                    if (int.Parse(row["notnull"].ToString()) == 1)
                        nullString = " NOT NULL";
                    if (!string.IsNullOrWhiteSpace(row["dflt_value"].AsString()))
                        nullString += $" DEFAULT ({row["dflt_value"]})";
                    result.Add($"[{row["name"]}]", $"{row["type"]}{nullString}");
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
            DataTable dt = new DataTable();
            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(cnn))
                {
                    cmd.CommandText = $"{SyncPragma} {sql}";

                    SQLiteDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
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
            Dictionary<string, object> result = new Dictionary<string, object>();

            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(cnn))
                {
                    cmd.CommandText = $"{SyncPragma} {sql}";
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DataTable schemaTable = reader.GetSchemaTable();
                            DataTable data = new DataTable();
                            foreach (DataRow row in schemaTable.Rows)
                            {
                                string colName = row.Field<string>("ColumnName");
                                Type t = row.Field<Type>("DataType");
                                data.Columns.Add(colName, t);
                            }
                            while (reader.Read())
                            {
                                var newRow = data.Rows.Add();
                                foreach (DataColumn col in data.Columns)
                                {
                                    newRow[col.ColumnName] = reader[col.ColumnName];
                                }
                            }
                            foreach (DataRow row in data.Rows)
                                result.Add(row[0].AsString(), row[1]);
                        }
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
            foreach (KeyValuePair<string, object> val in data)
            {
                columns.Add($"[{val.Key}]");
                values.Add($"'{val.Value}'");
            }

            string cmd = $"INSERT OR IGNORE into {table} ({string.Join(", ", columns.ToArray())}) VALUES ({string.Join(", ", values.ToArray())})";
            rowCount = ExecuteNonQuery(cmd);

            return rowCount;
        }

        /// <summary>
        /// Inserts or replace multi row.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="data">The data.</param>
        /// <param name="SettingsTableID">The settings table identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">No data to process</exception>
        public virtual int InsertOrReplaceMultiRow(string table, Dictionary<string, object> data, string SettingsTableID)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}' data: '{data}'");
            if (data == null || data.Count == 0)
            {
                throw new ArgumentOutOfRangeException("data", data, "No data to process");
            }
            int rowCount = 0;
            List<string> DataRow = new List<string>();

            // SQL Table Key Value
            foreach (KeyValuePair<string, object> Table_kvp in data)
            {
                DataRow.Add($"('{SettingsTableID}', '{Table_kvp.Key}','{Table_kvp.Value}')");
            }

            // INSERT OR IGNORE INTO my_table(SettingsTableID, Key, Value) VALUES('Karen', 34, ' '),(' ',' ',' ')
            // UPDATE my_table SET age = 34 WHERE name = 'Karen'

            var SQL = new StringBuilder()
                .Append($"INSERT OR REPLACE INTO {table}( SettingsTableID, Setting, Value)")
                .Append("VALUES")
                .Append(string.Join(", ", DataRow.ToArray()));
            rowCount = ExecuteNonQuery(SQL.ToString());

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
        public bool ColumnExist(string columnName, string table)
        {
            bool returnval = false;
            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                SQLiteCommand cmd = cnn.CreateCommand();
                cmd.CommandText = $"{SyncPragma}PRAGMA table_info({table})";

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    int nameIndex = reader.GetOrdinal("Name");

                    while (reader.Read())
                    {
                        if (reader.GetString(nameIndex).Equals(columnName))
                        {
                            returnval = true;
                            break;
                        }
                    }
                }
            }
            return returnval;
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

            return ExecuteNonQuery($"SELECT name FROM sqlite_master WHERE name='{ table }'") > 0;
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
            columnName = columnName.Replace("[", "").Replace("]", "");
            Logger.Debug<SQLiteDatabase>($"'{columnName}' FROM '{table}'");
            string KeyClause = "";
            Dictionary<string, string> Columns = GetAllCollumnNamesWithMetaData(table);
            if (!Columns.ContainsKey($"[{columnName}]"))
            {
                return -1;
            }
            Columns.Remove($"[{columnName}]");
            List<string> PrimaryKeys = GetTablePrimaryKeys(table);
            List<string> UniqueKeys = GetTableUniqueKeys(table);
            PrimaryKeys.Remove($"[{columnName}]");
            UniqueKeys.Remove($"[{columnName}]");
            if (PrimaryKeys != null && PrimaryKeys.Count > 0)
            {
                KeyClause = $", PRIMARY KEY ({string.Join(",", PrimaryKeys.ToArray())})";
            }

            if (UniqueKeys != null && UniqueKeys.Count > 0)
            {
                //CONSTRAINT constraint_name UNIQUE (uc_col1, uc_col2, ... uc_col_n)
                KeyClause += $", CONSTRAINT constraint_{table} UNIQUE ({string.Join(",", UniqueKeys.ToArray())})";
            }
            List<string> ColumnNames = new List<string>();
            List<string> ColumnNamesWithOutMetaData = new List<string>();
            foreach (KeyValuePair<string, string> kvp in Columns)
            {
                ColumnNames.Add($"{kvp.Key} {kvp.Value}");
                ColumnNamesWithOutMetaData.Add($"{kvp.Key}");
            }

            string columnNamesMetaData = string.Join(", ", ColumnNames.ToArray());

            StringBuilder sql = new StringBuilder()
                    .Append("ALTER TABLE ")
                    .Append($"{table} RENAME TO {table}backup ;")
                    .Append("CREATE TABLE ")
                    .Append($"{table }({columnNamesMetaData }{KeyClause}); INSERT INTO ")
                    .Append($"{table } SELECT ")
                    .Append($"{string.Join(", ", ColumnNamesWithOutMetaData.ToArray())} FROM ")
                    .Append($"{table }backup; DROP TABLE ")
                    .Append($"{table }backup;");

            return ExecuteNonQuery(sql.ToString());
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

            if (data == null || data.Count == 0)
            {
                throw new ArgumentOutOfRangeException("No data to process");
            }
            List<string> vals = new List<string>();
            foreach (var val in data)
            {
                vals.Add($"[{val.Key}] = '{val.Value.ToString()}'");
            }
            string cmd = $"UPDATE {table} SET {string.Join(", ", vals.ToArray())} WHERE { where};";

            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Gets the table primary keys.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public List<string> GetTablePrimaryKeys(string table)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}'");
            List<string> result = new List<string>();
            DataTable dt = GetDataTable($"PRAGMA Table_Info({table});");

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
        /// Gets the table unique keys.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public List<string> GetTableUniqueKeys(string table)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}'");
            List<string> result = new List<string>();

            DataTable IndexListDataTable = GetDataTable($"PRAGMA INDEX_LIST({table});");
            if (IndexListDataTable != null)
            {
                foreach (DataRow row in IndexListDataTable.Rows)
                {
                    if (int.Parse(row["unique"].ToString()) != 0)
                    {
                        DataTable indexXinfoDataTable = GetDataTable($"PRAGMA index_xinfo({row["name"]});");
                        if (indexXinfoDataTable != null)
                        {
                            foreach (DataRow row2 in indexXinfoDataTable.Rows)
                            {
                                string ColumnName = row2["name"].ToString();
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
        private List<string> GetAllColumnNames(string table)
        {
            Logger.Debug<SQLiteDatabase>($"'{table}'");
            //PRAGMA table_info(table_name);
            string sql = $"SELECT * FROM { table}";
            List<string> columnNames = new List<string>();

            using (SQLiteConnection cnn = new SQLiteConnection(dbConnection))
            {
                cnn.Open();
                using (SQLiteCommand SQLcommand = cnn.CreateCommand())
                {
                    SQLcommand.CommandText = $"{SyncPragma} {sql}";
                    using (SQLiteDataReader sqlDataReader = SQLcommand.ExecuteReader())
                    {
                        for (int i = 0; i <= sqlDataReader.VisibleFieldCount - 1; i++)
                        {
                            columnNames.Add($"[{ sqlDataReader.GetName(i) }]");
                        }
                    }
                }
            }

            return columnNames;
        }

        #endregion Private Methods
    }
}