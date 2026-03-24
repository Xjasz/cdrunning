Imports System.Text.RegularExpressions

Public Module Globals
    Public cmbLoadouts As New ComboBox
    Public cmbMappings As New ComboBox

    Public SWAP_MAPPER_ITEMS As New List(Of SwapMapperClass)
    Public CURRENT_SWAP_MAPPER_ITEM As New SwapMapperClass()

    Public Class SwapMapperClass
        Public smsName As String
        Public smsRect As Rectangle
        Public smsPoints As New List(Of Point)
        Public smsColors As New List(Of Color)

        Public Function SMSToString() As String
            Dim sName As String = ""
            Dim sRect As String = ""
            Dim sPoints As String = ""
            Dim sColors As String = ""
            If smsName <> Nothing Then
                sName = smsName.ToString()
            End If
            If smsRect <> Nothing Then
                sRect = smsRect.ToString()
            End If
            For Each p As Point In smsPoints
                sPoints += p.ToString()
            Next
            For Each c As Color In smsColors
                sColors += c.ToString()
            Next
            Dim fullString As String = sName + "::" + sRect + "::" + sPoints + "::" + sColors
            Return fullString
        End Function
    End Class

    Public Function StringToRectangle(input As String) As Rectangle
        Dim match As Match = Regex.Match(input, "{X=(?<x>\d+),Y=(?<y>\d+),Width=(?<width>\d+),Height=(?<height>\d+)}")

        If match.Success Then
            Dim x As Integer = Integer.Parse(match.Groups("x").Value)
            Dim y As Integer = Integer.Parse(match.Groups("y").Value)
            Dim width As Integer = Integer.Parse(match.Groups("width").Value)
            Dim height As Integer = Integer.Parse(match.Groups("height").Value)

            Return New Rectangle(x, y, width, height)
        Else
            Throw New FormatException("Input string is not in the correct format.")
        End If
    End Function

    Public Function StringToPoints(input As String) As List(Of Point)
        Dim pattern As String = "{X=(?<x>\d+),Y=(?<y>\d+)}"
        Dim matches As MatchCollection = Regex.Matches(input, pattern)

        Dim points As New List(Of Point)()

        For Each match As Match In matches
            Dim x As Integer = Integer.Parse(match.Groups("x").Value)
            Dim y As Integer = Integer.Parse(match.Groups("y").Value)
            points.Add(New Point(x, y))
        Next

        Return points
    End Function

    Public Function StringToColors(input As String) As List(Of Color)
        Dim pattern As String = "Color \[A=(?<a>\d+), R=(?<r>\d+), G=(?<g>\d+), B=(?<b>\d+)\]"
        Dim matches As MatchCollection = Regex.Matches(input, pattern)

        Dim colors As New List(Of Color)()

        For Each match As Match In matches
            Dim a As Integer = Integer.Parse(match.Groups("a").Value)
            Dim r As Integer = Integer.Parse(match.Groups("r").Value)
            Dim g As Integer = Integer.Parse(match.Groups("g").Value)
            Dim b As Integer = Integer.Parse(match.Groups("b").Value)

            colors.Add(Color.FromArgb(a, r, g, b))
        Next

        Return colors
    End Function

    Public Function StringToScanCode(keyString As String) As Byte
        Select Case keyString.ToLower()
        ' Alphabet
            Case "a"
                Return &H1E
            Case "b"
                Return &H30
            Case "c"
                Return &H2E
            Case "d"
                Return &H20
            Case "e"
                Return &H12
            Case "f"
                Return &H21
            Case "g"
                Return &H22
            Case "h"
                Return &H23
            Case "i"
                Return &H17
            Case "j"
                Return &H24
            Case "k"
                Return &H25
            Case "l"
                Return &H26
            Case "m"
                Return &H32
            Case "n"
                Return &H31
            Case "o"
                Return &H18
            Case "p"
                Return &H19
            Case "q"
                Return &H10
            Case "r"
                Return &H13
            Case "s"
                Return &H1F
            Case "t"
                Return &H14
            Case "u"
                Return &H16
            Case "v"
                Return &H2F
            Case "w"
                Return &H11
            Case "x"
                Return &H2D
            Case "y"
                Return &H15
            Case "z"
                Return &H2C

        ' Numbers
            Case "0"
                Return &HB
            Case "1"
                Return &H2
            Case "2"
                Return &H3
            Case "3"
                Return &H4
            Case "4"
                Return &H5
            Case "5"
                Return &H6
            Case "6"
                Return &H7
            Case "7"
                Return &H8
            Case "8"
                Return &H9
            Case "9"
                Return &HA

        ' Special keys
            Case "capslock"
                Return &H3A
            Case "leftshift"
                Return &H2A

        ' Numpad keys
            Case "numpad0"
                Return &H52
            Case "numpad1"
                Return &H4F
            Case "numpad2"
                Return &H50
            Case "numpad3"
                Return &H51
            Case "numpad4"
                Return &H4B
            Case "numpad5"
                Return &H4C
            Case "numpad6"
                Return &H4D
            Case "numpad7"
                Return &H47
            Case "numpad8"
                Return &H48
            Case "numpad9"
                Return &H49
            Case "numpad*"
                Return &H37
            Case "numpad+"
                Return &H4E
            Case "numpad-"
                Return &H4A
            Case "numpad."
                Return &H53
            Case "numpad/"
                Return &H35

            Case Else
                Throw New Exception("Invalid key string")
        End Select
    End Function

    Public Function StringToVirtualKey(key As String) As Integer
        Select Case key.ToUpper()
            Case "A"
                Return &H41 ' Virtual-Key code for A
            Case "B"
                Return &H42 ' Virtual-Key code for B
            Case "C"
                Return &H43 ' Virtual-Key code for C
            Case "D"
                Return &H44 ' Virtual-Key code for D
            Case "E"
                Return &H45 ' Virtual-Key code for E
            Case "F"
                Return &H46 ' Virtual-Key code for F
            Case "G"
                Return &H47 ' Virtual-Key code for G
            Case "H"
                Return &H48 ' Virtual-Key code for H
            Case "I"
                Return &H49 ' Virtual-Key code for I
            Case "J"
                Return &H4A ' Virtual-Key code for J
            Case "K"
                Return &H4B ' Virtual-Key code for K
            Case "L"
                Return &H4C ' Virtual-Key code for L
            Case "M"
                Return &H4D ' Virtual-Key code for M
            Case "N"
                Return &H4E ' Virtual-Key code for N
            Case "O"
                Return &H4F ' Virtual-Key code for O
            Case "P"
                Return &H50 ' Virtual-Key code for P
            Case "Q"
                Return &H51 ' Virtual-Key code for Q
            Case "R"
                Return &H52 ' Virtual-Key code for R
            Case "S"
                Return &H53 ' Virtual-Key code for S
            Case "T"
                Return &H54 ' Virtual-Key code for T
            Case "U"
                Return &H55 ' Virtual-Key code for U
            Case "V"
                Return &H56 ' Virtual-Key code for V
            Case "W"
                Return &H57 ' Virtual-Key code for W
            Case "X"
                Return &H58 ' Virtual-Key code for X
            Case "Y"
                Return &H59 ' Virtual-Key code for Y
            Case "Z"
                Return &H5A ' Virtual-Key code for Z
            Case "0"
                Return &H30 ' Virtual-Key code for 0
            Case "1"
                Return &H31 ' Virtual-Key code for 1
            Case "2"
                Return &H32 ' Virtual-Key code for 2
            Case "3"
                Return &H33 ' Virtual-Key code for 3
            Case "4"
                Return &H34 ' Virtual-Key code for 4
            Case "5"
                Return &H35 ' Virtual-Key code for 5
            Case "6"
                Return &H36 ' Virtual-Key code for 6
            Case "7"
                Return &H37 ' Virtual-Key code for 7
            Case "8"
                Return &H38 ' Virtual-Key code for 8
            Case "9"
                Return &H39 ' Virtual-Key code for 9
            Case "F1"
                Return &H70 ' Virtual-Key code for F1
            Case "F2"
                Return &H71 ' Virtual-Key code for F2
            Case "F3"
                Return &H72 ' Virtual-Key code for F3
            Case "F4"
                Return &H73 ' Virtual-Key code for F4
            Case "F5"
                Return &H74 ' Virtual-Key code for F5
            Case "F6"
                Return &H75 ' Virtual-Key code for F6
            Case "F7"
                Return &H76 ' Virtual-Key code for F7
            Case "F8"
                Return &H77 ' Virtual-Key code for F8
            Case "F9"
                Return &H78 ' Virtual-Key code for F9
            Case "F10"
                Return &H79 ' Virtual-Key code for F10
            Case "F11"
                Return &H7A ' Virtual-Key code for F11
            Case "F12"
                Return &H7B ' Virtual-Key code for F12
            Case "NUMPAD0"
                Return &H60 ' Virtual-Key code for NUMPAD 0
            Case "NUMPAD1"
                Return &H61 ' Virtual-Key code for NUMPAD 1
            Case "NUMPAD2"
                Return &H62 ' Virtual-Key code for NUMPAD 2
            Case "NUMPAD3"
                Return &H63 ' Virtual-Key code for NUMPAD 3
            Case "NUMPAD4"
                Return &H64 ' Virtual-Key code for NUMPAD 4
            Case "NUMPAD5"
                Return &H65 ' Virtual-Key code for NUMPAD 5
            Case "NUMPAD6"
                Return &H66 ' Virtual-Key code for NUMPAD 6
            Case "NUMPAD7"
                Return &H67 ' Virtual-Key code for NUMPAD 7
            Case "NUMPAD8"
                Return &H68 ' Virtual-Key code for NUMPAD 8
            Case "NUMPAD9"
                Return &H69 ' Virtual-Key code for NUMPAD 9
            Case "MULTIPLY"
                Return &H6A ' Virtual-Key code for MULTIPLY (* on NUMPAD)
            Case "ADD"
                Return &H6B ' Virtual-Key code for ADD (+ on NUMPAD)
            Case "SEPARATOR"
                Return &H6C ' Virtual-Key code for ENTER (only available on some keyboards)
            Case "SUBTRACT"
                Return &H6D ' Virtual-Key code for SUBTRACT (- on NUMPAD)
            Case "DECIMAL"
                Return &H6E ' Virtual-Key code for DECIMAL (. on NUMPAD)
            Case "DIVIDE"
                Return &H6F ' Virtual-Key code for DIVIDE (/ on NUMPAD)
            Case Else
                Return 0 ' Default value (no key)
        End Select
    End Function
End Module