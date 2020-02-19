Partial Class MainWindow

    Private Sub BtnAbout_Click(sender As Object, e As RoutedEventArgs) Handles BtnAbout.Click
        Dim aboutWindow = Application.AboutWindow
        aboutWindow.Owner = Me
        aboutWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner
        aboutWindow.ShowDialog()
    End Sub

    Private Sub BtnMinimize_Click(sender As Object, e As RoutedEventArgs) Handles BtnMinimize.Click
        WindowState = WindowState.Minimized
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles BtnClose.Click
        Close()
    End Sub

End Class
