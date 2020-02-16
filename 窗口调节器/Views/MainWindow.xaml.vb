Imports System.ComponentModel

Class MainWindow
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        TxtWindowNameFormat.Text = Application.Settings.WindowPattern
        ExpBasicSettings.IsExpanded = True
        DetectGameWindow(silent:=True)
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Application.Settings.WindowPattern = TxtWindowNameFormat.Text
    End Sub

    Private _gameWindow As RgssWindow

    Public Function GetGameWindow(Optional silent As Boolean = False) As RgssWindow
        If _gameWindow Is Nothing Then
            Dim gameWnd = RgssWindow.TryFindGameWindow(TxtWindowNameFormat.Text)
            If gameWnd Is Nothing Then
                If Not silent Then MsgBox("找不到游戏窗口。", vbExclamation, "错误")
                Return Nothing
            End If
            _gameWindow = gameWnd
            Return gameWnd
        End If

        If _gameWindow.IsAlive Then
            Return _gameWindow
        End If

        _gameWindow = Nothing
        Return GetGameWindow()
    End Function
End Class
