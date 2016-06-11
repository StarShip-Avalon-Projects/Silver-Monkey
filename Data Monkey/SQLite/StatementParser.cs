using System;
using System.Data;

namespace SQLiteEditor
{
	/// <summary>
	/// Summary description for StatementParser.
	/// </summary>
	public class StatementParser
	{
		public StatementParser()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static void ReturnResults(string SQLStatement, string DatabaseLocation)
		{
			DataSet ds = null;
			ReturnResults(SQLStatement, DatabaseLocation, ref ds);
		}

		public static void ReturnResults(string SQLStatement, string DatabaseLocation, ref DataSet ds)
		{
			//Add a call here to a parser that will 
			//ensure the SQLStatement is properly formed

			SQLiteDatabase db = new SQLiteDatabase(DatabaseLocation);

			if (SQLStatement.ToLower().StartsWith("select") || SQLStatement.ToLower().StartsWith("pragma"))
				ds = db.ExecuteQuery(SQLStatement);
			else
				db.ExecuteNonQuery(SQLStatement);			
			

		}


	}
}
