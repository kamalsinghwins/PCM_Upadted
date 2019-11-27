Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Entities
Imports pcm.BusinessLayer

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class screensaver
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetImages(ByVal wsPassword As String) As entScreensaverImages

        If wsPassword <> "JaiRL10nFMNo$forany" Then
            Return Nothing
        End If

        Dim Blayer As New ScreensaverHOBL("010")

        Dim sIPAddress As String
        sIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If sIPAddress = "" Then sIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Return Blayer.GetImages

    End Function

End Class