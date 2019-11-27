Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports System.IO
Public Class Specials
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Shared _Bl As New ManageHOBL

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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.Specials) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            txtStartDate.Text = Format(Now, "yyyy-MM-dd")
            txtEndDate.Text = Format(Now, "yyyy-MM-dd")
        End If

    End Sub

    Private Sub Specials_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub





End Class