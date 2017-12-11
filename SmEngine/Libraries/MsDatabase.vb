﻿Imports System.Data.SQLite
Imports System.IO
Imports MonkeyCore
Imports MonkeyCore.Utils.Logging
Imports Monkeyspeak
Imports SilverMonkeyEngine.Engine.Libraries.MsLibHelper

Namespace Engine.Libraries

    ''' <summary>
    ''' SQLite Database Access... Create tables Store records ect. in Silver Monkey
    ''' <para>
    ''' To view and edit these tables manually Please look as Data Monkey
    ''' </para>
    ''' <para>
    ''' Conditions (1:500) (1:531)
    ''' </para>
    ''' <para>
    ''' Effects: (5:500) - (5:559)
    ''' </para>
    ''' <pra> Bot Testers: Be aware this class needs to be tested any way
    ''' possible! </pra>
    ''' <para>
    ''' Default SQLite database file: <see cref="Paths.SilverMonkeyBotPath"/>\SilverMonkey.db
    ''' </para>
    ''' <para>
    ''' NOTE: PhoenixSpeak Database is not SQL based like SQLite. Phoenix
    '''       Speak resembles an XML style system
    ''' </para>
    ''' </summary>
    Public NotInheritable Class MsDatabase
        Inherits MonkeySpeakLibrary

#Region "Public Fields"

        ''' <summary>
        ''' </summary>
        Public Shared SQLreader As SQLiteDataReader = Nothing

#End Region

#Region "Private Fields"

        ''' <summary>
        ''' Shared(Static) Database file
        ''' </summary>
        Private Shared _SQLitefile As String

        Private cache As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
        Private lock As New Object
        Private QueryRun As Boolean = False

#End Region

