Imports System.IO

Public Class SettingsModel
    Public ReadOnly Property IsLoaded As Boolean

    Public Property WindowPattern As String = "夜弦之音 ver#.##"

    Const SaveFileName = "应用设置.xml"

    Sub Load()
        If Not File.Exists(SaveFileName) Then
            Return
        End If
        Dim doc = XDocument.Load(SaveFileName)
        WindowPattern = doc.Root.<WindowSearchOptions>.@Pattern
        _IsLoaded = True
    End Sub

    Sub Save()
        Dim docRoot =
            <Settings Version="1.0">
                <WindowSearchOptions Pattern=<%= WindowPattern %>/>
            </Settings>
        Dim doc As New XDocument(docRoot)
        doc.Save(SaveFileName)
    End Sub
End Class
