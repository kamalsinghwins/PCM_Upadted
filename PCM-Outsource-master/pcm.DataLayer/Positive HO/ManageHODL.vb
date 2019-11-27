Imports Entities
Imports Entities.Manage
Imports Entities.SizeMatrix

Public Class ManageHODL
    Inherits DataAccessLayerBase
    Dim _DLayer As New GeneralHODL
    Dim baseResponse As New BaseResponse
    Dim searchGridResponse As New GridResponse
    Dim searchResponse As New SearchResponse
    Dim stockResponse As New AddStockResponse
#Region "Branch"
    Public Function GetBranches(ByVal SearchText As String, ByVal SearchDetails As String) As DataTable

        If SearchText = "Branch Code" Then
            tmpSQL = "SELECT branch_code,branch_name FROM branch_details WHERE branch_code LIKE '" & RG.Apos(SearchDetails.ToUpper) & "%' ORDER BY branch_code ASC "
        ElseIf SearchText = "Branch Name" Then
            tmpSQL = "SELECT branch_code,branch_name FROM branch_details WHERE branch_name LIKE '" & RG.Apos(SearchDetails.ToUpper) & "%' ORDER BY branch_name ASC "
        End If

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function
    Public Function GetBranchDetails(ByVal BranchCode As String) As DataTable

        tmpSQL = "SELECT * FROM branch_details WHERE branch_code = '" & RG.Apos(BranchCode) & "'"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function DeleteBranch(ByVal BranchCode As String, ByVal BranchName As String) As BaseResponse

        tmpSQL = "SELECT is_head_office FROM branch_details WHERE branch_code = '" & BranchCode & "'"

        Try

            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                If Ds.Tables(0).Rows(0)("is_head_office") = True Then
                    baseResponse.Message = BranchCode & " is a Head Office branch. You may not delete it."
                    baseResponse.Success = False
                    Return baseResponse
                End If
            Else
                baseResponse.Message = "Branch Code " & BranchCode.ToUpper & " does not exist"
                baseResponse.Success = False
                Return baseResponse
            End If


            'Check for stock on hand before allowing delete
            tmpSQL = "SELECT SUM(qty_on_hand) FROM stock_on_hand WHERE branch_code = '" & RG.Apos(BranchCode.ToUpper) & "'"
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                If Val(Ds.Tables(0).Rows(0)("sum").ToString) <> 0 Then
                    baseResponse.Message = "There is Stock on Hand for this branch." & vbCrLf & "Please zero all stock items before deleting a branch." & vbCrLf & "Branch " & BranchCode & " " & BranchName.ToUpper & " was not deleted."
                    baseResponse.Success = False
                    Return baseResponse
                End If
            End If


            'Delete the stockcodes from the stock on hand table
            tmpSQL = "DELETE FROM stock_on_hand WHERE branch_code = '" & RG.Apos(BranchCode.ToUpper) & "'"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

            'Delete the stockcodes from the stock minimum levels table
            tmpSQL = "DELETE FROM stockcodes_minimum_levels WHERE branch_code = '" & RG.Apos(BranchCode.ToUpper) & "'"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

            'Delete from version_numbers
            tmpSQL = "DELETE FROM version_numbers WHERE branch_code = '" & RG.Apos(BranchCode.ToUpper) & "'"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

            'Delete from the branches table
            tmpSQL = "DELETE FROM branch_details WHERE branch_code = '" & RG.Apos(BranchCode.ToUpper) & "'"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

            baseResponse.Success = True
            baseResponse.Message = "deleted successfully"

        Catch ex As Exception
            Throw ex
        End Try
        Return baseResponse
    End Function
    Public Function SaveBranch(ByVal branchDetails As BranchDetails) As BaseResponse
        tmpSQL = "SELECT is_head_office FROM branch_details WHERE branch_code = '" & branchDetails.BranchCode.ToUpper & "'"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                If Ds.Tables(0).Rows(0)("is_head_office") = True Then
                    If branchDetails.Blocked = True Then
                        baseResponse.Message = branchDetails.BranchCode & " " & branchDetails.BranchName.ToUpper & " is a Head Office branch. You cannot block a Head Office branch."
                        baseResponse.Success = False
                        Return baseResponse
                    End If
                End If
            End If

            tmpSQL = "SELECT branch_code FROM branch_details WHERE branch_code = '" & RG.Apos(branchDetails.BranchCode.ToUpper) & "'"
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then

                tmpSQL = "UPDATE branch_details SET " &
                            "branch_name = '" & RG.Apos(branchDetails.BranchName.ToUpper) & "'," &
                            "address_line_1 = '" & RG.Apos(branchDetails.AddressLine1) & "'," &
                            "address_line_2 = '" & RG.Apos(branchDetails.AddressLine2) & "'," &
                            "address_line_3 = '" & RG.Apos(branchDetails.AddressLine3) & "'," &
                            "address_line_4 = '" & RG.Apos(branchDetails.AddressLine4) & "'," &
                            "address_line_5 = '" & RG.Apos(branchDetails.AddressLine5) & "'," &
                            "telephone_number = '" & RG.Apos(branchDetails.TelephoneNumber) & "'," &
                            "fax_number = '" & RG.Apos(branchDetails.Fax) & "'," &
                            "email_address = '" & RG.Apos(branchDetails.Email) & "'," &
                            "pricelevel = '" & RG.Apos(branchDetails.Price) & "'," &
                            "tax_number = '" & RG.Apos(branchDetails.TAX) & "'," &
                            "is_blocked = '" & branchDetails.Blocked & "'," &
                            "branch_type = '" & branchDetails.BranchType & "'," &
                            "merchant_number = '" & RG.Apos(branchDetails.MerchantNumber) & "'," &
                            "municipality = '" & RG.Apos(branchDetails.Municipality) & "'," &
                            "province = '" & RG.Apos(branchDetails.Province) & "'," &
                            "store_square_metres = '" & branchDetails.StoreSquareMetres & "'," &
                            "trading_hour_start = '" & RG.Apos(branchDetails.TradingHourStart) & "'," &
                            "trading_hour_end = '" & RG.Apos(branchDetails.TradingHourEnd) & "'," &
                            "region = '" & RG.Apos(branchDetails.Region) & "'," &
                            "longitude = '" & RG.Apos(branchDetails.Longitude) & "'," &
                            "latitude = '" & RG.Apos(branchDetails.Latitude) & "'," &
                            "mall_type = '" & RG.Apos(branchDetails.TypeOfMall) & "'," &
                            "company_name = '" & RG.Apos(branchDetails.CompanyName) & "'," &
                            "url = '" & RG.Apos(branchDetails.URL) & "'," &
                            "branch_name_web='" & RG.Apos(branchDetails.BranchNameWeb) & "'," &
                            "store_status='" & RG.Apos(branchDetails.StoreStatus) & "'," &
                            "updated = '" & DateTime.Now.ToString("yyyy-MM-dd") & "'" &
                            " WHERE branch_code = '" & RG.Apos(branchDetails.BranchCode.ToUpper) & "'"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Message = "Branch " & branchDetails.BranchName & " updated."

            Else
                'Insert into company table
                tmpSQL = "INSERT INTO branch_details (branch_code,
                                                      branch_name,
                                                      address_line_1,
                                                      address_line_2,
                                                      address_line_3,
                                                      address_line_4,
                                                     " &
                                                     "address_line_5,
                                                     telephone_number,
                                                     fax_number,
                                                     email_address,
                                                     tax_number,
                                                     pricelevel,
                                                     is_head_office,
                                                     is_blocked,
                                                     branch_type,
                                                     inserted,
                                                     merchant_number,
                                                     region,
                                                     municipality,
                                                     province,
                                                     store_square_metres,
                                                     trading_hour_start,
                                                     trading_hour_end,
                                                     longitude,
                                                     latitude,
                                                     mall_type,
                                                     company_name,
                                                     url,
                                                     branch_name_web,
                                                     store_status
                                                     ) 
                                                     VALUES " &
                                                    "('" & RG.Apos(branchDetails.BranchCode.ToUpper) & "',
                                                      '" & RG.Apos(branchDetails.BranchName.ToUpper) & "',
                                                    '" & RG.Apos(branchDetails.AddressLine1) & "',
                                                      '" & RG.Apos(branchDetails.AddressLine2) & "',
                                                    " &
                                                    "'" & RG.Apos(branchDetails.AddressLine3) & "',
                                                    '" & RG.Apos(branchDetails.AddressLine4) & "',
                                                    '" & RG.Apos(branchDetails.AddressLine5) & "',
                                                    '" & RG.Apos(branchDetails.TelephoneNumber) & "'," &
                                                    "'" & RG.Apos(branchDetails.Fax) & "',
                                                    '" & RG.Apos(branchDetails.Email) & "',
                                                    '" & RG.Apos(branchDetails.TAX) & "',
                                                    '" & branchDetails.Price & "',
                                                    'False',
                                                    '" & branchDetails.Blocked & "',
                                                    " &
                                                    "'" & branchDetails.BranchType & "',
                                                    '" & DateTime.Now.ToString("yyyy-MM-dd") & "',
                                                    '" & RG.Apos(branchDetails.MerchantNumber) & "',
                                                    '" & RG.Apos(branchDetails.Region) & "',
                                                    '" & RG.Apos(branchDetails.Municipality) & "',
                                                    '" & RG.Apos(branchDetails.Province) & "',
                                                    '" & RG.Apos(branchDetails.StoreSquareMetres) & "',
                                                    '" & RG.Apos(branchDetails.TradingHourStart) & "',
                                                    '" & RG.Apos(branchDetails.TradingHourEnd) & "',
                                                    '" & RG.Apos(branchDetails.Longitude) & "',
                                                    '" & RG.Apos(branchDetails.Latitude) & "',
                                                    '" & RG.Apos(branchDetails.TypeOfMall) & "',
                                                    '" & RG.Apos(branchDetails.CompanyName) & "',
                                                    '" & RG.Apos(branchDetails.URL) & "',
                                                    '" & RG.Apos(branchDetails.BranchNameWeb) & "',
                                                    '" & RG.Apos(branchDetails.StoreStatus) & "'
                                                    )"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)



                'Create branch as field in stockcode
                tmpSQL = "INSERT INTO stock_on_hand (generated_code,branch_code) SELECT generated_code,'" & RG.Apos(branchDetails.BranchCode.ToUpper) & "' FROM stockcodes_master WHERE is_blocked = False"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

                'Create branch as field in stockcode
                tmpSQL = "INSERT INTO stockcodes_minimum_levels (master_code,generated_code,branch_code) SELECT master_code,generated_code,'" & RG.Apos(branchDetails.BranchCode.ToUpper) & "' FROM stockcodes_master WHERE is_blocked = False"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

                'Create branch as field in version_numbers
                tmpSQL = "INSERT INTO version_numbers (branch_code) VALUES ('" & RG.Apos(branchDetails.BranchCode.ToUpper) & "')"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

                baseResponse.Message = "Branch " & branchDetails.BranchName.ToUpper & " created."

            End If

            baseResponse.Success = True

        Catch ex As Exception
            Throw ex
        End Try
        Return baseResponse
    End Function
    Public Function GetAllBranches() As DataTable
        tmpSQL = "SELECT branch_code,
                  branch_name,
                  no_stock_until,
                  municipality
                  FROM branch_details 
                  WHERE is_blocked = 'False'
                  ORDER by branch_code ASC"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function
