Imports System.Drawing.Imaging

Public Class TextureManager

    Private textures(,) As Bitmap
    Private basicTilesTextures(,) As Bitmap
    Private surbBasicTilesTextures(,) As Bitmap
    Public ColorFrame As Bitmap
    Private Fade As Bitmap
    Public Wall As Bitmap

    Sub New()
        ''TILES
        Dim tilesSheet As Bitmap = My.Resources.tiles
        tilesSheet.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim g As Graphics
        'Basiques
        Dim N As Integer = tilesSheet.Width / CO.TILE_SIZE
        ReDim basicTilesTextures(CO.BASIC_TILE_COUNT - 1, N - 1)
        ReDim surbBasicTilesTextures(CO.BASIC_TILE_COUNT - 1, N - 1)
        For type As Integer = 0 To CO.BASIC_TILE_COUNT - 1
            For i As Integer = 0 To N - 1
                basicTilesTextures(type, i) = New Bitmap(CO.TILE_SIZE, CO.TILE_SIZE)
                basicTilesTextures(type, i).SetResolution(CO.RESOLUTION, CO.RESOLUTION)
                g = Graphics.FromImage(basicTilesTextures(type, i))
                g.DrawImage(tilesSheet, New Rectangle(0, 0, CO.TILE_SIZE, CO.TILE_SIZE), New Rectangle(i * CO.TILE_SIZE, type * CO.TILE_SIZE, CO.TILE_SIZE, CO.TILE_SIZE), GraphicsUnit.Pixel)
                'Surbrillance
                surbBasicTilesTextures(type, i) = New Bitmap(CO.TILE_SIZE, CO.TILE_SIZE)
                surbBasicTilesTextures(type, i).SetResolution(CO.RESOLUTION, CO.RESOLUTION)
                For x As Integer = 0 To CO.TILE_SIZE - 1
                    For y As Integer = 0 To CO.TILE_SIZE - 1
                        Dim col As Color = basicTilesTextures(type, i).GetPixel(x, y)
                        If col.A > 0 Then
                            surbBasicTilesTextures(type, i).SetPixel(x, y, Color.FromArgb(col.R / 2, col.G / 2, col.B / 2))
                        End If
                    Next
                Next
            Next
        Next
        'Autres
        ReDim textures(CO.TILE_TYPE_COUNT - 1, N - 1)

        For type As Integer = CO.BASIC_TILE_COUNT To tilesSheet.Height / CO.TILE_SIZE - 1
            For i As Integer = 0 To N - 1
                Dim typeOffset As Integer = type - CO.BASIC_TILE_COUNT + 1
                textures(typeOffset, i) = New Bitmap(CO.TILE_SIZE, CO.TILE_SIZE)
                textures(typeOffset, i).SetResolution(CO.RESOLUTION, CO.RESOLUTION)
                g = Graphics.FromImage(textures(typeOffset, i))
                g.DrawImage(tilesSheet, New Rectangle(0, 0, CO.TILE_SIZE, CO.TILE_SIZE), New Rectangle(i * CO.TILE_SIZE, type * CO.TILE_SIZE, CO.TILE_SIZE, CO.TILE_SIZE), GraphicsUnit.Pixel)
            Next
        Next

        'Cadre couleurs
        ColorFrame = New Bitmap(My.Resources.color_frame)
        ColorFrame.SetResolution(CO.RESOLUTION, CO.RESOLUTION)

        'Fade
        Fade = New Bitmap(My.Resources.fade)
        Fade.SetResolution(CO.RESOLUTION, CO.RESOLUTION)

        'Murs
        Wall = New Bitmap(My.Resources.wall)
        Wall.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
    End Sub

    Public Function GetBasicTile(color As Integer, hasRight As Boolean, hasDown As Boolean, hasLeft As Boolean, hasUp As Boolean)
        Dim index As Integer = (-8 * hasRight) + (-4 * hasDown) + (-2 * hasLeft) + (-1 * hasUp)
        Return basicTilesTextures(index, (color Mod 10) - 1)
    End Function
    Public Function GetSurbrillanceBasicTile(color As Integer, hasRight As Boolean, hasDown As Boolean, hasLeft As Boolean, hasUp As Boolean)
        Dim index As Integer = (-8 * hasRight) + (-4 * hasDown) + (-2 * hasLeft) + (-1 * hasUp)
        Return surbBasicTilesTextures(index, (color Mod 10) - 1)
    End Function
    Public Function GetEndTile(color As Integer)
        Return textures(1, (color Mod 10) - 1)
    End Function
    Public Function GetCompletedEndTile(color As Integer)
        Return textures(2, (color Mod 10) - 1)
    End Function
    Public Function GetStartTile(color As Integer)
        Return textures(3, (color Mod 10) - 1)
    End Function
    Public Function GetTile(tileType As CO.TileType, color As Integer)
        Return textures(tileType, (color Mod 10) - 1)
    End Function

    Public Function GetFadeImage(transparencyLevel As Double) As Bitmap
        Dim img As New Bitmap(CO.TILE_SIZE, CO.TILE_SIZE)
        img.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim ColMat As New ColorMatrix()
        ColMat.Matrix33 = 1 - transparencyLevel
        Dim ImgAtt As New ImageAttributes()
        ImgAtt.SetColorMatrix(ColMat)
        Dim g As Graphics = Graphics.FromImage(img)
        g.DrawImage(Fade, New Rectangle(0, 0, CO.TILE_SIZE, CO.TILE_SIZE), 0, 0, CO.TILE_SIZE, CO.TILE_SIZE, GraphicsUnit.Pixel, ImgAtt)
        Return img
    End Function

End Class
