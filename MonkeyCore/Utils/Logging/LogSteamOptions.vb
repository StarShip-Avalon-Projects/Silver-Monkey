Imports MonkeyCore2.IO

Namespace Utils.Logging

    ''' <summary>
    ''' Logging options for Bot Logs
    ''' </summary>
    <CLSCompliant(True)>
    Public Class LogSteamOptions
        Private Const ext As String = ".log"
        Private _log As Boolean
        Private _logIdx As Integer
        Private _logNamebase As String
        Private _logOption As Short
        Private _logPath As String

        Sub New()
            _logNamebase = "Default"
            _log = False
            _logPath = Paths.SilverMonkeyLogPath
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating whether this <see cref="LogSteamOptions"/> is enabled.
        ''' </summary>
        ''' <value>
        '''   <c>true</c> if enabled; otherwise, <c>false</c>.
        ''' </value>
        Public Property Enabled As Boolean
            Get
                Return _log
            End Get
            Set(ByVal value As Boolean)
                _log = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the index of the log.
        ''' </summary>
        ''' <value>
        ''' The index of the log.
        ''' </value>
        Public Property LogIdx As Integer
            Get
                Return _logIdx
            End Get
            Set(value As Integer)
                _logIdx = value
            End Set
        End Property

        ''' <summary>
        ''' Base Filename to build the log file name
        ''' <para/>
        ''' Generally this is BotName
        ''' </summary>
        ''' <returns></returns>
        Public Property LogNameBase As String
            Get
                Return _logNamebase
            End Get
            Set(value As String)
                _logNamebase = value
            End Set
        End Property

        Public Property LogOption As Short
            Get
                Return _logOption
            End Get
            Set(value As Short)
                _logOption = value
            End Set
        End Property

        ''' <summary>
        ''' File path to the Log folder.
        ''' <para/>
        ''' Default : Documents\Silver Monkey\Logs
        ''' </summary>
        ''' <returns></returns>
        Public Property LogPath As String
            Get
                Return _logPath
            End Get
            Set(value As String)
                _logPath = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the FileName for the LogFile
        ''' </summary>
        ''' <returns>
        ''' </returns>
        Public Function GetLogName() As String
            Dim LogFile As String = Nothing
            Select Case _logOption
                Case 0
                    LogFile = _logNamebase
                Case 1
                    _logIdx += 1

                    LogFile = LogNameBase & LogIdx.ToString
                Case 2
                    LogFile = LogNameBase & Date.Now().ToString("MM_dd_yyyy_H-mm-ss")
            End Select

            Return LogFile + ext
        End Function

    End Class

End Namespace