#End Region
#Region "Stationary"
    Public Function GetStationaryCodes(ByVal SearchType As String, ByVal SearchText As String) As DataTable

        If SearchType = "Stationary Code" Then
            tmpSQL = "SELECT stationary_code,
                            stationary_description
                            FROM stationary
                           WHERE stationary_code LIKE '" & RG.Apos(SearchText.ToUpper) & "%' ORDER BY stationary_code ASC "

        ElseIf SearchType = "Stationary Description" Then
            tmpSQL = "SELECT stationary_code,
                            stationary_description 
                            FROM stationary 
                            WHERE stationary_description LIKE '" & RG.Apos(SearchText.ToUpper) & "%' ORDER BY stationary_description ASC "
        End If


        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function GetStationaryDetails(ByVal StationaryCode As String) As DataTable

        tmpSQL = "SELECT * FROM stationary WHERE stationary_code = '" & RG.Apos(StationaryCode.ToUpper) & "' "

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function SaveStationary(ByVal Code As String, ByVal Description As String, ByVal Read As Boolean) As BaseResponse

        Try

            If Read = True Then
                tmpSQL = "UPDATE stationary SET stationary_description = '" & RG.Apos(Description) & "' " &
                            "WHERE stationary_code = '" & RG.Apos(Code) & "'"
                usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)
            Else
                tmpSQL = "SELECT * FROM stationary WHERE stationary_code = '" & RG.Apos(Code) & "'"
                Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                If usingObjDB.isR(Ds) Then
                    tmpSQL = "UPDATE stationary SET stationary_description = '" & RG.Apos(Description) & "' " &
                             "WHERE stationary_code = '" & RG.Apos(Code) & "'"
                    usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)
                Else

                    tmpSQL = "INSERT INTO stationary (stationary_code,stationary_description) VALUES (" &
                             "'" & RG.Apos(Code) & "','" & RG.Apos(Description) & "')"
                    usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)
                End If

            End If

            baseResponse.Success = True
            baseResponse.Message = "Stationary File Updated"
        Catch ex As Exception
            Throw ex
        End Try

        Return baseResponse
    End Function
    Public Function DeleteStationary(ByVal StationaryCode As String) As BaseResponse
        tmpSQL = "DELETE FROM stationary WHERE stationary_code = '" & RG.Apos(StationaryCode) & "'"

        Try
            usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        baseResponse.Success = True
        baseResponse.Message = "Stationary code deleted."
        Return baseResponse
    End Function
