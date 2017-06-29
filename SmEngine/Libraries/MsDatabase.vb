Imports System.Data.SQLite
Imports System.IO
Imports Furcadia.Util
Imports MonkeyCore
Imports Monkeyspeak
Imports SilverMonkeyEngine.SmConstants

Namespace Engine.Libraries

    ''' <summary>
    ''' Provides <see cref="System.Data.SQLite"/> access to Silver Monkey
    ''' <para>
    ''' Conditions (1:500) (1:531)
    ''' </para>
    ''' <para>
    ''' Effects: (5:500) - (5:559)
    ''' </para>
    ''' <para>
    ''' Default SQLite database file: <see cref="Paths.SilverMonkeyBotPath"/>\SilverMonkey.db
    ''' </para>
    ''' <para>
    ''' NOTE: PhoenixSpeak Database is not SQL based like SQLite. Phoenix
    '''       Speak resembles an XML style system
    ''' </para>
    ''' </summary>
    Public Class MsDatabase
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
            SQLitefile = Path.Combine(Paths.SilverMonkeyBotPath, "SilverMonkey.db")
            '(1:500) and the Database info {...} about the triggering furre is equal to #,
            Add(New Trigger(TriggerCategory.Condition, 500),
                AddressOf TriggeringFurreinfoEqualToNumber,
                "(1:500) and the Database info {...} about the triggering furre is equal to #,")

            '(1:501) and the Database info {...} about the triggering furre is not equal to #,
            Add(New Trigger(TriggerCategory.Condition, 501),
            AddressOf TriggeringFurreinfoNotEqualToNumber,
                "(1:501) and the Database info {...} about the triggering furre is not equal to #,")

            '(1:502) and the Database info {...} about the triggering furre is greater than #,
            Add(New Trigger(TriggerCategory.Condition, 502),
                AddressOf TriggeringFurreinfoGreaterThanNumber,
                "(1:502) and the Database info {...} about the triggering furre is greater than #,")
            '(1:503) and the Database info {...} about the triggering furre is less than #,
            Add(New Trigger(TriggerCategory.Condition, 503),
            AddressOf TriggeringFurreinfoLessThanNumber,
                "(1:503) and the Database info {...} about the triggering furre is less than #,")

            '(1:504) and the Database info {...} about the triggering furre is greater than or equal to #,
            Add(New Trigger(TriggerCategory.Condition, 504),
            AddressOf TriggeringFurreinfoGreaterThanOrEqualToNumber,
                "(1:504) and the Database info {...} about the triggering furre is greater than or equal to #,")

            '(1:505) and the Database info {...} about the triggering furre is less than or equal to#,
            Add(New Trigger(TriggerCategory.Condition, 505),
            AddressOf TriggeringFurreinfoLessThanOrEqualToNumber,
                "(1:505) and the Database info {...} about the triggering furre is less than or equal to #,")

            '(1:508) and the Database info {...} about the furre named {...} is equal to #,
            Add(New Trigger(TriggerCategory.Condition, 508),
            AddressOf FurreNamedinfoEqualToNumber, "(1:508) and the Database info {...} about the furre named {...} is equal to #,")

            '(1:509) and the Database info {...} about the furre named {...} is not equal to #,
            Add(New Trigger(TriggerCategory.Condition, 509),
            AddressOf FurreNamedinfoNotEqualToNumber,
                "(1:509) and the Database info {...} about the furre named {...} is not equal to #,")

            '(1:510) and the Database info {...} about the furre named {...} is greater than #,
            Add(New Trigger(TriggerCategory.Condition, 510),
            AddressOf FurreNamedinfoGreaterThanNumber,
                "(1:510) and the Database info {...} about the furre named {...} is greater than #,")

            '(1:511) and the Database info {...} about the furre named {...} is less than #,
            Add(New Trigger(TriggerCategory.Condition, 511),
            AddressOf FurreNamedinfoLessThanNumber,
                "(1:511) and the Database info {...} about the furre named {...} is less than #,")

            '(1:510) and the Database info {...} about the furre named {...} is greater than or equal to #,
            Add(New Trigger(TriggerCategory.Condition, 512),
        AddressOf FurreNamedinfoGreaterThanOrEqualToNumber,
                "(1:512) and the Database info {...} about the furre named {...} is greater than or equal to #,")
            '(1:511) and the Database info {...} about the furre named {...} is less than or equal to #,
            Add(New Trigger(TriggerCategory.Condition, 513),
        AddressOf FurreNamedinfoLessThanOrEqualToNumber,
                "(1:513) and the Database info {...} about the furre named {...} is less than or equal to #,")

            '(1:516) and the Database info {...} about the furre named {...} is equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 516),
                AddressOf FurreNamedinfoEqualToSTR,
                "(1:516) and the Database info {...} about the furre named {...} is equal to string {...},")

            '(1:517) and the Database info {...} about the furre named {...} is not equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 517),
        AddressOf FurreNamedinfoNotEqualToSTR,
                "(1:517) and the Database info {...} about the furre named {...} is not equal to string {...},")

            '(1:518) and the Database info {...} about the triggering furre is equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 518),
                AddressOf TriggeringFurreinfoEqualToSTR,
                "(1:518) and the Database info {...} about the triggering furre is equal to string {...},")

            '(1:519) and the Database info {...} about the triggering furre is not equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 519),
            AddressOf TriggeringFurreinfoNotEqualToSTR,
                "(1:519) and the Database info {...} about the triggering furre is not equal to string {...},")

            'Installed 7/13/120`16
            '(1:524) and the Database info  {...} in Settings Table {...} exists,
            Add(New Trigger(TriggerCategory.Condition, 524),
                 AddressOf SettingExist,
                "(1:524) and the Database info  {...} in Settings Table {...} exists,")

            '(1:525) and the Database info  {...} in Settings Table {...} doesn't exist,
            Add(New Trigger(TriggerCategory.Condition, 525),
                 AddressOf SettingNotExist,
                "(1:525) and the Database info  {...} in Settings Table {...} doesn't exist,")

            '(1:526) and the Database info {..} in Settings Table  {...} Is equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 526),
                 AddressOf SettingEqualTo, "(1:526) and the Database info {..} in Settings Table  {...} Is equal to {...},")

            '(1:527) and the Database info {..} in Settings Table  {...} Is Not equal to {...},
            Add(New Trigger(TriggerCategory.Condition, 527),
                 AddressOf SettingNotEqualTo,
                "(1:527) and the Database info {..} in Settings Table  {...} Is not equal to {...},")

            '(1:528) and the Database info {..} in Settings Table  {...} Is greater than #,
            Add(New Trigger(TriggerCategory.Condition, 528),
                 AddressOf SettingGreaterThan,
                "(1:528) and the Database info {..} in Settings Table  {...} Is greater than #,")

            '(1:529) and the Database info {..} in Settings Table  {...} Is greater than or equal to #,
            Add(New Trigger(TriggerCategory.Condition, 529),
                 AddressOf SettingGreaterThanOrEqualTo,
                "(1:529) and the Database info {..} in Settings Table  {...} Is greater than or equal to #,")

            '(1:530) and the Database info {..} in Settings Table  {...} Is less than #,
            Add(New Trigger(TriggerCategory.Condition, 530),
                 AddressOf SettingLessThan,
                "(1:530) and the Database info {..} in Settings Table  {...} Is less than #,")

            '(1:530) and the Database info {..} in Settings Table  {...} Is less than #,
            Add(New Trigger(TriggerCategory.Condition, 531),
                 AddressOf SettingLessThanOrEqualTo,
                "(1:531) and the Database info {..} in Settings Table  {...} Is less than or equal to #,")

            '(5:500) use SQLite database file {...} or create file if it does not exist.
            Add(New Trigger(TriggerCategory.Effect, 500), AddressOf UseOrCreateSQLiteFileIfNotExist,
                "(5:500) use SQLite database file {...} or create file if it does not exist.")

            '(5:505 ) Add the triggering furre with the default access level 0 to the Furre Table in the database if he, she or it don't exist.
            Add(New Trigger(TriggerCategory.Effect, 505), AddressOf insertTriggeringFurreRecord,
                "(5:505) add the triggering furre with the default access level ""0"" to the Furre Table in the database if he, she, or it doesn't exist.")

            '(5:506) Add furre named {...} with the default access level 0 to the Furre Table in the database if he, she or it don't exist.
            Add(New Trigger(TriggerCategory.Effect, 506), AddressOf InsertFurreNamed, "(5:506) add furre named {...} with the default access level ""0"" to the Furre Table in the database if he, she, or it doesn't exist.")

            '(5:507) update Database info {...} about the triggering furre will now be #.
            Add(New Trigger(TriggerCategory.Effect, 507), AddressOf UpdateTriggeringFurreField, "(5:507) update Database info {...} about the triggering furre will now be #.")
            '(5:508) update Database info {...} about the furre named {...} will now be #.
            Add(New Trigger(TriggerCategory.Effect, 508), AddressOf UpdateFurreNamed_Field, "(5:508) update Database info {...} about the furre named {...} will now be #.")
            '(5:509) update Database info {...} about the triggering furre will now be {...}.
            Add(New Trigger(TriggerCategory.Effect, 509), AddressOf UpdateTriggeringFurreFieldSTR, "(5:509) update Database info {...} about the triggering furre will now be {...}.")
            '(5:510) update Database info {...} about the furre named {...} will now be {...}.
            Add(New Trigger(TriggerCategory.Effect, 510), AddressOf UpdateFurreNamed_FieldSTR, "(5:510) update Database info {...} about the furre named {...} will now be {...}.")

            '(5:511) select Database info {...} about the triggering furre, and put it in variable %.
            Add(New Trigger(TriggerCategory.Effect, 511), AddressOf ReadDatabaseInfo, "(5:511) select Database info {...} about the triggering furre, and put it in variable %.")
            '(5:512) select Database info {...} about the furre named {...}, and put it in variable %.
            Add(New Trigger(TriggerCategory.Effect, 512), AddressOf ReadDatabaseInfoName, "(5:512) select Database info {...} about the furre named {...}, and put it in variable %.")

            '(5:513) add column {...} with type {...} to the Furre table.
            Add(New Trigger(TriggerCategory.Effect, 513), AddressOf AddColumn,
                "(5:513) add column {...} with type {...} to the Furre table.")

            '(5:518) delete all Database info about the triggering furre.
            Add(New Trigger(TriggerCategory.Effect, 518), AddressOf DeleteTriggeringFurre, "(5:518) delete all Database info about the triggering furre.")
            '(5:519) delete all Database info about the furre named {...}.
            Add(New Trigger(TriggerCategory.Effect, 519), AddressOf DeleteFurreNamed, "(5:519) delete all Database info about the furre named {...}.")

            '(5:522) get the total of records from table {...} and put it into variable %.
            Add(New Trigger(TriggerCategory.Effect, 522), AddressOf GetTotalRecords, "(5:522) get the total number of records from table {...} and put it into variable %Variable.")

            '(5:523) take the sum of column{...} in table {...} and put it into variable %
            Add(New Trigger(TriggerCategory.Effect, 523), AddressOf ColumnSum, "(5:523) take the sum of column{...} in table {...} and put it into variable %Variable.")

            '(5:550) take variable %Variable , prepare it for a query, and put it in variable %Variable .   (this is your escaping call, which would depend on however you have to do it internally)
            Add(New Trigger(TriggerCategory.Effect, 550), AddressOf PrepQuery,
                "(5:550) take variable %Variable , prepare it for a SQLite Database query, and put it in variable %Variable.")

            '(5:551) execute SQLite Database query {...} Select * from table where name=%2
            Add(New Trigger(TriggerCategory.Effect, 551), AddressOf ExecuteQuery,
                 "(5:551) execute SQLite Database query {...}.")

            '(5:552) retrieve field {...} from SQLite Database query and put it into variable %Variable .
            Add(New Trigger(TriggerCategory.Effect, 552), AddressOf RetrieveQuery,
                "(5:552) retrieve field {...} from SQLite Database query and put it into variable %Variable.")

            Add(New Trigger(TriggerCategory.Effect, 559), AddressOf VACUUM,
                "(5:559) execute ""VACUUM"" to rebuild the database and reclaim wasted space.")

            '(5:561) remember Database Info {...} for Settings Table {...} to {...}.
            '(5:562) forget Database info {...} from Settings Table{...}.
            '(5:563) forget all Settings Table Database info.

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
            Dim info As String = Nothing
            Dim Variable As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            Furre = FurcadiaShortName(reader.ReadString)
            Variable = ReadVariableOrNumber(reader, False)
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
            Dim Furre As String = FurcadiaShortName(reader.ReadString())
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
            Dim info As String = Nothing
            Dim Variable As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            Furre = FurcadiaShortName(reader.ReadString)
            Variable = ReadVariableOrNumber(reader, False)
            Dim check As Object = GetValueFromTable(info, Furre)
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
            Dim info As String = Nothing
            Dim Variable As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            Furre = FurcadiaShortName(reader.ReadString)
            Variable = ReadVariableOrNumber(reader, False)
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
            Dim info As String = Nothing
            Dim Variable As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            Furre = FurcadiaShortName(reader.ReadString)

            Variable = ReadVariableOrNumber(reader, False)
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, Furre).ToString, Value)
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
            Dim info As String = Nothing
            Dim Variable As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            Furre = FurcadiaShortName(reader.ReadString)
            Variable = ReadVariableOrNumber(reader, False)
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
            Dim info As String = Nothing
            Dim Variable As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            Furre = FurcadiaShortName(reader.ReadString)

            Variable = ReadVariableOrNumber(reader, False)
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
            Dim Info As String = reader.ReadString
            Dim Furre As String = FurcadiaShortName(reader.ReadString)

            Dim str As String = reader.ReadString

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
            Dim Info As String = reader.ReadString(True)
            Dim Table As String = reader.ReadString(True)
            Dim Value As String = reader.ReadString(True)
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
            Dim Info As String = reader.ReadString(True)
            Dim setting As String = reader.ReadString(True)
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
            Dim Setting As String = reader.ReadString(True)
            Dim Table As String = reader.ReadString(True)
            Dim Number As Double = ReadVariableOrNumber(reader)
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
            Dim Setting As String = reader.ReadString(True)
            Dim Table As String = reader.ReadString(True)
            Dim Number As Double = ReadVariableOrNumber(reader)
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
            Dim Setting As String = reader.ReadString(True)
            Dim Table As String = reader.ReadString(True)
            Dim Number As Double = ReadVariableOrNumber(reader)
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
            Dim Setting As String = reader.ReadString(True)
            Dim Table As String = reader.ReadString(True)
            Dim Number As Double = ReadVariableOrNumber(reader)
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
            Dim Info As String = reader.ReadString(True)
            Dim Table As String = reader.ReadString(True)
            Dim Value As String = reader.ReadString(True)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.[" + Table + "] from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Info + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + Info + "' "

            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
            Dim Info As String = reader.ReadString(True)
            Dim setting As String = reader.ReadString(True)
            Dim cmd As String =
            "select SettingsTable.*, SettingsTableMaster.ID from SettingsTable " +
            "inner join SettingsTableMaster on " +
            "SettingsTableMaster." + Info + " = SettingsTable.[SettingsTableID] " +
            "where SettingsTableMaster.Setting = '" + setting + "' "

            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
            Dim info As String = Nothing
            Dim number As Double = 0
            Dim Furre As String = Nothing
            Dim Num As Double = 0

            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MsPage.GetVariable(MS_Name).Value.ToString
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, FurcadiaShortName(Furre)).ToString, Value)

            Return number = Value

        End Function

        ''' <summary>
        ''' "(1:518) and the Database info {...} about the triggering furre
        ''' is equal to string {...},
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on success
        ''' </returns>
        Public Function TriggeringFurreinfoEqualToSTR(reader As TriggerReader) As Boolean
            Dim Info As String = reader.ReadString
            Dim Furre As String = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString)
            Dim str As String = reader.ReadString

            If str = GetValueFromTable(Info, Furre).ToString Then Return True
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
            Dim info As String = Nothing
            Dim number As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MsPage.GetVariable(MS_Name).Value.ToString
            Furre = FurcadiaShortName(Furre)
            Dim check As Object = GetValueFromTable(info, Furre)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value > number

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
            Dim info As String = Nothing
            Dim number As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString())
            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Furre)
            Double.TryParse(check.ToString, Num)
            Return Num >= number

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
            Dim info As String = Nothing
            Dim number As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString())
            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Furre)
            Double.TryParse(check.ToString, Num)

            Return Num < number

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
            Dim info As String = Nothing
            Dim number As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString())
            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Furre)
            Double.TryParse(check.ToString, Num)
            Return Num <= number

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
            Dim info As String = Nothing
            Dim number As Double = 0
            Dim Furre As String = Nothing

            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MsPage.GetVariable(MS_Name).Value.ToString
            Furre = FurcadiaShortName(Furre)
            Dim val As String = GetValueFromTable(info, Furre).ToString
            Dim Value As Double = 0
            Double.TryParse(val, Value)
            Return Value <> number

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
            Dim Info As String = reader.ReadString
            Dim Furre As String = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString())
            Dim str As String = reader.ReadString

            If str <> GetValueFromTable(Info, Furre).ToString Then Return True

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
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
        Public Function AddColumn(reader As TriggerReader) As Boolean
            Dim Column As String = reader.ReadString
            Dim Type As String = reader.ReadString
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
        Public Function ColumnSum(reader As TriggerReader) As Boolean
            Dim Table As String = ""
            Dim Column As String = ""
            Dim Total As Variable
            Dim TotalSum As Double = 0

            Column = reader.ReadString
            Table = reader.ReadString
            Total = reader.ReadVariable(True)

            Dim sql As String = "SELECT " & Column & " FROM " & Table & " ;"
            Dim dt As DataTable = SQLiteDatabase.GetDataTable(sql)
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
        Public Function DeleteFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = FurcadiaShortName(reader.ReadString)
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
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
            Dim Furre As String = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString)
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            Return 0 < SQLiteDatabase.ExecuteNonQuery("Delete from FURRE where Name='" & Furre & "'")

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
            Dim str As String = ""

            str = reader.ReadString
            str = str.Trim
            SyncLock (lock)
                cache.Clear()
                QueryRun = False
                If str.ToUpper.StartsWith("SELECT") Then
                    Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)

                    cache = db.GetValueFromTable(str)
                    QueryRun = True

                    Return cache.Count > 0
                End If
                SQLiteDatabase.ExecuteNonQuery(str)
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
        Public Function GetTotalRecords(reader As TriggerReader) As Boolean
            Dim Table As String = ""
            Dim Total As Variable
            Dim num As Double = 0

            Dim db As New SQLiteDatabase(MsDatabase.SQLitefile)
            Table = reader.ReadString().Replace("[", "").Replace("]", "").Replace("'", "''")
            Total = reader.ReadVariable(True)
            Dim count As String = SQLiteDatabase.ExecuteScalar("select count(*) from [" & Table & "]")
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
            Dim Furre As String = FurcadiaShortName(reader.ReadString)
            Dim info As String
            If reader.PeekString Then
                info = reader.ReadString
            Else
                info = ReadVariableOrNumber(reader).ToString()
            End If
            'Dim value As String = reader.ReadVariable.Value.ToString
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            Dim data As New Dictionary(Of String, String)()
            data.Add("Name", Furre)
            data.Add("date added", Date.Now.ToString)
            data.Add("date modified", Date.Now.ToString)
            data.Add("Access Level", info)
            db.Insert("FURRE", data)
            Return True

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
        Public Function insertTriggeringFurreRecord(reader As TriggerReader) As Boolean
            Dim Furre As String = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString)
            Dim info As String = reader.ReadString
            Dim value As String = "0"
            If reader.PeekNumber Or reader.PeekVariable Then
                value = ReadVariableOrNumber(reader).ToString()
            ElseIf reader.PeekString Then
                value = reader.ReadString
                Double.TryParse(value, 0)
            End If
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            Dim data As New Dictionary(Of String, String)()
            data.Add("Name", Furre)
            data.Add("date added", Date.Now.ToString)
            data.Add("date modified", Date.Now.ToString)
            data.Add("Access Level", value)

            db.Insert("FURRE", data)
            Return True

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
        Public Function PrepQuery(reader As TriggerReader) As Boolean
            Dim var1 As Variable
            Dim var2 As Variable

            var1 = reader.ReadVariable(True)
            var2 = reader.ReadVariable(True)
            Dim str As String = var1.Value.ToString
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

            Dim db As New SQLiteDatabase(MsDatabase.SQLitefile)
            Dim Info As String = reader.ReadString
            Dim Variable As Variable = reader.ReadVariable(True)
            Dim Furre As String = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString)
            'Dim db As SQLiteDatabase = New SQLiteDatabase(file)
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
        Public Function ReadDatabaseInfoName(reader As TriggerReader) As Boolean

            Dim db As New SQLiteDatabase(SQLitefile)
            Dim Info As String = reader.ReadString
            Dim Furre As String = FurcadiaShortName(reader.ReadString)
            Dim Variable As Variable = reader.ReadVariable(True)
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
        Public Function RecordIndex(reader As TriggerReader) As Boolean
            Dim info As String = ""
            Dim Idx As Variable
            Dim OutVar As Variable
            Dim Table As String = ""

            Table = reader.ReadString(True).Replace("[", "").Replace("]", "").Replace("'", "''")
            info = reader.ReadString(True)
            Idx = reader.ReadVariable(True)
            OutVar = reader.ReadVariable(True)

            Dim sql As String = "SELECT " & info & " FROM [" & Table & "] ;"
            Dim dt As DataTable = SQLiteDatabase.GetDataTable(sql)
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
            Dim Field As String
            Dim Var As Variable

            Field = reader.ReadString
            Var = reader.ReadVariable(True)

            If QueryRun Then
                SyncLock (lock)
                    If cache.Count > 0 Then

                        For Each key As KeyValuePair(Of String, Object) In cache
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
            Dim info As String = reader.ReadString
            Dim Furre As String = reader.ReadString
            'Dim Furre As String = MSpage.GetVariable("~Name").Value.ToString
            Dim value As String = ReadVariableOrNumber(reader, False).ToString
            Dim db As New SQLiteDatabase(SQLitefile)
            Dim data As New Dictionary(Of String, String)()
            data.Add("Name", FurcadiaShortName(Furre))
            data.Add(info, value)
            data.Add("date modified", Date.Now.ToString)

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
        Public Function UpdateFurreNamed_FieldSTR(reader As TriggerReader) As Boolean
            Dim info As String = reader.ReadString
            Dim Furre As String = FurcadiaShortName(reader.ReadString)
            'Dim Furre As String = MSpage.GetVariable("~Name").Value.ToString
            Dim value As String = reader.ReadString
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            Dim data As New Dictionary(Of String, String)()
            data.Add("Name", Furre)
            data.Add(info, value)
            data.Add("date modified", Date.Now.ToString)

            db.Update("FURRE", data, "[Name]='" & Furre & "'")
            Return True

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
            Dim info As String = reader.ReadString
            'Dim Furre As String = reader.ReadString
            Dim Furre As String = ""
            Furre = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString)
            Dim value As Double = ReadVariableOrNumber(reader)
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            Dim data As New Dictionary(Of String, String)()
            data.Add("Name", Furre)
            data.Add(info, value.ToString)
            data.Add("date modified", Date.Now.ToString)

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
            Dim info As String = reader.ReadString
            'Dim Furre As String = reader.ReadString
            Dim Furre As String = FurcadiaShortName(MsPage.GetVariable(MS_Name).Value.ToString)
            Dim value As String = reader.ReadString
            Dim db As SQLiteDatabase = New SQLiteDatabase(SQLitefile)
            Dim data As New Dictionary(Of String, String)()
            data.Add("Name", Furre)
            data.Add(info, value)
            data.Add("date modified", Date.Now.ToString)

            db.Update("FURRE", data, "[Name]='" & Furre & "'")
            Return True

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
        Public Function UseOrCreateSQLiteFileIfNotExist(reader As TriggerReader) As Boolean
            SQLitefile = Paths.CheckBotFolder(reader.ReadString())
            Console.WriteLine("NOTICE: SQLite Database file has changed to" + SQLitefile)
            Dim db As New SQLiteDatabase(SQLitefile)
            'db.CreateTbl("FURRE", FurreTable)
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
        Public Function VACUUM(reader As TriggerReader) As Boolean
            Dim start As Date = Date.Now
            SQLiteDatabase.ExecuteNonQuery("VACUUM")
            Dim ts As TimeSpan = Date.Now.Subtract(start)
            'SendClientMessage("SYSTEM:", "Executed Vacum in " + ts.Seconds.ToString + " seconds")
            'TODO: Provide Database Stats for feedback
            Return True
        End Function

        '(5:561) remember Database Info {...} for Settings Table {...} to {...}.
        '(5:562) forget Database info {...} from Settings Table{...}.
        '(5:563) forget all Settings Table Database info.

#End Region

    End Class

End Namespace