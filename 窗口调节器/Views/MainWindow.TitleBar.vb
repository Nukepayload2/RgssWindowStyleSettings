Imports System.Reflection

Partial Class MainWindow

    Private Sub BtnAbout_Click(sender As Object, e As RoutedEventArgs) Handles BtnAbout.Click
        Dim version = GetType(MainWindow).Assembly.
            GetCustomAttribute(Of AssemblyFileVersionAttribute)?.Version
        MsgBox($"版本: {version}
作者: Nukepayload2
禁止用于商业用途。如发此软件被贩卖，请举报店铺。", MsgBoxStyle.Information, "关于 " & Title)
    End Sub

    Private Sub BtnViewSource_Click(sender As Object, e As RoutedEventArgs) Handles BtnViewSource.Click
        Process.Start("explorer", "https://github.com/Nukepayload2/RgssWindowStyleSettings")
    End Sub

    Private Sub BtnViewReleases_Click(sender As Object, e As RoutedEventArgs) Handles BtnViewReleases.Click
        Process.Start("explorer", "https://gitee.com/nukepayload2/RgssWindowStyleMgrDocs/releases")
    End Sub

End Class
