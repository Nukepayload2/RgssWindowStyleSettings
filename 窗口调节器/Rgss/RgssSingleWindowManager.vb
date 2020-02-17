Public Class RgssSingleWindowManager

    Private Shared s_gameWindow As RgssWindow

    Public Shared Function GetGameWindow(Optional silent As Boolean = False) As RgssWindow
        If s_gameWindow Is Nothing Then
            Dim gameWnd = RgssWindow.TryFindGameWindow(Application.Settings.WindowPattern)
            If gameWnd Is Nothing Then
                If Not silent Then MsgBox("找不到游戏窗口。请确保游戏在运行，而且进阶设置中填写的游戏窗口正确。", vbExclamation, "错误")
                Return Nothing
            End If
            s_gameWindow = gameWnd
            Return gameWnd
        End If

        If s_gameWindow.IsAlive Then
            Return s_gameWindow
        End If

        s_gameWindow = Nothing
        Return GetGameWindow()
    End Function
End Class
