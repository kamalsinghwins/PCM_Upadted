Imports DevExpress.Web
Imports Entities
Imports Entities.Manage
Imports pcm.BusinessLayer
Public Class ManageBranches
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _bl As New ManageHOBL
    Dim ds As New DataSet
    Dim baseResponse As New BaseResponse
    Dim dt As New DataTable


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
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageBranches) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            PopulatePriceList()
            PopulateRegion()
            PopulateProvince()
            PopulateMallType()
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

        If hdWhichButton.Value = "SearchBranch" Then
            Search()
        End If

        If hdWhichButton.Value = "SelectBranch" Then
            SelectBranch
        End If

    End Sub
    Private Sub ManageBranches_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Private Sub PopulatePriceList()

        cboPriceList.Items.Clear()
        cboPriceList.Items.Add("1")
        cboPriceList.Items.Add("2")
        cboPriceList.Items.Add("3")

        cboBranchType.Items.Clear()
        cboBranchType.Items.Add("SHOES")
        cboBranchType.Items.Add("LADIES")
        cboBranchType.Items.Add("FACTORY")

        cboSearchType.Items.Clear()
        cboSearchType.Items.Add("Branch Code")
        cboSearchType.Items.Add("Branch Name")

    End Sub
    Private Sub ClearAll()
        txtBranchName.Text = String.Empty
        txtTelephoneNumber.Text = String.Empty
        txtFaxNumber.Text = String.Empty
        txtEmail.Text = String.Empty
        txtAddressLine1.Text = String.Empty
        txtAddressLine2.Text = String.Empty
        txtAddressLine3.Text = String.Empty
        txtAddressLine4.Text = String.Empty
        txtAddressLine5.Text = String.Empty
        txtTaxNumber.Text = String.Empty
        txtMerchantNumber.Text = String.Empty
        txtBranchCode.Text = String.Empty
        txtTradingHoursStart.Text = String.Empty
        txtTradingHoursEnd.Text = String.Empty
        txtMunicipality.Text = String.Empty
        txtStoreSquareMetres.Text = String.Empty
        chkBlocked.Checked = False
        cboProvince.SelectedIndex = -1
        cboPriceList.SelectedIndex = -1
        cboBranchType.SelectedIndex = -1
        cboRegion.SelectedIndex = -1
        txtLongitude.Text = String.Empty
        txtLatitude.Text = String.Empty
        txtCompanyName.Text = String.Empty
        cboTypeOfMall.SelectedIndex = -1
        txtUrl.Text = String.Empty
        txtBranchNameWeb.Text = String.Empty
        txtStoreStatus.Text = String.Empty
    End Sub
    Private Sub Populate()
        Dim dt As New DataTable
        If txtBranchCode.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select the branch code."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try
            dt = _bl.GetBranchDetails(Mid(txtBranchCode.Text.ToUpper, 1, 3))
            If dt.Rows.Count > 0 Then
                txtBranchName.Text = dt.Rows(0)("branch_name") & ""
                txtTelephoneNumber.Text = dt.Rows(0)("telephone_number") & ""
                txtFaxNumber.Text = dt.Rows(0)("fax_number") & ""
                txtEmail.Text = dt.Rows(0)("email_address") & ""
                txtAddressLine1.Text = dt.Rows(0)("address_line_1") & ""
                txtAddressLine2.Text = dt.Rows(0)("address_line_2") & ""
                txtAddressLine3.Text = dt.Rows(0)("address_line_3") & ""
                txtAddressLine4.Text = dt.Rows(0)("address_line_4") & ""
                txtAddressLine5.Text = dt.Rows(0)("address_line_5") & ""
                txtTaxNumber.Text = dt.Rows(0)("tax_number") & ""
                cboPriceList.Text = dt.Rows(0)("pricelevel") & ""
                chkBlocked.Checked = dt.Rows(0)("is_blocked") & ""
                cboBranchType.Text = dt.Rows(0)("branch_type") & ""
                txtMerchantNumber.Text = dt.Rows(0)("merchant_number") & ""
                cboRegion.Text = dt.Rows(0)("region") & ""
                cboProvince.Text = dt.Rows(0)("province") & ""
                txtMunicipality.Text = dt.Rows(0)("municipality") & ""
                txtStoreSquareMetres.Text = dt.Rows(0)("store_square_metres") & ""
                If IsDBNull(dt.Rows(0)("trading_hour_start")) Then
                    txtTradingHoursStart.Text = "00:00"
                Else
                    txtTradingHoursStart.Text = dt.Rows(0)("trading_hour_start").ToString()
                End If
                If IsDBNull(dt.Rows(0)("trading_hour_end")) Then
                    txtTradingHoursEnd.Text = "00:00"
                Else
                    txtTradingHoursEnd.Text = dt.Rows(0)("trading_hour_end").ToString()
                End If
                txtLatitude.Text = dt.Rows(0)("longitude") & ""
                txtLongitude.Text = dt.Rows(0)("latitude") & ""
                txtCompanyName.Text = dt.Rows(0)("company_name") & ""
                cboTypeOfMall.Text = dt.Rows(0)("mall_type") & ""
                txtUrl.Text = dt.Rows(0)("url") & ""
                txtBranchNameWeb.Text = dt.Rows(0)("branch_name_web") & ""
                txtStoreStatus.Text = dt.Rows(0)("store_status") & ""
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No data found"
                dxPopUpError.ShowOnPageLoad = True

            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    'Private Sub Delete()

    '    If txtBranchCode.Text = "" Then
    '        dxPopUpError.HeaderText = "Error"
    '        lblError.Text = "Please select the branch."
    '        dxPopUpError.ShowOnPageLoad = True
    '        Exit Sub
    '    End If

    '    Try

    '        baseResponse = _bl.DeleteBranch(Mid(txtBranchCode.Text.ToUpper, 1, 3), txtBranchName.Text)
    '        If baseResponse.Success <> True Then
    '            dxPopUpError.HeaderText = "Error"
    '            lblError.Text = baseResponse.Message
    '            dxPopUpError.ShowOnPageLoad = True

    '        Else
    '            ClearAll()
    '            dxPopUpError.HeaderText = "Success"
    '            lblError.Text = baseResponse.Message
    '            dxPopUpError.ShowOnPageLoad = True
    '        End If
    '    Catch ex As Exception
    '        _blErrorLogging.ErrorLogging(ex)
    '    End Try

    'End Sub
    Private Sub Save()

        If txtBranchCode.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You must input a Branch Code"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If Len(txtBranchCode.Text) <> 3 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Branch Codes must be 3 Characters in Length."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtBranchName.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "You must input a Branch Name"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        'Creating sequences with the branch code, so first character needs to be Alpha
        Dim tChar As String
        tChar = Mid$(txtBranchCode.Text, 1, 1)
        Dim isOK As Boolean = False
        If ((tChar >= "A") And (tChar <= "Z")) Or ((tChar >= "a") And (tChar <= "z")) Then
            isOK = True
        End If

        If isOK = False Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "The first Character of a Branch Code must be a letter. (A-Z)"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboBranchType.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Branch Type."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboPriceList.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Price List."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboProvince.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select Province."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtMunicipality.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter Municipality."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtStoreSquareMetres.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select Store Square Metres."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtTradingHoursStart.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter trading start hours."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtTradingHoursEnd.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter trading end hours."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        If txtLongitude.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter the longitude."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtLatitude.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter latitude."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboTypeOfMall.SelectedIndex = -1 Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select type of mall"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtCompanyName.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter company name"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtBranchNameWeb.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter branch name web"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        'If txtStoreStatus.Text = "" Then
        '    dxPopUpError.HeaderText = "Error"
        '    lblError.Text = "Please enter store status"
        '    dxPopUpError.ShowOnPageLoad = True
        '    Exit Sub
        'End If

        If CheckReserved(txtBranchCode.Text) = True Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = txtBranchCode.Text & " is Reserved. Please input an alternate Branch Code."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Try
            Dim branchDetails As New BranchDetails
            branchDetails.BranchCode = Mid(txtBranchCode.Text, 1, 3)
            branchDetails.BranchName = txtBranchName.Text
            branchDetails.TelephoneNumber = txtTelephoneNumber.Text
            branchDetails.Fax = txtFaxNumber.Text
            branchDetails.Email = txtEmail.Text
            branchDetails.TAX = txtTaxNumber.Text
            branchDetails.MerchantNumber = txtMerchantNumber.Text
            branchDetails.AddressLine1 = txtAddressLine1.Text
            branchDetails.AddressLine2 = txtAddressLine2.Text
            branchDetails.AddressLine3 = txtAddressLine3.Text
            branchDetails.AddressLine4 = txtAddressLine4.Text
            branchDetails.AddressLine5 = txtAddressLine5.Text
            branchDetails.Price = cboPriceList.Text
            branchDetails.Blocked = chkBlocked.Checked
            branchDetails.BranchType = cboBranchType.Text
            branchDetails.Region = cboRegion.Text
            branchDetails.Municipality = txtMunicipality.Text
            branchDetails.Province = cboProvince.Text
            branchDetails.StoreSquareMetres = txtStoreSquareMetres.Text
            branchDetails.TradingHourStart = txtTradingHoursStart.Text
            branchDetails.TradingHourEnd = txtTradingHoursEnd.Text
            branchDetails.Longitude = txtLongitude.Text
            branchDetails.Latitude = txtLatitude.Text
            branchDetails.CompanyName = txtCompanyName.Text
            branchDetails.TypeOfMall = cboTypeOfMall.Text
            branchDetails.URL = txtUrl.Text
            branchDetails.BranchNameWeb = txtBranchNameWeb.Text
            branchDetails.StoreStatus = txtStoreStatus.Text

            baseResponse = _bl.SaveBranch(branchDetails)
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
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Something went wrong"
            dxPopUpError.ShowOnPageLoad = True
        End Try

    End Sub
    Private Sub Search()
        Dim branches As DataTable
        Try

            branches = _bl.GetBranches(cboSearchType.Text, txtSearch.Text)
            If branches.Rows.Count > 0 Then
                lstSearch.Items.Clear()
                For Each drSCs As DataRow In branches.Rows
                    lstSearch.Items.Add(drSCs.Item("branch_code") & " - " & drSCs.Item("branch_name"))
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
    Protected Sub SelectBranch()
        Dim arrArray() As String
        arrArray = Split(lstSearch.Value, " - ")

        txtBranchCode.Text = arrArray(0)
        LookupMain.ShowOnPageLoad = False
        Populate()

    End Sub
    Public Function CheckReserved(ByVal strInput As String) As Boolean

        Dim tmpArr() As String
        Dim myX As String

        CheckReserved = False

        Dim lvTemp As ListBox = New ListBox

        myX = "ALL,ANALYSE,ANALYZE,AND,ANY,ARRAY,AS,ASC,ASYMMETRIC,BIGINT,BIT,BOOLEAN,BOTH,CASE,CAST,CHAR,CHARACTER,CHECK,COALESCE,COLLATE,COLUMN,CONSTRAINT,CREATE,CURRENT_DATE,CURRENT_ROLE,CURRENT_TIME,CURRENT_TIMESTAMP,CURRENT_USER,DEC,DECIMAL,DEFAULT,DEFERRABLE,DESC,DISTINCT,DO,ELSE,END,EXCEPT,EXISTS,EXTRACT,FALSE,FLOAT,FOR,FOREIGN,FROM,GRANT,GREATEST,GROUP,HAVING,IN,INITIALLY,INOUT,INT,INTEGER,INTERSECT,INTERVAL,INTO,LEADING,LEAST,LIMIT,LOCALTIME,LOCALTIMESTAMP,NATIONAL,NCHAR,NEW,NONE,NOT,NULL,NULLIF,NUMERIC,OFF,OFFSET,OLD,ON,ONLY,OR,ORDER,OUT,OVERLAY,PLACING,POSITION,PRECISION,PRIMARY,REAL,REFERENCES,RETURNING,ROW,SELECT,SESSION_USER,SETOF,SMALLINT,SOME,SUBSTRING,SYMMETRIC,TABLE,THEN,TIME,TIMESTAMP,TO,TRAILING,TREAT,TRIM,TRUE,UNION,UNIQUE,USER,USING,VALUES,VARCHAR,WHEN,WHERE,WITH,XMLATTRIBUTES,XMLCONCAT,XMLELEMENT,XMLFOREST,XMLPARSE,XMLPI,XMLROOT,XMLSERIALIZE"

        tmpArr = myX.Split(",")

        lvTemp.Items.Clear()

        For i As Integer = 0 To tmpArr.Length - 1
            lvTemp.Items.Add(tmpArr(i))
        Next

        Dim myTmpLoop As Long

        For myTmpLoop = 0 To lvTemp.Items.Count - 1
            If UCase(strInput) = lvTemp.Items(myTmpLoop).Text Then
                CheckReserved = True
                Exit Function
            End If
        Next


    End Function
    Private Sub PopulateRegion()
        Try
            dt = _bl.GetRegions()
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    cboRegion.Items.Add(dr("region"))
                Next

            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub PopulateProvince()
        Try
            dt = _bl.GetProvinces()
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    cboProvince.Items.Add(dr("province"))
                Next
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub PopulateMallType()
        cboTypeOfMall.Items.Add("Rural - CBD")
        cboTypeOfMall.Items.Add("Rural - Mall")
        cboTypeOfMall.Items.Add("Metropolitan - Strip mall/ CBD")
        cboTypeOfMall.Items.Add("Metropolitan - Convenience Centre")
        cboTypeOfMall.Items.Add("Metropolitan - Mall")
        cboTypeOfMall.Items.Add("Metropolitan - Regional")
        cboTypeOfMall.Items.Add("Metropolitan - Super Regional")
    End Sub
End Class