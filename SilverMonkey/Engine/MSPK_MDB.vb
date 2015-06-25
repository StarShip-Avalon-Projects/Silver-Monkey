
Imports Monkeyspeak
Imports SilverMonkey.ConfigStructs
Imports SilverMonkey.TextBoxWriter
Imports System.Data
Imports System.Data.SQLite
Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports System.IO


Public Class MSPK_MDB
    Inherits Monkeyspeak.Libraries.AbstractBaseLibrary
    Private Const FurreTable As String = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [Access Level] INTEGER, [date added] TEXT, [date modified] TEXT, [PSBackup] DOUBLE"

    Private writer As TextBoxWriter = Nothing
    Public Shared SQLreader As SQLiteDataReader = Nothing
    Private QueryRun As Boolean = False

    Dim lock As New Object
    Dim cache As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
    Public Sub New()
        Try
            writer = New TextBoxWriter(Variables.TextBox1)
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        Try
            Main.SQLitefile = CheckMyDocFile("SilverMonkey.db")
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try
        Try
            '(0:500) When the bot starts backing up the character Phoenix Speak,
            Add(Monkeyspeak.TriggerCategory.Cause, 500,
                Function()
                    Return True
                End Function, "(0:500) When the bot starts backing up the character Phoenix Speak,")
            '(0:501) When the bot completes backing up the characters Phoenix Speak,
            Add(Monkeyspeak.TriggerCategory.Cause, 501,
                Function()
                    Return True
                End Function, "(0:501) When the bot completes backing up the characters Phoenix Speak,")
            '(0:502) When the bot starts restoring the Dreams Character Phoenix Speak,
            Add(Monkeyspeak.TriggerCategory.Cause, 502,
                Function()
                    Return True
                End Function, "(0:502) When the bot starts restoring the Dreams Character Phoenix Speak,")
            '(0:503) When the bot finishes restoring the dreams character Phoenix Speak,
            Add(Monkeyspeak.TriggerCategory.Cause, 503,
                Function()
                    Return True
                End Function, "(0:503) When the bot finishes restoring the dreams character Phoenix Speak,")
            Add(Monkeyspeak.TriggerCategory.Cause, 504,
                Function()
                    Return True
                End Function, "(0:504) When the bot backsup Phoenix Speak for any Furre.")
            Add(Monkeyspeak.TriggerCategory.Cause, 505,
                AddressOf BackUpCharacterNamed, "(0:505) When the bot backsup Phoenix Speak for the furre named {...}.")
            Add(Monkeyspeak.TriggerCategory.Cause, 506,
                Function()
                    Return True
                End Function, "(0:506) When the bot restores any furre's Phoenix Speak.")
            Add(Monkeyspeak.TriggerCategory.Cause, 507,
                AddressOf BackUpCharacterNamed, "(0:507) When the bot restores the  Phoenix Speak for the furre named {...}.")

            '(1:500) and the Database info {...} about the triggering furre is equal to #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 500),
                AddressOf TriggeringFurreinfoEqualToNumber, "(1:500) and the Database info {...} about the triggering furre is equal to #,")
            '(1:501) and the Database info {...} about the triggering furre is not equal to #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 501),
                AddressOf TriggeringFurreinfoNotEqualToNumber, "(1:501) and the Database info {...} about the triggering furre is not equal to #,")
            '(1:502) and the Database info {...} about the triggering furre is greater than #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 502),
                AddressOf TriggeringFurreinfoGreaterThanNumber, "(1:502) and the Database info {...} about the triggering furre is greater than #,")
            '(1:503) and the Database info {...} about the triggering furre is less than #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 503),
                AddressOf TriggeringFurreinfoLessThanNumber, "(1:503) and the Database info {...} about the triggering furre is less than #,")

            '(1:504) and the Database info {...} about the triggering furre is greater than or equal to #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 504),
                AddressOf TriggeringFurreinfoGreaterThanOrEqualToNumber, "(1:504) and the Database info {...} about the triggering furre is greater than or equal to #,")
            '(1:505) and the Database info {...} about the triggering furre is less than or equal to#,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 505),
                AddressOf TriggeringFurreinfoLessThanOrEqualToNumber, "(1:505) and the Database info {...} about the triggering furre is less than or equal to #,")


            '(1:508) and the Database info {...} about the furre named {...} is equal to #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 508),
                AddressOf FurreNamedinfoEqualToNumber, "(1:508) and the Database info {...} about the furre named {...} is equal to #,")
            '(1:509) and the Database info {...} about the furre named {...} is not equal to #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 509),
                AddressOf FurreNamedinfoNotEqualToNumber, "(1:509) and the Database info {...} about the furre named {...} is not equal to #,")
            '(1:510) and the Database info {...} about the furre named {...} is greater than #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 510),
                AddressOf FurreNamedinfoGreaterThanNumber, "(1:510) and the Database info {...} about the furre named {...} is greater than #,")
            '(1:511) and the Database info {...} about the furre named {...} is less than #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 511),
                AddressOf FurreNamedinfoLessThanNumber, "(1:511) and the Database info {...} about the furre named {...} is less than #,")

            '(1:510) and the Database info {...} about the furre named {...} is greater than or equal to #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 512),
            AddressOf FurreNamedinfoGreaterThanOrEqualToNumber, "(1:512) and the Database info {...} about the furre named {...} is greater than or equal to #,")
            '(1:511) and the Database info {...} about the furre named {...} is less than or equal to #,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 513),
            AddressOf FurreNamedinfoLessThanOrEqualToNumber, "(1:513) and the Database info {...} about the furre named {...} is less than or equal to #,")


            '(1:516) and the Database info {...} about the furre named {...} is equal to {...},
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 516),
           AddressOf FurreNamedinfoEqualToSTR, "(1:516) and the Database info {...} about the furre named {...} is equal to string {...},")
            '(1:517) and the Database info {...} about the furre named {...} is not equal to {...},
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 517),
            AddressOf FurreNamedinfoNotEqualToSTR, "(1:517) and the Database info {...} about the furre named {...} is not equal to string {...},")
            '(1:518) and the Database info {...} about the triggering furre is equal to {...},
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 518),
                AddressOf TriggeringFurreinfoEqualToSTR, "(1:518) and the Database info {...} about the triggering furre is equal to string {...},")
            '(1:519) and the Database info {...} about the triggering furre is not equal to {...},
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 519),
                AddressOf TriggeringFurreinfoNotEqualToSTR, "(1:519) and the Database info {...} about the triggering furre is not equal to string {...},")

            '(1:520) and the bot is not in the middle of a PS Backup Process
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 520),
                  AddressOf BotBackup, "(1:520) and the bot is not in the middle of a PS Backup Process,")

            '(1:521) and the bot is in the middle of a PS Backup Process.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 521),
                     AddressOf NotBotBackup, "(1:521) and the bot is in the middle of a PS Backup Process,")

            '(1:522) and the bot is not in the middle of a PS Restore Process,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 522),
                 AddressOf BotRestore, "(1:522) and the bot is not in the middle of a PS Restore Process,")
            '(1:523) and the bot is in the middle of a PS Restore Process,
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Condition, 523),
                 AddressOf NotBotRestore, "(1:523) and the bot is in the middle of a PS Restore Process,")


            '(5:500) use SQLite database file {...} or create file if it does not exist.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 500), AddressOf createMDB, "(5:500) use SQLite database file {...} or create file if it does not exist.")

            '(5:505 ) Add the triggering furre with the default access level 0 to the Furre Table in the database if he, she or it don't exist.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 505), AddressOf insertTriggeringFurreRecord, "(5:505) add the triggering furre with the default access level ""0"" to the Furre Table in the database if he, she, or it doesn't exist.")
            '(5:506) Add furre named {...} with the default access level 0 to the Furre Table in the database if he, she or it don't exist.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 506), AddressOf InsertFurreNamed, "(5:506) add furre named {...} with the default access level ""0"" to the Furre Table in the database if he, she, or it doesn't exist.")

            '(5:507) update Database info {...} about the triggering furre will now be #.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 507), AddressOf UpdateTriggeringFurreField, "(5:507) update Database info {...} about the triggering furre will now be #.")
            '(5:508) update Database info {...} about the furre named {...} will now be #.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 508), AddressOf UpdateFurreNamed_Field, "(5:508) update Database info {...} about the furre named {...} will now be #.")
            '(5:509) update Database info {...} about the triggering furre will now be {...}.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 509), AddressOf UpdateTriggeringFurreFieldSTR, "(5:509) update Database info {...} about the triggering furre will now be {...}.")
            '(5:510) update Database info {...} about the furre named {...} will now be {...}.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 510), AddressOf UpdateFurreNamed_FieldSTR, "(5:510) update Database info {...} about the furre named {...} will now be {...}.")

            '(5:511) select Database info {...} about the triggering furre, and put it in variable %.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 511), AddressOf ReadDatabaseInfo, "(5:511) select Database info {...} about the triggering furre, and put it in variable %.")
            '(5:512) select Database info {...} about the furre named {...}, and put it in variable %.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 512), AddressOf ReadDatabaseInfoName, "(5:512) select Database info {...} about the furre named {...}, and put it in variable %.")

            '(5:513) add column {...} with type {...} to the Furre table.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 513), AddressOf AddColumn, "(5:513) add column {...} with type {...} to the Furre table.")


            '(5:518) delete all Database info about the triggering furre.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 518), AddressOf DeleteTriggeringFurre, "(5:518) delete all Database info about the triggering furre.")
            '(5:519) delete all Database info about the furre named {...}.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 519), AddressOf DeleteFurreNamed, "(5:519) delete all Database info about the furre named {...}.")

            '(5:522) get the total of records from table {...} and put it into variable %.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 522), AddressOf GetTotalRecords, "(5:522) get the total number of records from table {...} and put it into variable %Variable.")

            '(5:523) take the sum of column{...} in table {...} and put it into variable %
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 523), AddressOf ColumnSum, "(5:523) take the sum of column{...} in table {...} and put it into variable %Variable.")

            '(5:550) take variable %Variable , prepare it for a query, and put it in variable %Variable .   (this is your escaping call, which would depend on however you have to do it internally)
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 550), AddressOf PrepQuery,
                "(5:550) take variable %Variable , prepare it for a SQLite Database query, and put it in variable %Variable.")

            '(5:551) execute SQLite Database query {...} Select * from table where name=%2
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 551), AddressOf ExecuteQuery,
                 "(5:551) execute SQLite Database query {...}.")

            '(5:552) retrieve field {...} from SQLite Database query and put it into variable %Variable .
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 552), AddressOf RetrieveQuery,
                "(5:552) retrieve field {...} from SQLite Database query and put it into variable %Variable.")
            '(5:553) Backup All Character phoenixspeak for the dream
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 553), AddressOf BackupAllPS,
               "(5:553) backup All Phoenix Speak for the dream")
            '(5:554) backup Character named {...} phoenix speak 
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 554), AddressOf BackupSingleCharacterPS,
                   "(5:554) backup character named {...} Phoenix Speak. (use ""[DREAM]"" to restore information specific to the dream)")
            '(5:555) restore phoenix speak for character {...}
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 555), AddressOf RestoreCharacterPS,
                   "(5:555) restore Phoenix Speak for character {...}. (use ""[DREAM]"" to restore information specific to the dream)")
            '(5:556) restore all phoenxi speak characters for this dream.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 556), AddressOf restoreAllPSData,
                 "(5:556) restore all Phoenix Speak records for this dream.")
            '(5:557) remove Entries older then # days from Phoenix Speak Character backup.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 557), AddressOf PruneCharacterBackup,
                "(5:557) remove Entries older than # days from Phoenix Speak backup.")
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 558), AddressOf restorePS_DataOldrThanDays,
                "(5:558) restore Phoenix Speak records newer then # days.")
            '(5:559) execute VACUUM on the database to rebuild and reclaim wasted space.
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 559), AddressOf VACUUM,
                "(5:559) execute ""VACUUM"" to rebuild the database and reclaim wasted space.")
            Add(New Monkeyspeak.Trigger(Monkeyspeak.TriggerCategory.Effect, 560), AddressOf AbortPS,
                "(5:560) abort Phoenix Speak backup or restore process")
        Catch eX As Exception
            Dim logError As New ErrorLogging(eX, Me)
        End Try

    End Sub


