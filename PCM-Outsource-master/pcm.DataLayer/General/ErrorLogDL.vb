Imports Entities
Imports Newtonsoft.Json
Imports Npgsql
Imports System.IO
Public Class ErrorLogDL
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil


    Public Sub ErrorLogging(_error As Exception, Optional ByVal IPAddress As String = "")

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Try
            Dim _errorLog As New ErrorLog

            _errorLog.InnerException = Convert.ToString(_error.InnerException)
            _errorLog.Message = _error.Message
            _errorLog.Source = _error.Source
            _errorLog.StackTrace = _error.StackTrace
            _errorLog.HelpLink = _error.HelpLink
            _errorLog.HResult = _error.HResult
            _errorLog.TargetSite = _error.TargetSite.ToString()
            _errorLog.Data = _error.Data.ToString()

            tmpSQL = "insert into error_log(inner_exception,message,source,stack_trace,help_link,hresult,target_site,data,ip_address)" & "VALUES('" & RG.Apos(_errorLog.InnerException) & "','" & RG.Apos(_errorLog.Message) & "','" & _errorLog.Source & "','" & _errorLog.StackTrace & "','" & _errorLog.HelpLink & "','" & _errorLog.HResult & "','" & _errorLog.TargetSite & "','" & _errorLog.Data & "','" & IPAddress & "')"
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try
    End Sub

End Class
