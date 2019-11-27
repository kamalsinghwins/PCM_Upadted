Imports Npgsql
Imports Entities
Imports System.IO

Public Class StockcodesHODL
    Inherits DataAccessLayerBase
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing
    Dim _dlErrorLogging As New ErrorLogDL


    'Public Sub New(ByVal CompanyCode As String)
    '    objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite", "pos_" & CompanyCode)
    '    objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead", "pos_" & CompanyCode)
    '    connection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionStringPositiveRead").ConnectionString & "database=" & ConfigurationManager.AppSettings("CurrentDatabase"))

    'End Sub
    Public Sub New()
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = New NpgsqlConnection(ConfigurationManager.ConnectionStrings("PostgreConnectionStringPositiveRead").ConnectionString)

    End Sub
    Public Function RunCategoryReport(ByVal EMailAddresses As String,
                                      ByVal StartDate As String, ByVal EndDate As String) As String


        tmpSQL = "INSERT INTO tasks (task_type,file_to_run,timestamp_created,email_addresses,start_date,end_date) VALUES " &
               "('category_report','','" & Format(Now, "yyyy-MM-dd HH:mm") & "','" & EMailAddresses & "','" & StartDate & "','" & EndDate & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        Finally
            'objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function
    Public Function RunStockAllocation(ByVal Filename As String, ByVal EMailAddresses As String) As String


        'CSV must be in format:
        ',6,6,123,123,124,124                                           - ,master_code,master_code,master_code,master_code,master_code,master_code,    
        ',WH,WH,BR,BR,BL,BR                                             - colour grid
        ',D,A,D,A,A,D                                                   - pre packs
        'AL1, 1, 1, 0, 1, 1, 0                                          - branch_code,pre pack quantites
        'BR1, 1, 2, 0, 0, 1, 1                                          - branch_code,pre pack quantites

        'Build the dataset

        Dim isSuccess As Boolean = True
        Dim ErrorMessage As String = ""

        Using sr As New StreamReader(Filename)

            Dim _dt As New DataTable

            Dim stringdataline1(100) As String
            Dim stringdataline2(100) As String
            Dim stringdataline3(100) As String

            Dim _BLayer As New GeneralHODL

            Dim heading(200) As String
            Dim tmpheading0 As String
            Dim tmpheading1 As String
            Dim tmpheading2 As String

            Dim ReturnStatus As String = ""

            Dim dataline(100) As String

            Dim line As String

            Dim linenumber As Integer = 0

            ' Read and display lines from the file until the end of
            ' the file is reached.
            Do
                line = sr.ReadLine()
                If Not (line Is Nothing) Then
                    '--------------------------------------------
                    'Lines 1-3 (0 to 2 in read) are the headings
                    If linenumber = 0 Then
                        '1st line header
                        stringdataline1 = line.Split(",")
                    End If
                    If linenumber = 1 Then
                        '2st line header
                        stringdataline2 = line.Split(",")
                    End If
                    If linenumber = 2 Then
                        '3rd line header
                        stringdataline3 = line.Split(",")

                        'All the heading data has already been grabbed
                        'Now the codes need to be checked and built

                        'Loop through the array of the first line
                        For i As Integer = 0 To stringdataline1.Length - 1

                            'Line 1
                            tmpheading0 = stringdataline1(i)
                            If i > 1 Then
                                'First column can be blank, rest cannot
                                '2012-12-02: EDIT: 2nd column is now bay number
                                If stringdataline1(i) = "" Then
                                    Return "Unexpected blank space in Line: " & linenumber & " Column: " & i
                                End If

                                'Check that the master code exists
                                If _BLayer.CheckRecordExists("stockcodes_master", "master_code", tmpheading0) = False Then
                                    isSuccess = False
                                    ErrorMessage = "Stockcode " & tmpheading0 & " in line 1 does not exist \n"
                                End If

                            End If

                            'Line 2
                            tmpheading1 = stringdataline2(i)
                            If i > 1 Then
                                'First column can be blank, rest cannot
                                '2012-12-02: EDIT: 2nd column is now bay number
                                If stringdataline2(i) = "" Then
                                    Return "Unexpected blank space in Line: " & linenumber & " Column: " & i
                                End If

                                'Check that the colour exists for the master code
                                ReturnStatus = CheckColourExistsForMasterCode(tmpheading0, tmpheading1)
                                If ReturnStatus <> "Success" Then
                                    isSuccess = False
                                    ErrorMessage &= ReturnStatus & "\n"
                                End If
                            End If

                            'Line 3
                            tmpheading2 = stringdataline3(i)
                            If i > 1 Then
                                'First column cab be blank, rest cannot
                                '2012-12-02: EDIT: 2nd column is now bay number
                                If tmpheading2 = "" Then
                                    tmpheading2 = stringdataline3(i - 1)
                                End If

                                'Check if prepack matches the size grid of the stockcode
                                ReturnStatus = MatchPrePackToSize(tmpheading0, tmpheading2)
                                If ReturnStatus <> "Success" Then
                                    isSuccess = False
                                    ErrorMessage &= ReturnStatus & "\n"
                                End If
                            End If

                        Next

                    End If

                    If linenumber > 2 Then
                        dataline = line.Split(",")

                        'Check if branch code exists
                        If _BLayer.CheckRecordExists("branch_details", "branch_code", dataline(0)) = False Then
                            isSuccess = False
                            ErrorMessage &= "Branch code " & dataline(0) & " in line " & linenumber & " does not exist  \n"
                        End If

                    End If
                    linenumber += 1
                End If
            Loop Until line Is Nothing
        End Using

        If isSuccess = False Then
            Return ErrorMessage
        End If

        tmpSQL = "INSERT INTO tasks (task_type,file_to_run,timestamp_created,email_addresses) VALUES " &
                 "('allocation_upload','" & Filename & "','" & Format(Now, "yyyy-MM-dd HH:mm") & "','" & EMailAddresses & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        Finally
            'objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function
    Public Function CheckColourExistsForMasterCode(ByVal MasterCode As String, ByVal Colour As String) As String

        Dim tmpSizeGrid As String = ""

        'For some reason I didn't include colour in the query, so users were able to upload incorrect colours.
        'Fixed: 2015-05-12
        tmpSQL = "SELECT DISTINCT(item_colour) FROM stockcodes_master WHERE master_code = '" & RG.Apos(MasterCode.ToUpper) & "' AND item_colour = '" & Colour & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If Not objDBRead.isR(ds) Then
                Return "Colour " & Colour & " does not exist for code " & MasterCode
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try

        Return "Success"

    End Function
    Public Function MatchPrePackToSize(ByVal MasterCode As String, ByVal PrePack As String) As String

        Dim tmpSizeGrid As String = ""

        tmpSQL = "SELECT DISTINCT(size_grid) FROM stock_pre_packs WHERE pre_pack_code = '" & RG.Apos(PrePack.ToUpper) & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    tmpSizeGrid = dr("size_grid") & ""
                Next
            Else
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return "Pack size " & PrePack & " does not exist"
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try

        tmpSQL = "SELECT DISTINCT(size_matrix) FROM stockcodes_master WHERE master_code = '" & RG.Apos(MasterCode.ToUpper) & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If Trim(tmpSizeGrid.ToUpper) <> Trim(dr("size_matrix") & "".ToUpper) Then
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return "Pre Pack " & PrePack & " does not exist for Stockcode " & MasterCode
                    End If
                Next
            Else
                'This should never happen as the existence of the stockcode is checked previously
                'in the main function
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return "Stockcode " & MasterCode & " does not exist"
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try

        Return "Success"

    End Function
    Public Function InsertPrePack(ByVal PrePack As PrePack) As String

        'Check that the pre-pack grid doesn't belong to another size grid
        tmpSQL = "SELECT size_grid FROM stock_pre_packs WHERE pre_pack_code = '" & RG.Apos(PrePack.PrePackCode.ToUpper) & "' " &
                 "AND size_grid <> '" & RG.Apos(PrePack.SizeGrid.ToUpper) & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                Return "This Pre-Pack code is already in use with Size Grid " & ds.Tables(0).Rows(0)("size_grid")
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try

        'Delete the grid first
        '(Since this screen will be used very seldomly there is no insert / update option
        tmpSQL = "DELETE FROM stock_pre_packs WHERE pre_pack_code = '" & RG.Apos(PrePack.PrePackCode.ToUpper) & "'"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        For i As Integer = 0 To PrePack.PrePackGrid.Count - 1
            tmpSQL = "INSERT INTO stock_pre_packs (pre_pack_code,size_grid,size,pre_pack_quantity) VALUES " &
                    "('" & RG.Apos(Mid$(PrePack.PrePackCode.ToUpper, 1, 15)) & "','" & RG.Apos(Mid$(PrePack.SizeGrid.ToUpper, 1, 4)) & "'," &
                    "'" & RG.Apos(PrePack.PrePackGrid(i).Size.ToUpper) & "','" & RG.Numb(PrePack.PrePackGrid(i).Quantity) & "')"
            Try
                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            Catch ex As Exception
                'objDBWrite.CloseConnection()
                _dlErrorLogging.ErrorLogging(ex)
                Return ex.Message
            Finally
                'objDBWrite.CloseConnection()
            End Try

        Next

        Return "Success"

    End Function
    Public Function InsertStockcode(ByVal _Stockcode As Stockcode) As String

        Dim _GeneralLayer As New GeneralHODL

        'Generate the barcode if neccessary
        If _Stockcode.Barcode = "" Then
            _Stockcode.Barcode = _GeneralLayer.GenerateSKU
        Else
            If _GeneralLayer.CheckRecordExists("stockcodes_master", "sku_number", RG.Apos(_Stockcode.Barcode.ToUpper)) = True Then
                Return "The Barcode you have inputted is already assigned to a different Stockcode."
            End If
        End If

        If Mid$(_Stockcode.MasterCode, 1, 2) = "##" Then
            _Stockcode.isServiceItem = True
        End If

        If Mid$(_Stockcode.MasterCode, 1, 2) = "@@" Then
            _Stockcode.isServiceItem = True
        End If

        'Insert into stockcodes_master
        tmpSQL = "INSERT INTO stockcodes_master (master_code,generated_code,sku_number,is_service_item,description," &
                "purchase_tax_group,sales_tax_group,supplier,suppliers_code,is_not_discountable,is_blocked," &
                "minimum_stock_level,item_size,item_colour,size_matrix,colour_matrix) VALUES " &
                "('" & RG.Apos(Mid$(_Stockcode.MasterCode.ToUpper, 1, 20)) & "','" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "'," &
                "'" & RG.Apos(Mid$(_Stockcode.Barcode.ToUpper, 1, 30)) & "'," &
                "'" & _Stockcode.isServiceItem & "','" & RG.Apos(Mid$(_Stockcode.Description.ToUpper, 1, 50)) & "'," &
                "'" & _Stockcode.PurchaseTaxGroup & "','" & _Stockcode.SalesTaxGroup & "','" & RG.Apos(Mid$(_Stockcode.Supplier.ToUpper, 1, 30)) & "'," &
                "'" & RG.Apos(Mid$(_Stockcode.SupplierCode.ToUpper, 1, 30)) & "'," & _Stockcode.isNotDiscountable & "," & _Stockcode.isBlocked & "," &
                "'" & Val(_Stockcode.MinumumLevel) & "','" & _Stockcode.ItemSize & "','" & _Stockcode.ItemColour & "'," &
                "'" & RG.Apos(Mid$(_Stockcode.SizeGrid, 1, 5)) & "','" & RG.Apos(Mid$(_Stockcode.ColourString, 1, 100)) & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        'Insert categories
        tmpSQL = "INSERT INTO stockcodes_categories (master_code,generated_code,category_1,category_2,category_3) " &
                 "VALUES('" & RG.Apos(Mid$(_Stockcode.MasterCode.ToUpper, 1, 20)) & "','" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "'," &
                 "'" & RG.Apos(Mid$(_Stockcode.Category1.ToUpper, 1, 4)) & "','" & RG.Apos(Mid$(_Stockcode.Category2.ToUpper, 1, 4)) & "'," &
                 "'" & RG.Apos(Mid$(_Stockcode.Category3.ToUpper, 1, 4)) & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        'Insert prices
        tmpSQL = "INSERT INTO stockcodes_prices (master_code,generated_code,estimated_cost,selling_price_1,selling_price_2,selling_price_3) " &
                 "VALUES('" & RG.Apos(Mid$(_Stockcode.MasterCode.ToUpper, 1, 20)) & "','" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "'," &
                 "'" & RG.Numb(_Stockcode.CostExclusive, 3) & "','" & RG.Numb(_Stockcode.SellingPrice1, 3) & "'," &
                 "'" & RG.Numb(_Stockcode.SellingPrice2, 3) & "','" & RG.Numb(_Stockcode.SellingPrice3, 3) & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        Dim tmpGUID As String
        tmpGUID = Guid.NewGuid.ToString

        tmpSQL = "INSERT INTO stockcodes_price_updates (guid,date_of_change,time_of_change,master_code," &
                 "generated_code,new_price_1,new_price_2,new_price_3,user_name) " &
                 " VALUES ('" & tmpGUID & "','" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "'," &
                 "'" & RG.Apos(Mid$(_Stockcode.MasterCode.ToUpper, 1, 20)) & "','" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "'," &
                 "'" & RG.Numb(_Stockcode.SellingPrice1, 3) & "'," &
                 "'" & RG.Numb(_Stockcode.SellingPrice2, 3) & "','" & RG.Numb(_Stockcode.SellingPrice3, 3) & "','WEB')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        'Insert Dates
        'Set the inserted to now() (Postgres system time) as we don't want a shop downloading before this upload has run
        'and therefore missing the codes in the time difference
        tmpSQL = "INSERT INTO stockcodes_dates (master_code,generated_code,created) VALUES('" & RG.Apos(Mid$(_Stockcode.MasterCode.ToUpper, 1, 20)) & "'," &
                 "'" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "',now())"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        'Insert into stock_on_hand
        'Dim objclsDatabase2 As New clsNPDatabase(pubDataBaseServerString, pubDataBaseNameString, pubDataBaseUserString, pubDataBasePassString, pubDataBasePortString)

        'tmpSQL = "SELECT branch_code FROM branch_details"
        'Try
        '    ds = objDBRead.GetDataSet(tmpSQL)
        '    If objDBRead.isR(ds) Then
        '        For Each dr As DataRow In ds.Tables(0).Rows
        '            If dr("branch_code") <> "" Then
        'tmpSQL = "INSERT INTO stock_on_hand (generated_code,branch_code) VALUES ('" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "'," & _
        '                          "'" & dr("branch_code").ToString.ToUpper & "')"

        tmpSQL = "INSERT INTO stock_on_hand (generated_code, branch_code) SELECT '" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "', branch_code FROM branch_details;"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try
        '            End If
        '        Next
        '    Else

        '    End If
        'Catch ex As Exception
        '    If (objDBRead IsNot Nothing) Then
        '        objDBRead.CloseConnection()
        '    End If
        '    Return Nothing
        'Finally
        '    If (objDBRead IsNot Nothing) Then
        '        objDBRead.CloseConnection()
        '    End If
        'End Try

        'Insert into stock_bestsellers_monthly
        tmpSQL = "INSERT INTO stock_bestsellers_monthly (master_code,generated_code) VALUES ('" & RG.Apos(Mid$(_Stockcode.MasterCode.ToUpper, 1, 20)) & "'," &
                 "'" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        'Insert into stock_bestsellers_weekly
        tmpSQL = "INSERT INTO stock_bestsellers_weekly (master_code,generated_code) VALUES ('" & RG.Apos(Mid$(_Stockcode.MasterCode.ToUpper, 1, 20)) & "'," &
                 "'" & RG.Apos(Mid$(_Stockcode.GeneratedCode.ToUpper, 1, 30)) & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message

        Finally
            'objDBWrite.CloseConnection()
        End Try

        'TODO: Change database here to local DB

        Return "Success"


    End Function
    Public Function UpdateStockcode(ByVal _Stockcode As Stockcode) As String

        Dim _GeneralLayer As New GeneralHODL

        'Generate the barcode if neccessary
        If _Stockcode.Barcode = "" Then
            _Stockcode.Barcode = _GeneralLayer.GenerateSKU
        End If

        Dim CodeCount As Long = 0
        'There are 2 types of updates. A general update and an update that colours have been added.
        'The update queries for master and generated codes are the same, besides for the where, which is a 
        'switch on UpdatedOnMasterCode
        'No need to update size, size grid and colour on normal updates as these cannot change
        'When updating master codes which have generated codes, it's important that the barcode is not updated.
        '(check with (Auto Generated))

        'Check if colours have been added

        If _Stockcode.isGeneratedCode = False Then
            tmpSQL = "SELECT master_code,generated_code FROM stockcodes_master WHERE master_code = '" & RG.Apos(_Stockcode.MasterCode.ToUpper) & "'"
            Try
                'ds = objDBRead.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If dr("master_code") <> "" Then
                            CodeCount += 1
                            tmpSQL = "INSERT INTO stockcodes_price_updates (guid,date_of_change,time_of_change,master_code," &
                                     "generated_code,new_price_1,new_price_2,new_price_3,user_name) " &
                                     " VALUES ('" & Guid.NewGuid.ToString & "','" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "'," &
                                     "'" & dr("master_code") & "','" & dr("generated_code") & "'," &
                                     "'" & RG.Numb(_Stockcode.SellingPrice1, 3) & "','" & RG.Numb(_Stockcode.SellingPrice2, 3) & "'," &
                                     "'" & RG.Numb(_Stockcode.SellingPrice3, 3) & "','WEB')"

                            Try
                                'objDBWrite.ExecuteQuery(tmpSQL)
                                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                            Catch ex As Exception
                                'objDBWrite.CloseConnection()
                                _dlErrorLogging.ErrorLogging(ex)
                                Return ex.Message
                            Finally
                                'objDBWrite.CloseConnection()
                            End Try
                        End If
                    Next
                End If
            Catch ex As Exception
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return Nothing
            Finally
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
            End Try

        Else

            tmpSQL = "SELECT master_code FROM stockcodes_master WHERE generated_code = '" & RG.Apos(_Stockcode.GeneratedCode.ToUpper) & "'"
            Try
                'ds = objDBRead.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If dr("master_code") <> "" Then
                            tmpSQL = "INSERT INTO stockcodes_price_updates (guid,date_of_change,time_of_change,master_code," &
                                     "generated_code,new_price_1,new_price_2,new_price_3,user_name) " &
                                     " VALUES ('" & Guid.NewGuid.ToString & "','" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "'," &
                                     "'" & dr("master_code") & "','" & RG.Apos(_Stockcode.GeneratedCode.ToUpper) & "'," &
                                     "'" & RG.Numb(_Stockcode.SellingPrice1, 3) & "','" & RG.Numb(_Stockcode.SellingPrice2, 3) & "'," &
                                     "'" & RG.Numb(_Stockcode.SellingPrice3, 3) & "','WEB')"

                            Try
                                'objDBWrite.ExecuteQuery(tmpSQL)
                                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                            Catch ex As Exception
                                'objDBWrite.CloseConnection()
                                _dlErrorLogging.ErrorLogging(ex)
                                Return ex.Message
                            Finally
                                'objDBWrite.CloseConnection()
                            End Try
                        End If
                    Next
                End If
            Catch ex As Exception
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return Nothing
            Finally
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
            End Try

        End If

        'Update stockcodes_master
        If CodeCount > 1 Then 'Don't update description when there are generated codes - 2013-10-30: and barcode - and never update the generated codes!
            tmpSQL = "UPDATE stockcodes_master SET " &
               "is_service_item='" & _Stockcode.isServiceItem & "'," &
               "purchase_tax_group='" & _Stockcode.PurchaseTaxGroup & "'," &
               "sales_tax_group='" & _Stockcode.SalesTaxGroup & "',supplier ='" & RG.Apos(_Stockcode.Supplier.ToUpper) & "'," &
               "suppliers_code='" & RG.Apos(_Stockcode.SupplierCode.ToUpper) & "',is_not_discountable='" & _Stockcode.isNotDiscountable & "'," &
               "is_blocked='" & _Stockcode.isBlocked & "',minimum_stock_level='" & Val(_Stockcode.MinumumLevel) & "'," &
               "colour_matrix='" & RG.Apos(_Stockcode.ColourString.ToUpper) & "'"
        Else
            tmpSQL = "UPDATE stockcodes_master SET " &
               "sku_number='" & RG.Apos(_Stockcode.Barcode.ToUpper) & "',is_service_item='" & RG.Apos(_Stockcode.isServiceItem) & "'," &
               "purchase_tax_group='" & _Stockcode.PurchaseTaxGroup & "',description='" & Mid$(RG.Apos(_Stockcode.Description.ToUpper), 1, 50) & "'," &
               "sales_tax_group='" & _Stockcode.SalesTaxGroup & "',supplier ='" & RG.Apos(_Stockcode.Supplier.ToUpper) & "'," &
               "suppliers_code='" & RG.Apos(_Stockcode.SupplierCode.ToUpper) & "',is_not_discountable='" & _Stockcode.isNotDiscountable & "'," &
               "is_blocked='" & _Stockcode.isBlocked & "',minimum_stock_level='" & Val(_Stockcode.MinumumLevel) & "'," &
               "colour_matrix='" & RG.Apos(_Stockcode.ColourString.ToUpper) & "'"
        End If

        If _Stockcode.isGeneratedCode = False Then
            tmpSQL = tmpSQL & " WHERE master_code = '" & RG.Apos(_Stockcode.MasterCode.ToUpper) & "'"
        Else
            tmpSQL = tmpSQL & " WHERE generated_code = '" & RG.Apos(_Stockcode.GeneratedCode.ToUpper) & "'"
        End If

        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try


        'Update categories
        tmpSQL = "UPDATE stockcodes_categories SET " &
                 "category_1='" & RG.Apos(_Stockcode.Category1.ToUpper) & "',category_2='" & RG.Apos(_Stockcode.Category2.ToUpper) & "', " &
                 "category_3='" & RG.Apos(_Stockcode.Category3.ToUpper) & "'"

        If _Stockcode.isGeneratedCode = False Then
            tmpSQL = tmpSQL & " WHERE master_code = '" & RG.Apos(_Stockcode.MasterCode.ToUpper) & "'"
        Else
            tmpSQL = tmpSQL & " WHERE generated_code = '" & RG.Apos(_Stockcode.GeneratedCode.ToUpper) & "'"
        End If

        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try


        'Update prices
        tmpSQL = "UPDATE stockcodes_prices SET " &
                 "estimated_cost='" & RG.Numb(_Stockcode.CostExclusive, 3) & "'," &
                 "selling_price_1='" & RG.Numb(_Stockcode.SellingPrice1, 3) & "',selling_price_2='" & RG.Numb(_Stockcode.SellingPrice2, 3) & "'," &
                 "selling_price_3='" & RG.Numb(_Stockcode.SellingPrice3, 3) & "'"

        If _Stockcode.isGeneratedCode = False Then
            tmpSQL = tmpSQL & " WHERE master_code = '" & RG.Apos(_Stockcode.MasterCode.ToUpper) & "'"
        Else
            tmpSQL = tmpSQL & " WHERE generated_code = '" & RG.Apos(_Stockcode.GeneratedCode.ToUpper) & "'"
        End If

        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        'Update dates
        'Set the updated to now() (Postgres system time) as we don't want a shop downloading before this upload has run
        'and therefore missing the codes in the time difference
        tmpSQL = "UPDATE stockcodes_dates SET updated = now()"

        If _Stockcode.isGeneratedCode = False Then
            tmpSQL = tmpSQL & " WHERE master_code = '" & RG.Apos(_Stockcode.MasterCode.ToUpper) & "'"
        Else
            tmpSQL = tmpSQL & " WHERE generated_code = '" & RG.Apos(_Stockcode.GeneratedCode.ToUpper) & "'"
        End If

        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        End Try

        Return "Success"

    End Function
    Public Function GetStockcode(ByVal SearchCriteria As String, ByVal SearchString As String, ByVal isMasterCode As Boolean) As DataTable


        Dim xData As New DataTable

        Try
            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            If isMasterCode = True Then
                If SearchCriteria = "Master Code" Then
                    tmpSQL = "SELECT master_code AS stockcode,description,sku_number FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE master_code LIKE " &
                              "'" & RG.Apos(SearchString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC LIMIT 50"
                ElseIf SearchCriteria = "Barcode" Then
                    tmpSQL = "SELECT master_code AS stockcode,description,sku_number FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE sku_number LIKE " &
                              "'" & RG.Apos(SearchString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC LIMIT 50"
                ElseIf SearchCriteria = "Description" Then
                    tmpSQL = "SELECT master_code,description,sku_number FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE description LIKE " &
                             "'" & RG.Apos(SearchString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC LIMIT 50"
                End If
            Else
                If SearchCriteria = "Generated Code" Then
                    tmpSQL = "SELECT generated_code AS stockcode,description,sku_number FROM stockcodes_master WHERE generated_code LIKE " &
                             "'" & RG.Apos(SearchString.ToUpper) & "%' ORDER BY generated_code ASC LIMIT 50"
                ElseIf SearchCriteria = "Barcode" Then
                    tmpSQL = "SELECT generated_code AS stockcode,description,sku_number FROM stockcodes_master WHERE sku_number LIKE " &
                             "'" & RG.Apos(SearchString.ToUpper) & "%' ORDER BY sku_number ASC LIMIT 50"
                ElseIf SearchCriteria = "Description" Then
                    tmpSQL = "SELECT generated_code AS stockcode,description,sku_number FROM stockcodes_master WHERE description LIKE " &
                             "'" & RG.Apos(SearchString.ToUpper) & "%' ORDER BY description ASC LIMIT 50"
                End If
            End If

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

        Catch ex As Exception
            'Probably a databind on clearing the page
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetPrePacks(ByVal SizeGrid As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT DISTINCT(pre_pack_code) AS pre_pack_code FROM stock_pre_packs WHERE size_grid = '" & RG.Apos(SizeGrid) & "'"

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetPrePackData(ByVal SizeGrid As String, ByVal PrePackCode As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT * FROM stock_pre_packs WHERE size_grid = '" & RG.Apos(SizeGrid) & "' AND pre_pack_code = '" & RG.Apos(PrePackCode) & "' ORDER BY size_grid"

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetCategoryList(ByVal CategoryNumber As String, ByVal SearchString As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT category_code,category_description " &
                     "FROM categories " &
                     "WHERE category_number = '" & CategoryNumber & "' " &
                     "AND (category_code LIKE '%" & SearchString & "%' OR category_description LIKE '%" & SearchString & "%') " &
                     "ORDER BY category_code"

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetSizeGridDescription(ByVal GridNumber As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT * FROM size_grids WHERE grid_number = '" & GridNumber & "'"

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetSupplierList(ByVal SearchString As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT supplier_code,supplier_name " &
                     "FROM supplier_details " &
                     "WHERE supplier_code LIKE '%" & SearchString & "%' OR supplier_name LIKE '%" & SearchString & "%' LIMIT 10"

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetColourGridDescription(ByVal ColourGridCode As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT colour_description FROM colour_grids WHERE colour_code = '" & RG.Apos(ColourGridCode.ToUpper) & "'"

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetSupplier(ByVal SearchString As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT supplier_code,supplier_name " &
                     "FROM supplier_details " &
                     "WHERE supplier_code = '" & SearchString & "'"
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function DeleteMasterCode(ByVal _Stockcode As String, ByVal Username As String, ByVal Email As String, ByVal json As String) As String

        tmpSQL = "SELECT guid from transaction_line_items WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "' LIMIT 1"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                'Transactions exist
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return "Failed"

            End If

            DeleteStockCode(Username, Email, json)
            Return "Success"

        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            _dlErrorLogging.ErrorLogging(ex)
            Return "Failed"
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try


        'tmpSQL = "DELETE FROM stock_on_hand WHERE generated_code LIKE '" & RG.Apos(_Stockcode.ToUpper) & "%';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return "Failed"
        'End Try

        'tmpSQL = "DELETE FROM stock_bestsellers_monthly WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return "Failed"
        'End Try

        'tmpSQL = "DELETE FROM stock_bestsellers_weekly WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return "Failed"
        'End Try

        'tmpSQL = "DELETE FROM stockcodes_categories WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return "Failed"
        'End Try

        'tmpSQL = "DELETE FROM stockcodes_dates WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return "Failed"
        'End Try

        'tmpSQL = "DELETE FROM stockcodes_prices WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return "Failed"
        'End Try

        'tmpSQL = "DELETE FROM stockcodes_price_updates WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return "Failed"
        'End Try

        'tmpSQL = "DELETE FROM stockcodes_minimum_levels WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return ex.Message
        'End Try

        'tmpSQL = "DELETE FROM stockcodes_price_updates WHERE generated_code LIKE '" & RG.Apos(_Stockcode.ToUpper) & "%';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return "Failed"
        'End Try

        'tmpSQL = "DELETE FROM stockcodes_master WHERE master_code = '" & RG.Apos(_Stockcode.ToUpper) & "';"
        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL)
        'Catch ex As Exception
        '    objDBWrite.CloseConnection()
        '    _dlErrorLogging.ErrorLogging(ex)
        '    Return ex.Message
        'Finally
        '    objDBWrite.CloseConnection()
        'End Try


    End Function
    Public Function GetMasterCodeInfo(ByVal Stockcode As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "Select " &
                     "stockcodes_master.master_code,stockcodes_master.generated_code,stockcodes_master.sku_number,stockcodes_master.is_service_item," &
                     "stockcodes_master.description,stockcodes_master.purchase_tax_group,stockcodes_master.sales_tax_group,stockcodes_master.supplier," &
                     "stockcodes_master.suppliers_code,stockcodes_master.is_not_discountable,stockcodes_master.is_blocked," &
                     "stockcodes_master.minimum_stock_level,stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix," &
                     "stockcodes_master.colour_matrix,stockcodes_categories.category_1,stockcodes_categories.category_2,stockcodes_categories.category_3," &
                     "stockcodes_prices.selling_price_3,stockcodes_prices.selling_price_2,stockcodes_prices.selling_price_1," &
                     "stockcodes_prices.estimated_cost,stockcodes_prices.cost_price " &
                     "FROM " &
                     "stockcodes_master " &
                     "LEFT OUTER JOIN stockcodes_prices On stockcodes_master.generated_code = stockcodes_prices.generated_code " &
                     "LEFT OUTER JOIN stockcodes_categories On stockcodes_master.generated_code = stockcodes_categories.generated_code " &
                     "WHERE stockcodes_master.master_code = '" & RG.Apos(Stockcode.ToUpper) & "'"

            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetGeneratedCodeInfo(ByVal Stockcode As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT " &
                 "stockcodes_master.master_code,stockcodes_master.generated_code,stockcodes_master.sku_number,stockcodes_master.is_service_item," &
                 "stockcodes_master.description,stockcodes_master.purchase_tax_group,stockcodes_master.sales_tax_group,stockcodes_master.supplier," &
                 "stockcodes_master.suppliers_code,stockcodes_master.is_not_discountable,stockcodes_master.is_blocked," &
                 "stockcodes_master.minimum_stock_level,stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix," &
                 "stockcodes_master.colour_matrix,stockcodes_categories.category_1,stockcodes_categories.category_2,stockcodes_categories.category_3," &
                 "stockcodes_prices.selling_price_3,stockcodes_prices.selling_price_2,stockcodes_prices.selling_price_1," &
                 "stockcodes_prices.estimated_cost,stockcodes_prices.cost_price " &
                 "FROM " &
                 "stockcodes_master " &
                 "LEFT OUTER JOIN stockcodes_prices ON stockcodes_master.generated_code = stockcodes_prices.generated_code " &
                 "LEFT OUTER JOIN stockcodes_categories ON stockcodes_master.generated_code = stockcodes_categories.generated_code " &
                 "WHERE stockcodes_master.generated_code = '" & RG.Apos(Stockcode.ToUpper) & "'"

            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetSingleStockOnHand(ByVal GeneratedCode As String, ByVal BranchCode As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT qty_on_hand FROM stock_on_hand WHERE generated_code = '" & RG.Apos(GeneratedCode) & "' " &
                     "AND branch_code = '" & RG.Apos(BranchCode) & "'"

            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetColourGrids() As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT colour_code,colour_description FROM colour_grids ORDER BY colour_code ASC"

            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetSizeGrids() As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT grid_number,grid_description FROM size_grids ORDER BY grid_number ASC"

            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function DeleteStockCode(ByVal Username As String, ByVal Email As String, ByVal json As String) As String


        tmpSQL = "INSERT INTO tasks (
                                    task_type,
                                    file_to_run,
                                    timestamp_created,
                                    username,
                                    email_addresses,
                                    start_date,
                                    end_date,
                                    additional_options
                                    ) 
                                    VALUES " &
                                   "(
                                   'delete_mastercode',
                                   '',
                                   '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                    '" & Username & "',
                                   '" & Email & "',
                                   '',
                                   '',
                                   '" & json & "'
                                   )"


        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            _dlErrorLogging.ErrorLogging(ex)
            Return ex.Message
        Finally
            'objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function
    Public Function GetStockcodes(ByVal MasterCode As String) As DataTable
        Dim xData As New DataTable

        Try
            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT master_code AS stockcode,description,sku_number FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE master_code LIKE " &
                      "'" & RG.Apos(MasterCode.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC"
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter()
            'reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xData

    End Function
    Public Function GetStockOnHandByBranch(ByVal Mastercode As String) As DataTable
        Dim xData As New DataTable
        Dim generated_codes As String = String.Empty
        Dim cases As String = String.Empty
        Dim query As String = String.Empty
        Dim yData As New DataTable


        Try
            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            tmpSQL = "select generated_code from stockcodes_master where master_code='" & Mastercode & "'"
            xData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            'Dim reader As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
            'reader.Fill(xData)

            If xData IsNot Nothing AndAlso xData.Rows.Count > 0 Then
                For Each dr As DataRow In xData.Rows
                    cases &= "sum(case when generated_code = '" & dr("generated_code") & "' then qty_on_hand END) as " & dr("generated_code").ToString.ToUpper & ","
                    generated_codes &= "'" & dr("generated_code").ToString.ToString & "'" & ","
                Next

                cases = cases.Remove(cases.Length - 1)
                generated_codes = generated_codes.Remove(generated_codes.Length - 1)

                tmpSQL = "select stock_on_hand.branch_code as branchcode,
                             MAX(branch_name) as BranchName,
                            " & cases & "
                            from stock_on_hand
                            left outer join branch_details bd on bd.branch_code = stock_on_hand.branch_code 
                            where generated_code in(" & generated_codes & ")
                            GROUP BY stock_on_hand.branch_code
                            order by stock_on_hand.branch_code"
                yData = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
                'Dim getdatatable As New NpgsqlDataAdapter(tmpSQL, _POSReadConnectionString)
                'getdatatable.Fill(yData)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return yData
    End Function
End Class
