' (C) Copyright 2013 by Matthieu Niess

Public Class ConvertirNombre

    Public Shared Function SeparateurMilliers(ByVal MonTexteAConvertir As String)

        Dim Longueur As Integer = MonTexteAConvertir.Length
        Dim MonTexteConverti As String = ""
        Dim i As Integer = 0

        If Longueur < 4 Then
            MonTexteConverti = MonTexteAConvertir.ToString
            GoTo 1
        End If

        While i <= Longueur
            If MonTexteConverti = "" Then
                MonTexteConverti = MonTexteAConvertir.Substring(Longueur - 3, 3)
                i = i + 3
            Else
                Select Case (Longueur - i)
                    Case Is <= 0
                        GoTo 1
                    Case Is = 1
                        MonTexteConverti = MonTexteAConvertir.Substring(0, 1) & " " & MonTexteConverti
                        GoTo 1
                    Case Is = 2
                        MonTexteConverti = MonTexteAConvertir.Substring(0, 2) & " " & MonTexteConverti
                        GoTo 1
                    Case Is = 3
                        MonTexteConverti = MonTexteAConvertir.Substring(0, 3) & " " & MonTexteConverti
                        GoTo 1
                    Case Is > 3
                        MonTexteConverti = MonTexteAConvertir.Substring(Longueur - i - 3, 3) & " " & MonTexteConverti
                        i = i + 3
                End Select
            End If
        End While

1:      Return MonTexteConverti

    End Function
End Class
