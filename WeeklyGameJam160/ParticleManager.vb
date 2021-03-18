Public Class ParticleManager

    Public particles As List(Of Particle)
    Private currentAge As Long

    Sub New()
        currentAge = 0
        particles = New List(Of Particle)
    End Sub
    Public Function Age() As Boolean
        If Date.Now.Ticks - currentAge > CO.PARTICULE_AGE_MS * 10000 Then
            currentAge = Date.Now.Ticks
            Dim particlesToRemove As New List(Of Particle)
            For Each part As Particle In particles
                part.Age()
                If part.IsDead Then
                    particlesToRemove.Add(part)
                End If
            Next
            For Each part As Particle In particlesToRemove
                particles.Remove(part)
            Next
            Return True
        End If
        Return False
    End Function

    Public Sub GenerateTileParticles(location As Point, color As Integer)
        Dim startingColor As Color
        If color = 1 Then
            startingColor = Drawing.Color.Red
        ElseIf color = 2 Then
            startingColor = Drawing.Color.Blue
        Else
            startingColor = Drawing.Color.Lime
        End If
        Dim N As Integer = CO.Rand.Next(CO.TILE_PART_COUNT_MIN, CO.TILE_PART_COUNT_MAX + 1)
        For i As Integer = 0 To N - 1
            Dim angle As Double = CO.GetRandomDouble(0, 2 * Math.PI)
            Dim spawnDistance As Double = CO.GetRandomDouble(CO.TILE_PART_SPAWN_RADIUS_MIN, CO.TILE_PART_SPAWN_RADIUS_MAX)
            Dim center As New PointF(location.X + spawnDistance * Math.Cos(angle), location.Y + spawnDistance * Math.Sin(angle))
            Dim timeToLive As Integer = 1000 * CO.GetRandomDouble(CO.TILE_PART_TIME_MIN_S, CO.TILE_PART_TIME_MAX_S) / CO.PARTICULE_AGE_MS
            particles.Add(New TileParticle(center, CO.Rand.Next(CO.TILE_PART_RADIUS_MIN, CO.TILE_PART_RADIUS_MAX + 1), New PointF(CO.TILE_PART_VELOCITY * Math.Cos(angle), CO.TILE_PART_VELOCITY * Math.Sin(angle)), New PointF(-CO.TILE_PART_VELOCITY * Math.Cos(angle) / timeToLive, -CO.TILE_PART_VELOCITY * Math.Sin(angle) / timeToLive), timeToLive, startingColor))
        Next
    End Sub
    Public Function GenerateTexture() As Bitmap
        Dim img As New Bitmap(CO.WIDTH, CO.GAME_HEIGHT)
        img.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(img)
        For Each part As Particle In particles
            g.DrawImage(part.GetTexture(), part.GetLocation())
        Next
        Return img
    End Function

End Class