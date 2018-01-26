using Engine.Libraries.Data;
using MonkeyCore2.IO;
using Monkeyspeak;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Engine.Libraries.MsLibHelper;

namespace Engine.Libraries
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
        /// Shared(Static) Database file
        /// </summary>
        private static string _SQLitefile;

        //   private SQLiteDatabase database = null;
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

        /// <summary>
        /// (5:412) select Database info {...} about the furre named {...},
        /// and put it in variable %Variable.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public static bool ReadDatabaseInfoName(TriggerReader reader)
        {
            var db = new SQLiteDatabase(SQLitefile);
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadVariable(true);
            //  Dim db As SQLiteDatabase = New SQLiteDatabase(file)
            string cmd = $"SELECT [{info}] FROM FURRE Where [Name]='{Furre}'";
            Variable.Value = db.ExecuteScalar(cmd);
            return true;
        }

        /// <summary>
        /// (5:424) from table {...} take column {...} from record offest # and
        /// and put it into variable %
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool CollumnAtRowIndex(TriggerReader reader)
        {
            var Table = reader.ReadString().Replace("[", "").Replace("]", "").Replace("'", "''");
            var Column = reader.ReadString();
            var RecordOffset = reader.ReadNumber();
            var OutVar = reader.ReadVariable(true);
            string sql = $"SELECT {Column} FROM [{Table }] Where offset=RecordIndex;";
            var db = new SQLiteDatabase(SQLitefile);
            var value = db.ExecuteScalar(sql);
            OutVar.Value = value;
            return true;
        }

        /// <summary>
        /// (5:423) take the sum of column{...} in table {...} and put it
        /// into variable %
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool ColumnSum(TriggerReader reader)
        {
            var Column = reader.ReadString();
            var Table = reader.ReadString();
            var Total = reader.ReadVariable(true);
            string sql = $"SELECT { Column } FROM {Table }";
            var db = new SQLiteDatabase(SQLitefile);
            var dt = db.GetDataTable(sql);
            Column = Column.Replace("[", "").Replace("]", "");
            double suma = 0;

            foreach (DataRow row in dt.Rows)
            {
                double.TryParse(row[Column].ToString(), out double num);
                suma = suma + num;
            }

            Total.Value = suma;
            return true;
        }

        /// <summary>
        /// (5:419) delete all Database info about the furre named {...}.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool DeleteFurreNamed(TriggerReader reader)
        {
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var db = new SQLiteDatabase(SQLitefile);
            return 0 < db.ExecuteNonQuery($"Delete from FURRE where Name='{Furre }'");
        }

        /// <summary>
        /// (5:422) get the total number of records from table {...} and put
        /// it into variable %.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public bool GetTotalRecords(TriggerReader reader)
        {
            var db = new SQLiteDatabase(SQLitefile);
            var Table = reader.ReadString().Replace("[", "").Replace("]", "");
            var Total = reader.ReadVariable(true);
            var count = db.ExecuteScalar($"select count(*) from [{Table }]");
            Total.Value = count;
            return true;
        }

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            SQLitefile = Paths.CheckBotFolder("SilverMonkey.db");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoEqualToNumber(r),
                "and the Database info {...} about the triggering furre is equal to #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoNotEqualToNumber(r),
                "and the Database info {...} about the triggering furre is not equal to #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoGreaterThanNumber(r),
                "and the Database info {...} about the triggering furre is greater than #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoLessThanNumber(r),
                "and the Database info {...} about the triggering furre is less than #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoGreaterThanOrEqualToNumber(r),
                "and the Database info {...} about the triggering furre is greater than or equal to #,");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoLessThanOrEqualToNumber(r),
                "and the Database info {...} about the triggering furre is less than or equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedinfoEqualToNumber(r),
                "and the Database info {...} about the furre named {...} is equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedinfoNotEqualToNumber(r),
                "and the Database info {...} about the furre named {...} is not equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedinfoGreaterThanNumber(r),
                "and the Database info {...} about the furre named {...} is greater than #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedinfoLessThanNumber(r), "and the Database info {...} about the furre named {...} is less than #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedinfoGreaterThanOrEqualToNumber(r),
                "and the Database info {...} about the furre named {...} is greater than or equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedinfoLessThanOrEqualToNumber(r),
                "and the Database info {...} about the furre named {...} is less than or equal to #,");

            Add(TriggerCategory.Condition,
                r => FurreNamedinfoEqualToSTR(r),
                "and the Database info {...} about the furre named {...} is equal to string {...},");

            Add(TriggerCategory.Condition,
                r => FurreNamedinfoNotEqualToSTR(r),
                "and the Database info {...} about the furre named {...} is not equal to string {...},");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoEqualToSTR(r),
                "and the Database info {...} about the triggering furre is equal to string {...},");

            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoNotEqualToSTR(r),
                "and the Database info {...} about the triggering furre is not equal to string {...},");

            Add(TriggerCategory.Effect,
                r => UseOrCreateSQLiteFileIfNotExist(r),
                "use SQLite database file {...} or create file if it does not exist.");

            Add(TriggerCategory.Effect,
                r => InsertTriggeringFurreRecord(r),
                "add the triggering furre with the access level # to the Furre Table in the database if he, she, or it doesn\t exist.");

            Add(TriggerCategory.Effect,
                r => InsertFurreNamed(r)
                , "add furre named {...} with the access level # to the Furre Table in the database if he, she, or it doesn\t exist.");

            Add(TriggerCategory.Effect,
                r => UpdateTriggeringFurreField(r),
                "update Database info {...} about the triggering furre will now be #.");

            Add(TriggerCategory.Effect,
                r => UpdateFurreNamed_Field(r),
                "update Database info {...} about the furre named {...} will now be #.");

            Add(TriggerCategory.Effect,
                r => UpdateTriggeringFurreFieldSTR(r),
                "update Database info {...} about the triggering furre will now be {...}.");

            Add(TriggerCategory.Effect,
                r => UpdateFurreNamed_FieldSTR(r),
                "update Database info {...} about the furre named {...} will now be {...}.");

            Add(TriggerCategory.Effect,
                r => ReadDatabaseInfoForTheTriggeringFurre(r),
                "select database {...} about the triggering furre, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r => ReadDatabaseInfoName(r),
                "select Database info {...} about the furre named {...}, and put it in variable %.");

            Add(TriggerCategory.Effect,
                r =>
                {
                    throw new NotImplementedException();
                },
                "add column {...} with type {...} to the Furre table.");

            Add(TriggerCategory.Effect,
                r =>
                {
                    throw new NotImplementedException();
                },
                "remove column {...} from the Furre table.");

            Add(TriggerCategory.Effect,
                r => DeleteTriggeringFurre(r),
                "delete all Database info about the triggering furre.");

            Add(TriggerCategory.Effect,
                r => DeleteFurreNamed(r),
                "delete all Database info about the furre named {...}.");

            Add(TriggerCategory.Effect,
                r => GetTotalRecords(r),
                "get the total number of records from database table {...} and put it into variable %Variable.");

            Add(TriggerCategory.Effect,
                r => ColumnSum(r),
                "take the sum of column{...} in database table {...} and put it into variable %Variable.");

            Add(TriggerCategory.Effect,
                r => GetVariableTableFromDatabaseTable(r),
                "get the database table {...} and put it into table % .");

            Add(TriggerCategory.Effect,
                r =>
                {
                    throw new NotImplementedException();
                },
                "take table % and put it into database table {...}.");

            Add(TriggerCategory.Effect,
                r =>
                {
                    throw new NotImplementedException();
                },
                "take table % and put it into database table {...}.");

            Add(TriggerCategory.Effect,
                r =>
                {
                    throw new NotImplementedException();
                },
                "clear the database table {...}.");

            Add(TriggerCategory.Effect,
                r => VACUUM(r),
                "execute \"VACUUM\"to rebuild the database and reclaim wasted space.");
        }

        public override void Unload(Page page)
        { }

        #endregion Public Methods

        #region Private Methods

        private bool DeleteTriggeringFurre(TriggerReader reader)
        {
            var db = new SQLiteDatabase(SQLitefile);
            return 0 < db.ExecuteNonQuery($"Delete from FURRE where Name='{Player.ShortName}'");
        }

        private bool FurreNamedinfoEqualToNumber(TriggerReader reader)
        {
            var db = new SQLiteDatabase(SQLitefile);
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var result = db.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);
            return Value == Variable;
        }

        private bool FurreNamedinfoEqualToSTR(TriggerReader reader)
        {
            string info = reader.ReadString();
            string Furre = reader.ReadString().ToFurcadiaShortName();
            string str = reader.ReadString();
            var db = new SQLiteDatabase(SQLitefile);
            var result = db.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            return result.ToString() == str;
        }

        private bool FurreNamedinfoGreaterThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var db = new SQLiteDatabase(SQLitefile);
            var result = db.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value > Variable;
        }

        private bool FurreNamedinfoGreaterThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var db = new SQLiteDatabase(SQLitefile);
            var result = db.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value >= Variable;
        }

        private bool FurreNamedinfoLessThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var db = new SQLiteDatabase(SQLitefile);
            var result = db.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);
            return Value < Variable;
        }

        private bool FurreNamedinfoLessThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var db = new SQLiteDatabase(SQLitefile);
            var result = db.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value <= Variable;
        }

        private bool FurreNamedinfoNotEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var db = new SQLiteDatabase(SQLitefile);
            var result = db.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value != Variable;
        }

        private bool FurreNamedinfoNotEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var str = reader.ReadString();
            var db = new SQLiteDatabase(SQLitefile);
            var result = db.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            return str != result.ToString();
        }

        private object GetValueFromTable(string Column, string Name)
        {
            var db = new SQLiteDatabase(SQLitefile);
            var result = db.ExecuteScalar($"SELECT {Column} FROM FURRE Where Name = '{Name.ToFurcadiaShortName()}'");
            return result;
        }

        private bool InsertFurreNamed(TriggerReader reader)
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
            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name",$"{Furre}"},
                { "Access Level",$"{info}"}
            };

            return db.Insert("FURRE", data) > 0;
        }

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
            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name",$"{Player.ShortName}"},
                { "Access Level",$"{info}"}
            };

            return db.Insert("FURRE", data) > 0;
        }

        private bool ReadDatabaseInfoForTheTriggeringFurre(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Variable = reader.ReadVariable(true);
            var Furre = reader.Page.GetVariable(TriggeringFurreShortNameVariable).Value.ToString();

            string cmd = $"SELECT [{info }] FROM FURRE Where [Name]='{Furre}'";
            var db = new SQLiteDatabase(SQLitefile);
            Variable.Value = db.ExecuteScalar(cmd);
            return true;
        }

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

            var db = new SQLiteDatabase(SQLitefile);
            var data = db.GetValueFromTable(SelectSettngSQL.ToString());

            foreach (KeyValuePair<string, object> kvp in data)
                VarTable.Add($"%{kvp.Key}", kvp.Value);
            return true;
        }

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

            var db = new SQLiteDatabase(SQLitefile);
            return db.GetValueFromTable(SelectSettngSQL.ToString()).Count < 0;
        }

        private bool TriggeringFurreinfoEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            double.TryParse(GetValueFromTable(info, Player.ShortName).ToString(), out double Value);

            return Number == Value;
        }

        private bool TriggeringFurreinfoEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var str = reader.ReadString();

            return str == GetValueFromTable(info, Player.ShortName).ToString();
        }

        private bool TriggeringFurreinfoGreaterThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetValueFromTable(info, Player.ShortName);
            double.TryParse(check.ToString(), out double Value);

            return Value > Number;
        }

        private bool TriggeringFurreinfoGreaterThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetValueFromTable(info, Player.ShortName);
            double.TryParse(check.ToString(), out double Num);

            return Num >= Number;
        }

        private bool TriggeringFurreinfoLessThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetValueFromTable(info, Player.ShortName);
            double.TryParse(check.ToString(), out double Num);

            return Num < Number;
        }

        private bool TriggeringFurreinfoLessThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            var check = GetValueFromTable(info, Player.ShortName);
            double.TryParse(check.ToString(), out double Num);

            return Num <= Number;
        }

        private bool TriggeringFurreinfoNotEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            string val = GetValueFromTable(info, Player.ShortName).ToString();
            double.TryParse(val, out double Value);

            return Value != Number;
        }

        private bool TriggeringFurreinfoNotEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var str = reader.ReadString();

            return str != GetValueFromTable(info, Player.ShortName).ToString();
        }

        private bool UpdateFurreNamed_Field(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Furre = reader.ReadString().ToFurcadiaShortName();
            var value = reader.ReadNumber().ToString();
            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name", Furre},
                { info, value },
                { "date modified" ,DateTime.Now.ToShortDateString()}
            };

            return db.Update("FURRE", data, "[Name]='" + Furre + "'");
        }

        private bool UpdateFurreNamed_FieldSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();

            var value = reader.ReadString();
            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name", $"{Furre}"},
                { "date modified", DateTime.Now.ToLongDateString() },
                { $"{info}",$"{value}"}
            };

            return db.Update("FURRE", data, $"[Name]='{Furre}'");
        }

        private bool UpdateTriggeringFurreField(TriggerReader reader)
        {
            var info = reader.ReadString();
            var value = reader.ReadNumber();

            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name", Player.ShortName},
                { info, value.ToString() },
                { "date modified" ,DateTime.Now.ToShortDateString()}
            };

            return db.Update("FURRE", data, "[Name]='" + Player.ShortName + "'");
        }

        private bool UpdateTriggeringFurreFieldSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.Page.GetVariable(TriggeringFurreShortNameVariable).Value.ToString();
            var value = reader.ReadString();

            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name", Player.ShortName},
                { info, value.ToString() },
                { "date modified" ,DateTime.Now.ToShortDateString()}
            };
            return db.Update("FURRE", data, $"[Name]='{ Furre }'");
        }

        private bool UseOrCreateSQLiteFileIfNotExist(TriggerReader reader)
        {
            SQLitefile = Paths.CheckBotFolder(reader.ReadString());
            Monkeyspeak.Logging.Logger.Warn<MsDatabase>($"NOTICE: SQLite Database file is now {SQLitefile}");
            return true;
        }

        private bool VACUUM(TriggerReader reader)
        {
            DateTime startDate = DateTime.Now;
            var db = new SQLiteDatabase(SQLitefile);
            var rows = db.ExecuteNonQuery("VACUUM");
            var time = DateTime.Now.Subtract(startDate);
            Monkeyspeak.Logging.Logger.Info<MsDatabase>($"VAACUM operation took {time.Seconds} and {rows} were updated");
            return true;
        }

        #endregion Private Methods

        // (5:561) remember Database info {...} for Settings Table {...} to {...}.
        // (5:562) forget Database info {...} from Settings Table{...}.
        // (5:563) forget all Settings Table Database info.
    }
}