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

    Private _hasMoveKeyDown As Boolean

    Private ReadOnly _downKeys As New List(Of VirtualKey)
    Private ReadOnly _upKeys As New List(Of VirtualKey)

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
        MapKeys(downKeys, upKeys)
        BindShiftToMoveKeys(downKeys, upKeys)

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
        Next
        SendInput(inputs.Length, inputs, Marshal.SizeOf(Of INPUT))
    End Sub

    Private Sub MapKeys(downKeys As List(Of VirtualKey), upKeys As List(Of VirtualKey))
        Dim autoDash = ChkAutoDash.IsChecked.GetValueOrDefault

        For i = 0 To _keyMapping.Length - 1
            Dim keyMp = _keyMapping(i)
            With keyMp
                If Keyboard.IsKeyDown(.mapFrom) Then
                    If .isDown Then
                        ' 按键状态一致
                    Else
                        downKeys.Add(.mapTo)
                        Debug.WriteLine("按下 " & .mapTo.ToString)
                        .isDown = True
                    End If
                Else
                    If .isDown Then
                        upKeys.Add(.mapTo)
                        Debug.WriteLine("放开 " & .mapTo.ToString)
                        .isDown = False
                    Else
                        ' 按键状态一致。
                    End If
                End If
            End With
            _keyMapping(i) = keyMp
        Next
    End Sub

    Private Sub BindShiftToMoveKeys(downKeys As List(Of VirtualKey), upKeys As List(Of VirtualKey))
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
                downKeys.Add(VirtualKey.LeftShift)
                Debug.WriteLine("按下 Shift")
                _hasMoveKeyDown = True
            End If
        Else
            If _hasMoveKeyDown Then
                upKeys.Add(VirtualKey.LeftShift)
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

    Private Sub BtnOpenScreenKeyboard_Click(sender As Object, e As RoutedEventArgs) Handles BtnOpenScreenKeyboard.Click
        Application.ScreenKeyboardWindow.Show()
    End Sub
End Class
