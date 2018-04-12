#region Usings

using Furcadia.Extensions;
using IO;
using MonkeyCore.Data;
using MonkeyCore.Logging;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Libraries.MsLibHelper;

#endregion Usings

namespace Libraries
{
    /// <summary>
    /// SQLite Database Access... Create tables Store records ect. in Silver Monkey
    /// <para>
    /// To view and edit these tables manually Please look as Data Monkey
    /// </para>
    /// <para>
    /// Conditions (1:500) (1:531)
    /// </para>
    /// <para>
    /// Effects: (5:500) - (5:559)
    /// </para>
    /// <pra> Bot Testers: Be aware this class needs to be tested any way
    /// possible! </pra>
    /// <para>
    /// Default SQLite database file: <see cref="Paths.SilverMonkeyBotPath"/>\SilverMonkey.db
    /// </para>
    /// <para>
    /// NOTE: PhoenixSpeak Database is not SQL based like SQLite. Phoenix
    ///       Speak resembles an XML style system
    /// </para>
    /// </summary>
    public class MsDatabase : MonkeySpeakLibrary
    {
        #region Private Fields

        /// <summary>
        /// These collumns not allowed for deletion or modification, They are core
        /// columns to SM operation
        /// </summary>
        private readonly List<string> SafeFurreTableFields = new List<string>
        {
            "[date added]",
            "[date modified]",
            "[Access Level]",
            "[Name]",
            "[ID]",
            "[PSBackup]"
        };

        private readonly List<string> ReadOnlyFurreTableFields = new List<string>
        {
            "[Access Level]",
            "[Name]",
            "[ID]",
        };

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>
        /// The base identifier.
        /// </value>
        public override int BaseId => 500;

        #endregion Public Properties

        #region Public Methods

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);

