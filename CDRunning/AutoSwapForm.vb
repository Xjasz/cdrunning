Imports System.ComponentModel

Public Class AutoSwapForm

    Public Shared SWAPMAPPER_ACTIVE As Boolean = False
    Public Shared SWAPMAPPERDRAG_LOCKED As Boolean = False

    Private Sub LoadSwapForm_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Not Visible Then Return
        LoadSwapForm()
        cmbPresets.Items.Clear()
        For Each item In CDRunningForm.cmbPresets.Items
            cmbPresets.Items.Add(item)
        Next
        cmbPresets.SelectedIndex = CDRunningForm.cmbPresets.SelectedIndex
    End Sub

    Private Sub LoadSwapForm_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        CDRunningForm.Show()
        RefreshSwapper()
    End Sub

    Private Sub cmbAllSwaps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAllSwaps.SelectedIndexChanged
        If cmbAllSwaps.Text <> "" Then
            For Each item As SwapMapperClass In SWAP_MAPPER_ITEMS
                If item.smsName = cmbAllSwaps.Text Then
                    CURRENT_SWAP_MAPPER_ITEM = item
                End If
            Next
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnOther.Click
        RefreshSwapper()
        Hide()
        CDRunningForm.Show()
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        If cmbPresets.Text <> "" Then
            If btnCreate.Text = "Create" Then
                For Each item As SwapMapperClass In SWAP_MAPPER_ITEMS
                    If item.smsName = cmbPresets.Text Then
                        MsgBox("There is already one with that name")
                        Exit Sub
                    End If
                Next
                If SWAPMAPPER_ACTIVE = False Then
                    btnCreate.Text = "Cancel"
                    SWAPMAPPER_ACTIVE = True
                    SWAPMAPPERDRAG_LOCKED = False
                    CURRENT_SWAP_MAPPER_ITEM = New SwapMapperClass With {.smsName = CDRunningForm.cmbPresets.Text}
                    lblCurrentMapper.Text = CURRENT_SWAP_MAPPER_ITEM.SMSToString()
                    DragMe.Show()
                    DragMe.Opacity = 1
                    btnLock.Enabled = True
                Else
                    RefreshSwapper()
                End If
            ElseIf btnCreate.Text = "Cancel" Then
                RefreshSwapper()
            End If
        Else
            MsgBox("Must select preset before creating a mapping", MsgBoxStyle.Critical)
            btnLock.Enabled = False
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If SWAPMAPPER_ACTIVE And SWAPMAPPERDRAG_LOCKED Then
            If CURRENT_SWAP_MAPPER_ITEM.smsName <> Nothing And CURRENT_SWAP_MAPPER_ITEM.smsRect <> Nothing And CURRENT_SWAP_MAPPER_ITEM.smsPoints.Count > 0 And CURRENT_SWAP_MAPPER_ITEM.smsColors.Count <> Nothing Then
                For Each item As SwapMapperClass In SWAP_MAPPER_ITEMS
                    If item.smsName = CURRENT_SWAP_MAPPER_ITEM.smsName Then
                        Dim temp_items As New List(Of SwapMapperClass)
                        For Each xitem As SwapMapperClass In SWAP_MAPPER_ITEMS
                            If xitem.smsName <> CURRENT_SWAP_MAPPER_ITEM.smsName Then
                                temp_items.Add(xitem)
                            End If
                        Next
                        SWAP_MAPPER_ITEMS = temp_items
                    End If
                Next
                SWAP_MAPPER_ITEMS.Add(CURRENT_SWAP_MAPPER_ITEM)
                CDRunningForm.CURRENT_RECOIL_INDEX = -1
                RefreshSwapper()
                LoadSwapForm()
            End If
        Else
            MsgBox("There is nothing to save", MsgBoxStyle.Critical)
        End If
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        If cmbAllSwaps.Text <> "" Then
            Dim activeSMSObject As SwapMapperClass = Nothing
            DragMe.Hide()
            If IsNothing(CURRENT_SWAP_MAPPER_ITEM) = False Then
                Using bmp As New Bitmap(10, 1)
                    activeSMSObject = CDRunningForm.CheckSwapMapperItem(CURRENT_SWAP_MAPPER_ITEM, bmp)
                    If IsNothing(activeSMSObject) = False Then
                        MsgBox(CURRENT_SWAP_MAPPER_ITEM.smsName & " was found")
                    End If
                End Using
            End If
            DragMe.Show()
        Else
            MsgBox("Must select a Mapping before trying to test", MsgBoxStyle.Critical)
        End If

    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If cmbAllSwaps.Text <> "" Then
            'If SWAPMAPPER_ACTIVE Then
            RemoveSwapItem(cmbAllSwaps.Text)
            RefreshSwapper()

            'End If
        Else
            MsgBox("Must select a Mapping to remove first", MsgBoxStyle.Critical)
        End If
    End Sub
    Public Sub RemoveSwapItem(ByVal value As String)
        Dim temp_items As New List(Of SwapMapperClass)
        For Each item As SwapMapperClass In SWAP_MAPPER_ITEMS
            If item.smsName <> value Then
                temp_items.Add(item)
            End If
        Next
        SWAP_MAPPER_ITEMS = temp_items
        CDRunningForm.CURRENT_RECOIL_INDEX = -1
        RefreshSwapper()
        LoadSwapForm()
    End Sub
    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        If cmbAllSwaps.Text <> "" Then
            Dim loadName = cmbAllSwaps.Text
            For Each item As SwapMapperClass In SWAP_MAPPER_ITEMS
                If item.smsName = loadName Then
                    CURRENT_SWAP_MAPPER_ITEM = item
                End If
            Next
            If IsNothing(CURRENT_SWAP_MAPPER_ITEM) = False Then
                lblCurrentMapper.Text = CURRENT_SWAP_MAPPER_ITEM.SMSToString()
                SWAPMAPPER_ACTIVE = True
                SWAPMAPPERDRAG_LOCKED = True
                btnCreate.Text = "Cancel"
                btnLock.Enabled = True
                btnLock.Text = "Unlock"
                DragMe.Show()
                DragMe.Opacity = 0.4
                DragMe.Location = CURRENT_SWAP_MAPPER_ITEM.smsRect.Location
            End If
        Else
            MsgBox("There is nothing to load", MsgBoxStyle.Critical)
        End If
    End Sub

    Private Sub btnLock_Click(sender As Object, e As EventArgs) Handles btnLock.Click
        If SWAPMAPPER_ACTIVE Then
            If SWAPMAPPERDRAG_LOCKED Then
                btnLock.Text = "Lock"
                SWAPMAPPERDRAG_LOCKED = False
                DragMe.Opacity = 1
                CURRENT_SWAP_MAPPER_ITEM.smsRect = Nothing
                lblCurrentMapper.Text = CURRENT_SWAP_MAPPER_ITEM.SMSToString()
            Else
                SWAPMAPPERDRAG_LOCKED = True
                btnLock.Text = "Unlock"
                DragMe.Opacity = 0.4
                Dim dragMeRect = New Rectangle(DragMe.Location, DragMe.Size)
                CURRENT_SWAP_MAPPER_ITEM.smsRect = dragMeRect
                Debug.WriteLine(CURRENT_SWAP_MAPPER_ITEM.SMSToString())
                lblCurrentMapper.Text = CURRENT_SWAP_MAPPER_ITEM.SMSToString()
            End If
        End If
    End Sub

    Private Sub RefreshSwapper()
        btnCreate.Text = "Create"
        btnLock.Text = "Lock"
        btnLock.Enabled = False
        SWAPMAPPER_ACTIVE = False
        SWAPMAPPERDRAG_LOCKED = False
        DragMe.Opacity = 1
        CURRENT_SWAP_MAPPER_ITEM = New SwapMapperClass()
        lblCurrentMapper.Text = ""
        DragMe.Hide()
    End Sub

    Private Sub LoadSwapForm()
        cmbAllSwaps.Items.Clear()
        For Each item As SwapMapperClass In SWAP_MAPPER_ITEMS
            cmbAllSwaps.Items.Add(item.smsName)
        Next
        If cmbAllSwaps.Items.Count > 0 Then
            cmbAllSwaps.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmbPresets_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPresets.SelectedIndexChanged
        If cmbPresets.SelectedIndex >= 0 Then CDRunningForm.cmbPresets.SelectedIndex = cmbPresets.SelectedIndex
    End Sub

End Class