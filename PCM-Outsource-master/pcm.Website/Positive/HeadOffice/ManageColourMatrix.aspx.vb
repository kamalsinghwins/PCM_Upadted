Imports DevExpress.Web
Imports Entities
Imports Entities.Manage
Imports pcm.BusinessLayer

Public Class ManageColourMatrix
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _BLayer As New ManageHOBL()
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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ColourMatrix) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            PopulateSearchList()
        End If
    End Sub

    Private Sub ManageColourMatrix_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "Save" Then
            Save()
        End If

        If hdWhichButton.Value = "SearchColourCode" Then
            SearchColourCode()
        End If

        If hdWhichButton.Value = "SelectColourCode" Then
            SelectColourCode
        End If


    End Sub

    Private Sub PopulateSearchList()
        cboSearch.Items.Clear()
        cboSearch.Items.Add("Colour Code")
        cboSearch.Items.Add("Colour Description")

    End Sub

    Private Sub SearchColourCode()
        Dim colourMatrix As DataTable

        If cboSearch.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select Search Type"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        'If txtSearch.Text = "" Then
        '    dxPopUpError.HeaderText = "Error"
        '    lblError.Text = "Please enter Search Details"
        '    dxPopUpError.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        Try

            colourMatrix = _BLayer.GetColourCodes(cboSearch.Text, txtSearch.Text)
            If colourMatrix.Rows.Count > 0 Then
                lstSearch.Items.Clear()
                For Each drSCs As DataRow In colourMatrix.Rows
                    lstSearch.Items.Add(drSCs.Item("colour_code") & " - " & drSCs.Item("colour_description"))
                Next

            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No colour mode found"
                dxPopUpError.ShowOnPageLoad = True

            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Private Sub SelectColourCode()
        Dim arrArray() As String
        arrArray = Split(lstSearch.Value, " - ")
        txtColourCode.Text = arrArray(0)
        txtDescription.Text = arrArray(1)
        LookupMain.ShowOnPageLoad = False

    End Sub

    Private Sub Save()

        If txtColourCode.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You need to enter a Stationary Code."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtDescription.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You need to enter a Description."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        Try

            baseResponse = _BLayer.SaveColourCode(txtColourCode.Text.ToUpper, txtDescription.Text)
            If baseResponse.Success <> True Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Something Went wrong"
                dxPopUpError.ShowOnPageLoad = True

            Else
                Clear()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try


    End Sub

    Private Sub Clear()
        txtColourCode.Text = String.Empty
        txtDescription.Text = String.Empty
        txtColourCode.Focus()
    End Sub

    Private Sub Delete()

        If txtColourCode.ReadOnly = False Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You need to enter a Stationary Code."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        'If MsgBox("Are you sure you would like to delete Stationary code: " & txtCode.Text & "?",
        '         MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Question?") = MsgBoxResult.No Then
        '    Exit Sub
        'End If

        Try

            baseResponse = _BLayer.DeleteStationary(txtColourCode.Text)
            If baseResponse.Success = True Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            End If
            Clear()

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)

        End Try
    End Sub
End Class