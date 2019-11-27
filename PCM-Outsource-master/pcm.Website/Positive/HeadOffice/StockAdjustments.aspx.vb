Imports Entities
Imports Entities.IBT_Out
Imports Newtonsoft.Json
Imports pcm.BusinessLayer
Public Class StockAdjustments
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Public Shared _IBT_Out As IBT_OutHOBL = New IBT_OutHOBL

    Public TaxGroup(11) As String
    Public TaxDescript(11) As String
    Public TaxRate(11) As String

    Public TaxRateString As String
    Public TaxGroupString As String
    Public TaxDescriptString As String
    Public Current_User As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                Response.Redirect("~/Intranet/Default.aspx")
            Else
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.StockAdjustments) Then
                    Response.Redirect("~/Intranet/Welcome.aspx")
                End If
            End If

        Else
            Session("username") = "DANIEL"
            Session("is_pcm_admin") = True
        End If

        Try
            BindTaxDropDown()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Private Sub StockAdjustments_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Private Sub BindTaxDropDown()

        Dim taxDropDownSource As GetTaxResponse
        Current_User = Session("username")

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

End Class