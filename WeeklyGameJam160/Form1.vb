Imports System.IO, System.Drawing.Imaging, System.Web.Script.Serialization, System.Threading

Public Class Form1

    'Jeu
    Private gState As CO.GameState
    Private currentLevel As Level
    Private levelNumber As Integer
    Private mouseLocation As Point
    Private scrShaker As ScreenShaker

    'Son
    Private audioThread As Thread
    Private audioPlayer As SoundManager
    Private musicStartTicks As Long

    'Police
    Private PFC As Drawing.Text.PrivateFontCollection
    Private gameFont As Font
    Private foundFont As Boolean
    Private SF As StringFormat
    Private SF2 As StringFormat

    'Affichage
    Private gameImage As Bitmap
    Private movingImage As Bitmap
    Private chunk As PictureBox
    Private TM As TextureManager

    'Affichage surbrillance
    Private surbPoint As Point
    Private surbNeighbors As List(Of Point)
    Private isSurb As Boolean

    'Terrain
    Private terrain(,) As Integer '0 : rien / 1-9 : couleurs / 11-19 : arrivée couleur / 21-29 : arrivée couleur complétée / 31-39 : départ couleurs
    Private lastAdded() As List(Of Point)
    Private endCount As Integer
    Private completedEndCount As Integer
    Private tileCounts() As Integer

    'Joueur
    Private currentColor As Integer

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Fenêtre
        Me.Location = New Point((Screen.PrimaryScreen.Bounds.Width - Me.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - Me.Height) / 2)
        Me.Icon = My.Resources.icon

        'Extérieur
        GenerateFoldersAndFiles()

        'Jeu
        Init()

    End Sub

    'INITIALISATION
    Private Sub GenerateFoldersAndFiles()
        If Not Directory.Exists(CO.FILES_DIR) Then
            Directory.CreateDirectory(CO.FILES_DIR)
        End If
        If Not Directory.Exists(CO.FONT_DIR) Then
            Directory.CreateDirectory(CO.FONT_DIR)
        End If
        If Not File.Exists(CO.FONT_PATH) Then
            File.WriteAllBytes(CO.FONT_PATH, My.Resources.font)
        End If
        If Not Directory.Exists(CO.SOUNDS_DIR) Then
            Directory.CreateDirectory(CO.SOUNDS_DIR)
        End If
    End Sub
    Private Sub Init()
        'Son
        CO.Rand = New Random()
        audioPlayer = New SoundManager()
        musicTimer.Start()

        'Affichage
        InitRender()
        InitFont()
        gamePanel.BackgroundImage = My.Resources.menu
        chunk.Visible = True
        scrShaker = New ScreenShaker()
        particleTimer.Start()

        'Terrain
        ReDim terrain(CO.N_TILE - 1, CO.N_TILE - 1)
        currentColor = 1

        'Jeu
        gState = CO.GameState.Menu
    End Sub
    Private Sub InitRender()
        gameImage = New Bitmap(CO.WIDTH, CO.HEIGHT)
        movingImage = New Bitmap(CO.WIDTH, CO.HEIGHT)
        chunk = New PictureBox()
        With chunk
            .Size = New Size(CO.WIDTH, CO.HEIGHT)
            .Location = New Point(0, 0)
            .BackColor = Color.Transparent
            .Enabled = False
            .Visible = False
            .SendToBack()
        End With
        gamePanel.Controls.Add(chunk)
        gameImage = My.Resources.background
        Me.BackColor = Color.Black
        TM = New TextureManager()
    End Sub
    Private Sub InitFont()
        PFC = New Drawing.Text.PrivateFontCollection()
        If File.Exists(CO.FONT_PATH) Then
            Try
                PFC.AddFontFile(CO.FONT_PATH)
                foundFont = True
            Catch ex As Exception
                foundFont = False
            End Try
        Else
            foundFont = False
        End If

        SF = New StringFormat()
        SF.Alignment = StringAlignment.Center
        SF.LineAlignment = StringAlignment.Center

        SF2 = New StringFormat()
        SF2.Alignment = StringAlignment.Near
        SF2.LineAlignment = StringAlignment.Near

    End Sub
    Private Sub InitLevel()
        ReDim terrain(CO.N_TILE - 1, CO.N_TILE - 1)
        currentLevel = CO.Serialiseur.Deserialize(Of Level)(My.Resources.levels.Split("/")(levelNumber))
        tileCounts = currentLevel.TileCounts.Clone()
        If tileCounts(currentColor - 1) = 0 Then
            For i As Integer = 0 To 2
                If tileCounts(i) > 0 Then
                    currentColor = i + 1
                    Exit For
                End If
            Next
        End If
        completedEndCount = 0
        ReDim lastAdded(CO.TILE_TYPE_COUNT - 1)
        For i As Integer = 0 To CO.TILE_TYPE_COUNT - 1
            lastAdded(i) = New List(Of Point)
        Next

        For Each endTile As TileClass In currentLevel.Ends
            terrain(endTile.X, endTile.Y) = endTile.Value
        Next

        For Each startTile As TileClass In currentLevel.Starts
            terrain(startTile.X, startTile.Y) = startTile.Value
        Next

        For Each wallTile As TileClass In currentLevel.Walls
            terrain(wallTile.X, wallTile.Y) = wallTile.Value
        Next
    End Sub

    'AFFICHAGE
    Private Sub ResetDraw()
        gameImage = New Bitmap(CO.WIDTH, CO.HEIGHT)
        gameImage.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        movingImage = New Bitmap(CO.WIDTH, CO.HEIGHT)
        movingImage.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
    End Sub
    Private Sub Draw()
        gamePanel.Location = scrShaker.offset
        Dim img As New Bitmap(CO.WIDTH, CO.HEIGHT)
        img.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(img)
        g.DrawImage(gameImage, 0, 0)
        g.DrawImage(movingImage, 0, 0)
        chunk.Image = img
        chunk.Refresh()
    End Sub
    Private Sub DrawLevel()
        ResetDraw()
        Dim g As Graphics = Graphics.FromImage(gameImage)

        For Each endTile As TileClass In currentLevel.Ends
            g.DrawImage(TM.GetEndTile(endTile.Value), endTile.X * CO.TILE_SIZE, endTile.Y * CO.TILE_SIZE)
        Next

        For Each startTile As TileClass In currentLevel.Starts
            g.DrawImage(TM.GetStartTile(startTile.Value), startTile.X * CO.TILE_SIZE, startTile.Y * CO.TILE_SIZE)
        Next

        For Each wallTile As TileClass In currentLevel.Walls
            g.DrawImage(TM.Wall, wallTile.X * CO.TILE_SIZE, wallTile.Y * CO.TILE_SIZE)
        Next

        If foundFont Then
            g.DrawString(currentLevel.Text, New Font(PFC.Families(0), CO.TEXT_FONT_SIZE, FontStyle.Regular), Brushes.White, CO.TEXT_RECT, SF2)
        Else

        End If

        DrawMovingObjects()
        Draw()
    End Sub
    Private Sub UpdateBasicTile(X As Integer, Y As Integer, color As Integer, add As Boolean)
        If add Then
            DrawTile(X, Y, CO.TileType.BasicTile, color)
        Else
            DrawTile(X, Y, CO.TileType.EmptyTile, color)
        End If
        '<
        If X > 0 AndAlso terrain(X - 1, Y) = color Then
            DrawTile(X - 1, Y, CO.TileType.BasicTile, color, Not add)
        End If
        '>
        If X < CO.N_TILE - 1 AndAlso terrain(X + 1, Y) = color Then
            DrawTile(X + 1, Y, CO.TileType.BasicTile, color, Not add)
        End If
        '^
        If Y > 0 AndAlso terrain(X, Y - 1) = color Then
            DrawTile(X, Y - 1, CO.TileType.BasicTile, color, Not add)
        End If
        'v
        If Y < CO.N_TILE - 1 AndAlso terrain(X, Y + 1) = color Then
            DrawTile(X, Y + 1, CO.TileType.BasicTile, color, Not add)
        End If
    End Sub
    Private Sub DrawTile(X As Integer, Y As Integer, tileType As CO.TileType, Optional color As Integer = -1, Optional remove As Boolean = False)
        Dim g As Graphics = Graphics.FromImage(gameImage)

        If tileType = CO.TileType.EmptyTile Then
            g.DrawImage(My.Resources.background, New Rectangle(X * CO.TILE_SIZE, Y * CO.TILE_SIZE, CO.TILE_SIZE, CO.TILE_SIZE), New Rectangle(X * CO.TILE_SIZE, Y * CO.TILE_SIZE, CO.TILE_SIZE, CO.TILE_SIZE), GraphicsUnit.Pixel)
        ElseIf tileType = CO.TileType.BasicTile Then
            If remove Then
                g.DrawImage(My.Resources.background, New Rectangle(X * CO.TILE_SIZE, Y * CO.TILE_SIZE, CO.TILE_SIZE, CO.TILE_SIZE), New Rectangle(X * CO.TILE_SIZE, Y * CO.TILE_SIZE, CO.TILE_SIZE, CO.TILE_SIZE), GraphicsUnit.Pixel)
            End If
            g.DrawImage(TM.GetBasicTile(color, (X < CO.N_TILE - 1 AndAlso terrain(X + 1, Y) Mod 10 = color Mod 10), (Y < CO.N_TILE - 1 AndAlso terrain(X, Y + 1) Mod 10 = color Mod 10), (X > 0 AndAlso terrain(X - 1, Y) Mod 10 = color Mod 10), (Y > 0 AndAlso terrain(X, Y - 1) Mod 10 = color Mod 10)), X * CO.TILE_SIZE, Y * CO.TILE_SIZE)
        Else
            g.DrawImage(TM.GetTile(tileType, currentColor), X * CO.TILE_SIZE, Y * CO.TILE_SIZE)
        End If

        Draw()
    End Sub
    Private Sub DrawMovingObjects()
        movingImage = New Bitmap(CO.WIDTH, CO.HEIGHT)
        movingImage.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(movingImage)

        'Couleur
        Dim offsetColor As Integer = currentColor - 1
        g.DrawImage(TM.ColorFrame, offsetColor * (CO.COLOR_SIZE + CO.COLOR_X_OFFSET) + CO.COLOR_X_OFFSET, CO.COLOR_Y)
        'Restantes
        For i As Integer = 0 To currentLevel.TileCounts.Length - 1
            If foundFont Then
                g.DrawString(tileCounts(i), New Font(PFC.Families(0), CO.COLOR_FONT_SIZE, FontStyle.Regular), Brushes.Black, New Rectangle(i * (CO.COLOR_SIZE + CO.COLOR_X_OFFSET) + CO.COLOR_X_OFFSET, CO.COLOR_Y + 3, CO.COLOR_SIZE, CO.COLOR_SIZE), SF)
            Else
                g.DrawString(tileCounts(i), New Font("Arial", CO.COLOR_FONT_SIZE, FontStyle.Regular), Brushes.Black, New Rectangle(i * (CO.COLOR_SIZE + CO.COLOR_X_OFFSET) + CO.COLOR_X_OFFSET, CO.COLOR_Y, CO.COLOR_SIZE, CO.COLOR_SIZE), SF)
            End If
        Next

        'Surbrillance
        If isSurb Then
            Dim X As Integer = surbPoint.X
            Dim Y As Integer = surbPoint.Y
            g.DrawImage(TM.GetSurbrillanceBasicTile(currentColor, (X < CO.N_TILE - 1 AndAlso terrain(X + 1, Y) Mod 10 = currentColor Mod 10), (Y < CO.N_TILE - 1 AndAlso terrain(X, Y + 1) Mod 10 = currentColor Mod 10), (X > 0 AndAlso terrain(X - 1, Y) Mod 10 = currentColor Mod 10), (Y > 0 AndAlso terrain(X, Y - 1) Mod 10 = currentColor Mod 10)), X * CO.TILE_SIZE, Y * CO.TILE_SIZE)

            For Each neighbor As Point In surbNeighbors
                X = neighbor.X
                Y = neighbor.Y
                Dim temp As Integer = terrain(surbPoint.X, surbPoint.Y)
                terrain(surbPoint.X, surbPoint.Y) = currentColor
                g.DrawImage(TM.GetBasicTile(currentColor, (X < CO.N_TILE - 1 AndAlso terrain(X + 1, Y) Mod 10 = currentColor Mod 10), (Y < CO.N_TILE - 1 AndAlso terrain(X, Y + 1) Mod 10 = currentColor Mod 10), (X > 0 AndAlso terrain(X - 1, Y) Mod 10 = currentColor Mod 10), (Y > 0 AndAlso terrain(X, Y - 1) Mod 10 = currentColor Mod 10)), X * CO.TILE_SIZE, Y * CO.TILE_SIZE)
                terrain(surbPoint.X, surbPoint.Y) = temp
            Next
        End If

        'Fade
        For C As Integer = 0 To lastAdded.Length - 1
            Dim count As Integer = lastAdded(C).Count
            Dim transparency As Double = 1
            For i As Integer = count - 1 To 0 Step -1
                g.DrawImage(TM.GetFadeImage(transparency), lastAdded(C)(i).X * CO.TILE_SIZE, lastAdded(C)(i).Y * CO.TILE_SIZE)
                transparency -= (1 - CO.FADE_MIN) / currentLevel.TileCountLimit
            Next
        Next

        'Fin de niveau
        If gState = CO.GameState.EndOfLevel Then
            Dim levelEndScreen As Bitmap = My.Resources.level_complete
            levelEndScreen.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
            Dim g2 As Graphics = Graphics.FromImage(levelEndScreen)

            Dim texts(currentLevel.TileCounts.Length - 1) As String
            For i As Integer = 0 To currentLevel.TileCounts.Length - 1
                texts(i) = (currentLevel.TileCounts(i) - tileCounts(i)) & "/" & currentLevel.DoableTileCounts(i)
                If foundFont Then
                    g2.DrawString(texts(i), New Font(PFC.Families(0), CO.EOL_COLOR_COUNT_FONT_SIZE, FontStyle.Regular), CO.COUNT_BRUSHES(i), CO.COUNT_RECTS(i), SF)
                Else
                    g2.DrawString(texts(i), New Font("Arial", CO.EOL_COLOR_COUNT_FONT_SIZE, FontStyle.Regular), CO.COUNT_BRUSHES(i), CO.COUNT_RECTS(i), SF)
                End If
            Next

            g.DrawImage(levelEndScreen, CO.EOL_X, CO.EOL_Y)

        End If


        Draw()
    End Sub

    'TERRAIN
    Private Function PaintTile(X As Integer, Y As Integer) As Boolean
        Dim colorOffset As Integer = (currentColor Mod 10) - 1
        If tileCounts(colorOffset) > 0 Then
            Dim previousColor As Integer = terrain(X, Y)
            terrain(X, Y) = currentColor
            If previousColor > 0 AndAlso previousColor < CO.END_COMP_LIMIT Then
                UpdateBasicTile(X, Y, previousColor, False)
            End If
            lastAdded(colorOffset).Add(New Point(X, Y))
            If lastAdded(colorOffset).Count > currentLevel.TileCountLimit Then
                Dim tileToRemovePoint As Point = lastAdded(colorOffset)(0)
                terrain(tileToRemovePoint.X, tileToRemovePoint.Y) = CO.EMPTY_TILE
                UpdateBasicTile(tileToRemovePoint.X, tileToRemovePoint.Y, currentColor, False)
                lastAdded((currentColor Mod 10) - 1).RemoveAt(0)
            End If
            tileCounts(colorOffset) -= 1
            scrShaker.Shake()
            DrawMovingObjects()
            UpdateBasicTile(X, Y, currentColor, True)
            PlaySound(audioPlayer.GetTileName())
            Return True
        End If
        Return False
    End Function
    Private Sub CompleteEnd(X As Integer, Y As Integer)
        terrain(X, Y) += 10
        DrawTile(X, Y, CO.TileType.CompletedEndTile)
        completedEndCount += 1

        If completedEndCount >= currentLevel.Ends.Count Then
            Win()
        Else
            PlaySound(audioPlayer.GetCompleteEndName())
        End If
    End Sub
    Private Sub TestSurbrillance(X As Integer, Y As Integer)
        If X >= 0 AndAlso X < CO.N_TILE AndAlso Y >= 0 AndAlso Y < CO.N_TILE Then
            isSurb = False
            If terrain(X, Y) Mod 10 <> currentColor Mod 10 AndAlso terrain(X, Y) < CO.COLOR_LIMIT Then
                surbNeighbors = New List(Of Point)
                If X > 0 AndAlso (terrain(X - 1, Y) = currentColor OrElse terrain(X - 1, Y) = currentColor + CO.END_COMP_LIMIT) Then
                    Surbrillance(X, Y, X - 1, Y, terrain(X - 1, Y) = currentColor)
                End If
                If X < CO.N_TILE - 1 AndAlso (terrain(X + 1, Y) = currentColor OrElse terrain(X + 1, Y) = currentColor + CO.END_COMP_LIMIT) Then
                    Surbrillance(X, Y, X + 1, Y, terrain(X + 1, Y) = currentColor)
                End If
                If Y > 0 AndAlso (terrain(X, Y - 1) = currentColor OrElse terrain(X, Y - 1) = currentColor + CO.END_COMP_LIMIT) Then
                    Surbrillance(X, Y, X, Y - 1, terrain(X, Y - 1) = currentColor)
                End If
                If Y < CO.N_TILE - 1 AndAlso (terrain(X, Y + 1) = currentColor OrElse terrain(X, Y + 1) = currentColor + CO.END_COMP_LIMIT) Then
                    Surbrillance(X, Y, X, Y + 1, terrain(X, Y + 1) = currentColor)
                End If
            End If
            DrawMovingObjects()
        End If
    End Sub
    Private Sub Surbrillance(X As Integer, Y As Integer, XN As Integer, YN As Integer, addNeighbor As Boolean)
        isSurb = True
        surbPoint = New Point(X, Y)
        If addNeighbor Then
            surbNeighbors.Add(New Point(XN, YN))
        End If
    End Sub

    'FINS
    Private Sub Win()
        PlaySound(audioPlayer.GetEndOfLevelName())
        gState = CO.GameState.EndOfLevel
        DrawMovingObjects()
    End Sub
    Private Sub EndOfGame()
        PlaySound(audioPlayer.GetEndOfGameName())
        gState = CO.GameState.EndOfGame
        chunk.Visible = False
        movingImage = New Bitmap(CO.WIDTH, CO.GAME_HEIGHT)
        gamePanel.BackgroundImage = My.Resources.end_of_game
    End Sub
    Private Sub CloseGame()
        audioPlayer.CloseAll(True)
        Me.Close()
    End Sub

    'SONS
    Private Sub PlaySound(name As String)
        audioThread = New Thread(Sub() audioPlayer.PlaySound(name))
        audioThread.Start()
    End Sub

    ''INPUTS    
    'CLIC
    Private Sub Form1_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        Dim hasClickedOnButton As Boolean = False
        If gState = CO.GameState.Game Then
            If e.Location.Y < CO.GAME_HEIGHT Then 'Clic sur le terrain
                Dim X As Integer = e.Location.X \ CO.TILE_SIZE
                Dim Y As Integer = e.Location.Y \ CO.TILE_SIZE
                If (terrain(X, Y) < CO.COLOR_LIMIT AndAlso terrain(X, Y) <> currentColor) AndAlso ((X > 0 AndAlso (terrain(X - 1, Y) = currentColor OrElse terrain(X - 1, Y) = currentColor + CO.END_COMP_LIMIT)) OrElse (Y > 0 AndAlso (terrain(X, Y - 1) = currentColor OrElse terrain(X, Y - 1) = currentColor + CO.END_COMP_LIMIT)) OrElse (X < CO.N_TILE - 1 AndAlso (terrain(X + 1, Y) = currentColor OrElse terrain(X + 1, Y) = currentColor + CO.END_COMP_LIMIT)) OrElse (Y < CO.N_TILE - 1 AndAlso (terrain(X, Y + 1) = currentColor OrElse terrain(X, Y + 1) = currentColor + CO.END_COMP_LIMIT))) Then 'Placer couleur
                    If PaintTile(X, Y) Then
                        'Arrivée
                        If (X > 0 AndAlso terrain(X - 1, Y) = currentColor + CO.COLOR_LIMIT) Then
                            CompleteEnd(X - 1, Y)
                        End If
                        If (Y > 0 AndAlso terrain(X, Y - 1) = currentColor + CO.COLOR_LIMIT) Then
                            CompleteEnd(X, Y - 1)
                        End If
                        If (X < CO.N_TILE - 1 AndAlso terrain(X + 1, Y) = currentColor + CO.COLOR_LIMIT) Then
                            CompleteEnd(X + 1, Y)
                        End If
                        If (Y < CO.N_TILE - 1 AndAlso terrain(X, Y + 1) = currentColor + CO.COLOR_LIMIT) Then
                            CompleteEnd(X, Y + 1)
                        End If
                    End If
                End If
            Else 'Clic sous le terrain
                If e.Y >= CO.COLOR_Y AndAlso e.Y <= CO.COLOR_Y + CO.COLOR_SIZE Then
                    If e.X >= CO.COLOR_X_OFFSET AndAlso e.X <= CO.COLOR_X_OFFSET + CO.COLOR_SIZE Then
                        currentColor = 1
                        hasClickedOnButton = True
                    End If
                    If e.X >= CO.COLOR_SIZE + 2 * CO.COLOR_X_OFFSET AndAlso e.X <= 2 * (CO.COLOR_X_OFFSET + CO.COLOR_SIZE) Then
                        currentColor = 2
                        hasClickedOnButton = True
                    End If
                    If e.X >= 2 * (CO.COLOR_SIZE + CO.COLOR_X_OFFSET) + CO.COLOR_X_OFFSET AndAlso e.X <= 3 * (CO.COLOR_X_OFFSET + CO.COLOR_SIZE) Then
                        currentColor = 3
                        hasClickedOnButton = True
                    End If
                    DrawMovingObjects()
                Else
                    If CO.IsInRectangle(e.Location, CO.RESTART_RECT) Then
                        hasClickedOnButton = True
                        InitLevel()
                        DrawLevel()
                    End If
                End If
            End If
            If CO.IsInRectangle(e.Location, CO.QUIT_RECT) Then
                hasClickedOnButton = True
                CloseGame()
            End If
        ElseIf gState = CO.GameState.EndOfLevel Then
            If CO.IsInRectangle(e.Location, CO.EOL_RESTART_RECT) Then
                hasClickedOnButton = True
                InitLevel()
                DrawLevel()
                gState = CO.GameState.Game
            ElseIf CO.IsInRectangle(e.Location, CO.EOL_NEXT_LEVEL_RECT) Then
                If levelNumber = CO.LEVELS_COUNT - 1 Then
                    EndOfGame()
                Else
                    hasClickedOnButton = True
                    levelNumber += 1
                    InitLevel()
                    DrawLevel()
                    gState = CO.GameState.Game
                End If
            End If
            If CO.IsInRectangle(e.Location, CO.QUIT_RECT) Then
                hasClickedOnButton = True
                CloseGame()
            End If
        ElseIf gState = CO.GameState.Menu Then
            If CO.IsInRectangle(e.Location, CO.PLAY_RECT) Then
                hasClickedOnButton = True
                gamePanel.BackgroundImage = My.Resources.background
                levelNumber = 0
                InitLevel()
                DrawLevel()
                gState = CO.GameState.Game
            ElseIf CO.IsInRectangle(e.Location, CO.QUIT_MENU_RECT) Then
                hasClickedOnButton = True
                CloseGame()
            End If
        ElseIf gState = CO.GameState.EndOfGame Then
            If CO.IsInRectangle(e.Location, CO.QUIT_EOG_RECT) Then
                hasClickedOnButton = True
                CloseGame()
            End If
        End If
        If hasClickedOnButton Then
            PlaySound(audioPlayer.GetButtonName())
        End If
    End Sub
    'Déplacement souris
    Private Sub Form1_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        mouseLocation = e.Location
        If gState = CO.GameState.Game Then
            If e.Y <= CO.GAME_HEIGHT Then
                If tileCounts(currentColor - 1) > 0 Then
                    Dim X As Integer = e.X \ CO.TILE_SIZE
                    Dim Y As Integer = e.Y \ CO.TILE_SIZE
                    TestSurbrillance(X, Y)
                End If
            End If
        End If
    End Sub
    'Touches
    Private Sub Form1_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If gState = CO.GameState.Game Then
            If e.KeyCode = CO.KEYS(0) Then
                If currentColor <> 1 Then
                    isSurb = False
                End If
                currentColor = 1
                TestSurbrillance(mouseLocation.X \ CO.TILE_SIZE, mouseLocation.Y \ CO.TILE_SIZE)
            ElseIf e.KeyCode = CO.KEYS(1) Then
                If currentColor <> 2 Then
                    isSurb = False
                End If
                currentColor = 2
                TestSurbrillance(mouseLocation.X \ CO.TILE_SIZE, mouseLocation.Y \ CO.TILE_SIZE)
            ElseIf e.KeyCode = CO.KEYS(2) Then
                If currentColor <> 3 Then
                    isSurb = False
                End If
                currentColor = 3
                TestSurbrillance(mouseLocation.X \ CO.TILE_SIZE, mouseLocation.Y \ CO.TILE_SIZE)
            ElseIf e.KeyCode = CO.KEYS(3) Then
                InitLevel()
                DrawLevel()
            End If
        End If
    End Sub

    'TIMERS
    Private Sub musicTimer_Tick(sender As System.Object, e As System.EventArgs) Handles musicTimer.Tick
        If Date.Now.Ticks - musicStartTicks > CO.MUSIC_DURATION_MS * 10000 Then
            musicStartTicks = Date.Now.Ticks
            PlaySound(audioPlayer.GetMusicName())
        End If
    End Sub
    Dim a As Integer = 0
    Private Sub particleTimer_Tick(sender As System.Object, e As System.EventArgs) Handles particleTimer.Tick
        If gState = CO.GameState.Game Then
            If scrShaker.Age() Then
                Draw()
            End If
        End If
    End Sub
End Class