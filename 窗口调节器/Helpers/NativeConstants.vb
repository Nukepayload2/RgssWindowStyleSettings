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
	Public Const CCHDEVICENAME As Integer = 32
	Public Const MONITOR_DEFAULTTONEAREST = 2
	Public Const SWP_NOZORDER = 4
	Public Const SWP_NOOWNERZORDER = &H200
	Public Const SWP_NOACTIVATE = &H10
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

''' <summary>
''' The MONITORINFOEX structure contains information about a display monitor.
''' The GetMonitorInfo function stores information into a MONITORINFOEX structure or a MONITORINFO structure.
''' The MONITORINFOEX structure is a superset of the MONITORINFO structure. The MONITORINFOEX structure adds a string member to contain a name
''' for the display monitor.
''' </summary>
<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
Structure MonitorInfoEx
	''' <summary>
	''' The size, in bytes, of the structure. Set this member to sizeof(MONITORINFOEX) (72) before calling the GetMonitorInfo function.
	''' Doing so lets the function determine the type of structure you are passing to it.
	''' </summary>
	Public Size As Integer

	''' <summary>
	''' A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates.
	''' Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
	''' </summary>
	Public Monitor As RECT

	''' <summary>
	''' A RECT structure that specifies the work area rectangle of the display monitor that can be used by applications,
	''' expressed in virtual-screen coordinates. Windows uses this rectangle to maximize an application on the monitor.
	''' The rest of the area in rcMonitor contains system windows such as the task bar and side bars.
	''' Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
	''' </summary>
	Public WorkArea As RECT

	''' <summary>
	''' The attributes of the display monitor.
	'''
	''' This member can be the following value:
	'''   1 : MONITORINFOF_PRIMARY
	''' </summary>
	Public Flags As UInteger

	''' <summary>
	''' A string that specifies the device name of the monitor being used. Most applications have no use for a display monitor name,
	''' and so can save some bytes by using a MONITORINFO structure.
	''' </summary>
	<MarshalAs(UnmanagedType.ByValTStr, SizeConst:=CCHDEVICENAME)>
	Public DeviceName As String

	Public Sub Init()
		Size = Marshal.SizeOf(Of MonitorInfoEx)
		DeviceName = String.Empty
	End Sub
End Structure

''' <summary>
''' The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle. 
''' </summary>
''' <remarks>
''' https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect
''' By convention, the right and bottom edges of the rectangle are normally considered exclusive.
''' In other words, the pixel whose coordinates are ( right, bottom ) lies immediately outside of the the rectangle.
''' For example, when RECT is passed to the FillRect function, the rectangle is filled up to, but not including,
''' the right column and bottom row of pixels. This structure is identical to the RECTL structure.
''' </remarks>
<StructLayout(LayoutKind.Sequential)>
Public Structure RECT
	''' <summary>
	''' The x-coordinate of the upper-left corner of the rectangle.
	''' </summary>
	Public Left As Integer

	''' <summary>
	''' The y-coordinate of the upper-left corner of the rectangle.
	''' </summary>
	Public Top As Integer

	''' <summary>
	''' The x-coordinate of the lower-right corner of the rectangle.
	''' </summary>
	Public Right As Integer

	''' <summary>
	''' The y-coordinate of the lower-right corner of the rectangle.
	''' </summary>
	Public Bottom As Integer
End Structure
