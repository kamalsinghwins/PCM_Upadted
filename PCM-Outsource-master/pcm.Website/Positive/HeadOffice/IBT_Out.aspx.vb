Imports Entities
Imports Entities.IBT_Out
Imports Newtonsoft.Json
Imports pcm.BusinessLayer

Public Class IBT_Out
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL


    Public Shared _IBT_Out As IBT_OutHOBL = New IBT_OutHOBL
    'System Permissions
    '=======================================================================
    Public Current_Company As String
    Public Current_Company_Code As String
    Public AllowNegativeStockBool As Boolean            'Allow Negative Stock Qtys
    Public CurrencySymbol As String                     'Currency Symbol
    Public RoundingValueInt As Integer                  'Value to round down by
    Public MaxDiscountPerLineInt As Integer             'Maximum Discount per line item
    Public MaxDiscountPerDocInt As Integer              'Maximum Discount per transaction
    Public DefaultSalesTaxInt As Integer                'Default Sales Tax Group
    Public DefaultPurchaseTaxInt As Integer             'Default Purchase Tax Group
    Public ImageServer As String                        'Location of Image Server
    '=======================================================================
    '=======================================================================

    'Branch Details
    Public Branch_Name As String
    Public Shared Current_Branch_Code As String = "HHH"   'Branch from Login
    Public Branch_Address_Line_1 As String
    Public Branch_Address_Line_2 As String
    Public Branch_Address_Line_3 As String
    Public Branch_Address_Line_4 As String
    Public Branch_Address_Line_5 As String
    Public Branch_Telephone_Number As String
    Public Branch_Fax_Number As String
    Public Branch_EMail_Address As String
    Public Branch_Tax_Number As String
    Public Branch_PriceLevel As String
    '=======================================================================

    '=======================================================================
    'Tax Groups
    Public TaxGroup(11) As String
    Public TaxDescript(11) As String
    Public TaxRate(11) As String

    Public TaxRateString As String
    Public TaxGroupString As String
    Public TaxDescriptString As String
    Public Current_User As String

    '=======================================================================
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then

                Response.Redirect("~/Intranet/Default.aspx")
            Else
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.IBTOUT) Then
                    Response.Redirect("~/Intranet/Welcome.aspx")
                End If
            End If

        Else
            Session("username") = "DANIEL"
            Session("is_pcm_admin") = True
        End If

        Try
            GetCompanySettings()
            BindTaxDropDown()
            GetBranchSettings(Current_Branch_Code)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub IBT_Out_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Private Sub BindTaxDropDown()
        Dim taxDropDownSource As GetTaxResponse
        taxDropDownSource = _IBT_Out.GetTaxGroups()
        dropDownTax.DataSource = taxDropDownSource.dt
        dropDownTax.DataTextField = "TaxInfo"
        dropDownTax.DataValueField = "tax_group"
        dropDownTax.DataBind()
        dropDownTax.Items.Insert(0, New ListItem("", ""))


        For Each dr As DataRow In taxDropDownSource.dt.Rows
            If Val(dr("tax_group")) <> 0 Then
                TaxGroup(Val(dr("tax_group"))) = dr("tax_group") & ""
                TaxDescript(Val(dr("tax_group"))) = dr("tax_description") & ""
                TaxRate(Val(dr("tax_group"))) = dr("tax_value") & ""
            End If
        Next

        TaxRateString = JsonConvert.SerializeObject(TaxRate)
        TaxGroupString = JsonConvert.SerializeObject(TaxGroup)
        TaxDescriptString = JsonConvert.SerializeObject(TaxDescript)

    End Sub

    Public Function GetCompanySettings() As Boolean
        Dim getCompanyResponse As New GetCompanyResponse
        Current_User = Session("username")
        getCompanyResponse = _IBT_Out.GetCompanySettings()
        If getCompanyResponse.Success = True Then
            Current_Company = getCompanyResponse.dt.Rows(0)("company_name")
            Current_Company_Code = getCompanyResponse.dt.Rows(0)("company_code")
            AllowNegativeStockBool = getCompanyResponse.dt.Rows(0)("allow_negative_stock") & ""
            CurrencySymbol = getCompanyResponse.dt.Rows(0)("currency_symbol") & ""
            RoundingValueInt = Val(getCompanyResponse.dt.Rows(0)("round_down_to_closest") & "")
            MaxDiscountPerLineInt = Val(getCompanyResponse.dt.Rows(0)("maximum_discount_per_row") & "")
            MaxDiscountPerDocInt = Val(getCompanyResponse.dt.Rows(0)("maximum_discount_per_document") & "")
            DefaultSalesTaxInt = Val(getCompanyResponse.dt.Rows(0)("default_sales_tax") & "")
            DefaultPurchaseTaxInt = Val(getCompanyResponse.dt.Rows(0)("default_purchase_tax") & "")
            ImageServer = getCompanyResponse.dt.Rows(0)("image_server") & ""


        End If



    End Function

    Public Function GetBranchSettings(ByVal BranchCode As String) As Boolean
        Dim getBranchSettingsResponse As New GetBranchSettingsResponse
        getBranchSettingsResponse = _IBT_Out.GetBranchSettings(Current_Branch_Code)

        If getBranchSettingsResponse.Success = True Then
            Branch_Name = getBranchSettingsResponse.dt.Rows(0)("branch_name") & ""
            Branch_Address_Line_1 = getBranchSettingsResponse.dt.Rows(0)("address_line_1") & ""
            Branch_Address_Line_2 = getBranchSettingsResponse.dt.Rows(0)("address_line_2") & ""
            Branch_Address_Line_3 = getBranchSettingsResponse.dt.Rows(0)("address_line_3") & ""
            Branch_Address_Line_4 = getBranchSettingsResponse.dt.Rows(0)("address_line_4") & ""
            Branch_Address_Line_5 = getBranchSettingsResponse.dt.Rows(0)("address_line_5") & ""
            Branch_Telephone_Number = getBranchSettingsResponse.dt.Rows(0)("telephone_number") & ""
            Branch_Fax_Number = getBranchSettingsResponse.dt.Rows(0)("fax_number") & ""
            Branch_EMail_Address = getBranchSettingsResponse.dt.Rows(0)("email_address") & ""
            Branch_Tax_Number = getBranchSettingsResponse.dt.Rows(0)("tax_number") & ""
            Branch_PriceLevel = getBranchSettingsResponse.dt.Rows(0)("pricelevel") & ""
        End If


    End Function


End Class