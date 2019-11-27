Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports System.IO
Imports Entities.LoyaltyDiscount

Public Class LoyaltyDiscount
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _loyaltyDiscountHOBL As New LoyaltyDiscountHOBL
    Private _discountList As DataSet = Nothing


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.LoyaltyDiscount) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("username") = "DANIEL"
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
            GetLoyaltyDiscount()
        End If
    End Sub

    Private Sub LoyaltyDiscount_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)


        If hdWhichButton.Value = "CreateDiscount" Then
                SaveLoyaltyDiscount()
            End If

            If hdWhichButton.Value = "OpenAddLoyaltyDiscountPopup" Then
                OpenCreateDiscountPopup()
            End If

            If hdWhichButton.Value = "OpenEditLoyaltyDiscountPopup" Then
                OpenEditLoyaltyDiscountPopup()
            End If

        If hdWhichButton.Value = "Run" Then
            Run()
        End If

    End Sub

    Private Sub GetLoyaltyDiscount()
        _discountList = _loyaltyDiscountHOBL.GetDiscountList()
        Session("_LoyaltyDiscountDS") = _discountList
        grdLoyaltyDiscount.DataBind()
    End Sub

    Protected Sub OpenCreateDiscountPopup()
        txtDiscountName.Text = String.Empty
        txtDiscountDate.Text = Format(Now, "yyyy-MM-dd")
        chkIsActive.Checked = False
        txtDiscountPercentage.Text = String.Empty
        HDLoyaltyDiscountName.Value = ""

        DiscountPopup.HeaderText = "Add Discount"
        cmdDiscount.Text = "Create"
        DiscountPopup.ShowOnPageLoad = True
    End Sub

    Protected Sub OpenEditLoyaltyDiscountPopup()
        Dim selectedValues = New List(Of Object)()

        selectedValues = Nothing

        Try

            selectedValues = grdLoyaltyDiscount.GetSelectedFieldValues("discount_name")

            If selectedValues.Count > 0 Then
                Dim strDiscountName As String = selectedValues(selectedValues.Count - 1)
                Dim discountResponse As New GetDiscountResponse


                discountResponse = _loyaltyDiscountHOBL.GetSelectedDiscountDetails(strDiscountName)

                If (discountResponse.Success = False) Then
                    dxPopUpError.HeaderText = "Error"
                    lblError.Text = discountResponse.Message
                    dxPopUpError.ShowOnPageLoad = True
                    Exit Sub
                End If

                Dim _dt As DataTable = discountResponse.GetSelectedDiscountResponse

                If _dt.Rows.Count = 0 Then
                    Exit Sub
                End If


                txtDiscountName.Text = _dt.Rows(0)("discount_name")
                txtDiscountPercentage.Text = _dt.Rows(0)("discount_percentage")
                chkIsActive.Checked = _dt.Rows(0)("is_enabled")
                txtDiscountDate.Text = Format(_dt.Rows(0)("discount_date"), "yyyy-MM-dd")
                HDLoyaltyDiscountName.Value = _dt.Rows(_dt.Rows.Count - 1)("guid")


                DiscountPopup.HeaderText = "Update Discount"
                cmdDiscount.Text = "Update"
                DiscountPopup.ShowOnPageLoad = True

            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Please select record to perform action"
                dxPopUpError.ShowOnPageLoad = True
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Protected Sub grdLoyaltyDiscount_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        If IsNothing(Session("_LoyaltyDiscountDS")) Then
            grdLoyaltyDiscount.DataSource = Nothing
        Else
            grdLoyaltyDiscount.DataSource = Session("_LoyaltyDiscountDS")
        End If
    End Sub

    Private Sub SaveLoyaltyDiscount()
        Dim baseResponse As New BaseResponse

        If txtDiscountName.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter Discount Name."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtDiscountDate.Text = "" Then

            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select Discount Date."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtDiscountPercentage.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please enter Discount Percentage."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If


        Dim loyaltyDiscount As New SaveDiscount
        loyaltyDiscount.LoyaltyDiscountId = HDLoyaltyDiscountName.Value
        loyaltyDiscount.DiscountName = txtDiscountName.Text
        loyaltyDiscount.DiscountDate = txtDiscountDate.Text
        loyaltyDiscount.DiscountPercentage = txtDiscountPercentage.Text
        loyaltyDiscount.IsActive = chkIsActive.Checked
        Try

            baseResponse = _loyaltyDiscountHOBL.SaveDiscount(loyaltyDiscount)

            If baseResponse.Success = False Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            Else

                GetLoyaltyDiscount()
                DiscountPopup.ShowOnPageLoad = False
                dxPopUpError.HeaderText = "Success"
                lblError.Text = baseResponse.Message
                dxPopUpError.ShowOnPageLoad = True
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

    Public Sub Run()
        Try
            Session.Remove("_LoyaltyDiscountDS")
            _discountList = _loyaltyDiscountHOBL.GetLoyaltyDiscounts(txtFromDate.Text, txtToDate.Text)
            Session("_LoyaltyDiscountDS") = _discountList
            grdLoyaltyDiscount.DataBind()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)

        End Try
    End Sub
    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub

    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsxToResponse()
    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
        Exporter.WriteCsvToResponse()
    End Sub

End Class