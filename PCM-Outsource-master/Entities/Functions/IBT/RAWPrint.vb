Imports System.IO
Imports System.Runtime.InteropServices

Public Class clsRAWPrint

#Region " Private constants "
    Private Const GENERIC_WRITE As Integer = &H40000000
    Private Const OPEN_EXISTING As Integer = 3
#End Region

#Region " Private members "
    Private _SafeFileHandle As Microsoft.Win32.SafeHandles.SafeFileHandle
    Private _fileWriter As StreamWriter
    Private _outFile As FileStream
#End Region

#Region " private structures "
    <StructLayout(LayoutKind.Sequential)>
    Public Structure SECURITY_ATTRIBUTES
        Private nLength As Integer
        Private lpSecurityDescriptor As Integer
        Private bInheritHandle As Integer
    End Structure
#End Region

#Region " com calls "
    Private Declare Function CreateFile Lib "kernel32" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As Integer, ByVal dwShareMode As Integer, <MarshalAs(UnmanagedType.Struct)> ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES, ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, ByVal hTemplateFile As Integer) As Microsoft.Win32.SafeHandles.SafeFileHandle
#End Region

#Region " Public methods "
    ''' <summary>
    ''' This function must be called first.  Printer path must be a COM Port or a UNC path.
    ''' </summary>
    Public Sub StartWrite(ByVal printerPath As String)
        Dim SA As SECURITY_ATTRIBUTES

        'Create connection
        _SafeFileHandle = CreateFile(printerPath, GENERIC_WRITE, 0, SA, OPEN_EXISTING, 0, 0)

        'Create file stream
        Try
            _outFile = New FileStream(_SafeFileHandle, FileAccess.Write)
            _fileWriter = New StreamWriter(_outFile)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub Write(ByVal rawLine As String)
        If _fileWriter IsNot Nothing Then
            _fileWriter.WriteLine(rawLine)
        End If
    End Sub

    ''' <summary>
    ''' This function must be called after writing to the zebra printer.
    ''' </summary>
    Public Sub EndWrite()
        'Clean up
        If _fileWriter IsNot Nothing Then
            _fileWriter.Flush()
            _fileWriter.Close()
            _outFile.Close()
        End If
        _SafeFileHandle.Close()
        _SafeFileHandle.Dispose()

    End Sub
#End Region

End Class
