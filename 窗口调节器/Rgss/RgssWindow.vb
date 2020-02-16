Option Compare Text
Imports System.Runtime.InteropServices

Public Class RgssWindow
    Private ReadOnly _handle As IntPtr
    Public ReadOnly Property IsAlive As Boolean
        Get
            Return IsWindow(_handle)
        End Get
    End Property

    Public ReadOnly Property IsForeground As Boolean
        Get
            Dim inputFocus = GetForegroundWindow
            If inputFocus = Nothing Then
                Return False
            End If
            Return inputFocus = _handle
        End Get
    End Property

    Sub New(handle As IntPtr)
        _handle = handle
    End Sub

    Private Shared Function FindGameWindowHandle(wildcard As String) As IntPtr
        Dim gameWnd As IntPtr
        Do
            gameWnd = FindWindowEx(Nothing, gameWnd, "RGSS Player", Nothing)
            Dim title = GetWindowTitle(gameWnd)
            If title Like wildcard Then
                Return gameWnd
            End If
        Loop While gameWnd <> Nothing
    End Function

    Private Shared Function GetWindowTitle(hWnd As IntPtr) As String
        Dim length As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)
        Dim title As New String(New Char, length)
        SendMessage(hWnd, WM_GETTEXT, length + 1, title)
        Return title
    End Function

    Public Sub Activate()
        ActivateInternal(_handle)
    End Sub

    Private Shared Sub ActivateInternal(hwndApp As IntPtr)

        ' SetActiveWindow on Win32 only activates the Window - it does
        ' not bring to to the foreground unless the window belongs to
        ' the current thread.  NativeMethods.SetForegroundWindow() activates the
        ' window, moves it to the foreground, and bumps the priority
        ' of the thread which owns the window.

        Dim dwDummy As Integer ' dummy arg for SafeNativeMethods.GetWindowThreadProcessId

        ' Attach ourselves to the window we want to set focus to
        AttachThreadInput(0, GetWindowThreadProcessId(hwndApp, dwDummy), 1)
        ' Make it foreground and give it focus, this will occur
        ' synchronously because we are attached.
        SetForegroundWindow(hwndApp)
        SetFocus(hwndApp)
        ' Unattach ourselves from the window
        AttachThreadInput(0, GetWindowThreadProcessId(hwndApp, dwDummy), 0)
    End Sub

    Public Shared Function TryFindGameWindow(wildcard As String) As RgssWindow
        Dim hwnd = FindGameWindowHandle(wildcard)
        If hwnd = Nothing Then
            Return Nothing
        End If
        Return New RgssWindow(hwnd)
    End Function

    Public Function TryDetectWindowStyle() As RgssWindowStyle
        Dim wndStyle = GetWindowLong(_handle, GWL_STYLE).ToInt64
        If wndStyle = 0 AndAlso Marshal.GetLastWin32Error <> 0 Then
            Return Nothing
        End If
        Dim canResize = HasFlag(wndStyle, WS_THICKFRAME)
        Dim canMaximize = (wndStyle And WS_MAXIMIZEBOX) = WS_MAXIMIZEBOX
        Return New RgssWindowStyle(canResize, canMaximize)
    End Function

    Public Function TryApplyWindowStyle(style As RgssWindowStyle) As Boolean
        Dim wndStyle = GetWindowLong(_handle, GWL_STYLE).ToInt64
        If wndStyle = 0 AndAlso Marshal.GetLastWin32Error <> 0 Then
            Return False
        End If
        Dim newStyle = wndStyle
        If style.CanResize Then
            newStyle = AddFlag(newStyle, WS_THICKFRAME)
        Else
            newStyle = RemoveFlag(newStyle, WS_THICKFRAME)
        End If
        If style.CanMaximize Then
            newStyle = AddFlag(newStyle, WS_MAXIMIZEBOX)
        Else
            newStyle = RemoveFlag(newStyle, WS_MAXIMIZEBOX)
        End If
        SetWindowLong(_handle, GWL_STYLE, newStyle)
        Return Marshal.GetLastWin32Error = 0
    End Function

    Private Shared Function HasFlag(wndStyle As Long, flag As Long) As Boolean
        Return (wndStyle And flag) = flag
    End Function

    Private Shared Function AddFlag(newStyle As Long, flag As Long) As Long
        Return newStyle Or flag
    End Function

    Private Shared Function RemoveFlag(newStyle As Long, flag As Long) As Long
        Return newStyle And (Not flag)
    End Function
End Class
