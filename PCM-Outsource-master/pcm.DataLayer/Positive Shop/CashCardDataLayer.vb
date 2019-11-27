Imports Entities

Public Class CashCardDataLayer
    Inherits DataAccessLayerBase
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Public Function ValidID(ByVal IdNum As String) As Boolean

        Dim rOdds As String
        Dim rEvens As String
        Dim lsEvens As String
        Dim lsOdds As String
        Dim Toae As String
        Dim vDiffs As String

        If Len(IdNum) <> 13 Then
            ValidID = False
            Exit Function
        End If

        rOdds = Mid$(IdNum, 1, 1) & Mid$(IdNum, 3, 1) & Mid$(IdNum, 5, 1) & Mid$(IdNum, 7, 1) & Mid$(IdNum, 9, 1) & Mid$(IdNum, 11, 1)
        rEvens = Mid$(IdNum, 2, 1) & Mid$(IdNum, 4, 1) & Mid$(IdNum, 6, 1) & Mid$(IdNum, 8, 1) & Mid$(IdNum, 10, 1) & Mid$(IdNum, 12, 1)

        rEvens = Val(rEvens) * 2
        'done with rough odds and evens

        lsEvens = "0"
        lsOdds = "0"


        For mLoop = 1 To Len(rOdds)
            lsOdds = Val(lsOdds) + Val(Mid$(rOdds, mLoop, 1))
        Next mLoop

        For mLoop = 1 To Len(rEvens)
            lsEvens = Val(lsEvens) + Val(Mid$(rEvens, mLoop, 1))
        Next mLoop

        Toae = Val(lsEvens) + Val(lsOdds) + Mid$(IdNum, 13, 1)

        vDiffs = Val(Toae) / 10

        If Len(vDiffs) > 2 Then
            ValidID = False
        Else
            ValidID = True
        End If


    End Function

    Private Sub InsertIntoCustomerLog(ByVal Database As String, ByVal BranchCode As String, ByVal Username As String,
                                      ByVal AccountNumber As String, ByVal FieldName As String,
                                      ByVal OldValue As String, ByVal NewValue As String)

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")

        tmpSQL = "INSERT INTO customer_change_log (guid,account_number,timestamp_of_change,username,branch_code,field_name,old_value,new_value) VALUES " &
                 "('" & Guid.NewGuid.ToString & "','" & AccountNumber & "',localtimestamp,'" & Username & "','" & BranchCode & "','" & RG.Apos(FieldName) & "'," &
                 "'" & RG.Apos(OldValue) & "','" & RG.Apos(NewValue) & "')"

        Try
            'objDBWrite.ExecuteQuery(tmpSQL, BranchCode)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, BranchCode)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
        End Try

        'objDBWrite.CloseConnection()

    End Sub

    Public Function AssignCard(ByVal AccountDetails As CashCardCustomer) As String

        Dim _Response As String

        Dim _Log As New WriteToLog

        'Check if password is valid
        If AccountDetails.Password <> "JaiRL10nFMNo$forany" Then

            _Log.WriteLog(AccountDetails.BranchCode,
                     "GetCustomerDetails: Wrong Web Service Password", AccountDetails.IPAddress)
            _Response = "General Error"
            Return _Response
        End If

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")

        tmpSQL = "SELECT * FROM cash_card_details WHERE card_number = '" & RG.Apos(AccountDetails.CardNumber) & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, AccountDetails.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, AccountDetails.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If dr("account_number") & "" <> "" Then
                        If dr("account_number") & "" <> AccountDetails.IDNumber Then
                            'Card is already being used for another account
                            _Response = "This Card is already assigned to another account." & vbCrLf & "You may not use it for this account!"
                            'If (objDBRead IsNot Nothing) Then
                            '    objDBRead.CloseConnection()
                            'End If
                            Return _Response
                        Else
                            'Already assigned to the current account
                            _Response = "Card already assigned to this account."
                            'If (objDBRead IsNot Nothing) Then
                            '    objDBRead.CloseConnection()
                            'End If
                            Return _Response
                        End If
                    Else
                        If dr("is_active") = False Then
                            'Card has been marked as blocked
                            _Response = "This card is not active." & vbCrLf & "You may not use it for this account!"
                            'If (objDBRead IsNot Nothing) Then
                            '    objDBRead.CloseConnection()
                            'End If
                            Return _Response
                        End If

                        'Success!
                        'Update the card details table
                        tmpSQL = "UPDATE cash_card_details SET account_number = '" & AccountDetails.IDNumber & "',timestamp_card_assigned = localtimestamp," &
                                 "card_assigned_by_user = '" & AccountDetails.Username & "',card_assigned_at_store = '" & AccountDetails.BranchCode & "' " &
                                 "WHERE card_number = '" & AccountDetails.CardNumber & "'"
                        Try
                            'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                        Catch ex As Exception
                            'objDBWrite.CloseConnection()
                            _Response = ex.Message
                            Return _Response
                        End Try

                        'Update the customer change log
                        tmpSQL = "INSERT INTO customer_change_log (guid,account_number,timestamp_of_change,username,branch_code,field_name,old_value,new_value) VALUES " &
                                 "('" & Guid.NewGuid.ToString & "','" & AccountDetails.IDNumber & "',localtimestamp,'" & AccountDetails.Username & "','" & AccountDetails.BranchCode & "'," &
                                 "'Card Assigned','','" & AccountDetails.CardNumber & "')"
                        Try
                            'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                        Catch ex As Exception
                            'objDBWrite.CloseConnection()
                            _Response = ex.Message
                            Return _Response
                        End Try

                        'If the account had a card assigned to it previously, we now need to make the old card inactive
                        If AccountDetails.OldCardNumber <> "" Then
                            If AccountDetails.OldCardNumber <> AccountDetails.CardNumber Then
                                tmpSQL = "UPDATE cash_card_details SET is_active = False WHERE card_number = '" & AccountDetails.OldCardNumber & "'"
                                Try
                                    'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                                Catch ex As Exception
                                    'objDBWrite.CloseConnection()
                                    _Response = ex.Message
                                    Return _Response
                                End Try
                            End If
                        End If



                    End If
                Next
            Else
                _Response = "This Card Number does not exist!"
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return _Response
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        End Try

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

        'If (objDBWrite IsNot Nothing) Then
        '    objDBWrite.CloseConnection()
        'End If

        _Response = "The Card has been assigned to the account."

        Return _Response

    End Function

    Public Function GetCustomerList(ByVal AccountDetails As CashCardCustomer,
                                    ByVal isSearchByID As Boolean, ByVal SearchCriteria As String) As List(Of CashCardCustomer)
        Dim _Log As New WriteToLog

        Dim _CustomerList As New List(Of CashCardCustomer)

        'Check if password is valid
        If AccountDetails.Password <> "JaiRL10nFMNo$forany" Then

            _Log.WriteLog(AccountDetails.BranchCode,
                     "GetCustomerDetails: Wrong Web Service Password", AccountDetails.IPAddress)
            Return Nothing
        End If

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        'Get details
        If isSearchByID = True Then
            tmpSQL = "SELECT p.account_number,account_name,first_name,last_name,contact_number " &
                     "FROM customer_personal p " &
                     "LEFT OUTER JOIN customer_contact_details d ON p.account_number = d.account_number " &
                     "WHERE p.account_number LIKE '" & RG.Apos(SearchCriteria.ToUpper) & "%' ORDER BY p.account_number ASC LIMIT 50"
        Else
            tmpSQL = "SELECT p.account_number,account_name,first_name,last_name,contact_number " &
                     "FROM customer_personal p " &
                     "LEFT OUTER JOIN customer_contact_details d ON p.account_number = d.account_number " &
                     "WHERE p.last_name LIKE '" & RG.Apos(SearchCriteria.ToUpper) & "%' ORDER BY p.last_name ASC LIMIT 50"
        End If

        Try
            'ds = objDBRead.GetDataSet(tmpSQL, AccountDetails.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, AccountDetails.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _NewCustomer As New CashCardCustomer
                    _NewCustomer.IDNumber = dr("account_number") & ""
                    _NewCustomer.FullName = dr("first_name") & " " & dr("last_name")
                    _NewCustomer.ContactNumber = dr("contact_number") & ""

                    _CustomerList.Add(_NewCustomer)
                Next
            Else

            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        End Try

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

        Return _CustomerList

    End Function


    Public Function UpdateCustomerTable(ByVal AccountDetails As CashCardCustomer) As String

        Dim _CashCardCustomer As String

        Dim _Log As New WriteToLog

        'Check if password is valid
        If AccountDetails.Password <> "JaiRL10nFMNo$forany" Then

            _Log.WriteLog(AccountDetails.BranchCode,
                     "GetCustomerDetails: Wrong Web Service Password", AccountDetails.IPAddress)
            _CashCardCustomer = "General Error"
            Return _CashCardCustomer
        End If

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim PointsToCash As Double = 0

        Dim isSA As Boolean = True

        If ValidID(AccountDetails.IDNumber) = False Then
            isSA = False
        End If


        'If Val(Mid$(AccountDetails.IDNumber, 7, 1)) >= 5 Then
        '    '5 or bigger is Male
        '    AccountDetails.Sex = False
        'Else
        '    AccountDetails.Sex = True
        'End If


        'Get details
        tmpSQL = "SELECT cp.first_name,cp.last_name,cp.customer_status,cp.is_female,cp.send_promos, " &
                 "ccd.email_address,ccd.contact_number,ccd.state " &
                 "FROM " &
                 "customer_personal cp " &
                 "LEFT OUTER JOIN customer_contact_details ccd ON cp.account_number = ccd.account_number " &
                 "WHERE cp.account_number = '" & AccountDetails.IDNumber & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, AccountDetails.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, AccountDetails.BranchCode)
            If usingObjDB.isR(ds) Then
                'Account already exists
                For Each dr As DataRow In ds.Tables(0).Rows

                    tmpSQL = "UPDATE customer_personal SET first_name = '" & RG.Apos(AccountDetails.FirstName) & "'," &
                    "last_name = '" & RG.Apos(AccountDetails.LastName) & "',customer_status = '" & RG.Apos(AccountDetails.Status) & "'," &
                    "day_of_birth = '" & AccountDetails.DayOfBirth & "',month_of_birth = '" & AccountDetails.MonthOfBirth & "', " &
                    "year_of_birth = '" & AccountDetails.YearOfBirth & "', " &
                    "updated = '" & Format(Now, "yyyy-MM-dd") & "',is_female = '" & AccountDetails.Sex & "',send_promos = '" & AccountDetails.isPromo & "' " &
                    "WHERE account_number = '" & AccountDetails.IDNumber & "'"
                    Try
                        'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                        usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                    Catch ex As Exception
                        'objDBWrite.CloseConnection()
                        _CashCardCustomer = ex.Message
                        Return _CashCardCustomer
                    End Try


                    tmpSQL = "UPDATE customer_contact_details SET email_address = '" & RG.Apos(AccountDetails.EMail) & "', contact_number = '" & RG.Apos(AccountDetails.ContactNumber) & "', " &
                                 "state = '" & RG.Apos(AccountDetails.Province) & "' WHERE account_number = '" & AccountDetails.IDNumber & "'"
                    Try
                        'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                        usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                    Catch ex As Exception
                        'objDBWrite.CloseConnection()
                        _CashCardCustomer = ex.Message
                        Return _CashCardCustomer
                    End Try

                    If dr("first_name") & "" <> AccountDetails.FirstName Then
                        InsertIntoCustomerLog("pos_010", AccountDetails.BranchCode, AccountDetails.Username, AccountDetails.IDNumber,
                                               "first_name", dr("first_name") & "", AccountDetails.FirstName)
                    End If

                    If dr("last_name") & "" <> AccountDetails.LastName Then
                        InsertIntoCustomerLog("pos_010", AccountDetails.BranchCode, AccountDetails.Username, AccountDetails.IDNumber,
                                               "last_name", dr("last_name") & "", AccountDetails.LastName)
                    End If

                    If dr("email_address") & "" <> AccountDetails.EMail Then
                        InsertIntoCustomerLog("pos_010", AccountDetails.BranchCode, AccountDetails.Username, AccountDetails.IDNumber,
                                               "email_address", dr("email_address") & "", AccountDetails.EMail)
                    End If

                    If dr("contact_number") & "" <> AccountDetails.ContactNumber Then
                        InsertIntoCustomerLog("pos_010", AccountDetails.BranchCode, AccountDetails.Username, AccountDetails.IDNumber,
                                               "contact_number", dr("contact_number") & "", AccountDetails.ContactNumber)
                    End If

                    If dr("customer_status") & "" <> AccountDetails.Status Then
                        InsertIntoCustomerLog("pos_010", AccountDetails.BranchCode, AccountDetails.Username, AccountDetails.IDNumber,
                                               "customer_status", dr("customer_status") & "", AccountDetails.Status)
                    End If

                    If dr("state") & "" <> AccountDetails.Province Then
                        InsertIntoCustomerLog("pos_010", AccountDetails.BranchCode, AccountDetails.Username, AccountDetails.IDNumber,
                                               "state", dr("state") & "", AccountDetails.Province)
                    End If

                    If dr("send_promos") & "" <> AccountDetails.isPromo Then
                        InsertIntoCustomerLog("pos_010", AccountDetails.BranchCode, AccountDetails.Username, AccountDetails.IDNumber,
                                               "send_promos", dr("send_promos") & "", AccountDetails.isPromo)
                    End If

                Next
            Else
                'New account
                tmpSQL = "INSERT INTO customer_personal (account_number,first_name,last_name,customer_status,day_of_birth," &
                     "month_of_birth,year_of_birth,branch_opened,card_number,inserted,is_female,send_promos) VALUES " &
                     "('" & AccountDetails.IDNumber & "','" & RG.Apos(AccountDetails.FirstName) & "','" & RG.Apos(AccountDetails.LastName) & "'," &
                     "'" & RG.Apos(AccountDetails.Status) & "','" & AccountDetails.DayOfBirth & "','" & AccountDetails.MonthOfBirth & "'," &
                     "'" & AccountDetails.YearOfBirth & "','" & AccountDetails.BranchCode & "',''," &
                     "'" & Format(Now, "yyyy-MM-dd") & "','" & AccountDetails.Sex & "','" & AccountDetails.isPromo & "')"
                Try
                    'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                Catch ex As Exception
                    'objDBWrite.CloseConnection()
                    _CashCardCustomer = ex.Message
                    Return _CashCardCustomer
                End Try

                tmpSQL = "INSERT INTO customer_contact_details (account_number,email_address,contact_number,state) VALUES " &
                         "('" & AccountDetails.IDNumber & "','" & RG.Apos(AccountDetails.EMail) & "'," &
                         "'" & RG.Apos(AccountDetails.ContactNumber) & "','" & RG.Apos(AccountDetails.Province) & "')"
                Try
                    'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                Catch ex As Exception
                    'objDBWrite.CloseConnection()
                    _CashCardCustomer = ex.Message
                    Return _CashCardCustomer
                End Try

                tmpSQL = "INSERT INTO customer_dates (account_number,date_account_opened) " &
                         "VALUES ('" & AccountDetails.IDNumber & "','" & Format(Now, "yyyy-MM-dd") & "')"
                Try
                    'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                Catch ex As Exception
                    'objDBWrite.CloseConnection()
                    _CashCardCustomer = ex.Message
                    Return _CashCardCustomer
                End Try

                'Need to credit the account with 30 bonus points
                tmpSQL = "INSERT INTO customer_balances (account_number,total_spent,current_balance,current_points_balance,total_points_accrued,credit_limit) " &
                         "VALUES ('" & AccountDetails.IDNumber & "',0,0,30,30,0)"
                Try
                    'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                Catch ex As Exception
                    'objDBWrite.CloseConnection()
                    _CashCardCustomer = ex.Message
                    Return _CashCardCustomer
                End Try

                tmpSQL = "INSERT INTO transaction_customer " &
                 "(guid,sale_date,sale_time,branch_code,account_number,transaction_type,transaction_number,transaction_total," &
                 "transaction_points)" &
                 " VALUES('" & RG.Apos(Guid.NewGuid.ToString) & "','" & Format(Now, "yyyy-MM-dd") & "'," &
                 "'" & Format(Now, "HH:mm:ss") & "','" & AccountDetails.BranchCode & "'," &
                 "'" & AccountDetails.IDNumber & "','BONUS','SU-BONUS','0','30')"

                Try
                    'objDBWrite.ExecuteQuery(tmpSQL, AccountDetails.BranchCode)
                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, AccountDetails.BranchCode)
                Catch ex As Exception
                    _Log.WriteLog(AccountDetails.BranchCode,
                           "Sign Up Bonus, Account Number: " & AccountDetails.IDNumber & " Error: " & ex.Message & " SQL: " & tmpSQL, AccountDetails.IPAddress)
                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If
                    Return True
                Finally
                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If
                End Try
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        End Try

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

        'If (objDBWrite IsNot Nothing) Then
        '    objDBWrite.CloseConnection()
        'End If

        _CashCardCustomer = "Success"

        Return _CashCardCustomer

    End Function

    Public Function GetCustomerDetails(ByVal AccountDetails As CashCardCustomer) As CashCardCustomer

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim PointsToCash As Double = 0

        Dim _CashCardCustomer As New CashCardCustomer

        Dim _Log As New WriteToLog

        'Check if password is valid
        If AccountDetails.Password <> "JaiRL10nFMNo$forany" Then

            _Log.WriteLog(AccountDetails.BranchCode,
                     "GetCustomerDetails: Wrong Web Service Password", AccountDetails.IPAddress)
            _CashCardCustomer.ReturnMessage = "General Error"
            Return _CashCardCustomer
        End If


        'Get details
        tmpSQL = "SELECT cp.first_name,cp.last_name,cp.day_of_birth,cp.month_of_birth,cp.year_of_birth,cp.customer_status,cp.is_female,cp.send_promos,cb.total_spent, " &
                 "cb.current_points_balance,cb.total_points_accrued,cd.card_number,ccd.email_address,ccd.contact_number,ccd.state " &
                 "FROM " &
                 "customer_personal cp " &
                 "LEFT OUTER JOIN customer_balances cb ON cp.account_number = cb.account_number " &
                 "LEFT OUTER JOIN cash_card_details cd ON cp.account_number = cd.account_number " &
                 "LEFT OUTER JOIN customer_contact_details ccd ON cp.account_number = ccd.account_number " &
                 "WHERE cp.account_number = '" & AccountDetails.IDNumber & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, AccountDetails.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, AccountDetails.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _CashCardCustomer.FirstName = dr("first_name") & ""
                    _CashCardCustomer.LastName = dr("last_name") & ""
                    _CashCardCustomer.EMail = dr("email_address") & ""
                    _CashCardCustomer.ContactNumber = dr("contact_number") & ""
                    _CashCardCustomer.Status = dr("customer_status") & ""
                    _CashCardCustomer.Province = dr("state") & ""
                    _CashCardCustomer.Sex = dr("is_female") & ""
                    _CashCardCustomer.DayOfBirth = dr("day_of_birth") & ""
                    _CashCardCustomer.MonthOfBirth = dr("month_of_birth") & ""
                    _CashCardCustomer.YearOfBirth = dr("year_of_birth") & ""

                    _CashCardCustomer.isPromo = dr("send_promos") & ""

                    _CashCardCustomer.TotalSpent = dr("total_spent") & ""
                    _CashCardCustomer.CurrentPointsBalance = dr("current_points_balance") & ""
                    _CashCardCustomer.TotalPointsAccrued = dr("total_points_accrued") & ""

                    _CashCardCustomer.CardNumber = dr("card_number") & ""

                Next
            Else
                _CashCardCustomer.ReturnMessage = "Account Not Found"
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return _CashCardCustomer
            End If
        Catch ex As Exception
            _CashCardCustomer.ReturnMessage = ex.Message
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return _CashCardCustomer
        End Try

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

        Return _CashCardCustomer

    End Function

    Public Function ProcessGiftCardSale(ByVal CashCardTransactionData As CashCardEntity) As String

        Dim objDBPCMRead As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBPCMWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        Dim AccountNumber As String = ""

        Dim _Log As New WriteToLog

        'Check if password is valid
        If CashCardTransactionData.Password <> "JaiRL10nFMNo$forany" Then

            _Log.WriteLog(CashCardTransactionData.BranchCode,
                     "ProcessGiftCardSale: Wrong Web Service Password," & CashCardTransactionData.CardNumber & "," & AccountNumber & "," &
                       CashCardTransactionData.TransactionDate & "," & CashCardTransactionData.TransactionTime & "," &
                       CashCardTransactionData.TransactionType & "," & CashCardTransactionData.TransactionTotal, CashCardTransactionData.IPAddress)
            Return "Error 112"
        End If

        If CashCardTransactionData.GiftCardNumber = "" Then
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                     "ProcessGiftCardSale: No Gift Card number supplied," & CashCardTransactionData.CardNumber & "," & AccountNumber & "," &
                       CashCardTransactionData.TransactionDate & "," & CashCardTransactionData.TransactionTime & "," &
                       CashCardTransactionData.TransactionType & "," & CashCardTransactionData.TransactionTotal, CashCardTransactionData.IPAddress)
            Return "No Gift Card number supplied"
        End If

        If Val(CashCardTransactionData.TransactionTotal) <= 0 Then
            Return "You cannot process a negative amount on a Gift Card Sale, Daniel."
        End If

        '---------------------------------------------------
        'Working with the PCM database
        '---------------------------------------------------
        'check if the gift card is active
        Dim CurrentBalance As Decimal

        tmpSQL = "SELECT " &
                 "card_details.card_number," &
                 "card_details.current_status AS card_status,balance " &
                 "FROM " &
                 "card_details " &
                 "INNER JOIN card_gift_cards gc ON gc.card_number = card_details.card_number " &
                 "WHERE card_details.card_number = '" & CashCardTransactionData.GiftCardNumber & "'"
        Try
            'ds = objDBPCMRead.GetDataSet(tmpSQL, CashCardTransactionData.BranchCode)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    'Check if card is active
                    If dr("card_status") & "" <> "ACTIVE" Then
                        'If (objDBPCMRead IsNot Nothing) Then
                        '    objDBPCMRead.CloseConnection()
                        'End If
                        Return "Error 971"
                    End If

                    CurrentBalance = Val(dr("balance") & "")

                Next
            Else
                'The card number doesn't exist
                'If (objDBPCMRead IsNot Nothing) Then
                '    objDBPCMRead.CloseConnection()
                'End If
                Return "The Gift Card number does not exist on the system"
            End If
        Catch ex As Exception
            'If (objDBPCMRead IsNot Nothing) Then
            '    objDBPCMRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'Check if there is enough money on the gift card to complete the transaction
        If Val(CurrentBalance) < Val(CashCardTransactionData.TransactionTotal) Then
            Return "There is not enough money on this Gift Card to complete the transaction."
        End If

        'Get the current PCM period
        Dim CurrentPeriod As String
        tmpSQL = "SELECT current_period FROM general_settings"
        Try
            'ds = objDBPCMRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    CurrentPeriod = dr("current_period")
                Next
            End If
        Catch ex As Exception
            'If (objDBPCMRead IsNot Nothing) Then
            '    objDBPCMRead.CloseConnection()
            'End If
            Return ex.Message
        Finally
            'If (objDBPCMRead IsNot Nothing) Then
            '    objDBPCMRead.CloseConnection()
            'End If
        End Try

        'Update the balance on the gift card
        tmpSQL = "UPDATE card_gift_cards SET balance = balance - " & Val(CashCardTransactionData.TransactionTotal) & "," &
                 "total_spent = total_spent +  " & Val(CashCardTransactionData.TransactionTotal) & " WHERE card_number = '" & CashCardTransactionData.GiftCardNumber & "'; "
        Try
            'objDBPCMWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
        Catch ex As Exception
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                   "ProcessGiftCardSale: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                   " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
            'If (objDBPCMWrite IsNot Nothing) Then
            '    objDBPCMWrite.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'Write the transaction
        tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," &
                 "transaction_type,transaction_amount,pay_type) VALUES " &
                 "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & CurrentPeriod & "'," &
                 "'WEB SERVICE','" & CashCardTransactionData.GiftCardNumber & "','" & CashCardTransactionData.BranchCode & CashCardTransactionData.TransactionNumber & "'," &
                 "'GCS','" & RG.Apos(CashCardTransactionData.TransactionTotal) & "','0000'); "
        Try
            'objDBPCMWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
        Catch ex As Exception
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                   "ProcessGiftCardSale: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                   " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
            'If (objDBPCMWrite IsNot Nothing) Then
            '    objDBPCMWrite.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'Update the last used date on the card
        tmpSQL = "UPDATE card_dates SET date_last_used = '" & Format(Now, "yyyy-MM-dd") & "' WHERE card_number = '" & CashCardTransactionData.GiftCardNumber & "';"
        Try
            'objDBPCMWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
        Catch ex As Exception
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                   "ProcessGiftCardSale: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                   " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
            'If (objDBPCMWrite IsNot Nothing) Then
            '    objDBPCMWrite.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'write log
        _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "ProcessGiftCardSale: Transaction Completed," & CashCardTransactionData.GiftCardNumber & "," & AccountNumber & "," &
                       CashCardTransactionData.TransactionDate & "," & CashCardTransactionData.TransactionTime & "," &
                       CashCardTransactionData.TransactionType & "," & CashCardTransactionData.TransactionTotal & "," &
                       CashCardTransactionData.PointsToRedeem, CashCardTransactionData.IPAddress)

        'If (objDBPCMRead IsNot Nothing) Then
        '    objDBPCMRead.CloseConnection()
        'End If

        'If (objDBPCMWrite IsNot Nothing) Then
        '    objDBPCMWrite.CloseConnection()
        'End If

        Return "Success"

    End Function

    Public Function RedeemPoints(ByVal CashCardTransactionData As CashCardEntity) As String

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")

        Dim objDBPCMRead As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBPCMWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        Dim AccountNumber As String = ""
        Dim PointsToCash As Double

        Dim _Log As New WriteToLog

        'Check if password is valid
        If CashCardTransactionData.Password <> "JaiRL10nFMNo$forany" Then

            _Log.WriteLog(CashCardTransactionData.BranchCode,
                     "RedeemPoints: Wrong Web Service Password," & CashCardTransactionData.CardNumber & "," & AccountNumber & "," &
                       CashCardTransactionData.TransactionDate & "," & CashCardTransactionData.TransactionTime & "," &
                       CashCardTransactionData.TransactionType & "," & CashCardTransactionData.TransactionTotal, CashCardTransactionData.IPAddress)
            Return "Error 102"
        End If

        If CashCardTransactionData.GiftCardNumber = "" Then
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                     "RedeemPoints: No Gift Card number supplied," & CashCardTransactionData.CardNumber & "," & AccountNumber & "," &
                       CashCardTransactionData.TransactionDate & "," & CashCardTransactionData.TransactionTime & "," &
                       CashCardTransactionData.TransactionType & "," & CashCardTransactionData.TransactionTotal, CashCardTransactionData.IPAddress)
            Return "No Gift Card number supplied"
        End If

        'get cash to points
        tmpSQL = "SELECT points_to_cash FROM company_details"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, CashCardTransactionData.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    PointsToCash = Val(dr("points_to_cash") & "")
                    If PointsToCash = 0 Then
                        _Log.WriteLog(CashCardTransactionData.BranchCode,
                               "RedeemPoints: Points to Cash is zero", CashCardTransactionData.IPAddress)
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return "Points to Cash is zero"
                    End If
                Next
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return ex.Message
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try

        'Get cash card account details
        tmpSQL = "SELECT customer_personal.account_number,customer_balances.current_points_balance,cash_card_details.is_active " &
                 "FROM cash_card_details " &
                 "INNER JOIN customer_personal ON cash_card_details.account_number = customer_personal.account_number " &
                 "INNER JOIN customer_balances ON cash_card_details.account_number = customer_balances.account_number " &
                 "where cash_card_details.card_number = '" & RG.Apos(CashCardTransactionData.CardNumber) & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, CashCardTransactionData.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If dr("is_active") = False Then
                        _Log.WriteLog(CashCardTransactionData.BranchCode,
                               "RedeemPoints: Card Not Active: " & CashCardTransactionData.CardNumber, CashCardTransactionData.IPAddress)
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return "Cash Card Not Active"
                    End If

                    AccountNumber = dr("account_number")

                    If CashCardTransactionData.PointsToRedeem > Val(dr("current_points_balance") & "") Then
                        _Log.WriteLog(CashCardTransactionData.BranchCode,
                               "RedeemPoints: Not enough points to redeem " & CashCardTransactionData.CardNumber, CashCardTransactionData.IPAddress)
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return "Not enough points to redeem"
                    End If

                Next
            Else
                'Card doesn't exist
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                               "RedeemPoints: Card not associated with an account: " & CashCardTransactionData.CardNumber, CashCardTransactionData.IPAddress)
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return "Cash Card not associated with an account"
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        '---------------------------------------------------
        'Working with the PCM database
        '---------------------------------------------------
        'check if the cash card is active
        tmpSQL = "SELECT " &
                 "card_details.card_number," &
                 "card_details.current_status AS card_status " &
                 "FROM " &
                 "card_details " &
                 "WHERE card_details.card_number = '" & CashCardTransactionData.GiftCardNumber & "'"
        Try
            'ds = objDBPCMRead.GetDataSet(tmpSQL, CashCardTransactionData.BranchCode)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    'Check if card is active
                    If dr("card_status") & "" <> "ACTIVE" Then
                        'If (objDBPCMRead IsNot Nothing) Then
                        '    objDBPCMRead.CloseConnection()
                        'End If
                        Return "Error 971"
                    End If
                Next
            Else
                'The card number doesn't exist
                'If (objDBPCMRead IsNot Nothing) Then
                '    objDBPCMRead.CloseConnection()
                'End If
                Return "The Gift Card number does not exist on the system"
            End If
        Catch ex As Exception
            'If (objDBPCMRead IsNot Nothing) Then
            '    objDBPCMRead.CloseConnection()
            'End If
            Return ex.Message
        Finally
            'If (objDBPCMRead IsNot Nothing) Then
            '    objDBPCMRead.CloseConnection()
            'End If
        End Try

        'Get the next transaction number
        Dim TransactionNumber As String
        tmpSQL = "SELECT nextval('cash_card_transaction')"
        Try
            'ds = objDBPCMWrite.GetDataSet(tmpSQL, CashCardTransactionData.BranchCode)
            ds = usingObjDB.GetDataSet(_PCMWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    TransactionNumber = dr("nextval")
                Next
            End If
        Catch ex As Exception
            'If (objDBPCMWrite IsNot Nothing) Then
            '    objDBPCMWrite.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'Get the current PCM period
        Dim CurrentPeriod As String
        tmpSQL = "SELECT current_period FROM general_settings"
        Try
            'ds = objDBPCMRead.GetDataSet(tmpSQL, CashCardTransactionData.BranchCode)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    CurrentPeriod = dr("current_period")
                Next
            End If
        Catch ex As Exception
            'If (objDBPCMRead IsNot Nothing) Then
            '    objDBPCMRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'Calculate how much to put on the Card
        Dim CashToCredit As Double = RG.Numb(Val(CashCardTransactionData.PointsToRedeem) / PointsToCash)

        'DO THIS FIRST!
        'Take the points off the customer's account
        tmpSQL = "UPDATE customer_balances SET current_points_balance = current_points_balance - " & Val(CashCardTransactionData.PointsToRedeem) & " " &
                 "WHERE account_number = '" & Mid$(RG.Apos(AccountNumber), 1, 30) & "'"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
        Catch ex As Exception
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                   "RedeemPoints: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber & " Error: " &
                   ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'Write the transaction to PCM
        tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," &
                 "transaction_type,transaction_amount,pay_type) VALUES " &
                 "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & CurrentPeriod & "'," &
                 "'POINTS REDEMPTION','" & CashCardTransactionData.GiftCardNumber & "','" & CashCardTransactionData.BranchCode & TransactionNumber & CashCardTransactionData.TransactionNumber & "'," &
                 "'GCP','" & CashToCredit & "','0000'); "
        Try
            'objDBPCMWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
        Catch ex As Exception
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                   "RedeemPoints: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                   " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
            'If (objDBPCMWrite IsNot Nothing) Then
            '    objDBPCMWrite.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'Update the balance on the gift card
        tmpSQL = "UPDATE card_gift_cards SET balance = balance + " & CashToCredit & " WHERE card_number = '" & CashCardTransactionData.GiftCardNumber & "'; "
        Try
            'objDBPCMWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
        Catch ex As Exception
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                   "RedeemPoints: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                   " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
            'If (objDBPCMWrite IsNot Nothing) Then
            '    objDBPCMWrite.CloseConnection()
            'End If
            Return ex.Message
        End Try

        'Write the customer transaction to Positive
        tmpSQL = "INSERT INTO transaction_customer " &
                 "(guid,sale_date,sale_time,branch_code,account_number,transaction_type,transaction_number,transaction_total," &
                 "transaction_points)" &
                 " VALUES('" & RG.Apos(Guid.NewGuid.ToString) & "','" & CashCardTransactionData.TransactionDate & "','" & CashCardTransactionData.TransactionTime & "','" & Mid$(RG.Apos(CashCardTransactionData.BranchCode), 1, 5) & "'," &
                 "'" & Mid$(RG.Apos(AccountNumber), 1, 30) & "','" & CashCardTransactionData.TransactionType & "'," &
                 "'" & CashCardTransactionData.BranchCode & TransactionNumber & CashCardTransactionData.TransactionNumber & "'," &
                 "'" & Val(CashCardTransactionData.TransactionTotal) & "'," &
                 "'" & RG.Apos(RG.Numb(CashCardTransactionData.PointsToRedeem)) & "')"

        Try
            'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
        Catch ex As Exception
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                   "RedeemPoints: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                   " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
            Return ex.Message
        End Try


        'write log
        _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "RedeemPoints: Transaction Completed," & CashCardTransactionData.CardNumber & "," & AccountNumber & "," &
                       CashCardTransactionData.TransactionDate & "," & CashCardTransactionData.TransactionTime & "," &
                       CashCardTransactionData.TransactionType & "," & CashCardTransactionData.TransactionTotal & "," &
                       CashCardTransactionData.PointsToRedeem, CashCardTransactionData.IPAddress)

        'If (objDBWrite IsNot Nothing) Then
        '    objDBWrite.CloseConnection()
        'End If

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

        'If (objDBPCMRead IsNot Nothing) Then
        '    objDBPCMRead.CloseConnection()
        'End If

        'If (objDBPCMWrite IsNot Nothing) Then
        '    objDBPCMWrite.CloseConnection()
        'End If


        Return "Success"

    End Function

    Public Function PostCashCardTransactionDL(ByVal CashCardTransactionData As CashCardEntity, ByVal IPAddress As String) As Boolean

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        Dim AccountNumber As String = ""
        Dim CashToPoints As Double

        Dim _Log As New WriteToLog

        'Check if password is valid
        If CashCardTransactionData.Password <> "JaiRL10nFMNo$forany" Then
            _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: Wrong Web Service Password," & CashCardTransactionData.CardNumber & "," & AccountNumber & "," &
                       CashCardTransactionData.TransactionDate & "," & CashCardTransactionData.TransactionTime & "," &
                       CashCardTransactionData.TransactionType & "," & CashCardTransactionData.TransactionTotal, CashCardTransactionData.IPAddress)
            Return True
        End If

        'Check if GuID has already been processed
        tmpSQL = "SELECT ws_guid FROM ws_guids WHERE ws_guid = '" & CashCardTransactionData.GuID & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, IPAddress)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, IPAddress)
            If objDBRead.isR(ds) Then
                'GuID has already been processed
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: GuID has already been processed: " & CashCardTransactionData.GuID & " Card Number: " &
                       CashCardTransactionData.CardNumber, CashCardTransactionData.IPAddress)

                'Just return True and let the GuID get deleted at client

                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If

                Return True
            Else
                'Insert the GuID into the table
                tmpSQL = "INSERT INTO ws_guids (ws_guid) VALUES ('" & CashCardTransactionData.GuID & "')"
                Try
                    'objDBWrite.ExecuteQuery(tmpSQL, IPAddress)
                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, IPAddress)
                Catch ex As Exception
                    _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                       " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If
                    Return True
                End Try
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        End Try


        'Check card number
        tmpSQL = "SELECT account_number, is_active FROM cash_card_details WHERE card_number = " &
                 "'" & CashCardTransactionData.CardNumber & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, CashCardTransactionData.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    'get account number
                    AccountNumber = dr("account_number") & ""
                    If AccountNumber = "" Then
                        _Log.WriteLog(CashCardTransactionData.BranchCode,
                               "PostCashCardTransactionDL: Card not associated with an account: " & CashCardTransactionData.CardNumber, CashCardTransactionData.IPAddress)
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return True
                    End If
                    If dr("is_active") = False Then
                        _Log.WriteLog(CashCardTransactionData.BranchCode,
                               "PostCashCardTransactionDL: Card Not Active: " & CashCardTransactionData.CardNumber, CashCardTransactionData.IPAddress)
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return True
                    End If
                Next
            Else
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: Card Number Does Not Exist: " & CashCardTransactionData.CardNumber, CashCardTransactionData.IPAddress)
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True

            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        End Try

        'get cash to points
        tmpSQL = "SELECT cash_to_points FROM company_details"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, CashCardTransactionData.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    CashToPoints = Val(dr("cash_to_points") & "")
                    If CashToPoints = 0 Then
                        _Log.WriteLog(CashCardTransactionData.BranchCode,
                               "PostCashCardTransactionDL: Cash To Points is zero", CashCardTransactionData.IPAddress)
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return True
                    End If
                Next
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        End Try

        'write trans
        Dim tmpPoints As Double

        If Val(CashToPoints) = 0 Then
            tmpPoints = 0
        Else
            tmpPoints = RG.Numb(Val(CashCardTransactionData.TransactionTotal) / Val(CashToPoints), 0)
        End If

        If CashCardTransactionData.TransactionType = "POSSALE" Then
            tmpSQL = "INSERT INTO transaction_customer " &
                 "(guid,sale_date,sale_time,branch_code,account_number,transaction_type,transaction_number,transaction_total," &
                 "transaction_points)" &
                 " VALUES('" & RG.Apos(Guid.NewGuid.ToString) & "','" & CashCardTransactionData.TransactionDate & "','" & CashCardTransactionData.TransactionTime & "','" & Mid$(RG.Apos(CashCardTransactionData.BranchCode), 1, 5) & "'," &
                 "'" & Mid$(RG.Apos(AccountNumber), 1, 30) & "','" & CashCardTransactionData.TransactionType & "','" & CashCardTransactionData.TransactionNumber & "'," &
                 "'" & Val(CashCardTransactionData.TransactionTotal) & "'," &
                 "'" & RG.Apos(RG.Numb(tmpPoints)) & "')"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber & " Error: " &
                       ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            Finally
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
            End Try

            tmpSQL = "UPDATE customer_balances SET total_spent = total_spent + " & Val(CashCardTransactionData.TransactionTotal) & ", " &
                                "current_balance = current_balance + " & Val(CashCardTransactionData.TransactionTotal) & ", " &
                                "current_points_balance = current_points_balance + " & Val(tmpPoints) & ", " &
                                "total_points_accrued = total_points_accrued + " & Val(tmpPoints) & " " &
                                "WHERE account_number = '" & Mid$(RG.Apos(AccountNumber), 1, 30) & "'"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                      "GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                      " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            End Try

            tmpSQL = "UPDATE customer_dates SET date_last_transaction = '" & CashCardTransactionData.TransactionDate & "', " &
                     "date_last_purchase = '" & CashCardTransactionData.TransactionDate & "' " &
                     "WHERE account_number = '" & Mid$(RG.Apos(AccountNumber), 1, 30) & "'"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                       " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            End Try

        ElseIf CashCardTransactionData.TransactionType = "POSCN" Then
            tmpSQL = "INSERT INTO transaction_customer " &
                     "(guid,sale_date,sale_time,branch_code,account_number,transaction_type,transaction_number,transaction_total," &
                     "transaction_points)" &
                     " VALUES('" & RG.Apos(Guid.NewGuid.ToString) & "','" & CashCardTransactionData.TransactionDate & "','" & CashCardTransactionData.TransactionTime & "','" & Mid$(RG.Apos(CashCardTransactionData.BranchCode), 1, 5) & "'," &
                     "'" & Mid$(RG.Apos(AccountNumber), 1, 30) & "','" & CashCardTransactionData.TransactionType & "','" & CashCardTransactionData.TransactionNumber & "'," &
                     "'-" & Val(CashCardTransactionData.TransactionTotal) & "'," &
                     "'-" & RG.Apos(RG.Numb(tmpPoints)) & "')"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                       " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            End Try


            tmpSQL = "UPDATE customer_balances SET total_spent = total_spent - " & Val(CashCardTransactionData.TransactionTotal) & ", " &
                     "current_balance = current_balance - " & Val(CashCardTransactionData.TransactionTotal) & ", " &
                     "current_points_balance = current_points_balance - " & Val(tmpPoints) & ", " &
                     "total_points_accrued = total_points_accrued - " & Val(tmpPoints) & " " &
                     "WHERE account_number = '" & Mid$(RG.Apos(AccountNumber), 1, 30) & "'"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber & " Error: " &
                       ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            End Try

            tmpSQL = "UPDATE customer_dates SET date_last_transaction = '" & CashCardTransactionData.TransactionDate & "' " &
                     "WHERE account_number = '" & Mid$(RG.Apos(AccountNumber), 1, 30) & "'"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                        "PostCashCardTransactionDL: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                        " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            End Try

        ElseIf CashCardTransactionData.TransactionType = "POSREF" Then
            tmpSQL = "INSERT INTO transaction_customer " &
                     "(guid,sale_date,sale_time,branch_code,account_number,transaction_type,transaction_number,transaction_total," &
                     "transaction_points)" &
                     " VALUES('" & RG.Apos(Guid.NewGuid.ToString) & "','" & CashCardTransactionData.TransactionDate & "','" & CashCardTransactionData.TransactionTime & "','" & Mid$(RG.Apos(CashCardTransactionData.BranchCode), 1, 5) & "'," &
                     "'" & Mid$(RG.Apos(AccountNumber), 1, 30) & "','" & CashCardTransactionData.TransactionType & "','" & CashCardTransactionData.TransactionNumber & "'," &
                     "'-" & Val(CashCardTransactionData.TransactionTotal) & "'," &
                     "'-" & RG.Apos(RG.Numb(tmpPoints)) & "')"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                       " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            End Try


            tmpSQL = "UPDATE customer_balances SET total_spent = total_spent - " & Val(CashCardTransactionData.TransactionTotal) & ", " &
                     "current_balance = current_balance - " & Val(CashCardTransactionData.TransactionTotal) & ", " &
                     "current_points_balance = current_points_balance - " & Val(tmpPoints) & ", " &
                     "total_points_accrued = total_points_accrued - " & Val(tmpPoints) & " " &
                     "WHERE account_number = '" & Mid$(RG.Apos(AccountNumber), 1, 30) & "'"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                       " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            End Try

            tmpSQL = "UPDATE customer_dates SET date_last_transaction = '" & CashCardTransactionData.TransactionDate & "' " &
                     "WHERE account_number = '" & Mid$(RG.Apos(AccountNumber), 1, 30) & "'"

            Try
                'objDBWrite.ExecuteQuery(tmpSQL, CashCardTransactionData.BranchCode)
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL, CashCardTransactionData.BranchCode)
            Catch ex As Exception
                _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: GuID: " & CashCardTransactionData.GuID & " Card Number: " & CashCardTransactionData.CardNumber &
                       " Error: " & ex.Message & " SQL: " & tmpSQL, CashCardTransactionData.IPAddress)
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return True
            End Try
        End If

        'write log
        _Log.WriteLog(CashCardTransactionData.BranchCode,
                       "PostCashCardTransactionDL: Transaction Completed," & CashCardTransactionData.CardNumber & "," & AccountNumber & "," &
                       CashCardTransactionData.TransactionDate & "," & CashCardTransactionData.TransactionTime & "," &
                       CashCardTransactionData.TransactionType & "," & CashCardTransactionData.TransactionTotal & "," &
                       tmpPoints, CashCardTransactionData.IPAddress)

        'If (objDBWrite IsNot Nothing) Then
        '    objDBWrite.CloseConnection()
        'End If

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

        Return True
    End Function

    Public Function ReturnPointsBalance(ByVal CardNumber As CashCardCustomer) As CashCardCustomer

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        Dim PointsToCash As Double = 0

        Dim _CashCardCustomer As New CashCardCustomer

        Dim _Log As New WriteToLog

        'Check if password is valid
        If CardNumber.Password <> "JaiRL10nFMNo$forany" Then

            _Log.WriteLog(CardNumber.BranchCode,
                     "ReturnPointsBalance: Wrong Web Service Password", CardNumber.IPAddress)
            _CashCardCustomer.ReturnMessage = "Card not associated with an account"
            Return _CashCardCustomer
        End If


        'Get details
        tmpSQL = "SELECT customer_personal.first_name,customer_personal.last_name,customer_balances.current_points_balance,cash_card_details.is_active " &
                 "FROM cash_card_details " &
                 "INNER JOIN customer_personal ON cash_card_details.account_number = customer_personal.account_number " &
                 "INNER JOIN customer_balances ON cash_card_details.account_number = customer_balances.account_number " &
                 "where cash_card_details.card_number = '" & RG.Apos(CardNumber.CardNumber) & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, CardNumber.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, CardNumber.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If dr("is_active") = False Then
                        _Log.WriteLog(CardNumber.BranchCode,
                               "ReturnPointsBalance: Card Not Active: " & CardNumber.CardNumber, CardNumber.IPAddress)
                        _CashCardCustomer.ReturnMessage = "Card Not Active"

                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return _CashCardCustomer
                    End If

                    _CashCardCustomer.FullName = dr("first_name") & " " & dr("last_name")
                    _CashCardCustomer.PointsBalance = Val(dr("current_points_balance") & "")

                Next
            Else
                'Card doesn't exist
                _Log.WriteLog(CardNumber.BranchCode,
                              "ReturnPointsBalance: Card not associated with an account: " & CardNumber.CardNumber, CardNumber.IPAddress)
                _CashCardCustomer.ReturnMessage = "Card not associated with an account"

                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return _CashCardCustomer
            End If
        Catch ex As Exception

            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        End Try

        'get cash to points
        tmpSQL = "SELECT points_to_cash FROM company_details"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL, CardNumber.BranchCode)
            ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL, CardNumber.BranchCode)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    PointsToCash = Val(dr("points_to_cash") & "")
                    If PointsToCash = 0 Then
                        _Log.WriteLog(CardNumber.BranchCode,
                              "PostCashCardTransactionDL: Cash To Points is zero: " & CardNumber.CardNumber, CardNumber.IPAddress)
                        _CashCardCustomer.ReturnMessage = "Cash To Points is zero"

                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return _CashCardCustomer
                    End If
                    _CashCardCustomer.PointsToCash = PointsToCash
                Next
            End If

        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        End Try

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

        Return _CashCardCustomer

    End Function

End Class