#End Region
#Region "ColourMatrix"
    Public Function GetColourCodes(ByVal SearchType As String, ByVal SearchText As String) As DataTable

        If SearchType = "Colour Code" Then
            tmpSQL = "SELECT colour_code,
                             colour_description 
                             FROM colour_grids 
                             WHERE colour_code LIKE '" & RG.Apos(SearchText.ToUpper) & "%' ORDER BY colour_code ASC "


        ElseIf SearchType = "Colour Description" Then
            tmpSQL = "SELECT colour_code,
                      colour_description
                      FROM colour_grids
                      WHERE colour_description LIKE '" & RG.Apos(SearchText.ToUpper) & "%' ORDER BY colour_description ASC "

        End If


        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function SaveColourCode(ByVal Code As String, ByVal Description As String) As BaseResponse

        Try

            tmpSQL = "SELECT colour_code FROM colour_grids WHERE colour_code = '" & RG.Apos(Code.ToUpper) & "'"
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                tmpSQL = "UPDATE colour_grids SET colour_description = '" & RG.Apos(Description.ToUpper) & "',updated = '" & DateTime.Now.ToString("yyyy-MM-dd") & "' WHERE colour_code = '" & RG.Apos(Code.ToUpper) & "'"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Message = "Colour updated successfully."

            Else

                tmpSQL = "INSERT INTO colour_grids (colour_code,colour_description,inserted) VALUES ('" & RG.Apos(Code.ToUpper) & "','" & RG.Apos(Description.ToUpper) & "','" & DateTime.Now.ToString("yyyy-MM-dd") & "')"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Message = "Colour inserted successfully."
            End If

            baseResponse.Success = True
        Catch ex As Exception
            Throw ex
        End Try

        Return baseResponse
    End Function
