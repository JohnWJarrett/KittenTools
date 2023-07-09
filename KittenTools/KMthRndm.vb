''' <summary>
''' A class for generating random numbers
''' <author>Kitten</author>
''' <created>2022-07-09</created>
''' </summary>
Public Class KMthRndm
    Private ReadOnly R As Random

    Public Sub New()
        R = New Random()
    End Sub

    ''' <summary>
    ''' Get a random integer between min and max (inclusive)
    ''' </summary>
    ''' <param name="Min">Integer: The minimum number</param>
    ''' <param name="Max">Integer: The maximum number</param>
    ''' <returns></returns>
    Public Function RanInt(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Result As Integer

        Result = R.Next(Min * 100, (Max + 1) * 100)

        If Not Result = 0 Then
            Result /= 100
        End If

        Return Result
    End Function


    ''' <summary>
    ''' Get a random double between min and max (inclusive)
    ''' </summary>
    ''' <param name="Min">Double: The minimum number</param>
    ''' <param name="Max">Double: The maximum number</param>
    ''' <returns></returns>
    Public Function RanDbl(ByVal Min As Double, ByVal Max As Double) As Double
        Dim Result As Double

        Result = R.NextDouble() * (Max - Min) + Min

        Return Result
    End Function

    ''' <summary>
    ''' A Random Boolean
    ''' </summary>
    ''' <returns></returns>
    Public Function RanBool() As Boolean
        Dim Result As Boolean

        Result = R.Next(0, 2)

        Return Result
    End Function

    ''' <summary>
    ''' A random number generated from a Gaussian distribution
    ''' </summary>
    ''' <param name="Mean">Double: Where along the line the max should be</param>
    ''' <param name="StdDev">Double: How wide the bell should be</param>
    ''' <returns></returns>
    Public Function RanGauss(ByVal Mean As Double, ByVal StdDev As Double) As Double
        Dim Result As Double
        Dim U1 As Double
        Dim U2 As Double

        U1 = R.NextDouble()
        U2 = R.NextDouble()

        Result = Math.Sqrt(-2.0 * Math.Log(U1)) * Math.Sin(2.0 * Math.PI * U2)

        Result = Mean + StdDev * Result

        Return Result
    End Function

    ''' <summary>
    ''' A random double that trends towards one side or the other
    ''' </summary>
    ''' <param name="minValue">Integer: The minimum number</param>
    ''' <param name="maxValue">Integer: The maximum number</param>
    ''' <param name="bias">Double: Below 1 will trend to the max value, above will trend to the minimum</param>
    ''' <returns></returns>
    Public Function RanBiasedDbl(minValue As Integer, maxValue As Integer, bias As Double) As Double
        Dim Result As Double
        Dim Rand As Double
        Dim BiasedRand As Double

        Rand = R.NextDouble()
        BiasedRand = Math.Pow(Rand, bias)

        Result = minValue + (BiasedRand * (maxValue - minValue))

        Return Math.Floor(Result)
    End Function

    ''' <summary>
    ''' A random Integer that trends towards one side or the other
    ''' </summary>
    ''' <param name="minValue">Integer: The minimum number</param>
    ''' <param name="maxValue">Integer: The maximum number</param>
    ''' <param name="bias">Double: Below 1 will trend to the max value, above will trend to the minimum</param>
    ''' <returns></returns>
    Public Function RanBiasedInt(minValue As Integer, maxValue As Integer, bias As Double) As Integer
        Return Math.Round(RanBiasedDbl(minValue, maxValue, bias))
    End Function

    ''' <summary>
    ''' A random double that trends towards the edges or the middle
    ''' </summary>
    ''' <param name="minValue">Integer: The minimum number</param>
    ''' <param name="maxValue">Integer: The maximum number</param>
    ''' <param name="biasTowardsEdges">Boolean: True will trend to the edges, false will trend to the middle</param>
    ''' <returns></returns>
    Public Function RanBiasedInOutDbl(minValue As Integer, maxValue As Integer, biasTowardsEdges As Boolean) As Double
        Dim Result As Double
        Dim Rand As Double

        Rand = R.NextDouble()

        If biasTowardsEdges Then
            If Rand < 0.5 Then
                Rand = Math.Pow(Rand * 2, 2) / 2
            Else
                Rand = 1 - Math.Pow((1 - Rand) * 2, 2) / 2
            End If
        Else
            If Rand < 0.5 Then
                Rand = Math.Sqrt(Rand * 2) / 2
            Else
                Rand = 1 - Math.Sqrt((1 - Rand) * 2) / 2
            End If
        End If

        Result = minValue + (Rand * (maxValue - minValue))

        Return Math.Floor(Result)
    End Function

    ''' <summary>
    ''' A random integer that trends towards the edges or the middle
    ''' </summary>
    ''' <param name="minValue">Integer: The minimum number</param>
    ''' <param name="maxValue">Integer: The maximum number</param>
    ''' <param name="biasTowardsEdges">Boolean: True will trend to the edges, false will trend to the middle</param>
    ''' <returns></returns>
    Public Function RanBiasedInOutInt(minValue As Integer, maxValue As Integer, biasTowardsEdges As Boolean) As Integer
        Return Math.Round(RanBiasedInOutDbl(minValue, maxValue, biasTowardsEdges))
    End Function

End Class
