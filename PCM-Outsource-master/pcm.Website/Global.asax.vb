Imports System.Net
Imports System.Web.SessionState
Imports DevExpress.XtraReports.UI
Imports Entities
Imports pcm.BusinessLayer




Public Class Global_asax
    Inherits System.Web.HttpApplication
    Private _blErrorLogging As New ErrorLogBL



    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize()

        DevExpress.XtraReports.UI.ExternalFileAccessSecurityLevelSettings.SecurityLevel = ExternalFileAccessSecurityLevel.Unrestricted

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started

    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        'Fires when an error occurs
        Dim LastError = Server.GetLastError()
        _blErrorLogging.ErrorLogging(LastError, Session("ipaddress"))


    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub


End Class