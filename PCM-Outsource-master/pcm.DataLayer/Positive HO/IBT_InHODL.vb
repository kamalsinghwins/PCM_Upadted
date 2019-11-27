Imports System.IO
Imports Entities.IBT_In
Imports Entities.IBT_Out
Imports Npgsql
Public Class IBT_InHODL
    Inherits DataAccessLayerBase
    Dim _DLayer As IBT_OutHODL
    Dim getDetailsResponse As New FetchDetailsResponse
    Dim generatedCodeInfoResponse As New GetGeneratedCodeInfoResponse
    Dim getIbt_InNumber As New GetIbt_InNumber


    Public Sub New()
        _DLayer = New IBT_OutHODL
    End Sub

    Public Function FetchDetails(ByVal detailsRequest As FetchDetails) As FetchDetailsResponse
        Dim tmpTaxArray() As String
        Dim tmpInTaxValue As String
        Dim TaxGroup As String
        Dim SellingInclusive As String
        Dim Current_Branch_Code = "HHH"
        Dim list As New List(Of Data)


        'If CheckRecordExistsLocal("upload_pos", "transaction_number", detailsRequest.IBTOutNumber) Then
        '    getDetailsResponse.Success = False
        '    getDetailsResponse.Message = "Please wait for the Uploader to Upload the current data."
        '    Return getDetailsResponse
        'End If

        'tmpSQL = "SELECT SUM(received_qty) FROM ibt_transactions WHERE ibt_transactions.sending_branch_code = '" & RG.Apos(detailsRequest.FromBranch) & "' " &
        '         "AND ibt_transactions.transaction_number = '" & RG.Apos(detailsRequest.IBTOutNumber.ToUpper) & "' " &
        '         "AND ibt_transactions.receiving_branch_code = '" & Current_Branch_Code.ToUpper & "'"

        tmpSQL = "SELECT SUM(received_qty) FROM ibt_transactions WHERE ibt_transactions.sending_branch_code = '" & RG.Apos(detailsRequest.FromBranch.ToUpper) & "' " &
          " And ibt_transactions.transaction_number = '" & RG.Apos(detailsRequest.IBTOutNumber.ToUpper) & "' " &
               "AND ibt_transactions.receiving_branch_code = '" & Current_Branch_Code.ToUpper & "'"
        Try

            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                If Val(Ds.Tables(0).Rows(0)("SUM") & "") <> 0 Then
                    getDetailsResponse.Success = False
                    getDetailsResponse.Message = "The IBT Number you have entered is not valid or has already been confirmed."
                    Return getDetailsResponse
                End If
            Else
                getDetailsResponse.Success = False
                getDetailsResponse.Message = "The IBT Number you have entered is not valid or has already been confirmed."
                Return getDetailsResponse
            End If

            tmpSQL = "SELECT " &
                     "ibt_transactions.sending_branch_code, " &
                     "ibt_transactions.transaction_number, " &
                     "ibt_transactions.receiving_branch_code, " &
                     "ibt_transactions.sent_qty, " &
                     "ibt_transactions.generated_code, " &
                     "ibt_transactions.selling_tax_group, " &
                     "stockcodes_master.master_code, " &
                     "stockcodes_master.description, " &
                     "stockcodes_master.purchase_tax_group, " &
                     "stockcodes_master.supplier, " &
                     "stockcodes_master.suppliers_code, " &
                     "stockcodes_categories.category_1, " &
                     "stockcodes_categories.category_2, " &
                     "stockcodes_categories.category_3, " &
                     "stockcodes_master.item_size, " &
                     "stockcodes_master.size_matrix, " &
                     "stockcodes_master.item_colour, " &
                     "stockcodes_master.colour_matrix, " &
                     "stockcodes_master.is_service_item, " &
                     "stockcodes_prices.estimated_cost, " &
                     "stockcodes_prices.cost_price, " &
                     "stockcodes_prices.selling_price_1 " &
                     "FROM " &
                     "ibt_transactions " &
                     "INNER JOIN stockcodes_master ON ibt_transactions.generated_code = stockcodes_master.generated_code " &
                     "INNER JOIN stockcodes_categories On stockcodes_categories.generated_code = stockcodes_master.generated_code " &
                     "INNER JOIN stockcodes_prices ON stockcodes_prices.generated_code = stockcodes_master.generated_code " &
                     "WHERE ibt_transactions.sending_branch_code = '" & RG.Apos(detailsRequest.FromBranch.ToUpper) & "' " &
                     "AND ibt_transactions.transaction_number = '" & RG.Apos(detailsRequest.IBTOutNumber.ToUpper) & "' " &
                     "AND ibt_transactions.receiving_branch_code =  '" & RG.Apos(Current_Branch_Code.ToUpper) & "' " &
                     "AND ibt_transactions.received_qty = 0 order by ibt_transactions.generated_code asc"


            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then

                For Each dr As DataRow In Ds.Tables(0).Rows
                    If dr("sending_branch_code") <> "" Then
                        generatedCodeInfoResponse = _DLayer.GetGeneratedCodeInfo(dr("generated_code").ToUpper, detailsRequest.FromBranch)
                        If generatedCodeInfoResponse.GetGeneratedCodeInfo = False Then
                            getDetailsResponse.Message = "There is a code in this IBT that does not exist in the database. " & vbCrLf & "You cannot Accept this IBT." & vbCrLf & "Please click on the 'Sync' button " &
                                "in the Positive Uploader to get the lastest stockcodes. " & vbCrLf & "Error with code " & dr("generated_code")
                            getDetailsResponse.Success = False
                            Return getDetailsResponse

                        End If

                        Dim listData As New Data
                        listData.ItemCode = dr("generated_code") '0
                        listData.Description = dr("description") '1
                        listData.Excl = RG.Numb(dr("selling_price_1")) 'Excl '2 
                        listData.Tax = dr("selling_tax_group") '3

                        '================================================================================
                        'Get the TAX percentage
                        TaxGroup = dr("selling_tax_group")

                        tmpTaxArray = Split(TaxGroup, "(")
                        tmpInTaxValue = Val(Mid$(tmpTaxArray(1), 1, Len(tmpTaxArray(1)) - 1))

                        SellingInclusive = RG.Numb(((Val(dr("selling_price_1")) * tmpInTaxValue) / 100) + Val(dr("selling_price_1")))
                        '================================================================================
                        listData.Incl = (RG.Numb(SellingInclusive)) '4
                        listData.Qty = (RG.Numb(dr("sent_qty"))) '5
                        listData.TotalCostIncl = (RG.Numb(Val(dr("sent_qty")) * Val(SellingInclusive))) '6
                        listData.MCode = (dr("master_code")) '7
                        listData.Cat1 = (dr("category_1")) '8
                        listData.Cat2 = (dr("category_2")) '9
                        listData.Cat3 = (dr("category_3")) '10
                        listData.ItemSize = (dr("item_size")) '11
                        listData.SizeGrid = (dr("size_matrix")) '12
                        listData.Colour = (dr("item_colour")) '13
                        listData.ColourGrid = (dr("colour_matrix")) '14
                        listData.OriginalCost = (RG.Numb((Val(SellingInclusive) - Val(dr("selling_price_1"))) * Val(dr("sent_qty")))) '15
                        listData.SellingPrice1 = (dr("selling_price_1")) '16
                        listData.Supplier = (dr("supplier")) '17
                        listData.SupplierCode = (dr("suppliers_code")) '18
                        listData.IsServiceItem = (dr("is_service_item")) '19
                        listData.CostPrice = (RG.Numb(dr("estimated_cost"))) '20
                        listData.SellingTaxGroup = (dr("selling_tax_group")) '21
                        listData.Quantity = (dr("sent_qty")) '22
                        list.Add(listData)

                    End If
                Next

            Else
                getDetailsResponse.Success = False
                getDetailsResponse.Message = "The IBT Number you have entered is not valid or has already been confirmed."
                Return getDetailsResponse

            End If

            getDetailsResponse.Data = list
            getDetailsResponse.Success = True
            Return getDetailsResponse

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Function GetNum(ByVal TransactionType As String, ByVal Current_Branch_Code As String) As GetIbt_InNumber

        tmpSQL = "SELECT nextval('" & Current_Branch_Code.ToUpper() & "_ibtin_seq')"
        Try

            Ds = usingObjDB.GetDataSet(_POSWriteConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                getIbt_InNumber.Number = Ds.Tables(0).Rows(0)("nextval") & ""
            Else

                getIbt_InNumber.Message = "Error retrieving number."
                getIbt_InNumber.Success = False
                Return getIbt_InNumber
            End If

        Catch ex As Exception
            Throw ex
        End Try

        getIbt_InNumber.Success = True
        Return getIbt_InNumber

    End Function
    Public Function GenerateGUID() As String
        GenerateGUID = Guid.NewGuid.ToString

    End Function

    Public Function CheckRecordExistsLocal(ByVal TableName As String, ByVal ColumnName As String, ByVal RecordToCheck As String,
                                     Optional ByVal ColumnName2 As String = "", Optional ByVal RecordToCheck2 As String = "",
                                     Optional ByVal ColumnName3 As String = "", Optional ByVal RecordToCheck3 As String = "") As Boolean

        tmpSQL = "SELECT " & ColumnName & " FROM " & TableName & " WHERE " & ColumnName & " = '" & RG.Apos(RecordToCheck) & "'"
        If ColumnName2 <> "" Then
            tmpSQL = tmpSQL & " AND " & ColumnName2 & " = '" & RG.Apos(RecordToCheck2) & "'"
        End If
        If ColumnName3 <> "" Then
            tmpSQL = tmpSQL & " AND " & ColumnName3 & " = '" & RG.Apos(RecordToCheck3) & "'"
        End If

        Try

            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                CheckRecordExistsLocal = True
            Else
                CheckRecordExistsLocal = False
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class
