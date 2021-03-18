Imports System.Web.Script.Serialization

Public Class CO

    'Extérieur
    Public Const FILES_DIR As String = "files\"
    Public Const FONT_DIR As String = FILES_DIR & "font\"
    Public Const FONT_PATH As String = FONT_DIR & "font.ttf"
    Public Const SOUNDS_DIR As String = FILES_DIR & "sounds\"

    'Général
    Public Const WIDTH As Integer = 500
    Public Const HEIGHT As Integer = 625
    Public Const GAME_HEIGHT As Integer = WIDTH
    Public Const RESOLUTION As Double = 72.0
    Public Shared Serialiseur As New JavaScriptSerializer()
    Public Shared LEVELS_COUNT As Integer = My.Resources.levels.Split("/").Length
    Public Shared Rand As Random

    'Jeu
    Public Enum GameState
        Menu
        Game
        EndOfLevel
        EndOfGame
    End Enum

    'Terrain
    Public Const TILE_SIZE As Integer = 25
    Public Const N_TILE As Integer = WIDTH / TILE_SIZE
    Public Const TILE_TYPE_COUNT As Integer = 4
    Public Enum TileType
        EmptyTile = 99
        BasicTile = 0
        EndTile = 1
        CompletedEndTile = 2
        StartTile = 3
        Wall = 4
    End Enum

    Public Const EMPTY_TILE As Integer = 0
    Public Const WALL_TILE As Integer = 100
    Public Const COLOR_LIMIT As Integer = 10
    Public Const END_LIMIT As Integer = 20
    Public Const END_COMP_LIMIT As Integer = 30

    Public Const BASIC_TILE_COUNT As Integer = 16

    Public Const FADE_MIN As Double = 1 / 10
    Public Const SURB_FADE As Double = 1 / 2

    'Touches
    Public Shared KEYS As Windows.Forms.Keys() = {Windows.Forms.Keys.D1, Windows.Forms.Keys.D2, Windows.Forms.Keys.D3, Windows.Forms.Keys.R}

    'IG Menu
    Public Const COLOR_SIZE As Integer = 50
    Public Const COLOR_X_OFFSET As Integer = 5
    Public Const COLOR_Y As Integer = 505
    Public Const COLOR_FONT_SIZE As Integer = 40

    Public Shared QUIT_RECT As New Rectangle(5, 565, 75, 55)
    Public Shared RESTART_RECT As New Rectangle(90, 565, 75, 55)

    Public Shared TEXT_RECT As New Rectangle(185, 505, 310, 115)
    Public Const TEXT_FONT_SIZE As Integer = 33

    'Menu
    Public Shared PLAY_RECT As New Rectangle(125, 325, 226, 76)
    Public Shared QUIT_MENU_RECT As New Rectangle(125, 450, 226, 76)

    'Fin du jeu
    Public Shared QUIT_EOG_RECT As New Rectangle(125, 475, 226, 76)

    'Fin de level
    Public Const EOL_WIDTH As Integer = 400
    Public Const EOL_HEIGHT As Integer = 300
    Public Const EOL_X As Integer = (WIDTH - EOL_WIDTH) / 2
    Public Const EOL_Y As Integer = (HEIGHT - EOL_HEIGHT) / 2

    Public Shared COUNT_RECTS As Rectangle() = {New Rectangle(265, 63 + 3, 129, 39), New Rectangle(265, 102 + 3, 129, 39), New Rectangle(265, 141 + 3, 129, 39)}
    Public Shared COUNT_BRUSHES As Brush() = {New SolidBrush(Color.Red), New SolidBrush(Color.Blue), New SolidBrush(Color.Lime)}
    Public Const EOL_COLOR_COUNT_FONT_SIZE As Integer = 49

    Public Shared EOL_RESTART_RECT As New Rectangle(50 + EOL_X, 200 + EOL_Y, 137, 75)
    Public Shared EOL_NEXT_LEVEL_RECT As New Rectangle(214 + EOL_X, 200 + EOL_Y, 137, 75)

    'Son
    Public Const MUSIC_DURATION_MS As Integer = 94981

    'Screenshake
    Public Const SHAKE_MAX_AMP As Integer = 1
    Public Const SHAKE_1PX_DURATION_MS As Integer = 10
    Public Const SHAKE_PROBABILITY As Double = 0.55

    'Particules
    Public Const PARTICULE_AGE_MS As Integer = 1

    Public Const TILE_PART_COUNT_MIN As Integer = 3
    Public Const TILE_PART_COUNT_MAX As Integer = 7
    Public Const TILE_PART_VELOCITY As Double = 0.7
    Public Const TILE_PART_RADIUS_MIN As Integer = 3
    Public Const TILE_PART_RADIUS_MAX As Integer = 7
    Public Const TILE_PART_SPAWN_RADIUS_MIN As Integer = 0
    Public Const TILE_PART_SPAWN_RADIUS_MAX As Integer = 4
    Public Const TILE_PART_TIME_MIN_S As Double = 0.8
    Public Const TILE_PART_TIME_MAX_S As Double = 1.8


    'Méthodes
    Public Shared Function IsInRectangle(P As Point, rect As Rectangle) As Boolean
        Return (P.X >= rect.X AndAlso P.X <= rect.X + rect.Width AndAlso P.Y >= rect.Y AndAlso P.Y <= rect.Y + rect.Height)
    End Function
    Public Shared Function GetRandomDouble(min As Double, max As Double) As Double
        Return (max - min) * Rand.NextDouble() + min
    End Function

End Class
