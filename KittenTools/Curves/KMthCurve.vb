''' <summary>
''' A Parent class representing the shared parts of a curve.
''' Can not be instantiated.
''' <author>Kitten</author>
''' <created>2022-07-09</created>
''' </summary>
Public MustInherit Class KMthCurve
    ''' <summary>
    ''' The number of points along the curve.
    ''' </summary>
    ''' <returns></returns>
    Public Property CurveResolution As Integer

    ''' <summary>
    ''' The an array of doubles containing all of the values of the curve itself.
    ''' </summary>
    ''' <returns></returns>
    Public Property Curve As Double()

    ''' <summary>
    ''' Returns the value of the curve at a given index.
    ''' </summary>
    ''' <param name="X">The index of the curve point.</param>
    ''' <returns>The value of the curve at the given index.</returns>
    Public Function GetValue(X As Integer) As Double
        If X >= 0 AndAlso X < CurveResolution Then
            Return Curve(X)
        Else
            Throw New Exception($"{X} is out of range, must be below {CurveResolution} and above 0.")
        End If
    End Function

    ''' <summary>
    ''' Returns a downsampled version of the curve with a specified resolution.
    ''' </summary>
    ''' <param name="newResolution">The resolution of the returned downsampled curve.</param>
    ''' <returns>An array representing the downsampled curve.</returns>
    ''' <exception cref="Exception">Thrown when the new resolution is larger than the original resolution.</exception>
    Public Function GetDownsampledCurve(newResolution As Integer) As Double()
        If newResolution > CurveResolution Then
            Throw New Exception($"The new resolution {newResolution} cannot be larger than the original resolution {CurveResolution}.")
        End If

        Dim downsampledCurve As Double() = New Double(newResolution - 1) {}
        For i As Integer = 0 To newResolution - 1
            Dim originalIndex As Double = i * (CurveResolution - 1) / (newResolution - 1)
            Dim lowerIndex As Integer = Math.Floor(originalIndex)
            Dim upperIndex As Integer = Math.Ceiling(originalIndex)
            If upperIndex >= CurveResolution Then
                upperIndex = CurveResolution - 1
            End If

            Dim fraction As Double = originalIndex - lowerIndex
            downsampledCurve(i) = Curve(lowerIndex) * (1 - fraction) + Curve(upperIndex) * fraction
        Next

        Return downsampledCurve
    End Function

End Class
