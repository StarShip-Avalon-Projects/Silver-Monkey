Namespace Libraries.Dice

#Region "Public Methods"

    ''' <summary>
    ''' Grab your dice and shake them till you're ready to roll
    ''' </summary>
    Public Class DiceRollCollection
        Inherits System.Collections.ObjectModel.Collection(Of Die)

#Region "Public Methods"

        ''' <summary>
        ''' Mix up the Dice in hand and roll them
        ''' </summary>
        ''' <returns>
        ''' Sum of the result as <see cref="Double"/>
        ''' </returns>
        Public Function RollAll() As Double
            Dim total As Double = 0

            For Each die As Die In Me.Items
                total += die.Roll()
            Next

            Return total
        End Function

#End Region

    End Class

#End Region

End Namespace