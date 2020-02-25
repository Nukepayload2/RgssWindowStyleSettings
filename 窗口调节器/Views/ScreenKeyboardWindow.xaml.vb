Imports System.Runtime.InteropServices
Imports System.Windows.Interop
Imports System.Windows.Media.Animation
Imports System.Windows.Threading
Imports Nukepayload2.Diagnostics.Preview
Imports Nukepayload2.UI.Win32

Public Class ScreenKeyboardWindow

    Private _xButtonPressed As Boolean
    Private _xButtonAsShift As Boolean

    Private Sub BtnPressX_MouseLeftButtonDown() Handles BtnPressX.PreviewMouseLeftButtonDown, BtnPressX.PreviewTouchDown, BtnPressX.TouchEnter
        If _xButtonPressed Then
            Return
        End If
        _xButtonPressed = True
        If _directionPressed > 0 Then
            _xButtonAsShift = True
            SendKeyDown(VirtualKey.LeftShift)
        End If
    End Sub

    Private Async Sub BtnPressX_LeftUpOrMouseLeave() Handles BtnPressX.PreviewMouseLeftButtonUp, BtnPressX.MouseLeave, BtnPressX.PreviewTouchUp, BtnPressX.TouchLeave
        If Not _xButtonPressed Then
            Return
        End If
        _xButtonPressed = False
        If _xButtonAsShift Then
            _xButtonAsShift = False
            SendKeyUp(VirtualKey.LeftShift)
        Else
            Await SimulateKeyPressAsync(VirtualKey.X)
        End If
    End Sub

    Private _zButtonPressed As Boolean
    Private Sub BtnPressZ_MouseLeftButtonDown() Handles BtnPressZ.PreviewMouseLeftButtonDown, BtnPressZ.PreviewTouchDown, BtnPressZ.TouchEnter
        _zButtonPressed = True
    End Sub

    Private Async Sub BtnPressZ_MouseLeftButtonUp() Handles BtnPressZ.PreviewMouseLeftButtonUp, BtnPressZ.MouseLeave, BtnPressZ.PreviewTouchUp, BtnPressZ.TouchLeave
        If Not _zButtonPressed Then
            Return
        End If
        _zButtonPressed = False
        Await SimulateKeyPressAsync(VirtualKey.Z)
    End Sub

    Private _directionPressed As Integer
    Private _upPressed As Boolean
    Private Sub BtnPressUp_MouseLeftButtonDown() Handles BtnPressUp.PreviewMouseLeftButtonDown, BtnPressUp.PreviewTouchDown, BtnPressUp.TouchEnter
        If _upPressed Then
            Return
        End If
        _upPressed = True
        OnDirectionPressed()
        SendKeyDown(VirtualKey.Up)
    End Sub

    Private Sub BtnPressUp_MouseLeaveOrLeftButtonUp() Handles BtnPressUp.MouseLeave, BtnPressUp.PreviewMouseLeftButtonUp, BtnPressUp.PreviewTouchUp, BtnPressUp.TouchLeave
        If _upPressed Then
            _directionPressed -= 1
            _upPressed = False
            SendKeyUp(VirtualKey.Up)
        End If
    End Sub

    Private _downPressed As Boolean
    Private Sub BtnPressDown_MouseLeftButtonDown() Handles BtnPressDown.PreviewMouseLeftButtonDown, BtnPressDown.PreviewTouchDown, BtnPressDown.TouchEnter
        If _downPressed Then
            Return
        End If
        _downPressed = True
        OnDirectionPressed()
        SendKeyDown(VirtualKey.Down)
    End Sub

    Private Sub BtnPressDown_MouseLeaveOrLeftButtonUp() Handles BtnPressDown.MouseLeave, BtnPressDown.PreviewMouseLeftButtonUp, BtnPressDown.PreviewTouchUp, BtnPressDown.TouchLeave
        If _downPressed Then
            _directionPressed -= 1
            _downPressed = False
            SendKeyUp(VirtualKey.Down)
        End If
    End Sub

    Private _rightPressed As Boolean
    Private Sub BtnPressRight_MouseLeftButtonDown() Handles BtnPressRight.PreviewMouseLeftButtonDown, BtnPressRight.PreviewTouchDown, BtnPressRight.TouchEnter
        If _rightPressed Then
            Return
        End If
        _rightPressed = True
        OnDirectionPressed()
        SendKeyDown(VirtualKey.Right)
    End Sub

    Private Sub BtnPressRight_MouseLeaveOrLeftButtonUp() Handles BtnPressRight.MouseLeave, BtnPressRight.PreviewMouseLeftButtonUp, BtnPressRight.PreviewTouchUp, BtnPressRight.TouchLeave
        If _rightPressed Then
            _directionPressed -= 1
            _rightPressed = False
            SendKeyUp(VirtualKey.Right)
        End If
    End Sub

    Private _leftPressed As Boolean
    Private Sub BtnPressLeft_MouseLeftButtonDown() Handles BtnPressLeft.PreviewMouseLeftButtonDown, BtnPressLeft.PreviewTouchDown, BtnPressLeft.TouchEnter
        If _leftPressed Then
            Return
        End If
        _leftPressed = True
        OnDirectionPressed()
        SendKeyDown(VirtualKey.Left)
    End Sub

    Private Sub BtnPressLeft_MouseLeaveOrLeftButtonUp() Handles BtnPressLeft.MouseLeave, BtnPressLeft.PreviewMouseLeftButtonUp, BtnPressLeft.PreviewTouchUp, BtnPressLeft.TouchLeave
        If _leftPressed Then
            _directionPressed -= 1
            _leftPressed = False
            SendKeyUp(VirtualKey.Left)
        End If
    End Sub

    Private Sub OnDirectionPressed()
        _directionPressed += 1
        If _xButtonPressed AndAlso Not _xButtonAsShift Then
            _xButtonAsShift = True
            SendKeyDown(VirtualKey.LeftShift)
        End If
    End Sub

    Private Sub ScreenKeyboardWindow_SourceInitialized(sender As Object, e As EventArgs) Handles Me.SourceInitialized
        Dim hWnd = New WindowInteropHelper(Me).Handle
        SetWindowLong(hWnd, GWL_EXSTYLE, GetWindowLong(hWnd, GWL_EXSTYLE).ToInt64 Or WS_EX_NOACTIVATE)
    End Sub

    Private Async Function SimulateKeyPressAsync(curKey As VirtualKey) As Task
        Dim gameWnd = RgssSingleWindowManager.GetGameWindow()
        If gameWnd Is Nothing Then
            Return
        End If
        If Not gameWnd.IsForeground Then
            gameWnd.Activate()
            Debug.WriteLine("Game window activated")
        End If
        SendKeyDown(curKey)
        Await Task.Delay(100)
        SendKeyUp(curKey)
        Debug.WriteLine("Send key " & curKey.ToString)
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

    Private Async Sub BtnClose_Click(sender As Object, e As RoutedEventArgs) Handles BtnClose.Click
        BtnClose.IsHitTestVisible = False
        Const AnimTime = 200
        Dim entranceAnim As New DoubleAnimation With {
            .From = 0, .[To] = ActualHeight,
            .Duration = TimeSpan.FromMilliseconds(AnimTime),
            .EasingFunction = New CubicEase With {
                .EasingMode = EasingMode.EaseIn
            }
        }
        EntranceTransform.BeginAnimation(TranslateTransform.YProperty, entranceAnim)
        Await Task.Delay(AnimTime)
        Dim mainWnd = Application.Current.MainWindow
        If mainWnd Is Nothing Then
            mainWnd = New MainWindow
            Application.Current.MainWindow = mainWnd
        End If
        mainWnd.Show()
        Close()
    End Sub

    WithEvents DockTimer As New DispatcherTimer With {.Interval = TimeSpan.FromSeconds(2)}

    Private Async Sub ScreenKeyboardWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        DockTimer_Tick()
        Const AnimTime = 200
        Dim entranceAnim As New DoubleAnimation With {
            .From = ActualHeight, .[To] = 0,
            .Duration = TimeSpan.FromMilliseconds(AnimTime),
            .EasingFunction = New CubicEase With {
                .EasingMode = EasingMode.EaseOut
            }
        }
        EntranceTransform.BeginAnimation(TranslateTransform.YProperty, entranceAnim)
        Await Task.Delay(AnimTime)
        DockTimer.Start()
    End Sub

    Private Sub ScreenKeyboardWindow_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        DockTimer.Stop()
    End Sub

    Private Sub DockTimer_Tick() Handles DockTimer.Tick
        Dim curWindow = New WindowInteropHelper(Me).Handle
        Dim scr = Forms.Screen.FromHandle(curWindow)
        With scr.WorkingArea
            Dim curHeight = ActualHeight
            Dim dpiHlp As New PerMonitorDpiAwareHelper
            Dim dpi = dpiHlp.GetWindowDpi(curWindow)
            If dpi IsNot Nothing Then
                Dim dpiScale = dpi.Value.Y / 96
                curHeight *= dpiScale
            End If
            SetWindowPos(curWindow, IntPtr.Zero,
                         .Left,
                         .Top + .Height - curHeight,
                         .Width,
                         curHeight,
                         SWP_NOZORDER Or SWP_NOOWNERZORDER Or SWP_NOACTIVATE)
        End With
    End Sub
End Class
