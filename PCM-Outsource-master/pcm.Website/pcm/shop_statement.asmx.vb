Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports pcm.BusinessLayer
Imports Entities

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class shop_statement
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function ReturnStatements(ByVal CardNumber As String) As ShopMiniStatement
        Dim MiniStatement As New ShopMiniStatementBusinessLayer
        Return MiniStatement.GetData(CardNumber, "")

    End Function

   

End Class