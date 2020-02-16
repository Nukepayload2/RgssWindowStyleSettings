Class MainWindow
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        TxtWindowNameFormat.Text = Application.Settings.WindowPattern
        ExpBasicSettings.IsExpanded = True
        DetectGameWindow(silent:=True)
    End Sub

    Private Async Sub ExpBasicSettings_Expanded(sender As Object, e As RoutedEventArgs) Handles ExpBasicSettings.Expanded
        ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
        ExpAdvancedSettings.IsExpanded = False
        Await Task.Delay(100)
        ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto
    End Sub

    Private Async Sub ExpAdvancedSettings_Expanded(sender As Object, e As RoutedEventArgs) Handles ExpAdvancedSettings.Expanded
        ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
        ExpBasicSettings.IsExpanded = False
        Await Task.Delay(100)
        ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto
    End Sub

    Private Sub ChkUseWasd_Checked(sender As Object, e As RoutedEventArgs) Handles ChkUseWasd.Checked

    End Sub

    Private Sub ChkUseWasd_Unchecked(sender As Object, e As RoutedEventArgs) Handles ChkUseWasd.Unchecked

    End Sub
End Class
