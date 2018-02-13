using System.Text;

namespace DataMonkey
{
    /// <summary>
    /// Summary description for StatementBuilder.
    /// </summary>
    public class StatementBuilder
    {
        #region Public Constructors

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.StatementBuilder()'

        public StatementBuilder()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.StatementBuilder()'
        {
        }

        #endregion Public Constructors

        #region BuildTableOpenSql

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildTableOpenSql(string)'

        public static string BuildTableOpenSql(string TableName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildTableOpenSql(string)'
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildAddColumnSQL(string, string, string)'

        public static string BuildAddColumnSQL(string TableName, string ColumnName, string ColumnType)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildAddColumnSQL(string, string, string)'
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildAddTableSQL(string)'

        public static string BuildAddTableSQL(string TableName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildAddTableSQL(string)'
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildMasterQuery()'

        public static string BuildMasterQuery()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildMasterQuery()'
        {
            return "Select name from sqlite_master where type = 'table' order by name";
        }

        #endregion BuildMasterquery

        #region BuildRenameTableSQL

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildRenameTableSQL(string, string)'

        public static string BuildRenameTableSQL(string TableName, string NewTableName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildRenameTableSQL(string, string)'
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildRowDeleteSQL(string, string, string)'

        public static string BuildRowDeleteSQL(string TableName, string RowName, string RowID)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildRowDeleteSQL(string, string, string)'
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildTableDeleteSQL(string)'

        public static string BuildTableDeleteSQL(string TableName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildTableDeleteSQL(string)'
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Drop Table ");
            sb.Append(TableName);

            return sb.ToString();
        }

        #endregion BuildTableDeleteSQL

        #region BuildIntegrityCheckSQL

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildIntegrityCheckSQL()'

        public static string BuildIntegrityCheckSQL()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StatementBuilder.BuildIntegrityCheckSQL()'
        {
            string retval = "PRAGMA integrity_check";
            return retval;
        }

        #endregion BuildIntegrityCheckSQL
    }
}