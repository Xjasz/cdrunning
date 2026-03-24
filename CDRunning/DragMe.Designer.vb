<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DragMe
    Inherits System.Windows.Forms.Form
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        SuspendLayout()
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.Lime
        ClientSize = New Size(100, 100)
        FormBorderStyle = FormBorderStyle.None
        Name = "DragMe"
        Text = "DragMe"
        TopMost = True
        TransparencyKey = Color.Transparent
        ResumeLayout(False)
    End Sub
End Class
