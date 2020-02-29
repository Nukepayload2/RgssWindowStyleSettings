Option Strict On

Imports System.Runtime.InteropServices
Imports System.Windows.Threading
Imports Nukepayload2.Diagnostics.Preview

Partial Class MainWindow

    WithEvents MapWasdTimer As New DispatcherTimer With {
        .Interval = TimeSpan.FromSeconds(1 / 30)
    }

    Private Sub ChkUseWasd_Checked(sender As Object, e As RoutedEventArgs) Handles ChkUseWasd.Checked
        MapWasdTimer.Start()
    End Sub

    Private Sub ChkUseWasd_Unchecked(sender As Object, e As RoutedEventArgs) Handles ChkUseWasd.Unchecked
        MapWasdTimer.Stop()
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

    Private ReadOnly _downKeys As New List(Of VirtualKey)
    Private ReadOnly _upKeys As New List(Of VirtualKey)
    Private ReadOnly _upKeysLater As New List(Of VirtualKey)

    Private Sub MapWasdTimer_Tick(sender As Object, e As EventArgs) Handles MapWasdTimer.Tick
        Dim gameWnd = RgssSingleWindowManager.GetGameWindow()
        If gameWnd Is Nothing Then
            StopWasdMapping()
            Return
        End If

        If Not gameWnd.IsForeground Then
            Return
        End If

        Dim downKeys = _downKeys
        Dim upKeys = _upKeys
        downKeys.Clear()
        upKeys.Clear()
        upKeys.AddRange(_upKeysLater)
        _upKeysLater.Clear()
        Dim upKeysLater = _upKeysLater
        MapKeys(downKeys, upKeys, upKeysLater)
        BindShiftToMoveKeys(downKeys, upKeys, upKeysLater)
        SendKeyboardStateChanges(downKeys, upKeys)
    End Sub

    Private Shared Sub SendKeyboardStateChanges(downKeys As List(Of VirtualKey), upKeys As List(Of VirtualKey))
        If downKeys.Count + upKeys.Count = 0 Then
            Return
        End If

        Dim inputs(downKeys.Count + upKeys.Count - 1) As INPUT
        For i = 0 To downKeys.Count - 1
            Dim keyInput As New InjectedInputKeyboardInfo With {
                .VirtualKey = downKeys(i)
            }
            inputs(i) = New INPUT With {
                .type = INPUT_KEYBOARD,
                .u = New InputUnion With {.ki = keyInput}
            }
            Debug.WriteLine("按下 " & keyInput.VirtualKey.ToString)
        Next
        For i = downKeys.Count To inputs.Length - 1
            Dim keyInput As New InjectedInputKeyboardInfo With {
                .VirtualKey = upKeys(i - downKeys.Count),
                .KeyOptions = InjectedInputKeyOptions.KeyUp
            }
            inputs(i) = New INPUT With {
                .type = INPUT_KEYBOARD,
                .u = New InputUnion With {.ki = keyInput}
            }
            Debug.WriteLine("放开 " & keyInput.VirtualKey.ToString)
        Next
        SendInput(inputs.Length, inputs, Marshal.SizeOf(Of INPUT))
    End Sub

    Private Sub MapKeys(downKeys As List(Of VirtualKey), upKeys As List(Of VirtualKey), upKeysLater As List(Of VirtualKey))
        For i = 0 To _keyMapping.Length - 1
            Dim keyMp = _keyMapping(i)
            With keyMp
                If .isMove Then
                    ' 方向键按下立即映射
                    If Keyboard.IsKeyDown(.mapFrom) Then
                        If .isDown Then
                            ' 按键状态一致
                        Else
                            downKeys.Add(.mapTo)
                            .isDown = True
                        End If
                    Else
                        If .isDown Then
                            upKeys.Add(.mapTo)
                            .isDown = False
                        Else
                            ' 按键状态一致。
                        End If
                    End If
                Else
                    ' 功能键放开的时候映射按下和放开
                    If Keyboard.IsKeyDown(.mapFrom) Then
                        .isDown = True
                    Else
                        If .isDown Then
                            downKeys.Add(.mapTo)
                            upKeysLater.Add(.mapTo)
                            .isDown = False
                        End If
                    End If
                End If
            End With
            _keyMapping(i) = keyMp
        Next
    End Sub

    Private _shiftPressed As Boolean

    Private Sub BindShiftToMoveKeys(downKeys As List(Of VirtualKey), upKeys As List(Of VirtualKey), upKeysLater As List(Of VirtualKey))
        Dim isJDown = Keyboard.IsKeyDown(Key.J)
        Dim isMoveDown = False
        For i = 0 To _keyMapping.Length - 1
            With _keyMapping(i)
                If .isDown AndAlso .isMove Then
                    isMoveDown = True
                    Exit For
                End If
            End With
        Next
        Dim isJAndMoveDown = isJDown AndAlso isMoveDown
        If isJAndMoveDown AndAlso Not _shiftPressed Then
            downKeys.Add(VirtualKey.LeftShift)
            _shiftPressed = True
        End If
        If Not isJDown AndAlso _shiftPressed Then
            downKeys.Remove(VirtualKey.X)
            upKeys.Remove(VirtualKey.X)
            upKeysLater.Remove(VirtualKey.X)
            upKeys.Add(VirtualKey.LeftShift)
            _shiftPressed = False
            Debug.WriteLine("屏蔽 X 键")
        End If
        If isMoveDown Then
            ' 按下方向的时候屏蔽 X，因为已经替换成 Shift 了。
            downKeys.Remove(VirtualKey.X)
            upKeys.Remove(VirtualKey.X)
            upKeysLater.Remove(VirtualKey.X)
            Debug.WriteLine("屏蔽 X 键")
        End If
    End Sub

    Private Sub StopWasdMapping()
        MapWasdTimer.Stop()
        ChkUseWasd.IsChecked = False
    End Sub

    Private Sub BtnOpenScreenKeyboard_Click(sender As Object, e As RoutedEventArgs) Handles BtnOpenScreenKeyboard.Click
        Dim screenKeyboardWindow = Application.ScreenKeyboardWindow
        screenKeyboardWindow.Owner = Me
        screenKeyboardWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner
        screenKeyboardWindow.Show()
        screenKeyboardWindow.Owner = Nothing
    End Sub
End Class
