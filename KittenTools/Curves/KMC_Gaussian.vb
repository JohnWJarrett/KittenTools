''' <summary>
''' A class representing a Gaussian curve.
''' <author>Kitten</author>
''' <created>2023-07-09</created>
''' </summary>
Public Class KMC_Gaussian
    Inherits KMthCurve

    Private a As Double
    Private mu As Double
    Private sigma As Double

    ''' <summary>
    ''' Initializes a new instance of the Gaussian class.
    ''' </summary>
    ''' <param name="a">The height of the curve's peak.</param>
    ''' <param name="mu">The position of the center of the peak.</param>
    ''' <param name="sigma">The standard deviation, dictating the width of the “bell”.</param>
    ''' <param name="resolution">The number of points along the curve.</param>
    Public Sub New(a As Double, mu As Double, sigma As Double, resolution As Integer)
        Me.a = a
        Me.mu = mu
        Me.sigma = sigma
        Me.CurveResolution = resolution
        GenerateCurve()
    End Sub

    ''' <summary>
    ''' Updates the parameters of the Gaussian curve and regenerates the curve.
    ''' </summary>
    ''' <param name="a">The new height of the curve's peak.</param>
    ''' <param name="mu">The new position of the center of the peak.</param>
    ''' <param name="sigma">The new standard deviation, dictating the width of the “bell”.</param>
    Public Sub Morph(a As Double, mu As Double, sigma As Double)
        Me.a = a
        Me.mu = mu
        Me.sigma = sigma
        GenerateCurve()
    End Sub

    Private Sub GenerateCurve()
        Curve = New Double(CurveResolution - 1) {}
        For i As Integer = 0 To CurveResolution - 1
            Dim x As Double = i / (CurveResolution - 1)
            Curve(i) = a * Math.Exp(-((x - mu) ^ 2) / (2 * sigma ^ 2))
        Next
    End Sub
End Class
