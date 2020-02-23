Imports Nukepayload2.UI.Win32

Partial Class MainWindow
    Private Sub TitleBarDragElement_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles TitleBarDragElement.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub MainWindow_SourceInitialized(sender As Object, e As EventArgs) Handles Me.SourceInitialized
        If Win32ApiInformation.IsWindowAcrylicApiPresent OrElse Win32ApiInformation.IsAeroGlassApiPresent Then
            ' Enable blur effect
            Dim windowCompositionFactory As New WindowCompositionFactory
            Dim composition = windowCompositionFactory.TryCreateForCurrentView
            If composition?.TrySetBlur(Me, True) Then
                TitleBar.Background = New SolidColorBrush(Color.FromArgb(&H99, &HFF, &HFF, &HFF))
                ClientArea.Background = New SolidColorBrush(Color.FromArgb(&HCC, &HFF, &HFF, &HFF))
                Background = Brushes.Transparent
            End If
        End If
    End Sub

End Class
