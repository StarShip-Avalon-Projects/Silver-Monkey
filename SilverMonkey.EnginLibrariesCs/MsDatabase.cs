using MonkeyCore2.Data;
using MonkeyCore2.IO;
using Monkeyspeak;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
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
        #region Public Fields

        /// <summary>
        /// </summary>
        public static SQLiteDataReader SQLreader = null;

        #endregion Public Fields

        #region Private Fields

        /// <summary>
        /// Shared(Static) Database file
        /// </summary>
        private static string _SQLitefile;

        private Dictionary<string, object> cache = new Dictionary<string, object>();

        private bool QueryRun = false;

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
                if (string.IsNullOrEmpty(_SQLitefile))
                {
                    _SQLitefile = Path.Combine(Paths.SilverMonkeyBotPath, "SilverMonkey.db");
                }

                return _SQLitefile;
            }
            set
            {
                _SQLitefile = Paths.CheckBotFolder(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override int BaseId => 500;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// (5:513) add column {...} with type {...} to the Furre table.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public static bool AddColumn(TriggerReader reader)
        {
            var Column = reader.ReadString();
            var Type = reader.ReadString();
            var db = new SQLiteDatabase(SQLitefile);
            db.AddColumn("FURRE", $"[{ Column }]", Type);
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
        public static bool CollumnAtRowIndex(TriggerReader reader)
        {
            var Table = reader.ReadString().Replace("[", "").Replace("]", "").Replace("'", "''");
            var Column = reader.ReadString();
            var RecordOffset = reader.ReadNumber();
            var OutVar = reader.ReadVariable(true);
            string sql = $"SELECT {Column}FROM [{Table }] Where offset=RecordIndex;";
            var value = SQLiteDatabase.ExecuteScalar(sql);
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
        public static bool ColumnSum(TriggerReader reader)
        {
            var Column = reader.ReadString();
            var Table = reader.ReadString();
            var Total = reader.ReadVariable(true);
            string sql = $"SELECT { Column } FROM {Table }";
            var dt = SQLiteDatabase.GetDataTable(sql);
            Column = Column.Replace("[", "").Replace("]", "");
            double suma = 0;

            foreach (DataRow row in dt.Rows)
            {
                double.TryParse(row[Column].ToString(), out double num);
                suma = suma + num;
            }

            // Console.WriteLine("Calculating TotalSum {0}", TotalSum.ToString())
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
        public static bool DeleteFurreNamed(TriggerReader reader)
        {
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var db = new SQLiteDatabase(SQLitefile);
            return 0 < SQLiteDatabase.ExecuteNonQuery($"Delete from FURRE where Name='{Furre }'");
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
        public static bool GetTotalRecords(TriggerReader reader)
        {
            var db = new SQLiteDatabase(SQLitefile);
            var Table = reader.ReadString().Replace("[", "").Replace("]", "");
            var Total = reader.ReadVariable(true);
            var count = SQLiteDatabase.ExecuteScalar($"select count(*) from [{Table }]");
            Total.Value = count;
            return true;
        }

        /// <summary>
        /// (5:550) take variable %Variable , prepare it for a query, and
        /// put it in variable %Variable (this is your escaping call, which
        /// would depend on however you have to do it internally)
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        public static bool PrepQuery(TriggerReader reader)
        {
            var var1 = reader.ReadVariable(true);
            var var2 = reader.ReadVariable(true);
            var str = var1.Value.ToString();
            str = str.Replace("'", "''");
            var2.Value = str;
            return true;
        }

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
            Variable.Value = SQLiteDatabase.ExecuteScalar(cmd);
            return true;
        }

        public override void Initialize(params object[] args)
        {
            base.Initialize(args);
            SQLitefile = Paths.CheckBotFolder("SilverMonkey.db");
            // (1:500) and the Database info {...} about the triggering furre is equal to #,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoEqualToNumber(r),
                "and the Database info {...} about the triggering furre is equal to #,");
            // (1:501) and the Database info {...} about the triggering furre is not equal to #,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoNotEqualToNumber(r),
                "and the Database info {...} about the triggering furre is not equal to #,");
            // (1:502) and the Database info {...} about the triggering furre is greater than #,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoGreaterThanNumber(r),
                "and the Database info {...} about the triggering furre is greater than #,");
            // (1:503) and the Database info {...} about the triggering furre is less than #,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoLessThanNumber(r),
                "and the Database info {...} about the triggering furre is less than #,");
            // (1:504) and the Database info {...} about the triggering furre is greater than or equal to #,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoGreaterThanOrEqualToNumber(r),
                "and the Database info {...} about the triggering furre is greater than or equal to #,");
            // (1:505) and the Database info {...} about the triggering furre is less than or equal to#,
            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoLessThanOrEqualToNumber(r),
                "and the Database info {...} about the triggering furre is less than or equal to #,");
            // (1:508) and the Database info {...} about the furre named {...} is equal to #,
            Add(TriggerCategory.Condition,
                r => FurreNamedinfoEqualToNumber(r),
                "and the Database info {...} about the furre named {...} is equal to #,");
            // (1:509) and the Database info {...} about the furre named {...} is not equal to #,
            Add(TriggerCategory.Condition,
                r => FurreNamedinfoNotEqualToNumber(r),
                "and the Database info {...} about the furre named {...} is not equal to #,");
            // (1:510) and the Database info {...} about the furre named {...} is greater than #,
            Add(TriggerCategory.Condition,
                r => FurreNamedinfoGreaterThanNumber(r),
                "and the Database info {...} about the furre named {...} is greater than #,");
            // (1:511) and the Database info {...} about the furre named {...} is less than #,
            Add(TriggerCategory.Condition,
                r => FurreNamedinfoLessThanNumber(r), "and the Database info {...} about the furre named {...} is less than #,");
            // (1:510) and the Database info {...} about the furre named {...} is greater than or equal to #,
            Add(TriggerCategory.Condition,
                r => FurreNamedinfoGreaterThanOrEqualToNumber(r),
                "and the Database info {...} about the furre named {...} is greater than or equal to #,");
            // (1:511) and the Database info {...} about the furre named {...} is less than or equal to #,
            Add(TriggerCategory.Condition,
                r => FurreNamedinfoLessThanOrEqualToNumber(r),
                "and the Database info {...} about the furre named {...} is less than or equal to #,");
            // (1:516) and the Database info {...} about the furre named {...} is equal to {...},
            Add(TriggerCategory.Condition,
                r => FurreNamedinfoEqualToSTR(r),
                "and the Database info {...} about the furre named {...} is equal to string {...},");
            // (1:517) and the Database info {...} about the furre named {...} is not equal to {...},
            Add(TriggerCategory.Condition,
                r => FurreNamedinfoNotEqualToSTR(r), "and the Database info {...} about the furre named {...} is not equal to string {...},");
            // (1:518) and the Database info {...} about the triggering furre is equal to {...},
            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoEqualToSTR(r),
                "and the Database info {...} about the triggering furre is equal to string {...},");
            // (1:519) and the Database info {...} about the triggering furre is not equal to {...},
            Add(TriggerCategory.Condition,
                r => TriggeringFurreinfoNotEqualToSTR(r),
                "and the Database info {...} about the triggering furre is not equal to string {...},");
            // Installed 7/13/120`16
            // (1:524) and the Database info  {...} in Settings Table {...} exists,
            Add(TriggerCategory.Condition,
                r => SettingExist(r),
                "and the Database info  {...} in Settings Table {...} exists,");
            // (1:525) and the Database info  {...} in Settings Table {...} doesn't exist,
            Add(TriggerCategory.Condition,
                r => SettingNotExist(r),
                "and the Database info  {...} in Settings Table {...} doesn\t exist,");
            // (1:526) and the Database info {..} in Settings Table  {...} Is equal to {...},
            Add(TriggerCategory.Condition,
                r => SettingEqualTo(r),
                "and the Database info {..} in Settings Table  {...} Is equal to {...},");
            // (1:527) and the Database info {..} in Settings Table  {...} Is Not equal to {...},
            Add(TriggerCategory.Condition,
                r => SettingNotEqualTo(r),
                "and the Database info {..} in Settings Table  {...} Is not equal to {...},");
            // (1:528) and the Database info {..} in Settings Table  {...} Is greater than #,
            Add(TriggerCategory.Condition,
                r => SettingGreaterThan(r),
                "and the Database info {..} in Settings Table  {...} Is greater than #,");
            // (1:529) and the Database info {..} in Settings Table  {...} Is greater than or equal to #,
            Add(TriggerCategory.Condition,
                r => SettingGreaterThanOrEqualTo(r),
                "and the Database info {..} in Settings Table  {...} Is greater than or equal to #,");
            // (1:530) and the Database info {..} in Settings Table  {...} Is less than #,
            Add(TriggerCategory.Condition,
                r => SettingLessThan(r),
                "and the Database info {..} in Settings Table  {...} Is less than #,");
            // (1:530) and the Database info {..} in Settings Table  {...} Is less than #,
            Add(TriggerCategory.Condition,
                r => SettingLessThanOrEqualTo(r),
                "and the Database info {..} in Settings Table  {...} Is less than or equal to #,");
            // (5:500) use SQLite database file {...} or create file if it does not exist.
            Add(TriggerCategory.Effect,
                r => UseOrCreateSQLiteFileIfNotExist(r),
                "use SQLite database file {...} or create file if it does not exist.");
            // (5:505 ) Add the triggering furre with the default access level 0 to the Furre Table in the database if he, she or it don't exist.
            Add(TriggerCategory.Effect,
                r => InsertTriggeringFurreRecord(r),
                "add the triggering furre with the default access level \"0\"to the Furre Table in the database if he, she, or it doesn\t exist.");
            // (5:506) Add furre named {...} with the default access level 0 to the Furre Table in the database if he, she or it don't exist.
            Add(TriggerCategory.Effect,
                r => InsertFurreNamed(r)
                , "add furre named {...} with the default access level \"0\"to the Furre Table in the database if he, she, or it doesn\t exist.");
            // (5:507) update Database info {...} about the triggering furre will now be #.
            Add(TriggerCategory.Effect,
                r => UpdateTriggeringFurreField(r),
                "update Database info {...} about the triggering furre will now be #.");
            // (5:508) update Database info {...} about the furre named {...} will now be #.
            Add(TriggerCategory.Effect,
                r => UpdateFurreNamed_Field(r),
                "update Database info {...} about the furre named {...} will now be #.");
            // (5:509) update Database info {...} about the triggering furre will now be {...}.
            Add(TriggerCategory.Effect,
                r => UpdateTriggeringFurreFieldSTR(r),
                "update Database info {...} about the triggering furre will now be {...}.");
            // (5:510) update Database info {...} about the furre named {...} will now be {...}.
            Add(TriggerCategory.Effect,
                r => UpdateFurreNamed_FieldSTR(r),
                "update Database info {...} about the furre named {...} will now be {...}.");
            // (5:511) select Database info {...} about the triggering furre, and put it in variable %.
            Add(TriggerCategory.Effect,
                r => ReadDatabaseInfoForTheTriggeringFurre(r),
                "select Database info {...} about the triggering furre, and put it in variable %.");
            // (5:512) select Database info {...} about the furre named {...}, and put it in variable %.
            Add(TriggerCategory.Effect,
                r => ReadDatabaseInfoName(r),
                "select Database info {...} about the furre named {...}, and put it in variable %.");
            // (5:513) add column {...} with type {...} to the Furre table.
            Add(TriggerCategory.Effect,
                r => AddColumn(r),
                "add column {...} with type {...} to the Furre table.");
            // (5:518) delete all Database info about the triggering furre.
            Add(TriggerCategory.Effect,
                r => DeleteTriggeringFurre(r),
                "delete all Database info about the triggering furre.");
            // (5:519) delete all Database info about the furre named {...}.
            Add(TriggerCategory.Effect,
                r => DeleteFurreNamed(r),
                "delete all Database info about the furre named {...}.");
            // (5:522) get the total of records from table {...} and put it into variable %.
            Add(TriggerCategory.Effect,
                r => GetTotalRecords(r),
                "get the total number of records from table {...} and put it into variable %Variable.");
            // (5:523) take the sum of column{...} in table {...} and put it into variable %
            Add(TriggerCategory.Effect,
                r => ColumnSum(r),
                "take the sum of column{...} in table {...} and put it into variable %Variable.");
            // (5:550) take variable %Variable , prepare it for a query, and put it in variable %Variable .   (this is your escaping call, which would depend on however you have to do it internally)
            Add(TriggerCategory.Effect,
                r => PrepQuery(r),
                "take variable %Variable , prepare it for a SQLite Database query, and put it in variable %Variable.");
            // (5:551) execute SQLite Database query {...} Select * from table where name=%2
            Add(TriggerCategory.Effect,
                r => ExecuteQuery(r),
                "execute SQLite Database query {...}.");
            // (5:552) retrieve field {...} from SQLite Database query and put it into variable %Variable .
            Add(TriggerCategory.Effect,
                r => RetrieveQuery(r),
                "retrieve field {...} from SQLite Database query and put it into variable %Variable.");
            Add(TriggerCategory.Effect,
                r => VACUUM(r),
                "execute \"VACUUM\"to rebuild the database and reclaim wasted space.");
            // (5:561) remember Database info {...} for Settings Table {...} to {...}.
            // (5:562) forget Database info {...} from Settings Table{...}.
            // (5:563) forget all Settings Table Database info.
        }

        public override void Unload(Page page)
        { }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// (5:418) delete all Database info about the triggering furre.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool DeleteTriggeringFurre(TriggerReader reader)
        {
            var db = new SQLiteDatabase(SQLitefile);
            return 0 < SQLiteDatabase.ExecuteNonQuery($"Delete from FURRE where Name='{Player.ShortName}'");
        }

        /// <summary>
        /// (5:551) execute query {...}.
        /// <para>
        /// Execute raw SQL commands on the database.
        /// </para>
        /// <para>
        /// For SELECT statements <see cref="RetrieveQuery"/>
        /// </para>
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool ExecuteQuery(TriggerReader reader)
        {
            var Str = reader.ReadString().Trim();
            cache.Clear();
            QueryRun = false;
            if (Str.ToUpper().StartsWith("SELECT"))
            {
                var db = new SQLiteDatabase(SQLitefile);

                cache = db.GetValueFromTable(Str);
                QueryRun = true;
                return cache.Count > 0;
            }

            SQLiteDatabase.ExecuteNonQuery(Str);
            return true;
        }

        //
        /// <summary>
        /// (1:508) and the Database info {...} about the furre named {...}
        /// is equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// True if there is no error, otherwise false and stops further
        /// conditions or effects processing in the currnt block
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool FurreNamedinfoEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);
            return Value == Variable;
        }

        /// <summary>
        /// (1:516) and the Database info {...} about the furre named {...}
        /// is equal to string {...},
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool FurreNamedinfoEqualToSTR(TriggerReader reader)
        {
            string info = reader.ReadString();
            string Furre = reader.ReadString().ToFurcadiaShortName();
            string str = reader.ReadString();
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            return result.ToString() == str;
        }

        /// <summary>
        /// (1:510) and the Database info {...} about the furre named {...}
        /// is greater than #, greater than #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool FurreNamedinfoGreaterThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value > Variable;
        }

        /// <summary>
        /// (1:512) and the Database info {...} about the furre named {...}
        /// is greater than or equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool FurreNamedinfoGreaterThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value >= Variable;
        }

        /// <summary>
        /// (1:511) and the Database info {...} about the furre named {...}
        /// is less than #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool FurreNamedinfoLessThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);
            return Value < Variable;
        }

        /// <summary>
        /// (1:513) and the Database info {...} about the furre named {...}
        /// is less than or equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool FurreNamedinfoLessThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return Value <= Variable;
        }

        /// <summary>
        /// (1:509) and the Database info {...} about the furre named {...}
        /// is not equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool FurreNamedinfoNotEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var Variable = reader.ReadNumber();
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            double.TryParse(result.ToString(), out double Value);

            return (Value != Variable);
        }

        /// <summary>
        /// (1:517) and the Database info {...} about the furre named {...}
        /// is not equal to string {...},
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool FurreNamedinfoNotEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();
            var str = reader.ReadString();
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {info} FROM FURRE Where Name = '{Furre}'");
            return str != result.ToString();
        }

        /// <summary>
        /// Gets information from the Furre Table for the specified furre
        /// </summary>
        /// <param name="Column">
        /// Collunm Name as string
        /// </param>
        /// <param name="Name">
        /// Furre Name as string
        /// </param>
        /// <returns>
        /// the Value for the specified Collumn for the specified furre
        /// </returns>
        private object GetValueFromTable(string Column, string Name)
        {
            var db = new SQLiteDatabase(SQLitefile);
            var result = SQLiteDatabase.ExecuteScalar($"SELECT {Column} FROM FURRE Where Name = '{Name.ToFurcadiaShortName()}'");
            return result;
        }

        /// <summary>
        /// (5:506) add furre named {%NewMember} with the default access
        /// level "1"to the Furre Table in the database if he, she, or it
        /// doesn't exist.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
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
                { "date modified", DateTime.Now.ToLongDateString() },
                { "Name",$"{Furre}"},
                { "date added",$"{DateTime.Now}"},
                { "Access Level",$"{info}"}
            };

            return db.Insert("FURRE", data);
        }

        /// <summary>
        /// (5:405) Add the triggering furre with default access level to
        /// the Furre Table in the database if he, she or it don't already exist.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
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
                { "date modified", DateTime.Now.ToLongDateString() },
                { "Name",$"{Player.ShortName}"},
                { $"Access Level",$"{info}"},
                { "date added",$"{DateTime.Now}"},
                { "Access Level",$"{info}"}
            };

            return db.Insert("FURRE", data);
        }

        /// <summary>
        /// (5:411) select Database info {...} about the triggering furre,
        /// and put it in variable %Variable.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool ReadDatabaseInfoForTheTriggeringFurre(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Variable = reader.ReadVariable(true);
            var Furre = reader.Page.GetVariable(TriggeringFurreShortNameVariable).Value.ToString();

            string cmd = $"SELECT [{info }] FROM FURRE Where [Name]='{Furre}'";

            Variable.Value = SQLiteDatabase.ExecuteScalar(cmd);
            return true;
        }

        /// <summary>
        /// (5:552) retrieve field {...} from query and put it into variable %Variable
        /// <para>
        /// <see cref="ExecuteQuery"/>
        /// </para>
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool RetrieveQuery(TriggerReader reader)
        {
            var Field = reader.ReadString();
            var Var = reader.ReadVariable(true);
            if (QueryRun)
            {
                if (cache.Count > 0)
                {
                    foreach (var kvp in cache)
                    {
                        if (kvp.Key == Field)
                        {
                            Var.Value = kvp.Value;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool SettingEqualTo(TriggerReader r)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (1:524) and the Database info {...} in Settings Table {...} exists,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool SettingExist(TriggerReader reader)
        {
            var info = reader.ReadString();
            var setting = reader.ReadString();

            string cmd = "select SettingsTable.*, SettingsTableMaster.ID from SettingsTable " + "inner join SettingsTableMaster on " + "SettingsTableMaster."
                       + info + "= SettingsTable.[SettingsTableID] " + "where SettingsTableMaster.Setting = '"
                       + setting + "' ";

            var db = new SQLiteDatabase(SQLitefile);
            cache = db.GetValueFromTable(cmd);
            QueryRun = true;
            return cache.Count > 0;
        }

        private bool SettingGreaterThan(TriggerReader r)
        {
            throw new NotImplementedException();
        }

        private bool SettingGreaterThanOrEqualTo(TriggerReader r)
        {
            throw new NotImplementedException();
        }

        private bool SettingLessThan(TriggerReader r)
        {
            throw new NotImplementedException();
        }

        private bool SettingLessThanOrEqualTo(TriggerReader r)
        {
            throw new NotImplementedException();
        }

        private bool SettingNotEqualTo(TriggerReader r)
        {
            throw new NotImplementedException();
        }

        private bool SettingNotExist(TriggerReader r)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (1:500) and the Database info {...} about the triggering furre
        /// is equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreinfoEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Number = reader.ReadNumber();

            double.TryParse(GetValueFromTable(info, Player.ShortName).ToString(), out double Value);

            return Number == Value;
        }

        /// <summary>
        /// "and the Database info {...} about the triggering furre
        /// is equal to string {...},
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreinfoEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();

            var str = reader.ReadString();

            if (str == GetValueFromTable(info, Player.ShortName).ToString())

            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// (1:502) and the Database info {...} about the triggering furre
        /// is greater than #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreinfoGreaterThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Number = reader.ReadNumber();
            var check = GetValueFromTable(info, Player.ShortName);
            double Value = 0;

            double.TryParse(check.ToString(), out Value);

            return (Value > Number);
        }

        /// <summary>
        /// (1:504) and the Database info {...} about the triggering furre
        /// is greater than or equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreinfoGreaterThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Number = reader.ReadNumber();
            double Num = 0;
            var check = GetValueFromTable(info, Player.ShortName);

            double.TryParse(check.ToString(), out Num);

            return (Num >= Number);
        }

        /// <summary>
        /// (1:503) and the Database info {...} about the triggering furre
        /// is less than #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreinfoLessThanNumber(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Number = reader.ReadNumber();
            var check = GetValueFromTable(info, Player.ShortName);

            double.TryParse(check.ToString(), out double Num);

            return Num < Number;
        }

        /// <summary>
        /// (1:505) and the Database info {...} about the triggering furre
        /// is less than or equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreinfoLessThanOrEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Number = reader.ReadNumber();
            var check = GetValueFromTable(info, Player.ShortName);

            double.TryParse(check.ToString(), out double Num);

            return (Num <= Number);
        }

        /// <summary>
        /// (1:501) and the Database info {...} about the triggering furre
        /// is not equal to #,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreinfoNotEqualToNumber(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Number = reader.ReadNumber();
            string val = GetValueFromTable(info, Player.ShortName).ToString();
            double Value = 0;

            double.TryParse(val, out Value);

            return (Value != Number);
        }

        /// <summary>
        /// (1:519) and the Database info {...} about the triggering furre
        /// is not equal to string {...},
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool TriggeringFurreinfoNotEqualToSTR(TriggerReader reader)
        {
            var info = reader.ReadString();

            var str = reader.ReadString();

            if (str != GetValueFromTable(info, Player.ShortName).ToString())

            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// (5:408) update Database info {...} about the furre named {...}
        /// will now be #.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool UpdateFurreNamed_Field(TriggerReader reader)
        {
            var info = reader.ReadString();

            var Furre = reader.ReadString().ToFurcadiaShortName();
            var value = reader.ReadNumber().ToString();
            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name", Furre},
                { info,       value },
                { "date modified" ,DateTime.Now.ToShortDateString()}
            };

            return db.Update("FURRE", data, "[Name]='" + Furre + "'");
        }

        /// <summary>
        /// (5:410) update database info {...} in the furre table  about the furre named {...}
        /// will now be {...}.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool UpdateFurreNamed_FieldSTR(TriggerReader reader)
        {
            var info = reader.ReadString();
            var Furre = reader.ReadString().ToFurcadiaShortName();

            // Dim Furre As String = MSpage.GetVariable("~Name").Value.ToString()
            var value = reader.ReadString();
            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "date modified", DateTime.Now.ToLongDateString() },
                { "Name",$"{Furre}"},
                { $"{info}",$"{value}"}
            };

            return db.Update("FURRE", data, $"[Name]='{Furre}'");
        }

        /// <summary>
        /// (5407) update Database info {...} about the triggering furre
        /// will now be #.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
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

        /// <summary>
        /// (5:409) update Database info {...} about the triggering furre
        /// will now be {...}.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool UpdateTriggeringFurreFieldSTR(TriggerReader reader)
        {
            var info = reader.ReadString();

            // Dim Furre As String = reader.ReadString()
            var Furre = reader.Page.GetVariable(TriggeringFurreShortNameVariable).Value.ToString();

            var value = reader.ReadString();
            var db = new SQLiteDatabase(SQLitefile);
            var data = new Dictionary<string, string>
            {
                { "Name", Player.ShortName},
                { info, value.ToString() },
                { "date modified" ,DateTime.Now.ToShortDateString()}
            };
            return db.Update("FURRE", data, "[Name]='" + Furre + "'");
        }

        /// <summary>
        /// (5:500) use SQLite database file {...} or create file if it does
        /// not exist.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool UseOrCreateSQLiteFileIfNotExist(TriggerReader reader)
        {
            SQLitefile = Paths.CheckBotFolder(reader.ReadString());
            Monkeyspeak.Logging.Logger.Warn<MsDatabase>($"NOTICE: SQLite Database file has changed to {SQLitefile}");
            return true;
        }

        /// <summary>
        /// (5:559) execute ""VACUUM""to rebuild the database and reclaim
        /// wasted space.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// true on success
        /// </returns>
        private bool VACUUM(TriggerReader reader)
        {
            DateTime startDate = DateTime.Now;

            var rows = SQLiteDatabase.ExecuteNonQuery("VACUUM");
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