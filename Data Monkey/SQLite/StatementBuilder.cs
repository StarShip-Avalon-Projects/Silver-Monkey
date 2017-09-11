using System.Text;

namespace DataMonkey
{
    /// <summary>
    /// Summary description for StatementBuilder.
    /// </summary>
    public class StatementBuilder
    {
        #region Public Constructors

        public StatementBuilder()
        {
        }

        #endregion Public Constructors

        #region BuildTableOpenSql

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

        public static string BuildMasterQuery()
        {
            return "Select name from sqlite_master where type = 'table' order by name";
        }

        #endregion BuildMasterquery

        #region BuildRenameTableSQL

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

        public static string BuildTableDeleteSQL(string TableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Drop Table ");
            sb.Append(TableName);

            return sb.ToString();
        }

        #endregion BuildTableDeleteSQL

        #region BuildIntegrityCheckSQL

        public static string BuildIntegrityCheckSQL()
        {
            string retval = "PRAGMA integrity_check";
            return retval;
        }

        #endregion BuildIntegrityCheckSQL
    }
}