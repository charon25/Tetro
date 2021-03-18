<Serializable()>
Public Class Level

    Public Ends As List(Of TileClass)
    Public Starts As List(Of TileClass)
    Public Walls As List(Of TileClass)
    Public TileCountLimit As Integer
    Public TileCounts() As Integer
    Public DoableTileCounts() As Integer
    Public Text As String

    Sub New()

    End Sub


End Class
<Serializable()>
Public Class TileClass

    Public X As Integer
    Public Y As Integer
    Public Value As Integer

    Sub New()

    End Sub

End Class