#Region "Condition Functions"

    Public Function BackUpCharacterNamed(reader As TriggerReader) As Boolean
        Dim furre As String = reader.ReadString
        Return callbk.Player.ShortName = furre.ToFurcShortName
    End Function

    '(1: ) and the Database info {...} about the triggering furre is equal to #,
    Public Function TriggeringFurreinfoEqualToNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim number As Double = 0
        Dim Furre As String = Nothing
        Dim Num As Double = 0

        Try
            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, Furre).ToString, Value)


            If number = Value Then Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function

    '(1: ) and the Database info {...} about the triggering furre is not equal to #,
    Public Function TriggeringFurreinfoNotEqualToNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim number As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString
            Furre = Furre.ToFurcShortName
            Dim val As String = GetValueFromTable(info, Furre).ToString
            Dim Value As Double = 0
            Double.TryParse(val, Value)
            Return Value <> number
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function
    '(1: ) and the Database info {...} about the triggering furre is greater than #,
    Public Function TriggeringFurreinfoGreaterThanNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim number As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString
            Furre = Furre.ToFurcShortName
            Dim check As Object = GetValueFromTable(info, Furre)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value > number

        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function
    '(1: ) and the Database info {...} about the triggering furre is less than #,
    Public Function TriggeringFurreinfoLessThanNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim number As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Furre)
            Double.TryParse(check.ToString, Num)

            Return Num < number
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function

    '(1: ) and the Database info {...} about the triggering furre is greater than or equal to #,
    Public Function TriggeringFurreinfoGreaterThanOrEqualToNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim number As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Furre)
            Double.TryParse(check.ToString, Num)
            Return Num >= number

        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function
    '(1: ) and the Database info {...} about the triggering furre is less than #,
    Public Function TriggeringFurreinfoLessThanOrEqualToNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim number As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            number = ReadVariableOrNumber(reader, False)
            Furre = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Dim Num As Double = 0
            Dim check As Object = GetValueFromTable(info, Furre)
            Double.TryParse(check.ToString, Num)
            Return Num <= number
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function

    '(1: ) and the Database info {...} about the furre named {...} is equal to #,
    Public Function FurreNamedinfoEqualToNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim Variable As Double = 0
        Dim Furre As String = Nothing

        Try
            info = reader.ReadString
            Furre = reader.ReadString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Variable = ReadVariableOrNumber(reader, False)
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, Furre).ToString, Value)
            Return Variable = Value
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function
    '(1: ) and the Database info {...} about the furre named {...} is not equal to #,
    Public Function FurreNamedinfoNotEqualToNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim Variable As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            Furre = reader.ReadString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Variable = ReadVariableOrNumber(reader, False)
            Dim check As Object = GetValueFromTable(info, Furre)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value <> Variable
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function
    '(1: ) and the Database info {...} about the furre named {...} is greater than #,
    Public Function FurreNamedinfoGreaterThanNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim Variable As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            Furre = reader.ReadString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Variable = ReadVariableOrNumber(reader, False)
            Dim check As Object = GetValueFromTable(info, Furre)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value > Variable
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function
    '(1: ) and the Database info {...} about the furre named {...} is less than #,
    Public Function FurreNamedinfoLessThanNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim Variable As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            Furre = reader.ReadString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Variable = ReadVariableOrNumber(reader, False)
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, Furre).ToString, Value)
            Return Value < Variable
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function

    '(1: ) and the Database info {...} about the furre named {...} is greater than #,
    Public Function FurreNamedinfoGreaterThanOrEqualToNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim Variable As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            Furre = reader.ReadString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Variable = ReadVariableOrNumber(reader, False)
            Dim Value As Double = 0
            Double.TryParse(GetValueFromTable(info, Furre).ToString, Value)
            Return Value >= Variable
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function
    '(1: ) and the Database info {...} about the furre named {...} is less than #,
    Public Function FurreNamedinfoLessThanOrEqualToNumber(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = Nothing
        Dim Variable As Double = 0
        Dim Furre As String = Nothing
        Try
            info = reader.ReadString
            Furre = reader.ReadString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Variable = ReadVariableOrNumber(reader, False)
            Dim check As Object = GetValueFromTable(info, Furre)
            Dim Value As Double = 0
            Double.TryParse(check.ToString, Value)
            Return Value <= Variable
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

        Return False
    End Function

    '(1: ) and the Database info {...} about the furre named {...} is equal to {...},
    Public Function FurreNamedinfoEqualToSTR(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Info As String = reader.ReadString
        Dim Furre As String = reader.ReadString()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        Dim str As String = reader.ReadString
        Try
            Return str = GetValueFromTable(Info, Furre).ToString
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return False
    End Function
    '(1: ) and the Database info {...} about the furre named {...} is not equal to {...},
    Public Function FurreNamedinfoNotEqualToSTR(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Info As String = reader.ReadString
        Dim Furre As String = reader.ReadString
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        Dim str As String = reader.ReadString

        Try
            Return str <> GetValueFromTable(Info, Furre).ToString
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return False
    End Function
    '(1: ) and the Database info {...} about the triggering furre is equal to {...},
    Public Function TriggeringFurreinfoEqualToSTR(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Info As String = reader.ReadString
        Dim Furre As String = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        Dim str As String = reader.ReadString
        Try
            If str = GetValueFromTable(Info, Furre).ToString Then Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return False
    End Function
    '(1: ) and the Database info {...} about the triggering furre is not equal to {...},
    Public Function TriggeringFurreinfoNotEqualToSTR(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Info As String = reader.ReadString
        Dim Furre As String = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        Dim str As String = reader.ReadString
        Try
            If str <> GetValueFromTable(Info, Furre).ToString Then Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return False
    End Function

    '(1:520) and the bot is not in the middle of a PS Backup Process
    Public Function BotBackup(reader As Monkeyspeak.TriggerReader) As Boolean

        Try
            Return Not callbk.PSBackupRunning
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return False
    End Function
    '(1:521) and the bot is in the middle of a PS Backup Process
    Public Function NotBotBackup(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Return callbk.PSBackupRunning
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return False
    End Function
    '(1:522) and the bot is not in the middle of a PS Restore Process
    Public Function BotRestore(reader As Monkeyspeak.TriggerReader) As Boolean

        Try
            Return Not callbk.PSRestoreRunning
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return False
    End Function
    '(1:523) and the bot is in the middle of a PS Restore Process
    Public Function NotBotRestore(reader As Monkeyspeak.TriggerReader) As Boolean

        Try
            Return callbk.PSRestoreRunning
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return False
    End Function
#End Region

#Region "Condition Helper Functions"
    Private Function GetValueFromTable(ByRef Column As String, ByRef Name As String) As Object
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
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


    Public Function createMDB(reader As Monkeyspeak.TriggerReader) As Boolean
        Main.SQLitefile = CheckMyDocFile(reader.ReadString())
        Dim db As New SQLiteDatabase(Main.SQLitefile)
        'db.CreateTbl("FURRE", FurreTable)
        Return True
    End Function


    '(5:405) Add the triggering furre with default access level to the Furre Table in the database if he, she or it don't exist.
    Public Function insertTriggeringFurreRecord(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Furre As String = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString()
        Dim info As String = reader.ReadString
        'Dim value As String = reader.ReadVariable.Value.ToString

        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim data As New Dictionary(Of [String], [String])()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        data.Add("[Name]", Furre)
        data.Add("[date added]", Date.Now.ToString)
        data.Add("[date modified]", Date.Now.ToString)
        data.Add("[Access Level]", "0")
        Try
            db.Insert("FURRE", data)
            Return True
        Catch crap As Exception
            MessageBox.Show(crap.Message)
            Return False
        End Try
    End Function

    '(5:506) add furre named {%NewMember} with the default access level "1" to the Furre Table in the database if he, she, or it doesn't exist.
    Public Function InsertFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Furre As String = reader.ReadString
        Dim info As String
        If reader.PeekString Then
            info = reader.ReadString
        Else
            info = reader.ReadVariableOrNumber.ToString
        End If
        'Dim value As String = reader.ReadVariable.Value.ToString
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim data As New Dictionary(Of [String], [String])()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        data.Add(MS_Name, Furre)
        data.Add("[date added]", Date.Now.ToString)
        data.Add("[date modified]", Date.Now.ToString)
        data.Add("[Access Level]", info)
        Try
            db.Insert("FURRE", data)
            Return True
        Catch crap As Exception
            MessageBox.Show(crap.Message)
            Return False
        End Try
    End Function
    '(5407) update Database info {...} about the triggering furre will now be #.
    Public Function UpdateTriggeringFurreField(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = reader.ReadString
        'Dim Furre As String = reader.ReadString
        Dim Furre As String = ""
        SyncLock lock
            Furre = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString
        End SyncLock
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        Dim value As Double = ReadVariableOrNumber(reader)
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim data As New Dictionary(Of [String], [String])()
        data.Add(MS_Name, Furre)
        data.Add("[" & info & "]", value.ToString)
        data.Add("[date modified]", Date.Now.ToString)
        Try
            db.Update("FURRE", data, "Name='" & Furre & "'")
            Return True
        Catch crap As Exception
            MessageBox.Show(crap.Message)
            Return False
        End Try
    End Function

    '(5:408) update Database info {...} about the furre named {...} will now be #.
    Public Function UpdateFurreNamed_Field(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = reader.ReadString
        Dim Furre As String = reader.ReadString
        'Dim Furre As String = MainEngine.MSpage.GetVariable("~Name").Value.ToString
        Dim value As String = ReadVariableOrNumber(reader, False).ToString
        Dim db As New SQLiteDatabase(Main.SQLitefile)
        Dim data As New Dictionary(Of [String], [String])()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        data.Add(MS_Name, Furre)
        data.Add("[" & info & "]", value)
        data.Add("[date modified]", Date.Now.ToString)
        Try
            db.Update("FURRE", data, "Name='" & Furre & "'")
            Return True
        Catch crap As Exception
            Dim e As New ErrorLogging(crap, Me)
            Return False
        End Try
    End Function

    '(5:409) update Database info {...} about the triggering furre will now be {...}.
    Public Function UpdateTriggeringFurreFieldSTR(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = reader.ReadString
        'Dim Furre As String = reader.ReadString
        Dim Furre As String = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString()
        Dim value As String = reader.ReadString
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim data As New Dictionary(Of [String], [String])()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        data.Add(MS_Name, Furre)
        data.Add("[" & info & "]", value)
        data.Add("[date modified]", Date.Now.ToString)
        Try
            db.Update("FURRE", data, "Name='" & Furre & "'")
            Return True
        Catch crap As Exception
            Dim e As New ErrorLogging(crap, Me)
            Return False
        End Try
    End Function
    '(5:410) update Database info {...} about the furre named {...} will now be {...}.
    Public Function UpdateFurreNamed_FieldSTR(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = reader.ReadString
        Dim Furre As String = reader.ReadString
        'Dim Furre As String = MainEngine.MSpage.GetVariable("~Name").Value.ToString
        Dim value As String = reader.ReadString
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Dim data As New Dictionary(Of [String], [String])()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        data.Add(MS_Name, Furre)
        data.Add("[" & info & "]", value)
        data.Add("[date modified]", Date.Now.ToString)
        Try
            db.Update("FURRE", data, "Name='" & Furre & "'")
            Return True
        Catch crap As Exception
            Dim e As New ErrorLogging(crap, Me)
            Return False
        End Try
    End Function

    '(5:411) select Database info {...} about the triggering furre, and put it in variable %Variable.
    Public Function ReadDatabaseInfo(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim Info As String = reader.ReadString
            Dim Variable As Monkeyspeak.Variable = reader.ReadVariable(True)
            Dim Furre As String = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString()
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            'Dim db As SQLiteDatabase = New SQLiteDatabase(file)
            Dim cmd As String = "SELECT [" & Info & "] FROM FURRE Where Name ='" & Furre & "'"
            Variable.Value = SQLiteDatabase.ExecuteScalar1(cmd)
            Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function

    '(5:412) select Database info {...} about the furre named {...}, and put it in variable %Variable.
    Public Function ReadDatabaseInfoName(reader As Monkeyspeak.TriggerReader) As Boolean
        Try
            Dim Info As String = reader.ReadString
            Dim Furre As String = reader.ReadString
            Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
            Dim Variable As Monkeyspeak.Variable = reader.ReadVariable(True)
            ' Dim db As SQLiteDatabase = New SQLiteDatabase(file)
            Dim cmd As String = "SELECT [" & Info & "] FROM FURRE Where Name ='" & Furre & "'"
            Variable.Value = SQLiteDatabase.ExecuteScalar1(cmd)
            Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function

    '(5:513) add column {...} with type {...} to the Furre table.
    Public Function AddColumn(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Column As String = reader.ReadString
        Dim Type As String = reader.ReadString
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        db.addColumn("FURRE", "[" & Column & "]", Type)
        Return True
    End Function
    '(5:418) delete all Database info about the triggering furre.
    Public Function DeleteTriggeringFurre(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Furre As String = MainEngine.MSpage.GetVariable(MS_Name).Value.ToString()
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Return 0 < SQLiteDatabase.ExecuteNonQuery("Delete from FURRE where Name='" & Furre & "'")

    End Function
    '(5:419) delete all Database info about the furre named {...}.
    Public Function DeleteFurreNamed(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Furre As String = reader.ReadString
        Furre = Regex.Replace(Furre.ToLower(), REGEX_NameFilter, "")
        Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)
        Return 0 < SQLiteDatabase.ExecuteNonQuery("Delete from FURRE where Name='" & Furre & "'")

    End Function

    '(5:422) get the total number of records from table {...} and put it into variable %.
    Public Function GetTotalRecords(reader As TriggerReader) As Boolean
        Dim Table As String = ""
        Dim Total As Variable
        Dim num As Double = 0

        Try
            Table = reader.ReadString
            Total = reader.ReadVariable(True)
            Dim count As String = SQLiteDatabase.ExecuteScalar1("select count(1) from [" & Table & "]")
            Total.Value = count
            Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try


    End Function
    '(5:423) take the sum of column{...} in table {...} and put it into variable %
    Public Function ColumnSum(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Table As String = ""
        Dim Column As String = ""
        Dim Total As Variable
        Dim TotalSum As Integer = 0

        Try
            Column = reader.ReadString
            Table = reader.ReadString
            Total = reader.ReadVariable(True)
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Dim sql As String = "SELECT " & Column & " FROM " & Table & " ;"
        Dim dt As DataTable = SQLiteDatabase.GetDataTable(sql)
        Column = Column.Replace("[", "")
        Column = Column.Replace("]", "")
        For Each row As DataRow In dt.Rows
            Try
                TotalSum += Convert.ToInt32(row(Column))
                'Console.WriteLine("Calculating TotalSum {0}", TotalSum.ToString)
            Catch
            End Try
        Next row
        Total.Value = TotalSum.ToString
        Return True
    End Function

    '(5:424) in table {...} take info {...} from record index % and and put it into variable %
    Public Function RecordIndex(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim info As String = ""
        Dim Idx As Variable
        Dim OutVar As Variable
        Dim Table As String = ""
        Try
            info = reader.ReadString(True)
            Idx = reader.ReadVariable(True)
            OutVar = reader.ReadVariable(True)

        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Dim sql As String = "SELECT " & info & " FROM " & Table & " ;"
        Dim dt As DataTable = SQLiteDatabase.GetDataTable(sql)
        info = info.Replace("[", "")
        info = info.Replace("]", "")
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

    '(5:550) take variable %Variable , prepare it for a query, and put it in variable %Variable   (this is your escaping call, which would depend on however you have to do it internally)
    Public Function PrepQuery(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim var1 As Monkeyspeak.Variable
        Dim var2 As Monkeyspeak.Variable
        Try
            var1 = reader.ReadVariable
            var2 = reader.ReadVariable(True)
            Dim str As String = var1.Value.ToString
            str = str.Replace("'", "''")
            var2.Value = str
            Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function
    '(5:551) execute query {...}. Select * from table where name=%2
    ' "Has a query been run since the last time someone asked for a result? If so, if read() then export one row.
    Public Function ExecuteQuery(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim str As String = ""

        Try
            str = reader.ReadString
            str = str.Trim
            SyncLock (lock)
                cache.Clear()
                QueryRun = False
                If str.ToUpper.StartsWith("SELECT") Then
                    Dim db As SQLiteDatabase = New SQLiteDatabase(Main.SQLitefile)

                    cache = db.GetValueFromTable(str)
                    QueryRun = True

                    Return cache.Count > 0
                ElseIf str.ToUpper.StartsWith("UPDATE") Then

                    Return SQLiteDatabase.ExecuteNonQuery(str) > 0
                End If

                SQLiteDatabase.ExecuteNonQuery(str)
                Return True
            End SyncLock
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message & " SQL: " & str
            writer.WriteLine(ErrorString)

            Debug.Print(ErrorString)
            Debug.Print(str)
            Return False
        End Try

    End Function

    '(5:552) retrieve field {...} from query and put it into variable %Variable
    Public Function RetrieveQuery(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim Field As String
        Dim Var As Monkeyspeak.Variable

        Try
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
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
    End Function

    '(5:553) Backup All Character phoenixspeak for the dream
    Function BackupAllPS(reader As TriggerReader) As Boolean
        Try
            If Not callbk.PSBackupRunning And Not callbk.PSRestoreRunning Then
                callbk.CurrentPS_Stage = Main.PS_BackupStage.GetList
                sendServer("ps get character.*")
            End If
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return True
    End Function

    '(5:554) backup Character named {...} phoenix speak 
    Function BackupSingleCharacterPS(reader As TriggerReader) As Boolean
        Try
            If Not callbk.PSBackupRunning And Not callbk.PSRestoreRunning And Not callbk.PSPruneRunning Then

                Dim str As String = reader.ReadString
                If str.ToUpper <> "[DREAM]" Then
                    str = str.ToFurcShortName
                Else
                    str = str.ToUpper
                End If

                Dim f As New Main.PSInfo_Struct
                f.name = str
                f.PS_ID = callbk.CharacterList.Count + 1
                callbk.CharacterList.Add(f)
                If callbk.CurrentPS_Stage <> Main.PS_BackupStage.GetSingle Then
                    callbk.CurrentPS_Stage = Main.PS_BackupStage.GetSingle
                    callbk.psReceiveCounter = 0
                    callbk.psSendCounter = 1
                    callbk.PSBackupRunning = True
                    If str <> "[DREAM]" Then
                        callbk.ServerStack.Enqueue("ps " + callbk.CharacterList.Count.ToString + " get character." + str + ".*")
                    Else
                        callbk.ServerStack.Enqueue("ps " + callbk.CharacterList.Count.ToString + " get character.dream.*")
                    End If
                End If
            End If
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return True
    End Function
    '(5:555) restore phoenix speak for character {...}
    Public Function RestoreCharacterPS(reader As Monkeyspeak.TriggerReader) As Boolean

        Try
            Dim furre As String = reader.ReadString()
            callbk.Build_PS_CMD(furre, True)
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try
        Return True
    End Function
    '(5:556) restore all phoenxi speak characters for this dream.
    Public Function restoreAllPSData(reader As TriggerReader) As Boolean

        Dim str As String = ""

        Try
            If Not callbk.PSBackupRunning And Not callbk.PSRestoreRunning Then
                callbk.RestorePS()
            End If
            Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

    End Function
    '(5:557) remove Entries older then # days from Phoenix Speak Character backup.

    Public Function PruneCharacterBackup(reader As TriggerReader) As Boolean

        Try
            Dim age As Double = ReadVariableOrNumber(reader)
            If Not callbk.PSBackupRunning And Not callbk.PSRestoreRunning Then
                callbk.PrunePS(age)
            End If
            Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

    End Function

    '(5:558) restore phoenix speak characters newer then # days.
    Public Function restorePS_DataOldrThanDays(reader As TriggerReader) As Boolean



        Try
            Dim days As Double = ReadVariableOrNumber(reader)
            If Not callbk.PSBackupRunning And Not callbk.PSRestoreRunning Then
                callbk.RestorePS(days)
            End If
            Return True
        Catch ex As Exception
            Dim tID As String = reader.TriggerId.ToString
            Dim tCat As String = reader.TriggerCategory.ToString
            Console.WriteLine(MS_ErrWarning)
            Dim ErrorString As String = "Error: (" & tCat & ":" & tID & ") " & ex.Message
            writer.WriteLine(ErrorString)
            Debug.Print(ErrorString)
            Return False
        End Try

    End Function

    Public Function VACUUM(reader As Monkeyspeak.TriggerReader) As Boolean
        Dim start As DateTime = DateTime.Now
        SQLiteDatabase.ExecuteNonQuery("VACUUM")
        Dim ts As TimeSpan = DateTime.Now.Subtract(start)
        callbk.SendClientMessage("SYSTEM:", "Executed Vacum in " + ts.Seconds.ToString + " seconds")
        Return True
    End Function

    Public Function AbortPS(reader As TriggerReader) As Boolean
        If callbk.PSBackupRunning Or callbk.PSRestoreRunning Then
            callbk.PS_Abort()
            callbk.SendClientMessage("SYSTEM:", "Aborted PS Backup/Restore process")
        End If
        Return True
    End Function
#End Region
#Region "Effects Helper Functions"



    Sub sendServer(ByRef var As String)
        Try
            callbk.sndServer(var)
        Catch ex As Exception
            Dim log As New ErrorLogging(ex, Me)
        End Try
    End Sub

#End Region
    Class SQLiteDatabase
        Dim lock As New Object
        Private Shared dbConnection As [String]
        Private Shared writer As TextBoxWriter = Nothing
        ' ''' <summary>
        ' '''     Default Constructor for SQLiteDatabase Class.
        ' ''' </summary>
        'Public Sub New()
        '    dbConnection = "Data Source=" & mPath() & "SilverMonkey.s3db"
        'End Sub

        ''' <summary>
        '''     Single Param Constructor for specifying the DB file.
        ''' </summary>
        ''' <param name="inputFile">The File containing the DB</param>
        Public Sub New(inputFile As [String])
            writer = New TextBoxWriter(Variables.TextBox1)
            dbConnection = [String].Format("Data Source={0};", inputFile)
            If Not File.Exists(inputFile) Then
                CreateTbl("FURRE", FurreTable)
            End If
        End Sub



        '''<Summary>
        '''    Create a Table
        ''' </Summary>
        ''' <param name="Table"></param><param name="Titles"></param>
        Public Sub CreateTbl(Table As String)
            Using SQLconnect As New SQLiteConnection(dbConnection)
                Using SQLcommand As SQLiteCommand = SQLconnect.CreateCommand

                    SQLconnect.Open()
                    'SQL query to Create Table

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS " & Table & "(id INTEGER PRIMARY KEY AUTOINCREMENT );"
                    SQLcommand.ExecuteNonQuery()
                    SQLcommand.Dispose()
                End Using
                SQLconnect.Close()
                SQLconnect.Dispose()
            End Using
        End Sub
        '''<Summary>
        '''    Create a Table with Titles
        ''' </Summary>
        ''' <param name="Table"></param><param name="Titles"></param>
        Public Sub CreateTbl(Table As String, ByRef Titles As String)
            Using SQLconnect As New SQLiteConnection(dbConnection)
                Using SQLcommand As SQLiteCommand = SQLconnect.CreateCommand

                    SQLconnect.Open()
                    'SQL query to Create Table

                    ' [Access Level] INTEGER, [date added] TEXT, [date modified] TEXT, 
                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS " & Table & "( " & Titles & " );"
                    SQLcommand.ExecuteNonQuery()
                    SQLcommand.Dispose()
                End Using
                SQLconnect.Close()
                SQLconnect.Dispose()
            End Using
        End Sub

        ''' <summary>
        '''     Single Param Constructor for specifying advanced connection options.
        ''' </summary>
        ''' <param name="connectionOpts">A dictionary containing all desired options and their values</param>
        Public Sub New(connectionOpts As Dictionary(Of [String], [String]))
            Dim str As [String] = ""
            For Each row As KeyValuePair(Of [String], [String]) In connectionOpts
                str += [String].Format("{0}={1}; ", row.Key, row.Value)
            Next
            str = str.Trim().Substring(0, str.Length - 1)
            dbConnection = str
        End Sub

        Private Function getAllColumnName(ByVal tableName As String) As String
            Dim sql As String = "SELECT * FROM " & tableName
            Dim columnNames As New ArrayList
            Using SQLconnect As New SQLiteConnection(dbConnection)
                SQLconnect.Open()

                Using SQLcommand As SQLiteCommand = SQLconnect.CreateCommand

                    SQLcommand.CommandText = sql

                    Try
                        Dim sqlDataReader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()

                        For i As Integer = 0 To sqlDataReader.VisibleFieldCount - 1
                            columnNames.Add("[" + sqlDataReader.GetName(i) + "]")
                        Next
                    Catch ex As Exception
                        Dim log As New ErrorLogging(ex, Me)
                    End Try
                    SQLcommand.Dispose()
                End Using
                SQLconnect.Close()
                SQLconnect.Dispose()
            End Using
            Return String.Join(",", columnNames.ToArray)
        End Function
        Public Function isColumnExist(ByVal columnName As String, ByVal tableName As String) As Boolean
            Dim columnNames As String = getAllColumnName(tableName)
            Return columnNames.Contains(columnName)
        End Function
        Public Sub removeColumn(ByVal tableName As String, ByVal columnName As String)
            Dim columnNames As String = getAllColumnName(tableName)
            columnNames = columnNames.Replace(columnName + ", ", "")
            columnNames = columnNames.Replace(", " + columnName, "")
            columnNames = columnNames.Replace(columnName, "")
            ExecuteNonQuery("CREATE TEMPORARY TABLE " + tableName + "backup(" + columnNames + ");")
            ExecuteNonQuery("INSERT INTO " + tableName + "backup SELECT " + columnNames + " FROM " + tableName + ";")
            ExecuteNonQuery("DROP TABLE " + tableName + ";")
            ExecuteNonQuery("CREATE TABLE " + tableName + "(" + columnNames + ");")
            ExecuteNonQuery("INSERT INTO " + tableName + " SELECT " + columnNames + " FROM " + tableName + "backup;")
            ExecuteNonQuery("DROP TABLE " + tableName + "backup;")
        End Sub

        'Add a column is much more easy
        Public Sub addColumn(ByVal tableName As String, ByVal columnName As String)
            If isColumnExist(columnName, tableName) = True Then Exit Sub
            ExecuteNonQuery("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " ;")
        End Sub
        Public Sub addColumn(ByVal tableName As String, ByVal columnName As String, ByVal columnType As String)
            If isColumnExist(columnName, tableName) = True Then Exit Sub
            ExecuteNonQuery("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + columnType + ";")
        End Sub
        Public Sub addColumn(ByVal tableName As String, ByVal columnName As String, ByVal columnType As String, ByVal DefaultValue As String)
            If isColumnExist(columnName, tableName) = True Then Exit Sub
            ExecuteNonQuery("ALTER TABLE( " + tableName + " ADD COLUMN " + columnName + " " + columnType + " DEFAULT" + DefaultValue + ");")
        End Sub
        ''' <summary>
        '''     Allows the programmer to run a query against the Database.
        ''' </summary>
        ''' <param name="sql">The SQL to run</param>
        ''' <returns>A DataTable containing the result set.</returns>
        Public Shared Function GetDataTable(sql As String) As DataTable
            Dim dt As New DataTable()

            Try
                Using cnn As New SQLiteConnection(dbConnection)
                    cnn.Open()
                    Dim mycommand As New SQLiteCommand(cnn)
                    mycommand.CommandText = sql
                    Dim reader As SQLiteDataReader = mycommand.ExecuteReader()
                    dt.Load(reader)
                    reader.Close()
                    cnn.Close()
                    cnn.Dispose()
                End Using
            Catch e As Exception
                Throw e
            End Try
            Return dt
        End Function

        Public Function GetValueFromTable(str As String) As Dictionary(Of String, Object)
            'Dim str As String = "SELECT * FROM FURRE WHERE WHERE =" & Name & ";"
            Dim test3 As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            Using cnn As New SQLiteConnection(dbConnection)
                cnn.Open()
                Dim mycommand As New SQLiteCommand(cnn)
                mycommand.CommandText = str


                Dim reader As SQLiteDataReader = Nothing
                Try
                    reader = mycommand.ExecuteReader()
                    Dim Size As Integer = 0

                    While reader.Read()
                        Size = reader.VisibleFieldCount
                        For i As Integer = 0 To Size - 1
                            test3.Add(reader.GetName(i), reader.GetValue(i).ToString)
                        Next

                    End While

                Catch ex As Exception
                    cnn.Close()
                    cnn.Dispose()
                    Throw ex
                    Return Nothing
                    'Console.WriteLine(ex.Message)
                End Try

                cnn.Close()
                cnn.Dispose()
            End Using
            Return test3
        End Function

        ''' <summary>
        '''     Allows the programmer to interact with the database for purposes other than a query.
        ''' </summary>
        ''' <param name="sql">The SQL to be run.</param>
        ''' <returns>An Integer containing the number of rows updated.</returns>
        Public Shared Function ExecuteNonQuery(sql As String) As Integer
            Dim rowsUpdated As Integer
            Using cnn As New SQLiteConnection(dbConnection)
                cnn.Open()
                Using cmd As SQLiteCommand = cnn.CreateCommand()
                    cmd.CommandText = "PRAGMA synchronous=0;"
                    cmd.ExecuteNonQuery()
                End Using

                Using mycommand As New SQLiteCommand(cnn)
                    mycommand.CommandText = sql
                    rowsUpdated = mycommand.ExecuteNonQuery()
                End Using

                cnn.Close()
                cnn.Dispose()
            End Using
            Return rowsUpdated
        End Function

        '''<summary>
        ''' 
        ''' </summary>
        ''' <param name="tableName">
        ''' 
        ''' </param>
        ''' <returns>
        ''' 
        ''' </returns>
        Public Function isTableExists(tableName As [String]) As [Boolean]
            Return ExecuteNonQuery("SELECT name FROM sqlite_master WHERE name='" & tableName & "'") > 0
        End Function
        ''' <summary>
        '''     Allows the programmer to retrieve single items from the DB.
        ''' </summary>
        ''' <param name="sql">The query to run.</param>
        ''' <returns>A string.</returns>
        Public Shared Function ExecuteScalar1(ByVal sql As String) As String
            Dim Value As Object = Nothing
            Using cnn As New SQLiteConnection(dbConnection)
                cnn.Open()
                Using cmd As SQLiteCommand = cnn.CreateCommand()
                    cmd.CommandText = "PRAGMA synchronous=0;"
                    cmd.ExecuteNonQuery()
                End Using

                Using mycommand As New SQLiteCommand(cnn)
                    mycommand.CommandText = sql

                    Try
                        Value = mycommand.ExecuteScalar()
                    Catch Ex As Exception
                        ' Throw New Exception(Ex.Message)
                        cnn.Close()
                        Return ""
                    Finally
                        'cnn.Close()
                    End Try
                End Using

                cnn.Close()
                cnn.Dispose()
            End Using
            If Value IsNot Nothing Then
                Return Value.ToString()
            End If
            Return ""
        End Function

        ''' <summary>
        '''     Allows the programmer to easily update rows in the DB.
        ''' </summary>
        ''' <param name="tableName">The table to update.</param>
        ''' <param name="data">A dictionary containing Column names and their new values.</param>
        ''' <param name="where">The where clause for the update statement.</param>
        ''' <returns>A boolean true or false to signify success or failure.</returns>
        Public Function Update(tableName As [String], data As Dictionary(Of [String], [String]), where As [String]) As Boolean
            Dim vals As [String] = ""
            Dim returnCode As [Boolean] = True
            If data.Count >= 1 Then
                For Each val As KeyValuePair(Of [String], [String]) In data
                    vals += [String].Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString())
                Next
                vals = vals.Substring(0, vals.Length - 1)
            End If
            Try
                Dim cmd As String = [String].Format("update {0} set {1} where {2};", tableName, vals, where)
                SQLiteDatabase.ExecuteNonQuery(cmd)

            Catch
                returnCode = False
            End Try
            Return returnCode
        End Function

        ''' <summary>
        '''     Allows the programmer to easily delete rows from the DB.
        ''' </summary>
        ''' <param name="tableName">The table from which to delete.</param>
        ''' <param name="where">The where clause for the delete.</param>
        ''' <returns>A boolean true or false to signify success or failure.</returns>
        Public Function Delete(tableName As [String], where As [String]) As Boolean
            Dim returnCode As [Boolean] = True
            Try
                SQLiteDatabase.ExecuteNonQuery([String].Format("delete from {0} where {1};", tableName, where))
            Catch fail As Exception
                MessageBox.Show(fail.Message)
                returnCode = False
            End Try
            Return returnCode
        End Function

        ''' <summary>
        '''     Allows the programmer to easily insert into the DB
        ''' </summary>
        ''' <param name="tableName">The table into which we insert the data.</param>
        ''' <param name="data">A dictionary containing the column names and data for the insert.</param>
        ''' <returns>A boolean true or false to signify success or failure.</returns>
        Public Function Insert(tableName As [String], data As Dictionary(Of [String], [String])) As Boolean
            Dim columns As New ArrayList
            Dim values As New ArrayList
            For Each val As KeyValuePair(Of [String], [String]) In data
                columns.Add([String].Format(" {0}", val.Key.ToString()))
                values.Add([String].Format(" '{0}'", val.Value))
            Next
            Try
                Dim cmd As String = [String].Format("INSERT OR IGNORE into {0}({1}) values({2});", tableName, String.Join(", ", columns.ToArray), String.Join(", ", values.ToArray))
                SQLiteDatabase.ExecuteNonQuery(cmd)
                Return True
            Catch fail As Exception
                MessageBox.Show(fail.Message)
                Return False
            End Try
            Return True
        End Function




        ''' <summary>
        '''     Allows the programmer to easily delete all data from the DB.
        ''' </summary>
        ''' <returns>A boolean true or false to signify success or failure.</returns>
        Public Function ClearDB() As Boolean
            Dim tables As DataTable
            Try
                tables = SQLiteDatabase.GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;")
                For Each table As DataRow In tables.Rows
                    Me.ClearTable(table("NAME").ToString())
                Next
                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        '''     Allows the user to easily clear all data from a specific table.
        ''' </summary>
        ''' <param name="table">The name of the table to clear.</param>
        ''' <returns>A boolean true or false to signify success or failure.</returns>
        Public Function ClearTable(table As [String]) As Boolean
            Try
                SQLiteDatabase.ExecuteNonQuery([String].Format("delete from {0};", table))
                Return True
            Catch
                Return False
            End Try
        End Function


    End Class
End Class
