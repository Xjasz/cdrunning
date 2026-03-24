Public Class DragMe

    Public Shared DragMe_Down As Boolean = False

    Private Sub DragMe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Size = New Size(50, 50)
    End Sub

    Private Sub DragMe_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        If AutoSwapForm.SWAPMAPPER_ACTIVE And AutoSwapForm.SWAPMAPPERDRAG_LOCKED = False Then
            Dim currentPoint = CDRunningForm.GetMousePosition()
            Dim Xpos = currentPoint.X - Width / 2
            Dim Ypos = currentPoint.Y - Height / 2
            Dim newPoint As New Point(Xpos, Ypos)
            Location = newPoint
            DragMe_Down = True
        End If
    End Sub

    Private Sub DragMe_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        If AutoSwapForm.SWAPMAPPER_ACTIVE And AutoSwapForm.SWAPMAPPERDRAG_LOCKED = False Then
            DragMe_Down = False
        End If
    End Sub

    Private Sub DragMe_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        If AutoSwapForm.SWAPMAPPER_ACTIVE And AutoSwapForm.SWAPMAPPERDRAG_LOCKED = False Then
            If DragMe_Down Then
                Dim currentPoint = CDRunningForm.GetMousePosition()
                Dim Xpos = currentPoint.X - Width / 2
                Dim Ypos = currentPoint.Y - Height / 2
                Dim newPoint As New Point(Xpos, Ypos)
                Location = newPoint
            End If
        End If
    End Sub

    Private Sub DragMe_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick
        If AutoSwapForm.SWAPMAPPER_ACTIVE And AutoSwapForm.SWAPMAPPERDRAG_LOCKED Then
            Hide()
            SetDragMePixel()
            Show()
            AutoSwapForm.lblCurrentMapper.Text = CURRENT_SWAP_MAPPER_ITEM.SMSToString()
            Debug.WriteLine(CURRENT_SWAP_MAPPER_ITEM.SMSToString())
        End If
    End Sub

    Private Sub SetDragMePixel()
        Dim currentPoint = CDRunningForm.GetMousePosition()
        Using bmp As New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.CopyFromScreen(currentPoint, New Point(0, 0), New Size(1, 1))
            End Using
            Dim Xpos = currentPoint.X - CURRENT_SWAP_MAPPER_ITEM.smsRect.Left
            Dim Ypos = currentPoint.Y - CURRENT_SWAP_MAPPER_ITEM.smsRect.Top
            Dim dragMePoint As New Point(Xpos, Ypos)
            CURRENT_SWAP_MAPPER_ITEM.smsPoints.Add(dragMePoint)
            CURRENT_SWAP_MAPPER_ITEM.smsColors.Add(bmp.GetPixel(0, 0))
        End Using
    End Sub

End Class