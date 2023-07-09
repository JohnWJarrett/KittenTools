''' <summary>
''' A class representing an S curve.
''' <author>Kitten</author>
''' <created>2022-07-09</created>
''' </summary>
Public Class KMC_S
    Inherits KMthCurve

    Private k1 As Double
    Private k2 As Double
    Private x0 As Double
    Private x1 As Double

    ''' <summary>
    ''' Initializes a new instance of the Custom_S_Curve class.
    ''' </summary>
    ''' <param name="k1">The logistic growth rate or steepness of the 'ease in' part of the curve.</param>
    ''' <param name="k2">The logistic growth rate or steepness of the 'ease out' part of the curve.</param>
    ''' <param name="x0">The x-coordinate at which the curve's increase is fastest.</param>
    ''' <param name="x1">The x-coordinate at which the curve's decrease begins.</param>
    ''' <param name="resolution">The number of points along the curve.</param>
    Public Sub New(k1 As Double, k2 As Double, x0 As Double, x1 As Double, resolution As Integer)
        Me.k1 = k1
        Me.k2 = k2
        Me.x0 = x0
        Me.x1 = x1
        CurveResolution = resolution
        CalculateCurve()
    End Sub

    ''' <summary>
    ''' Updates the steepness and transition point parameters for the curve and recalculates the curve.
    ''' </summary>
    ''' <param name="k1">The logistic growth rate or steepness of the 'ease in' part of the curve.</param>
    ''' <param name="k2">The logistic growth rate or steepness of the 'ease out' part of the curve.</param>
    ''' <param name="x0">The x-coordinate at which the curve's increase is fastest.</param>
    ''' <param name="x1">The x-coordinate at which the curve's decrease begins.</param>
    Public Sub Morph(k1 As Double, k2 As Double, x0 As Double, x1 As Double)
        Me.k1 = k1
        Me.k2 = k2
        Me.x0 = x0
        Me.x1 = x1
        CalculateCurve()
    End Sub

    Private Sub CalculateCurve()
        Curve = New Double(CurveResolution - 1) {}
        For i As Integer = 0 To CurveResolution - 1
            Dim x As Double = i / (CurveResolution - 1) ' scale x to range [0,1]
            If x < x0 Then
                ' Ease in
                Curve(i) = 1 / (1 + Math.Exp(-k1 * (x - x0)))
            Else
                ' Ease out
                Curve(i) = 1 - (1 / (1 + Math.Exp(-k2 * (x - x1))))
            End If
        Next
    End Sub
End Class
