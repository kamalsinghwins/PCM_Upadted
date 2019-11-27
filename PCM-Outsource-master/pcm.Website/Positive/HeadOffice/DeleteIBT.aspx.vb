Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Public Class DeleteIBT
    Inherits System.Web.UI.Page
    Dim reports As New ReportsBusinessLayer
    Dim ds As New DataSet
    Dim _blErrorLogging As New ErrorLogBL
    Dim _blayer As New DeleteIBTHOBL




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
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.DeleteIBT) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            Try
                ds = reports.GetBranches()
                If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    For Each drBranch As DataRow In ds.Tables(0).Rows
                        cboSendBranch.Items.Add(drBranch.Item("branch_code") & " - " & drBranch.Item("branch_name") & "")
                        cboReceieveBranch.Items.Add(drBranch.Item("branch_code") & " - " & drBranch.Item("branch_name") & "")

                    Next
                End If

                cboReason.Items.Clear()
                cboReason.Text = ""
                cboReason.Items.Add("Duplication")
                cboReason.Items.Add("Store Closed / Converted")
                cboReason.Items.Add("Store Overstocked")
                cboReason.Items.Add("Stock Lost In Warehouse")
                cboReason.Items.Add("Stock Lost In Transit")
            Catch ex As Exception
                _blErrorLogging.ErrorLogging(ex)
            End Try
        End If
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Delete" Then
            DeleteIBT
        End If

        If hdWhichButton.Value = "Clear" Then
            ClearScreen()
        End If

    End Sub
    Private Sub DeleteIBT_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"

    End Sub

    Private Sub DeleteIBT()
        Dim username As String = String.Empty
        If Session("username") = "" Then
            ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
            Exit Sub
        Else
            username = Session("username")
        End If

        If cboSendBranch.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select sending branch."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboReceieveBranch.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select receieving branch."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtIbtNumber.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter IBT Number."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtAuthorisedBy.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter the authorised name ."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboReason.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select reason."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try
            Dim baseResponse As New BaseResponse

            baseResponse = _blayer.DeleteIBT(Mid(cboSendBranch.Text.ToUpper, 1, 3), Mid(cboReceieveBranch.Text.ToUpper, 1, 3), cboReason.Text, txtAuthorisedBy.Text, txtIbtNumber.Text, username)
            If baseResponse.Success = False Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True

            Else
                ClearScreen()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub


    Private Sub ClearScreen()
        txtIbtNumber.Text = String.Empty
        cboReason.SelectedIndex = -1
        txtAuthorisedBy.Text = String.Empty
        cboReceieveBranch.SelectedIndex = -1
        cboSendBranch.SelectedIndex = -1
    End Sub
End Class