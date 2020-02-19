Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports Nukepayload2.Diagnostics
Imports Nukepayload2.Diagnostics.Preview

Public Class ScreenKeyboardWindow

    Private Async Sub BtnPressUp_Click(sender As Object, e As RoutedEventArgs) Handles BtnPressUp.Click
        Await SimulateKeyPressAsync(BtnPressUp, VirtualKey.Up)
    End Sub

    Private Async Sub BtnPressDown_Click(sender As Object, e As RoutedEventArgs) Handles BtnPressDown.Click
        Await SimulateKeyPressAsync(BtnPressDown, VirtualKey.Down)
    End Sub

    Private Async Sub BtnPressLeft_Click(sender As Object, e As RoutedEventArgs) Handles BtnPressLeft.Click
        Await SimulateKeyPressAsync(BtnPressLeft, VirtualKey.Left)
    End Sub

    Private Async Sub BtnPressRight_Click(sender As Object, e As RoutedEventArgs) Handles BtnPressRight.Click
        Await SimulateKeyPressAsync(BtnPressRight, VirtualKey.Right)
    End Sub

    Private Async Sub BtnPressX_Click(sender As Object, e As RoutedEventArgs) Handles BtnPressX.Click
        Await SimulateKeyPressAsync(BtnPressX, VirtualKey.X)
    End Sub

    Private Async Sub BtnPressZ_Click(sender As Object, e As RoutedEventArgs) Handles BtnPressZ.Click
        Await SimulateKeyPressAsync(BtnPressZ, VirtualKey.Z)
    End Sub

    Private Sub ScreenKeyboardWindow_SourceInitialized(sender As Object, e As EventArgs) Handles Me.SourceInitialized
        Dim hWnd = New WindowInteropHelper(Me).Handle
        SetWindowLong(hWnd, GWL_EXSTYLE, GetWindowLong(hWnd, GWL_EXSTYLE).ToInt64 Or WS_EX_NOACTIVATE)
    End Sub

    Private Sub BtnPressDown_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles BtnPressDown.PreviewMouseLeftButtonDown
        Debug.WriteLine("Down press")
    End Sub

    Private Sub BtnPressDown_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles BtnPressDown.PreviewMouseLeftButtonUp
        Debug.WriteLine("Down up")
    End Sub

    Private Async Function SimulateKeyPressAsync(curButton As UIElement, CurKey As VirtualKey) As Task
        Dim gameWnd = RgssSingleWindowManager.GetGameWindow()
        If gameWnd Is Nothing Then
            Return
        End If
        curButton.IsEnabled = False
        Try
            If Not gameWnd.IsForeground Then
                gameWnd.Activate()
                Debug.WriteLine("Game window activated")
            End If
            Await SendKeyPressToGameAsync(CurKey)
            Debug.WriteLine("Send key " & CurKey.ToString)
        Finally
            curButton.IsEnabled = True
        End Try
    End Function

    Private Sub SendKeyUp(key As VirtualKey)
        Dim keyInput As New InjectedInputKeyboardInfo With {
            .VirtualKey = key,
            .KeyOptions = InjectedInputKeyOptions.KeyUp
        }
        Dim inputs As INPUT() = {
            New INPUT With {
                .type = INPUT_KEYBOARD,
                .u = New InputUnion With {.ki = keyInput}
            }}
        SendInput(inputs.Length, inputs, Marshal.SizeOf(Of INPUT))
    End Sub

    Private Sub SendKeyDown(key As VirtualKey)
        Dim keyInput As New InjectedInputKeyboardInfo With {
            .VirtualKey = key,
            .KeyOptions = InjectedInputKeyOptions.KeyUp
        }
        Dim inputs As INPUT() = {
            New INPUT With {
                .type = INPUT_KEYBOARD,
                .u = New InputUnion With {.ki = keyInput}
            }}
        SendInput(inputs.Length, inputs, Marshal.SizeOf(Of INPUT))
    End Sub

End Class
