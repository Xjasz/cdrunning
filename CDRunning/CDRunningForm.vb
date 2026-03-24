Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class CDRunningForm

    Private Const KEYEVENT_DOWNKEY As UInteger = &H0 ' Keyboard down event
    Private Const KEYEVENT_UPKEY As UInteger = &H1 ' Keyboard up event
    Private Const KEYEVENT_UPKEY2 As UInteger = &H2 ' Keyboard up event
    Private Const KEYEVENT_SCANKEY As UInteger = &H8 ' Keyboard scan event
    Private Const MOUSE_RIGHT_KEY As Integer = 14 ' Mouse Left-key 
    Private Const KEYBOARD_J_KEY As Short = &H4A ' Keyboard J-key
    Private Const KEYBOARD_R_KEY As Short = &H52 ' Keyboard R-key
    Private Const KEYBOARD_Z_KEY As Short = &H2C ' Keyboard Z-key
    Private Const KEYBOARD_CAPS As Short = &H3A ' Caps Lock
    Private Const KEYBOARD_I_KEY As Integer = &H49
    Private Const KEYBOARD_U_KEY As Short = &H55
    Private Const KEYBOARD_SPACE_KEY As Short = &H39 ' Keyboard Space-key
    Private Const WH_MOUSE_LL As Integer = 14
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const MOUSEEVENTF_MOVE As UInteger = &H1
    Private Const INPUT_MOUSE As Integer = 0

    Public Shared AUTOFOCUSLOCKED As Boolean = False
    Public Shared LEFTCLICK_CLICKED As Boolean = False
    Public Shared RIGHTCLICK_DOWN As Boolean = False
    Public Shared LEFTCLICK_DOWN As Boolean = False
    Public Shared PINGDELAY As Long = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
    Public Shared CLICKPOSDELAY = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
    Public Shared RECOILSWAPDELAY = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
    Public Shared RESOURCE_DIR As String = "Resources\"
    Public Shared SETTINGS_FILE As String = "settings.txt"
    Public Shared DEFAULTKEY_FILE As String = "defaultkeys.txt"
    Public Shared POSITIONS_FILE As String = "positions.txt"
    Public Shared MIN_WHITE As Integer = 240
    Public Shared CURRENT_RECOIL_INDEX As Integer = -1
    Public Shared rightClickStartTime As DateTime
    Public Shared RAPID_FIRE_COUNT As Integer = 0

    Private Shared SimulatedClick As Boolean = False
    Private Shared hHook As IntPtr = IntPtr.Zero

    <DllImport("user32.dll", SetLastError:=True)>
    Friend Shared Function SendInput(ByVal cInputs As Int32, ByRef pInputs As INPUT, ByVal cbSize As Int32) As Int32
    End Function

    <DllImport("user32.dll")>
    Friend Shared Function GetMessageExtraInfo() As IntPtr
    End Function

    <StructLayout(LayoutKind.Explicit, Pack:=1, Size:=28)>
    Friend Structure INPUT
        <FieldOffset(0)>
        Public dwType As InputType
        <FieldOffset(4)>
        Public mi As MOUSEINPUT
        <FieldOffset(4)>
        Public ki As KEYBDINPUT
        <FieldOffset(4)>
        Public hi As HARDWAREINPUT
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Friend Structure MOUSEINPUT
        Public dx As Int32
        Public dy As Int32
        Public mouseData As Int32
        Public dwFlags As MOUSEEVENTF
        Public time As Int32
        Public dwExtraInfo As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Friend Structure KEYBDINPUT
        Public wVk As Int16
        Public wScan As Int16
        Public dwFlags As UInt32
        Public time As Int32
        Public dwExtraInfo As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Friend Structure HARDWAREINPUT
        Public uMsg As Int32
        Public wParamL As Int16
        Public wParamH As Int16
    End Structure
    <DllImport("user32.dll")>
    Public Shared Function MapVirtualKey(wCode As UInteger, wMapType As UInteger) As UInteger
    End Function

    Friend Enum InputType As Integer
        Mouse = 0
        Keyboard = 1
        Hardware = 2
    End Enum

    <Flags()>
    Friend Enum MOUSEEVENTF As Integer
        MOVE = &H1
        LEFTDOWN = &H2
        LEFTUP = &H4
        RIGHTDOWN = &H8
        RIGHTUP = &H10
        MIDDLEDOWN = &H20
        MIDDLEUP = &H40
        XDOWN = &H80
        XUP = &H100
        VIRTUALDESK = &H400
        WHEEL = &H800
        ABSOLUTE = &H8000
    End Enum
    <Flags()>
    Public Enum KEYEVENTF As Integer
        KEYDOWN = 0
        EXTENDEDKEY = 1
        KEYUP = 2
        UNICODE = 4
        SCANCODE = 8
    End Enum

    Public Shared Function GetMousePosition() As Point
        Return Cursor.Position
    End Function

    <DllImport("user32.dll")>
    Private Shared Function SetCursorPos(ByVal x As Int32, ByVal y As Int32) As Boolean
    End Function

    ' API calls for mouse and keyboard detection
    <DllImport("user32.dll")>
    Private Shared Function GetAsyncKeyState(ByVal vKey As Integer) As Short
    End Function

    ' API call to get current mouse position
    <DllImport("user32.dll")>
    Private Shared Function GetCursorPos(ByRef lpPoint As Point) As Boolean
    End Function

    Private Sub CDRunningForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists(Application.StartupPath & RESOURCE_DIR & SETTINGS_FILE) Then
            Using sw As New StreamReader(Application.StartupPath & RESOURCE_DIR & SETTINGS_FILE)
                Do
                    cmbPresets.Items.Add(sw.ReadLine)
                    cmbLoadouts.Items.Add(sw.ReadLine)
                Loop Until sw.EndOfStream
                sw.Close()
            End Using
        Else
            cmbPresets.Items.Add("Standard Key:NUMPAD0")
            cmbLoadouts.Items.Add("2:0:750;1:0:5000")
        End If
        If File.Exists(Application.StartupPath & RESOURCE_DIR & DEFAULTKEY_FILE) Then
            Using sw As New StreamReader(Application.StartupPath & RESOURCE_DIR & DEFAULTKEY_FILE)
                cmbStartKey.SelectedItem = sw.ReadLine
                cmbBRKey.SelectedItem = sw.ReadLine
                cmbShootKey.SelectedItem = sw.ReadLine
                cmbFocusKey.SelectedItem = sw.ReadLine
                cmbPingKey.SelectedItem = sw.ReadLine
            End Using
        Else
            cmbStartKey.Text = "F6"
            cmbBRKey.Text = "F5"
            cmbShootKey.Text = "U"
            cmbFocusKey.Text = "capslock"
            cmbPingKey.Text = "I"

        End If
        If File.Exists(Application.StartupPath & RESOURCE_DIR & POSITIONS_FILE) Then
            Dim propsFile As New FileInfo(Application.StartupPath & RESOURCE_DIR & POSITIONS_FILE)
            Dim sizeInBytes As Long = propsFile.Length
            If sizeInBytes > 10 Then
                Using sw As New StreamReader(Application.StartupPath & RESOURCE_DIR & POSITIONS_FILE)
                    Dim lineName, lineRect, linePoints, lineColors As String
                    lineName = sw.ReadLine
                    Do Until String.IsNullOrEmpty(lineName)
                        lineRect = sw.ReadLine
                        linePoints = sw.ReadLine
                        lineColors = sw.ReadLine
                        Dim smsItem As New SwapMapperClass()
                        smsItem.smsName = lineName
                        smsItem.smsRect = Globals.StringToRectangle(lineRect)
                        smsItem.smsPoints = Globals.StringToPoints(linePoints)
                        smsItem.smsColors = Globals.StringToColors(lineColors)
                        SWAP_MAPPER_ITEMS.Add(smsItem)
                        lineName = sw.ReadLine
                    Loop
                End Using
            End If
        End If

        SaveEverything()

        cmbPresets.SelectedIndex = 0
        tmrKeyBinds.Enabled = True
    End Sub

    Private Sub CDRunningForm_Layout(sender As Object, e As LayoutEventArgs) Handles MyBase.Layout
    End Sub

    Private Sub CDRunningForm_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        SaveEverything()
    End Sub
    Private Sub SaveEverything()
        If cmbPresets.Items.Count > 0 Then
            Using sw As New StreamWriter(Application.StartupPath & RESOURCE_DIR & SETTINGS_FILE)
                For x As Integer = 0 To cmbPresets.Items.Count - 1
                    sw.WriteLine(cmbPresets.Items.Item(x))
                    sw.WriteLine(cmbLoadouts.Items.Item(x))
                Next
                sw.Close()
            End Using
        End If
        Using sw As New StreamWriter(Application.StartupPath & RESOURCE_DIR & DEFAULTKEY_FILE)
            sw.WriteLine(cmbStartKey.SelectedItem)
            sw.WriteLine(cmbBRKey.SelectedItem)
            sw.WriteLine(cmbShootKey.SelectedItem)
            sw.WriteLine(cmbFocusKey.SelectedItem)
            sw.WriteLine(cmbPingKey.SelectedItem)
            sw.Close()
        End Using
        If SWAP_MAPPER_ITEMS.Count < 1 Then
            If File.Exists(Application.StartupPath & RESOURCE_DIR & POSITIONS_FILE) = True Then
                File.Delete(Application.StartupPath & RESOURCE_DIR & POSITIONS_FILE)
                Debug.WriteLine("Removed: " & Application.StartupPath & RESOURCE_DIR & POSITIONS_FILE)
            End If
        Else
            Using sw As New StreamWriter(Application.StartupPath & RESOURCE_DIR & POSITIONS_FILE)
                For Each item As SwapMapperClass In SWAP_MAPPER_ITEMS
                    Dim sPoints As String = ""
                    For Each p As Point In item.smsPoints
                        sPoints += p.ToString()
                    Next
                    Dim sColors As String = ""
                    For Each c As Color In item.smsColors
                        sColors += c.ToString()
                    Next
                    sw.WriteLine(item.smsName)
                    sw.WriteLine(item.smsRect.ToString())
                    sw.WriteLine(sPoints)
                    sw.WriteLine(sColors)
                Next
                sw.Close()
            End Using
        End If
    End Sub
    Private Sub StartStopProcess()
        cmbMappings.Items.Clear()
        For x As Integer = 0 To cmbPresets.Items.Count - 1
            If cmbPresets.Items.Item(x).ToString.Contains("Key:") Then
                Dim toBind As String = Split(cmbPresets.Items.Item(x), "Key:")(1)
                cmbMappings.Items.Add(x & "-" & toBind)
                Debug.WriteLine("KeyPressed: " & toBind)
            End If
        Next
        If lblStart.Text = "start" Then
            tmrStartStop.Interval = 1 ' Check every 100 milliseconds
            tmrStartStop.Start()
            lblStart.Text = "stop"
            Me.BackColor = Color.Green
        Else
            tmrStartStop.Stop()
            lblStart.Text = "start"
            Me.BackColor = Color.Red
        End If
    End Sub

    Private Sub TimerStartStop_Tick(sender As Object, e As EventArgs) Handles tmrStartStop.Tick
        CheckMouseButtons()
        'CheckKeyboardButtons()

        Dim isAutoRecoilEvent As Boolean = AutoRecoilCheck()

        Dim recoilSwap As Long = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
        If chkAutoRecoilSwap.Checked = True And RECOILSWAPDELAY <= recoilSwap Then
            RECOILSWAPDELAY = (DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond) + 500
            RecoilSwapEvent2()
        End If

        Dim clickPosTime As Long = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
        If chkClickPosition.Checked And LEFTCLICK_DOWN And CLICKPOSDELAY <= clickPosTime Then
            Dim locationColor = ReturnPixelColor()
            Debug.WriteLine(locationColor)
            CLICKPOSDELAY = (DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond) + 2000
        End If

        Dim pingTime As Long = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
        If LEFTCLICK_DOWN And RIGHTCLICK_DOWN And chkAutoPing.Checked And PINGDELAY <= pingTime Then
            PINGDELAY = (Date.Now.Ticks \ TimeSpan.TicksPerMillisecond) + 2000
            PingPress(cmbPingKey.Text)
        End If

        If chkAutoFocus.Checked And RIGHTCLICK_DOWN Then
            If AUTOFOCUSLOCKED = False Then
                FocusKeyPress(cmbFocusKey.Text)
                rightClickStartTime = DateTime.Now
                AUTOFOCUSLOCKED = True
            ElseIf AUTOFOCUSLOCKED = True Then
                If DateTime.Now.Subtract(rightClickStartTime).TotalMilliseconds >= 2500 Then
                    FocusKeyPress(cmbFocusKey.Text)
                    rightClickStartTime = DateTime.Now.AddDays(1)
                End If
            End If
        End If


        If LEFTCLICK_DOWN And RIGHTCLICK_DOWN Then
            'MouseMoveNew()
            RAPID_FIRE_COUNT = 0
            lblStatus2.Text = "RFC: " + RAPID_FIRE_COUNT.ToString()

            Dim movements = txtStages.Text.Split(";")
            Dim x As Integer = 0
            lblStatus.Text = "Starting Stage"
            lblStatus.Refresh()
            For Each movement In movements
                If Not String.IsNullOrEmpty(movement) Then
                    x += 1
                    Dim innerPart = movement.Split(":")
                    lblStatus.Text = "Stage: " & x
                    lblStatus.Refresh()
                    If innerPart.Length > 1 Then
                        ' X:Y:MiliSeconds
                        Dim strengthY As Integer = Integer.Parse(innerPart(0))
                        Dim strengthX As Integer = Integer.Parse(innerPart(1))
                        Dim strengthTime As Integer = Integer.Parse(innerPart(2))

                        Dim currentTime As Long = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
                        Dim stopTime As Long = currentTime + strengthTime

                        If movements.Length = x - 1 Then
                            stopTime = currentTime + 6000
                        End If

                        While currentTime <= stopTime
                            CheckMouseButtons()  ' Check the button state again
                            If Not (LEFTCLICK_DOWN And RIGHTCLICK_DOWN) Then
                                lblStatus.Text = "Stopped Pressing"
                                Return
                            End If

                            If AutoRecoilCheck() Then
                                Return
                            End If

                            If cmbPresets.Items.Item(x).ToString.Contains("__") Then
                                cmbPresets.SelectedIndex = x
                                Debug.WriteLine("Set preset to: " & cmbPresets.Items.Item(x))
                                CURRENT_RECOIL_INDEX = -1
                                x = cmbPresets.Items.Count
                            End If


                            Debug.WriteLine("currentTime:" + currentTime.ToString + " :: " + "stopTime:" + stopTime.ToString)

                            'CURRENT_POINT = GetMousePosition()
                            'CURRENT_POINT.X += strengthX
                            'CURRENT_POINT.Y += strengthY
                            'DoMouse(MOUSEEVENTF.MOVE Or MOUSEEVENTF.ABSOLUTE, CURRENT_POINT)

                            Debug.WriteLine("Horizontal: " & strengthX)
                            Debug.WriteLine("Vertical: " & strengthY)

                            NewMouse(strengthX, strengthY)

                            If chkBR.Checked Then
                                'SimulateKeyPress(cmbShootKey.Text)
                                SendLeftClick()
                                Thread.Sleep(40)
                            Else
                                Thread.Sleep(25)
                            End If

                            currentTime = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
                        End While
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub tmrKeyBinds_Tick(sender As Object, e As EventArgs) Handles tmrKeyBinds.Tick
        If GetAsyncKeyState(StringToVirtualKey(cmbStartKey.Text)) Then
            StartStopProcess()
        End If
        If GetAsyncKeyState(StringToVirtualKey(cmbBRKey.Text)) Then
            chkBR.Checked = Not chkBR.Checked
        End If

        For x As Integer = 0 To cmbMappings.Items.Count - 1
            Dim virtualKeyCode As Integer = StringToVirtualKey(Split(cmbMappings.Items.Item(x), "-")(1))
            If virtualKeyCode <> 0 AndAlso (GetAsyncKeyState(virtualKeyCode) And &H8000) <> 0 Then
                cmbPresets.SelectedIndex = Split(cmbMappings.Items.Item(x), "-")(0)
            End If
        Next
    End Sub

    Private Sub HardwareLevelMouseMovement(ByVal dx As Integer, ByVal dy As Integer)
        Dim currentPoint As New Point()
        GetCursorPos(currentPoint)
        currentPoint.X += dx
        currentPoint.Y += dy
        DoMouse(MOUSEEVENTF.MOVE Or MOUSEEVENTF.ABSOLUTE, currentPoint)
    End Sub

    Private Function AutoRecoilCheck()
        Dim recoilSwap As Long = DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond
        If chkAutoRecoilSwap.Checked = True And RECOILSWAPDELAY <= recoilSwap Then
            RECOILSWAPDELAY = (DateTime.Now.Ticks \ TimeSpan.TicksPerMillisecond) + 500
            RecoilSwapEvent2()
            Return True
        End If
        Return False
    End Function

    Public Sub RecoilSwapEvent2()
        Dim activeSMSObject As SwapMapperClass = Nothing
        Dim skipCheck As Boolean = False
        If CURRENT_RECOIL_INDEX <> -1 Then
            activeSMSObject = SWAP_MAPPER_ITEMS(CURRENT_RECOIL_INDEX)
            Using bmp As New Bitmap(10, 1)
                activeSMSObject = CheckSwapMapperItem(activeSMSObject, bmp)
                If IsNothing(activeSMSObject) = False Then
                    skipCheck = True
                End If
            End Using
        End If
        If skipCheck = False Then
            If SWAP_MAPPER_ITEMS.Count > 0 Then
                Dim totalSwapMapperCount As Integer = SWAP_MAPPER_ITEMS.Count
                Using bmp As New Bitmap(10, 1)
                    For currentSMSIndex As Integer = 0 To totalSwapMapperCount - 1
                        Dim smsItem As SwapMapperClass = SWAP_MAPPER_ITEMS(currentSMSIndex)
                        activeSMSObject = CheckSwapMapperItem(smsItem, bmp)
                        If IsNothing(activeSMSObject) = False Then
                            CURRENT_RECOIL_INDEX = currentSMSIndex
                            currentSMSIndex = totalSwapMapperCount
                        End If
                    Next
                End Using
            End If
            If IsNothing(activeSMSObject) = False Then
                For x As Integer = 0 To cmbPresets.Items.Count - 1
                    If cmbPresets.Items.Item(x).ToString = activeSMSObject.smsName Then
                        cmbPresets.SelectedIndex = x
                        Debug.WriteLine("Set preset to: " & activeSMSObject.smsName)
                        x = cmbPresets.Items.Count
                    End If
                Next
            Else
                For x As Integer = 0 To cmbPresets.Items.Count - 1
                    If cmbPresets.Items.Item(x).ToString.Contains("__") Then
                        cmbPresets.SelectedIndex = x
                        Debug.WriteLine("Set preset to: " & cmbPresets.Items.Item(x))
                        CURRENT_RECOIL_INDEX = -1
                        x = cmbPresets.Items.Count
                    End If
                Next
            End If
        End If
    End Sub

    Public Function CheckSwapMapperItem(smsItem As SwapMapperClass, bmp As Bitmap) As SwapMapperClass
        Dim smsFound As Boolean = True
        Using g As Graphics = Graphics.FromImage(bmp)
            Dim totalCount As Integer = smsItem.smsPoints.Count
            For index As Integer = 0 To totalCount - 1
                Dim sRect = smsItem.smsRect
                Dim sPoint As Point = smsItem.smsPoints(index)
                Dim sColor As Color = smsItem.smsColors(index)
                g.CopyFromScreen(New Point(sRect.Left + sPoint.X, sRect.Top + sPoint.Y), New Point(0, 0), New Size(1, 1))
                Dim screenPixCol As Color = bmp.GetPixel(0, 0)
                Debug.WriteLine(smsItem.smsName & " sPoint=" + sPoint.ToString & " sColor=" + sColor.ToString + " screenPixCol=" + screenPixCol.ToString)
                If screenPixCol.R <> sColor.R And screenPixCol.G <> sColor.G And screenPixCol.B <> sColor.B Then
                    Debug.WriteLine("Invalid SMS Object")
                    smsFound = False
                    index = totalCount
                End If
            Next
            If smsFound Then
                Debug.WriteLine("SMS Object Found")
                Return smsItem
            End If
        End Using
        Return Nothing
    End Function


    Public Sub RecoilSwapEvent()
        Using bmp As New Bitmap(3, 6)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.CopyFromScreen(New Point(1581, 988), New Point(0, 0), New Size(1, 1))
                g.CopyFromScreen(New Point(1640, 999), New Point(1, 0), New Size(1, 1))
                g.CopyFromScreen(New Point(1629, 974), New Point(2, 0), New Size(1, 1))
                g.CopyFromScreen(New Point(1571, 974), New Point(0, 1), New Size(1, 1))
                g.CopyFromScreen(New Point(1593, 1001), New Point(1, 1), New Size(1, 1))
                g.CopyFromScreen(New Point(1626, 973), New Point(2, 1), New Size(1, 1))
                g.CopyFromScreen(New Point(1518, 921), New Point(0, 2), New Size(1, 1))
                g.CopyFromScreen(New Point(1541, 912), New Point(1, 2), New Size(1, 1))
                g.CopyFromScreen(New Point(1483, 912), New Point(2, 2), New Size(1, 1))
                g.CopyFromScreen(New Point(1447, 913), New Point(0, 3), New Size(1, 1))
                g.CopyFromScreen(New Point(1497, 914), New Point(1, 3), New Size(1, 1))
                g.CopyFromScreen(New Point(1466, 943), New Point(2, 3), New Size(1, 1))

                g.CopyFromScreen(New Point(1543, 966), New Point(0, 4), New Size(1, 1))
                g.CopyFromScreen(New Point(1598, 966), New Point(1, 4), New Size(1, 1))
                g.CopyFromScreen(New Point(1561, 997), New Point(2, 4), New Size(1, 1))

                g.CopyFromScreen(New Point(1574, 969), New Point(0, 5), New Size(1, 1))
                g.CopyFromScreen(New Point(1639, 966), New Point(1, 5), New Size(1, 1))
                g.CopyFromScreen(New Point(1615, 977), New Point(2, 5), New Size(1, 1))
                '1574,969 1639,966, 1615, 977
                '1543,966 1598,966 1561,997
            End Using
            Dim checkColor1 = bmp.GetPixel(0, 0)
            Dim checkColor2 = bmp.GetPixel(1, 0)
            Dim checkColor3 = bmp.GetPixel(2, 0)
            Dim checkColor4 = bmp.GetPixel(0, 1)
            Dim checkColor5 = bmp.GetPixel(1, 1)
            Dim checkColor6 = bmp.GetPixel(2, 1)
            Dim checkColor7 = bmp.GetPixel(0, 2)
            Dim checkColor8 = bmp.GetPixel(1, 2)
            Dim checkColor9 = bmp.GetPixel(2, 2)
            Dim checkColor10 = bmp.GetPixel(0, 3)
            Dim checkColor11 = bmp.GetPixel(1, 3)
            Dim checkColor12 = bmp.GetPixel(2, 3)


            Dim checkColor13 = bmp.GetPixel(0, 4)
            Dim checkColor14 = bmp.GetPixel(1, 4)
            Dim checkColor15 = bmp.GetPixel(2, 4)
            Dim checkColor16 = bmp.GetPixel(0, 5)
            Dim checkColor17 = bmp.GetPixel(1, 5)
            Dim checkColor18 = bmp.GetPixel(2, 5)

            If checkColor1.R > MIN_WHITE And checkColor1.G > MIN_WHITE And checkColor1.B > MIN_WHITE And checkColor2.R > MIN_WHITE And checkColor2.G > MIN_WHITE And checkColor2.B > MIN_WHITE And checkColor3.R > MIN_WHITE And checkColor3.G > MIN_WHITE And checkColor3.B > MIN_WHITE Then
                If CURRENT_RECOIL_INDEX <> 0 Then
                    CURRENT_RECOIL_INDEX = 0
                    cmbPresets.SelectedIndex = 1
                End If
            ElseIf checkColor7.R > MIN_WHITE And checkColor7.G > MIN_WHITE And checkColor7.B > MIN_WHITE And checkColor8.R > MIN_WHITE And checkColor8.G > MIN_WHITE And checkColor8.B > MIN_WHITE And checkColor9.R > MIN_WHITE And checkColor9.G > MIN_WHITE And checkColor9.B > MIN_WHITE Then
                If CURRENT_RECOIL_INDEX <> 0 Then
                    CURRENT_RECOIL_INDEX = 0
                    cmbPresets.SelectedIndex = 1
                End If
            ElseIf checkColor13.R > MIN_WHITE And checkColor13.G > MIN_WHITE And checkColor13.B > MIN_WHITE And checkColor14.R > MIN_WHITE And checkColor14.G > MIN_WHITE And checkColor14.B > MIN_WHITE And checkColor15.R > MIN_WHITE And checkColor15.G > MIN_WHITE And checkColor15.B > MIN_WHITE Then
                If CURRENT_RECOIL_INDEX <> 0 Then
                    CURRENT_RECOIL_INDEX = 0
                    cmbPresets.SelectedIndex = 1
                End If
            ElseIf checkColor4.R > MIN_WHITE And checkColor4.G > MIN_WHITE And checkColor4.B > MIN_WHITE And checkColor5.R > MIN_WHITE And checkColor5.G > MIN_WHITE And checkColor5.B > MIN_WHITE And checkColor6.R > MIN_WHITE And checkColor6.G > MIN_WHITE And checkColor6.B > MIN_WHITE Then
                If CURRENT_RECOIL_INDEX <> 1 Then
                    CURRENT_RECOIL_INDEX = 1
                    cmbPresets.SelectedIndex = 2
                End If
            ElseIf checkColor10.R > MIN_WHITE And checkColor10.G > MIN_WHITE And checkColor10.B > MIN_WHITE And checkColor11.R > MIN_WHITE And checkColor11.G > MIN_WHITE And checkColor11.B > MIN_WHITE And checkColor12.R > MIN_WHITE And checkColor12.G > MIN_WHITE And checkColor12.B > MIN_WHITE Then
                If CURRENT_RECOIL_INDEX <> 1 Then
                    CURRENT_RECOIL_INDEX = 1
                    cmbPresets.SelectedIndex = 2
                End If
            ElseIf checkColor10.R > MIN_WHITE And checkColor16.G > MIN_WHITE And checkColor16.B > MIN_WHITE And checkColor16.R > MIN_WHITE And checkColor17.G > MIN_WHITE And checkColor17.B > MIN_WHITE And checkColor18.R > MIN_WHITE And checkColor18.G > MIN_WHITE And checkColor18.B > MIN_WHITE Then
                If CURRENT_RECOIL_INDEX <> 1 Then
                    CURRENT_RECOIL_INDEX = 1
                    cmbPresets.SelectedIndex = 2
                End If
            ElseIf CURRENT_RECOIL_INDEX <> 2 Then
                CURRENT_RECOIL_INDEX = 2
                cmbPresets.SelectedIndex = 0
            End If
        End Using
    End Sub

    Public Function ReturnPixelColor() As String
        Dim theLocation As String = ""
        Dim theColor As String = ""
        Using bmp As New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.CopyFromScreen(Windows.Forms.Cursor.Position, New Point(0, 0), New Size(1, 1))
            End Using

            theColor = bmp.GetPixel(0, 0).ToString

            Dim currentPoint As New Point()
            GetCursorPos(currentPoint)
            theLocation = currentPoint.X & ", " & currentPoint.Y
        End Using
        Return theColor & ":" & theLocation
    End Function

    Private Shared Sub NewMouse(ByVal dx As Integer, ByVal dy As Integer)
        Dim input As New INPUT()
        Dim mi As New MOUSEINPUT()

        input.dwType = InputType.Mouse
        input.mi = mi
        input.mi.dwExtraInfo = IntPtr.Zero

        ' Set relative movement
        input.mi.dx = dx  ' 1 pixel right
        input.mi.dy = dy  ' 3 pixels down

        Dim formatedText As String = String.Format("dx: {0} , dy: {1} ", input.mi.dx.ToString(), input.mi.dy.ToString())
        Debug.WriteLine(formatedText)

        input.mi.time = 0
        input.mi.dwFlags = MOUSEEVENTF.MOVE  ' Indicate this is a move event

        Dim cbSize As Integer = Marshal.SizeOf(GetType(INPUT))
        SendInput(1, input, cbSize)
    End Sub

    Private Shared Sub DoMouse(flags As MOUSEEVENTF, newPoint As Point, Optional scrollSize As Integer = 0)
        Dim input As New INPUT()
        Dim mi As New MOUSEINPUT()

        input.dwType = InputType.Mouse
        input.mi = mi
        input.mi.dwExtraInfo = IntPtr.Zero

        ' Set relative movement
        input.mi.dx = newPoint.X  ' 1 pixel right
        input.mi.dy = newPoint.Y  ' 3 pixels down

        Dim formatedText As String = String.Format("dx: {0} , dy: {1} ", input.mi.dx.ToString(), input.mi.dy.ToString())
        Debug.WriteLine(formatedText)

        input.mi.time = 0
        input.mi.dwFlags = MOUSEEVENTF.MOVE  ' Indicate this is a move event

        Dim cbSize As Integer = Marshal.SizeOf(GetType(INPUT))
        SendInput(1, input, cbSize)
    End Sub

    Private Sub CheckMouseButtons()
        If Not SimulatedClick AndAlso GetAsyncKeyState(Keys.LButton) Then
            LEFTCLICK_DOWN = True
        Else
            LEFTCLICK_DOWN = False
        End If


        If GetAsyncKeyState(Keys.RButton) Then
            RIGHTCLICK_DOWN = True
        Else
            RIGHTCLICK_DOWN = False
            AUTOFOCUSLOCKED = False
        End If
    End Sub

    Private Sub SendLeftClick()
        SimulatedClick = True
        Dim input1 As New INPUT()
        input1.dwType = InputType.Mouse
        input1.mi.dwExtraInfo = IntPtr.Zero
        input1.mi.dwFlags = MOUSEEVENTF.LEFTDOWN
        SendInput(1, input1, Marshal.SizeOf(GetType(INPUT)))

        Thread.Sleep(30)

        Dim input2 As New INPUT()
        input2.dwType = InputType.Mouse
        input2.mi.dwExtraInfo = IntPtr.Zero
        input2.mi.dwFlags = MOUSEEVENTF.LEFTUP
        SendInput(1, input2, Marshal.SizeOf(GetType(INPUT)))
        SimulatedClick = False

        RAPID_FIRE_COUNT = RAPID_FIRE_COUNT + 1
        lblStatus2.Text = "RFC: " + RAPID_FIRE_COUNT.ToString()
    End Sub

    Public Shared Sub FocusKeyPress(key As String)
        Dim input As New INPUT()
        Dim ki As New KEYBDINPUT()
        input.dwType = InputType.Keyboard
        input.ki = ki
        input.ki.dwExtraInfo = GetMessageExtraInfo()
        input.ki.wVk = 0
        Dim wsKey = Globals.StringToScanCode(key)
        input.ki.wScan = wsKey

        Thread.Sleep(100)
        input.ki.dwFlags = KEYEVENT_DOWNKEY Or KEYEVENT_SCANKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY2
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Public Shared Sub PingPress(key As String)
        Dim input As New INPUT()
        Dim ki As New KEYBDINPUT()

        input.dwType = InputType.Keyboard
        input.ki = ki
        input.ki.dwExtraInfo = GetMessageExtraInfo()
        Dim wsKey = Globals.StringToScanCode(key)
        input.ki.wVk = 0
        input.ki.wScan = wsKey

        input.ki.dwFlags = KEYEVENT_DOWNKEY Or KEYEVENT_SCANKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY2
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Public Shared Sub RecoilPressEvent(key As String)
        Dim input As New INPUT()
        Dim ki As New KEYBDINPUT()

        input.dwType = InputType.Keyboard
        input.ki = ki
        input.ki.dwExtraInfo = GetMessageExtraInfo()
        Dim wsKey = Globals.StringToScanCode(key)
        input.ki.wVk = 0
        input.ki.wScan = wsKey

        input.ki.dwFlags = KEYEVENT_DOWNKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY2
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Public Shared Sub SimulateKeyPress(key As String)
        Dim input As New INPUT()
        Dim ki As New KEYBDINPUT()

        input.dwType = InputType.Keyboard
        input.ki = ki
        input.ki.dwExtraInfo = GetMessageExtraInfo()
        Dim wsKey = Globals.StringToScanCode(key)
        input.ki.wVk = 0
        input.ki.wScan = wsKey

        input.ki.dwFlags = KEYEVENT_DOWNKEY Or KEYEVENT_SCANKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY2
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
    End Sub


    Public Shared Sub SimulateKeyPressTesting(key As String)
        Dim input As New INPUT()
        Dim ki As New KEYBDINPUT()

        input.dwType = InputType.Keyboard
        input.ki = ki
        input.ki.dwExtraInfo = GetMessageExtraInfo()
        Dim wsKey = Globals.StringToScanCode(key)
        input.ki.wVk = 0
        input.ki.wScan = wsKey

        input.ki.dwFlags = KEYEVENT_DOWNKEY Or KEYEVENT_SCANKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
        Thread.Sleep(20)
        input.ki.dwFlags = KEYEVENT_UPKEY2
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
    End Sub


    Public Sub SimulateKeyPressDown(key As String)
        Dim input As New INPUT()

        ' Get the virtual key code from the string key
        Dim vkCode As Integer = StringToVirtualKey(key)

        ' Set up the INPUT for the key down event
        input.dwType = InputType.Keyboard
        input.ki.wVk = CShort(vkCode)
        input.ki.dwFlags = 0 ' 0 for key down
        input.ki.dwFlags = KEYEVENT_DOWNKEY Or KEYEVENT_SCANKEY
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Public Sub SimulateKeyPressUp(key As String)
        Dim input As New INPUT()

        ' Get the virtual key code from the string key
        Dim vkCode As Integer = StringToVirtualKey(key)
        ' Set up the INPUT for the key up event
        input.ki.dwFlags = KEYEVENTF.KEYUP
        SendInput(1, input, Marshal.SizeOf(GetType(INPUT)))
    End Sub

    Private Sub lblStart_Click(sender As Object, e As EventArgs) Handles lblStart.Click
        StartStopProcess()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnTesty.Click
        DoMouse(MOUSEEVENTF.MOVE, New Point() With {.X = 200, .Y = 200})
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPresets.SelectedIndexChanged
        If cmbPresets.Text <> "" Then
            txtStages.Text = cmbLoadouts.Items.Item(cmbPresets.SelectedIndex)
            If cmbPresets.Text.ToString.Contains("_BR_") Then
                chkBR.Checked = True
            Else
                chkBR.Checked = False
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim response As String = InputBox("Enter name of new item", "Enter Item", "")
        If response <> "" Then
            cmbPresets.Items.Add(response)
        End If
        SaveEverything()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If cmbPresets.SelectedIndex < 0 Then
            MsgBox("First select item to remove", MsgBoxStyle.Critical)
        Else
            Dim response As String = MsgBox("Are you sure you want to delete?", MsgBoxStyle.YesNo)
            If response = vbYes Then
                AutoSwapForm.RemoveSwapItem(cmbPresets.Items.Item(cmbPresets.SelectedIndex))
                cmbPresets.Items.RemoveAt(cmbPresets.SelectedIndex)
                SaveEverything()
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim response As String = MsgBox("Are you sure you want to override current loadout?", MsgBoxStyle.YesNo)
        If response = vbYes Then
            cmbLoadouts.Items.Item(cmbPresets.SelectedIndex) = txtStages.Text
            SaveEverything()

        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim response As String = InputBox("Type what you want to rename this to", "Rename Item", cmbPresets.Items.Item(cmbPresets.SelectedIndex))
        If response <> "" Then
            For Each item As SwapMapperClass In SWAP_MAPPER_ITEMS
                If item.smsName = cmbPresets.Items.Item(cmbPresets.SelectedIndex) Then
                    item.smsName = response
                End If
            Next

            cmbPresets.Items.Item(cmbPresets.SelectedIndex) = response
            SaveEverything()
        End If

    End Sub

    Private Sub btnSaveDrag_Click(sender As Object, e As EventArgs) Handles btnSaveDrag.Click
        Dim dragMeTopLeft = DragMe.Location
        Dim dragMeSize = DragMe.Size
        SaveDragMeImage(dragMeTopLeft, dragMeSize)
    End Sub

    Public Sub SaveDragMeImage(dragMeTopLeft As Point, dragMeSize As Size)
        DragMe.Hide()
        Using bmp As New Bitmap(dragMeSize.Width, dragMeSize.Height)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.CopyFromScreen(dragMeTopLeft, New Point(0, 0), dragMeSize)
            End Using
            Dim saveName As String = cmbPresets.Text.Replace(":", "_")
            Dim bmpSavePath As String = Application.StartupPath & RESOURCE_DIR & saveName
            bmp.Save(bmpSavePath & ".bmp", Imaging.ImageFormat.Bmp)
        End Using
        DragMe.Show()
    End Sub

    Private Sub btnOpenSwapMapper_Click(sender As Object, e As EventArgs) Handles btnOpenSwapMapper.Click
        Hide()
        AutoSwapForm.Show()
    End Sub

End Class