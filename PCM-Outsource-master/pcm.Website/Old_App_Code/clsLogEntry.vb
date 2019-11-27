Imports System.IO
Imports Microsoft.VisualBasic

Public Class clsLogEntry

    Public Sub New(ByVal Login As clsLogin)

        'If IO.Directory.Exists(HttpContext.Current.Server.MapPath("\Logs")) = False Then
        '    IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("\Logs"))
        'End If

        Dim FileName As String

        FileName = HttpContext.Current.Server.MapPath("Logs\") & Format(Now, "yyyy-MM-dd") & ".txt"

        Dim CreateFile As New FileStream(FileName, FileMode.Append)
        Dim strStreamWriter As New StreamWriter(CreateFile)

        strStreamWriter.WriteLine(Format(Now, "HH:mm:ss") & ",ID Number," & Login.IDNumber & ",Last Name," & Login.LastName & ",Captcha Code," & Login.CaptchaCode & ",IP Address," & Login.IPAddress & _
                                  ",Successful," & Login.Successful)

        strStreamWriter.Close()
        strStreamWriter.Dispose()

        CreateFile.Close()
        CreateFile.Dispose()

    End Sub

    'Public Sub WriteLine(ByVal Login As clsLogin)

    '    If IO.Directory.Exists(HttpContext.Current.Server.MapPath("\Logs")) = False Then
    '        IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("\Logs"))
    '    End If

    '    Dim FileName As String

    '    FileName = HttpContext.Current.Server.MapPath("\Logs\") & Format(Now, "yyyy-MM-dd") & ".txt"

    '    Dim CreateFile As New FileStream(FileName, FileMode.Append)
    '    Dim strStreamWriter As New StreamWriter(CreateFile)

    '    strStreamWriter.WriteLine(Format(Now, "HH:mm:ss") & ",ID Number," & Login.IDNumber & ",Last Name," & Login.LastName & ",Captcha Code," & Login.CaptchaCode & ",IP Address," & Login.IPAddress & _
    '                              ",Successful," & Login.Successful)

    '    strStreamWriter.Close()
    '    strStreamWriter.Dispose()

    '    CreateFile.Close()
    '    CreateFile.Dispose()

    'End Sub

End Class
