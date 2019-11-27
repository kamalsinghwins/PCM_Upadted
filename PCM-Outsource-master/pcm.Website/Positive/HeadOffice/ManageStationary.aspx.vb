Imports DevExpress.Web
Imports Entities
Imports Entities.Manage
Imports pcm.BusinessLayer
Public Class ManageStationary
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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageStationary) Then
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

    Private Sub ManageBranches_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdWhichButton.Value = "Save" Then
            Save()
        End If

        If hdWhichButton.Value = "SearchStationary" Then
            SearchCode()
        End If

        If hdWhichButton.Value = "SelectStationary" Then
            SelectCode()
        End If


    End Sub

    Private Sub PopulateSearchList()
        cboStationaryType.Items.Clear()
        cboStationaryType.Items.Add("Stationary Code")
        cboStationaryType.Items.Add("Stationary Description")

    End Sub

    Private Sub SearchCode()
        Dim stationaryCodes As DataTable

        If cboStationaryType.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select Search Type"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtSearch.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter Search Details"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try

            stationaryCodes = _BLayer.GetStationaryCodes(cboStationaryType.Text, txtSearch.Text)
            If stationaryCodes.Rows.Count > 0 Then
                lstSearch.Items.Clear()
                For Each drSCs As DataRow In stationaryCodes.Rows
                    lstSearch.Items.Add(drSCs.Item("stationary_code") & " - " & drSCs.Item("stationary_description"))
                Next

            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No stationary found"
                dxPopUpError.ShowOnPageLoad = True

            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Private Sub SelectCode()
        Dim arrArray() As String
        arrArray = Split(lstSearch.Value, " - ")
        txtStationaryCode.Text = arrArray(0)
        LookupMain.ShowOnPageLoad = False
        Populate()

    End Sub

    Private Sub Populate()
        Dim dt As New DataTable

        If txtStationaryCode.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select stationary code."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try
            dt = _BLayer.GetStationaryDetails(txtStationaryCode.Text.ToUpper)

            If dt.Rows.Count > 0 Then
                txtDescription.Text = dt.Rows(0)("stationary_description") & ""
                txtStationaryCode.ClientEnabled = False
            End If

            txtDescription.Focus()


        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Private Sub Save()

        If txtStationaryCode.Text = "" Then
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

        Dim Read As Boolean

        If txtStationaryCode.ClientEnabled = False Then
            Read = True
        Else
            Read = False

        End If

        Try

            baseResponse = _BLayer.SaveStationary(txtStationaryCode.Text.ToUpper, txtDescription.Text, Read)
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
        txtStationaryCode.ReadOnly = False
        txtStationaryCode.Text = String.Empty
        txtDescription.Text = String.Empty
        txtStationaryCode.Focus()
    End Sub

    Private Sub Delete()

        If txtStationaryCode.ReadOnly = False Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You need to enter a Stationary Code."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        Try

            baseResponse = _BLayer.DeleteStationary(txtStationaryCode.Text)
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