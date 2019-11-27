Imports pcm.DataLayer
Imports Entities

Public Class StockcodesHOBL

    Dim _DLayer As StockcodesHODL

    'Public Sub New(ByVal CompanyCode As String)
    '    _DLayer = New StockcodesHODL(CompanyCode)
    'End Sub

    Public Sub New()
        _DLayer = New StockcodesHODL
    End Sub

    Public Function RunStockAllocation(ByVal Filename As String, ByVal EmailAddresses As String) As String

        Return _DLayer.RunStockAllocation(Filename, EmailAddresses)

    End Function

    Public Function RunCategoryReport(ByVal EMailAddresses As String,
                                      ByVal StartDate As String, ByVal EndDate As String) As String

        Return _DLayer.RunCategoryReport(EMailAddresses, StartDate, EndDate)

    End Function

    Public Function InsertPrePack(ByVal PrePack As PrePack) As String

        Return _DLayer.InsertPrePack(PrePack)

    End Function

    Public Function GetPrePackData(ByVal SizeGrid As String, ByVal PrePackCode As String) As DataTable

        Return _DLayer.GetPrePackData(SizeGrid, PrePackCode)

    End Function

    Public Function GetPrePacks(ByVal SizeGrid As String) As DataTable

        Return _DLayer.GetPrePacks(SizeGrid)

    End Function

    Public Function GetStockcode(ByVal SearchCriteria As String, ByVal SearchString As String, ByVal isMasterCode As Boolean) As DataTable

        Return _DLayer.GetStockcode(SearchCriteria, SearchString, isMasterCode)

    End Function

    Public Function GetCategoryList(ByVal CategoryNumber As String, ByVal SearchString As String) As DataTable

        Return _DLayer.GetCategoryList(CategoryNumber, SearchString)

    End Function

    Public Function GetSupplierList(ByVal SearchString As String) As DataTable

        Return _DLayer.GetSupplierList(SearchString)

    End Function

    Public Function GetSupplier(ByVal SearchString As String) As DataTable

        Return _DLayer.GetSupplier(SearchString)

    End Function

    Public Function GetSizeGrids() As DataTable

        Return _DLayer.GetSizeGrids

    End Function

    Public Function GetColourGrids() As DataTable

        Return _DLayer.GetColourGrids

    End Function

    Public Function GetGeneratedCodeInfo(ByVal Stockcode As String) As DataTable

        Return _DLayer.GetGeneratedCodeInfo(Stockcode)

    End Function

    Public Function GetMasterCodeInfo(ByVal Stockcode As String) As DataTable

        Return _DLayer.GetMasterCodeInfo(Stockcode)

    End Function


    Public Function DeleteMasterCode(ByVal Stockcode As String, ByVal Username As String, ByVal Email As String, ByVal json As String) As String

        Return _DLayer.DeleteMasterCode(Stockcode, Username, Email, json)

    End Function

    Public Function GetSingleStockOnHand(ByVal GeneratedCode As String, ByVal BranchCode As String) As DataTable

        Return _DLayer.GetSingleStockOnHand(GeneratedCode, BranchCode)

    End Function

    Public Function GetSizeGridDescription(ByVal GridNumber As String) As DataTable

        Return _DLayer.GetSizeGridDescription(GridNumber)

    End Function

    Public Function GetColourGridDescription(ByVal ColourGridCode As String) As DataTable

        Return _DLayer.GetSizeGridDescription(ColourGridCode)

    End Function

    Private Function InsertStockcodes(ByVal _Stockcode As Stockcode) As String

        Dim _BLayer As New GeneralHOBL
        Dim _DLayerStock As New StockcodesHODL

        If _BLayer.CheckRecordExists("stockcodes_master", "master_code", _Stockcode.MasterCode) = True Then
            Return "The master code you entered already belongs to another stockcode."
        End If

        If _BLayer.CheckRecordExists("stockcodes_master", "sku_number", _Stockcode.Barcode) = True Then
            Return "The barcode you entered already belongs to another stockcode."
        End If

        If _Stockcode.SizeGrid = "" And _Stockcode.ColourString = "" Then
            'Plain vanilla. No size, no colour
            _Stockcode.GeneratedCode = _Stockcode.MasterCode
            Return _DLayerStock.InsertStockcode(_Stockcode)

        ElseIf _Stockcode.SizeGrid <> "" And _Stockcode.ColourString = "" Then
            'Just size
            Dim tmpSizeArray(32) As String
            Dim SuccessString = ""

            Dim _st As DataTable
            _st = _DLayerStock.GetSizeGridDescription(_Stockcode.SizeGrid)

            For gLoop As Integer = 1 To 30
                tmpSizeArray(gLoop) = _st.Rows(0)("s" & gLoop) & ""
            Next

            Dim PermanentDescription As String = _Stockcode.Description

            For gLoop As Integer = 1 To 30
                If tmpSizeArray(gLoop) <> "" Then

                    _Stockcode.GeneratedCode = _Stockcode.MasterCode & tmpSizeArray(gLoop)
                    _Stockcode.Description = PermanentDescription & " " & tmpSizeArray(gLoop)
                    _Stockcode.ItemSize = tmpSizeArray(gLoop)
                    _Stockcode.Barcode = ""

                    SuccessString = _DLayerStock.InsertStockcode(_Stockcode)

                    If SuccessString <> "Success" Then
                        Return SuccessString
                    End If

                End If
            Next

        ElseIf _Stockcode.SizeGrid = "" And _Stockcode.ColourString <> "" Then
            'Just colour    
            Dim tmpColourArray() As String = Split(_Stockcode.ColourString, ":")

            Dim PermanentDescription As String = _Stockcode.Description
            Dim SuccessString = ""

            For gLoop As Integer = 0 To tmpColourArray.Count - 1

                _Stockcode.GeneratedCode = _Stockcode.MasterCode & tmpColourArray(gLoop)
                _Stockcode.Description = PermanentDescription & " " & tmpColourArray(gLoop)
                _Stockcode.ItemColour = tmpColourArray(gLoop)

                _Stockcode.Barcode = ""
                SuccessString = _DLayerStock.InsertStockcode(_Stockcode)

                If SuccessString <> "Success" Then
                    Return SuccessString
                End If
            Next

        ElseIf _Stockcode.SizeGrid <> "" And _Stockcode.ColourString <> "" Then
            'Colour and Size

            Dim tmpColourArray() As String = Split(_Stockcode.ColourString, ":")

            Dim tmpSizeArray(32) As String

            Dim _st As DataTable
            _st = _DLayerStock.GetSizeGridDescription(_Stockcode.SizeGrid)

            For gLoop As Integer = 1 To 30
                tmpSizeArray(gLoop) = _st.Rows(0)("s" & gLoop) & ""
            Next

            Dim PermanentDescription As String = _Stockcode.Description
            Dim SuccessString = ""

            For gLoop As Integer = 0 To tmpColourArray.Count - 1
                For iLoop As Integer = 1 To 30
                    If tmpSizeArray(iLoop) <> "" Then

                        _Stockcode.GeneratedCode = _Stockcode.MasterCode & tmpColourArray(gLoop) & tmpSizeArray(iLoop)
                        _Stockcode.Description = PermanentDescription & " " & tmpColourArray(gLoop) & " " & tmpSizeArray(iLoop)
                        _Stockcode.ItemSize = tmpSizeArray(iLoop)
                        _Stockcode.ItemColour = tmpColourArray(gLoop)
                        _Stockcode.Barcode = ""

                        SuccessString = _DLayerStock.InsertStockcode(_Stockcode)

                        If SuccessString <> "Success" Then
                            Return SuccessString
                        End If

                    End If
                Next
            Next

        End If

        Return "Success"

    End Function

    Private Function UpdateStockcodes(ByVal _Stockcode As Stockcode) As String

        Dim _BLayer As New GeneralHOBL
        Dim _DLayerStock As New StockcodesHODL

        Dim SuccessString As String = ""

        If _Stockcode.isGeneratedCode = True Then
            'If the new barcode isn't the same as the old barcode, it cannot exist in the stockcode_master table
            If Trim(_Stockcode.OriginalBarcode) <> Trim(_Stockcode.Barcode) Then
                If _BLayer.CheckRecordExists("stockcodes_master", "sku_number", _Stockcode.Barcode) = True Then
                    Return "The barcode you entered already belongs to another stockcode."
                End If
            End If

            _Stockcode.GeneratedCode = _Stockcode.MasterCode
            Return _DLayerStock.UpdateStockcode(_Stockcode)

        Else
            'Master code updated
            'If the new barcode isn't the same as the old barcode, it cannot exist in the stockcode_master table
            If Trim(_Stockcode.OriginalBarcode) <> Trim(_Stockcode.Barcode) Then
                If _BLayer.CheckRecordExists("stockcodes_master", "sku_number", _Stockcode.Barcode) = True Then
                    Return "The barcode you entered already belongs to another stockcode."
                End If
            End If

            'Check if colours have been added
            Dim tmpNewColoursSTR As String = ""
            Dim tmpColours() As String = Split(_Stockcode.ColourString, (":"))

            If _Stockcode.ColourString <> "" Then
                'Loop through the colours to see if any have been added
                For gLoop As Long = 0 To tmpColours.Count - 1
                    If Not _Stockcode.OriginalColourString.ToUpper.Contains(tmpColours(gLoop).ToUpper) Then
                        'Colour does not exist in current colours, need to add
                        If tmpNewColoursSTR = "" Then
                            tmpNewColoursSTR = tmpColours(gLoop)
                        Else
                            tmpNewColoursSTR = tmpNewColoursSTR & ":" & tmpColours(gLoop)
                        End If
                    End If
                Next
            End If

            _Stockcode.GeneratedCode = _Stockcode.MasterCode
            SuccessString = _DLayerStock.UpdateStockcode(_Stockcode)

            If SuccessString <> "Success" Then
                Return SuccessString
            End If

            'If colours have been added, INSERT them (copy from INSERT)
            If tmpNewColoursSTR <> "" Then
                If _Stockcode.SizeGrid = "" Then
                    'Just colour    
                    Dim tmpColourArray() As String = Split(tmpNewColoursSTR, ":")

                    Dim PermanentDescription As String = _Stockcode.Description

                    For gLoop As Integer = 0 To tmpColourArray.Count - 1

                        _Stockcode.GeneratedCode = _Stockcode.MasterCode & tmpColourArray(gLoop)
                        _Stockcode.Description = PermanentDescription & " " & tmpColourArray(gLoop)
                        _Stockcode.ItemColour = tmpColourArray(gLoop)

                        _Stockcode.Barcode = ""
                        SuccessString = _DLayerStock.InsertStockcode(_Stockcode)

                        If SuccessString <> "Success" Then
                            Return SuccessString
                        End If
                    Next

                ElseIf _Stockcode.SizeGrid <> "" Then
                    'Colour and Size

                    Dim tmpColourArray() As String = Split(tmpNewColoursSTR, ":")

                    Dim tmpSizeArray(32) As String

                    Dim _st As DataTable
                    _st = _DLayerStock.GetSizeGridDescription(_Stockcode.SizeGrid)

                    For gLoop As Integer = 1 To 30
                        tmpSizeArray(gLoop) = _st.Rows(0)("s" & gLoop) & ""
                    Next

                    Dim PermanentDescription As String = _Stockcode.Description

                    For gLoop As Integer = 0 To tmpColourArray.Count - 1
                        For iLoop As Integer = 1 To 30
                            If tmpSizeArray(iLoop) <> "" Then

                                _Stockcode.GeneratedCode = _Stockcode.MasterCode & tmpColourArray(gLoop) & tmpSizeArray(iLoop)
                                _Stockcode.Description = PermanentDescription & " " & tmpColourArray(gLoop) & " " & tmpSizeArray(iLoop)
                                _Stockcode.ItemSize = tmpSizeArray(iLoop)
                                _Stockcode.ItemColour = tmpColourArray(gLoop)
                                _Stockcode.Barcode = ""

                                SuccessString = _DLayerStock.InsertStockcode(_Stockcode)

                                If SuccessString <> "Success" Then
                                    Return SuccessString
                                End If

                            End If
                        Next
                    Next

                End If
            End If

            Return "Success"

        End If
    End Function
    Public Function UpdateStockcodeTable(ByVal _Stockcode As Stockcode) As String

        Dim _BLayer As New GeneralHOBL
        Dim _DLayerStock As New StockcodesHODL

        If _Stockcode.Category1 <> "" Then
            If _BLayer.CheckRecordExists("categories", "category_type", "STOCK", "category_number", "1", "category_code", _Stockcode.Category1) = False Then
                Return "Category 1 does not exist"
            End If
        End If

        If _Stockcode.Category2 <> "" Then
            If _BLayer.CheckRecordExists("categories", "category_type", "STOCK", "category_number", "2", "category_code", _Stockcode.Category2) = False Then
                Return "Category 2 does not exist"
            End If
        End If

        If _Stockcode.Category3 <> "" Then
            If _BLayer.CheckRecordExists("categories", "category_type", "STOCK", "category_number", "3", "category_code", _Stockcode.Category3) = False Then
                Return "Category 3 does not exist"
            End If
        End If

        If _Stockcode.isForUpdate = True Then
            Return UpdateStockcodes(_Stockcode)
        Else
            'This is a new code
            Return InsertStockcodes(_Stockcode)
        End If

    End Function

    'Stock On Hand By Branch Report
    Public Function GetStockcodes(ByVal Mastercode As String) As DataTable
        Return _DLayer.GetStockcodes(Mastercode)
    End Function
    Public Function GetStockOnHandByBranch(ByVal Mastercode As String) As DataTable
        Return _DLayer.GetStockOnHandByBranch(Mastercode)

    End Function

End Class
