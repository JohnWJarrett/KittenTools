Imports System.ComponentModel
Imports System.IO

''' <summary>
''' Makes a list and checks it twice, or rather makes a list from a file or an array of strings and gives methods to get entries from it, mostly at random.
''' Please note that only editing the list(s) will be thread safe, getting entries from it will not be.
''' <author>Kitten</author>
''' <created>2023-07-11</created>
''' </summary>
Public Class KRndmGen

    ' ================== Variables ==================

    Private ReadOnly KRnd As KMthRndm
    Private EntryList As List(Of String)

    Private ReadOnly DefaultEntryList As List(Of String)

    Private FileName As String
    Private CommentChar As Char

    Private CIndx As Integer = 0

    Private ReadOnly EntryLock As New Object()

    Private Const NoFile As String = "NA"

    ' ================== Enums ==================

    ''' <summary>
    ''' What kind of operation to use.
    ''' </summary>
    Public Enum RandomType
        <Description("Uses a normal Integer Random number between 0 and max entries")>
        Int
        <Description("Uses a Gaussian distribution with a mean of X and a standard deviation of Y")>
        Gaussian
        <Description("Uses the Biased Random number generator with a bias towards the start of the list")>
        BiasedStart
        <Description("Uses the Biased Random number generator with a bias towards the middle of the list")>
        BiasedMiddle
        <Description("Uses the Biased Random number generator with a bias towards the end of the list")>
        BiasedEnd
        <Description("Gets the next entry in the list, when it reaches the end, it starts again")>
        NextEntry
        <Description("Uses a Gaussian distribution with a mean of X and a standard deviation of Y, then removes the result from the list")>
        TrimGaussian
        <Description("Uses the Biased Random number generator with a bias towards the start of the list, then removes the result from the list")>
        TrimBiasedStart
        <Description("Uses the Biased Random number generator with a bias towards the middle of the list, then removes the result from the list")>
        TrimBiasedMiddle
        <Description("Uses the Biased Random number generator with a bias towards the end of the list, then removes the result from the list")>
        TrimBiasedEnd
        <Description("Gets the entry at the specified index")>
        Indx
    End Enum

    ''' <summary>
    ''' Which list to target
    ''' </summary>
    Public Enum ListOpsTarget
        <Description("This is the list that is realoaded if using the 'Reset' command")>
        DefaultList
        <Description("The working list that is used for the random selection")>
        EntryList
    End Enum

    ' ================== List Ops ==================

    ''' <summary>
    ''' Creates a new KRndmGen object from file.
    ''' </summary>
    ''' <param name="File">The File to read in</param>
    ''' <param name="cmnt">What character is considered a comment if used first in a line (Not including white space)</param>
    Public Sub New(File As String, cmnt As Char)
        KRnd = New KMthRndm()

        EntryList = New List(Of String)
        DefaultEntryList = New List(Of String)

        ReadFile(File, cmnt)
    End Sub

    ''' <summary>
    ''' Creates a new KRndmGen object from an array of strings
    ''' </summary>
    ''' <param name="StringArr">The array of strings</param>
    ''' <param name="cmnt">What character is considered a comment if used first in a line (Not including white space)</param>
    Public Sub New(StringArr As String(), cmnt As Char)
        KRnd = New KMthRndm()

        EntryList = New List(Of String)
        DefaultEntryList = New List(Of String)

        FileName = NoFile
        CommentChar = cmnt

        For Each l As String In StringArr
            If Not l.TrimStart().StartsWith(cmnt) Then
                DefaultEntryList.Add(l)
            End If
        Next
    End Sub


    ''' <summary>
    ''' Returns a copy of the list of entries so that you can read through it or get it's count.
    ''' </summary>
    ''' <returns>a COPY of the entry list</returns>
    Public Function GetEntries() As IEnumerable(Of String)
        Return New List(Of String)(EntryList)
    End Function

    ' Reads in the file
    Private Sub ReadFile(File As String, cmnt As Char)
        FileName = File
        CommentChar = cmnt
        SetList()
        EntryList = DefaultEntryList
    End Sub

    ''' <summary>
    ''' Reset the list back to the DefaultEntryList
    ''' </summary>
    Public Sub ResetList()
        SyncLock EntryLock
            EntryList = DefaultEntryList
        End SyncLock
    End Sub

    ''' <summary>
    ''' Reloads the list from the file
    ''' </summary>
    Public Sub ReloadList()
        If Not FileName = NoFile Then
            SetList()
            EntryList = DefaultEntryList
        Else
            Throw New IOException("No file was specified when creating the object.")
        End If
    End Sub

    ' Reads in the file
    Private Sub SetList()
        If IO.File.Exists(FileName) Then
            For Each l As String In IO.File.ReadAllLines(FileName)
                If Not l.TrimStart().StartsWith(CommentChar) Then
                    DefaultEntryList.Add(l)
                End If
            Next
        Else
            Throw New IOException($"{FileName} does not exist or no longer exists.")
        End If
    End Sub

    ''' <summary>
    ''' Adds an entry to the list
    ''' </summary>
    ''' <param name="NewEntry">The new entry to add</param>
    ''' <param name="ListToTarget">What list to add the entry to</param>
    Public Sub AddEntry(NewEntry As String, Optional ListToTarget As ListOpsTarget = ListOpsTarget.EntryList)
        SyncLock EntryLock
            Select Case ListToTarget
                Case ListOpsTarget.DefaultList
                    DefaultEntryList.Add(NewEntry)
                Case ListOpsTarget.EntryList
                    EntryList.Add(NewEntry)
            End Select
        End SyncLock
    End Sub

    ''' <summary>
    ''' Removes an entry from the list
    ''' </summary>
    ''' <param name="Entry">Which entry to remove</param>
    ''' <param name="ListToTarget">What list to remove it from</param>
    Public Sub RemoveEntry(Entry As String, Optional ListToTarget As ListOpsTarget = ListOpsTarget.EntryList)
        SyncLock EntryLock
            Select Case ListToTarget
                Case ListOpsTarget.DefaultList
                    DefaultEntryList.Remove(Entry)
                Case ListOpsTarget.EntryList
                    EntryList.Remove(Entry)
            End Select
        End SyncLock
    End Sub

    ''' <summary>
    ''' Removes an entry from the list at the specified index
    ''' </summary>
    ''' <param name="index">What index to remove</param>
    ''' <param name="ListToTarget">What list to remove it from</param>
    Public Sub RemoveEntryAt(index As Integer, Optional ListToTarget As ListOpsTarget = ListOpsTarget.EntryList)
        SyncLock EntryLock
            Select Case ListToTarget
                Case ListOpsTarget.DefaultList
                    DefaultEntryList.RemoveAt(index)
                Case ListOpsTarget.EntryList
                    EntryList.RemoveAt(index)
            End Select
        End SyncLock
    End Sub

    ''' <summary>
    ''' Edit an entry in the list
    ''' </summary>
    ''' <param name="Entry">Which entry to edit</param>
    ''' <param name="NewEntry">What should it now be</param>
    ''' <param name="ListToTarget">What list to edit it in</param>
    Public Sub EditEntry(Entry As String, NewEntry As String, Optional ListToTarget As ListOpsTarget = ListOpsTarget.EntryList)
        Dim indx As Integer

        Select Case ListToTarget
            Case ListOpsTarget.DefaultList
                indx = DefaultEntryList.IndexOf(Entry)
            Case ListOpsTarget.EntryList
                indx = EntryList.IndexOf(Entry)
        End Select

        If indx > -1 Then
            SyncLock EntryLock
                Select Case ListToTarget
                    Case ListOpsTarget.DefaultList
                        DefaultEntryList(indx) = NewEntry
                    Case ListOpsTarget.EntryList
                        EntryList(indx) = NewEntry
                End Select
            End SyncLock
        Else
            Throw New ArgumentException($"Entry {Entry} does not exist in the list.")
        End If
    End Sub

    ''' <summary>
    ''' Reset the index back to 0, for use with the GetNextEntry function
    ''' </summary>
    Public Sub ResetIndex()
        CIndx = 0
    End Sub

    ' ================== Random Ops ==================

    ''' <summary>
    ''' Get an entry from the lists based on different methods.
    ''' </summary>
    ''' <param name="rndmtyp">Determines the method to get the entry.</param>
    ''' <param name="index">ONLY Required if 'rndmtyp' is Indx. Represents the index in the list to get the entry from.</param>
    ''' <param name="GaussX">ONLY Required if 'rndmtyp' is Gaussian. Represents the mean value of the Gaussian distribution.</param>
    ''' <param name="GaussY">ONLY Required if 'rndmtyp' is Gaussian. Represents the standard deviation of the Gaussian distribution.</param>
    ''' <returns>The selected entry from the list.</returns>
    Public Function GetRandomEntry(rndmtyp As RandomType, Optional index As Integer = -1, Optional GaussX As Double = 0.0, Optional GaussY As Double = 1.0) As String
        Dim Result As String

        If EntryList.Count = 0 Then
            Throw New InvalidOperationException("The List is empty.")
        End If

        Select Case rndmtyp
            Case RandomType.TrimBiasedEnd, RandomType.TrimBiasedMiddle, RandomType.TrimBiasedStart
                Result = GetRandomEntryThenTrim(rndmtyp, index, GaussX, GaussY)
            Case Else
                Result = RandomEntry(rndmtyp, index, GaussX, GaussY)
        End Select

        Return Result
    End Function

    ' Gets a random entry based on what method was specified
    Private Function RandomEntry(rndmtyp As RandomType, index As Integer, GaussX As Double, GaussY As Double) As String
        Dim Result As String = ""

        Select Case rndmtyp
            Case RandomType.Int
                Result = EntryList(KRnd.RanInt(0, EntryList.Count - 1))
            Case RandomType.Gaussian
                Result = EntryList(KRnd.RanGauss(GaussX, GaussY))
            Case RandomType.BiasedStart
                Result = EntryList(KRnd.RanBiasedInt(0, EntryList.Count - 1, 0.5))
            Case RandomType.BiasedMiddle
                Result = EntryList(KRnd.RanBiasedInOutInt(0, EntryList.Count - 1, False))
            Case RandomType.BiasedEnd
                Result = EntryList(KRnd.RanBiasedInt(0, EntryList.Count - 1, 1.5))
            Case RandomType.NextEntry
                Result = EntryList(CIndx)
                CIndx += 1
                If CIndx = EntryList.Count Then CIndx = 0
            Case RandomType.Indx
                If Not index < 0 Or index > EntryList.Count - 1 Then
                    Result = EntryList(index)
                Else
                    Throw New IndexOutOfRangeException($"Index {index} is out of range, it should be more than 0 and less than {EntryList.Count - 1}.")
                End If
        End Select

        Return Result
    End Function

    ' Gets a random entry based on what method was specified, then removes it from the list
    Private Function GetRandomEntryThenTrim(rndmtyp As RandomType, index As Integer, GaussX As Double, GaussY As Double) As String
        Dim Result As String = ""

        Select Case rndmtyp
            Case RandomType.TrimGaussian
                Result = RandomEntry(RandomType.Gaussian, index, GaussX, GaussY)
            Case RandomType.TrimBiasedStart
                Result = RandomEntry(RandomType.BiasedStart, index, GaussX, GaussY)
            Case RandomType.TrimBiasedMiddle
                Result = RandomEntry(RandomType.BiasedMiddle, index, GaussX, GaussY)
            Case RandomType.TrimBiasedEnd
                Result = RandomEntry(RandomType.BiasedEnd, index, GaussX, GaussY)
        End Select

        RemoveEntry(Result)

        Return Result
    End Function
End Class
