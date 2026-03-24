<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CDRunningForm
    Inherits System.Windows.Forms.Form
    <System.Diagnostics.DebuggerNonUserCode()>
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

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        lblStart = New Button()
        txtStages = New TextBox()
        tmrStartStop = New Timer(components)
        cmbPresets = New ComboBox()
        lblStatus = New Label()
        chkBR = New CheckBox()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        Button5 = New Button()
        cmbStartKey = New ComboBox()
        cmbBRKey = New ComboBox()
        tmrKeyBinds = New Timer(components)
        cmbShootKey = New ComboBox()
        chkAutoFocus = New CheckBox()
        cmbFocusKey = New ComboBox()
        cmbPingKey = New ComboBox()
        chkAutoPing = New CheckBox()
        chkAutoShoot = New CheckBox()
        chkClickPosition = New CheckBox()
        chkAutoRecoilSwap = New CheckBox()
        btnSaveDrag = New Button()
        lblObjectSwapMapper = New Label()
        btnOpenSwapMapper = New Button()
        lblStatus2 = New Label()
        SuspendLayout()
        ' 
        ' lblStart
        ' 
        lblStart.Location = New Point(12, 12)
        lblStart.Name = "lblStart"
        lblStart.Size = New Size(75, 23)
        lblStart.TabIndex = 0
        lblStart.Text = "start"
        lblStart.UseVisualStyleBackColor = True
        ' 
        ' txtStages
        ' 
        txtStages.BorderStyle = BorderStyle.FixedSingle
        txtStages.Location = New Point(135, 66)
        txtStages.Multiline = True
        txtStages.Name = "txtStages"
        txtStages.Size = New Size(226, 93)
        txtStages.TabIndex = 1
        txtStages.Text = "2:0:1000;"
        ' 
        ' tmrStartStop
        ' 
        ' 
        ' cmbPresets
        ' 
        cmbPresets.DropDownStyle = ComboBoxStyle.DropDownList
        cmbPresets.FlatStyle = FlatStyle.Flat
        cmbPresets.FormattingEnabled = True
        cmbPresets.Location = New Point(135, 14)
        cmbPresets.Name = "cmbPresets"
        cmbPresets.Size = New Size(226, 23)
        cmbPresets.TabIndex = 2
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        lblStatus.Location = New Point(400, 265)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(93, 21)
        lblStatus.TabIndex = 4
        lblStatus.Text = "status: none"
        ' 
        ' chkBR
        ' 
        chkBR.AutoSize = True
        chkBR.Location = New Point(8, 70)
        chkBR.Name = "chkBR"
        chkBR.Size = New Size(79, 19)
        chkBR.TabIndex = 5
        chkBR.Text = "BattleRifle"
        chkBR.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(307, 38)
        Button2.Name = "Button2"
        Button2.Size = New Size(24, 23)
        Button2.TabIndex = 6
        Button2.Text = "+"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(337, 38)
        Button3.Name = "Button3"
        Button3.Size = New Size(24, 23)
        Button3.TabIndex = 7
        Button3.Text = "-"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(165, 38)
        Button4.Name = "Button4"
        Button4.Size = New Size(24, 23)
        Button4.TabIndex = 8
        Button4.Text = "S"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(135, 38)
        Button5.Name = "Button5"
        Button5.Size = New Size(24, 23)
        Button5.TabIndex = 9
        Button5.Text = "R"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' cmbStartKey
        ' 
        cmbStartKey.DropDownStyle = ComboBoxStyle.DropDownList
        cmbStartKey.FormattingEnabled = True
        cmbStartKey.Items.AddRange(New Object() {"F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "NUMPAD0", "NUMPAD1", "NUMPAD2", "NUMPAD3", "NUMPAD4", "NUMPAD5", "NUMPAD6", "NUMPAD7", "NUMPAD8", "NUMPAD9"})
        cmbStartKey.Location = New Point(13, 38)
        cmbStartKey.Name = "cmbStartKey"
        cmbStartKey.Size = New Size(74, 23)
        cmbStartKey.TabIndex = 12
        ' 
        ' cmbBRKey
        ' 
        cmbBRKey.DropDownStyle = ComboBoxStyle.DropDownList
        cmbBRKey.FormattingEnabled = True
        cmbBRKey.Items.AddRange(New Object() {"F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "NUMPAD0", "NUMPAD1", "NUMPAD2", "NUMPAD3", "NUMPAD4", "NUMPAD5", "NUMPAD6", "NUMPAD7", "NUMPAD8", "NUMPAD9"})
        cmbBRKey.Location = New Point(13, 88)
        cmbBRKey.Name = "cmbBRKey"
        cmbBRKey.Size = New Size(74, 23)
        cmbBRKey.TabIndex = 13
        ' 
        ' tmrKeyBinds
        ' 
        tmrKeyBinds.Interval = 25
        ' 
        ' cmbShootKey
        ' 
        cmbShootKey.DropDownStyle = ComboBoxStyle.DropDownList
        cmbShootKey.FormattingEnabled = True
        cmbShootKey.Items.AddRange(New Object() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "capslock"})
        cmbShootKey.Location = New Point(13, 212)
        cmbShootKey.Name = "cmbShootKey"
        cmbShootKey.Size = New Size(74, 23)
        cmbShootKey.TabIndex = 14
        ' 
        ' chkAutoFocus
        ' 
        chkAutoFocus.AutoSize = True
        chkAutoFocus.Location = New Point(8, 111)
        chkAutoFocus.Name = "chkAutoFocus"
        chkAutoFocus.Size = New Size(83, 19)
        chkAutoFocus.TabIndex = 15
        chkAutoFocus.Text = "AutoFocus"
        chkAutoFocus.UseVisualStyleBackColor = True
        ' 
        ' cmbFocusKey
        ' 
        cmbFocusKey.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFocusKey.FormattingEnabled = True
        cmbFocusKey.Items.AddRange(New Object() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "capslock"})
        cmbFocusKey.Location = New Point(13, 129)
        cmbFocusKey.Name = "cmbFocusKey"
        cmbFocusKey.Size = New Size(74, 23)
        cmbFocusKey.TabIndex = 16
        ' 
        ' cmbPingKey
        ' 
        cmbPingKey.DropDownStyle = ComboBoxStyle.DropDownList
        cmbPingKey.FormattingEnabled = True
        cmbPingKey.Items.AddRange(New Object() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "capslock"})
        cmbPingKey.Location = New Point(13, 170)
        cmbPingKey.Name = "cmbPingKey"
        cmbPingKey.Size = New Size(74, 23)
        cmbPingKey.TabIndex = 18
        ' 
        ' chkAutoPing
        ' 
        chkAutoPing.AutoSize = True
        chkAutoPing.Location = New Point(8, 152)
        chkAutoPing.Name = "chkAutoPing"
        chkAutoPing.Size = New Size(76, 19)
        chkAutoPing.TabIndex = 17
        chkAutoPing.Text = "AutoPing"
        chkAutoPing.UseVisualStyleBackColor = True
        ' 
        ' chkAutoShoot
        ' 
        chkAutoShoot.AutoSize = True
        chkAutoShoot.Location = New Point(8, 193)
        chkAutoShoot.Name = "chkAutoShoot"
        chkAutoShoot.Size = New Size(83, 19)
        chkAutoShoot.TabIndex = 19
        chkAutoShoot.Text = "AutoShoot"
        chkAutoShoot.UseVisualStyleBackColor = True
        ' 
        ' chkClickPosition
        ' 
        chkClickPosition.AutoSize = True
        chkClickPosition.Location = New Point(8, 257)
        chkClickPosition.Name = "chkClickPosition"
        chkClickPosition.Size = New Size(95, 19)
        chkClickPosition.TabIndex = 20
        chkClickPosition.Text = "ClickPosition"
        chkClickPosition.UseVisualStyleBackColor = True
        ' 
        ' chkAutoRecoilSwap
        ' 
        chkAutoRecoilSwap.AutoSize = True
        chkAutoRecoilSwap.Location = New Point(8, 237)
        chkAutoRecoilSwap.Name = "chkAutoRecoilSwap"
        chkAutoRecoilSwap.Size = New Size(112, 19)
        chkAutoRecoilSwap.TabIndex = 21
        chkAutoRecoilSwap.Text = "AutoRecoilSwap"
        chkAutoRecoilSwap.UseVisualStyleBackColor = True
        ' 
        ' btnSaveDrag
        ' 
        btnSaveDrag.Location = New Point(275, 266)
        btnSaveDrag.Name = "btnSaveDrag"
        btnSaveDrag.Size = New Size(75, 23)
        btnSaveDrag.TabIndex = 24
        btnSaveDrag.Text = "SaveDrag"
        btnSaveDrag.UseVisualStyleBackColor = True
        ' 
        ' lblObjectSwapMapper
        ' 
        lblObjectSwapMapper.AutoSize = True
        lblObjectSwapMapper.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        lblObjectSwapMapper.Location = New Point(384, 9)
        lblObjectSwapMapper.Name = "lblObjectSwapMapper"
        lblObjectSwapMapper.Size = New Size(155, 21)
        lblObjectSwapMapper.TabIndex = 26
        lblObjectSwapMapper.Text = "Object Swap Mapper"
        ' 
        ' btnOpenSwapMapper
        ' 
        btnOpenSwapMapper.Location = New Point(418, 37)
        btnOpenSwapMapper.Name = "btnOpenSwapMapper"
        btnOpenSwapMapper.Size = New Size(75, 23)
        btnOpenSwapMapper.TabIndex = 31
        btnOpenSwapMapper.Text = "Open"
        btnOpenSwapMapper.UseVisualStyleBackColor = True
        ' 
        ' lblStatus2
        ' 
        lblStatus2.AutoSize = True
        lblStatus2.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
        lblStatus2.Location = New Point(400, 233)
        lblStatus2.Name = "lblStatus2"
        lblStatus2.Size = New Size(93, 21)
        lblStatus2.TabIndex = 32
        lblStatus2.Text = "status: none"
        ' 
        ' CDRunningForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(551, 303)
        Controls.Add(lblStatus2)
        Controls.Add(btnOpenSwapMapper)
        Controls.Add(lblObjectSwapMapper)
        Controls.Add(btnSaveDrag)
        Controls.Add(chkAutoRecoilSwap)
        Controls.Add(chkClickPosition)
        Controls.Add(chkAutoShoot)
        Controls.Add(cmbPingKey)
        Controls.Add(chkAutoPing)
        Controls.Add(cmbFocusKey)
        Controls.Add(chkAutoFocus)
        Controls.Add(cmbShootKey)
        Controls.Add(cmbBRKey)
        Controls.Add(cmbStartKey)
        Controls.Add(Button5)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(chkBR)
        Controls.Add(lblStatus)
        Controls.Add(cmbPresets)
        Controls.Add(txtStages)
        Controls.Add(lblStart)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "CDRunningForm"
        Text = "CR Running"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblStart As Button
    Friend WithEvents txtStages As TextBox
    Friend WithEvents tmrStartStop As Timer
    Friend WithEvents cmbPresets As ComboBox
    Friend WithEvents btnTesty As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents chkBR As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents cmbStartKey As ComboBox
    Friend WithEvents cmbBRKey As ComboBox
    Friend WithEvents txtStartKey As TextBox
    Friend WithEvents txtBRKey As TextBox
    Friend WithEvents tmrKeyBinds As Timer
    Friend WithEvents cmbShootKey As ComboBox
    Friend WithEvents chkAutoFocus As CheckBox
    Friend WithEvents cmbFocusKey As ComboBox
    Friend WithEvents cmbPingKey As ComboBox
    Friend WithEvents chkAutoPing As CheckBox
    Friend WithEvents chkAutoShoot As CheckBox
    Friend WithEvents chkClickPosition As CheckBox
    Friend WithEvents chkAutoRecoilSwap As CheckBox
    Friend WithEvents btnSaveDrag As Button
    Friend WithEvents btnCreateSwap As Button
    Friend WithEvents lblObjectSwapMapper As Label
    Friend WithEvents btnSwapSave As Button
    Friend WithEvents btnLock As Button
    Friend WithEvents btnOpenSwapMapper As Button
    Friend WithEvents lblStatus2 As Label
End Class