#End Region
#Region "SizeMatrix"
    Public Function GetSizeGridDetails(ByVal GridNumber As String) As GridResponse
        tmpSQL = "Select * FROM size_grids WHERE grid_number = '" & RG.Apos(GridNumber.ToUpper) & "' "

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                searchGridResponse.searchGridList = Ds.Tables(0)
                searchGridResponse.Success = True

            Else
                searchGridResponse.Success = False
                searchGridResponse.Message = "No Data found"
            End If
            Return searchGridResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteSizeGrid(ByVal GridNumber As String) As BaseResponse

        tmpSQL = "SELECT generated_code FROM stockcodes_master WHERE size_matrix = '" & RG.Apos(GridNumber.ToUpper) & "'"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                baseResponse.Success = False
                baseResponse.Message = "This size grid is assigned to an item code. Please remove the item code before you continue."
                Return baseResponse
            End If

            tmpSQL = "DELETE FROM size_grids WHERE grid_number = '" & RG.Apos(GridNumber.ToUpper) & "'"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            baseResponse.Success = False
            baseResponse.Message = "Size matrix deleted successfully."
        Catch ex As Exception
            Throw ex
        End Try
        Return baseResponse

    End Function
    Public Function GetGrids(ByVal SearchType As String, ByVal SearchText As String) As GridResponse

        If SearchType = "Grid Number" Then
            tmpSQL = "SELECT grid_number,
                             grid_description
                            FROM size_grids 
                            WHERE grid_number LIKE '" & RG.Apos(SearchText.ToUpper) & "%' ORDER BY grid_number ASC"

        ElseIf SearchType = "Grid Description" Then
            tmpSQL = "SELECT grid_number,
                             grid_description 
                            FROM size_grids
                           WHERE grid_description LIKE '" & RG.Apos(SearchText.ToUpper) & "%' ORDER BY grid_description ASC"
        End If


        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                searchGridResponse.searchGridList = Ds.Tables(0)
                searchGridResponse.Success = True

            Else
                searchGridResponse.Success = False
                searchGridResponse.Message = "No Data to display"
            End If
            Return searchGridResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveSizeGrid(ByVal SaveGrid As SaveGrid) As BaseResponse

        Dim tmpSizeGrids As String      'Loop through the matrix. Used for fieldnames in tmpSQL
        Dim tmpRowValues(3) As String   'Used to get values for the associated grid numbers

        Try

            tmpSQL = "SELECT grid_number FROM size_grids WHERE grid_number = '" & RG.Apos(SaveGrid.GridNumber.ToUpper) & "'"
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                tmpSQL = "SELECT generated_code FROM stockcodes_master WHERE size_matrix = '" & RG.Apos(SaveGrid.GridNumber.ToUpper) & "'"
                Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                If usingObjDB.isR(Ds) Then
                    Ds.Clear()
                    baseResponse.Success = False
                    baseResponse.Message = "This size grid has already been assigned to item codes. You must delete the items before you can Edit this size grid."
                    Return baseResponse
                End If

                tmpSQL = "UPDATE size_grids SET " &
                     "grid_description = '" & RG.Apos(SaveGrid.Description.ToUpper) & "'," &
                     "s1 = '" & RG.Apos(SaveGrid.s1.ToUpper) & "',s2 = '" & RG.Apos(SaveGrid.s2.ToUpper) & "',s3 = '" & RG.Apos(SaveGrid.s3.ToUpper) & "'," &
                     "s4 = '" & RG.Apos(SaveGrid.s4.ToUpper) & "',s5 = '" & RG.Apos(SaveGrid.s5.ToUpper) & "',s6 = '" & RG.Apos(SaveGrid.s6.ToUpper) & "'," &
                     "s7 = '" & RG.Apos(SaveGrid.s7.ToUpper) & "',s8 = '" & RG.Apos(SaveGrid.s8.ToUpper) & "',s9 = '" & RG.Apos(SaveGrid.s9.ToUpper) & "'," &
                     "s10 = '" & RG.Apos(SaveGrid.s10.ToUpper) & "',s11 = '" & RG.Apos(SaveGrid.s11.ToUpper) & "',s12 = '" & RG.Apos(SaveGrid.s12.ToUpper) & "'," &
                     "s13 = '" & RG.Apos(SaveGrid.s13.ToUpper) & "',s14 = '" & RG.Apos(SaveGrid.s14.ToUpper) & "',s15 = '" & RG.Apos(SaveGrid.s15.ToUpper) & "'," &
                     "s16 = '" & RG.Apos(SaveGrid.s16.ToUpper) & "',s17 = '" & RG.Apos(SaveGrid.s17.ToUpper) & "',s18 = '" & RG.Apos(SaveGrid.s18.ToUpper) & "'," &
                     "s19 = '" & RG.Apos(SaveGrid.s19.ToUpper) & "',s20 = '" & RG.Apos(SaveGrid.s20.ToUpper) & "',s21 = '" & RG.Apos(SaveGrid.s21.ToUpper) & "'," &
                     "s22 = '" & RG.Apos(SaveGrid.s22.ToUpper) & "',s23 = '" & RG.Apos(SaveGrid.s23.ToUpper) & "',s24 = '" & RG.Apos(SaveGrid.s24.ToUpper) & "'," &
                     "s25 = '" & RG.Apos(SaveGrid.s25.ToUpper) & "',s26 = '" & RG.Apos(SaveGrid.s26.ToUpper) & "',s27 = '" & RG.Apos(SaveGrid.s27.ToUpper) & "'," &
                     "s28 = '" & RG.Apos(SaveGrid.s28.ToUpper) & "',s29 = '" & RG.Apos(SaveGrid.s29.ToUpper) & "',s30 = '" & RG.Apos(SaveGrid.s30.ToUpper) & "'," &
                     "updated = '" & DateTime.Now.ToString("yyyy-MM-dd") & "' WHERE grid_number = '" & RG.Apos(SaveGrid.GridNumber.ToUpper) & "'"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Message = "Size matrix updated successfully."
                baseResponse.Success = True
            Else
                Ds.Clear()

                tmpSizeGrids = ""
                For gLoop As Long = 1 To 30
                    tmpSizeGrids = tmpSizeGrids & ",s" & gLoop
                Next


                tmpSQL = "INSERT INTO size_grids (grid_number,
                                             grid_description,
                                             inserted" & tmpSizeGrids & ") " &
                                             "VALUES ('" & RG.Apos(SaveGrid.GridNumber.ToUpper) & "',
                                             '" & RG.Apos(SaveGrid.Description) & "',
                                             '" & DateTime.Now.ToString("yyyy-MM-dd") & "'," &
                                            " '" & RG.Apos(SaveGrid.s1.ToUpper) & "', '" & RG.Apos(SaveGrid.s2.ToUpper) & "', '" & RG.Apos(SaveGrid.s3.ToUpper) & "'," &
                                            " '" & RG.Apos(SaveGrid.s4.ToUpper) & "', '" & RG.Apos(SaveGrid.s5.ToUpper) & "','" & RG.Apos(SaveGrid.s6.ToUpper) & "'," &
                                            "'" & RG.Apos(SaveGrid.s7.ToUpper) & "', '" & RG.Apos(SaveGrid.s8.ToUpper) & "', '" & RG.Apos(SaveGrid.s9.ToUpper) & "'," &
                                            " '" & RG.Apos(SaveGrid.s10.ToUpper) & "', '" & RG.Apos(SaveGrid.s11.ToUpper) & "', '" & RG.Apos(SaveGrid.s12.ToUpper) & "'," &
                                            "'" & RG.Apos(SaveGrid.s13.ToUpper) & "','" & RG.Apos(SaveGrid.s14.ToUpper) & "','" & RG.Apos(SaveGrid.s15.ToUpper) & "'," &
                                            "'" & RG.Apos(SaveGrid.s16.ToUpper) & "','" & RG.Apos(SaveGrid.s17.ToUpper) & "', '" & RG.Apos(SaveGrid.s18.ToUpper) & "'," &
                                            "'" & RG.Apos(SaveGrid.s19.ToUpper) & "', '" & RG.Apos(SaveGrid.s20.ToUpper) & "', '" & RG.Apos(SaveGrid.s21.ToUpper) & "'," &
                                            "'" & RG.Apos(SaveGrid.s22.ToUpper) & "', '" & RG.Apos(SaveGrid.s23.ToUpper) & "','" & RG.Apos(SaveGrid.s24.ToUpper) & "'," &
                                            " '" & RG.Apos(SaveGrid.s25.ToUpper) & "', '" & RG.Apos(SaveGrid.s26.ToUpper) & "','" & RG.Apos(SaveGrid.s27.ToUpper) & "'," &
                                            "'" & RG.Apos(SaveGrid.s28.ToUpper) & "', '" & RG.Apos(SaveGrid.s29.ToUpper) & "', '" & RG.Apos(SaveGrid.s30.ToUpper) & "')"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Message = "Size matrix added successfully."
                baseResponse.Success = True

            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return baseResponse
    End Function
