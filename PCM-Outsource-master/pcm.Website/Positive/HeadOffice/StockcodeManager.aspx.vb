Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports Newtonsoft.Json
Imports DevExpress

Public Class StockcodeManager
    Inherits System.Web.UI.Page

    Private _ErrorLogBL As New ErrorLogBL
    Dim RG As New clsRegality
    Dim _blErrorLogging As New ErrorLogBL


    Private Sub StockcodeManager_Init(sender As Object, e As EventArgs) Handles Me.Init

        'grdPrices.DataSource = ReturnTable()
        'grdPrices.DataBind()

        Dim url As String = Request.Url.AbsoluteUri


        If url.Contains("localhost") Then
            Session("current_company") = "010"
        End If

        Me.Form.DefaultButton = cmdSubmit.UniqueID

        Page.Server.ScriptTimeout = 300

        'If (Not IsCallback) Then
        Dim _dt As New DataTable
        Dim _dtCompany As New DataTable
        Dim _BLayer As New GeneralHOBL()

        Dim tmpAvailable As ASPxListBox
        tmpAvailable = navGrid.Groups.FindByName("Grids").Items(0).FindControl("lstAvailable")

        Dim tmpSelected As ASPxListBox
        tmpSelected = navGrid.Groups.FindByName("Grids").Items(0).FindControl("lstSelected")

        'cboPurchaseTax.Items.Clear()
        'cboPurchaseTax.DataSource = _dt


        'cboSalesTax.Items.Clear()
        'cboSalesTax.DataSource = _dt
        'cboSalesTax.DataBind()

        If (Not IsPostBack) Then
            _dt = _BLayer.GetTaxGroups

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboSalesTax.Items.Add(_dt(i)("taxgroups"))
                cboPurchaseTax.Items.Add(_dt(i)("taxgroups"))
            Next

            _dtCompany = _BLayer.GetCompanySettings
            cboPurchaseTax.SelectedIndex = Val(_dtCompany.Rows(0)("default_purchase_tax")) - 1
            cboSalesTax.SelectedIndex = Val(_dtCompany.Rows(0)("default_sales_tax")) - 1

            Dim _BLayerStock As New StockcodesHOBL()
            _dt = _BLayerStock.GetSizeGrids

            Dim tmpSize As ASPxComboBox
            tmpSize = navGrid.Groups.FindByName("Grids").Items(0).FindControl("cboSizeGrid")
            For i As Integer = 0 To _dt.Rows.Count - 1
                tmpSize.Items.Add(_dt(i)("grid_number") & " - " & _dt(i)("grid_description"))
            Next

            _dt = _BLayerStock.GetColourGrids

            tmpAvailable.Items.Clear()
            For i As Integer = 0 To _dt.Rows.Count - 1
                tmpAvailable.Items.Add(_dt(i)("colour_code") & " - " & _dt(i)("colour_description"))
            Next

            hdClient.Set("colourstring", "")
            hdClient.Set("isforupdate", "false")
            hdClient.Set("originalcolourstring", "")
            hdClient.Set("originalbarcode", "")
            hdClient.Set("isgeneratedcode", "")

            _dt = _BLayerStock.GetCategoryList("1", "")

            cboCategory1.DataSource = _dt
            cboCategory1.DataBind()

            _dt = _BLayerStock.GetCategoryList("2", "")

            cboCategory2.DataSource = _dt
            cboCategory2.DataBind()

            _dt = _BLayerStock.GetCategoryList("3", "")

            cboCategory3.DataSource = _dt
            cboCategory3.DataBind()


            'navGrid.Enabled = True
        End If

        tmpAvailable.Attributes("ondblclick") = "AddToSelectedDbl();"
        tmpSelected.Attributes("ondblclick") = "AddToAvailableDbl();"

        '_tmpCBO.Text = "Master Code"
        Dim _tmpCBO As ASPxComboBox
        _tmpCBO = pcMain.FindControl("cboSearchType")
        _tmpCBO.Items.Clear()

        _tmpCBO.Items.Add("Master Code")
        _tmpCBO.Items.Add("Generated Code")
        _tmpCBO.Items.Add("Barcode")
        _tmpCBO.Items.Add("Description")

        grdPrices.DataSource = ReturnTable()
        grdPrices.DataBind()
        'End If

        cmdDelete.Enabled = False

        If Not IsPostBack Then


        End If

    End Sub


    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        Dim _tmpLBL As ASPxLabel
        Dim IsAdmin As Boolean = False
        Dim email As String = String.Empty
        Dim Username As String = String.Empty
        Dim IPAddress As String = String.Empty

        _tmpLBL = dxPopUpError.FindControl("lblError")

        'Search
        If hdWhichButton.Value = "Search" Then
            ClearScreen(False)
            grdStockcodeSearch.DataBind()
        End If

        'Select "searched" code
        If hdWhichButton.Value = "Select" Then

            Dim selectedValues = New List(Of Object)()

            selectedValues = Nothing

            selectedValues = grdStockcodeSearch.GetSelectedFieldValues("stockcode")

            If selectedValues.Count > 0 Then
                Dim _dt As New DataTable
                Dim _BLayer As New StockcodesHOBL()

                If chkMasterCode.Checked Then
                    _dt = _BLayer.GetMasterCodeInfo(selectedValues(selectedValues.Count - 1))
                    If _dt.Rows.Count = 0 Then
                        Exit Sub
                    End If
                    txtStockcode.Text = _dt.Rows(_dt.Rows.Count - 1)("master_code")
                    pnlWarning.Visible = True

                Else
                    _dt = _BLayer.GetGeneratedCodeInfo(selectedValues(selectedValues.Count - 1))
                    If _dt.Rows.Count = 0 Then
                        Exit Sub
                    End If
                    txtStockcode.Text = _dt.Rows(_dt.Rows.Count - 1)("generated_code")
                    pnlWarning.Visible = False
                    hdClient.Set("isgeneratedcode", "true")

                    'User cannot update size / colour
                    navGrid.Enabled = False
                End If

                txtBarcode.Text = _dt.Rows(_dt.Rows.Count - 1)("sku_number")
                hdClient("originalbarcode") = _dt.Rows(_dt.Rows.Count - 1)("sku_number")

                txtDescription.Text = _dt.Rows(_dt.Rows.Count - 1)("description")
                cboCategory1.Text = _dt.Rows(_dt.Rows.Count - 1)("category_1")
                cboCategory2.Text = _dt.Rows(_dt.Rows.Count - 1)("category_2")
                cboCategory3.Text = _dt.Rows(_dt.Rows.Count - 1)("category_3")
                drpSupplier.Text = _dt.Rows(_dt.Rows.Count - 1)("supplier")
                txtSupplierItemCode.Text = _dt.Rows(_dt.Rows.Count - 1)("suppliers_code")


                'Options navbar
                Dim MinimumQTY As ASPxTextBox
                MinimumQTY = navOptions.Groups.FindByName("Options").Items(0).FindControl("txtMinimumLevel")

                Dim chkIsBlocked As ASPxCheckBox
                Dim chkNotDiscountable As ASPxCheckBox
                Dim chkServiceItem As ASPxCheckBox

                chkIsBlocked = navOptions.Groups.FindByName("Options").Items(0).FindControl("chkIsBlocked")
                chkNotDiscountable = navOptions.Groups.FindByName("Options").Items(0).FindControl("chkNotDiscountable")
                chkServiceItem = navOptions.Groups.FindByName("Options").Items(0).FindControl("chkServiceItem")

                MinimumQTY.Text = RG.Numb(_dt.Rows(_dt.Rows.Count - 1)("minimum_stock_level"))

                chkIsBlocked.Checked = _dt.Rows(_dt.Rows.Count - 1)("is_blocked")
                chkNotDiscountable.Checked = _dt.Rows(_dt.Rows.Count - 1)("is_not_discountable")
                chkServiceItem.Checked = _dt.Rows(_dt.Rows.Count - 1)("is_service_item")

                cboPurchaseTax.SelectedIndex = Val(_dt.Rows(_dt.Rows.Count - 1)("purchase_tax_group")) - 1
                cboSalesTax.SelectedIndex = Val(_dt.Rows(_dt.Rows.Count - 1)("sales_tax_group")) - 1

                txtCostExclusive.Text = RG.Numb(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost"))
                txtAverageCost.Text = RG.Numb(_dt.Rows(_dt.Rows.Count - 1)("cost_price"))


                Dim PurchaseTax As Double
                Dim SalesTax As Double

                Dim p() As String = Split(cboPurchaseTax.Text, "(")
                Dim p1() As String = Split(p(1), ")")
                PurchaseTax = Val(p1(0))

                Dim s() As String = Split(cboSalesTax.Text, "(")
                Dim s1() As String = Split(s(1), ")")
                SalesTax = Val(s1(0))

                txtCostPriceIncl.Text = RG.Numb(Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost")) * (1 + PurchaseTax / 100))

                Dim markup(3) As Double
                Dim gp(3) As Double
                Dim sellin(3) As Double

                markup(1) = RG.Numb((Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_1") - Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost"))) / Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost")) * 100))
                markup(2) = RG.Numb((Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_2") - Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost"))) / Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost")) * 100))
                markup(3) = RG.Numb((Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_3") - Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost"))) / Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost")) * 100))

                gp(1) = RG.Numb((Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_1") - Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost"))) / Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_1")) * 100))
                gp(2) = RG.Numb((Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_2") - Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost"))) / Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_2")) * 100))
                gp(3) = RG.Numb((Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_3") - Val(_dt.Rows(_dt.Rows.Count - 1)("estimated_cost"))) / Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_3")) * 100))

                sellin(1) = RG.Numb(Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_1")) * (1 + SalesTax / 100))
                sellin(2) = RG.Numb(Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_2")) * (1 + SalesTax / 100))
                sellin(3) = RG.Numb(Val(_dt.Rows(_dt.Rows.Count - 1)("selling_price_3")) * (1 + SalesTax / 100))

                Dim _st As DataTable

                _st = _BLayer.GetSizeGridDescription(_dt.Rows(_dt.Rows.Count - 1)("size_matrix"))
                If _st.Rows.Count > 0 Then
                    Dim tmpSize As ASPxComboBox
                    tmpSize = navGrid.Groups.FindByName("Grids").Items(0).FindControl("cboSizeGrid")
                    tmpSize.Text = _dt.Rows(_dt.Rows.Count - 1)("size_matrix")
                End If

                'Disable the Size
                Dim txtSize As ASPxComboBox
                txtSize = navGrid.Groups.FindByName("Grids").Items(0).FindControl("cboSizeGrid")
                txtSize.Enabled = False

                Dim dt As New DataTable

                dt.Columns.Add("ID")
                dt.Columns.Add("PriceLevel")
                dt.Columns.Add("Markup")
                dt.Columns.Add("GP")
                dt.Columns.Add("SellExcl")
                dt.Columns.Add("SellIncl")

                dt.Rows.Add("1", "Price 1", RG.Numb(markup(1)), RG.Numb(gp(1)), RG.Numb(_dt.Rows(_dt.Rows.Count - 1)("selling_price_1")), RG.Numb(sellin(1)))
                dt.Rows.Add("2", "Price 2", RG.Numb(markup(2)), RG.Numb(gp(2)), RG.Numb(_dt.Rows(_dt.Rows.Count - 1)("selling_price_2")), RG.Numb(sellin(2)))
                dt.Rows.Add("3", "Price 3", RG.Numb(markup(3)), RG.Numb(gp(3)), RG.Numb(_dt.Rows(_dt.Rows.Count - 1)("selling_price_3")), RG.Numb(sellin(3)))

                grdPrices.DataSource = dt
                grdPrices.DataBind()

                hdClient("originalcolourstring") = _dt.Rows(_dt.Rows.Count - 1)("colour_matrix")
                hdClient("colourstring") = _dt.Rows(_dt.Rows.Count - 1)("colour_matrix")

                hdClient("isforupdate") = "True"

                Dim tmpColourArray() As String = Split(_dt.Rows(_dt.Rows.Count - 1)("colour_matrix"), ":")

                Dim tmpAvailable As ASPxListBox
                tmpAvailable = navGrid.Groups.FindByName("Grids").Items(0).FindControl("lstAvailable")
                tmpAvailable.Items.Clear()

                Dim tmpSelected As ASPxListBox
                tmpSelected = navGrid.Groups.FindByName("Grids").Items(0).FindControl("lstSelected")
                tmpSelected.Items.Clear()

                _dt = _BLayer.GetColourGrids

                Dim isFound As Boolean = False

                'array - BL:BR:WH
                'dt: colour_code,colour_description
                '    BL,BLACK
                '    BR,BROWB

                If tmpColourArray.Length > 0 Then
                    For i As Integer = 0 To _dt.Rows.Count - 1
                        isFound = False

                        For ii As Integer = 0 To tmpColourArray.Length - 1
                            If tmpColourArray(ii) = _dt.Rows(i)("colour_code") Then
                                isFound = True
                            End If
                        Next

                        If isFound = True Then
                            tmpSelected.Items.Add(_dt.Rows(i)("colour_code") & " - " & _dt.Rows(i)("colour_description"))
                        Else
                            tmpAvailable.Items.Add(_dt.Rows(i)("colour_code") & " - " & _dt.Rows(i)("colour_description"))
                        End If

                    Next
                End If

            End If

            'navGrid.Groups.FindByName("Grids").Expanded = True

            'navGrid.Enabled = False

            txtStockcode.ReadOnly = True

            cmdDelete.Enabled = True

            pcMain.ShowOnPageLoad = False

        End If

        If hdWhichButton.Value = "Delete" Then

            'If txtStockcode.ReadOnly = False Then
            '    _tmpLBL.Text = "Please select a valid Master Code"
            '    pcMain.ShowOnPageLoad = True
            '    Exit Sub
            'End If

            If hdClient("isgeneratedcode") = "true" Then
                _tmpLBL.Text = "You can only delete a Master Code"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            Dim _BLayer As New StockcodesHOBL()

            '==============================================================================================
            'Adding parameters for delete masterocode report
            If Session("username") = "" Then
                ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                Exit Sub
            Else
                Username = Session("username")
                email = Session("email")
                IsAdmin = Session("is_pcm_admin")
                IPAddress = Session("ipaddress")
            End If
            Dim deleteStockcode As New DeleteStockcode
            deleteStockcode.IsAdmin = IsAdmin
            deleteStockcode.StockCode = txtStockcode.Text
            deleteStockcode.IPAddress = IPAddress
            Dim json As String = JsonConvert.SerializeObject(deleteStockcode)
            '===================================================================================================

            Dim isDeleted As String = _BLayer.DeleteMasterCode(txtStockcode.Text, Username, email, json)

            If isDeleted <> "Success" Then
                _tmpLBL.Text = "You cannot delete a stockcode on which transactions have been processed!"
            Else
                _tmpLBL.Text = "This Master Code will be deleted within the next 30 minutes"
                ClearScreen()
            End If

            dxPopUpError.ShowOnPageLoad = True



        End If

        If hdWhichButton.Value = "Submit" Then
            'Fill ze class
            Dim _Stockode As New Stockcode

            _Stockode.Barcode = txtBarcode.Text
            _Stockode.OriginalBarcode = hdClient("originalbarcode")

            _Stockode.Category1 = cboCategory1.Text
            _Stockode.Category2 = cboCategory2.Text
            _Stockode.Category3 = cboCategory3.Text

            _Stockode.CostAverage = txtAverageCost.Text
            _Stockode.CostExclusive = txtCostExclusive.Text
            _Stockode.Description = txtDescription.Text

            'isBlocked
            '--------------------------------------------
            Dim chkIsBlocked As ASPxCheckBox
            chkIsBlocked = navOptions.Groups.FindByName("Options").Items(0).FindControl("chkIsBlocked")

            _Stockode.isBlocked = chkIsBlocked.Checked
            '--------------------------------------------

            'isNotDiscountable
            '--------------------------------------------
            Dim chkNotDiscountable As ASPxCheckBox
            chkNotDiscountable = navOptions.Groups.FindByName("Options").Items(0).FindControl("chkNotDiscountable")

            _Stockode.isNotDiscountable = chkNotDiscountable.Checked
            '--------------------------------------------

            'isServiceItems
            '--------------------------------------------
            Dim chkServiceItem As ASPxCheckBox
            chkServiceItem = navOptions.Groups.FindByName("Options").Items(0).FindControl("chkServiceItem")

            _Stockode.isServiceItem = chkServiceItem.Checked
            '--------------------------------------------

            'Minimum Level
            '--------------------------------------------
            Dim MinimumLevel As ASPxTextBox
            MinimumLevel = navOptions.Groups.FindByName("Options").Items(0).FindControl("txtMinimumLevel")

            _Stockode.MinumumLevel = MinimumLevel.Text
            '--------------------------------------------

            _Stockode.PurchaseTaxGroup = Mid$(cboPurchaseTax.Text, 1, 2)
            _Stockode.SalesTaxGroup = Mid$(cboSalesTax.Text, 1, 2)

            'Selling prices
            '--------------------------------------------
            Dim column1 As GridViewDataColumn = TryCast(grdPrices.Columns("SellExcl"), GridViewDataColumn)
            Dim SellingPrice1 As ASPxTextBox = CType(grdPrices.FindRowCellTemplateControl(0, column1, "txtBox"), ASPxTextBox)

            Dim column2 As GridViewDataColumn = TryCast(grdPrices.Columns("SellExcl"), GridViewDataColumn)
            Dim SellingPrice2 As ASPxTextBox = CType(grdPrices.FindRowCellTemplateControl(1, column2, "txtBox"), ASPxTextBox)

            Dim column3 As GridViewDataColumn = TryCast(grdPrices.Columns("SellExcl"), GridViewDataColumn)
            Dim SellingPrice3 As ASPxTextBox = CType(grdPrices.FindRowCellTemplateControl(2, column3, "txtBox"), ASPxTextBox)

            _Stockode.SellingPrice1 = SellingPrice1.Text
            _Stockode.SellingPrice2 = SellingPrice2.Text
            _Stockode.SellingPrice3 = SellingPrice3.Text

            'Size grid
            '--------------------------------------------
            Dim tmpSize As ASPxComboBox

            tmpSize = navGrid.Groups.FindByName("Grids").Items(0).FindControl("cboSizeGrid")

            If tmpSize.Text <> "" Then
                Dim size() As String
                size = tmpSize.Text.Split(" - ")
                _Stockode.SizeGrid = size(0)
            Else
                _Stockode.SizeGrid = ""
            End If
            '--------------------------------------------

            _Stockode.MasterCode = txtStockcode.Text

            _Stockode.Supplier = drpSupplier.Text
            _Stockode.SupplierCode = txtSupplierItemCode.Text

            If hdClient("isforupdate") = "True" Then
                'Existing code for update
                _Stockode.isForUpdate = True
                _Stockode.OriginalColourString = hdClient("originalcolourstring")
                _Stockode.ColourString = hdClient("colourstring")
                If hdClient("isgeneratedcode") = "true" Then
                    _Stockode.isGeneratedCode = True
                Else
                    _Stockode.isGeneratedCode = False
                End If
            Else
                'New code for insert

                Dim tmpColourArray() As String
                Dim tmpColourGridString As String = ""

                Dim tmpSelected As ASPxListBox
                tmpSelected = navGrid.Groups.FindByName("Grids").Items(0).FindControl("lstSelected")

                'If tmpSelected.Items.Count > 0 Then
                '    tmpColourArray = Split(tmpSelected.Items(0).ToString, " - ")
                '    tmpColourGridString = tmpColourArray(0)
                '    If tmpSelected.Items.Count > 1 Then
                '        For gLoop As Long = 1 To tmpSelected.Items.Count - 1
                '            tmpColourArray = Split(tmpSelected.Items(gLoop).ToString, " - ")
                '            tmpColourGridString = tmpColourGridString & ":" & tmpColourArray(0)
                '        Next
                '    End If
                'End If
                '_Stockode.ColourString = tmpColourGridString

                _Stockode.ColourString = hdClient("colourstring")

                _Stockode.isForUpdate = False
            End If

            'Send it!
            Dim _BLayer As New StockcodesHOBL()
            Dim Result As String = ""

            Result = _BLayer.UpdateStockcodeTable(_Stockode)

            If Result <> "Success" Then
                _tmpLBL.Text = Result
                ClearScreen()
            Else
                _tmpLBL.Text = "The stockcode listing has been updated!"
            End If


            dxPopUpError.ShowOnPageLoad = True

        End If

        If hdWhichButton.Value = "Clear" Then
            ClearScreen()
        End If
    End Sub


    Private Sub ClearScreen(Optional ClearSearch As Boolean = True)

        navGrid.Enabled = True
        txtStockcode.ReadOnly = False
        pnlWarning.Visible = True

        Dim txtSize As ASPxComboBox
        txtSize = navGrid.Groups.FindByName("Grids").Items(0).FindControl("cboSizeGrid")
        txtSize.Enabled = True

        hdClient("colourstring") = ""
        hdClient("isforupdate") = ""
        hdClient("originalcolourstring") = ""
        hdClient("originalbarcode") = ""
        hdClient("isgeneratedcode") = ""

        txtStockcode.Text = ""
        txtDescription.Text = ""
        txtBarcode.Text = ""

        drpSupplier.Text = ""
        txtSupplierItemCode.Text = ""

        cboCategory1.Text = ""
        cboCategory2.Text = ""
        cboCategory3.Text = ""

        txtAverageCost.Text = ""
        txtCostExclusive.Text = ""
        txtCostPriceIncl.Text = ""

        ASPxEdit.ClearEditorsInContainer(navGrid)
        ASPxEdit.ClearEditorsInContainer(navOptions)


        Dim _dt As New DataTable
        Dim _dtCompany As New DataTable
        Dim _BLayer As New GeneralHOBL()

        Dim tmpAvailable As ASPxListBox
        tmpAvailable = navGrid.Groups.FindByName("Grids").Items(0).FindControl("lstAvailable")

        Dim tmpSelected As ASPxListBox
        tmpSelected = navGrid.Groups.FindByName("Grids").Items(0).FindControl("lstSelected")

        _dtCompany = _BLayer.GetCompanySettings
        cboPurchaseTax.SelectedIndex = Val(_dtCompany.Rows(0)("default_purchase_tax")) - 1
        cboSalesTax.SelectedIndex = Val(_dtCompany.Rows(0)("default_sales_tax")) - 1

        Dim _BLayerStock As New StockcodesHOBL()
        _dt = _BLayerStock.GetSizeGrids

        Dim tmpSize As ASPxComboBox
        tmpSize = navGrid.Groups.FindByName("Grids").Items(0).FindControl("cboSizeGrid")
        For i As Integer = 0 To _dt.Rows.Count - 1
            tmpSize.Items.Add(_dt(i)("grid_number") & " - " & _dt(i)("grid_description"))
        Next

        _dt = _BLayerStock.GetColourGrids

        tmpAvailable.Items.Clear()
        For i As Integer = 0 To _dt.Rows.Count - 1
            tmpAvailable.Items.Add(_dt(i)("colour_code") & " - " & _dt(i)("colour_description"))
        Next

        tmpAvailable.Attributes("ondblclick") = "AddToSelectedDbl();"
        tmpSelected.Attributes("ondblclick") = "AddToAvailableDbl();"

        Dim _tmpCBO As ASPxComboBox
        _tmpCBO = pcMain.FindControl("cboSearchType")
        _tmpCBO.Items.Clear()

        _tmpCBO.Items.Add("Master Code")
        _tmpCBO.Items.Add("Generated Code")
        _tmpCBO.Items.Add("Barcode")
        _tmpCBO.Items.Add("Description")

        grdPrices.DataSource = ReturnTable()
        grdPrices.DataBind()

        cboCategory1.Text = ""
        _dt = _BLayerStock.GetCategoryList("1", "")

        cboCategory1.DataSource = _dt
        cboCategory1.DataBind()

        cboCategory2.Text = ""
        _dt = _BLayerStock.GetCategoryList("2", "")

        cboCategory2.DataSource = _dt
        cboCategory2.DataBind()

        cboCategory3.Text = ""
        _dt = _BLayerStock.GetCategoryList("3", "")

        cboCategory3.DataSource = _dt
        cboCategory3.DataBind()

        If ClearSearch = True Then
            _dt = Nothing
            grdStockcodeSearch.DataSource = _dt
            grdStockcodeSearch.DataBind()

            ASPxEdit.ClearEditorsInContainer(pcMain)
        End If

        cmdDelete.Enabled = False

        chkSure.Checked = False

        pnlWarning.Visible = False

    End Sub

    'Protected Sub cboCategory1_OnItemsRequestedByFilterCondition_SQL(ByVal source As Object, ByVal e As ListEditItemsRequestedByFilterConditionEventArgs)
    '    'Dim comboBox As ASPxComboBox = CType(source, ASPxComboBox)

    '    Dim comboBox As ASPxComboBox = CType(source, ASPxComboBox)

    '    'Don't scroll
    '    If e.EndIndex > 9 Then
    '        Exit Sub
    '    End If

    '    Dim _dt As New DataTable
    '    Dim _BLayer As New StockcodesHOBL()

    '    _dt = _BLayer.GetCategoryList("1", e.Filter)

    '    cboCategory1.DataSource = _dt
    '    cboCategory1.DataBind()

    'End Sub

    'Protected Sub cboCategory1_OnItemRequestedByValue_SQL(ByVal source As Object, ByVal e As ListEditItemRequestedByValueEventArgs)
    '    'The OnItemRequestedByValue should be dealt with however the implementation is not worth the small (very occasional) problem that is caused by not dealing with it causes

    '    Dim value As Long = 0
    '    If e.Value Is Nothing OrElse (Not Int64.TryParse(e.Value.ToString(), value)) Then
    '        Return
    '    End If

    '    Dim comboBox As ASPxComboBox = CType(source, ASPxComboBox)

    '    Dim _BLayer As New ShopDownloadsBL()

    '    Dim dt As DataTable = _BLayer.GetEmployeeClockNumbers(e.Value.ToString)

    '    Session.Remove("employees")

    '    Session("employees") = dt

    '    comboBox.DataSource = dt
    '    comboBox.DataBind()
    'End Sub

    'Protected Sub cboCategory2_OnItemsRequestedByFilterCondition_SQL(ByVal source As Object, ByVal e As ListEditItemsRequestedByFilterConditionEventArgs)
    '    'The OnItemRequestedByValue should be dealt with however the implementation is not worth the small (very occasional) problem that is caused by not dealing with it causes
    'End Sub

    'Protected Sub cboCategory3_OnItemRequestedByValue_SQL(ByVal source As Object, ByVal e As ListEditItemRequestedByValueEventArgs)
    '    'The OnItemRequestedByValue should be dealt with however the implementation is not worth the small (very occasional) problem that is caused by not dealing with it causes
    'End Sub

    'Protected Sub cboCategory3_OnItemsRequestedByFilterCondition_SQL(ByVal source As Object, ByVal e As ListEditItemsRequestedByFilterConditionEventArgs)
    '    'Dim comboBox As ASPxComboBox = CType(source, ASPxComboBox)

    '    If Len(e.Filter) < 1 Then Exit Sub
    '    Dim _dt As New DataTable
    '    Dim _BLayer As New StockcodesHOBL()

    '    _dt = _BLayer.GetCategoryList("2", e.Filter)

    '    cboCategory1.DataSource = _dt
    '    cboCategory1.DataBind()

    'End Sub

    'Protected Sub cboCategory2_OnItemRequestedByValue_SQL(ByVal source As Object, ByVal e As ListEditItemRequestedByValueEventArgs)
    '    'The OnItemRequestedByValue should be dealt with however the implementation is not worth the small (very occasional) problem that is caused by not dealing with it causes
    'End Sub

    Protected Sub cboSupplier_OnItemRequestedByValue_SQL(ByVal source As Object, ByVal e As ListEditItemRequestedByValueEventArgs)
        If e.Value Is Nothing Then
            Return
        End If

        Dim _dt As New DataTable
        Dim _BLayer As New StockcodesHOBL()
        Try
            _dt = _BLayer.GetSupplier(e.Value)

            drpSupplier.DataSource = _dt
            drpSupplier.DataBind()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try


    End Sub

    Protected Sub cboSupplier_OnItemsRequestedByFilterCondition_SQL(ByVal source As Object, ByVal e As ListEditItemsRequestedByFilterConditionEventArgs)
        Dim comboBox As ASPxComboBox = CType(source, ASPxComboBox)

        If Len(e.Filter) < 1 Then Exit Sub
        Dim _dt As New DataTable
        Dim _BLayer As New StockcodesHOBL()
        Try
            _dt = _BLayer.GetSupplierList(e.Filter)

            drpSupplier.DataSource = _dt
            drpSupplier.DataBind()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri
        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.StockcodeManager) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

    End Sub

    Protected Sub cboPurchaseTax_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim _dt As New DataTable
        Dim _BLayer As New GeneralHOBL()
        Try
            _dt = _BLayer.GetTaxGroups
            cboPurchaseTax.Items.Clear()
            cboPurchaseTax.DataSource = _dt
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

    Protected Sub grdStockcodeSearch_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdStockcodeSearch.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim _Blayer As New StockcodesHOBL()

        Dim _tmpCBO As ASPxComboBox

        _tmpCBO = pcMain.FindControl("cboSearchType")

        Dim _tmpTXT As ASPxTextBox

        _tmpTXT = pcMain.FindControl("txtStockcodeSearch")

        Dim _tmpCheck As ASPxCheckBox

        _tmpCheck = pcMain.FindControl("chkMasterCode")

        Try
            Dim data As DataTable = _Blayer.GetStockcode(_tmpCBO.Text, _tmpTXT.Text, _tmpCheck.Checked)

            gridView.KeyFieldName = "stockcode" 'data.PrimaryKey(0).ColumnName
            gridView.DataSource = data

            grdStockcodeSearch.EndUpdate()
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

    Protected Sub txtMarkup_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim tb As ASPxTextBox = TryCast(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(tb.NamingContainer, GridViewDataItemTemplateContainer)

        Dim key As String = container.KeyValue.ToString()

        tb.ClientInstanceName = "markup" + key
        tb.ClientSideEvents.KeyUp = String.Format("function(s, e) {{ OnKeyUpMarkup(s, e, {0}); }}", key)
        tb.ClientSideEvents.KeyPress = "NumericOnly"

    End Sub

    Protected Sub txtGP_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim tb As ASPxTextBox = TryCast(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(tb.NamingContainer, GridViewDataItemTemplateContainer)

        Dim key As String = container.KeyValue.ToString()

        tb.ClientInstanceName = "gp" + key

    End Sub

    Protected Sub txtSellIncl_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim tb As ASPxTextBox = TryCast(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(tb.NamingContainer, GridViewDataItemTemplateContainer)

        Dim key As String = container.KeyValue.ToString()

        tb.ClientInstanceName = "sellincl" + key
        tb.ClientSideEvents.KeyUp = String.Format("function(s, e) {{ OnKeyUpSellIncl(s, e, {0}); }}", key)
        tb.ClientSideEvents.KeyPress = "NumericOnly"

    End Sub

    Protected Sub txtSellEx_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim tb As ASPxTextBox = TryCast(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(tb.NamingContainer, GridViewDataItemTemplateContainer)

        Dim key As String = container.KeyValue.ToString()

        tb.ClientInstanceName = "sellex" + key
        tb.ClientSideEvents.KeyUp = String.Format("function(s, e) {{ OnKeyUpSellEx(s, e, {0}); }}", key)
        tb.ClientSideEvents.KeyPress = "NumericOnly"

    End Sub

    Protected Sub ASPxGridView1_HtmlDataCellPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableDataCellEventArgs)
        e.Cell.Attributes.Add("onclick", "OnCellClick(" & e.VisibleIndex & "," & e.DataColumn.FieldName & ");")
    End Sub

    Private Sub StockcodeManager_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Private Function ReturnTable() As DataTable

        Dim dt As New DataTable

        dt.Columns.Add("ID")
        dt.Columns.Add("PriceLevel")
        dt.Columns.Add("Markup")
        dt.Columns.Add("GP")
        dt.Columns.Add("SellExcl")
        dt.Columns.Add("SellIncl")

        dt.Rows.Add("1", "Price 1", "", "", "", "")
        dt.Rows.Add("2", "Price 2", "", "", "", "")
        dt.Rows.Add("3", "Price 3", "", "", "", "")

        Return dt


    End Function


End Class
