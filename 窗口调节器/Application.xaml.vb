Class Application

    ' 应用程序级事件(例如 Startup、Exit 和 DispatcherUnhandledException)
    ' 可以在此文件中进行处理。

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        Dim oldCurDir = IO.Directory.GetCurrentDirectory
        SetCurDirToLocalState()
        Forms.Application.EnableVisualStyles()
        Try
            If Not Settings.IsLoaded Then
                Settings.Load()
            End If
        Catch ex As Exception
        End Try
        IO.Directory.SetCurrentDirectory(oldCurDir)
    End Sub

    Private Sub SetCurDirToLocalState()
        Dim localState = TryGetLocalStateFolder()
        If localState IsNot Nothing Then
            IO.Directory.SetCurrentDirectory(localState)
        End If
    End Sub

    Private Const PackageSeriesId = "72c889b9-99b2-4bc8-b922-9d031e1252cf_h0pghn313xt82"

    Private Function TryGetLocalStateFolder() As String
        Dim folder = $"%localappdata%\Packages\{PackageSeriesId}\LocalState"
        folder = Environment.ExpandEnvironmentVariables(folder)
        If Not IO.Directory.Exists(folder) Then
            Return Nothing
        End If
        Return folder
    End Function

    Private Sub Application_Exit(sender As Object, e As ExitEventArgs) Handles Me.[Exit]
        Dim oldCurDir = IO.Directory.GetCurrentDirectory
        SetCurDirToLocalState()
        Try
            Settings.Save()
        Catch ex As Exception
        End Try
        IO.Directory.SetCurrentDirectory(oldCurDir)
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


#Region "AboutWindow 窗口的单实例管理"
    Private Shared _AboutWindow As AboutWindow

    Public Shared ReadOnly Property AboutWindow As AboutWindow
        Get
            If _AboutWindow Is Nothing Then
                _AboutWindow = New AboutWindow
                AddHandler _AboutWindow.Closed, AddressOf AboutWindowClosed
            End If

            Return _AboutWindow
        End Get
    End Property

    Private Shared Sub AboutWindowClosed(sender As Object, e As EventArgs)
        RemoveHandler _AboutWindow.Closed, AddressOf AboutWindowClosed
        _AboutWindow = Nothing
    End Sub
#End Region

End Class
