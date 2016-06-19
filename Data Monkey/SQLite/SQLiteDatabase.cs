/*
 * Created by SharpDevelop.
 * User: Gerolkae
 * Date: 5/5/2016
 * Time: 12:19 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;

namespace SQLiteEditor
{
	/// <summary>
	/// Description of SQLite3.
	/// </summary>
public class SQLiteDatabase
	{
    String FurreTable = @"[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [Access Level] INTEGER, [date added] TEXT DEFAULT current_timestamp, [date modified] TEXT, [PSBackup] DOUBLE";
        //const String FurreTable = @"[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Setting] TEXT Unique, [Value] TEXT";
		object @lock = new object();
		private static String dbConnection;
		//private static TextBoxWriter writer = null;
		// ''' <summary>
		// '''     Default Constructor for SQLiteDatabase Class.
		// ''' </summary>
		public  SQLiteDatabase()
		{
		    //dbConnection = "Data Source=" & mPath() & "SilverMonkey.s3db";
		}

		/// <summary>
		///     Single Param Constructor for specifying the DB file.
		/// </summary>
		/// <param name="inputFile">The File containing the DB</param>
		public SQLiteDatabase(String inputFile)
		{
			//writer = new TextBoxWriter(Variables.TextBox1);
			dbConnection = String.Format("Data Source={0};", inputFile);
			if (!File.Exists(inputFile)) {
				CreateTbl("FURRE", FurreTable);
                CreateTbl("SETTINGSMASTER", "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [date modified] TEXT");
                CreateTbl("SETTINGS", "[NameID] INTEGER, [Key] TEXT, [Value] TEXT");
			}
		}



		///<Summary>
		///    Create a Table
		/// </Summary>
		/// <param name="Table"></param><param name="Titles"></param>
		public void CreateTbl(string Table)
		{
			using (SQLiteConnection SQLconnect = new SQLiteConnection(dbConnection)) {
				using (SQLiteCommand SQLcommand = SQLconnect.CreateCommand()) {

					SQLconnect.Open();
					//SQL query to Create Table

					SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS " + Table + "(id INTEGER PRIMARY KEY AUTOINCREMENT );";
					SQLcommand.ExecuteNonQuery();
					SQLcommand.Dispose();
				}
				SQLconnect.Close();
				SQLconnect.Dispose();
			}
		}
		///<Summary>
		///    Create a Table with Titles
		/// </Summary>
		/// <param name="Table"></param><param name="Titles"></param>
		public void CreateTbl(string Table, string Titles)
		{
			using (SQLiteConnection SQLconnect = new SQLiteConnection(dbConnection)) {
				using (SQLiteCommand SQLcommand = SQLconnect.CreateCommand()) {

					SQLconnect.Open();
					//SQL query to Create Table

					// [Access Level] INTEGER, [date added] TEXT, [date modified] TEXT, 
					SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS " + Table + "( " + Titles + " );";
					SQLcommand.ExecuteNonQuery();
					SQLcommand.Dispose();
				}
				SQLconnect.Close();
				SQLconnect.Dispose();
			}
		}

		/// <summary>
		///     Single Param Constructor for specifying advanced connection options.
		/// </summary>
		/// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
		public SQLiteDatabase(Dictionary<String, String> connectionOpts)
		{
			String str = "";
			foreach (KeyValuePair<String, String> row in connectionOpts) {
				str += String.Format("{0}={1}; ", row.Key, row.Value);
			}
			str = str.Trim().Substring(0, str.Length - 1);
			dbConnection = str;
		}

		private string getAllColumnName(string tableName)
		{
			string sql = "SELECT * FROM " + tableName;
			ArrayList columnNames = new ArrayList();
			using (SQLiteConnection SQLconnect = new SQLiteConnection(dbConnection)) {
				SQLconnect.Open();

				using (SQLiteCommand SQLcommand = SQLconnect.CreateCommand()) {

					SQLcommand.CommandText = sql;

					try {
						System.Data.SQLite.SQLiteDataReader sqlDataReader = SQLcommand.ExecuteReader();

						for (int i = 0; i <= sqlDataReader.VisibleFieldCount - 1; i++) {
							columnNames.Add("[" + sqlDataReader.GetName(i) + "]");
						}
					} catch (Exception ex) {
						//ErrorLogging log = new ErrorLogging(ex, this);
					}
					SQLcommand.Dispose();
				}
				SQLconnect.Close();
				SQLconnect.Dispose();
			}
			return string.Join(",", columnNames.ToArray());
		}
		public bool isColumnExist(string columnName, string tableName)
		{
			string columnNames = getAllColumnName(tableName);
			return columnNames.Contains(columnName);
		}
		public void removeColumn(string tableName, string columnName)
		{
			string columnNames = getAllColumnName(tableName);
			columnNames = columnNames.Replace(columnName + ", ", "");
			columnNames = columnNames.Replace(", " + columnName, "");
			columnNames = columnNames.Replace(columnName, "");
			ExecuteNonQuery("CREATE TEMPORARY TABLE " + tableName + "backup(" + columnNames + ");");
			ExecuteNonQuery("INSERT INTO " + tableName + "backup SELECT " + columnNames + " FROM " + tableName + ";");
			ExecuteNonQuery("DROP TABLE " + tableName + ";");
			ExecuteNonQuery("CREATE TABLE " + tableName + "(" + columnNames + ");");
			ExecuteNonQuery("INSERT INTO " + tableName + " SELECT " + columnNames + " FROM " + tableName + "backup;");
			ExecuteNonQuery("DROP TABLE " + tableName + "backup;");
		}

		//Add a column is much more easy
		public void addColumn(string tableName, string columnName)
		{
			if (isColumnExist(columnName, tableName) == true)
				return;
			ExecuteNonQuery("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " ;");
		}
		public void addColumn(string tableName, string columnName, string columnType)
		{
			if (isColumnExist(columnName, tableName) == true)
				return;
			ExecuteNonQuery("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + columnType + ";");
		}
		public void addColumn(string tableName, string columnName, string columnType, string DefaultValue)
		{
			if (isColumnExist(columnName, tableName) == true)
				return;
			ExecuteNonQuery("ALTER TABLE( " + tableName + " ADD COLUMN " + columnName + " " + columnType + " DEFAULT" + DefaultValue + ");");
		}
		/// <summary>
		///     Allows the programmer to run a query against the Database.
		/// </summary>
		/// <param name="sql">The SQL to run</param>
		/// <returns>A DataTable containing the result set.</returns>
		public static DataTable GetDataTable(string sql)
		{
			DataTable dt = new DataTable();

			try {
				using (SQLiteConnection cnn = new SQLiteConnection(dbConnection)) {
					cnn.Open();
					SQLiteCommand mycommand = new SQLiteCommand(cnn);
					mycommand.CommandText = sql;
					SQLiteDataReader reader = mycommand.ExecuteReader();
					dt.Load(reader);
					reader.Close();
					cnn.Close();
					cnn.Dispose();
				}
			} catch (Exception e) {
				throw e;
			}
			return dt;
		}

		public Dictionary<string, object> GetValueFromTable(string str)
		{
			//Dim str As String = "SELECT * FROM FURRE WHERE WHERE =" & Name & ";"
			Dictionary<string, object> test3 = new Dictionary<string, object>();
			using (SQLiteConnection cnn = new SQLiteConnection(dbConnection)) {
				cnn.Open();
				SQLiteCommand mycommand = new SQLiteCommand(cnn);
				mycommand.CommandText = str;


				SQLiteDataReader reader = null;
				try {
					reader = mycommand.ExecuteReader();
					int Size = 0;

					while (reader.Read()) {
						Size = reader.VisibleFieldCount;
						for (int i = 0; i <= Size - 1; i++) {
							test3.Add(reader.GetName(i), reader.GetValue(i).ToString());
						}

					}

				} catch (Exception ex) {
					cnn.Close();
					cnn.Dispose();
					throw ex;
					//return null;
					//Console.WriteLine(ex.Message)
				}

				cnn.Close();
				cnn.Dispose();
			}
			return test3;
		}
		
		/// <summary>
		///     Allows the programmer to interact with the database for purposes other than a query.
		/// </summary>
		/// <param name="sql">The SQL to be run.</param>
		/// <returns>An Integer containing the number of rows updated.</returns>
		public DataSet ExecuteQuery(string sql)
		{
			DataSet ds = new DataSet();
			using (SQLiteConnection cnn = new SQLiteConnection(dbConnection)) {
				cnn.Open();
//"PRAGMA synchronous=0; " +
				using (SQLiteCommand mycommand = new SQLiteCommand(cnn)) {
					mycommand.CommandText = sql;
					DataTable table = new DataTable();
        			table.Load(mycommand.ExecuteReader());
        			ds.Tables.Add(table);
				}

				cnn.Close();
				cnn.Dispose();
			}
			return ds;
		}

		/// <summary>
		///     Allows the programmer to interact with the database for purposes other than a query.
		/// </summary>
		/// <param name="sql">The SQL to be run.</param>
		/// <returns>An Integer containing the number of rows updated.</returns>
		public int ExecuteNonQuery(string sql)
		{
			int rowsUpdated = 0;
			using (SQLiteConnection cnn = new SQLiteConnection(dbConnection)) {
				cnn.Open();

				using (SQLiteCommand mycommand = new SQLiteCommand(cnn)) {
					mycommand.CommandText = "PRAGMA synchronous=0;" +sql;
					rowsUpdated = mycommand.ExecuteNonQuery();
				}

				cnn.Close();
				cnn.Dispose();
			}
			return rowsUpdated;
		}

		///<summary>
		/// 
		/// </summary>
		/// <param name="tableName">
		/// 
		/// </param>
		/// <returns>
		/// 
		/// </returns>
		public Boolean isTableExists(String tableName)
		{
			return ExecuteNonQuery("SELECT name FROM sqlite_master WHERE name='" + tableName + "'") > 0;
		}
		/// <summary>
		///     Allows the programmer to retrieve single items from the DB.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <returns>A string.</returns>
		public static string ExecuteScalar1(string sql)
		{
			object Value = null;
			using (SQLiteConnection cnn = new SQLiteConnection(dbConnection)) {
				cnn.Open();
				using (SQLiteCommand cmd = cnn.CreateCommand()) {
					cmd.CommandText = "PRAGMA synchronous=0;";
					cmd.ExecuteNonQuery();
				}

				using (SQLiteCommand mycommand = new SQLiteCommand(cnn)) {
					mycommand.CommandText = sql;

					try {
						Value = mycommand.ExecuteScalar();
					} catch (Exception Ex) {
						// Throw New Exception(Ex.Message)
						cnn.Close();
						return "";
					} finally {
						//cnn.Close()
					}
				}

				cnn.Close();
				cnn.Dispose();
			}
			if (Value != null) {
				return Value.ToString();
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
		public bool Update(String tableName, Dictionary<String, String> data, String @where)
		{
			String vals = "";
			Boolean returnCode = true;
			if (data.Count >= 1) {
				foreach (KeyValuePair<String, String> val in data) {
					vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
				}
				vals = vals.Substring(0, vals.Length - 1);
			}
			try {
				string cmd = String.Format("update {0} set {1} where {2};", tableName, vals, @where);
				ExecuteNonQuery(cmd);

			} catch {
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
		public bool Delete(String tableName, String @where)
		{
			Boolean returnCode = true;
			try {
				ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, @where));
			} catch (Exception fail) {
				MessageBox.Show(fail.Message);
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
			ArrayList columns = new ArrayList();
			ArrayList values = new ArrayList();
			foreach (KeyValuePair<String, String> val in data) {
				columns.Add(String.Format(" {0}", val.Key.ToString()));
				values.Add(String.Format(" '{0}'", val.Value));
			}
			try {
				string cmd = String.Format("INSERT OR IGNORE into {0}({1}) values({2});", tableName, string.Join(", ", columns.ToArray()), string.Join(", ", values.ToArray()));
				ExecuteNonQuery(cmd);
				return true;
			} catch (Exception fail) {
				MessageBox.Show(fail.Message);
				return false;
			}
			//return true;
		}




		/// <summary>
		///     Allows the programmer to easily delete all data from the DB.
		/// </summary>
		/// <returns>A boolean true or false to signify success or failure.</returns>
		public bool ClearDB()
		{
			DataTable tables = null;
			try {
				tables = GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
				foreach (DataRow table in tables.Rows) {
					this.ClearTable(table["NAME"].ToString());
				}
				return true;
			} catch {
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
			try {
				ExecuteNonQuery(String.Format("delete from {0};", table));
				return true;
			} catch {
				return false;
			}
		}


	}
}
