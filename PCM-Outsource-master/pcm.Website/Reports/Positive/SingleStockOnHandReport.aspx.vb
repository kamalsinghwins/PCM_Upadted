Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress

Public Class SingleStockOnHandReport
    Inherits System.Web.UI.Page

    Dim RG As New clsRegality

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not HttpContext.Current.IsDebuggingEnabled Then
                If Session("username") = "" Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Default.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                    End If
                Else
                    If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.SingleSOH) Then
                        If Not IsCallback Then
                            Response.Redirect("~/Intranet/Welcome.aspx")
                        Else
                            ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                        End If
                    End If
                End If

            End If

            Dim _BLayer As New GeneralHOBL()
            Dim _dt As DataTable

            _dt = _BLayer.GetBranches

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboBranch.Items.Add(_dt(i)("branch_code") & " - " & _dt(i)("branch_name"))
            Next

        End If
    End Sub

    Protected Sub grdStockcodeSearch_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        grdStockcodeSearch.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim _Blayer As New StockcodesHOBL()

        Dim _tmpTXT As ASPxTextBox

        _tmpTXT = pcMain.FindControl("txtStockcodeSearch")

        Dim data As DataTable = _Blayer.GetStockcode("Generated Code", _tmpTXT.Text, False)

        gridView.KeyFieldName = "stockcode" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        grdStockcodeSearch.EndUpdate()

    End Sub

    Private Sub StockcodeManager_Init(sender As Object, e As EventArgs) Handles Me.Init

        Dim url As String = Request.Url.AbsoluteUri

        'If url.Contains("localhost") Then
        Session("current_company") = "010"
        'End If

        Me.Form.DefaultButton = cmdFetch.UniqueID

    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        'Search
        If hdWhichButton.Value = "Search" Then
            'ClearScreen(False)
            grdStockcodeSearch.DataBind()
        End If

        'Select "searched" code
        If hdWhichButton.Value = "Select" Then

            Dim selectedValues = New List(Of Object)()

            selectedValues = Nothing

            selectedValues = grdStockcodeSearch.GetSelectedFieldValues("stockcode")

            If selectedValues.Count > 0 Then
                txtStockcode.Text = selectedValues(selectedValues.Count - 1)

            End If

            ' No reason to be read onlu
            'txtStockcode.ReadOnly = True

            pcMain.ShowOnPageLoad = False

        End If

        If hdWhichButton.Value = "Fetch" Then



            Dim _Blayer As New StockcodesHOBL()

            Dim data As DataTable = _Blayer.GetSingleStockOnHand(txtStockcode.Text, Mid(cboBranch.Text, 1, 3))

            Dim _tmpLBL As ASPxLabel
            _tmpLBL = dxPopUpError.FindControl("lblError")

            Dim tmpQtyOnHand As String = data.Rows(data.Rows.Count - 1)("qty_on_hand")

            _tmpLBL.Text = "The Stock on Hand for " & txtStockcode.Text & " at Branch " & Mid(cboBranch.Text, 1, 3) & " is: " & RG.Numb(tmpQtyOnHand)

            'ClearScreen()

            'dxPopUpError.ShowOnPageLoad = True


            dxPopUpError.ShowOnPageLoad = True

        End If

        If hdWhichButton.Value = "Clear" Then
            ClearScreen(True)
        End If

    End Sub

    Private Sub ClearScreen(Optional ClearSearch As Boolean = True)

        txtStockcode.ReadOnly = False

        txtStockcode.Text = ""

        cboBranch.Text = ""

        Dim _dt As DataTable

        If ClearSearch = True Then
            _dt = Nothing
            grdStockcodeSearch.DataSource = _dt
            grdStockcodeSearch.DataBind()

            ASPxEdit.ClearEditorsInContainer(pcMain)
        End If

    End Sub

    Private Sub SingleStockOnHandReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
End Class