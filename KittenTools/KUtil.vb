''' <summary>
''' Shared utility functions.
''' <author>Kitten</author>
''' <created>2023-07-09</created>
''' </summary>
Public Class KUtil

    ''' <summary>
    ''' Maps a number from one range to another.
    ''' </summary>
    ''' <param name="x">The input value to map.</param>
    ''' <param name="inMin">The lower bound of the input range.</param>
    ''' <param name="inMax">The upper bound of the input range.</param>
    ''' <param name="outMin">The lower bound of the output range.</param>
    ''' <param name="outMax">The upper bound of the output range.</param>
    ''' <returns>The input value mapped to the output range.</returns>
    Public Shared Function Map(x As Double, inMin As Double, inMax As Double, outMin As Double, outMax As Double) As Double
        Return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin
    End Function

    ''' <summary>
    ''' Maps a number from the range 0 to 1 to another specified range.
    ''' </summary>
    ''' <param name="x">The input value to map, expected to be in the range 0 to 1.</param>
    ''' <param name="outMin">The lower bound of the output range.</param>
    ''' <param name="outMax">The upper bound of the output range.</param>
    ''' <returns>The input value mapped to the output range.</returns>
    Public Shared Function MapFrom01(x As Double, outMin As Integer, outMax As Integer) As Integer
        Return Map(x, 0, 1, outMin, outMax)
    End Function

    ''' <summary>
    ''' Maps a number from a specified range to the range 0 to 1.
    ''' </summary>
    ''' <param name="x">The input value to map.</param>
    ''' <param name="inMin">The lower bound of the input range.</param>
    ''' <param name="inMax">The upper bound of the input range.</param>
    ''' <returns>The input value mapped to the range 0 to 1.</returns>
    Public Shared Function MapTo01(x As Double, inMin As Double, inMax As Double) As Double
        Return Map(x, inMin, inMax, 0, 1)
    End Function

    Public Enum RoundOps
        ToBaseNumber
        ToNextNumber
        Point5Down
        Point5Up
    End Enum

    ''' <summary>
    ''' Rounds a number to the nearest integer based on the specified rounding operation.
    ''' </summary>
    ''' <param name="input">The number to round.</param>
    ''' <param name="roundOp">The rounding operation to perform. <c>ToBaseNumber</c> rounds down, <c>ToNextNumber</c> rounds up, <c>Point5Down</c> rounds half down, and <c>Point5Up</c> rounds half up.</param>
    ''' <returns>The rounded number.</returns>
    Public Shared Function Round(input As Double, roundOp As RoundOps) As Integer
        Dim result As Integer = Math.Truncate(input) ' This makes Result be the base number
        Dim RoundUp As Boolean = False

        Select Case roundOp
            Case RoundOps.ToNextNumber
                RoundUp = True
            Case RoundOps.Point5Down
                If (input - result) > 0.5 Then
                    RoundUp = True
                End If
            Case RoundOps.Point5Up
                If (input - result) >= 0.5 Then
                    RoundUp = True
                End If
        End Select

        If RoundUp Then
            result += 1
        End If

        Return result
    End Function

    ''' <summary>
    ''' Adds leading zeros to a number to make it a specified number of digits long.
    ''' </summary>
    ''' <param name="input">The value</param>
    ''' <param name="digits">How many leading 0's do you want?</param>
    ''' <returns></returns>
    Public Shared Function LeadingZeros(input As Integer, digits As Integer) As String
        Dim result As String = input.ToString()
        While result.Length < digits
            result = "0" & result
        End While
        Return result
    End Function

    ''' <summary>
    ''' Reads a text file into a dictionary, seperated by the delimiter. Good for reading in config files.
    ''' </summary>
    ''' <param name="path">Where is the file</param>
    ''' <param name="Delim">What do you want to use between the property name and value?</param>
    ''' <param name="Cmnt">What is considered a comment, so we can ignore them.</param>
    ''' <returns></returns>
    Public Shared Function ReadTextToDictionary(path As String, Delim As Char, Cmnt As Char) As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)
        Dim lines As String()

        If IO.File.Exists(path) Then
            lines = IO.File.ReadAllLines(path)
        Else
            Return result
        End If

        For Each line As String In lines
            line = line.TrimStart()
            If Not line.StartsWith(Cmnt) Then
                Dim delimIndex As Integer = line.IndexOf(Delim)
                If delimIndex >= 0 Then
                    Dim key As String = line.Substring(0, delimIndex).Trim()
                    Dim value As String = line.Substring(delimIndex + 1).Trim()
                    result.Add(key, value)
                End If
            End If
        Next

        Return result
    End Function

    Public Shared Function GetRangeFromArray(Arr As Array, rngMin As Integer, rngMax As Integer) As Array
        Dim result As Array = Array.CreateInstance(Arr.GetType().GetElementType(), rngMax - rngMin + 1)
        Dim i As Integer = 0
        Dim j As Integer = 0

        For Each item As Object In Arr
            If i >= rngMin And i <= rngMax Then
                result.SetValue(item, j)
                j += 1
            End If
            i += 1
        Next

        Return result
    End Function

    Public Enum StartPoint
        IStart
        IMiddle
        IEnd
    End Enum


    ''' <summary>
    ''' Get's a percent of an array and returns it as a new array.
    ''' </summary>
    ''' <param name="arr">The array you want to get a percentage of</param>
    ''' <param name="index">What index should we be working around, for example, if you want to get the first 10th percentile of an array, you would have the index be 0 and use the "IStart" for Start point.</param>
    ''' <param name="percent">What is the percentage you want, it must be a number between 1 and 99, as 0 would be none of the array and 100 would be the whole array.</param>
    ''' <param name="startPoint">Where do you want to start, IStart will mean the index you gave will be the first element of the new list, IEnd it would be the last, and IMiddle it will be somewhere in the middle.</param>
    ''' <returns></returns>
    Public Shared Function GetPercentageFromArray(arr As Array, index As Integer, percent As Integer, startPoint As StartPoint) As Array
        ' Calculate how many elements we need
        Dim sindx As Integer = 0
        Dim eindx As Integer = arr.Length
        Dim indadd As Integer

        If percent < 1 Or percent > 99 Then
            Throw New Exception($"Percentage must be between 1 and 99, you have chosen {percent}.")
        End If


        indadd = arr.Length * (percent / 100)

        Select Case startPoint
            Case StartPoint.IStart
                sindx = index
                eindx = index + indadd
            Case StartPoint.IMiddle
                sindx = index - Round(indadd / 2, RoundOps.ToBaseNumber)
                eindx = index + (indadd - Round(indadd / 2, RoundOps.ToBaseNumber))
            Case StartPoint.IEnd
                sindx = index - indadd
                eindx = index
        End Select

        If sindx < 0 Then
            sindx = 0
        End If

        If eindx > arr.Length Then
            eindx = arr.Length
        End If

        Return GetRangeFromArray(arr, sindx, eindx)
    End Function
End Class