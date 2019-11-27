Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Public Class ManageTills
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _bl As New ManageHOBL
    Dim dt As New DataTable
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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageTills) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            dt = _bl.GetAllBranches()
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each drBranch As DataRow In dt.Rows
                    cboBranch.Items.Add(drBranch.Item("branch_code") & " - " & drBranch.Item("branch_name") & "")
                Next
            End If
        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "Till" Then
            PopulateTills()
        End If

        If hdWhichButton.Value = "Delete" Then
            Delete()
        End If

        If hdWhichButton.Value = "Save" Then
            Save()
        End If

    End Sub
    Private Sub ManageTills_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Sub PopulateTills()

        cboTill.Items.Clear()
        cboTill.Text = ""

        Dim BranchCode As String
        BranchCode = Mid(cboBranch.Text, 1, 3)

        Try
            dt = _bl.GetTills(BranchCode)
            For Each dr As DataRow In dt.Rows
                If dr("till_number") <> "" Then
                    cboTill.Items.Add(dr("till_number") & "")
                End If

            Next
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub
    Private Sub ClearAll()
        cboBranch.SelectedIndex = -1
        cboTill.SelectedIndex = -1
    End Sub
    Private Sub Delete()
        Dim Till As String
        Dim BranchCode As String
        BranchCode = Mid(cboBranch.Text, 1, 3)
        Till = cboTill.Text

        If BranchCode.ToUpper = "HHH" And cboTill.Text = "1" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You cannot delete Till 1 for the Head Office branch."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try
            baseResponse = _bl.DeleteTill(BranchCode, Till)
            If baseResponse.Success = True Then
                dxPopUpError.HeaderText = "Success"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
                ClearAll()
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

    Private Sub Save()

        Dim Till As String
        Dim BranchCode As String
        BranchCode = Mid(cboBranch.Text, 1, 3)
        Till = cboTill.Text

        If cboBranch.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a valid shop before proceeding."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If
        If cboTill.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please input a new till number before proceeding"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If
        If Not IsNumeric(cboTill.Text) Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Till numbers must be numeric"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try
            baseResponse = _bl.SaveTill(BranchCode, Till)
            dxPopUpError.HeaderText = IIf(baseResponse.Success = True, "Success", "Error")
            lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            ClearAll()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

End Class