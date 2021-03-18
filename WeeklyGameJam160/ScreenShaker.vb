Public Class ScreenShaker

    Public offset As Point
    Private offsetInc As Point
    Private currentAge As Long
    Private isAging As Boolean

    Sub New()
        isAging = False
    End Sub

    Public Sub Shake()
        If CO.Rand.NextDouble() <= CO.SHAKE_PROBABILITY Then
            offset = New Point(0, 0)
            While offset.X = 0 AndAlso offset.Y = 0
                offset = New Point(CO.Rand.Next(-CO.SHAKE_MAX_AMP, CO.SHAKE_MAX_AMP + 1), CO.Rand.Next(-CO.SHAKE_MAX_AMP, CO.SHAKE_MAX_AMP + 1))
            End While
            offsetInc = New Point(-Math.Sign(offset.X), -Math.Sign(offset.Y))
            isAging = True
        End If
    End Sub
    Public Function Age() As Boolean
        If isAging AndAlso Date.Now.Ticks - currentAge >= CO.SHAKE_1PX_DURATION_MS * 10000 Then
            offset = New Point(offset.X + offsetInc.X, offset.Y + offsetInc.Y)
            isAging = (offset.X <> 0 OrElse offset.Y <> 0)
            currentAge = Date.Now.Ticks
            Return True
        End If
        Return False
    End Function

End Class
