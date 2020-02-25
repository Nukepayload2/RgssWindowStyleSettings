Imports System.Reflection
Imports Newtonsoft.Json.Linq
Imports Nukepayload2.UI.Win32

Public Class AboutWindow

    Private Sub BtnViewSource_Click(sender As Object, e As RoutedEventArgs) Handles BtnViewSource.Click
        Process.Start("explorer", "https://github.com/Nukepayload2/RgssWindowStyleSettings")
    End Sub

    Private Sub BtnViewReleases_Click(sender As Object, e As RoutedEventArgs) Handles BtnViewReleases.Click
        Process.Start("explorer", "https://gitee.com/nukepayload2/RgssWindowStyleMgrDocs/releases")
    End Sub

    Private Async Sub AboutWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim version = GetType(MainWindow).Assembly.GetCustomAttribute(Of AssemblyFileVersionAttribute)?.Version
        TblDescription.Text = $"版本: {version}
作者: Nukepayload2
禁止用于商业用途。如发此软件被贩卖，请举报店铺。"
        Beep()
        TblLatestStable.Text = Await TryGetLatestStableAsync()
    End Sub

    Private Shared s_cachedVersion As String

    Private Async Function TryGetLatestStableAsync() As Task(Of String)
        If s_cachedVersion IsNot Nothing Then
            Return s_cachedVersion
        End If

        Const ErrFailedToGetVersion = "获取失败"
        Try
            Dim wc As New System.Net.Http.HttpClient
            Dim result = Await wc.GetAsync("http://nukepayload2.gitee.io/rgsswindowstylemgrdocs/api/v1/version-info")
            If result.IsSuccessStatusCode Then
                Dim versionJson = Await result.Content.ReadAsStringAsync
                Dim verInfo = JObject.Parse(versionJson)
                Dim latestStable = CType(verInfo("latest-stable"), JValue).Value
                s_cachedVersion = latestStable
                Return latestStable
            Else
                s_cachedVersion = ErrFailedToGetVersion
            End If
        Catch ex As Exception
            s_cachedVersion = ErrFailedToGetVersion
        End Try
        Return s_cachedVersion
    End Function

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

    Private Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles BtnClose.Click, BtnOK.Click
        Close()
    End Sub

    Private Sub TitleBarDragElement_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles TitleBarDragElement.PreviewMouseLeftButtonDown
        DragMove()
    End Sub

    Private Sub BtnSubmitIssues_Click(sender As Object, e As RoutedEventArgs) Handles BtnSubmitIssues.Click
        Process.Start("explorer", "https://gitee.com/nukepayload2/RgssWindowStyleMgrDocs/issues")
    End Sub
End Class