#End Region
#Region "TILL"
    Public Function GetTills(ByVal BranchCode As String) As DataTable
        tmpSQL = "SELECT till_number FROM till_numbers
              WHERE branch_code = '" & RG.Apos(BranchCode.ToUpper) & "'"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function DeleteTill(ByVal BranchCode As String, ByVal TillNumber As String) As BaseResponse
        tmpSQL = "DELETE FROM till_numbers WHERE branch_code = '" & BranchCode.ToUpper & "' AND till_number = '" & TillNumber & "'"

        Try
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        baseResponse.Success = True
        baseResponse.Message = "Till Number deleted successfully"
        Return baseResponse

    End Function

    Public Function SaveTill(ByVal BranchCode As String, ByVal TillNumber As String) As BaseResponse
        tmpSQL = "SELECT till_number FROM till_numbers WHERE branch_code = '" & BranchCode.ToUpper & "' AND till_number = '" & TillNumber & "'"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If Not usingObjDB.isR(Ds) Then
                tmpSQL = "INSERT INTO till_numbers (branch_code,till_number,inserted) VALUES ('" & BranchCode.ToUpper & "','" & TillNumber & "','" & DateTime.Now.ToString("yyyy-MM-dd") & "')"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Success = True
                baseResponse.Message = "Till Added."
            Else
                baseResponse.Success = False
                baseResponse.Message = "Till already exists."
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return baseResponse
    End Function
