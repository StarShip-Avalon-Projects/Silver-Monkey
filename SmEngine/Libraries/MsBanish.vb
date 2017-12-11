Imports Monkeyspeak

Namespace Engine.Libraries

    ''' <summary>
    ''' Cause:s (0:50) - (0:62
    ''' <para>
    ''' Conditions: (1:50) - (1:53)
    ''' </para>
    ''' Effects: (5:49) - (5:56)
    ''' <para>
    ''' Banish Monkey Speak
    ''' </para>
    ''' This system mirrors Furcadia's banish system by tracking the banish
    ''' commands sent to the game aerver and keep a list of banished furres
    ''' locally. To help keep the list accurate, It is reconmended to ask
    ''' the server for a banish list upon joining the dream. It maybe a good
    ''' idea to run a daily schedule to refresh the list for temp banishes
    ''' to drop off.
    ''' </summary>
    ''' <remarks>
    ''' This Lib contains the following unnamed delegates
    ''' <para>
    ''' (0:50) When the bot fails to banish a furre,
    ''' </para>
    ''' (0:54) When the bot sees the banish list,
    ''' <para>
    ''' (0:55) When the bot fails to remove a furre from the banish list,
    ''' </para>
    ''' <para>
    ''' (0:57) When the bot successfully removes a furre from the banish list,
    ''' </para>
    ''' (0:59) When the bot fails to empty the banish list,
    ''' <para>
    ''' (0:60) When the bot successfully clears the banish list,
    ''' </para>
    ''' (0:61) When the bot successfully temp banishes a furre,
    ''' </remarks>
    Public NotInheritable Class MsBanish
        Inherits MonkeySpeakLibrary

#Region "Public Constructors"

        Public Overrides Sub Initialize(ParamArray args() As Object)
            '(0: ) When the bot fails to banish a furre,
            Add(TriggerCategory.Cause, 50,
             Function() True, " When the bot fails to banish a furre,")
            '(0: ) When the bot fails to banish the furre named {...},
            Add(TriggerCategory.Cause, 51, AddressOf AndBanishFurreNamed,
                " When the bot fails to banish the furre named {...}")
            '(0: ) When the bot successfully banishes a furre,
            Add(TriggerCategory.Cause, 52,
             Function() True, " When the bot successfully banishes a furre,")
            '(0: ) When the bot successfully banishes the furre named {...},
            Add(TriggerCategory.Cause, 53, AddressOf AndBanishFurreNamed,
                " When the bot successfully banishes the furre named {...},")

            '(0: ) When the bot sees the banish list,
            Add(TriggerCategory.Cause, 54,
            Function() True, " When the bot sees the banish list,")
            '(0: ) When the bot fails to remove a furre from the banish list,
            Add(TriggerCategory.Cause, 55,
             Function() True, " When the bot fails to remove a furre from the banish list,")
            '(0: ) When the bot fails to remove the furre named {...} from the banish list,
            Add(TriggerCategory.Cause, 56, AddressOf AndBanishFurreNamed,
                " When the bot fails to remove the furre named {...} from the banish list,")

            '(0: ) When the bot successfully removes a furre from the banish list,
            Add(TriggerCategory.Cause, 57,
             Function() True, " When the bot successfully removes a furre from the banish list,")
            '(0: ) When the bot successfully removes the furre named {...} from the banish list,
            Add(TriggerCategory.Cause, 58, AddressOf AndBanishFurreNamed,
                " When the bot successfully removes the furre named {...} from the banish list,")
            '(0: ) When the bot fails to empty the banish list,
            Add(TriggerCategory.Cause, 59,
           Function() True, " When the bot fails to empty the banish list,")

            '(0: ) When the bot successfully clears the banish list,
            Add(TriggerCategory.Cause, 60,
         Function() True, " When the bot successfully clears the banish list,")

            '(0: ) When the bot successfully temp banishes a furee,
            Add(TriggerCategory.Cause, 61,
             Function() True, " When the bot successfully temp banishes a furre,")

            '(0:62) When the bot successfully temp banishes the furre named {...},
            Add(TriggerCategory.Cause, 62, AddressOf AndBanishFurreNamed,
                " When the bot successfully temp banishes the furre named {...},")

            '(1:50) and the triggering furre is not on the banish list,
            Add(TriggerCategory.Condition, 50, AddressOf TrigFurreIsNotBanished, " and the triggering furre is not on the banish list,")

            '(1:51) and the triggering furre is on the banish list,
            Add(TriggerCategory.Condition, 51, AddressOf TrigFurreIsBanished, " and the triggering furre is on the banish list,")
            '(1:52) and the furre named {...} is not on the banish list,
            Add(TriggerCategory.Condition, 52, AddressOf FurreNamedIsNotBanished, " and the furre named {...} is not on the banish list,")
            '(1:53) and the furre named {...} is on the banish list,
            Add(TriggerCategory.Condition, 53, AddressOf FurreNamedIsBanished, " and the furre named {...} is on the banish list,")

            ' (5: ) save the banish list to the variable % .
            Add(TriggerCategory.Effect, 49, AddressOf BanishSave,
                " save the banish list to the variable % .")

            '(5:x) as the server for the banish-list.
            Add(TriggerCategory.Effect, 50, AddressOf BanishList,
            " ask the server for the banish-list.")
            '(5:x) banish the triggering furre.
            Add(TriggerCategory.Effect, 51, AddressOf BanishTrigFurre,
            " banish the triggering furre.")
            '(5:x) banish the furre named {...}.
            Add(TriggerCategory.Effect, 52, AddressOf BanishFurreNamed,
            " banish the furre named {...}.")
            '(5:x) temporarily  banish the triggering furre for three days.
            Add(TriggerCategory.Effect, 53, AddressOf TempBanishTrigFurre,
                " temporarily  banish the triggering furre for three days.")

            '(5:x) temporarily banish the furre named {...} for three days.
            Add(TriggerCategory.Effect, 54, AddressOf TempBanishFurreNamed,
                " temporarily banish the furre named {...} for three days.")
            '(5:x) unbanish the triggering furre.
            Add(TriggerCategory.Effect, 55, AddressOf UnBanishTrigFurre,
                " unbanish the triggering furre.")
            '(5:x) unbanish the furre named {...}.
            Add(TriggerCategory.Effect, 56, AddressOf UnBanishFurreNamed,
                " unbanish the furre named {...}.")
        End Sub

        Public Overrides Sub Unload(page As Page)

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' (0:51) When the bot fails to banish the furre named {...}
        ''' <para>
        ''' (0:53) When the bot successfully banishes the furre named {...},
        ''' </para>
        ''' <para>
        ''' (0:56) When the bot fails to remove the furre named {...} from
        ''' the banish list,
        ''' </para>
        ''' <para>
        ''' (0:58) When the bot successfully removes the furre named {...}
        ''' from the banish list,
        ''' </para>
        ''' <para>
        ''' (0:62) When the bot successfully temp banishes the furre named {...},
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on Name Match
        ''' </returns>
        Public Function AndBanishFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = reader.ReadString
            Return Furre.ToFurcadiaShortName() = FurcadiaSession.BanishName.ToFurcadiaShortName()

        End Function

        ''' <summary>
        ''' (5:52) banish the furre named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on sending the banish command
        ''' </returns>
        Public Function BanishFurreNamed(reader As TriggerReader) As Boolean
            Dim Furre As String = reader.ReadString
            Return SendServer("banish " + Furre)

        End Function

        ''' <summary>
        ''' (5:50) ask the server for the banish-list.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on sending the Server Command
        ''' </returns>
        Public Function BanishList(reader As TriggerReader) As Boolean

            Return SendServer("banish-list")

        End Function

        ''' <summary>
        ''' (5:49) save the banish list to the variable % .
        ''' <para>
        ''' Reads the mirror banish list and stores is as a coma separated
        ''' list in a <see cref="MonkeySpeak.Variable"/>
        ''' </para>
        ''' <para>
        ''' It's a good idea to ask the server for the dreams banish list to
        ''' keep this list current
        ''' </para>
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True
        ''' </returns>
        Public Function BanishSave(reader As TriggerReader) As Boolean
            Dim NewVar = reader.ReadVariable(True)

            NewVar.Value = String.Join(" ", FurcadiaSession.BanishList.ToArray)
            Return True
        End Function

        ''' <summary>
        ''' (5:51) banish the triggering furre.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the server command
        ''' </returns>
        Public Function BanishTrigFurre(reader As TriggerReader) As Boolean
            Return SendServer("banish " + Player.Name)

        End Function

        '
        ''' <summary>
        ''' (1:53) and the furre named {...} is on the banish list,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True if the furre is on the mirror banish list
        ''' </returns>
        Public Function FurreNamedIsBanished(reader As TriggerReader) As Boolean
            Return Not FurreNamedIsNotBanished(reader)
        End Function

        ''' <summary>
        ''' (1:52) and the furre named {...} is not on the banish list,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the furre is not on the mirror banish list
        ''' </returns>
        Public Function FurreNamedIsNotBanished(reader As TriggerReader) As Boolean
            Dim banishlist = FurcadiaSession.BanishList

            Dim f As String = reader.ReadString
            For Each Furre In banishlist
                If Furre.ToFurcadiaShortName() = f.ToFurcadiaShortName() Then Return False
            Next
            Return True

        End Function

        ''' <summary>
        ''' (5:54) temporarily banish the furre named {...} for three days.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the server command
        ''' </returns>
        Public Function TempBanishFurreNamed(reader As TriggerReader) As Boolean

            Return SendServer("tempbanish " + Player.Name)

        End Function

        ''' <summary>
        ''' (5:53) temporarily banish the triggering furre for three days.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the server command
        ''' </returns>
        Public Function TempBanishTrigFurre(reader As TriggerReader) As Boolean

            Dim Furre As String = reader.ReadString
            Return SendServer("tempbanish " + Furre)

        End Function

        ''' <summary>
        ''' (1:51) and the triggering furre is on the banish list,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true is the triggering furre is on the mirror banish list
        ''' </returns>
        Public Function TrigFurreIsBanished(reader As TriggerReader) As Boolean
            Return Not TrigFurreIsNotBanished(reader)
        End Function

        ''' <summary>
        ''' (1:50) and the triggering furre is not on the banish list,
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true if the triggering furre is not on the mirror banish list
        ''' </returns>
        Public Function TrigFurreIsNotBanished(reader As TriggerReader) As Boolean
            Dim banishlist As List(Of String) = FurcadiaSession.BanishList

            For Each Furre In banishlist
                If Furre.ToFurcadiaShortName() = Player.ShortName Then Return False
            Next

            Return True
        End Function

        ''' <summary>
        ''' (5:56) unbanish the furre named {...}.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' True on sending the server command
        ''' </returns>
        Public Function UnBanishFurreNamed(reader As TriggerReader) As Boolean

            Dim Furre As String = reader.ReadString
            Return SendServer("banish-off " + Furre)

        End Function

        ''' <summary>
        ''' (5:55) unbanish the triggering furre.
        ''' </summary>
        ''' <param name="reader">
        ''' <see cref="TriggerReader"/>
        ''' </param>
        ''' <returns>
        ''' true on sending the server command
        ''' </returns>
        Public Function UnBanishTrigFurre(reader As TriggerReader) As Boolean

            Return SendServer("banish-off " + Player.Name)

        End Function

#End Region

    End Class

End Namespace