#Region "Public Constructors"

        ''' <summary>
        ''' Default constructor initilizing the Monkey Speak lines with
        ''' reference to BotSession
        ''' </summary>
        ''' <param name="Session">
        ''' </param>
        Public Sub New(ByRef Session As BotSession)
            MyBase.New(Session)
            SQLitefile = Paths.CheckBotFolder("SilverMonkey.db")
        End Sub

        Public Overrides Sub Initialize(ParamArray args() As Object)
            '(1:500) and the Database info {...} about the triggering furre is equal to #,
            Add(TriggerCategory.Condition, 500,
                AddressOf TriggeringFurreinfoEqualToNumber,
                " and the Database info {...} about the triggering furre is equal to #,")

            '(1:501) and the Database info {...} about the triggering furre is not equal to #,
            Add(TriggerCategory.Condition, 501,
            AddressOf TriggeringFurreinfoNotEqualToNumber,
                " and the Database info {...} about the triggering furre is not equal to #,")

            '(1:502) and the Database info {...} about the triggering furre is greater than #,
            Add(TriggerCategory.Condition, 502,
                AddressOf TriggeringFurreinfoGreaterThanNumber,
                " and the Database info {...} about the triggering furre is greater than #,")
            '(1:503) and the Database info {...} about the triggering furre is less than #,
            Add(TriggerCategory.Condition, 503,
            AddressOf TriggeringFurreinfoLessThanNumber,
                " and the Database info {...} about the triggering furre is less than #,")

            '(1:504) and the Database info {...} about the triggering furre is greater than or equal to #,
            Add(TriggerCategory.Condition, 504,
            AddressOf TriggeringFurreinfoGreaterThanOrEqualToNumber,
                " and the Database info {...} about the triggering furre is greater than or equal to #,")

            '(1:505) and the Database info {...} about the triggering furre is less than or equal to#,
            Add(TriggerCategory.Condition, 505,
            AddressOf TriggeringFurreinfoLessThanOrEqualToNumber,
                " and the Database info {...} about the triggering furre is less than or equal to #,")

            '(1:508) and the Database info {...} about the furre named {...} is equal to #,
            Add(TriggerCategory.Condition, 508,
            AddressOf FurreNamedinfoEqualToNumber, " and the Database info {...} about the furre named {...} is equal to #,")

            '(1:509) and the Database info {...} about the furre named {...} is not equal to #,
            Add(TriggerCategory.Condition, 509,
            AddressOf FurreNamedinfoNotEqualToNumber,
                " and the Database info {...} about the furre named {...} is not equal to #,")

            '(1:510) and the Database info {...} about the furre named {...} is greater than #,
            Add(TriggerCategory.Condition, 510,
            AddressOf FurreNamedinfoGreaterThanNumber,
                " and the Database info {...} about the furre named {...} is greater than #,")

            '(1:511) and the Database info {...} about the furre named {...} is less than #,
            Add(TriggerCategory.Condition, 511,
            AddressOf FurreNamedinfoLessThanNumber,
                " and the Database info {...} about the furre named {...} is less than #,")

            '(1:510) and the Database info {...} about the furre named {...} is greater than or equal to #,
            Add(TriggerCategory.Condition, 512,
        AddressOf FurreNamedinfoGreaterThanOrEqualToNumber,
                " and the Database info {...} about the furre named {...} is greater than or equal to #,")
            '(1:511) and the Database info {...} about the furre named {...} is less than or equal to #,
            Add(TriggerCategory.Condition, 513,
        AddressOf FurreNamedinfoLessThanOrEqualToNumber,
                " and the Database info {...} about the furre named {...} is less than or equal to #,")

            '(1:516) and the Database info {...} about the furre named {...} is equal to {...},
            Add(TriggerCategory.Condition, 516,
                AddressOf FurreNamedinfoEqualToSTR,
                " and the Database info {...} about the furre named {...} is equal to string {...},")

            '(1:517) and the Database info {...} about the furre named {...} is not equal to {...},
            Add(TriggerCategory.Condition, 517,
        AddressOf FurreNamedinfoNotEqualToSTR,
                " and the Database info {...} about the furre named {...} is not equal to string {...},")

            '(1:518) and the Database info {...} about the triggering furre is equal to {...},
            Add(TriggerCategory.Condition, 518,
                AddressOf TriggeringFurreinfoEqualToSTR,
                " and the Database info {...} about the triggering furre is equal to string {...},")

            '(1:519) and the Database info {...} about the triggering furre is not equal to {...},
            Add(TriggerCategory.Condition, 519,
            AddressOf TriggeringFurreinfoNotEqualToSTR,
                " and the Database info {...} about the triggering furre is not equal to string {...},")

            'Installed 7/13/120`16
            '(1:524) and the Database info  {...} in Settings Table {...} exists,
            Add(TriggerCategory.Condition, 524,
                 AddressOf SettingExist,
                " and the Database info  {...} in Settings Table {...} exists,")

            '(1:525) and the Database info  {...} in Settings Table {...} doesn't exist,
            Add(TriggerCategory.Condition, 525,
                 AddressOf SettingNotExist,
                " and the Database info  {...} in Settings Table {...} doesn't exist,")

            '(1:526) and the Database info {..} in Settings Table  {...} Is equal to {...},
            Add(TriggerCategory.Condition, 526,
                 AddressOf SettingEqualTo, " and the Database info {..} in Settings Table  {...} Is equal to {...},")

            '(1:527) and the Database info {..} in Settings Table  {...} Is Not equal to {...},
            Add(TriggerCategory.Condition, 527,
                 AddressOf SettingNotEqualTo,
                " and the Database info {..} in Settings Table  {...} Is not equal to {...},")

            '(1:528) and the Database info {..} in Settings Table  {...} Is greater than #,
            Add(TriggerCategory.Condition, 528,
                 AddressOf SettingGreaterThan,
                " and the Database info {..} in Settings Table  {...} Is greater than #,")

            '(1:529) and the Database info {..} in Settings Table  {...} Is greater than or equal to #,
            Add(TriggerCategory.Condition, 529,
                 AddressOf SettingGreaterThanOrEqualTo,
                " and the Database info {..} in Settings Table  {...} Is greater than or equal to #,")

            '(1:530) and the Database info {..} in Settings Table  {...} Is less than #,
            Add(TriggerCategory.Condition, 530,
                 AddressOf SettingLessThan,
                " and the Database info {..} in Settings Table  {...} Is less than #,")

            '(1:530) and the Database info {..} in Settings Table  {...} Is less than #,
            Add(TriggerCategory.Condition, 531,
                 AddressOf SettingLessThanOrEqualTo,
                " and the Database info {..} in Settings Table  {...} Is less than or equal to #,")

            '(5:500) use SQLite database file {...} or create file if it does not exist.
            Add(TriggerCategory.Effect, 500, AddressOf UseOrCreateSQLiteFileIfNotExist,
                " use SQLite database file {...} or create file if it does not exist.")

            '(5:505 ) Add the triggering furre with the default access level 0 to the Furre Table in the database if he, she or it don't exist.
            Add(TriggerCategory.Effect, 505, AddressOf InsertTriggeringFurreRecord,
                " add the triggering furre with the default access level ""0"" to the Furre Table in the database if he, she, or it doesn't exist.")

            '(5:506) Add furre named {...} with the default access level 0 to the Furre Table in the database if he, she or it don't exist.
            Add(TriggerCategory.Effect, 506, AddressOf InsertFurreNamed, " add furre named {...} with the default access level ""0"" to the Furre Table in the database if he, she, or it doesn't exist.")

            '(5:507) update Database info {...} about the triggering furre will now be #.
            Add(TriggerCategory.Effect, 507, AddressOf UpdateTriggeringFurreField, " update Database info {...} about the triggering furre will now be #.")
            '(5:508) update Database info {...} about the furre named {...} will now be #.
            Add(TriggerCategory.Effect, 508, AddressOf UpdateFurreNamed_Field, " update Database info {...} about the furre named {...} will now be #.")
            '(5:509) update Database info {...} about the triggering furre will now be {...}.
            Add(TriggerCategory.Effect, 509, AddressOf UpdateTriggeringFurreFieldSTR, " update Database info {...} about the triggering furre will now be {...}.")
            '(5:510) update Database info {...} about the furre named {...} will now be {...}.
            Add(TriggerCategory.Effect, 510, AddressOf UpdateFurreNamed_FieldSTR, " update Database info {...} about the furre named {...} will now be {...}.")

            '(5:511) select Database info {...} about the triggering furre, and put it in variable %.
            Add(TriggerCategory.Effect, 511, AddressOf ReadDatabaseInfo, " select Database info {...} about the triggering furre, and put it in variable %.")
            '(5:512) select Database info {...} about the furre named {...}, and put it in variable %.
            Add(TriggerCategory.Effect, 512, AddressOf ReadDatabaseInfoName, " select Database info {...} about the furre named {...}, and put it in variable %.")

            '(5:513) add column {...} with type {...} to the Furre table.
            Add(TriggerCategory.Effect, 513, AddressOf AddColumn,
                " add column {...} with type {...} to the Furre table.")

            '(5:518) delete all Database info about the triggering furre.
            Add(TriggerCategory.Effect, 518, AddressOf DeleteTriggeringFurre, " delete all Database info about the triggering furre.")
            '(5:519) delete all Database info about the furre named {...}.
            Add(TriggerCategory.Effect, 519, AddressOf DeleteFurreNamed, " delete all Database info about the furre named {...}.")

            '(5:522) get the total of records from table {...} and put it into variable %.
            Add(TriggerCategory.Effect, 522, AddressOf GetTotalRecords, " get the total number of records from table {...} and put it into variable %Variable.")

            '(5:523) take the sum of column{...} in table {...} and put it into variable %
            Add(TriggerCategory.Effect, 523, AddressOf ColumnSum, " take the sum of column{...} in table {...} and put it into variable %Variable.")

            '(5:550) take variable %Variable , prepare it for a query, and put it in variable %Variable .   (this is your escaping call, which would depend on however you have to do it internally)
            Add(TriggerCategory.Effect, 550, AddressOf PrepQuery,
                " take variable %Variable , prepare it for a SQLite Database query, and put it in variable %Variable.")

            '(5:551) execute SQLite Database query {...} Select * from table where name=%2
            Add(TriggerCategory.Effect, 551, AddressOf ExecuteQuery,
                 " execute SQLite Database query {...}.")

            '(5:552) retrieve field {...} from SQLite Database query and put it into variable %Variable .
            Add(TriggerCategory.Effect, 552, AddressOf RetrieveQuery,
                " retrieve field {...} from SQLite Database query and put it into variable %Variable.")

            Add(TriggerCategory.Effect, 559, AddressOf VACUUM,
                " execute ""VACUUM"" to rebuild the database and reclaim wasted space.")

            '(5:561) remember Database Info {...} for Settings Table {...} to {...}.
            '(5:562) forget Database info {...} from Settings Table{...}.
            '(5:563) forget all Settings Table Database info.

            Add(TriggerCategory.Effect, 560, AddressOf InsertVariableTableToFurreTable, "store variable table %VariableTable to database table name {...}.")

        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Currenly used database filfe
        ''' </summary>
        ''' <returns>
        ''' SQLite database file with Silver Monkey system tables and user data
        ''' </returns>
        Public Shared Property SQLitefile As String
            Get
                If String.IsNullOrEmpty(_SQLitefile) Then
                    _SQLitefile = Path.Combine(Paths.SilverMonkeyBotPath, "SilverMonkey.db")
                End If
                Return _SQLitefile
            End Get
            Set(value As String)
                _SQLitefile = Paths.CheckBotFolder(value)
            End Set
        End Property

