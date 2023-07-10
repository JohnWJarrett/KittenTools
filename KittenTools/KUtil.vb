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
End Class