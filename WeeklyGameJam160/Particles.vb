Public MustInherit Class Particle

    Protected location As PointF
    Protected velocity As PointF
    Protected acceleration As PointF
    Protected currentAge As Integer
    Public IsDead As Boolean

    Public Sub Age()
        velocity = New PointF(velocity.X + acceleration.X, velocity.Y + acceleration.Y)
        location = New PointF(location.X + velocity.X, location.Y + velocity.Y)
        CustomAge()
        currentAge -= 1
        If currentAge <= 0 Then
            IsDead = True
        End If
    End Sub
    Public Function GetLocation() As Point
        Return New Point(location.X, location.Y)
    End Function
    Protected MustOverride Sub CustomAge()
    Public MustOverride Function GetTexture() As Bitmap

End Class
Public Class TileParticle
    Inherits Particle

    Private size As Integer
    Private colorDelta As Double()
    Private colorFloat As Double()
    Private color As Color

    Sub New(center As PointF, size As Integer, velocity As PointF, acceleration As PointF, timeToLive As Integer, startingColor As Color)
        Me.location = New Point(center.X - size / 2, center.Y - size / 2)
        Me.size = size
        Me.velocity = velocity
        Me.acceleration = acceleration
        Me.color = startingColor
        Me.currentAge = timeToLive
        ReDim colorDelta(3)
        colorDelta(0) = 0 '(0 - startingColor.A) / timeToLive
        colorDelta(1) = (255 - startingColor.R) / timeToLive
        colorDelta(2) = (255 - startingColor.G) / timeToLive
        colorDelta(3) = (255 - startingColor.B) / timeToLive
        ReDim colorFloat(3)
        colorFloat(0) = startingColor.A
        colorFloat(1) = startingColor.R
        colorFloat(2) = startingColor.G
        colorFloat(3) = startingColor.B
    End Sub

    Protected Overrides Sub CustomAge()
        For i As Integer = 0 To 3
            colorFloat(i) += colorDelta(i)
        Next
        color = color.FromArgb(Math.Max(0, Math.Min(255, colorFloat(0))), Math.Max(0, Math.Min(255, colorFloat(1))), Math.Max(0, Math.Min(255, colorFloat(2))), Math.Max(0, Math.Min(255, colorFloat(3))))
    End Sub

    Public Overrides Function GetTexture() As System.Drawing.Bitmap
        Dim img As New Bitmap(size, size)
        img.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(img)
        g.FillEllipse(New SolidBrush(color), New Rectangle(0, 0, size, size))
        Return img
    End Function
End Class