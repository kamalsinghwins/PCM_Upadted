Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress.Web

Public Class ManageMunicipality
    Inherits System.Web.UI.Page
    Dim dt As New DataTable
    Dim bl As New ManageHOBL
    Dim _municipality As New ManageHOBL
    Dim _blErrorLogging As New ErrorLogBL

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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageMunicipalities) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            GetBranches()
        End If
    End Sub
    Private Sub ManageMunicipality_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Update" Then
            UpdateMunicipality()
        End If
        If hdWhichButton.Value = "Save" Then
            AddMunicipality()
        End If
    End Sub

    Private Sub GetBranches()
        dt = _municipality.GetAllBranches()
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                cboBranch.Items.Add(dr("branch_code") & " - " & dr("branch_name"))
            Next
        End If
        Session("Municipality") = dt
        grdMunicipality.DataBind()

    End Sub
    Private Sub AddMunicipality()
        Dim baseResponse As New BaseResponse
        If txtMunicipality.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You must input a Municipality"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If
        If cboBranch.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You must select a branch"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If
        Try
            baseResponse = _municipality.SaveMunicipality(txtMunicipality.Text, cboBranch.Text)
            If baseResponse.Success <> True Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True

            Else
                GetBranches()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = BaseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
                txtMunicipality.Text = String.Empty
                cboBranch.SelectedIndex = -1
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub UpdateMunicipality()
        Dim baseResponse As New BaseResponse
        Dim selectedValues = New List(Of Object)()
        selectedValues = Nothing
        Try

            selectedValues = grdMunicipality.GetSelectedFieldValues("branch_code")
            If selectedValues.Count > 0 Then

                Dim branchcode As String = selectedValues(selectedValues.Count - 1)
                baseResponse = _municipality.UpdateMunicipality(branchcode)

                If (baseResponse.Success = False) Then
                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = baseResponse.Message
                    dxPopUpError.ShowOnPageLoad = True
                    Exit Sub
                Else
                    GetBranches()
                    dxPopUpError.HeaderText = "Success"
                    lblError.Text = baseResponse.Message
                    dxPopUpError.ShowOnPageLoad = True
                End If

            Else

                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select municipality to delete"
                dxPopUpError.ShowOnPageLoad = True
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)

        End Try
    End Sub
    Protected Sub grdMunicipality_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        If IsNothing(Session("Municipality")) Then
            grdMunicipality.DataSource = Nothing
        Else
            grdMunicipality.DataSource = Session("Municipality")
        End If
    End Sub
End Class