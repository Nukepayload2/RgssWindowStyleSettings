Imports System.Reflection
Imports Nukepayload2.UI.Win32

Public Class AboutWindow

    Private Sub BtnViewSource_Click(sender As Object, e As RoutedEventArgs) Handles BtnViewSource.Click
        Process.Start("explorer", "https://github.com/Nukepayload2/RgssWindowStyleSettings")
    End Sub

    Private Sub BtnViewReleases_Click(sender As Object, e As RoutedEventArgs) Handles BtnViewReleases.Click
        Process.Start("explorer", "https://gitee.com/nukepayload2/RgssWindowStyleMgrDocs/releases")
    End Sub

    Private Sub AboutWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim version = GetType(MainWindow).Assembly.GetCustomAttribute(Of AssemblyFileVersionAttribute)?.Version
        TblDescription.Text = $"版本: {version}
作者: Nukepayload2
禁止用于商业用途。如发此软件被贩卖，请举报店铺。"
        Beep()
    End Sub

    Private Sub MainWindow_SourceInitialized(sender As Object, e As EventArgs) Handles Me.SourceInitialized
        Dim windowCompositionFactory As New WindowCompositionFactory
        If Win32ApiInformation.IsWindowAcrylicApiPresent OrElse Win32ApiInformation.IsAeroGlassApiPresent Then
            ' Enable blur effect
            Dim composition = windowCompositionFactory.TryCreateForCurrentView
            If composition?.TrySetBlur(Me, True) Then
                TitleBar.Background = New SolidColorBrush(Color.FromArgb(&H99, &HFF, &HFF, &HFF))
                ClientArea.Background = New SolidColorBrush(Color.FromArgb(&HCC, &HFF, &HFF, &HFF))
                Background = Brushes.Transparent
            End If
        End If
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles BtnClose.Click
        Close()
    End Sub

    Private Sub TitleBarDragElement_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles TitleBarDragElement.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub BtnSubmitIssues_Click(sender As Object, e As RoutedEventArgs) Handles BtnSubmitIssues.Click
        Process.Start("explorer", "https://gitee.com/nukepayload2/RgssWindowStyleMgrDocs/issues")
    End Sub
End Class
