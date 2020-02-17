Partial Class MainWindow

    Private Sub BtnHelpTitleFormat_Click(sender As Object, e As RoutedEventArgs) Handles BtnHelpTitleFormat.Click
        Process.Start("explorer", "https://gitee.com/nukepayload2/RgssWindowStyleMgrDocs/blob/master/%E7%AA%97%E5%8F%A3%E5%90%8D%E7%A7%B0%E6%A0%BC%E5%BC%8F%E8%AF%B4%E6%98%8E.md")
    End Sub

    Private Sub TxtWindowNameFormat_LostFocus(sender As Object, e As RoutedEventArgs) Handles TxtWindowNameFormat.LostFocus
        Application.Settings.WindowPattern = TxtWindowNameFormat.Text
    End Sub
End Class
