Imports DevExpress.Web
Imports Entities
Imports Entities.Reports
Imports Newtonsoft.Json
Imports pcm.BusinessLayer
Public Class ColourGrids
    Inherits System.Web.UI.Page
    Shared _blErrorLogging As New ErrorLogBL
    Shared _blayer As New ReportsBusinessLayer
    Shared ds As New DataSet
    Shared dt As New DataTable
    Shared searchResponse As New SearchResponse
    Public Username As String = String.Empty
    Public Email As String = String.Empty
    Public IsAdmin As Boolean
    Public IPAddress As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.ColourGridReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
            SetValues()
        End If
    End Sub
    Private Sub ColourGrid_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Sub SetValues()
        Username = Session("username")
        Email = Session("email")
        IsAdmin = Session("is_pcm_admin")
        IPAddress = Session("ipaddress")
    End Sub
End Class