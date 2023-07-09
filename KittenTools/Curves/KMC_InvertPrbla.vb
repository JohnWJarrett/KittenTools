''' <summary>
''' A class representing an Inverted Parabola curve.
''' <author>Kitten</author>
''' <created>2023-07-09</created>
''' </summary>
Public Class KMC_InvertPrbla
    Inherits KMthCurve

    Private p As Double

    ''' <summary>
    ''' Creates a new instance of the InvertedParabolaCurve class.
    ''' </summary>
    ''' <param name="p">The power to which the curve's function is raised. p = 1 gives a linear curve, p = 2 gives an inverted parabola. Higher values of p give steeper sides and a flatter peak.</param>
    ''' <param name="resolution">The number of points to generate for the curve.</param>
    Public Sub New(p As Double, resolution As Integer)
        Me.p = p
        CurveResolution = resolution
        CalculateCurve()
    End Sub

    ''' <summary>
    ''' Updates the power parameter for the curve and recalculates the curve.
    ''' </summary>
    ''' <param name="p">The power to which the curve's function is raised.</param>
    Public Sub Morph(p As Double)
        Me.p = p
        CalculateCurve()
    End Sub

    Private Sub CalculateCurve()
        Curve = New Double(CurveResolution - 1) {}
        For i As Integer = 0 To CurveResolution - 1
            Dim x As Double = i / (CurveResolution - 1) ' Scale x to range [0, 1]
            Curve(i) = -(x ^ p) * ((x - 1) ^ p) ' Customizable inverted parabola
        Next
    End Sub
End Class
