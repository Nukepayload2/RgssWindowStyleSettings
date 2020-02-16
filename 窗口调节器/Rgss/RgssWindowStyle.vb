Public Class RgssWindowStyle
    Public Sub New(canResize As Boolean, canMaximize As Boolean)
        Me.CanResize = canResize
        Me.CanMaximize = canMaximize
    End Sub

    Public ReadOnly Property CanResize As Boolean
    Public ReadOnly Property CanMaximize As Boolean
End Class
