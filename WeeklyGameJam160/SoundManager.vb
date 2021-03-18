Imports System.IO

Public Class SoundManager

    Private Const FOLDER As String = CO.SOUNDS_DIR
    Private Const EXT As String = ".wav"

    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" _
(ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As _
Integer, ByVal hwndCallback As Integer) As Integer
    Private names As Dictionary(Of String, List(Of String))
    Private currentID As Integer
    Private channels As List(Of String)

    'Général
    Sub New()
        currentID = 0
        channels = New List(Of String)
        names = New Dictionary(Of String, List(Of String))
        GeneratedSoundsFiles()
    End Sub
    Private Sub GeneratedSoundsFiles()
        Dim name, prefix As String
        Dim tempMS As MemoryStream
        Dim resources = My.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, True, True)
        For Each res As DictionaryEntry In resources
            If res.Key.ToString().StartsWith("sound_") Then
                name = res.Key.ToString().Split("_")(1)
                tempMS = New MemoryStream()

                res.Value.CopyTo(tempMS)
                File.WriteAllBytes(FOLDER & name & EXT, tempMS.ToArray())

                prefix = name.Split("Z")(0)
                If Not names.ContainsKey(prefix) Then
                    names.Add(prefix, New List(Of String))
                End If
                names(prefix).Add(FOLDER & name & EXT)
            End If
        Next
    End Sub
    Private Function GetRandomElementOfList(ByRef l As List(Of String))
        Return l(CO.Rand.Next(l.Count))
    End Function

    'Son
    Public Sub CloseAll(Optional evenMusic As Boolean = False)
        Dim channelsToDelete As New List(Of String)
        For Each channel As String In channels
            If evenMusic OrElse Not channel.Contains("music") Then
                StopSound(channel)
                channelsToDelete.Add(channel)
            End If
        Next
        For Each channel As String In channelsToDelete
            channels.Remove(channel)
        Next
        channelsToDelete.Clear()
    End Sub
    Public Function PlaySound(name As String) As String
        Dim nom As String = Path.GetFileNameWithoutExtension(name) & "_" & CStr(currentID)
        channels.Add(nom)
        mciSendString("open " & Chr(34) & name & Chr(34) & " alias " & nom, Nothing, 0, 0)
        mciSendString("play " & nom, Nothing, 0, 0)
        currentID += 1
        Return nom
    End Function
    Private Sub StopSound(name As String)
        If name <> "" Then
            mciSendString("stop " & name, Nothing, 0, 0)
            mciSendString("close " & name, Nothing, 0, 0)
        End If
    End Sub

    'SONS
    Public Function GetMusicName()
        Return GetRandomElementOfList(names("music"))
    End Function
    Public Function GetTileName()
        Return GetRandomElementOfList(names("tile"))
    End Function
    Public Function GetCompleteEndName()
        Return GetRandomElementOfList(names("completeend"))
    End Function
    Public Function GetButtonName()
        Return GetRandomElementOfList(names("button"))
    End Function
    Public Function GetEndOfLevelName()
        Return GetRandomElementOfList(names("eol"))
    End Function
    Public Function GetEndOfGameName()
        Return GetRandomElementOfList(names("eog"))
    End Function

End Class
