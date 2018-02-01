using Libraries.Data;
using MonkeyCore2.IO;
using Monkeyspeak;
using Monkeyspeak.Libraries;
using Monkeyspeak.Logging;
using System;
using System.Collections.Generic;

using System.Text;
using static Libraries.MsLibHelper;

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

        private SQLiteDatabase database = null;
        private const string DateTimeFormat = "MM-dd-yyyy hh:mm:ss";
        private const string DataBaseTimeZone = "Central America Standard Time";

        /// <summary>
        /// Shared(Static) Database file
        /// </summary>
        private static string _SQLitefile;

        //   private SQLiteDatabase database == null;
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
        /// Currenly used database filfe
        /// </summary>
        /// <returns>
        /// SQLite database file with Silver Monkey system tables and user data
        /// </returns>
        public static string SQLitefile
        {
            get
            {
                return _SQLitefile;
            }
            set
            {
                _SQLitefile = Paths.CheckBotFolder(value);
            }
        }

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
                r => TriggeringFurreRecordInfoEqualToNumber(r),
                "and the Database Record {...} about the triggering furre is equal to #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoNotEqualToNumber(r),
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
                r => FurreNamedRecordInfoEqualToNumber(r),
                "and the Database Record {...} about the furre named {...} is equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoNotEqualToNumber(r),
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
                r => FurreNamedRecordInfoEqualToSTR(r),
                "and the Database Record {...} about the furre named {...} is equal to string {...},");

            Add(TriggerCategory.Condition,
                r => FurreNamedRecordInfoNotEqualToSTR(r),
                "and the Database Record {...} about the furre named {...} is not equal to string {...},");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoEqualToSTR(r),
                "and the Database Record {...} about the triggering furre is equal to string {...},");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreRecordInfoNotEqualToSTR(r),
                "and the Database Record {...} about the triggering furre is not equal to string {...},");

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
                "update Database Record {...} about the triggering furre will now be #.");

            Add(TriggerCategory.Effect,
                r => UpdateFurreNamed_Field(r),
                "update Database Record {...} about the furre named {...} will now be #.");

            Add(TriggerCategory.Effect,
                r => UpdateTriggeringFurreFieldSTR(r),
                "update Database Record {...} about the triggering furre will now be {...}.");

            Add(TriggerCategory.Effect,
                r => UpdateFurreNamed_FieldSTR(r),
                "update Database Record {...} about the furre named {...} will now be {...}.");

            Add(TriggerCategory.Effect,
                r => ReadDatabaseInfoForTheTriggeringFurre(r),
                "select Database Record {...} about the triggering furre, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => ReadDatabaseInfoName(r),
                "select Database Record {...} about the furre named {...}, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => GetDateAddedForFurreNamed(r),
                "get the date added time stamp for the furre named {...}, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => GetDateModifiedForFurreNamed(r),
                "get the date modified time stamp for the furre named {...}, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => GetDateAddedForTriggeringFurre(r),
                "get the date added time stamp for the triggering furre and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => GetDateModifiedForTriggeringFurre(r),
                "get the date modified time stamp for the triggering furre and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => AddRecordColumn(r),
                "add column {...} with type {...} to the Furre table.");

            Add(TriggerCategory.Effect,
                r => RemoveRecordColumn(r),
                "remove column {...} from the Furre table.");

            Add(TriggerCategory.Effect,
                r => DeleteTriggeringFurreRecord(r),
                "delete all Database Record about the triggering furre.");

            Add(TriggerCategory.Effect,
                r => DeleteFurreNamedRecord(r),
                "delete all Database Record about the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => GetVariableTableFromDatabaseTable(r),
                "get the database table {...} and put it into table % .");

            Add(TriggerCategory.Effect,
                r => PutVariableTableIntoDatabaseTable(r),
                "take table % and put it into database table {...}.");

            Add(TriggerCategory.Effect,
                r => ClearSpecifiedTable(r),
                "clear the database table {...}.");

            Add(TriggerCategory.Effect,
                r => VACUUM(r),
                "execute \"VACUUM\"to rebuild the database and reclaim wasted space.");
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
            if (database == null)
                database = new SQLiteDatabase(SQLitefile);
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadVariable(true);
            //  Dim db As SQLiteDatabase = New SQLiteDatabase(file)
            string cmd = $"SELECT [{info}] FROM FURRE Where [Name]='{Furre}'";
            Variable.Value = database.ExecuteScalar(cmd);
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
        private bool FurreNamedRecordInfoEqualToNumber(TriggerReader reader)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var result = database.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);
            return Value == Variable;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoEqualToSTR(TriggerReader reader)
        {
            string info = reader.ReadString();
            string Furre = reader.ReadString().ToFurcadiaShortName();
            string str = reader.ReadString();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var result = database.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            return result.ToString() == str;
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
            var result = database.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value > Variable;
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
            var result = database.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value >= Variable;
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
            var result = database.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);
            return Value < Variable;
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
            var result = database.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value <= Variable;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoNotEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var result = database.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value != Variable;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool FurreNamedRecordInfoNotEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var str = reader.ReadString();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var result = database.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            return str != result.ToString();
        }

        [TriggerStringParameter]
        [TriggerVariableParameter]
        private bool GetDateAddedForFurreNamed(TriggerReader reader)
        {
            var furre = reader.ReadString().ToFurcadiaShortName();
            var TimeStamp = reader.ReadVariable(true);

            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var ts = database.ExecuteScalar($"SELECT [date modified] FROM FURRE Where Name = '{furre}'");

            TimeStamp.Value = DateTime.Parse(ts.ToString());
            return true;
        }

        private bool GetDateAddedForTriggeringFurre(TriggerReader reader)
        {
            var TimeStamp = reader.ReadVariable(true);
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var ts = database.ExecuteScalar($"SELECT [date added] FROM FURRE Where Name = '{Player.ShortName}'");
            TimeStamp.Value = DateTime.Parse(ts.ToString()).ToString(DateTimeFormat);
            return true;
        }

        private bool GetDateModifiedForFurreNamed(TriggerReader reader)
        {
            var TimeStamp = reader.ReadVariable(true);
            var furre = reader.ReadString().ToFurcadiaShortName();
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var ts = database.ExecuteScalar($"SELECT [date modified] FROM FURRE Where Name = '{furre}'");

            TimeStamp.Value = DateTime.Parse(ts.ToString()).ToString(DateTimeFormat);
            return true;
        }

        private bool GetDateModifiedForTriggeringFurre(TriggerReader reader)
        {
            var TimeStamp = reader.ReadVariable(true);
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var ts = database.ExecuteScalar($"SELECT [date modified] FROM FURRE Where Name = '{Player.ShortName}'");

            TimeStamp.Value = DateTime.Parse(ts.ToString()).ToString(DateTimeFormat);
            return true;
        }

        private object GetRecordInfoForTheFurreNamed(string Column, string Name)
        {
            if (database == null) database = new SQLiteDatabase(SQLitefile);

            var result = database.ExecuteScalar($"SELECT [{Column}] FROM FURRE Where Name = '{Name.ToFurcadiaShortName()}'");
            return result;
        }

        [TriggerVariableParameter]
        private bool GetVariableTableFromDatabaseTable(TriggerReader reader)
        {
            var DatabaseTable = reader.ReadString();
            var VarTable = reader.ReadVariableTable(true);

            var SelectSettngSQL = new StringBuilder()
                .Append("select SettingsTable.*, SettingsTableMaster.ID from SettingsTable )")
                .Append("inner join SettingsTableMaster on ")
                .Append("SettingsTableMaster.")
                // .Append($"{Entry}= SettingsTable.[SettingsTableID] ")
                .Append($"where SettingsTableMaster.Setting = '{VarTable}' ");

            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = database.GetValueFromTable(SelectSettngSQL.ToString());

            foreach (KeyValuePair<string, object> kvp in data)
                VarTable.Add($"%{kvp.Key}", kvp.Value);
            return true;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool InsertFurreNamedRecord(TriggerReader reader)
        {
            string Furre = reader.ReadString().ToFurcadiaShortName();
            string info;
            if (reader.PeekString())
            {
                info = reader.ReadString();
            }
            else
            {
                info = reader.ReadNumber().ToString();
            }

            // Dim value As String = reader.ReadVariable.Value.ToString()
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name",$"{Furre}"},
                { "Access Level",$"{info}"},
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
            string info = "0";

            if (reader.PeekString())
            {
                info = reader.ReadString();
            }
            else
            {
                info = reader.ReadNumber().ToString();
            }

            // Dim value As String = reader.ReadVariable.Value.ToString()
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name",$"{Player.ShortName}"},
                { "Access Level",$"{info}"},
                { "date added",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) },
                { "date modified",TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(DataBaseTimeZone)).ToString(DateTimeFormat) }
            };

            return database.Insert("FURRE", data) >= 0;
        }

        //
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
                    data.Add($"[{column}]", kvp.Value.ToString());
            }
            return 0 < database.Update("FURRE", data);
        }

        [TriggerStringParameter]
        [TriggerVariableParameter]
        private bool ReadDatabaseInfoForTheTriggeringFurre(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Variable = reader.ReadVariable(true);
            var Furre = reader.Page.GetVariable(TriggeringFurreShortNameVariable).Value.ToString();

            string cmd = $"SELECT [{info }] FROM FURRE Where [Name]='{Furre}'";
            if (database == null) database = new SQLiteDatabase(SQLitefile);
            Variable.Value = database.ExecuteScalar(cmd);
            return true;
        }

        [TriggerDescription("Removes a column to the Furre Table")]
        [TriggerStringParameter]
        private bool RemoveRecordColumn(TriggerReader reader)
        {
            var column = reader.ReadString().Replace("[", "").Replace("]", "");
            if (SafeFurreTableFields.Contains($"[{column}]"))
            {
                Logger.Warn<MsDatabase>($"Attempt to delete {column}");
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
        private bool TriggeringFurreRecordInfoEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            double.TryParse(GetRecordInfoForTheFurreNamed(info, Player.ShortName).ToString(), out double Value);

            return Number == Value;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool TriggeringFurreRecordInfoEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var str = reader.ReadString();

            return str == GetRecordInfoForTheFurreNamed(info, Player.ShortName).ToString();
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoGreaterThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);
            double.TryParse(check.ToString(), out double Value);

            return Value > Number;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoGreaterThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);
            double.TryParse(check.ToString(), out double Num);

            return Num >= Number;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoLessThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);
            double.TryParse(check.ToString(), out double Num);

            return Num < Number;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoLessThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetRecordInfoForTheFurreNamed(info, Player.ShortName);
            double.TryParse(check.ToString(), out double Num);

            return Num <= Number;
        }

        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool TriggeringFurreRecordInfoNotEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            string val = GetRecordInfoForTheFurreNamed(info, Player.ShortName).ToString();
            double.TryParse(val, out double Value);

            return Value != Number;
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        private bool TriggeringFurreRecordInfoNotEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var str = reader.ReadString();

            return str != GetRecordInfoForTheFurreNamed(info, Player.ShortName).ToString();
        }

        [TriggerStringParameter]
        [TriggerStringParameter]
        [TriggerNumberParameter]
        private bool UpdateFurreNamed_Field(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Furre = reader.ReadString().ToFurcadiaShortName();
            var value = reader.ReadNumber().ToString();
            if (database == null)
                database = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
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
            var data = new Dictionary<string, string>
            {
                { "Name", $"{Furre}"},
                { $"{info}",$"{value}"},
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
            var data = new Dictionary<string, string>
            {
                { "Name", Player.ShortName},
                { info, value.ToString() },
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
            var data = new Dictionary<string, string>
            {
                { "Name", Player.ShortName},
                { info, value.ToString() },
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