Option Strict On

Imports System.Windows.Threading
Imports Nukepayload2.Diagnostics
Imports Nukepayload2.Diagnostics.Preview

Partial Class MainWindow
    Private _injector As InputInjector

    WithEvents MapWasdTimer As New DispatcherTimer With {
        .Interval = TimeSpan.FromSeconds(1 / 30)
    }

    Private Sub ChkUseWasd_Checked(sender As Object, e As RoutedEventArgs) Handles ChkUseWasd.Checked
        MapWasdTimer.Start()
    End Sub

    Private Sub ChkUseWasd_Unchecked(sender As Object, e As RoutedEventArgs) Handles ChkUseWasd.Unchecked
        MapWasdTimer.Stop()
    End Sub

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

    Private ReadOnly _keyMapping As (mapFrom As Key, mapTo As VirtualKey, isDown As Boolean)() = {
        (Key.W, VirtualKey.Up, False), (Key.A, VirtualKey.Left, False),
        (Key.S, VirtualKey.Down, False), (Key.D, VirtualKey.Right, False)
    }

    Private Sub MapWasdTimer_Tick(sender As Object, e As EventArgs) Handles MapWasdTimer.Tick
        Dim injector = GetInputInjector()
        If injector Is Nothing Then
            StopWasdMapping()
            Return
        End If
        Dim gameWnd = GetGameWindow()
        If gameWnd Is Nothing Then
            StopWasdMapping()
            Return
        End If

        If Not gameWnd.IsForeground Then
            Return
        End If

        For i = 0 To _keyMapping.Length - 1
            Dim keyMp = _keyMapping(i)
            With keyMp
                If Keyboard.IsKeyDown(.mapFrom) Then
                    If .isDown Then
                        ' 按键状态一致
                    Else
                        SendKeyDown(.mapTo)
                        .isDown = True
                    End If
                Else
                    If .isDown Then
                        SendKeyUp(.mapTo)
                        .isDown = False
                    Else
                        ' 按键状态一致
                    End If
                End If
            End With
            _keyMapping(i) = keyMp
        Next
    End Sub

    Private Sub StopWasdMapping()
        MapWasdTimer.Stop()
        ChkUseWasd.IsChecked = False
    End Sub

    Private Function GetInputInjector() As InputInjector
        If _injector Is Nothing Then
            Dim injector = InputInjector.TryCreateWithPreviewFeatures
            If injector Is Nothing Then
                MsgBox("当前操作系统不支持输入注入 API。", vbExclamation, "错误")
                Return Nothing
            End If
            _injector = injector
        End If
        Return _injector
    End Function

    Private Async Function SimulateKeyPressAsync(curButton As Button, CurKey As VirtualKey) As Task
        If Not InputInjectionApiInformation.IsInjectKeyboardInputApiPresent Then
            MsgBox("当前操作系统不支持键盘注入 API。", vbExclamation, "错误")
            Return
        End If

        Dim gameWnd = GetGameWindow()
        If gameWnd Is Nothing Then
            Return
        End If
        curButton.IsEnabled = False
        Try
            gameWnd.Activate()
            Await SendKeyPressToGameAsync(CurKey)
        Finally
            curButton.IsEnabled = True
        End Try
    End Function
End Class