#End Region
#Region "Specials"
    Public Function SearchSpecial(ByVal SpecialName As String) As SearchResponse
        tmpSQL = "SELECT special_name,
                         start_date,
                         end_date 
                         FROM specials_master 
                         WHERE special_name LIKE '" & RG.Apos(SpecialName.ToUpper) & "%' ORDER BY special_name ASC  "

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            searchResponse.dt = dt
            searchResponse.Success = True
            Return searchResponse
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SearchCode(ByVal SearchType As String, ByVal SearchText As String, ByVal Master As Boolean) As SearchResponse
        If Master = True Then
            If SearchType = "Generated Code" Then
                searchResponse.Message = "You cannot search by Generated Code when a Master Code search has been selected."
                searchResponse.Success = False
                Return searchResponse
            End If
            If SearchType = "Master Code" Then
                tmpSQL = "SELECT master_code as code,
                          description,
                          sku_number
                          FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE master_code LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC "

            ElseIf SearchType = "Barcode" Then
                tmpSQL = "SELECT master_code as code,
                          description,
                          sku_number 
                          FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE sku_number LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC "

            ElseIf SearchType = "Description" Then
                tmpSQL = "SELECT master_code as code,
                          description,
                          sku_number 
                          FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE description LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%') stockcodes_master ORDER BY master_code ASC"
            End If
        Else

            If SearchType = "Master Code" Then
                searchResponse.Message = "You cannot search by Master Code when a Generated Code search has been selected."
                searchResponse.Success = False
                Return searchResponse
            End If

            If SearchType = "Generated Code" Then
                tmpSQL = "SELECT generated_code as code,
                                 description,
                                 sku_number
                                 FROM stockcodes_master WHERE generated_code LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%' ORDER BY generated_code ASC "

            ElseIf SearchType = "Barcode" Then
                tmpSQL = "SELECT generated_code as code,
                                 description,
                                 sku_number
                                 FROM stockcodes_master WHERE sku_number LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%' ORDER BY sku_number ASC"

            ElseIf SearchType = "Description" Then
                tmpSQL = "SELECT generated_code as code,
                                 description,
                                 sku_number 
                                 FROM stockcodes_master WHERE description LIKE '" & RG.Apos(SearchText.ToString.ToUpper) & "%' ORDER BY description ASC "
            End If
        End If

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
            searchResponse.dt = dt
            searchResponse.Success = True
            Return searchResponse
        Catch ex As Exception
            Throw ex
        End Try


    End Function
    Public Function AddStock(ByVal Stockcode As String) As AddStockResponse
        Dim Exists As Boolean
        Dim data As String = String.Empty
        Try
            Exists = _DLayer.CheckRecordExists("stockcodes_master", "master_code", Stockcode)
            If Exists = True Then
                data = GetRecordData("stockcodes_master", "description", "master_code", Stockcode.ToUpper())
                stockResponse.Success = True
                stockResponse.data = data
            Else
                stockResponse.Success = False
                stockResponse.Message = "The code you entered does not exist. Please make sure that you enter a valid Master Code"
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return stockResponse
    End Function
    Public Function GetRecordData(ByVal SelectTableName As String, ByVal SelectFieldName As String, ByVal WhereFieldName As String, ByVal WhereFieldDataIs As String) As String

        tmpSQL = "SELECT " & SelectFieldName & " FROM " & SelectTableName & " WHERE " & WhereFieldName & " = '" & RG.Apos(WhereFieldDataIs) & "'"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                Return Ds.Tables(0).Rows(0)(SelectFieldName)
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function SaveSpecials(ByVal SpecialRequest As SaveSpecial) As BaseResponse

        Try

            tmpSQL = "DELETE FROM specials_line_items WHERE specials_link_id IN " &
                 "(SELECT special_id FROM specials_master WHERE special_name = '" & RG.Apos(SpecialRequest.SpecialName) & "')"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

            tmpSQL = "DELETE FROM specials_master WHERE special_name = '" & RG.Apos(SpecialRequest.SpecialName) & "'"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

            tmpSQL = "INSERT INTO specials_master (special_id,
                                                   start_date,
                                                   end_date,
                                                   is_active,
                                                   price,
                                                   update_date,
                                                   special_name) VALUES
                                                   ('" &
                                                   SpecialRequest.GUID & "',
                                                   '" & SpecialRequest.StartDate & "',
                                                   '" & SpecialRequest.EndDate & "',
                                                   '" & SpecialRequest.IsActive & "',
                                                   '" & SpecialRequest.Price & "',
                                                   '" & DateTime.Now.ToString("yyyy-MM-dd") & "',
                                                   '" & RG.Apos(SpecialRequest.SpecialName) & "')"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

            For Each SpecialLineItem In SpecialRequest.ListData

                tmpSQL = "INSERT INTO specials_line_items (guid,specials_link_id, master_code, qty) VALUES ('" & GenerateGUID() & "','" & SpecialLineItem.LinkGUID & "','" & SpecialLineItem.Mastercode & "','" &
                          SpecialLineItem.Quantity & "')"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

            Next

            baseResponse.Success = True
            baseResponse.Message = "Saved Successfully"

            Return baseResponse

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function GenerateGUID() As String
        GenerateGUID = Guid.NewGuid.ToString

    End Function

    Public Function Populate(ByVal SpecialName As String) As SaveSpecial
        Dim startDate As DateTime
        Dim endDate As DateTime

        tmpSQL = "SELECT m.start_date,m.end_date,m.is_active,m.price,m.update_date,m.special_name,l.master_code,l.qty, " &
                "(SELECT description FROM stockcodes_master WHERE master_code = l.master_code LIMIT 1) AS description " &
                "FROM specials_master m " &
                "INNER JOIN specials_line_items l ON m.special_id = l.specials_link_id " &
                "WHERE  special_name = '" & RG.Apos(SpecialName) & "'"
        Dim _Special As New SaveSpecial

        Try

            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                _Special.SpecialName = Ds.Tables(0).Rows(0)("special_name")
                '_Special.StartDate = Ds.Tables(0).Rows(0)("start_date")

                startDate = Ds.Tables(0).Rows(0)("start_date")
                endDate = Ds.Tables(0).Rows(0)("end_date").ToString

                _Special.StartDate = startDate.ToString("yyyy-MM-dd")
                _Special.EndDate = endDate.ToString("yyyy-MM-dd")

                '_Special.EndDate = Ds.Tables(0).Rows(0)("end_date")
                _Special.IsActive = Ds.Tables(0).Rows(0)("is_active")
                _Special.Price = Ds.Tables(0).Rows(0)("price")

                Dim _SpecialLines As New List(Of Manage.SpecialLineItems)

                For Each dr As DataRow In Ds.Tables(0).Rows
                    Dim _SpecialLine As New Manage.SpecialLineItems
                    _SpecialLine.Mastercode = dr("master_code") & ""
                    _SpecialLine.Description = dr("description") & ""
                    _SpecialLine.Quantity = dr("qty") & ""
                    _SpecialLines.Add(_SpecialLine)
                Next

                _Special.ListData = _SpecialLines
                _Special.Success = True
                Return _Special
            End If
            _Special.Success = False
            _Special.Message = "No data found"
            Return _Special
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "Regions"
    Public Function SaveRegion(ByVal Region As String) As BaseResponse
        Try


            tmpSQL = "INSERT INTO branch_region (region
                                                   ) VALUES
                                                   ('" &
                                                   Region.ToUpper & "'
                                                   )"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            baseResponse.Success = True
            baseResponse.Message = "Saved Successfully"
            Return baseResponse

        Catch ex As Exception
            If ex.Message.ToString.Contains("duplicate key value violates unique constraint ") Then
                baseResponse.Success = False
                baseResponse.Message = "This region already exists"
                Return baseResponse
            Else
                Throw ex
            End If
        End Try
    End Function
    Public Function DeleteRegion(ByVal Region As String) As BaseResponse
        Try


            tmpSQL = "select branch_code from branch_details where region = '" & RG.Apos(Region.ToUpper) & "'"
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                baseResponse.Success = False
                baseResponse.Message = "You cannot delete this region"
            Else
                tmpSQL = "DELETE FROM branch_region WHERE region = '" & RG.Apos(Region.ToUpper) & "'"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Success = True
                baseResponse.Message = "Region deleted successfully"
            End If


        Catch ex As Exception
            Throw ex
        End Try

        Return baseResponse
    End Function
    Public Function GetRegions() As DataTable

        tmpSQL = "SELECT region FROM branch_region  ORDER BY region desc "

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function GetProvinces() As DataTable
        tmpSQL = "SELECT province FROM provinces  ORDER BY province desc"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

