Imports Nukepayload2.Diagnostics
Imports Nukepayload2.Diagnostics.Preview

Public Class ScreenKeyboardWindow
    Private _injector As InputInjector

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

    Private Function GetWin10InputInjector() As InputInjector
        If _injector Is Nothing Then
            Dim injector = InputInjector.TryCreateWithPreviewFeatures
            If injector Is Nothing Then
                MsgBox("当前操作系统不支持 Win10 输入注入 API。", vbExclamation, "错误")
                Return Nothing
            End If
            _injector = injector
        End If
        Return _injector
    End Function

    Private Async Function SimulateKeyPressAsync(curButton As UIElement, CurKey As VirtualKey) As Task
        If Not InputInjectionApiInformation.IsInjectKeyboardInputApiPresent Then
            MsgBox("当前操作系统不支持 Win10 键盘注入 API。", vbExclamation, "错误")
            Return
        End If

        Dim gameWnd = RgssSingleWindowManager.GetGameWindow()
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
