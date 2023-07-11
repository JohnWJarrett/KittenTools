''' <summary>
''' A class representing a Gaussian curve.
''' <author>Kitten</author>
''' <created>2023-07-09</created>
''' </summary>
Public Class KMC_Gaussian
    Inherits KMthCurve

    Private peakHeight As Double
    Private centerPosition As Double
    Private standardDeviation As Double

    ''' <summary>
    ''' Initializes a new instance of the Gaussian class.
    ''' </summary>
    ''' <param name="peakHeight">The height of the curve's peak.</param>
    ''' <param name="centerPosition">The position of the center of the peak.</param>
    ''' <param name="standardDeviation">The standard deviation, dictating the width of the "bell".</param>
    ''' <param name="resolution">The number of points along the curve.</param>
    Public Sub New(peakHeight As Double, centerPosition As Double, standardDeviation As Double, resolution As Integer)
        Me.peakHeight = peakHeight
        Me.centerPosition = centerPosition
        Me.standardDeviation = standardDeviation
        Me.CurveResolution = resolution
        GenerateCurve()
    End Sub

    ''' <summary>
    ''' Updates the parameters of the Gaussian curve and regenerates the curve.
    ''' </summary>
    ''' <param name="peakHeight">The new height of the curve's peak.</param>
    ''' <param name="centerPosition">The new position of the center of the peak.</param>
    ''' <param name="standardDeviation">The new standard deviation, dictating the width of the "bell".</param>
    Public Sub Morph(peakHeight As Double, centerPosition As Double, standardDeviation As Double)
        Me.peakHeight = peakHeight
        Me.centerPosition = centerPosition
        Me.standardDeviation = standardDeviation
        GenerateCurve()
    End Sub

    Private Sub GenerateCurve()
        Curve = New Double(CurveResolution - 1) {}
        For i As Integer = 0 To CurveResolution - 1
            Dim x As Double = i / (CurveResolution - 1)
            Curve(i) = peakHeight * Math.Exp(-((x - centerPosition) ^ 2) / (2 * standardDeviation ^ 2))
        Next
    End Sub
End Class
