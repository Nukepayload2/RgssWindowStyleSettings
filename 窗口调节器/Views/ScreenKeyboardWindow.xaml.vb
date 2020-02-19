Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports Nukepayload2.Diagnostics.Preview

Public Class ScreenKeyboardWindow

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

    Private Async Function SimulateKeyPressAsync(curButton As UIElement, CurKey As VirtualKey) As Task
        Dim gameWnd = RgssSingleWindowManager.GetGameWindow()
        If gameWnd Is Nothing Then
            Return
        End If
        If Not gameWnd.IsForeground Then
            gameWnd.Activate()
            Debug.WriteLine("Game window activated")
        End If
        Await SendKeyPressToGameAsync(CurKey)
        Debug.WriteLine("Send key " & CurKey.ToString)
    End Function

    Private Sub SendKey(key As VirtualKey, options As InjectedInputKeyOptions)
        Dim gameWnd = RgssSingleWindowManager.GetGameWindow(
            silent:=options <> InjectedInputKeyOptions.None)
        If gameWnd Is Nothing Then
            Return
        End If
        If Not gameWnd.IsForeground Then
            gameWnd.Activate()
            Debug.WriteLine("Game window activated")
        End If
        Dim keyInput As New InjectedInputKeyboardInfo With {
            .VirtualKey = key,
            .KeyOptions = options
        }
        Dim inputs As INPUT() = {
            New INPUT With {
                .type = INPUT_KEYBOARD,
                .u = New InputUnion With {.ki = keyInput}
            }}
        SendInput(inputs.Length, inputs, Marshal.SizeOf(Of INPUT))
    End Sub

    Private Sub SendKeyDown(key As VirtualKey)
        SendKey(key, InjectedInputKeyOptions.None)
    End Sub

    Private Sub SendKeyUp(key As VirtualKey)
        SendKey(key, InjectedInputKeyOptions.KeyUp)
    End Sub

    Private _upPressed As Boolean
    Private Sub BtnPressUp_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles BtnPressUp.PreviewMouseLeftButtonDown
        _upPressed = True
        SendKeyDown(VirtualKey.Up)
    End Sub

    Private Sub BtnPressUp_MouseLeaveOrLeftButtonUp() Handles BtnPressUp.MouseLeave, BtnPressUp.PreviewMouseLeftButtonUp
        If _upPressed Then
            _upPressed = False
            SendKeyUp(VirtualKey.Up)
        End If
    End Sub

    Private _downPressed As Boolean
    Private Sub BtnPressDown_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles BtnPressDown.PreviewMouseLeftButtonDown
        _downPressed = True
        SendKeyDown(VirtualKey.Down)
    End Sub

    Private Sub BtnPressDown_MouseLeaveOrLeftButtonUp() Handles BtnPressDown.MouseLeave, BtnPressDown.PreviewMouseLeftButtonUp
        If _downPressed Then
            _downPressed = False
            SendKeyUp(VirtualKey.Down)
        End If
    End Sub

    Private _rightPressed As Boolean
    Private Sub BtnPressRight_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles BtnPressRight.PreviewMouseLeftButtonDown
        _rightPressed = True
        SendKeyDown(VirtualKey.Right)
    End Sub

    Private Sub BtnPressRight_MouseLeaveOrLeftButtonUp() Handles BtnPressRight.MouseLeave, BtnPressRight.PreviewMouseLeftButtonUp
        If _rightPressed Then
            _rightPressed = False
            SendKeyUp(VirtualKey.Right)
        End If
    End Sub

    Private _leftPressed As Boolean
    Private Sub BtnPressLeft_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles BtnPressLeft.PreviewMouseLeftButtonDown
        _leftPressed = True
        SendKeyDown(VirtualKey.Left)
    End Sub

    Private Sub BtnPressLeft_MouseLeaveOrLeftButtonUp() Handles BtnPressLeft.MouseLeave, BtnPressLeft.PreviewMouseLeftButtonUp
        If _leftPressed Then
            _leftPressed = False
            SendKeyUp(VirtualKey.Left)
        End If
    End Sub

End Class