#End Region
#Region "Suppliers"
    Public Function SaveUpdateSuppliers(ByVal supplier As SaveUpdateSupplier) As BaseResponse
        Try

            tmpSQL = "SELECT supplier_code FROM supplier_details WHERE supplier_code = '" & supplier.SupplierCode.ToUpper & "'"
            Ds = usingObjDB.GetDataSet(_POSWriteConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                'Update 
                tmpSQL = "UPDATE supplier_details SET " &
                    "supplier_name = '" & RG.Apos(supplier.SupplierName.ToUpper) & "'," &
                    "address_line_1 = '" & RG.Apos(supplier.AddressLine1) & "'," &
                    "address_line_2 = '" & RG.Apos(supplier.AddressLine2) & "'," &
                    "address_line_3 = '" & RG.Apos(supplier.AddressLine3) & "'," &
                    "address_line_4 = '" & RG.Apos(supplier.AddressLine4) & "'," &
                    "address_line_5 = '" & RG.Apos(supplier.AddressLine5) & "'," &
                    "telephone_number = '" & RG.Apos(supplier.Telephone) & "'," &
                    "fax_number = '" & RG.Apos(supplier.FAX) & "'," &
                    "email_address = '" & RG.Apos(supplier.Email) & "'," &
                    "tax_number = '" & RG.Apos(supplier.TAX) & "'," &
                    "is_blocked = '" & supplier.IsBlocked & "'" &
                    " WHERE supplier_code = '" & RG.Apos(supplier.SupplierCode.ToUpper) & "'"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Success = True
                baseResponse.Message = "Updated Successfully"
            Else
                'Insert 
                tmpSQL = "INSERT INTO supplier_details (supplier_code,supplier_name,address_line_1,address_line_2,address_line_3,address_line_4," &
                         "address_line_5,telephone_number,fax_number,email_address,tax_number,is_blocked) VALUES " &
                        "('" & RG.Apos(supplier.SupplierCode.ToUpper) & "','" & RG.Apos(supplier.SupplierName.ToUpper) & "','" & RG.Apos(supplier.AddressLine1) & "','" & RG.Apos(supplier.AddressLine2) & "'," &
                        "'" & RG.Apos(supplier.AddressLine3) & "','" & RG.Apos(supplier.AddressLine4) & "','" & RG.Apos(supplier.AddressLine5) & "','" & RG.Apos(supplier.Telephone) & "'," &
                        "'" & RG.Apos(supplier.FAX) & "','" & RG.Apos(supplier.Email) & "','" & RG.Apos(supplier.TAX) & "','" & supplier.IsBlocked & "')"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Success = True
                baseResponse.Message = "Saved Successfully"
            End If

        Catch ex As Exception
            baseResponse.Success = False
            baseResponse.Message = "Something went wrong"
            Throw ex
            Return baseResponse
        End Try

        Return baseResponse
    End Function
    Public Function DeleteSupplier(ByVal SupplierCode As String) As BaseResponse
        Try

            tmpSQL = "DELETE FROM supplier_details WHERE supplier_code = '" & RG.Apos(SupplierCode.ToUpper) & "'"
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            baseResponse.Success = True
            baseResponse.Message = "Supplier " & SupplierCode & " deleted successfully."
        Catch ex As Exception
            Throw ex
        End Try
        Return baseResponse
    End Function
    Public Function SearchSupplier(ByVal SearchType As String, ByVal SearchText As String) As DataTable

        If SearchType = "Account" Then
            tmpSQL = "SELECT supplier_code,supplier_name FROM supplier_details WHERE supplier_code LIKE '" & RG.Apos(SearchText.ToUpper) & "%' ORDER BY supplier_code ASC "
        ElseIf SearchType = "Company Name" Then
            tmpSQL = "SELECT supplier_code,supplier_name FROM supplier_details WHERE supplier_name LIKE '" & RG.Apos(SearchText.ToUpper) & "%' ORDER BY supplier_name ASC "
        End If

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function GetSupplierDetails(ByVal SupplierCode As String) As DataTable

        tmpSQL = "SELECT * FROM supplier_details WHERE supplier_code = '" & RG.Apos(SupplierCode.ToUpper) & "'"
        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
#End Region
#Region "PointsMaintenance"
    Public Function GetCompanySettings() As DataTable
        tmpSQL = "SELECT * FROM company_details"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
    Public Function SaveCompanySettings(ByVal Dollars As String, ByVal Points As String) As BaseResponse
        Try

            tmpSQL = "UPDATE company_details
                                SET cash_to_points = '" & Val(Dollars) & "',
                                points_to_cash = '" & Val(Points) & "',
                                updated = '" & Format(Now, "yyyy-MM-dd") & "'"

            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            baseResponse.Success = True
            baseResponse.Message = "Settings Updated"
            Return baseResponse

        Catch ex As Exception
            Throw ex
        End Try
        Return baseResponse
    End Function
#End Region
#Region "CategoryMaintenance"
    Public Function GetCategories(ByVal CategoryType As String, ByVal CategoryNumber As Integer) As DataTable
        tmpSQL = "SELECT * FROM categories WHERE category_type = '" & CategoryType & "' AND category_number = '" & CategoryNumber & "' ORDER BY category_code ASC"

        Try
            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function SaveCategory(ByVal categories As Categories) As BaseResponse
        Try

            'Check if category already exists
            tmpSQL = "SELECT * FROM categories WHERE category_type = '" & categories.CategoryType & "' AND category_number = '" & Mid$(categories.CategoryNumber, 10) & "' AND category_code = '" & RG.Apos(categories.CategoryCode.ToUpper) & "'"
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                'Category exists
                Ds.Clear()

                tmpSQL = "UPDATE categories SET category_description = '" & RG.Apos(categories.Description.ToUpper) & "',updated = '" & Format(Now, "yyyy-MM-dd") & "' WHERE category_type = '" & categories.CategoryType & "' AND category_number = '" & Mid$(categories.CategoryNumber, 10) & "' AND category_code = '" & RG.Apos(categories.CategoryCode.ToUpper) & "'"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Message = "Category " & categories.CategoryCode & " Updated"

            Else
                Ds.Clear()

                tmpSQL = "INSERT INTO categories (category_type,category_number,category_code,category_description,inserted) " &
                          "VALUES ('" & categories.CategoryType & "','" & Mid$(categories.CategoryNumber, 10) & "','" & RG.Apos(categories.CategoryCode.ToUpper) & "','" & RG.Apos(categories.Description.ToUpper) & "','" & Format(Now, "yyyy-MM-dd") & "')"
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                baseResponse.Message = "Category " & categories.CategoryCode & " Inserted"

            End If
        Catch ex As Exception
            Throw ex
        End Try
        baseResponse.Success = True
        Return baseResponse
    End Function
    Public Function DeleteCategory(ByVal categories As Categories) As BaseResponse
        Try
            tmpSQL = "DELETE FROM categories WHERE category_type = '" & categories.CategoryType & "' AND category_number = '" & categories.CategoryNumber & "' AND category_code = '" & RG.Apos(categories.CategoryCode.ToUpper) & "'"
            usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)

            tmpSQL = "UPDATE stockcodes_categories SET category_1 = '' WHERE category_1 = '" & categories.CategoryNumber.ToString.ToUpper & "'"
            usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)

            baseResponse.Success = True
            baseResponse.Message = "Category " & categories.CategoryCode & " deleted"
        Catch ex As Exception
            Throw ex
        End Try
        Return baseResponse
    End Function
