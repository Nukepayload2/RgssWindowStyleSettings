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

    Private Async Sub BtnPressX_Click(sender As Object, e As RoutedEventArgs) Handles BtnPressX.Click
        Await SimulateKeyPressAsync(BtnPressX, VirtualKey.X)
    End Sub

    Private Async Sub BtnPressZ_Click(sender As Object, e As RoutedEventArgs) Handles BtnPressZ.Click
        Await SimulateKeyPressAsync(BtnPressZ, VirtualKey.Z)
    End Sub

    Private ReadOnly _keyMapping As (
        mapFrom As Key, mapTo As VirtualKey,
        isDown As Boolean, isMove As Boolean)() = {
        (Key.W, VirtualKey.Up, False, True),
        (Key.A, VirtualKey.Left, False, True),
        (Key.S, VirtualKey.Down, False, True),
        (Key.D, VirtualKey.Right, False, True),
        (Key.J, VirtualKey.X, False, False),
        (Key.K, VirtualKey.Z, False, False)
    }

    Private _hasMoveKeyDown As Boolean

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

        MapKeys()
        BindShiftToMoveKeys()
    End Sub

    Private Sub MapKeys()
        Dim autoDash = ChkAutoDash.IsChecked.GetValueOrDefault

        For i = 0 To _keyMapping.Length - 1
            Dim keyMp = _keyMapping(i)
            With keyMp
                If Keyboard.IsKeyDown(.mapFrom) Then
                    If .isDown Then
                        ' 按键状态一致
                    Else
                        SendKeyDown(.mapTo)
                        Debug.WriteLine("按下 " & .mapTo.ToString)
                        .isDown = True
                    End If
                Else
                    If .isDown Then
                        SendKeyUp(.mapTo)
                        Debug.WriteLine("放开 " & .mapTo.ToString)
                        .isDown = False
                    Else
                        ' 按键状态一致。但是不发这个非常容易卡住。
                        If autoDash Then SendKeyUp(.mapTo)
                    End If
                End If
            End With
            _keyMapping(i) = keyMp
        Next
    End Sub

    Private Sub BindShiftToMoveKeys()
        Dim autoDash = ChkAutoDash.IsChecked.GetValueOrDefault
        If Not autoDash Then
            Return
        End If

        Dim curShiftDown = False
        For i = 0 To _keyMapping.Length - 1
            With _keyMapping(i)
                If .isDown AndAlso .isMove Then
                    curShiftDown = True
                    Exit For
                End If
            End With
        Next

        If curShiftDown Then
            If _hasMoveKeyDown Then
                ' 不变。
            Else
                SendKeyDown(VirtualKey.LeftShift)
                Debug.WriteLine("按下 Shift")
                _hasMoveKeyDown = True
            End If
        Else
            If _hasMoveKeyDown Then
                SendKeyUp(VirtualKey.LeftShift)
                Debug.WriteLine("松开 Shift")
                _hasMoveKeyDown = False
            Else
                ' 不变。
            End If
        End If
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

    Private Async Function SimulateKeyPressAsync(curButton As UIElement, CurKey As VirtualKey) As Task
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
