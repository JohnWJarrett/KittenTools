''' <summary>
''' A class representing an S curve.
''' <author>Kitten</author>
''' <created>2023-07-09</created>
''' </summary>
Public Class KMC_S
    Inherits KMthCurve

    Private easeInSteepness As Double
    Private easeOutSteepness As Double
    Private fastestIncreaseX As Double
    Private decreaseStartX As Double

    ''' <summary>
    ''' Initializes a new instance of the Custom_S_Curve class.
    ''' </summary>
    ''' <param name="easeInSteepness">The logistic growth rate or steepness of the 'ease in' part of the curve.</param>
    ''' <param name="easeOutSteepness">The logistic growth rate or steepness of the 'ease out' part of the curve.</param>
    ''' <param name="fastestIncreaseX">The x-coordinate at which the curve's increase is fastest.</param>
    ''' <param name="decreaseStartX">The x-coordinate at which the curve's decrease begins.</param>
    ''' <param name="resolution">The number of points along the curve.</param>
    Public Sub New(easeInSteepness As Double, easeOutSteepness As Double, fastestIncreaseX As Double, decreaseStartX As Double, resolution As Integer)
        Me.easeInSteepness = easeInSteepness
        Me.easeOutSteepness = easeOutSteepness
        Me.fastestIncreaseX = fastestIncreaseX
        Me.decreaseStartX = decreaseStartX
        CurveResolution = resolution
        CalculateCurve()
    End Sub

    ''' <summary>
    ''' Updates the steepness and transition point parameters for the curve and recalculates the curve.
    ''' </summary>
    ''' <param name="easeInSteepness">The logistic growth rate or steepness of the 'ease in' part of the curve.</param>
    ''' <param name="easeOutSteepness">The logistic growth rate or steepness of the 'ease out' part of the curve.</param>
    ''' <param name="fastestIncreaseX">The x-coordinate at which the curve's increase is fastest.</param>
    ''' <param name="decreaseStartX">The x-coordinate at which the curve's decrease begins.</param>
    Public Sub Morph(easeInSteepness As Double, easeOutSteepness As Double, fastestIncreaseX As Double, decreaseStartX As Double)
        Me.easeInSteepness = easeInSteepness
        Me.easeOutSteepness = easeOutSteepness
        Me.fastestIncreaseX = fastestIncreaseX
        Me.decreaseStartX = decreaseStartX
        CalculateCurve()
    End Sub

    Private Sub CalculateCurve()
        Curve = New Double(CurveResolution - 1) {}
        For i As Integer = 0 To CurveResolution - 1
            Dim x As Double = i / (CurveResolution - 1) ' scale x to range [0,1]
            If x < fastestIncreaseX Then
                ' Ease in
                Curve(i) = 1 / (1 + Math.Exp(-easeInSteepness * (x - fastestIncreaseX)))
            Else
                ' Ease out
                Curve(i) = 1 - (1 / (1 + Math.Exp(-easeOutSteepness * (x - decreaseStartX))))
            End If
        Next
    End Sub
End Class
