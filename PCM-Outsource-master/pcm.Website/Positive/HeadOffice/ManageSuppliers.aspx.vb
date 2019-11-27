Imports DevExpress.Web
Imports Entities.Manage
Imports pcm.BusinessLayer
Imports Entities

Public Class ManageSuppliers
    Inherits System.Web.UI.Page
    Dim baseResponse As New BaseResponse
    Dim _bl As New ManageHOBL
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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageSuppliers) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
        If Not IsPostBack Then
            cboSearchType.Items.Add("Account")
            cboSearchType.Items.Add("Company Name")
        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Populate" Then
            Populate()
        End If

        If hdWhichButton.Value = "Clear" Then
            ClearAll()
        End If

        If hdWhichButton.Value = "Save" Then
            Save()
        End If

        If hdWhichButton.Value = "SearchSupplier" Then
            Search()
        End If

        If hdWhichButton.Value = "SelectSupplier" Then
            SelectSupplier()
        End If
    End Sub
    Private Sub ManageSuppliers_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Sub Save()
        If txtSupplierCode.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You must input a Supplier Code"
            dxPopUpError.ShowOnPageLoad = True
        End If

        If txtSupplierName.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You must input a Supplier Name"
            dxPopUpError.ShowOnPageLoad = True
        End If

        Try
            Dim supplier As New SaveUpdateSupplier
            supplier.SupplierCode = txtSupplierCode.Text
            supplier.SupplierName = txtSupplierName.Text
            supplier.Telephone = txtTelephoneNumber.Text
            supplier.FAX = txtFAXNumber.Text
            supplier.Email = txtEmailAddress.Text
            supplier.TAX = txtTAXNumber.Text
            supplier.AddressLine1 = txtAddressLine1.Text
            supplier.AddressLine2 = txtAddressLine2.Text
            supplier.AddressLine3 = txtAddressLine3.Text
            supplier.AddressLine4 = txtAddressLine4.Text
            supplier.AddressLine5 = txtAddressLine5.Text
            supplier.IsBlocked = chkBlocked.Checked

            baseResponse = _bl.SaveUpdateSuppliers(supplier)
            If baseResponse.Success <> True Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True

            Else
                ClearAll()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub
    Private Sub ClearAll()
        txtSupplierCode.Text = String.Empty
        txtSupplierName.Text = String.Empty
        txtTelephoneNumber.Text = String.Empty
        txtFAXNumber.Text = String.Empty
        txtEmailAddress.Text = String.Empty
        txtTAXNumber.Text = String.Empty
        txtAddressLine1.Text = String.Empty
        txtAddressLine2.Text = String.Empty
        txtAddressLine3.Text = String.Empty
        txtAddressLine4.Text = String.Empty
        txtAddressLine5.Text = String.Empty
        chkBlocked.Checked = False
    End Sub
    Private Sub Search()
        Dim branches As DataTable
        Try

            branches = _bl.SearchSupplier(cboSearchType.Text, txtSearch.Text)
            If branches.Rows.Count > 0 Then
                lstSearch.Items.Clear()
                For Each drSCs As DataRow In branches.Rows
                    lstSearch.Items.Add(drSCs.Item("supplier_code") & " - " & drSCs.Item("supplier_name"))
                Next

            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No branch found"
                dxPopUpError.ShowOnPageLoad = True

            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Protected Sub SelectSupplier()
        Dim arrArray() As String
        arrArray = Split(lstSearch.Value, " - ")

        txtSupplierCode.Text = arrArray(0)
        LookupMain.ShowOnPageLoad = False
        Populate()

    End Sub
    Private Sub Populate()
        Dim dt As New DataTable

        If txtSupplierCode.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select the supplier code."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try
            dt = _bl.GetSupplierDetails(txtSupplierCode.Text.ToUpper)
            If dt.Rows.Count > 0 Then
                txtSupplierName.Text = dt.Rows(0)("supplier_name") & ""
                txtTelephoneNumber.Text = dt.Rows(0)("telephone_number") & ""
                txtFAXNumber.Text = dt.Rows(0)("fax_number") & ""
                txtEmailAddress.Text = dt.Rows(0)("email_address") & ""
                txtAddressLine1.Text = dt.Rows(0)("address_line_1") & ""
                txtAddressLine2.Text = dt.Rows(0)("address_line_2") & ""
                txtAddressLine3.Text = dt.Rows(0)("address_line_3") & ""
                txtAddressLine4.Text = dt.Rows(0)("address_line_4") & ""
                txtAddressLine5.Text = dt.Rows(0)("address_line_5") & ""
                txtTAXNumber.Text = dt.Rows(0)("tax_number") & ""
                chkBlocked.Checked = dt.Rows(0)("is_blocked") & ""

            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No data found"
                dxPopUpError.ShowOnPageLoad = True

            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
End Class