Partial Class MainWindow
    Private Async Function EndExpanderOperationAsync() As Task
        Await Task.Delay(100)
        ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto
    End Function

    Private Sub BeginExpanderOperation()
        ContentScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
    End Sub

    Private Async Sub ExpBasicSettings_Expanded(sender As Object, e As RoutedEventArgs) Handles ExpBasicSettings.Expanded
        BeginExpanderOperation()
        ExpAdvancedSettings.IsExpanded = False
        ExpKeyboardSettings.IsExpanded = False
        Await EndExpanderOperationAsync()
    End Sub

    Private Async Sub ExpAdvancedSettings_Expanded(sender As Object, e As RoutedEventArgs) Handles ExpAdvancedSettings.Expanded
        BeginExpanderOperation()
        ExpBasicSettings.IsExpanded = False
        ExpKeyboardSettings.IsExpanded = False
        Await EndExpanderOperationAsync()
    End Sub

    Private Async Sub ExpKeyboardSettings_Expanded(sender As Object, e As RoutedEventArgs) Handles ExpKeyboardSettings.Expanded
        BeginExpanderOperation()
        ExpBasicSettings.IsExpanded = False
        ExpAdvancedSettings.IsExpanded = False
        Await EndExpanderOperationAsync()
    End Sub
End Class