            SQLitefile = Paths.CheckBotFolder("SilverMonkey.db");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoEqualToNumberOrVariable(r),
                "and the Database Record {...} about the triggering furre is equal to #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoNotEqualToNumberOrVariable(r),
                "and the Database Record {...} about the triggering furre is not equal to #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoGreaterThanNumber(r),
                "and the Database Record {...} about the triggering furre is greater than #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoLessThanNumber(r),
                "and the Database Record {...} about the triggering furre is less than #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoGreaterThanOrEqualToNumber(r),
                "and the Database Record {...} about the triggering furre is greater than or equal to #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoLessThanOrEqualToNumber(r),
                "and the Database Record {...} about the triggering furre is less than or equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoEqualToNumberOrVariable(r),
                "and the Database Record {...} about the furre named {...} is equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoNotEqualToNumberOrVariable(r),
                "and the Database Record {...} about the furre named {...} is not equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoGreaterThanNumber(r),
                "and the Database Record {...} about the furre named {...} is greater than #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoLessThanNumber(r),
                "and the Database Record {...} about the furre named {...} is less than #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoGreaterThanOrEqualToNumber(r),
                "and the Database Record {...} about the furre named {...} is greater than or equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoLessThanOrEqualToNumber(r),
                "and the Database Record {...} about the furre named {...} is less than or equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoEqualToString(r),
                "and the Database Record {...} about the furre named {...} is equal to string {...},");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoNotEqualToString(r),
                "and the Database Record {...} about the furre named {...} is not equal to string {...},");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoEqualToString(r),
                "and the Database Record {...} about the triggering furre is equal to string {...},");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoNotEqualToString(r),
                "and the Database Record {...} about the triggering furre is not equal to string {...},");

            // TODO: Add Monkey Speak
            // (1:xx) and the Database Record {...} about the triggerig furre exists,
            // (1:xx) and the Database Record{...} about the triggerig furre does not exist,
            // (1:xx) and the Database Record {...} about the furre named {...} exists,
            // (1:xx) and the Database Record {...} about the furre named {...} does not exist,

            Add(TriggerCategory.Effect,
                r => UseOrCreateSQLiteFileIfNotExist(r),
                "use SQLite database file {...} or create file if it does not exist.");

            Add(TriggerCategory.Effect,
                r => InsertTriggeringFurreRecord(r),
                "add the triggering furre with the access level # to the furre records in the database if he, she, or it doesn\t exist.");

            Add(TriggerCategory.Effect,
                r => InsertFurreNamedRecord(r),
                "add furre named {...} with the access level # to the furre records in the database if he, she, or it doesn\t exist.");

            Add(TriggerCategory.Effect,
                r => UpdateTriggeringFurreField(r),
                "memorize Database Record {...} about the triggering furre will now be #.");

            Add(TriggerCategory.Effect,
                r => UpdateFurreNamed_Field(r),
                "memorize Database Record {...} about the furre named {...} will now be #.");

            Add(TriggerCategory.Effect,
                r => UpdateTriggeringFurreFieldSTR(r),
                "memorize Database Record {...} about the triggering furre will now be {...}.");

            Add(TriggerCategory.Effect,
                r => UpdateFurreNamed_FieldSTR(r),
                "memorize Database Record {...} about the furre named {...} will now be {...}.");

            Add(TriggerCategory.Effect,
                r => ReadDatabaseInfoForTheTriggeringFurre(r),
                "remember Database Record {...} about the triggering furre, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => ReadDatabaseInfoName(r),
                "remember Database Record {...} about the furre named {...}, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => GetDateAddedForFurreNamed(r),
                "remember the date added time stamp for the furre named {...}, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => GetDateModifiedForFurreNamed(r),
                "remember the date modified time stamp for the furre named {...}, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => GetDateAddedForTriggeringFurre(r),
                "remember the date added time stamp for the triggering furre and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => GetDateModifiedForTriggeringFurre(r),
                "remember the date modified time stamp for the triggering furre and put it in variable %.");

            Add(TriggerCategory.Effect,
              r => GetMemberList(r),
              "get the member-list from furre records and put them in table %");

            Add(TriggerCategory.Effect,
                r => AddRecordColumn(r),
                "add column {...} with type {...} to the Furre table.");

            Add(TriggerCategory.Effect,
                r => RemoveRecordColumn(r),
                "remove column {...} from the Furre table.");

            Add(TriggerCategory.Effect,
                r => DeleteTriggeringFurreRecord(r),
                "forget all Database Record about the triggering furre.");

            Add(TriggerCategory.Effect,
                r => DeleteFurreNamedRecord(r),
                "forget all Database Records about the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => GetVariableTableFromDatabaseTable(r),
                "remember the database table {...} and put it into table % .");

            Add(TriggerCategory.Effect,
                r => PutVariableTableIntoDatabaseTable(r),
                "memorize table % to database table {...}.");

            Add(TriggerCategory.Effect,
                r => ClearSpecifiedTable(r),
                "clear the database table {...}.");

            Add(TriggerCategory.Effect,
                r => VACUUM(r),
                "execute \"VACUUM\"to rebuild the database and reclaim wasted space.");
        }

        [TriggerDescription("Gets a the list of furres from furre records and put them into %table")]
        [TriggerVariableParameter]
        private bool GetMemberList(TriggerReader reader)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var table = reader.ReadVariableTable(true);
            table.Clear();
            string sql = $"SELECT [Name] FROM FURRE";
            var dataTable = database.GetDataTable(sql);
            int index = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                index++;
                table.Add(index.ToString(), row["Name"]);
            }
            return true;
        }

        /// <summary>
        /// Called when page is disposing or resetting.
        /// </summary>
        /// <param name="page">The page.</param>
        public override void Unload(Page page)
        {
            database = null;
        }

        #endregion Public Methods

        #region Private Methods

        [TriggerDescription("reads column information for the specified furre from Furre Records into the specified variable")]
        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerVariableParameter]
        private bool ReadDatabaseInfoName(TriggerReader reader)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);

            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadVariable(true);
            //  Dim db As SQLiteDatabase = New SQLiteDatabase(file)
            string sql = $"SELECT [{info}] FROM FURRE Where [Name]='{Furre}'";
            Variable.Value = database.ExecuteScalar(sql);
            return true;
        }

        [TriggerDescription("Adds a column to the Furre Table")]
        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool AddRecordColumn(TriggerReader reader)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var colum = reader.ReadString().Replace("[", "").Replace("]", "");
            var type = reader.ReadString();
            return 0 <= database.AddColumn("FURRE", colum, type);
        }

        [TriggerDescription("Clears the specified table")]
        [TriggerStringParameter]
        private bool ClearSpecifiedTable(TriggerReader reader)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var tableName = reader.ReadString().Replace("[", "").Replace("]", "");
            var TableId = database.ExecuteScalar($"SELECT [ID] FROM [SettingsTableMaster] WHERE [SettingsTable] = {tableName}");
            var result = database.ExecuteNonQuery($"DELETE FROM [SettingsTable] Where [SettingsTableID]  =  {TableId};");
            result += database.ExecuteNonQuery($"DELETE FROM [SettingsTableMaster] Where [ID]  =  {TableId};");
            return 0 < result;
        }

        [TriggerDescription("Deletes the specified Furre record from he Furre Table")]
        [TriggerStringParameter]
        private bool DeleteFurreNamedRecord(TriggerReader reader)
        {
            var Furre = reader.ReadString().ToFurcadiaShortName();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            return 0 < database.ExecuteNonQuery($"Delete from FURRE where Name='{Furre }'");
        }

        [TriggerDescription("Deletes the Triggering Furre record from he Furre Table")]
        private bool DeleteTriggeringFurreRecord(TriggerReader reader)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            return 0 < database.ExecuteNonQuery($"Delete from FURRE where Name='{Player.ShortName}'");
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoEqualToNumberOrVariable(TriggerReader reader)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var check = database.ExecuteScalar($"SELECT [{info}] FROM FURRE Where Name = '{Furre}'");
            return check.AsDouble(0d) == Variable;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoEqualToString(TriggerReader reader)
        {
            string info = reader.ReadString();
            string Furre = reader.ReadString().ToFurcadiaShortName();

            if (reader.PeekString())
            {
                string str = reader.ReadString();
                if (database == null) database = new SQLiteDatabase(SQLitefile);
                var result = database.ExecuteScalar($"SELECT [{info}] FROM FURRE Where Name = '{Furre}'");
                return result.ToString() == str;
            }
            return false;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoGreaterThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var check = database.ExecuteScalar($"SELECT [{info}] FROM FURRE Where Name = '{Furre}'");
            return check.AsDouble(0d) > Variable;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoGreaterThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var check = database.ExecuteScalar($"SELECT [{info}] FROM FURRE Where Name = '{Furre}'");

            return check.AsDouble(0d) >= Variable;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoLessThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var check = database.ExecuteScalar($"SELECT [{info}] FROM FURRE Where Name = '{Furre}'");
            return check.AsDouble(0d) < Variable;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoLessThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var check = database.ExecuteScalar($"SELECT [{info}] FROM FURRE Where Name = '{Furre}'");
            return check.AsDouble(0d) <= Variable;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoNotEqualToNumberOrVariable(TriggerReader reader)
        {
            return !FurreNamedRecordInfoEqualToNumberOrVariable(reader);
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoNotEqualToString(TriggerReader reader)
        {
            return !FurreNamedRecordInfoEqualToString(reader);
        }

        [TriggerStringParameter]
        [TriggerVariableParameter]
        private bool GetDateAddedForFurreNamed(TriggerReader reader)
        {
            var furre = reader.ReadString().ToFurcadiaShortName();
            var TimeStamp = reader.ReadVariable(true);

            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var result = database.ExecuteScalar($"SELECT [date modified] FROM FURRE Where Name = '{furre}'");

            TimeStamp.Value = DateTime.Parse(result.ToString());
            return true;
        }

        private bool GetDateAddedForTriggeringFurre(TriggerReader reader)
        {
            var TimeStamp = reader.ReadVariable(true);
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var result = database.ExecuteScalar($"SELECT [date added] FROM FURRE Where Name = '{Player.ShortName}'");
            TimeStamp.Value = DateTime.Parse(result.ToString()).ToString(DateTimeFormat);
            return true;
        }

        private bool GetDateModifiedForFurreNamed(TriggerReader reader)
        {
            var TimeStamp = reader.ReadVariable(true);
            var furre = reader.ReadString().ToFurcadiaShortName();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var result = database.ExecuteScalar($"SELECT [date modified] FROM FURRE Where Name = '{furre}'");

            TimeStamp.Value = DateTime.Parse(result.ToString()).ToString(DateTimeFormat);
            return true;
        }

        private bool GetDateModifiedForTriggeringFurre(TriggerReader reader)
        {
            var TimeStamp = reader.ReadVariable(true);
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var result = database.ExecuteScalar($"SELECT [date modified] FROM FURRE Where Name = '{Player.ShortName}'");

            TimeStamp.Value = DateTime.Parse(result.ToString()).ToString(DateTimeFormat);
            return true;
        }

        private object GetRecordInfoForTheFurreNamed(string Column, string Name)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);

            return database.ExecuteScalar($"SELECT [{Column}] FROM FURRE Where Name = '{Name.ToFurcadiaShortName()}'");
        }

        [TriggerVariableParameter]
        private bool GetVariableTableFromDatabaseTable(TriggerReader reader)
        {
            var DatabaseTable = reader.ReadString();
            var VarTable = reader.ReadVariableTable(true);

            var SQL = new StringBuilder()
                .Append("select SettingsTable.*, SettingsTableMaster.ID from SettingsTable )")
                .Append("inner join SettingsTableMaster on ")
                .Append("SettingsTableMaster.")
                // .Append($"{Entry}= SettingsTable.[SettingsTableID] ")
                .Append($"where SettingsTableMaster.Setting = '{VarTable}' ");

            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = database.GetValueFromTable(SQL.ToString());

            foreach (KeyValuePair<string, object> kvp in data)
                VarTable.Add(kvp.Key, kvp.Value);
            return true;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool InsertFurreNamedRecord(TriggerReader reader)
        {
            string Furre = reader.ReadString().ToFurcadiaShortName();
            object info;
            if (reader.PeekString())
            {
                info = reader.ReadString();
            }
            else
            {
                info = reader.ReadNumber();
            }

            // Dim value As String = reader.ReadVariable.Value.ToString()
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, object>
            {
                { "Name", Furre},
                { "Access Level",info},
                { "date added",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) },
                { "date modified",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) }
            };

            return 0 <= database.Insert("FURRE", data);
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool InsertTriggeringFurreRecord(TriggerReader reader)
        {
            object info = "0";

            if (reader.PeekString())
            {
                info = reader.ReadString();
            }
            else
            {
                info = reader.ReadNumber();
            }

            // Dim value As String = reader.ReadVariable.Value.ToString()
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, object>
            {
                { "Name",Player.ShortName},
                { "Access Level",info},
                { "date added",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) },
                { "date modified",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) }
            };

            return database.Insert("FURRE", data) >= 0;
        }

        //memorize table % to database table {...}.
        [TriggerDescription("Store the specified variable table to the specified database table")]
        [TriggerVariableParameter]
        [TriggerStringParameter]
        private bool PutVariableTableIntoDatabaseTable(TriggerReader reader)
        {
            var VarTable = reader.ReadVariableTable(true);
            var table = reader.ReadString().Replace("[", "").Replace("]", "");

            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> kvp in VarTable)
            {
                string column = kvp.Key.Substring(1);
                if (!ReadOnlyFurreTableFields.Contains($"[{column}]"))
                    data.Add($"{column}", kvp.Value);
            }
            return 0 < database.InsertOrReplace("FURRE", data);
        }

        [TriggerStringParameter]
        [TriggerVariableParameter]
        private bool ReadDatabaseInfoForTheTriggeringFurre(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Variable = reader.ReadVariable(true);
            var Furre = reader.Page.GetVariable(TriggeringFurreShortNameVariable).Value.ToString();

            string sql = $"SELECT [{info }] FROM FURRE Where [Name]='{Furre}'";
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            Variable.Value = database.ExecuteScalar(sql);
            return true;
        }

        [TriggerDescription("Removes a column to the Furre Table")]
        [TriggerStringParameter]
        private bool RemoveRecordColumn(TriggerReader reader)
        {
            var column = reader.ReadString().Replace("[", "").Replace("]", "");
            if (SafeFurreTableFields.Contains($"[{column}]"))
            {
                Logger.Warn<MsDatabase>($"Attempt to forget {column}");
                return false;
            }
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            return 0 < database.RemoveColumn("FURRE", $"{column}");
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool TableEntryExistInTable(TriggerReader reader)
        {
            var Entry = reader.ReadString();
            var Table = reader.ReadString();

            var SelectSettngSQL = new StringBuilder()
                .Append("select SettingsTable.*, SettingsTableMaster.ID from SettingsTable )")
                .Append("inner join SettingsTableMaster on ")
                .Append("SettingsTableMaster.")
                .Append($"{Entry}= SettingsTable.[SettingsTableID] ")
                .Append($"where SettingsTableMaster.Setting = '{Table}' ");

            if (database == null) database = new SQLiteDatabase(SQLitefile);
            return database.GetValueFromTable(SelectSettngSQL.ToString()).Count < 0;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoEqualToNumberOrVariable(TriggerReader reader)
        {
            if (Player.FurreID == -1 || Player.ShortName == "furcadiagameserver")
                return false;
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);
            return Number == check.AsDouble(0d);
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool TriggeringFurreRecordInfoEqualToString(TriggerReader reader)
        {
            var info = reader.ReadString();
            var str = reader.ReadString();

            return str == GetRecordInfoForTheFurreNamed(info, Player.ShortName).AsString();
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoGreaterThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);

            return check.AsDouble(0d) > Number;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoGreaterThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);

            return check.AsDouble(0d) >= Number;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoLessThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);

            return check.AsDouble(0d) < Number;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoLessThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);

            return check.AsDouble(0d) <= Number;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoNotEqualToNumberOrVariable(TriggerReader reader)
        {
            return !TriggeringFurreRecordInfoEqualToNumberOrVariable(reader);
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool TriggeringFurreRecordInfoNotEqualToString(TriggerReader reader)
        {
            return !TriggeringFurreRecordInfoEqualToString(reader);
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool UpdateFurreNamed_Field(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Furre = reader.ReadString().ToFurcadiaShortName();
            var value = reader.ReadNumber();
            if (database == null)
                database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, object>
            {
                { "Name", Furre},
                { info, value },
                { "date modified",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) }
            };

            return 0 <= database.Update("FURRE", data, "[Name]='" + Furre + "'");
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool UpdateFurreNamed_FieldSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();

            var value = reader.ReadString();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, object>
            {
                { "Name", Furre},
                { info,value},
                { "date modified",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) }
            };

            return 0 <= database.Update("FURRE", data, $"[Name]='{Furre}'");
        }

        [TriggerStringParameter]
        [TriggerVariableParameter]
        [TriggerStringParameter]
        private bool UpdateTriggeringFurreField(TriggerReader reader)
        {
            var info = reader.ReadString();
            var value = reader.ReadNumber();

            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, object>
            {
                { "Name", Player.ShortName},
                { info, value },
                { "date modified",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) }
            };

            return 0 <= database.Update("FURRE", data, "[Name]='" + Player.ShortName + "'");
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool UpdateTriggeringFurreFieldSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.Page.GetVariable(TriggeringFurreShortNameVariable).Value.ToString();
            var value = reader.ReadString();

            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, object>
            {
                { "Name", Player.ShortName},
                { info, value },
                { "date modified",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) }
            };
            return 0 <= database.Update("FURRE", data, $"[Name]='{ Furre }'");
        }

        [TriggerDescription("Creates a new database or reuses the specified datase.")]
        [TriggerStringParameter]
        private bool UseOrCreateSQLiteFileIfNotExist(TriggerReader reader)
        {
            SQLitefile = Paths.CheckBotFolder(reader.ReadString());
            database = new SQLiteDatabase(SQLitefile);
            Logger.Warn<MsDatabase>($"NOTICE: SQLite Database file is now \"{SQLitefile}\"");
            return true;
        }

        [TriggerDescription("This is like a system defrag for the database.")]
        private bool VACUUM(TriggerReader reader)
        {
            DateTime startDate = DateTime.Now;
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var rows = database.ExecuteNonQuery("VACUUM");
            var time = DateTime.Now.Subtract(startDate);
            Logger.Info<MsDatabase>($"VAACUM operation took {time.Seconds} and {rows} were updated");
            return true;
        }

        #endregion Private Methods
    }
}