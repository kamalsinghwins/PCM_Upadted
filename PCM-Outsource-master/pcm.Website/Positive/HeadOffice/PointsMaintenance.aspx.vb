Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Public Class PointsMaintenance
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _blayer As New ManageHOBL
    Dim _dt As New DataTable
    Dim RG As New DataLayer.Utilities.clsUtil
    Dim baseResponse As New BaseResponse
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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManagePoints) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            _dt = _blayer.GetCompanySettings()
            For Each dr As DataRow In _dt.Rows
                txtDollars.Text = RG.Numb(dr("cash_to_points"))
                txtPoints.Text = RG.Numb(dr("points_to_cash"))
            Next
        End If
    End Sub
    Private Sub PointsMaintenance_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Try

            If hdWhichButton.Value = "Run" Then
                Save()
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Private Sub Save()
        Try
            baseResponse = _blayer.SaveCompanySettings(txtDollars.Text, txtPoints.Text)
            If baseResponse.Success = True Then
                dxPopUpError.HeaderText = "Success"
                lblError.Text = baseResponse.Message
                clearForm()
            Else
                lblError.Text = "Something went wrong"
            End If

            dxPopUpError.ShowOnPageLoad = True
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Private Sub clearForm()
        txtDollars.Text = String.Empty
        txtPoints.Text = String.Empty
    End Sub
End Class