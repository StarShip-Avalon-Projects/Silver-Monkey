Imports System.Data.Common
Imports System.Data.SQLite
Imports System.IO
Imports IO
Imports Logging

''' <summary>
''' Monkey Systems generic interface to System.Data.Sqlite
''' </summary>
Public Class SQLiteDatabase

#Region "Public Fields"

    Public Const PlayerDbName As String = "Name"

#End Region

#Region "Private Fields"

    Private Const DefaultFile As String = "SilverMonkey.db"
    Private Const FurreTable As String = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [Access Level] INTEGER, [date added] TEXT, [date modified] TEXT, [PSBackup] DOUBLE"
    Private Const SyncPragma As String = "PRAGMA encoding = ""UTF-16""; " 'PRAGMA synchronous=0;
    Private Shared dbConnection As String
    Private Shared ExecuteScarlarLock As New Object
    Private Shared insertLock As New Object
    Private Shared nonQueryLock As New Object
    Private Shared QueryLock As New Object
    Private Shared writer As TextBoxWriter = Nothing
    Dim lock As New Object

#End Region

#Region "Public Constructors"

    ''' <summary>
    ''' Default Constructor for SQLiteDatabase Class.
    ''' </summary>
    Public Sub New()
        Dim inputFile As String = Path.Combine(Paths.SilverMonkeyBotPath, DefaultFile)
        dbConnection = "Data Source=" & inputFile
        CreateTbl("FURRE", FurreTable)
        CreateTbl("BACKUPMASTER", "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [" + PlayerDbName + "] TEXT Unique, [date modified] TEXT")
        CreateTbl("BACKUP", "[NameID] INTEGER, [Key] TEXT, [Value] TEXT, PRIMARY KEY ([NameID],[Key])")
        CreateTbl("SettingsTableMaster", "[ID] INTEGER UNIQUE, [SettingsTable] TEXT Unique, [date modified] TEXT, PRIMARY KEY ([ID],[SettingsTable])")
        CreateTbl("SettingsTable", "[ID] INTEGER UNIQUE,[SettingsTableID] INTEGER, [Setting] TEXT, [Value] TEXT")
    End Sub

    ''' <summary>
    ''' Single Param Constructor for specifying the DB file.
    ''' </summary>
    ''' <param name="inputFile">
    ''' The File containing the DB
    ''' </param>
    Public Sub New(inputFile As String)

        If String.IsNullOrEmpty(inputFile) Then
            inputFile = Path.Combine(Paths.SilverMonkeyBotPath, DefaultFile)
        End If
        Dim dir As String = Path.GetDirectoryName(inputFile)
        If String.IsNullOrEmpty(dir) Then
            inputFile = Path.Combine(Paths.SilverMonkeyBotPath, inputFile)
        End If
        dbConnection = String.Format("Data Source={0};", inputFile)
        CreateTbl("FURRE", FurreTable)
        CreateTbl("BACKUPMASTER", "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [date modified] TEXT")
        CreateTbl("BACKUP", "[NameID] INTEGER, [Key] TEXT, [Value] TEXT, PRIMARY KEY ([NameID],[Key])")
        CreateTbl("SettingsTableMaster", "[ID] INTEGER UNIQUE, [SettingsTable] TEXT Unique, [date modified] TEXT, PRIMARY KEY ([ID],[SettingsTable])")
        CreateTbl("SettingsTable", "[ID] INTEGER UNIQUE,[SettingsID] INTEGER UNIQUE, [Setting] TEXT, [Value] TEXT")
    End Sub

    ''' <summary>
    ''' Single Param Constructor for specifying advanced connection options.
    ''' </summary>
    ''' <param name="connectionOpts">
    ''' A dictionary containing all desired options and their values
    ''' </param>
    Public Sub New(connectionOpts As Dictionary(Of String, String))
        Dim str As String = ""
        For Each row As KeyValuePair(Of String, String) In connectionOpts
            str += String.Format("{0}={1}; ", row.Key, row.Value)
        Next
        str = str.Trim().Substring(0, str.Length - 1)
        dbConnection = str
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Allows the programmer to interact with the database for purposes
    ''' other than a query.
    ''' </summary>
    ''' <param name="sql">
    ''' The SQL to be run.
    ''' </param>
    ''' <returns>
    ''' An Integer containing the number of rows updated.
    ''' </returns>
    Public Shared Function ExecuteNonQuery(sql As String) As Integer
        Dim rowsUpdated As Integer

        Using cnn As New SQLiteConnection(dbConnection)
            cnn.Open()
            Using mycommand As New SQLiteCommand(cnn)
                Try
                    mycommand.CommandText = sql
                    rowsUpdated = mycommand.ExecuteNonQuery()
                Catch ex As SQLiteException
                    rowsUpdated = -1
                End Try
                cnn.Close()
            End Using
        End Using

        Return rowsUpdated
    End Function

    ''' <summary>
    ''' Allows the programmer to retrieve single items from the DB.
    ''' </summary>
    ''' <param name="sql">
    ''' The query to run.
    ''' </param>
    ''' <returns>
    ''' A string.
    ''' </returns>
    Public Shared Function ExecuteScalar(ByVal sql As String) As String
        Dim value As Object
        Using cnn As New SQLiteConnection(dbConnection)
            cnn.Open()
            Using mycommand As New SQLiteCommand(cnn)
                mycommand.CommandText = sql
                value = mycommand.ExecuteScalar()

            End Using
            cnn.Close()
        End Using

        If value IsNot Nothing Then
            Return value.ToString()
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Allows the programmer to run a query against the Database.
    ''' </summary>
    ''' <param name="sql">
    ''' The SQL to run
    ''' </param>
    ''' <returns>
    ''' A DataTable containing the result set.
    ''' </returns>
    Public Shared Function GetDataTable(sql As String) As DataTable
        Dim dt = New DataTable()
        Using cnn As New SQLiteConnection(dbConnection)
            cnn.Open()
            Using mycommand As New SQLiteCommand(cnn)
                mycommand.CommandText = sql
                Dim reader As SQLiteDataReader
                Try
                    reader = mycommand.ExecuteReader()
                    dt.Load(reader)
                    reader.Close()
                Catch ex As Exception
                    dt = Nothing
                End Try
            End Using
            cnn.Close()
        End Using

        Return dt
    End Function

    ''' <summary>
    ''' Allows the programmer to easily insert into the DB
    ''' </summary>
    ''' <param name="tableName">
    ''' The table into which we insert the data.
    ''' </param>
    ''' <param name="data">
    ''' A dictionary containing the column names and data for the insert.
    ''' </param>
    ''' <returns>
    ''' A boolean true or false to signify success or failure.
    ''' </returns>
    Public Shared Function InsertMultiRow(tableName As String, ID As Integer, data As Dictionary(Of String, String)) As Boolean
        Dim values As New List(Of String)
        Dim i As Integer = 0
        Try

            ' Monitor.Enter(insertLock)

            For Each val As KeyValuePair(Of String, String) In data
                values.Add(String.Format(" ( '{0}', '{1}', '{2}' )", ID, val.Key, val.Value))
            Next

            If values.Count > 0 Then
                'INSERT INTO 'table' ('column1', 'col2', 'col3') VALUES (1,2,3),  (1, 2, 3), (etc);
                Dim cmd As String = String.Format("INSERT into '{0}' ([NameID], [Key], [Value]) Values {1};", tableName, String.Join(", ", values.ToArray))
                i = ExecuteNonQuery(cmd)
            End If
        Finally
            ' Monitor.Exit(insertLock)
        End Try
        ' i = -1 if there's an SQLte error
        Return values.Count <> 0 AndAlso i > -1
    End Function

    ''' <summary>
    ''' Adds a column to the specified table
    ''' </summary>
    ''' <param name="tableName">
    ''' </param>
    ''' <param name="columnName">
    ''' </param>
    Public Sub addColumn(ByVal tableName As String, ByVal columnName As String)
        If isColumnExist(columnName, tableName) = True Then Exit Sub
        ExecuteNonQuery("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " ;")
    End Sub

    ''' <summary>
    ''' Adds a column to the specified table
    ''' </summary>
    ''' <param name="tableName">
    ''' </param>
    ''' <param name="columnName">
    ''' </param>
    ''' <param name="columnType">
    ''' </param>
    Public Sub addColumn(ByVal tableName As String, ByVal columnName As String, ByVal columnType As String)
        If isColumnExist(columnName, tableName) = True Then Exit Sub
        ExecuteNonQuery("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + columnType + ";")
    End Sub

    ''' <summary>
    ''' Adds a column to the specified table
    ''' </summary>
    ''' <param name="tableName">Name of the table.</param>
    ''' <param name="columnName">Name of the column.</param>
    ''' <param name="columnType">Type of the column.</param>
    ''' <param name="DefaultValue">The default value.</param>
    Public Sub addColumn(ByVal tableName As String, ByVal columnName As String, ByVal columnType As String, ByVal DefaultValue As String)
        If isColumnExist(columnName, tableName) = True Then Exit Sub
        ExecuteNonQuery("ALTER TABLE( " + tableName + " ADD COLUMN " + columnName + " " + columnType + " DEFAULT" + DefaultValue + ");")
    End Sub

    ''' <summary>
    ''' Allows the programmer to easily delete all data from the DB.
    ''' </summary>
    ''' <returns>
    ''' A boolean true or false to signify success or failure.
    ''' </returns>
    Public Function ClearDB() As Boolean

        Using tables As DataTable = GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;")
            For Each table As DataRow In tables.Rows
                ClearTable(table("NAME").ToString())
            Next
            Return True
        End Using
        Return False

    End Function

    ''' <summary>
    ''' Allows the user to easily clear all data from a specific table.
    ''' </summary>
    ''' <param name="table">
    ''' The name of the table to clear.
    ''' </param>
    ''' <returns>
    ''' A boolean true or false to signify success or failure.
    ''' </returns>
    Public Shared Function ClearTable(table As String) As Boolean
        Try
            Return ExecuteNonQuery(String.Format("delete from {0};", table)) > -1
        Catch
            Return False
        End Try
    End Function

    '''<Summary>
    '''    Create a Table with Titles
    ''' </Summary>
    ''' <param name="Table"></param><param name="Titles"></param>
    Public Shared Sub CreateTbl(Table As String, ByRef Titles As String)
        Using SQLconnect As New SQLiteConnection(dbConnection)
            Using SQLcommand As SQLiteCommand = SQLconnect.CreateCommand
                SQLconnect.Open()
                'SQL query to Create Table
                ' [Access Level] INTEGER, [date added] TEXT, [date modified] TEXT,
                SQLcommand.CommandText = SyncPragma + "CREATE TABLE IF NOT EXISTS " & Table & "( " & Titles & " );"
                SQLcommand.ExecuteNonQuery()
            End Using
            SQLconnect.Close()
        End Using
        '
    End Sub

    ''' <summary>
    ''' Allows the programmer to easily delete rows from the DB.
    ''' </summary>
    ''' <param name="tableName">
    ''' The table from which to delete.
    ''' </param>
    ''' <param name="where">
    ''' The where clause for the delete.
    ''' </param>
    ''' <returns>
    ''' A boolean true or false to signify success or failure.
    ''' </returns>
    Public Function Delete(tableName As String, where As String) As Boolean

        Try
            Return ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where)) > -1
        Catch ex As Exception
            Monkeyspeak.Logging.Logger.Error(Of SQLiteDatabase)(ex)

        End Try
        Return False
    End Function

    ''' <summary>
    ''' Executes the query.
    ''' </summary>
    ''' <param name="sql">
    ''' The SQL.
    ''' </param>
    ''' <returns>
    ''' </returns>
    Public Function ExecuteQuery(sql As String) As DataSet
        Dim rowsUpdated = New DataSet
        ' Monitor.Enter(QueryLock)
        Using cnn As New SQLiteConnection(dbConnection)
            cnn.Open()
            Using mycommand As New SQLiteCommand(cnn)
                mycommand.CommandText = SyncPragma + sql
                Using a As DataAdapter = New SQLiteDataAdapter(mycommand)
                    Try
                        a.Fill(rowsUpdated)
                    Catch ex As SQLiteException

                        rowsUpdated = Nothing

                        ' Finally a.Dispose()
                    End Try
                End Using
            End Using
            cnn.Close()
        End Using

        ' Monitor.Exit(QueryLock)
        Return rowsUpdated
    End Function

    ''' <summary>
    ''' Get a set of values from the specified table
    ''' </summary>
    ''' <param name="str">
    ''' </param>
    ''' <returns>
    ''' a dictionary of values
    ''' </returns>
    Public Function GetValueFromTable(str As String) As Dictionary(Of String, Object)
        'Dim str As String = "SELECT * FROM FURRE WHERE WHERE =" & Name & ";"
        Dim test3 As Dictionary(Of String, Object) = Nothing
        Using cnn As New SQLiteConnection(dbConnection)
            cnn.Open()
            Using mycommand As New SQLiteCommand(cnn)
                mycommand.CommandText = str
                Using reader As SQLiteDataReader = mycommand.ExecuteReader()
                    Dim Size As Integer = 0
                    test3 = New Dictionary(Of String, Object)
                    While reader.Read()
                        Size = reader.VisibleFieldCount
                        For i As Integer = 0 To Size - 1
                            test3.Add(reader.GetName(i), reader.GetValue(i).ToString)
                        Next
                    End While
                    reader.Close()
                End Using
            End Using
            cnn.Close()
        End Using

        Return test3
    End Function

    ''' <summary>
    ''' Allows the programmer to easily insert into the DB
    ''' </summary>
    ''' <param name="tableName">
    ''' The table into which we insert the data.
    ''' </param>
    ''' <param name="data">
    ''' A dictionary containing the column names and data for the insert.
    ''' </param>
    ''' <returns>
    ''' A boolean true or false to signify success or failure.
    ''' </returns>
    Public Function Insert(tableName As String, data As Dictionary(Of String, String)) As Boolean
        Dim columns As New List(Of String)
        Dim values As New List(Of String)
        For Each val As KeyValuePair(Of String, String) In data
            columns.Add(String.Format("[{0}]", val.Key))
            values.Add(String.Format("'{0}'", val.Value))
        Next
        Try
            Dim cmd As String = String.Format("INSERT OR IGNORE into {0}({1}) values({2});", tableName, String.Join(", ", columns.ToArray()), String.Join(", ", values.ToArray()))
            Return ExecuteNonQuery(cmd) > -1
        Catch ex As Exception
            Dim err As New ErrorLogging(ex, Me)
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Does the Column name exist in the specified table
    ''' </summary>
    ''' <param name="columnName">
    ''' </param>
    ''' <param name="tableName">
    ''' </param>
    ''' <returns>
    ''' </returns>
    Public Function isColumnExist(ByVal columnName As String, ByVal tableName As String) As Boolean
        Dim columnNames As String = getAllColumnName(tableName)
        Return columnNames.Contains(columnName)
    End Function

    ''' <summary>
    ''' Determines whether [is table exists] [the specified table name].
    ''' </summary>
    ''' <param name="tableName">
    ''' Name of the table.
    ''' </param>
    ''' <returns>
    ''' True if ExecuteNonQurey returns one or more tables
    ''' </returns>
    Public Function isTableExists(tableName As String) As Boolean
        Return ExecuteNonQuery("SELECT name FROM sqlite_master WHERE name='" & tableName & "'") > 0
    End Function

    ''' <summary>
    ''' removes a column of data from the specified table
    ''' </summary>
    ''' <param name="tableName">
    ''' </param>
    ''' <param name="columnName">
    ''' </param>
    ''' <returns>
    ''' </returns>
    Public Function RemoveColumn(ByVal tableName As String, ByVal columnName As String) As Integer
        Dim columnNames As String = getAllColumnName(tableName)
        If Not columnNames.Contains(columnName) Then
            Return -1
        End If
        columnNames = columnNames.Replace(columnName + ", ", "")
        columnNames = columnNames.Replace(", " + columnName, "")
        columnNames = columnNames.Replace(columnName, "").Replace("[],", "").Replace(",[]", "")
        Return ExecuteNonQuery("CREATE TEMPORARY TABLE " + tableName + "backup(" + columnNames + ");" +
            "INSERT INTO " + tableName + "backup SELECT " + columnNames + " FROM " + tableName + ";" +
            "DROP TABLE " + tableName + ";" +
            "CREATE TABLE " + tableName + "(" + columnNames + ");" +
            "INSERT INTO " + tableName + " SELECT " + columnNames + " FROM " + tableName + "backup;" +
            "DROP TABLE " + tableName + "backup;")
    End Function

    ''' <summary>
    ''' Allows the programmer to easily update rows in the DB.
    ''' </summary>
    ''' <param name="tableName">
    ''' The table to update.
    ''' </param>
    ''' <param name="data">
    ''' A dictionary containing Column names and their new values.
    ''' </param>
    ''' <param name="where">
    ''' The where clause for the update statement.
    ''' </param>
    ''' <returns>
    ''' A boolean true or false to signify success or failure.
    ''' </returns>
    Public Function Update(tableName As String, data As Dictionary(Of String, String), where As String) As Boolean
        Dim vals As New List(Of String)
        If data.Count = 0 Then Return False
        For Each val As KeyValuePair(Of String, String) In data
            vals.Add(String.Format("'{0}'='{1}'", val.Key, val.Value))
        Next
        Try
            Dim cmd As String = String.Format("update {0} set {1} where {2};", tableName, String.Join(", ", vals.ToArray), where)
            Return ExecuteNonQuery(cmd) < -1
        Catch ex As Exception
            Dim err As New ErrorLogging(ex, Me)
            Return False
        End Try
    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' gets all table column name in a string
    ''' </summary>
    ''' <param name="tableName">
    ''' </param>
    ''' <returns>
    ''' </returns>
    Private Function getAllColumnName(ByVal tableName As String) As String
        Dim sql As String = SyncPragma + "SELECT * FROM " & tableName
        Dim columnNames As New List(Of String)
        Using SQLconnect As New SQLiteConnection(dbConnection)
            SQLconnect.Open()
            Using SQLcommand As DbCommand = SQLconnect.CreateCommand
                SQLcommand.CommandText = sql
                Using sqlDataReader As DbDataReader = SQLcommand.ExecuteReader()
                    For i As Integer = 0 To sqlDataReader.VisibleFieldCount - 1
                        columnNames.Add("[" + sqlDataReader.GetName(i) + "]")
                    Next i
                    sqlDataReader.Close()
                End Using
            End Using
            SQLconnect.Close()
        End Using

        Return String.Join(",", columnNames.ToArray)
    End Function

#End Region

End Class