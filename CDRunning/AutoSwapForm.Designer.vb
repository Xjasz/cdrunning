<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AutoSwapForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        cmbAllSwaps = New ComboBox()
        btnOther = New Button()
        btnCreate = New Button()
        btnTest = New Button()
        btnSave = New Button()
        lblCurrentMapper = New Label()
        btnLock = New Button()
        btnRemove = New Button()
        btnLoad = New Button()
        cmbPresets = New ComboBox()
        SuspendLayout()
        ' 
        ' cmbAllSwaps
        ' 
        cmbAllSwaps.DropDownStyle = ComboBoxStyle.DropDownList
        cmbAllSwaps.FlatStyle = FlatStyle.Flat
        cmbAllSwaps.FormattingEnabled = True
        cmbAllSwaps.Location = New Point(91, 36)
        cmbAllSwaps.Name = "cmbAllSwaps"
        cmbAllSwaps.Size = New Size(226, 23)
        cmbAllSwaps.TabIndex = 3
        ' 
        ' btnOther
        ' 
        btnOther.Location = New Point(12, 204)
        btnOther.Name = "btnOther"
        btnOther.Size = New Size(64, 23)
        btnOther.TabIndex = 33
        btnOther.Text = "Other"
        btnOther.UseVisualStyleBackColor = True
        ' 
        ' btnCreate
        ' 
        btnCreate.Location = New Point(12, 62)
        btnCreate.Name = "btnCreate"
        btnCreate.Size = New Size(64, 23)
        btnCreate.TabIndex = 34
        btnCreate.Text = "Create"
        btnCreate.UseVisualStyleBackColor = True
        ' 
        ' btnTest
        ' 
        btnTest.Location = New Point(12, 175)
        btnTest.Name = "btnTest"
        btnTest.Size = New Size(64, 23)
        btnTest.TabIndex = 35
        btnTest.Text = "Test"
        btnTest.UseVisualStyleBackColor = True
        ' 
        ' btnSave
        ' 
        btnSave.Location = New Point(12, 120)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(64, 23)
        btnSave.TabIndex = 36
        btnSave.Text = "Save"
        btnSave.UseVisualStyleBackColor = True
        ' 
        ' lblCurrentMapper
        ' 
        lblCurrentMapper.Font = New Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point)
        lblCurrentMapper.Location = New Point(108, 62)
        lblCurrentMapper.Name = "lblCurrentMapper"
        lblCurrentMapper.Size = New Size(243, 139)
        lblCurrentMapper.TabIndex = 37
        lblCurrentMapper.Text = "none"
        ' 
        ' btnLock
        ' 
        btnLock.Enabled = False
        btnLock.Location = New Point(323, 11)
        btnLock.Name = "btnLock"
        btnLock.Size = New Size(74, 23)
        btnLock.TabIndex = 38
        btnLock.Text = "Lock"
        btnLock.UseVisualStyleBackColor = True
        ' 
        ' btnRemove
        ' 
        btnRemove.Location = New Point(12, 146)
        btnRemove.Name = "btnRemove"
        btnRemove.Size = New Size(64, 23)
        btnRemove.TabIndex = 39
        btnRemove.Text = "Remove"
        btnRemove.UseVisualStyleBackColor = True
        ' 
        ' btnLoad
        ' 
        btnLoad.Location = New Point(12, 91)
        btnLoad.Name = "btnLoad"
        btnLoad.Size = New Size(64, 23)
        btnLoad.TabIndex = 40
        btnLoad.Text = "Load"
        btnLoad.UseVisualStyleBackColor = True
        ' 
        ' cmbPresets
        ' 
        cmbPresets.DropDownStyle = ComboBoxStyle.DropDownList
        cmbPresets.FlatStyle = FlatStyle.Flat
        cmbPresets.FormattingEnabled = True
        cmbPresets.Location = New Point(91, 7)
        cmbPresets.Name = "cmbPresets"
        cmbPresets.Size = New Size(226, 23)
        cmbPresets.TabIndex = 41
        ' 
        ' AutoSwapForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(411, 239)
        Controls.Add(cmbPresets)
        Controls.Add(btnLoad)
        Controls.Add(btnRemove)
        Controls.Add(btnLock)
        Controls.Add(lblCurrentMapper)
        Controls.Add(btnSave)
        Controls.Add(btnTest)
        Controls.Add(btnCreate)
        Controls.Add(btnOther)
        Controls.Add(cmbAllSwaps)
        FormBorderStyle = FormBorderStyle.FixedSingle
        ImeMode = ImeMode.On
        Name = "AutoSwapForm"
        Text = "AutoSwapForm"
        TopMost = True
        ResumeLayout(False)
    End Sub

    Friend WithEvents cmbAllSwaps As ComboBox
    Friend WithEvents btnLoadSwap As Button
    Friend WithEvents btnOther As Button
    Friend WithEvents btnCreate As Button
    Friend WithEvents btnTest As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents lblCurrentMapper As Label
    Friend WithEvents btnLock As Button
    Friend WithEvents btnRemove As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents cmbPresets As ComboBox
End Class
