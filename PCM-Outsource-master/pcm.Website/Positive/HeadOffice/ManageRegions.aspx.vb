Imports DevExpress.Web
Imports Entities
Imports Entities.Manage
Imports pcm.BusinessLayer
Public Class ManageRegions
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _bl As New ManageHOBL
    Dim _dt As New DataTable
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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageRegions) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            Session.Remove("_RegionDS")
            GetRegions()
        End If

    End Sub
    Private Sub ManageRegions_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "Save" Then
            AddRegion()
        End If

        If hdWhichButton.Value = "Delete" Then
            DeleteRegion()
        End If

    End Sub
    Private Sub AddRegion()

        If txtBranchRegion.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You must input a Region"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try
            baseResponse = _bl.SaveRegion(txtBranchRegion.Text)
            If baseResponse.Success <> True Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True

            Else
                GetRegions()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
                txtBranchRegion.Text = String.Empty
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub DeleteRegion()
        Dim selectedValues = New List(Of Object)()
        selectedValues = Nothing
        Try

            selectedValues = grdRegion.GetSelectedFieldValues("region")
            If selectedValues.Count > 0 Then

                Dim region As String = selectedValues(selectedValues.Count - 1)
                baseResponse = _bl.DeleteRegion(region)

                If (baseResponse.Success = False) Then
                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = baseResponse.Message
                    dxPopUpError.ShowOnPageLoad = True
                    Exit Sub
                Else
                    GetRegions()
                    dxPopUpError.HeaderText = "Success"
                    lblError.Text = baseResponse.Message
                    dxPopUpError.ShowOnPageLoad = True
                End If

            Else

                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select region to delete"
                dxPopUpError.ShowOnPageLoad = True
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)

        End Try
    End Sub
    Private Sub GetRegions()
        Try
            _dt = _bl.GetRegions()
            Session("_RegionDS") = _dt
            grdRegion.DataBind()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Protected Sub grdRegion_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        If IsNothing(Session("_RegionDS")) Then
            grdRegion.DataSource = Nothing
        Else
            grdRegion.DataSource = Session("_RegionDS")
        End If
    End Sub

End Class