#End Region

#Region "Condition Functions"

        '
        ''' <summary>
        ''' (1:508) and the Database info {...} about the furre named {...}
        ''' is equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True if there is no error, otherwise false and stops further
        ''' conditions or effects processing in the currnt block
        ''' </returns>
        ''' <remarks>
        ''' </remarks>
        Public Function FurreNamedinfoEqualToNumber(reader As TriggerReader) As Boolean
            Dim info = reader.ReadString
            Dim Furre = reader.ReadString().ToFurcadiaShortName()
            Dim Variable = ReadVariableOrNumber(reader, False)
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, Furre).ToString, Value)
            Return Variable = Value

        End Function

        ''' <summary>
        ''' (1:516) and the Database info {...} about the furre named {...}
        ''' is equal to string {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedinfoEqualToSTR(reader As TriggerReader) As Boolean
            Dim Info As String = reader.ReadString
            Dim Furre As String = reader.ReadString().ToFurcadiaShortName()
            Dim str As String = reader.ReadString

            Return str = GetValueFromTable(Info, Furre).ToString

        End Function

        ''' <summary>
        ''' (1:510) and the Database info {...} about the furre named {...}
        ''' is greater than #, greater than #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedinfoGreaterThanNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Furre = reader.ReadString().ToFurcadiaShortName()
            Dim Variable = ReadVariableOrNumber(reader, False)
            Dim check = GetValueFromTable(info, Furre)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value > Variable

        End Function

        ''' <summary>
        ''' (1:512) and the Database info {...} about the furre named {...}
        ''' is greater than or equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedinfoGreaterThanOrEqualToNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Furre = reader.ReadString().ToFurcadiaShortName()
            Dim Variable = ReadVariableOrNumber(reader, False)
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, Furre).ToString, Value)
            Return Value >= Variable

        End Function

        ''' <summary>
        ''' (1:511) and the Database info {...} about the furre named {...}
        ''' is less than #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedinfoLessThanNumber(reader As TriggerReader) As Boolean
            Dim info = reader.ReadString
            Dim FurreName = reader.ReadString().ToFurcadiaShortName()

            Dim Variable = ReadVariableOrNumber(reader, False)
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, FurreName).ToString, Value)
            Return Value < Variable

        End Function

        ''' <summary>
        ''' (1:513) and the Database info {...} about the furre named {...}
        ''' is less than or equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedinfoLessThanOrEqualToNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Furre = reader.ReadString().ToFurcadiaShortName()
            Dim Variable = ReadVariableOrNumber(reader, False)
            Dim check As Object = GetValueFromTable(info, Furre)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value <= Variable

        End Function

        ''' <summary>
        ''' (1:509) and the Database info {...} about the furre named {...}
        ''' is not equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedinfoNotEqualToNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Furre = reader.ReadString().ToFurcadiaShortName()

            Dim Variable = ReadVariableOrNumber(reader, False)
            Dim check As Object = GetValueFromTable(info, Furre)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value <> Variable

        End Function

        ''' <summary>
        ''' (1:517) and the Database info {...} about the furre named {...}
        ''' is not equal to string {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function FurreNamedinfoNotEqualToSTR(reader As TriggerReader) As Boolean
            Dim Info = reader.ReadString
            Dim Furre = reader.ReadString().ToFurcadiaShortName()

            Dim str = reader.ReadString

            Return str <> GetValueFromTable(Info, Furre).ToString

        End Function

        ''' <summary>
        ''' (1:526) and the Database info {..} in Settings Table {...} Is
        ''' equal to {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SettingEqualTo(reader As TriggerReader) As Boolean
            Dim Info = reader.ReadString(True)
            Dim Table = reader.ReadString(True)
            Dim Value = reader.ReadString(True)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.[" + Table + "] from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Info + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + Info + "' "

            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            cache = db.GetValueFromTable(cmd)
            QueryRun = True

            Return cache.Item(Info).ToString = Value

        End Function

        ''' <summary>
        ''' (1:524) and the Database info {...} in Settings Table {...} exists,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SettingExist(reader As TriggerReader) As Boolean
            Dim Info = reader.ReadString(True)
            Dim setting = reader.ReadString(True)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.ID from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Info + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + setting + "' "

            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            cache = db.GetValueFromTable(cmd)
            QueryRun = True
            Return cache.Count > 0

        End Function

        ''' <summary>
        ''' (1:528) and the Database info {..} in Settings Table {...} Is
        ''' greater than #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SettingGreaterThan(reader As TriggerReader) As Boolean
            Dim Setting = reader.ReadString(True)
            Dim Table = reader.ReadString(True)
            Dim Number = ReadVariableOrNumber(reader)
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.[" + Table + "] from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Setting + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + Setting + "' "

            cache = db.GetValueFromTable(cmd)
            QueryRun = True
            Dim num As Double = 0
            Double.TryParse(cache.Item(Setting).ToString, num)
            Return num > Number

        End Function

        ''' <summary>
        ''' (1:529) and the Database info {..} in Settings Table {...} Is
        ''' greater than or equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SettingGreaterThanOrEqualTo(reader As TriggerReader) As Boolean
            Dim Setting = reader.ReadString(True)
            Dim Table = reader.ReadString(True)
            Dim Number = ReadVariableOrNumber(reader)
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.[" + Table + "] from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Setting + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + Setting + "' "

            cache = db.GetValueFromTable(cmd)
            QueryRun = True
            Dim num As Double = 0
            Double.TryParse(cache.Item(Setting).ToString, num)
            Return num >= Number

        End Function

        ''' <summary>
        ''' (1:530) and the Database info {..} in Settings Table {...} Is
        ''' less than #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SettingLessThan(reader As TriggerReader) As Boolean
            Dim Setting = reader.ReadString(True)
            Dim Table = reader.ReadString(True)
            Dim Number = ReadVariableOrNumber(reader)
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.[" + Table + "] from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Setting + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + Setting + "' "

            cache = db.GetValueFromTable(cmd)
            QueryRun = True
            Dim num As Double = 0
            Double.TryParse(cache.Item(Setting).ToString, num)
            Return num < Number

        End Function

        ''' <summary>
        ''' (1:531) and the Database info {..} in Settings Table {...} Is
        ''' less than or equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SettingLessThanOrEqualTo(reader As TriggerReader) As Boolean
            Dim Setting = reader.ReadString(True)
            Dim Table = reader.ReadString(True)
            Dim Number = ReadVariableOrNumber(reader)
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.[" + Table + "] from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Setting + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + Setting + "' "

            cache = db.GetValueFromTable(cmd)
            QueryRun = True
            Dim num As Double = 0
            Double.TryParse(cache.Item(Setting).ToString, num)
            Return num <= Number

        End Function

        ''' <summary>
        ''' (1:527) and the Database info {..} in Settings Table {...} Is
        ''' not equal to {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SettingNotEqualTo(reader As TriggerReader) As Boolean
            Dim Info = reader.ReadString(True)
            Dim Table = reader.ReadString(True)
            Dim Value = reader.ReadString(True)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.[" + Table + "] from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Info + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + Info + "' "

            Dim db = New SQLiteDatabase(SQLitefile)
            cache = db.GetValueFromTable(cmd)
            QueryRun = True
            Return cache.Item(Info).ToString <> Value

        End Function

        ''' <summary>
        ''' (1:525) and the Database info {...} in Settings Table {...}
        ''' doesn't exist, doesn't exist,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function SettingNotExist(reader As TriggerReader) As Boolean
            Dim Info = reader.ReadString(True)
            Dim setting = reader.ReadString(True)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.ID from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Info + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + setting + "' "

            Dim db = New SQLiteDatabase(SQLitefile)
            cache = db.GetValueFromTable(cmd)
            QueryRun = True
            Return cache.Count = 0

        End Function

        ''' <summary>
        ''' (1:500) and the Database info {...} about the triggering furre
        ''' is equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoEqualToNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Number = ReadVariableOrNumber(reader, False)
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, Player.ShortName).ToString, Value)

            Return Number = Value

        End Function

        ''' <summary>
        ''' " and the Database info {...} about the triggering furre
        ''' is equal to string {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoEqualToSTR(reader As TriggerReader) As Boolean
            Dim Info = reader.ReadString
            Dim str = reader.ReadString

            If str = GetValueFromTable(Info, Player.ShortName).ToString Then Return True
            Return False
        End Function

        ''' <summary>
        ''' (1:502) and the Database info {...} about the triggering furre
        ''' is greater than #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoGreaterThanNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Number = ReadVariableOrNumber(reader, False)
            Dim check As Object = GetValueFromTable(info, Player.ShortName)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value > Number

        End Function

        ''' <summary>
        ''' (1:504) and the Database info {...} about the triggering furre
        ''' is greater than or equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoGreaterThanOrEqualToNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Number = ReadVariableOrNumber(reader, False)
            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Player.ShortName)
            Double.TryParse(check.ToString, Num)
            Return Num >= Number

        End Function

        ''' <summary>
        ''' (1:503) and the Database info {...} about the triggering furre
        ''' is less than #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoLessThanNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Number = ReadVariableOrNumber(reader, False)

            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Player.ShortName)
            Double.TryParse(check.ToString, Num)

            Return Num < Number

        End Function

        ''' <summary>
        ''' (1:505) and the Database info {...} about the triggering furre
        ''' is less than or equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoLessThanOrEqualToNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Number = ReadVariableOrNumber(reader, False)
            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Player.ShortName)
            Double.TryParse(check.ToString, Num)
            Return Num <= Number

        End Function

        ''' <summary>
        ''' (1:501) and the Database info {...} about the triggering furre
        ''' is not equal to #,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoNotEqualToNumber(reader As TriggerReader) As Boolean

            Dim info = reader.ReadString
            Dim Number = ReadVariableOrNumber(reader, False)
            Dim val As String = GetValueFromTable(info, Player.ShortName).ToString
            Dim Value As Double = 0
            Double.TryParse(val, Value)
            Return Value <> Number

        End Function

        ''' <summary>
        ''' (1:519) and the Database info {...} about the triggering furre
        ''' is not equal to string {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoNotEqualToSTR(reader As TriggerReader) As Boolean
            Dim Info = reader.ReadString

            Dim str = reader.ReadString

            If str <> GetValueFromTable(Info, Player.ShortName).ToString Then Return True

            Return False
        End Function

#End Region

#Region "Condition Helper Functions"

        ''' <summary>
        ''' Gets information from the Furre Table for the specified furre
        ''' </summary>
        ''' <param name="Column">
        ''' Collunm Name as string
        ''' </param>
        ''' <param name="Name">
        ''' Furre Name as string
        ''' </param>
        ''' <returns>
        ''' the Value for the specified Collumn for the specified furre
        ''' </returns>
        Private Function GetValueFromTable(Column As String, ByRef Name As String) As Object
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim str As String = "SELECT * FROM FURRE WHERE Name='" & Name & "';"
            SyncLock lock
                cache = db.GetValueFromTable(str)
            End SyncLock
            QueryRun = True
            Try
                If Not IsNothing(cache) Then
                    If cache.ContainsKey(Column) Then
                        Return cache.Item(Column)
                    End If
                End If
            Catch ex As Exception
                Dim err As New ErrorLogging(ex, Me)
            End Try
            Return ""
        End Function

#End Region

#Region "Effect Functions"

        ''' <summary>
        ''' (5:513) add column {...} with type {...} to the Furre table.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function AddColumn(reader As TriggerReader) As Boolean
            Dim Column = reader.ReadString
            Dim Type = reader.ReadString
            Dim db = New SQLiteDatabase(SQLitefile)
            db.addColumn("FURRE", "[" & Column & "]", Type)
            Return True
        End Function

        ''' <summary>
        ''' (5:423) take the sum of column{...} in table {...} and put it
        ''' into variable %
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function ColumnSum(reader As TriggerReader) As Boolean

            Dim Column = reader.ReadString
            Dim Table = reader.ReadString
            Dim Total = reader.ReadVariable(True)

            Dim sql As String = "SELECT " & Column & " FROM " & Table & " ;"
            Dim dt = SQLiteDatabase.GetDataTable(sql)
            Column = Column.Replace("[", "")
            Column = Column.Replace("]", "")
            Dim suma As Double = 0.0R
            For Each row As DataRow In dt.Rows
                Dim num As Double = 0R
                Double.TryParse(row(Column).ToString, num)
                suma += num
            Next row
            'Console.WriteLine("Calculating TotalSum {0}", TotalSum.ToString)
            Total.Value = suma
            Return True
        End Function

        ''' <summary>
        ''' (5:419) delete all Database info about the furre named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function DeleteFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre = reader.ReadString().ToFurcadiaShortName()
            Dim db = New SQLiteDatabase(SQLitefile)
            Return 0 < SQLiteDatabase.ExecuteNonQuery("Delete from FURRE where Name='" & Furre & "'")

        End Function

        ''' <summary>
        ''' (5:418) delete all Database info about the triggering furre.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function DeleteTriggeringFurre(reader As TriggerReader) As Boolean

            Dim db = New SQLiteDatabase(SQLitefile)
            Return 0 < SQLiteDatabase.ExecuteNonQuery($"Delete from FURRE where Name='{Player.ShortName}'")

        End Function

        ''' <summary>
        ''' (5:551) execute query {...}.
        ''' <para>
        ''' Execute raw SQL commands on the database.
        ''' </para>
        ''' <para>
        ''' For SELECT statements <see cref="RetrieveQuery"/>
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function ExecuteQuery(reader As TriggerReader) As Boolean

            Dim Str = reader.ReadString
            Str = Str.Trim
            SyncLock (lock)
                cache.Clear()
                QueryRun = False
                If Str.ToUpper.StartsWith("SELECT") Then
                    Dim db = New SQLiteDatabase(SQLitefile)

                    cache = db.GetValueFromTable(Str)
                    QueryRun = True

                    Return cache.Count > 0
                End If
                SQLiteDatabase.ExecuteNonQuery(Str)
                Return True
            End SyncLock

        End Function

        ''' <summary>
        ''' (5:422) get the total number of records from table {...} and put
        ''' it into variable %.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function GetTotalRecords(reader As TriggerReader) As Boolean

            Dim db = New SQLiteDatabase(SQLitefile)
            Dim Table = reader.ReadString().Replace("[", "").Replace("]", "").Replace("'", "''")
            Dim Total = reader.ReadVariable(True)
            Dim count = SQLiteDatabase.ExecuteScalar("select count(*) from [" & Table & "]")
            Total.Value = count
            Return True

        End Function

        ''' <summary>
        ''' (5:506) add furre named {%NewMember} with the default access
        ''' level "1" to the Furre Table in the database if he, she, or it
        ''' doesn't exist.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function InsertFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = reader.ReadString().ToFurcadiaShortName()
            Dim info As String
            If reader.PeekString Then
                info = reader.ReadString
            Else
                info = ReadVariableOrNumber(reader).ToString()
            End If
            'Dim value As String = reader.ReadVariable.Value.ToString
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim data = New Dictionary(Of String, String) From {
                {"Name", Furre},
                {"date added", Date.Now.ToString},
                {"date modified", Date.Now.ToString},
                {"Access Level", info}
            }
            Return db.Insert("FURRE", data)

        End Function

        ''' <summary>
        ''' store variable table %VariableTable to database table name {...}.
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        Public Function InsertVariableTableToFurreTable(reader As TriggerReader) As Boolean
            Dim VarTable = reader.ReadVariableTable(True)
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim TableName = reader.ReadString()
            Return db.Insert(TableName, VarTable.Values)
        End Function

        ''' <summary>
        ''' (5:405) Add the triggering furre with default access level to
        ''' the Furre Table in the database if he, she or it don't already exist.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function InsertTriggeringFurreRecord(reader As TriggerReader) As Boolean

            Dim value As String = "0"
            If reader.PeekNumber Or reader.PeekVariable Then
                value = ReadVariableOrNumber(reader).ToString()
            ElseIf reader.PeekString Then
                value = reader.ReadString
                Double.TryParse(value, 0)
            End If
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            Dim data As New Dictionary(Of String, String) From {
                {"Name", Player.ShortName},
                {"date added", Date.Now.ToString},
                {"date modified", Date.Now.ToString},
                {"Access Level", value}
            }

            Return db.Insert("FURRE", data)

        End Function

        ''' <summary>
        ''' (5:550) take variable %Variable , prepare it for a query, and
        ''' put it in variable %Variable (this is your escaping call, which
        ''' would depend on however you have to do it internally)
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function PrepQuery(reader As TriggerReader) As Boolean
            Dim var1 = reader.ReadVariable(True)
            Dim var2 = reader.ReadVariable(True)
            Dim str = var1.Value.ToString
            str = str.Replace("'", "''")
            var2.Value = str
            Return True

        End Function

        ''' <summary>
        ''' (5:411) select Database info {...} about the triggering furre,
        ''' and put it in variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function ReadDatabaseInfo(reader As TriggerReader) As Boolean

            Dim Info = reader.ReadString
            Dim Variable = reader.ReadVariable(True)
            Dim Furre = reader.Page.GetVariable(ShortNameVariable).Value.ToString()

            Dim cmd As String = "SELECT [" & Info & "] FROM FURRE Where [Name]='" & Furre & "'"
            Variable.Value = SQLiteDatabase.ExecuteScalar(cmd)
            Return True

        End Function

        ''' <summary>
        ''' (5:412) select Database info {...} about the furre named {...},
        ''' and put it in variable %Variable.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function ReadDatabaseInfoName(reader As TriggerReader) As Boolean

            Dim db = New SQLiteDatabase(SQLitefile)
            Dim Info = reader.ReadString
            Dim Furre = reader.ReadString().ToFurcadiaShortName()
            Dim Variable = reader.ReadVariable(True)
            ' Dim db As SQLiteDatabase = New SQLiteDatabase(file)
            Dim cmd As String = "SELECT [" & Info & "] FROM FURRE Where [Name]='" & Furre & "'"
            Variable.Value = SQLiteDatabase.ExecuteScalar(cmd)
            Return True

        End Function

        ''' <summary>
        ''' (5:424) in table {...} take info {...} from record index % and
        ''' and put it into variable %
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function RecordIndex(reader As TriggerReader) As Boolean

            Dim Table = reader.ReadString(True).Replace("[", "").Replace("]", "").Replace("'", "''")
            Dim info = reader.ReadString(True)
            Dim Idx = reader.ReadVariable(True)
            Dim OutVar = reader.ReadVariable(True)

            Dim sql As String = "SELECT " & info & " FROM [" & Table & "] ;"
            Dim dt = SQLiteDatabase.GetDataTable(sql)
            info = info.Replace("[", "").Replace("]", "")
            Dim i As Double = 0
            For Each row As DataRow In dt.Rows
                Try
                    If i = Double.Parse(Idx.Value.ToString) Then
                        OutVar.Value = row(info)
                        Return True
                    End If
                    i += 1
                Catch
                End Try
            Next row
            Return True
        End Function

        ''' <summary>
        ''' (5:552) retrieve field {...} from query and put it into variable %Variable
        ''' <para>
        ''' <see cref="ExecuteQuery"/>
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function RetrieveQuery(reader As TriggerReader) As Boolean
            Dim Field = reader.ReadString
            Dim Var = reader.ReadVariable(True)

            If QueryRun Then
                SyncLock (lock)
                    If cache.Count > 0 Then

                        For Each key In cache
                            If key.Key = Field Then
                                Var.Value = cache.Item(Field)
                                Return True
                            End If
                        Next

                    End If
                End SyncLock
            End If
            Return True

        End Function

        ''' <summary>
        ''' (5:408) update Database info {...} about the furre named {...}
        ''' will now be #.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function UpdateFurreNamed_Field(reader As TriggerReader) As Boolean
            Dim info = reader.ReadString
            Dim Furre = reader.ReadString
            Dim value = ReadVariableOrNumber(reader, False).ToString
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim data As New Dictionary(Of String, String) From {
                {"Name", Furre.ToFurcadiaShortName()},
                {info, value},
                {"date modified", Date.Now.ToString}
            }

            Return db.Update("FURRE", data, "[Name]='" & Furre & "'")

        End Function

        ''' <summary>
        ''' (5:410) update Database info {...} about the furre named {...}
        ''' will now be {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function UpdateFurreNamed_FieldSTR(reader As TriggerReader) As Boolean
            Dim info = reader.ReadString
            Dim Furre = reader.ReadString().ToFurcadiaShortName()
            'Dim Furre As String = MSpage.GetVariable("~Name").Value.ToString
            Dim value = reader.ReadString
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim data = New Dictionary(Of String, String) From {
                {"Name", Furre},
                {info, value},
                {"date modified", Date.Now.ToString}
            }

            Return db.Update("FURRE", data, "[Name]='" & Furre & "'")

        End Function

        ''' <summary>
        ''' (5407) update Database info {...} about the triggering furre
        ''' will now be #.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function UpdateTriggeringFurreField(reader As TriggerReader) As Boolean
            Dim info = reader.ReadString

            Dim Furre = reader.Page.GetVariable(ShortNameVariable).Value.ToString()
            Dim value = ReadVariableOrNumber(reader)
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim data = New Dictionary(Of String, String) From {
                {"Name", Furre},
                {info, value.ToString},
                {"date modified", Date.Now.ToString}
            }

            Return db.Update("FURRE", data, "[Name]='" & Furre & "'")
        End Function

        ''' <summary>
        ''' (5:409) update Database info {...} about the triggering furre
        ''' will now be {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function UpdateTriggeringFurreFieldSTR(reader As TriggerReader) As Boolean
            Dim info = reader.ReadString
            'Dim Furre As String = reader.ReadString
            Dim Furre = reader.Page.GetVariable(ShortNameVariable).Value.ToString
            Dim value = reader.ReadString
            Dim db = New SQLiteDatabase(SQLitefile)
            Dim data = New Dictionary(Of String, String) From {
                {"Name", Furre},
                {info, value},
                {"date modified", Date.Now.ToString}
            }

            Return db.Update("FURRE", data, "[Name]='" & Furre & "'")

        End Function

        ''' <summary>
        ''' (5:500) use SQLite database file {...} or create file if it does
        ''' not exist.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function UseOrCreateSQLiteFileIfNotExist(reader As TriggerReader) As Boolean
            SQLitefile = Paths.CheckBotFolder(reader.ReadString())
            Logging.Logger.Warn(Of MsDatabase)($"NOTICE: SQLite Database file has changed to {SQLitefile}")
            Return True
        End Function

        ''' <summary>
        ''' (5:559) execute ""VACUUM"" to rebuild the database and reclaim
        ''' wasted space.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Shared Function VACUUM(reader As TriggerReader) As Boolean
            Dim startDate = Date.Now
            Dim rows = SQLiteDatabase.ExecuteNonQuery("VACUUM")
            Dim ts As TimeSpan = Date.Now.Subtract(startDate)
            Logging.Logger.Debug(Of MsDatabase)($"Executed ""VACUUM"" in {ts.Seconds.ToString} seconds, {rows} are affected")
            'TODO: Provide Database Stats for feedback
            Return True
        End Function

        Public Overrides Sub Unload(page As Page)

        End Sub

        '(5:561) remember Database Info {...} for Settings Table {...} to {...}.
        '(5:562) forget Database info {...} from Settings Table{...}.
        '(5:563) forget all Settings Table Database info.

#End Region

    End Class

End Namespace