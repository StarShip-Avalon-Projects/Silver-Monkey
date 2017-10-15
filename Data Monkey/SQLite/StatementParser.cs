using MonkeyCore;
using System.Data;

namespace DataMonkey
{
    /// <summary>
    /// Summary description for StatementParser.
    /// </summary>
    public static class StatementParser
    {
        #region Public Methods

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementParser.ReturnResults(string, string, out string)'

        public static bool ReturnResults(string SQLStatement, string DatabaseLocation, out string message)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementParser.ReturnResults(string, string, out string)'
        {
            DataSet ds = null;

            return ReturnResults(SQLStatement, DatabaseLocation, ref ds, out message);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementParser.ReturnResults(string, string, ref DataSet, out string)'

        public static bool ReturnResults(string SQLStatement, string DatabaseLocation, ref DataSet ds, out string message)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementParser.ReturnResults(string, string, ref DataSet, out string)'
        {
            //Add a call here to a parser that will
            //ensure the SQLStatement is properly formed

            if (SQLStatement.ToLower().StartsWith("select") || SQLStatement.ToLower().StartsWith("pragma"))
            {
                SQLiteDatabase db = new SQLiteDatabase(DatabaseLocation);
                ds = db.ExecuteQuery(SQLStatement);
                message = string.Format(ds != null ? "ExecuteQuery: ok" : "ExecuteQuery Failed");
                return ds == null;
            }
            else
            {
                int result = SQLiteDatabase.ExecuteNonQuery(SQLStatement);
                ds = null;
                message = string.Format("ExecuteNonQuery: Records Modified {0}", result);
                return result > -1;
            }
        }

        #endregion Public Methods
    }
}