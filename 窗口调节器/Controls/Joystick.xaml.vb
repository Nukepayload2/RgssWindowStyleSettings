Public Class Joystick

    Public Event JoystickDirectionChanged(oldDirection As JoystickDirection, newDirection As JoystickDirection)

    Private _Direction As JoystickDirection
    Public Property Direction As JoystickDirection
        Get
            Return _Direction
        End Get
        Private Set(value As JoystickDirection)
            If _Direction <> value Then
                Dim old = _Direction
                _Direction = value
                RaiseEvent JoystickDirectionChanged(old, value)
            End If
        End Set
    End Property

    Private Function PushJoystick(pt As Point) As JoystickDirection
        Const MaxLength = 50
        Const HalfSize = 40
        Const ThresholdSquared = 16 ^ 2
        Dim direction As New Vector(pt.X - HalfSize, pt.Y - HalfSize)

        Dim lengthSquared = direction.LengthSquared
        Dim length = Math.Sqrt(lengthSquared)
        If length > MaxLength Then
            Dim rate = length / MaxLength
            direction = New Vector(direction.X / rate, direction.Y / rate)
        End If
        JoystickTransform.X = direction.X
        JoystickTransform.Y = direction.Y

        If lengthSquared > ThresholdSquared Then

            If direction.X > 0 Then
                If direction.Y > 0 Then
                    If direction.X > direction.Y Then
                        Return JoystickDirection.Right
                    Else
                        Return JoystickDirection.Down
                    End If
                Else
                    If direction.X > -direction.Y Then
                        Return JoystickDirection.Right
                    Else
                        Return JoystickDirection.Up
                    End If
                End If
            Else
                If direction.Y > 0 Then
                    If -direction.X > direction.Y Then
                        Return JoystickDirection.Left
                    Else
                        Return JoystickDirection.Down
                    End If
                Else
                    If -direction.X > -direction.Y Then
                        Return JoystickDirection.Left
                    Else
                        Return JoystickDirection.Up
                    End If
                End If
            End If
        Else
            Return JoystickDirection.None
        End If
    End Function

    Private Sub Joystick_TouchDown(sender As Object, e As TouchEventArgs) Handles Me.TouchDown
        CaptureTouch(e.TouchDevice)
        Dim pt = e.GetTouchPoint(JoystickHatBack).Position
        Direction = PushJoystick(pt)
    End Sub

    Private Sub Joystick_TouchMove(sender As Object, e As TouchEventArgs) Handles Me.TouchMove
        Dim pt = e.GetTouchPoint(JoystickHatBack).Position
        Direction = PushJoystick(pt)
    End Sub

    Private Sub Joystick_TouchUp(sender As Object, e As TouchEventArgs) Handles Me.TouchUp
        ReleaseTouchCapture(e.TouchDevice)
        Direction = JoystickDirection.None

        JoystickTransform.X = 0
        JoystickTransform.Y = 0
    End Sub

    Private _mousePressed As Boolean
    Private Sub Joystick_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.PreviewMouseLeftButtonDown
        If Not e.LeftButton = MouseButtonState.Pressed Then
            Return ' 不能跟触摸输入混淆了
        End If

        _mousePressed = True
        Dim pt = e.GetPosition(JoystickHatBack)
        CaptureMouse()
        Direction = PushJoystick(pt)
    End Sub

    Private Sub Joystick_PreviewMouseMove(sender As Object, e As MouseEventArgs) Handles Me.PreviewMouseMove
        If Not _mousePressed OrElse Not e.LeftButton = MouseButtonState.Pressed Then
            Return
        End If
        Dim pt = e.GetPosition(JoystickHatBack)
        Direction = PushJoystick(pt)
    End Sub

    Private Sub Joystick_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles Me.PreviewMouseLeftButtonUp
        If Not _mousePressed Then
            Return
        End If
        _mousePressed = False
        ReleaseMouseCapture()
        Direction = JoystickDirection.None
        JoystickTransform.X = 0
        JoystickTransform.Y = 0
    End Sub
End Class

Public Enum JoystickDirection
    None
    Left
    Up
    Right
    Down
End Enum
