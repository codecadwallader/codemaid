Module Module1
    Public Sub RoundNumber(ByRef RoundN, _
   Optional Decimals As Integer)
        On Error Resume Next
        Dim RoundAmount As Single, Result
        'Creates powers to the decimal places
        RoundAmount = 10 ^ Decimals
        'Creates a whole number, rounds, applies decimal places
        Result = (CLng((RoundN) * RoundAmount) / RoundAmount)
        RoundN = Result
    End Sub

    '
    ' Calculates age in years from a given date to today's date.
    '
    '

    Function Age(varBirthDate As Object) As Integer

        Dim varAge As Object
        If Not IsDate(varBirthDate) Then Exit Function
        varAge = DateDiff("yyyy", varBirthDate, Now)
        If Date < DateSerial(Year(Now), Month(varBirthDate), _
                        Day(varBirthDate)) Then
            varAge = varAge - 1
        End If
        Age = CInt(varAge)

    End Function
End Module