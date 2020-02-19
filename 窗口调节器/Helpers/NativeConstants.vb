Imports System.Runtime.InteropServices

Module NativeConstants
    Public Const WM_GETTEXTLENGTH = &HE
    Public Const WM_GETTEXT = &HD
    Public Const GWL_STYLE = -16
    Public Const WS_THICKFRAME = &H40000
    Public Const WS_MAXIMIZEBOX = &H10000
    Public Const INPUT_KEYBOARD = 1
    Public Const GWL_EXSTYLE = -20
    Public Const WS_EX_NOACTIVATE = &H8000000
End Module

<StructLayout(LayoutKind.Explicit)>
Structure InputUnion
    <FieldOffset(0)>
    Public mi As Nukepayload2.Diagnostics.Preview.InjectedInputMouseInfo
    <FieldOffset(0)>
    Public ki As Nukepayload2.Diagnostics.Preview.InjectedInputKeyboardInfo
End Structure

Structure INPUT
    Public type As Integer
    Public u As InputUnion
End Structure