#End Region
#Region "Municipality"
    Public Function SaveMunicipality(ByVal Municipality As String, ByVal BranchCode As String) As BaseResponse
        Try
            tmpSQL = "update branch_details set municipality='" & Municipality & "' where branch_code ='" & Mid(BranchCode, 1, 3) & "' "
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            baseResponse.Success = True
            baseResponse.Message = "Saved Successfully"
            Return baseResponse

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function UpdateMunicipality(ByVal BranchCode As String) As BaseResponse
        Try
            tmpSQL = "update branch_details set municipality=null where branch_code ='" & BranchCode & "' "
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            baseResponse.Success = True
            baseResponse.Message = "Updated Successfully"
        Catch ex As Exception
            Throw ex
        End Try

        Return baseResponse
    End Function
#End Region
#Region "Import"
    Public Function RunBranchImport(ByVal Filename As String, ByVal Email As String, ByVal ImportType As String, ByVal Username As String) As String
        Dim TaskType As String = String.Empty

        If ImportType = "Branches" Then
            TaskType = "import_branches"
        Else
            TaskType = "stock_quantity_import"
        End If

        tmpSQL = "INSERT INTO tasks (task_type,file_to_run,timestamp_created,email_addresses,username) VALUES " &
                 "('" & TaskType & "','" & Filename & "','" & Format(Now, "yyyy-MM-dd HH:mm") & "','" & Email & "','" & Username & "')"
        Try
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        Finally
        End Try

        Return "Success"
    End Function
#End Region
End Class
