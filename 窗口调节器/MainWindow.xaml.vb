Imports System.ComponentModel
Imports System.Reflection

Class MainWindow
    Private Sub BtnResizable_Click(sender As Object, e As RoutedEventArgs) Handles BtnResizable.Click
        TblStatus.Text = String.Empty
        Dim gameWnd = RgssWindow.TryFindGameWindow(TxtWindowNameFormat.Text)
        If gameWnd Is Nothing Then
            MsgBox("找不到游戏窗口。", vbExclamation, "错误")
            Return
        End If
        Dim wndStyle As New RgssWindowStyle(ChkCanResize.IsChecked, ChkCanMaximize.IsChecked)
        If Not gameWnd.TryApplyWindowStyle(wndStyle) Then
            MsgBox("应用样式时发生问题。", vbExclamation, "错误")
        End If
        TblStatus.Text = "已应用游戏窗口样式"
    End Sub

    Private Sub BtnDetect_Click(sender As Object, e As RoutedEventArgs) Handles BtnDetect.Click
        DetectGameWindow(silent:=False)
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        TxtWindowNameFormat.Text = Application.Settings.WindowPattern
        ExpBasicSettings.IsExpanded = True
        DetectGameWindow(silent:=True)
    End Sub

    Private Sub DetectGameWindow(silent As Boolean)
        TblStatus.Text = String.Empty
        Dim gameWnd = RgssWindow.TryFindGameWindow(TxtWindowNameFormat.Text)
        If gameWnd Is Nothing Then
            If Not silent Then MsgBox("找不到游戏窗口。", vbExclamation, "错误")
            Return
        End If
        Dim detectResult = gameWnd.TryDetectWindowStyle
        If detectResult Is Nothing Then
            If Not silent Then MsgBox("获取游戏窗口样式失败。", vbExclamation, "错误")
            Return
        End If
        ChkCanMaximize.IsChecked = detectResult.CanMaximize
        ChkCanResize.IsChecked = detectResult.CanResize
        If Not silent Then TblStatus.Text = "已检测游戏窗口样式"
    End Sub

    Private Sub ChkCanResize_Click(sender As Object, e As RoutedEventArgs) Handles ChkCanResize.Click
        TblStatus.Text = String.Empty
    End Sub

    Private Sub ChkCanMaximize_Click(sender As Object, e As RoutedEventArgs) Handles ChkCanMaximize.Click
        TblStatus.Text = String.Empty
    End Sub

    Private Sub BtnAbout_Click(sender As Object, e As RoutedEventArgs) Handles BtnAbout.Click
        Dim version = GetType(MainWindow).Assembly.GetCustomAttribute(Of AssemblyFileVersionAttribute)?.Version
        MsgBox("版本: " & version, MsgBoxStyle.Information, "关于 " & Title)
    End Sub

    Private Sub ExpBasicSettings_Expanded(sender As Object, e As RoutedEventArgs) Handles ExpBasicSettings.Expanded
        ExpAdvancedSettings.IsExpanded = False
    End Sub

    Private Sub ExpAdvancedSettings_Expanded(sender As Object, e As RoutedEventArgs) Handles ExpAdvancedSettings.Expanded
        ExpBasicSettings.IsExpanded = False
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Application.Settings.WindowPattern = TxtWindowNameFormat.Text
    End Sub

    Private Sub BtnViewSource_Click(sender As Object, e As RoutedEventArgs) Handles BtnViewSource.Click
        Process.Start("explorer", "https://github.com/Nukepayload2/RgssWindowStyleSettings")
    End Sub
End Class
