Imports Npgsql
Imports Microsoft.VisualBasic
Imports System.Data
Imports Entities

Public Class SelfAssistDL
    Inherits DataAccessLayerBase

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing

    Public Sub New()
        'If Not HttpContext.Current.IsDebuggingEnabled Then
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = Me.DataBase("PostgreConnectionStringPositiveRead")
        'Else
        'objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWriteTesting", "pos_demo")
        'objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting", "pos_demo")
        'connection = Me.DataBase("PostgreConnectionStringPositiveReadTesting", "pos_demo")
        'End If
    End Sub

    Public Function RunStatement(ByVal AccountNumber As String) As ShopMiniStatement

        Dim _Statement As New ShopMiniStatement
        Dim _LineItems As New List(Of StatementLineItems)

        Return _Statement

    End Function

    Public Function GetLatestStyles(ByVal BranchCode As String) As StyleNumbers

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim _Return As New StyleNumbers
        Dim styles As New List(Of StyleNumber)

        Dim ThenDate As Date = Format(Now, "yyyy-MM-dd")
        ThenDate = ThenDate.AddDays(-150)

        Dim strDate As String = Format(ThenDate, "yyyy-MM-dd")

        tmpSQL = "SELECT " & _
                 "	sm.master_code, " & _
                 "	MAX (sm.description) description, " & _
                 "	MAX (selling_price_1) selling_price_1, " & _
                 "	MAX (tax_value) tax_value, " & _
                 "  MAX(bsi.image_location) as image_location, " & _
                 "  MAX(categories.category_description) AS category_description, " & _
                 "  MAX(sm.colour_matrix) AS colour_matrix " & _
                 "        FROM " & _
                 "        transaction_master " & _
                 "  INNER JOIN transaction_line_items ON transaction_line_items.link_guid = transaction_master.guid " & _
                 "  INNER JOIN stockcodes_master sm ON transaction_line_items.generated_code = sm.generated_code " & _
                 "  INNER JOIN stockcodes_prices sp ON sm.generated_code = sp.generated_code " & _
                 "  INNER JOIN tax_groups tg ON sm.purchase_tax_group = tg.tax_group " & _
                 "  INNER JOIN buying_stockcodes_images bsi ON sm.master_code = substr(item_code,1,(CHAR_LENGTH(item_code) - 2)) " & _
                 "  INNER JOIN stockcodes_categories ON stockcodes_categories.generated_code = sm.generated_code " & _
                 "  INNER JOIN categories ON stockcodes_categories.category_2 = categories.category_code AND categories.category_number = 2 " & _
                 "        WHERE " & _
                 "	transaction_type = 'IBTIN' " & _
                 "AND branch_code = '" & BranchCode & "' " & _
                 "AND sale_date BETWEEN '" & strDate & "' " & _
                 "AND '" & Format(Now, "yyyy-MM-dd") & "' " & _
                 "GROUP BY " & _
                 "	sm.master_code " & _
                 "ORDER BY " & _
                 "	MAX (sale_date) DESC " & _
                 "LIMIT 50 "
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim curString As New StyleNumber
                    curString.Style = dr("master_code") & ""
                    curString.Description = dr("description") & ""
                    curString.Price = RG.Numb(Val(dr("selling_price_1") & "") * (1 + (Val(dr("tax_value") & "") / 100)))
                    curString.ImageLocation = dr("image_location") & ""
                    curString.CategoryDescription = dr("category_description") & ""
                    curString.ColourMatrix = dr("colour_matrix") & ""
                    styles.Add(curString)
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        _Return.StyleNumberList = styles

        Dim colours As New List(Of ColourCodes)

        tmpSQL = "SELECT colour_code,colour_description FROM colour_grids "
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim curString As New ColourCodes
                    curString.ColourCode = dr("colour_code") & ""
                    curString.ColourDescription = dr("colour_description") & ""
                    colours.Add(curString)
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        _Return.ColourMatrix = colours

        Return _Return

    End Function

    Public Function GetAccountDetails(ByVal AccountDetails As Debtor) As Debtor

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        Dim _NewDebtor As New Debtor

        If AccountDetails.CardNumber <> "" Then
            'Get account number from card number
            tmpSQL = "SELECT " & _
                     "debtor_personal.account_number," & _
                     "debtor_personal.first_name, " & _
                     "debtor_personal.last_name, " & _
                     "card_number " & _
                     "FROM " & _
                     "debtor_personal " & _
                     "INNER JOIN card_details ON debtor_personal.account_number = card_details.account_number " & _
                     "WHERE card_number = '" & RG.Apos(AccountDetails.CardNumber) & "'"
        ElseIf AccountDetails.AccountNumber <> "" Then
            tmpSQL = "SELECT " & _
                     "debtor_personal.account_number," & _
                     "debtor_personal.first_name, " & _
                     "debtor_personal.last_name, " & _
                     "card_number " & _
                     "FROM " & _
                     "debtor_personal " & _
                     "INNER JOIN card_details ON debtor_personal.account_number = card_details.account_number " & _
                     "WHERE debtor_personal.account_number = '" & RG.Apos(AccountDetails.AccountNumber) & "'"
        Else
            tmpSQL = "SELECT " & _
                     "debtor_personal.account_number," & _
                     "debtor_personal.first_name, " & _
                     "debtor_personal.last_name, " & _
                     "card_number " & _
                     "FROM " & _
                     "debtor_personal " & _
                     "INNER JOIN card_details ON debtor_personal.account_number = card_details.account_number " & _
                     "WHERE id_number = '" & Trim(RG.Apos(AccountDetails.IDNumber)) & "' " & _
                     "AND TRIM(both ' '  from UPPER(last_name)) = '" & Trim(RG.Apos(AccountDetails.LastName.ToUpper)) & "'"
        End If

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _NewDebtor.AccountNumber = dr("account_number") & ""
                    _NewDebtor.FirstName = dr("first_name") & ""
                    _NewDebtor.LastName = dr("last_name") & ""
                    _NewDebtor.CardNumber = dr("card_number") & ""

                    'Record login
                    Dim _guid As String
                    _guid = Guid.NewGuid.ToString
                    _NewDebtor.GuID = _guid

                    tmpSQL = "INSERT INTO self_assist_master (account_number,action,guid,branch_code) VALUES ('" & _NewDebtor.AccountNumber & "','Login','" & _guid & "','" & AccountDetails.BranchCode & "')"
                    Try
                        objDBWrite.ExecuteQuery(tmpSQL)
                    Catch ex As Exception
                        If (objDBWrite IsNot Nothing) Then
                            objDBWrite.CloseConnection()
                        End If
                        Return Nothing
                    Finally
                        If (objDBWrite IsNot Nothing) Then
                            objDBWrite.CloseConnection()
                        End If
                    End Try
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        Return _NewDebtor

    End Function

    Public Function GetStockOnHand(ByVal BranchCode As String, ByVal Stockcode As String) As DataSet

        Dim xData As New DataSet

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            tmpSQL = "SELECT " & _
                     "CASE WHEN stock_on_hand.qty_on_hand > 0 THEN 'YES' ELSE 'NO' END AS in_stock, " & _
                     "colour_grids.colour_description, stockcodes_master.item_size " & _
                     "FROM stock_on_hand " & _
                     "INNER JOIN stockcodes_master ON stockcodes_master.generated_code = stock_on_hand.generated_code " & _
                     "INNER JOIN colour_grids ON stockcodes_master.item_colour = colour_grids.colour_code " & _
                     "WHERE stock_on_hand.branch_code = '" & RG.Apos(BranchCode) & "' AND stockcodes_master.master_code IN " & _
                     "(SELECT master_code FROM stockcodes_master WHERE stockcodes_master.generated_code = '" & RG.Apos(Stockcode) & "' " & _
                     "OR stockcodes_master.master_code = '" & RG.Apos(Stockcode) & "') " & _
                     "ORDER BY colour_code,item_size"

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function UploadImage(ByVal ImageName As String, ByVal ImageDescription As String, ByVal FileLocation As String) As String

        tmpSQL = "INSERT INTO self_assist_images " & _
           "(image_name,image_description,is_active,date_of_insert,file_location) " & _
           "VALUES ('" & Mid$(RG.Apos(ImageName), 1, 100) & "','" & Mid$(RG.Apos(ImageDescription), 1, 50) & "','True',now(),'" & Mid$(RG.Apos(FileLocation), 1, 250) & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ex.Message
        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        End Try

        Return "Success"

    End Function

    Public Function UpdateImages(ByVal lstImages As entScreensaverImages) As String

        For i As Integer = 0 To lstImages.lstImages.Count - 1

            tmpSQL = "SELECT is_active FROM self_assist_images " & _
                     "WHERE image_name = '" & RG.Apos(lstImages.lstImages(i).image_name) & "'"
            Try
                ds = objDBRead.GetDataSet(tmpSQL)
                If objDBRead.isR(ds) Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        If dr("is_active").ToString.ToLower <> lstImages.lstImages(i).is_active.ToLower Then
                            'Need to update
                            tmpSQL = "UPDATE self_assist_images " & _
                                     "SET is_active = '" & lstImages.lstImages(i).is_active & "',updated = '" & Format(Now, "yyyy-MM-dd") & "' " & _
                                     "WHERE image_name = '" & RG.Apos(lstImages.lstImages(i).image_name) & "'"
                            Try
                                objDBWrite.ExecuteQuery(tmpSQL)
                            Catch ex As Exception
                                objDBWrite.CloseConnection()
                                Return ex.Message
                            Finally
                                'objDBWrite.CloseConnection()
                            End Try
                        End If
                    Next
                End If
            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return Nothing
            Finally
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
            End Try

        Next

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return "Success"

    End Function

    Private Function ReturnImageAsByte(ByVal FileLocation As String) As Byte()

        If System.IO.File.Exists(FileLocation) Then
            Return System.IO.File.ReadAllBytes(FileLocation)
        Else
            Return New Byte() {0}
        End If


    End Function

    Public Function GetCustomerDetails(ByVal AccountDetails As CashCardCustomer) As CashCardCustomer

        Dim PointsToCash As Double = 0

        Dim _CashCardCustomer As New CashCardCustomer

        Dim _Log As New WriteToLog

        'Get details
        tmpSQL = "SELECT cp.account_number,cp.first_name,cp.last_name,cp.customer_status,cp.is_female,cp.send_promos,cb.total_spent, " & _
                 "cb.current_points_balance,cb.total_points_accrued,cd.card_number,ccd.email_address,ccd.contact_number,ccd.state " & _
                 "FROM " & _
                 "customer_personal cp " & _
                 "LEFT OUTER JOIN customer_balances cb ON cp.account_number = cb.account_number " & _
                 "LEFT OUTER JOIN cash_card_details cd ON cp.account_number = cd.account_number " & _
                 "LEFT OUTER JOIN customer_contact_details ccd ON cp.account_number = ccd.account_number "

        If AccountDetails.IDNumber = "" Then
            'We got a card number
            tmpSQL &= "WHERE cd.card_number = '" & AccountDetails.CardNumber & "'"
        Else
            'We got an ID Number
            tmpSQL &= "WHERE cp.account_number = '" & AccountDetails.IDNumber & "'"
        End If

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _CashCardCustomer.FirstName = dr("first_name") & ""
                    _CashCardCustomer.LastName = dr("last_name") & ""
                    _CashCardCustomer.EMail = dr("email_address") & ""
                    _CashCardCustomer.ContactNumber = dr("contact_number") & ""
                    _CashCardCustomer.Status = dr("customer_status") & ""
                    _CashCardCustomer.Province = dr("state") & ""
                    '_CashCardCustomer.Sex = dr("is_female") & ""
                    _CashCardCustomer.isPromo = dr("send_promos") & ""

                    _CashCardCustomer.TotalSpent = dr("total_spent") & ""
                    _CashCardCustomer.CurrentPointsBalance = dr("current_points_balance") & ""
                    _CashCardCustomer.TotalPointsAccrued = dr("total_points_accrued") & ""

                    _CashCardCustomer.CardNumber = dr("card_number") & ""
                    _CashCardCustomer.IDNumber = dr("account_number") & ""

                Next
            Else
                _CashCardCustomer.ReturnMessage = "Account Not Found"
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return _CashCardCustomer
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        Return _CashCardCustomer

    End Function

    Public Function GetImages() As entScreensaverImages

        'Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim SixMonthsAgo As String
        SixMonthsAgo = Format(Now.Date.AddMonths(-6), "yyyy-MM-dd")

        Dim _return As New entScreensaverImages

        tmpSQL = "SELECT * FROM self_assist_images " & _
                 "WHERE (is_active = True OR updated >= '" & SixMonthsAgo & "')"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _si As New ssImages
                    _si.image_name = dr("image_name") & ""
                    _si.is_active = dr("is_active") & ""

                    If dr("is_active") & "" = "True" Then
                        _si.image_byte = ReturnImageAsByte(dr("file_location") & "")
                    End If

                    _return.lstImages.Add(_si)

                Next
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

        Return _return

    End Function

    Public Function GetImageList() As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT running_id as ID,image_name,image_description,is_active,file_location FROM self_assist_images ORDER BY date_of_insert DESC"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function


End Class
