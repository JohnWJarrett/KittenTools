''' <summary>
''' A class representing an Exponential Growth curve.
''' <author>Kitten</author>
''' <created>2023-07-09</created>
''' </summary>
Public Class KMC_ExpGrowth
    Inherits KMthCurve

    Private scale As Double
    Private growthRate As Double

    ''' <summary>
    ''' Creates a new instance of the ExponentialGrowthCurve class.
    ''' </summary>
    ''' <param name="scale">The scale factor for the curve.</param>
    ''' <param name="growthRate">The rate of growth for the curve.</param>
    ''' <param name="resolution">The number of points to generate for the curve.</param>
    Public Sub New(scale As Double, growthRate As Double, resolution As Integer)
        Me.scale = scale
        Me.growthRate = growthRate
        CurveResolution = resolution
        CalculateCurve()
    End Sub

    ''' <summary>
    ''' Updates the scale factor and rate of growth parameters for the curve and recalculates the curve.
    ''' </summary>
    ''' <param name="scale">The scale factor for the curve.</param>
    ''' <param name="growthRate">The rate of growth for the curve.</param>
    Public Sub Morph(scale As Double, growthRate As Double)
        Me.scale = scale
        Me.growthRate = growthRate
        CalculateCurve()
    End Sub

    Private Sub CalculateCurve()
        Curve = New Double(CurveResolution - 1) {}
        For i As Integer = 0 To CurveResolution - 1
            Dim x As Double = i / (CurveResolution - 1) ' scale x to range [0,1]
            Curve(i) = scale * Math.Exp(growthRate * x)
        Next
    End Sub
End Class
