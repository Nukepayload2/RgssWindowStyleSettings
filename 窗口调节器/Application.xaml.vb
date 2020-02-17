Class Application

    ' 应用程序级事件(例如 Startup、Exit 和 DispatcherUnhandledException)
    ' 可以在此文件中进行处理。

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        Forms.Application.EnableVisualStyles()
        Try
            If Not Settings.IsLoaded Then
                Settings.Load()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Application_Exit(sender As Object, e As ExitEventArgs) Handles Me.[Exit]
        Settings.Save()
    End Sub

    Public Shared ReadOnly Property Settings As New SettingsModel

#Region "ScreenKeyboardWindow 窗口的单实例管理"
    Private Shared _ScreenKeyboardWindow As ScreenKeyboardWindow

    Public Shared ReadOnly Property ScreenKeyboardWindow As ScreenKeyboardWindow
        Get
            If _ScreenKeyboardWindow Is Nothing Then
                _ScreenKeyboardWindow = New ScreenKeyboardWindow
                AddHandler _ScreenKeyboardWindow.Closed, AddressOf ScreenKeyboardWindowClosed
            End If

            Return _ScreenKeyboardWindow
        End Get
    End Property

    Private Shared Sub ScreenKeyboardWindowClosed(sender As Object, e As EventArgs)
        RemoveHandler _ScreenKeyboardWindow.Closed, AddressOf ScreenKeyboardWindowClosed
        _ScreenKeyboardWindow = Nothing
    End Sub
#End Region

End Class
