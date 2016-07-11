Imports System.IO
Imports Furcadia.IO
Imports MonkeySpeakEditor.IniFile

Public Class ConfigStructs


    Public Shared Function pPath() As String
        Dim str As String = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                      "Furcadia Framework/SilverMonkey")
        Directory.CreateDirectory(str)
        Return str
    End Function
    Public Shared SetFile As String = pPath() & "/Settings.Ini"
    Public Shared Function mPath() As String
        mPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Furcadia")
        Directory.CreateDirectory(mPath)
        Return mPath
    End Function
    Public Shared Function msPath() As String
        msPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Silver Monkey")
        Directory.CreateDirectory(mPath)
        Return msPath
    End Function
    Public Class EditSettings

        Private _FurcPath As String
        'Dragon Speak
        Private _IDcolor As Color
        Private _CommentColor As Color
        Private _StringColor As Color
        Private _NumberColor As Color
        Private _VariableColor As Color
        Private _StringVariableColor As Color

        'Monkey Speak
        Private _MS_IDcolor As Color
        Private _MS_CommentColor As Color
        Private _MS_StringColor As Color
        Private _MS_NumberColor As Color
        Private _MS_VariableColor As Color

        Private _AutoCompleteEnable As Boolean

        Public Property FurcPath As String
            Get
                Return _FurcPath
            End Get
            Set(value As String)
                _FurcPath = value
            End Set
        End Property

        Public Property AutoCompleteEnable As Boolean
            Get
                Return _AutoCompleteEnable
            End Get
            Set(value As Boolean)
                _AutoCompleteEnable = value
            End Set
        End Property

        Public Property IDColor() As Color
            Get
                Return _IDcolor
            End Get
            Set(ByVal value As Color)
                _IDcolor = value
            End Set
        End Property
        Public Property StringColor() As Color
            Get
                Return _StringColor
            End Get
            Set(ByVal value As Color)
                _StringColor = value
            End Set
        End Property
        Public Property VariableColor() As Color
            Get
                Return _VariableColor
            End Get
            Set(ByVal value As Color)
                _VariableColor = value
            End Set
        End Property
        Public Property StringVariableColor() As Color
            Get
                Return _StringVariableColor
            End Get
            Set(ByVal value As Color)
                _StringVariableColor = value
            End Set
        End Property
        Public Property NumberColor() As Color
            Get
                Return _NumberColor
            End Get
            Set(ByVal value As Color)
                _NumberColor = value
            End Set
        End Property
        Public Property CommentColor() As Color
            Get
                Return _CommentColor
            End Get
            Set(ByVal value As Color)
                _CommentColor = value
            End Set
        End Property

        'MonkeySpeak
        Public Property MS_IDColor() As Color
            Get
                Return _MS_IDcolor
            End Get
            Set(ByVal value As Color)
                _MS_IDcolor = value
            End Set
        End Property
        Public Property MS_StringColor() As Color
            Get
                Return _MS_StringColor
            End Get
            Set(ByVal value As Color)
                _MS_StringColor = value
            End Set
        End Property
        Public Property MS_VariableColor() As Color
            Get
                Return _MS_VariableColor
            End Get
            Set(ByVal value As Color)
                _MS_VariableColor = value
            End Set
        End Property

        Public Property MS_NumberColor() As Color
            Get
                Return _MS_NumberColor
            End Get
            Set(ByVal value As Color)
                _MS_NumberColor = value
            End Set
        End Property
        Public Property MS_CommentColor() As Color
            Get
                Return _MS_CommentColor
            End Get
            Set(ByVal value As Color)
                _MS_CommentColor = value
            End Set
        End Property

        Public Property PluginList As Dictionary(Of String, Boolean)

        Public Sub New()

            Dim s As String = ""
            ini.AddSection("Editor").AddKey("IDColor").Value = Color.Blue.ToArgb.ToString
            ini.AddSection("Editor").AddKey("CommentColor").Value = Color.Green.ToArgb.ToString
            ini.AddSection("Editor").AddKey("StringColor").Value = Color.Red.ToArgb.ToString
            ini.AddSection("Editor").AddKey("VariableColor").Value = Color.DarkGray.ToArgb.ToString
            ini.AddSection("Editor").AddKey("StringVariableColor").Value = Color.Blue.ToArgb.ToString
            ini.AddSection("Editor").AddKey("NumberColor").Value = Color.Brown.ToArgb.ToString

            'MonkeySpeak
            ini.AddSection("MonkeySpeak").AddKey("IDColor").Value = Color.Blue.ToArgb.ToString
            ini.AddSection("MonkeySpeak").AddKey("CommentColor").Value = Color.Green.ToArgb.ToString
            ini.AddSection("MonkeySpeak").AddKey("StringColor").Value = Color.Red.ToArgb.ToString
            ini.AddSection("MonkeySpeak").AddKey("VariableColor").Value = Color.DarkGray.ToArgb.ToString
            ini.AddSection("MonkeySpeak").AddKey("NumberColor").Value = Color.Brown.ToArgb.ToString.ToString

            ini.AddSection("Editor").AddKey("AutoComplete").Value = True.ToString

            'Dragon Speak
            Dim Count As Integer = KeysIni.GetKeyValue("Init-Types", "Count").ToInteger
            ini.AddSection("Init-Types").AddKey("Count").Value = Count.ToString

            For i As Integer = 1 To Count
                Dim key As String = KeysIni.GetKeyValue("Init-Types", i.ToString)
                Dim val As String = KeysIni.GetKeyValue("Indent-Lookup", key)
                ini.SetKeyValue("Ident-LookUp", key, i.ToString)
                ini.SetKeyValue("Init-Types", i.ToString, key)

                Dim dvalue As Integer = KeysIni.GetKeyValue("C-Indents", key).ToInteger
                ini.AddSection("C-Indents")
                ini.SetKeyValue("C-Indents", key, dvalue.ToString)

            Next

            'MonkeySpeak\
            Count = MS_KeysIni.GetKeyValue("Init-Types", "Count").ToInteger
            ini.AddSection("MS-Init-Types").AddKey("Count").Value = Count.ToString
            For i As Integer = 1 To Count
                Dim key As String = MS_KeysIni.GetKeyValue("Init-Types", i.ToString)
                Dim val As String = MS_KeysIni.GetKeyValue("Indent-Lookup", key)
                ini.AddSection("MS-Init-Types").AddKey(i.ToString).Value = key
                ini.AddSection("MS-Indent-Lookup").AddKey(key).Value = val

                Dim dvalue As Integer = MS_KeysIni.GetKeyValue("C-Indents", key).ToInteger
                ini.AddSection("MS-C-Indents").AddKey(key).Value = dvalue.ToString

            Next

            If File.Exists(SetFile) Then
                ini.Load(SetFile, True)
            End If



            'Dragon Speak
            _IDcolor = ColorTranslator.FromHtml(ini.GetKeyValue("Editor", "IDColor"))
            _CommentColor = ColorTranslator.FromHtml(ini.GetKeyValue("Editor", "CommentColor"))
            _StringColor = ColorTranslator.FromHtml(ini.GetKeyValue("Editor", "StringColor"))
            _VariableColor = ColorTranslator.FromHtml(ini.GetKeyValue("Editor", "VariableColor"))
            _StringVariableColor = ColorTranslator.FromHtml(ini.GetKeyValue("Editor", "StringVariableColor"))
            _NumberColor = ColorTranslator.FromHtml(ini.GetKeyValue("Editor", "NumberColor"))

            'Monkey Speak
            _MS_IDcolor = ColorTranslator.FromHtml(ini.GetKeyValue("MonkeySpeak", "IDColor"))
            _MS_CommentColor = ColorTranslator.FromHtml(ini.GetKeyValue("MonkeySpeak", "CommentColor"))
            _MS_StringColor = ColorTranslator.FromHtml(ini.GetKeyValue("MonkeySpeak", "StringColor"))
            _MS_VariableColor = ColorTranslator.FromHtml(ini.GetKeyValue("MonkeySpeak", "VariableColor"))
            _MS_NumberColor = ColorTranslator.FromHtml(ini.GetKeyValue("MonkeySpeak", "NumberColor"))

            _AutoCompleteEnable = Convert.ToBoolean(ini.GetKeyValue("Editor", "AutoComplete"))

            Dim DSSection As IniSection = ini.GetSection("plugins")
            If Not IsNothing(DSSection) Then
                PluginList = New Dictionary(Of String, Boolean)
                For Each K As IniSection.IniKey In DSSection.Keys
                    s = K.Value
                    If Not String.IsNullOrEmpty(s) Then
                        PluginList.Add(K.Name, Convert.ToBoolean(K.Value))
                    Else
                        PluginList.Add(K.Name, True)
                    End If
                Next
            End If

            s = ini.GetKeyValue("Main", "FurcPath")
            If Not String.IsNullOrEmpty(s) Then _FurcPath = s
        End Sub

        Public Sub SaveEditorSettings()
            'Main Settings
            ini.SetKeyValue("Main", "FurcPath", _FurcPath)

            ini.SetKeyValue("Editor", "IDColor", ColorTranslator.ToHtml(_IDcolor).ToString)
            ini.SetKeyValue("Editor", "NumberColor", ColorTranslator.ToHtml(_NumberColor).ToString)
            ini.SetKeyValue("Editor", "StringColor", ColorTranslator.ToHtml(_StringColor).ToString)
            ini.SetKeyValue("Editor", "VariableColor", ColorTranslator.ToHtml(_VariableColor).ToString)
            ini.SetKeyValue("Editor", "StringVariableColor", ColorTranslator.ToHtml(_StringVariableColor).ToString)
            ini.SetKeyValue("Editor", "CommentColor", ColorTranslator.ToHtml(_CommentColor).ToString)

            ini.SetKeyValue("MonkeySpeak", "IDColor", ColorTranslator.ToHtml(_MS_IDcolor).ToString)
            ini.SetKeyValue("MonkeySpeak", "NumberColor", ColorTranslator.ToHtml(_MS_NumberColor).ToString)
            ini.SetKeyValue("MonkeySpeak", "StringColor", ColorTranslator.ToHtml(_MS_StringColor).ToString)
            ini.SetKeyValue("MonkeySpeak", "VariableColor", ColorTranslator.ToHtml(_MS_VariableColor).ToString)
            ini.SetKeyValue("MonkeySpeak", "CommentColor", ColorTranslator.ToHtml(_MS_CommentColor).ToString)

            ini.SetKeyValue("Editor", "AutoComplete", _AutoCompleteEnable.ToString)

            ini.RemoveSection("Plugins")
            For Each kv As KeyValuePair(Of String, Boolean) In PluginList
                ini.SetKeyValue("Plugins", kv.Key, kv.Value.ToString)
            Next

            ini.Save(SetFile)
        End Sub

    End Class


End Class
