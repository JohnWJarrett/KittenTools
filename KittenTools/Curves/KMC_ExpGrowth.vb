''' <summary>
''' A class representing an Exponential Growth curve.
''' <author>Kitten</author>
''' <created>2022-07-09</created>
''' </summary>
Public Class KMC_ExpGrowth
    Inherits KMthCurve

    Private a As Double
    Private b As Double

    ''' <summary>
    ''' Creates a new instance of the ExponentialGrowthCurve class.
    ''' </summary>
    ''' <param name="a">The scale factor for the curve.</param>
    ''' <param name="b">The rate of growth for the curve.</param>
    ''' <param name="resolution">The number of points to generate for the curve.</param>
    Public Sub New(a As Double, b As Double, resolution As Integer)
        Me.a = a
        Me.b = b
        CurveResolution = resolution
        CalculateCurve()
    End Sub

    ''' <summary>
    ''' Updates the scale factor and rate of growth parameters for the curve and recalculates the curve.
    ''' </summary>
    ''' <param name="a">The scale factor for the curve.</param>
    ''' <param name="b">The rate of growth for the curve.</param>
    Public Sub Morph(a As Double, b As Double)
        Me.a = a
        Me.b = b
        CalculateCurve()
    End Sub

    Private Sub CalculateCurve()
        Curve = New Double(CurveResolution - 1) {}
        For i As Integer = 0 To CurveResolution - 1
            Dim x As Double = i / (CurveResolution - 1) ' scale x to range [0,1]
            Curve(i) = a * Math.Exp(b * x)
        Next
    End Sub
End Class
