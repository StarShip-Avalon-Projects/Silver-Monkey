using MonkeyCore.Data;
using System.Data;

namespace DataMonkey
{
    /// <summary>
    /// Summary description for StatementParser.
    /// </summary>
    public static class StatementParser
    {
        #region Public Methods

        /// <summary>
        /// Returns the results.
        /// </summary>
        /// <param name="SQLStatement">The SQL statement.</param>
        /// <param name="DatabaseLocation">The database location.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static bool ReturnResults(string SQLStatement, string DatabaseLocation, out string message)
        {
            DataSet ds = null;

            return ReturnResults(SQLStatement, DatabaseLocation, ref ds, out message);
        }

        /// <summary>
        /// Returns the results.
        /// </summary>
        /// <param name="SQLStatement">The SQL statement.</param>
        /// <param name="DatabaseLocation">The database location.</param>
        /// <param name="ds">The ds.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static bool ReturnResults(string SQLStatement, string DatabaseLocation, ref DataSet ds, out string message)
        {
            //Add a call here to a parser that will
            //ensure the SQLStatement is properly formed
            SQLiteDatabase db = new SQLiteDatabase(DatabaseLocation);
            if (SQLStatement.ToLower().StartsWith("select") || SQLStatement.ToLower().StartsWith("pragma"))
            {
                ds = db.ExecuteQuery(SQLStatement);
                message = string.Format(ds != null ? "ExecuteQuery: ok" : "ExecuteQuery Failed");
                return ds == null;
            }
            else
            {
                int result = db.ExecuteNonQuery(SQLStatement);
                ds = null;
                message = string.Format("ExecuteNonQuery: Records Modified {0}", result);
                return result > -1;
            }
        }

        #endregion Public Methods
    }
}