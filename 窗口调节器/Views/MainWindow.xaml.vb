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
End Class
