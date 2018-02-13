Imports System.Data.SQLite
Imports System.IO
Imports System.Windows.Forms

Public Class SQLiteDatabase

    Private Const FurreTable As String = "[ID] INTEGER PRIMARY KEY AUTOINCREMENT, [Name] TEXT Unique, [Access Level] INTEGER, [date added] TEXT, [date modified] TEXT, [PSBackup] DOUBLE"

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
        dbConnection = [String].Format("Data Source={0};", inputFile)
        If Not File.Exists(inputFile) Then
            CreateTbl("FURRE", FurreTable)
        End If
    End Sub



    '''<Summary>
    '''    Create a Table
    ''' </Summary>
    ''' <param name="Table"></param>
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

    Public Function ExecuteQuery(sql As String) As DataSet
        Dim rowsUpdated As New DataSet
        Using cnn As New SQLiteConnection(dbConnection)
            cnn.Open()
            Using mycommand As New SQLiteCommand(cnn)
                mycommand.CommandText = "PRAGMA synchronous=0;" + sql
                Using a As SQLiteDataAdapter = New SQLiteDataAdapter(mycommand)
                    a.Fill(rowsUpdated)
                End Using
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

        Try
            Return 0 < SQLiteDatabase.ExecuteNonQuery([String].Format("delete from {0} where {1};", tableName, where))
        Catch fail As Exception
            MessageBox.Show(fail.Message)

        End Try
        Return False
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
