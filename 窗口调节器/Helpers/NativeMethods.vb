Imports System.Runtime.InteropServices

Module NativeMethods
    Declare Unicode Function FindWindowEx Lib "user32.dll" Alias "FindWindowExW" (
        hWndParent As IntPtr, hWndChildAfter As IntPtr,
        lpClassName As String, lpWindowName As String) As IntPtr

    Private Declare Unicode Function GetWindowLong32 Lib "user32.dll" Alias "GetWindowLongW" (
        hWnd As IntPtr, nIndex As Integer) As IntPtr

    Private Declare Unicode Function GetWindowLong64 Lib "user32.dll" Alias "GetWindowLongPtrW" (
        hWnd As IntPtr, nIndex As Integer) As IntPtr

    Public Function GetWindowLong(hWnd As IntPtr, nIndex As Integer) As IntPtr
        If IntPtr.Size = 4 Then
            Return GetWindowLong32(hWnd, nIndex)
        Else
            Return GetWindowLong64(hWnd, nIndex)
        End If
    End Function

    Private Declare Unicode Function SetWindowLong32 Lib "user32" Alias "SetWindowLongW" (
        hWnd As IntPtr, index As Integer, newLong As IntPtr) As IntPtr

    Private Declare Unicode Function SetWindowLong64 Lib "user32" Alias "SetWindowLongPtrW" (
        hWnd As IntPtr, index As Integer, newLong As IntPtr) As IntPtr

    Public Function SetWindowLong(hWnd As IntPtr, index As Integer, newLong As IntPtr) As IntPtr
        If IntPtr.Size = 4 Then
            Return SetWindowLong32(hWnd, index, newLong)
        Else
            Return SetWindowLong64(hWnd, index, newLong)
        End If
    End Function

    Declare Unicode Function SendMessage Lib "user32" Alias "SendMessageW" (
        hWnd As IntPtr, Msg As Integer, wParam As IntPtr,
        <MarshalAs(UnmanagedType.LPWStr)> lParam As String) As IntPtr

    Declare Function GetWindowThreadProcessId Lib "user32" (
        hwnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer

    Declare Function AttachThreadInput Lib "user32" (
        idAttach As Integer, idAttachTo As Integer, fAttach As Integer) As Integer

    Declare Function SetForegroundWindow Lib "user32" (
        hwnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    Declare Function SetFocus Lib "user32" (
        hwnd As IntPtr) As IntPtr

    Declare Function GetForegroundWindow Lib "user32" () As IntPtr

    Declare Function IsWindow Lib "user32" (
        hwnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

End Module
