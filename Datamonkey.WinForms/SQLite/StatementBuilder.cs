using System.Text;

namespace DataMonkey
{
    /// <summary>
    /// Summary description for StatementBuilder.
    /// </summary>
    public class StatementBuilder
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementBuilder"/> class.
        /// </summary>
        public StatementBuilder()
        {
        }

        #endregion Public Constructors

        #region BuildTableOpenSql

        /// <summary>
        /// Builds the table open SQL.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns></returns>
        public static string BuildTableOpenSql(string TableName)
        {
            StringBuilder sb = new StringBuilder();

            //Star could be replaced with a function that pulls out "column" names for
            //better readability
            sb.Append("Select * From ");
            sb.Append(TableName);

            return sb.ToString();
        }

        #endregion BuildTableOpenSql

        #region BuildAddColumnSQL

        /// <summary>
        /// Builds the add column SQL.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="ColumnName">Name of the column.</param>
        /// <param name="ColumnType">Type of the column.</param>
        /// <returns></returns>
        public static string BuildAddColumnSQL(string TableName, string ColumnName, string ColumnType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Alter Table ");
            sb.Append(TableName);
            sb.Append(" Add Column ");
            sb.Append(ColumnName);
            sb.Append(" ");
            sb.Append(ColumnType);

            return sb.ToString();
        }

        #endregion BuildAddColumnSQL

        #region Build Add Table Sql

        /// <summary>
        /// Builds the add table SQL.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns></returns>
        public static string BuildAddTableSQL(string TableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Create Table ");
            sb.Append(TableName);
            sb.Append(" (");
            sb.Append(TableName);
            sb.Append("_ID integer)");

            return sb.ToString();
        }

        #endregion Build Add Table Sql

        #region BuildMasterquery

        /// <summary>
        /// Builds the master query.
        /// </summary>
        /// <returns></returns>
        public static string BuildMasterQuery()
        {
            return "Select name from sqlite_master where type = 'table' order by name";
        }

        #endregion BuildMasterquery

        #region BuildRenameTableSQL

        /// <summary>
        /// Builds the rename table SQL.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="NewTableName">New name of the table.</param>
        /// <returns></returns>
        public static string BuildRenameTableSQL(string TableName, string NewTableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Alter table ");
            sb.Append(TableName);
            sb.Append(" Rename To ");
            sb.Append(NewTableName);

            return sb.ToString();
        }

        #endregion BuildRenameTableSQL

        #region BuildRowDeleteSQL

        /// <summary>
        /// Builds the row delete SQL.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <param name="RowName">Name of the row.</param>
        /// <param name="RowID">The row identifier.</param>
        /// <returns></returns>
        public static string BuildRowDeleteSQL(string TableName, string RowName, string RowID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Delete from ");
            sb.Append(TableName);
            sb.Append(" Where ");
            sb.Append(RowName);
            sb.Append(" = ");
            sb.Append(RowID);

            return sb.ToString();
        }

        #endregion BuildRowDeleteSQL

        #region BuildTableDeleteSQL

        /// <summary>
        /// Builds the table delete SQL.
        /// </summary>
        /// <param name="TableName">Name of the table.</param>
        /// <returns></returns>
        public static string BuildTableDeleteSQL(string TableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Drop Table ");
            sb.Append(TableName);

            return sb.ToString();
        }

        #endregion BuildTableDeleteSQL

        #region BuildIntegrityCheckSQL

        /// <summary>
        /// Builds the integrity check SQL.
        /// </summary>
        /// <returns></returns>
        public static string BuildIntegrityCheckSQL()
        {
            string retval = "PRAGMA integrity_check";
            return retval;
        }

        #endregion BuildIntegrityCheckSQL
    }
}