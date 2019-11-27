Imports Entities
Imports Npgsql
Imports NpgsqlTypes
Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.IO

Public Class ContactNumbers
    Public _contactnumber As String
    Public _numberdate As String

End Class

Public Class DebtorsDataLayer
    Inherits DataAccessLayerBase

    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil
    Dim getDetailsResponse As New GetDetails

    Public Function GetSelfActivateDetails(ByVal IDNumber As String) As Debtor

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")


        'Dim dlChk As New CashCardDataLayer

        Dim ReturnDebtor As New Debtor


        If RG.ValidID(IDNumber) = False Then
            ReturnDebtor.ReturnMessage = "Invalid ID Number"
            ReturnDebtor.SelfActivate = False
            Return ReturnDebtor
        End If

        ReturnDebtor.SelfActivate = True

        Dim ds As DataSet
        Dim dsB As DataSet

        '2014-08-05
        'Now that the high bad debt stores require a "deposit" on first purchase, they will be allowed to self activate
        'tmpSQL = "SELECT branch_code FROM no_self_activate WHERE branch_code = '" & BranchCode & "'"

        'ds = objDBRead.GetDataSet(tmpSQL)

        'If objDBRead.isR(ds) Then
        '    ReturnDebtor.SelfActivate = False
        'End If
        Dim LostCardCharge As Integer

        tmpSQL = "SELECT lost_card_charge FROM general_settings"
        Try
            'dsB = objDBRead.GetDataSet(tmpSQL)
            dsB = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(dsB) Then
                LostCardCharge = Val(dsB.Tables(0).Rows(0)("lost_card_charge") & "")
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If

            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ex.Message

            Return ReturnDebtor
        End Try


        tmpSQL = "SELECT dp.account_number,dp.first_name,dp.last_name,dp.status,dp.cell_number,dp.itc_rating," &
                 "dp.card_protection,cd.card_number,fcl.credit_limit " &
                 "FROM debtor_personal dp " &
                 "LEFT OUTER JOIN card_details cd ON dp.account_number = cd.account_number " &
                 "LEFT OUTER JOIN financial_balances fcl ON dp.account_number = fcl.account_number " &
                 "WHERE id_number = '" & IDNumber & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                If ds.Tables(0).Rows(0)("card_number") & "" <> "" Then
                    'Existing Card. Check if the customer has LCP
                    If ds.Tables(0).Rows(0)("card_protection") & "" = False Then
                        ReturnDebtor.PayNewCard = LostCardCharge
                        ReturnDebtor.ReturnMessage = "This customer does not have Lost Card Protection and R" &
                            LostCardCharge & " will be charged to their account to receive a new card. Please ask the customer if they want to proceed!"
                    End If
                    'ReturnDebtor.SelfActivate = False
                End If

                If ds.Tables(0).Rows(0)("status") & "" <> "ACTIVE" Then
                    ReturnDebtor.ReturnMessage = "The account is not ACTIVE."

                    ReturnDebtor.SelfActivate = False
                    Return ReturnDebtor
                End If

                ReturnDebtor.AccountNumber = ds.Tables(0).Rows(0)("account_number") & ""
                ReturnDebtor.FirstName = ds.Tables(0).Rows(0)("first_name") & ""
                ReturnDebtor.LastName = ds.Tables(0).Rows(0)("last_name") & ""
                ReturnDebtor.ContactNumber = ds.Tables(0).Rows(0)("cell_number")
                ReturnDebtor.CreditLimit = ds.Tables(0).Rows(0)("credit_limit") & ""
                ReturnDebtor.CurrentStatus = ds.Tables(0).Rows(0)("status") & ""

                'If Val(ds.Tables(0).Rows(0)("credit_limit") & "") > 1500 Then
                '    'MsgBox("This account cannot be self activated." & vbCrLf & "Please call the Call Centre to activate.", MsgBoxStyle.Exclamation)
                '    'DoButtons(Me, True)
                '    'ds.Clear()
                '    'Exit Sub
                '    If (objDBRead IsNot Nothing) Then
                '        objDBRead.CloseConnection()
                '    End If
                '    ReturnDebtor.SelfActivate = False
                'End If

                'objDBRead.CloseConnection()
                ds.Clear()
                Return ReturnDebtor

            Else
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If

                ReturnDebtor.ReturnMessage = "ID Number does not exist on the system"

                ReturnDebtor.SelfActivate = False
                Return ReturnDebtor
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If

            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ex.Message

            Return ReturnDebtor
        End Try

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

    End Function

    Public Function InsertSelfActivated(ByVal DebtorDetails As Debtor) As Debtor
        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        Dim ReturnDebtor As New Debtor
        ReturnDebtor.SelfActivate = True

        Dim ds As DataSet

        tmpSQL = "SELECT card_number,current_status,account_number FROM card_details WHERE card_number = '" & DebtorDetails.CardNumber & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                If ds.Tables(0).Rows(0)("current_status") = "STOLEN" Or ds.Tables(0).Rows(0)("current_status") = "LOST" _
                    Or ds.Tables(0).Rows(0)("current_status") = "BLACKLISTED" Then
                    ReturnDebtor.SelfActivate = False
                    ReturnDebtor.ReturnMessage = ("The card that you used has been Blocked." & vbCrLf & "Please use another card.")
                    'objDBRead.CloseConnection()
                    ds.Clear()
                    Return ReturnDebtor
                End If

                'Card is already in use
                If ds.Tables(0).Rows(0)("account_number") & "" <> "" Then
                    ReturnDebtor.SelfActivate = False
                    'I'm guessing we give this message as if we do say that it is use, the staff will try to buy on it.
                    ReturnDebtor.ReturnMessage = ("The card that you used has been Blocked by Head Office." & vbCrLf & "Please use another card.")
                    'objDBRead.CloseConnection()
                    ds.Clear()
                    Return ReturnDebtor
                End If


            Else
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Invalid Card Number.")
                'objDBRead.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End If
        Catch ex As Exception
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            'objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try


        tmpSQL = "SELECT cell_number,account_number FROM debtor_personal WHERE cell_number = '" & DebtorDetails.ContactNumber & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                If ds.Tables(0).Rows(0)("account_number") <> DebtorDetails.AccountNumber Then
                    ReturnDebtor.SelfActivate = False
                    ReturnDebtor.ReturnMessage = ("This Cellphone number already exists in the Database for another account. Please call the Call Centre to assign a Card to this Account.")
                    'objDBRead.CloseConnection()
                    ds.Clear()
                    Return ReturnDebtor
                End If
            End If
        Catch ex As Exception
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            'objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try

        'Delete all previous cards for this customer
        tmpSQL = "UPDATE card_details SET account_number = NULL, current_status = 'LOST'," &
                 "delivered_by = '" & DebtorDetails.EmployeeNumber & "',assigned_by = '" & DebtorDetails.EmployeeNumber & "'," &
                 "assigned_at_branch = '" & DebtorDetails.BranchCode & "' WHERE account_number = '" & DebtorDetails.AccountNumber & "'"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            'objDBRead.CloseConnection()
            'objDBWrite.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try


        Dim current_period As Integer = 0
        Dim transaction_number As String = ""
        If Val(DebtorDetails.PayNewCard) <> 0 Then
            'Getting a new card
            'Need to charge the customer as they didn't have LCP
            Try
                tmpSQL = "SELECT current_period FROM general_settings"

                'ds = objDBRead.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    current_period = Val(ds.Tables(0).Rows(0)("current_period"))
                End If
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                'objDBRead.CloseConnection()
                'objDBWrite.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End Try

            Try
                tmpSQL = "SELECT nextval('enum_journal_debit_seq')"

                'ds = objDBWrite.GetDataSet(tmpSQL)
                ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
                If usingObjDB.isR(ds) Then
                    transaction_number = ds.Tables(0).Rows(0)("nextval")
                End If
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                'objDBRead.CloseConnection()
                'objDBWrite.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End Try

            'Post the transaction
            Try
                tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," &
                         "transaction_type,transaction_amount,pay_type,branch_code) VALUES " &
                         "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "'," &
                         "'" & current_period & "','WS','" & DebtorDetails.AccountNumber & "'," &
                         "'HO-" & transaction_number & "','LEDD'," &
                         "'" & Val(DebtorDetails.PayNewCard) & "','0000','" & DebtorDetails.BranchCode & "')"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                'objDBRead.CloseConnection()
                'objDBWrite.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End Try

            'Update balances
            Try
                tmpSQL = "UPDATE financial_balances SET total = total + '" & Val(DebtorDetails.PayNewCard) & "'," &
                         "current_balance = '" & RG.Numb(Val(DebtorDetails.PayNewCard) / 6) & "' " &
                         "WHERE account_number = '" & DebtorDetails.AccountNumber & "'"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                'objDBRead.CloseConnection()
                'objDBWrite.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End Try

            'Insert payment plan
            'To charge the full amount in one go might stop people from paying
            Try
                tmpSQL = "INSERT INTO financial_payment_plans (sale_date,sale_time,account_number,reference_number,total_amount,current_period," &
                         "period_1,amount_1,period_2,amount_2,period_3,amount_3,period_4,amount_4,period_5,amount_5,period_6,amount_6) " &
                         "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & DebtorDetails.AccountNumber & "'," &
                         "'HO-" & transaction_number & "','" & Val(DebtorDetails.PayNewCard) & "'," &
                         "'" & current_period & "'," &
                         "'" & current_period & "','" & RG.Numb(Val(DebtorDetails.PayNewCard) / 6) & "'," &
                         "'" & current_period + 1 & "','" & RG.Numb(Val(DebtorDetails.PayNewCard) / 6) & "'," &
                         "'" & current_period + 2 & "','" & RG.Numb(Val(DebtorDetails.PayNewCard) / 6) & "'," &
                         "'" & current_period + 3 & "','" & RG.Numb(Val(DebtorDetails.PayNewCard) / 6) & "'," &
                         "'" & current_period + 4 & "','" & RG.Numb(Val(DebtorDetails.PayNewCard) / 6) & "'," &
                         "'" & current_period + 5 & "','" & RG.Numb(Val(DebtorDetails.PayNewCard) / 6) & "')"

                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                'objDBRead.CloseConnection()
                'objDBWrite.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End Try

        End If

        Dim autoincrease As Boolean = False
        Dim lostcard As Boolean = False

        If DebtorDetails.Autoincrease = True Then
            autoincrease = True
        End If

        If DebtorDetails.LostCard = True Then
            lostcard = True
        End If

        'Check if the customer is already assigned to a branch
        Try
            tmpSQL = "SELECT branch_code FROM debtor_personal WHERE account_number = '" & DebtorDetails.AccountNumber & "'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                If ds.Tables(0).Rows(0)("branch_code") & "" = "" Then
                    'Assign the branch code to the customer
                    tmpSQL = "UPDATE debtor_personal SET branch_code = '" & DebtorDetails.BranchCode & "',auto_increase = '" & autoincrease & "',card_protection = '" & lostcard & "', " &
                         "preferred_language = '" & DebtorDetails.PreferredLanguage & "' WHERE account_number = '" & DebtorDetails.AccountNumber & "'"
                Else
                    tmpSQL = "UPDATE debtor_personal SET auto_increase = '" & autoincrease & "',card_protection = '" & lostcard & "', " &
                         "preferred_language = '" & DebtorDetails.PreferredLanguage & "' WHERE account_number = '" & DebtorDetails.AccountNumber & "'"
                End If
            End If

            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBRead.CloseConnection()
            'objDBWrite.CloseConnection()
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try

        Try
            tmpSQL = "UPDATE card_details SET account_number = '" & DebtorDetails.AccountNumber & "',current_status = 'ACTIVE'," &
                     "delivered_by = '" & DebtorDetails.EmployeeNumber & "',assigned_by = '" & DebtorDetails.EmployeeNumber & "'," &
                     "self_activated = True, assigned_at_branch = '" & DebtorDetails.BranchCode & "' " &
                     "WHERE card_number = '" & DebtorDetails.CardNumber & "'"

            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBRead.CloseConnection()
            'objDBWrite.CloseConnection()
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try

        Try
            tmpSQL = "UPDATE card_dates SET date_assigned = '" & Format(Now, "yyyy-MM-dd") & "' WHERE card_number = '" & DebtorDetails.CardNumber & "'"

            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBRead.CloseConnection()
            'objDBWrite.CloseConnection()
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try

        Try
            tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " &
                     "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','WEB','" & DebtorDetails.AccountNumber & "'," &
                     "'Card Number: " & DebtorDetails.CardNumber & " assigned by " & DebtorDetails.EmployeeNumber & " at " & DebtorDetails.BranchCode & "'," &
                     "'','')"

            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBRead.CloseConnection()
            'objDBWrite.CloseConnection()
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card was still assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try


        'objDBRead.CloseConnection()
        'objDBWrite.CloseConnection()

        Return ReturnDebtor

    End Function

    Public Function GetBranchList(Optional ByVal BranchCode As String = "") As DataSet

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")

        If BranchCode <> "" Then
            tmpSQL = "SELECT * FROM branch_details WHERE branch_code = '" & BranchCode & "' ORDER BY branch_name"
        Else
            tmpSQL = "Select * FROM branch_details ORDER BY branch_name"
        End If


        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                Return ds
            Else
                Return Nothing
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
    End Function

    Public Function UpdateBureauData(ByVal AccountNumber As String) As Boolean

        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        Dim id_number As String = ""
        Dim first_name As String = ""
        Dim last_name As String = ""

        tmpSQL = "SELECT id_number,first_name,last_name " &
                 "FROM debtor_personal " &
                 "WHERE account_number = '" & AccountNumber & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                id_number = ds.Tables(0).Rows(0)("id_number")
                first_name = ds.Tables(0).Rows(0)("first_name")
                last_name = ds.Tables(0).Rows(0)("last_name")
            Else
                Return False
            End If
        Catch
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return False
        End Try


        Dim _result As New srTU.BureauResponse

        _result = GetBureaData(AccountNumber, id_number, first_name, last_name, GetSettings)

        Dim x As New XmlSerializer(_result.GetType)
        Dim sw As New IO.StringWriter()
        x.Serialize(sw, _result)
        Dim xml_input As String = sw.ToString

        If IsNothing(_result) Then
            Return False
        End If

        If _result.ErrorCode <> "" Then
            'Bureau error
            Return False
        End If

        tmpSQL = "SELECT * FROM debtor_xml WHERE account_number = '" & AccountNumber & "'"
        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                'Record exists
                'Backup the existing data

                tmpSQL = "INSERT into debtor_old_xml (account_number,xml_data) SELECT '" & AccountNumber & "',xml_data FROM debtor_xml WHERE account_number = '" & AccountNumber & "'"
                Try
                    'objDBWrite.ExecuteQuery(tmpSQL)
                    usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
                Catch ex As Exception
                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If
                    Return False
                End Try

                'Update with the new data
                tmpSQL = "UPDATE debtor_xml SET xml_data = '" & RG.Apos(xml_input) & "' WHERE account_number = '" & AccountNumber & "'"
                Try
                    'objDBWrite.ExecuteQuery(tmpSQL)
                    usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
                Catch ex As Exception
                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If
                    Return False
                End Try
            Else
                'No xml_data exists for this account
                'Need to insert
                tmpSQL = "INSERT INTO debtor_xml (account_number,xml_data) VALUES ('" & AccountNumber & "','" & RG.Apos(xml_input) & "')"
                Try
                    'objDBWrite.ExecuteQuery(tmpSQL)
                    usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
                Catch ex As Exception
                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If
                    Return False
                End Try
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return False
        End Try

        'Dim _listofnumbers As New List(Of ContactNumbers)
        '_listofnumbers = GetContactNumbers(_result)

        DoContactNumbers(AccountNumber, _result)


    End Function

    Private Sub DoContactNumbers(ByVal AccountNumber As String, ByVal _input As srTU.BureauResponse)


        If IsNothing(_input.ConsumerTelephoneHistoryNW01) Then
            Exit Sub
        End If

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        For i As Integer = 0 To _input.ConsumerTelephoneHistoryNW01.HomeNumbers.Count - 1

            tmpSQL = "INSERT INTO debtor_alternative_contact_numbers (account_number,contact_number,date_of_number) VALUES " &
                     "('" & AccountNumber & "'," &
                     "'" & Mid(_input.ConsumerTelephoneHistoryNW01.HomeNumbers(i).AreaCode, 1, 3) & "-" & Mid$(_input.ConsumerTelephoneHistoryNW01.HomeNumbers(i).Number, 1, 3) & "-" &
                     Mid$(_input.ConsumerTelephoneHistoryNW01.HomeNumbers(i).Number, 4, 4) & "','" &
                     _input.ConsumerTelephoneHistoryNW01.HomeNumbers(i).Date & "')"
            Try
                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
            Catch ex As Exception
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                Exit Sub
            End Try

        Next

        For i As Integer = 0 To _input.ConsumerTelephoneHistoryNW01.WorkNumbers.Count - 1

            tmpSQL = "INSERT INTO debtor_alternative_contact_numbers (account_number,contact_number,date_of_number) VALUES " &
                     "('" & AccountNumber & "'," &
                     "'" & Mid(_input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).AreaCode, 1, 3) & "-" & Mid$(_input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).Number, 1, 3) & "-" &
                     Mid$(_input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).Number, 4, 4) & "','" &
                     _input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).Date & "')"
            Try
                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
            Catch ex As Exception
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                Exit Sub
            End Try

        Next

        For i As Integer = 0 To _input.ConsumerTelephoneHistoryNW01.CellNumbers.Count - 1

            '00000000000845398877
            '12345678901234567890
            tmpSQL = "INSERT INTO debtor_alternative_contact_numbers (account_number,contact_number,date_of_number) VALUES " &
                     "('" & AccountNumber & "'," &
                     "'" & Mid$(_input.ConsumerTelephoneHistoryNW01.CellNumbers(i).Number, 11, 3) & "-" &
                     Mid$(_input.ConsumerTelephoneHistoryNW01.CellNumbers(i).Number, 14, 3) & "-" &
                     Mid$(_input.ConsumerTelephoneHistoryNW01.CellNumbers(i).Number, 17, 4) & "','" &
                     _input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).Date & "')"
            Try
                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
            Catch ex As Exception
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                Exit Sub
            End Try

        Next



    End Sub

    Private Function GetContactNumbers(ByVal _input As srTU.BureauResponse) As List(Of ContactNumbers)

        Dim _result As New List(Of ContactNumbers)

        If IsNothing(_input.ConsumerTelephoneHistoryNW01) Then
            Return _result
        End If

        For i As Integer = 0 To _input.ConsumerTelephoneHistoryNW01.HomeNumbers.Count - 1
            Dim _Phonenumber As New ContactNumbers

            _Phonenumber._contactnumber = _input.ConsumerTelephoneHistoryNW01.HomeNumbers(i).AreaCode & "-" &
                                            Mid$(_input.ConsumerTelephoneHistoryNW01.HomeNumbers(i).Number, 1, 3) & "-" &
                                            Mid$(_input.ConsumerTelephoneHistoryNW01.HomeNumbers(i).Number, 4, 4)
            _Phonenumber._numberdate = _input.ConsumerTelephoneHistoryNW01.HomeNumbers(i).Date

            'If i = 1 Then Exit For
            _result.Add(_Phonenumber)
        Next

        For i As Integer = 0 To _input.ConsumerTelephoneHistoryNW01.WorkNumbers.Count - 1
            Dim _Phonenumber As New ContactNumbers

            _Phonenumber._contactnumber = _input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).AreaCode & "-" &
                                            Mid$(_input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).Number, 1, 3) & "-" &
                                            Mid$(_input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).Number, 4, 4)
            _Phonenumber._numberdate = _input.ConsumerTelephoneHistoryNW01.WorkNumbers(i).Date

            'If i = 1 Then Exit For
            _result.Add(_Phonenumber)
        Next

        For i As Integer = 0 To _input.ConsumerTelephoneHistoryNW01.CellNumbers.Count - 1

            '00000000000845398877
            '12345678901234567890
            Dim _Phonenumber As New ContactNumbers

            _Phonenumber._contactnumber = Mid$(_input.ConsumerTelephoneHistoryNW01.CellNumbers(i).Number, 11, 3) & "-" &
                                            Mid$(_input.ConsumerTelephoneHistoryNW01.CellNumbers(i).Number, 14, 3) & "-" &
                                            Mid$(_input.ConsumerTelephoneHistoryNW01.CellNumbers(i).Number, 17, 4)
            _Phonenumber._numberdate = _input.ConsumerTelephoneHistoryNW01.CellNumbers(i).Date

            'If i = 1 Then Exit For
            _result.Add(_Phonenumber)
        Next

        Return _result

    End Function

    Public Function CheckAccountNumber(ByVal _AccountNumber As String) As DataTable

        Dim xdata As New DataTable

        'Dim connection = Me.DataBase
        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT " &
                     "debtor_personal.first_name," &
                     "debtor_personal.last_name," &
                     "financial_balances.total," &
                     "financial_balances.current_balance," &
                     "financial_balances.p30," &
                     "financial_balances.p60," &
                     "financial_balances.p90," &
                     "financial_balances.p120," &
                     "financial_balances.p150," &
                     "debtor_personal.account_number " &
                     "FROM " &
                     "debtor_personal " &
                     "INNER JOIN financial_balances ON financial_balances.account_number = debtor_personal.account_number " &
                     "WHERE debtor_personal.account_number = '" & RG.Apos(_AccountNumber) & "'"

            'Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            'reader.Fill(xdata)
            xdata = usingObjDB.GetDataTable(_PCMReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xdata

    End Function

    Public Function GetSettings() As DataTable

        Dim xdata As New DataTable

        'Dim connection = Me.DataBase
        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "Select * FROM sms_configuration"

            'Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            'reader.Fill(xdata)
            xdata = usingObjDB.GetDataTable(_PCMReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        Finally
            'If (connection IsNot Nothing) Then
            '    connection.Close()
            'End If

        End Try

        Return xdata

    End Function

    Private Function GetBureaData(ByVal AccountNumber As String, ByVal IDNumber As String, ByVal FirstName As String, ByVal LastName As String,
                                  ByVal _settings As DataTable) As srTU.BureauResponse


        Dim binding = New BasicHttpBinding()
        binding.Name = "cashcardsSoap"

        binding.MaxBufferSize = 1024000
        binding.MaxReceivedMessageSize = 1024000
        binding.TransferMode = TransferMode.Buffered

        binding.ReaderQuotas.MaxDepth = 1024
        binding.ReaderQuotas.MaxStringContentLength = 102400
        binding.ReaderQuotas.MaxArrayLength = 1024000
        binding.ReaderQuotas.MaxBytesPerRead = 1024000
        binding.ReaderQuotas.MaxNameTableCharCount = 1024000

        binding.Security.Mode = BasicHttpSecurityMode.None
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None
        binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None
        binding.Security.Transport.Realm = ""
        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName

        binding.Security.Mode = BasicHttpSecurityMode.Transport

        Dim NewEndPoint As New EndpointAddress(ReturnSetting(_settings, "tu_webservice_address"))

        Dim _sr As New srTU.ConsumerSoapClient(binding, NewEndPoint)

        Dim NewSendData As New srTU.BureauEnquiry13
        NewSendData.EnquirerContactName = ReturnSetting(_settings, "tu_webservice_enquirer_contact_name")
        NewSendData.EnquirerContactPhoneNo = ReturnSetting(_settings, "tu_webservice_enquirer_contact_phone_no")
        NewSendData.EnquiryType = "32"
        NewSendData.SubscriberCode = ReturnSetting(_settings, "tu_webservice_subscriber_code")
        NewSendData.SecurityCode = ReturnSetting(_settings, "tu_webservice_security_code")

        NewSendData.Surname = LastName
        NewSendData.Forename1 = FirstName
        NewSendData.IdentityNo1 = IDNumber

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")
        tmpSQL = "INSERT INTO transunion_log (guid,log_date,log_time,status,error_description,id_number,ip_address,account_number,xml_data) VALUES " &
                "('" & Guid.NewGuid.ToString & "','" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','SUCCESS','Getting New Bureau Data for Account'," &
                "'','127.0.0.1','" & AccountNumber & "','')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
            Return Nothing
        End Try

        Dim _result As New srTU.BureauResponse

        Try
            _result = _sr.ProcessRequestTrans13(NewSendData)
        Catch ex As Exception
            tmpSQL = "INSERT INTO transunion_log (guid,log_date,log_time,status,error_description,id_number,ip_address,account_number,xml_data) VALUES " &
                    "('" & Guid.NewGuid.ToString & "','" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','SUCCESS','Error getting New Bureau Data for Account'," &
                    "'','127.0.0.1','" & AccountNumber & "','" & ex.Message & "')"
            Try
                'objDBWrite.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
            Catch exInner As Exception
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
                Return Nothing
            End Try

            Return Nothing
        End Try

        Dim x As New XmlSerializer(_result.GetType)
        Dim sw As New IO.StringWriter()
        x.Serialize(sw, _result)
        Dim xml_input As String = sw.ToString

        tmpSQL = "INSERT INTO transunion_log (guid,log_date,log_time,status,error_description,id_number,ip_address,account_number,xml_data) VALUES " &
                    "('" & Guid.NewGuid.ToString & "','" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','SUCCESS','Success getting New Bureau Data for Account'," &
                    "'','127.0.0.1','" & AccountNumber & "','" & RG.Apos(xml_input) & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch exInner As Exception
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
            Return Nothing
        End Try

        Return _result

    End Function

    Private Function ReturnSetting(ByVal tmpSetting As DataTable, ByVal theSetting As String) As String

        Dim dr() As DataRow = tmpSetting.Select("sms_setting = '" & theSetting & "'")
        Return dr(0)("setting_value") & ""

    End Function

    Public Function RunPaymentUpload(ByVal Filename As String,
                                     ByVal EMailAddresses As String,
                                     ByVal IDNumberCheck As Boolean) As String


        'Just upload the file and let the Credit Limit Increaser :) run it

        Dim isSuccess As Boolean = True
        Dim ErrorMessage As String = ""

        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "INSERT INTO tasks (task_type,file_to_run,timestamp_created,require_id,email_addresses) VALUES " &
                 "('payment_upload','" & Filename & "','" & Format(Now, "yyyy-MM-dd HH:mm") & "'," &
                 "'" & IDNumberCheck & "','" & EMailAddresses & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            'objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function GetQueryResponse(_historyRequest As HistoryRequest) As GetHistoryResponse
        Dim getHistoryResponse As New GetHistoryResponse

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim hisName As String
        Dim hisId As String
        Dim hisContact As String
        Dim hisCard1 As String
        Dim hisCard2 As String
        'Dim CurBalance As String
        'Dim strCreditLimit As String
        'Dim dblRunningBalance As Double

        tmpSQL = "SELECT debtor_personal.title,debtor_personal.initials,debtor_personal.last_name,debtor_personal.id_number," &
                    "debtor_personal.home_telephone,debtor_personal.home_telephone_2,debtor_personal.cell_number,card_details.card_number," &
                    "financial_balances.credit_limit,financial_balances.total " &
                    "From debtor_personal " &
                    "LEFT OUTER JOIN card_details ON debtor_personal.account_number = card_details.account_number " &
                    "LEFT OUTER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
                    "WHERE debtor_personal.account_number = '" & RG.Apos(_historyRequest.Accountnumber) & "'"

        Try

            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                hisName = ds.Tables(0).Rows(0)("title") & " " & ds.Tables(0).Rows(0)("initials") & " " & ds.Tables(0).Rows(0)("last_name")
                hisId = ds.Tables(0).Rows(0)("id_number")

                If ds.Tables(0).Rows(0)("home_telephone") <> "___-___-____" Then
                    hisContact = ds.Tables(0).Rows(0)("home_telephone")
                Else
                    If ds.Tables(0).Rows(0)("home_telephone_2") <> "___-___-____" Then
                        hisContact = ds.Tables(0).Rows(0)("home_telephone_2")
                    Else
                        If ds.Tables(0).Rows(0)("cell_number") <> "___-___-____" Then
                            hisContact = ds.Tables(0).Rows(0)("cell_number")
                        End If
                    End If
                End If

                getHistoryResponse.CreditLimit = ds.Tables(0).Rows(0)("credit_limit") & ""
                getHistoryResponse.Balance = Format(Val(ds.Tables(0).Rows(0)("total")), "0.00")

                hisCard1 = ds.Tables(0).Rows(0)("card_number") & ""
                hisCard2 = "" 'rS(Mi).Fields("ccnum2") & ""
                'txtContact = rS(mi).Fields("")
            Else
                getHistoryResponse.Message = "The Account Number does not exist."
                getHistoryResponse.Success = False
                Return getHistoryResponse
            End If


            ' Account changes

            tmpSQL = "SELECT debtor_change_log.account_number,debtor_change_log.change_date,debtor_change_log.change_time," &
              "debtor_change_log.description,debtor_change_log.old_value,debtor_change_log.new_value," &
              "debtor_change_log.username " &
              "From debtor_change_log " &
              "Where debtor_change_log.account_number = '" & RG.Apos(_historyRequest.Accountnumber) & "'"

            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            getHistoryResponse.AccountChanges = ds.Tables(0)

            'Transactions

            tmpSQL = "SELECT * FROM financial_transactions WHERE account_number = '" & _historyRequest.Accountnumber & "' ORDER BY financial_transactions_id ASC"

            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            getHistoryResponse.Transactions = ds.Tables(0)

            ' Getting Closing Balances

            tmpSQL = "SELECT financial_closing_balances.total,financial_closing_balances.current_balance," &
                        "financial_closing_balances.p30,financial_closing_balances.p60,financial_closing_balances.p90," &
                        "financial_closing_balances.p120,financial_closing_balances.p150,financial_closing_balances.current_period " &
                        "From financial_closing_balances WHERE account_number = '" & RG.Apos(_historyRequest.Accountnumber) & "' " &
                        "ORDER BY financial_closing_balances_id ASC"

            'ds = objDB.GetDataSet(tmpSQL)
            ds=usingObjDB.GetDataSet(_PCMReadConnectionString,tmpSQL)
            getHistoryResponse.ClosingBalances = ds.Tables(0)

            ' Getting Age Analysis

            tmpSQL = "SELECT * FROM financial_balances WHERE account_number = '" & RG.Apos(_historyRequest.Accountnumber) & "' ORDER BY financial_balances_id ASC"

            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            getHistoryResponse.AgeAnalysis = ds.Tables(0)

            ' Payment Plans

            tmpSQL = "SELECT * FROM financial_payment_plans WHERE account_number = '" & RG.Apos(_historyRequest.Accountnumber) & "' ORDER BY financial_payment_plans_id ASC"

            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            getHistoryResponse.PaymentPlans = ds.Tables(0)


        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            getHistoryResponse.Message = "Something Went Wrong"
            getHistoryResponse.Success = False
            Return getHistoryResponse
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try
        getHistoryResponse.Success = True
        Return getHistoryResponse
    End Function


    Public Function GetCardDetails(_cardSumRequest As CardSumRequest) As GetDetails
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT debtor_personal.first_name,debtor_personal.title,debtor_personal.last_name,card_details.assigned_by,card_details.created_by," &
                 "card_details.delivered_by,card_details.account_number,card_details.assigned_at_branch,card_details.current_status," &
                 "card_dates.date_created,card_dates.date_modified,card_dates.date_assigned,card_dates.date_last_used " &
                 "From card_dates " &
                 "LEFT OUTER JOIN card_details ON card_details.card_number = card_dates.card_number " &
                 "LEFT OUTER JOIN debtor_personal ON card_details.account_number = debtor_personal.account_number " &
                 "Where card_details.card_number = '" & RG.Apos(_cardSumRequest.CardNumber) & "'"

        Try

            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            getDetailsResponse.GetDatable = ds.Tables(0)
            getDetailsResponse.Success = True
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            getDetailsResponse.Message = "Something Went Wrong"
            getDetailsResponse.Success = False
            Return getDetailsResponse
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return getDetailsResponse

    End Function

    Public Function GetDebtorData(ByVal AccountNumber As String) As Debtor

        Dim _NewDebtor As New Debtor
        Dim _NewAgeAnalysis As New Debtor_AgeAnalysis
        Dim _NewAgeAnalysisList As New List(Of Debtor_AgeAnalysis)
        Dim _NewPaymentPlan As New List(Of Debtor_PaymentPlan)
        Dim _NewTransactions As New List(Of Transactions)
        Dim _NewClosingBalances As New List(Of Debtor_ClosingBalances)
        Dim _NewContactHistory As New List(Of Debtor_ContactHistory)
        Dim _NewLastTransactions As New Debtor_LastTransactions

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT dp.account_number,dp.first_name,dp.last_name,dp.cell_number,dp.itc_rating," &
                 "dp.preferred_language,fb.credit_limit,fb.total,fb.current_balance,fb.p30,fb.p60," &
                 "fb.p90,fb.p120,fb.p150,fb.current_contact_level,fb.next_contact_time " &
                 "FROM debtor_personal dp " &
                 "INNER JOIN financial_balances fb ON dp.account_number = fb.account_number " &
                 "WHERE dp.account_number = '" & AccountNumber & "'"

        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _NewDebtor.AccountNumber = dr("account_number") & ""
                    _NewDebtor.FirstName = dr("first_name") & ""
                    _NewDebtor.LastName = dr("last_name") & ""
                    _NewDebtor.ContactNumber = dr("cell_number") & ""
                    _NewDebtor.CreditLimit = dr("credit_limit") & ""
                    _NewDebtor.Balance = dr("total") & ""
                    _NewDebtor.Overdue = Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & "")
                    _NewDebtor.CurrentContactLevel = Val(dr("current_contact_level") & "")
                    _NewDebtor.NextContactTime = dr("next_contact_time") & ""
                    _NewDebtor.PreferredLanguage = dr("preferred_language") & ""

                    _NewDebtor.ConsumerRating = dr("itc_rating") & ""

                    _NewAgeAnalysis.aaTotal = dr("total") & ""
                    _NewAgeAnalysis.aaCurrent = dr("current_balance") & ""
                    _NewAgeAnalysis.aa30Days = dr("p30") & ""
                    _NewAgeAnalysis.aa60Days = dr("p60") & ""
                    _NewAgeAnalysis.aa90Days = dr("p90") & ""
                    _NewAgeAnalysis.aa120Days = dr("p120") & ""
                    _NewAgeAnalysis.aa150Days = dr("p150") & ""
                    _NewAgeAnalysisList.Add(_NewAgeAnalysis)

                Next
                _NewPaymentPlan = GetPaymentsPlans(AccountNumber)
                _NewTransactions = GetTransactions(AccountNumber)
                _NewClosingBalances = GetClosingBalances(AccountNumber)
                _NewContactHistory = GetContactHistory(AccountNumber)
                _NewLastTransactions = GetLastTransactions(AccountNumber)

            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        If Not IsNothing(_NewLastTransactions) Then
            _NewDebtor.LastPaymentAmount = _NewLastTransactions.LastPaymentAmount
            _NewDebtor.LastPaymentDate = _NewLastTransactions.LastPaymentDate
            _NewDebtor.LastSaleAmount = _NewLastTransactions.LastSaleAmount
            _NewDebtor.LastSaleDate = _NewLastTransactions.LastSaleDate
        End If

        _NewDebtor.AgeAnalysis = _NewAgeAnalysisList
        _NewDebtor.PaymentPlans = _NewPaymentPlan
        _NewDebtor.Transactions = _NewTransactions
        _NewDebtor.ClosingBalances = _NewClosingBalances
        _NewDebtor.ContactHistory = _NewContactHistory

        Return _NewDebtor

    End Function

    Public Function GetDebtorContactInvestigationData(ByVal AccountNumber As String) As DebtorContactInvestigation

        Dim _NewDebtor As New DebtorContactInvestigation
        Dim _NewAgeAnalysis As New Debtor_AgeAnalysis
        Dim _NewAgeAnalysisList As New List(Of Debtor_AgeAnalysis)
        Dim _NewContactHistory As New List(Of Debtor_ContactHistory)
        Dim _NewLastTransactions As New Debtor_LastTransactions
        Dim _NewChanges As New List(Of DebtorChangeLog)
        Dim _AltNumbers As New List(Of AlternativeNumbers)
        Dim _NewTransactions As New List(Of Transactions)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT dp.account_number,dp.id_number,dp.first_name,dp.last_name,dp.itc_rating,dp.cell_number," &
                 "dp.preferred_language,fb.first_contact_investigation_failed,fb.credit_limit,fb.total,fb.current_balance,fb.p30,fb.p60," &
                 "fb.p90,fb.p120,fb.p150,fb.current_contact_level,fb.next_contact_time,dp.home_telephone,dp.home_telephone_2,dp.fax_number," &
                 "dp.spouse_cell_number,dp.next_of_kin,dp.next_of_kin_contact_number,dp.send_promos,de.employer_telephone_1 " &
                 "FROM debtor_personal dp " &
                 "INNER JOIN financial_balances fb ON dp.account_number = fb.account_number " &
                 "INNER JOIN debtor_employment de ON dp.account_number = de.account_number " &
                 "WHERE dp.account_number = '" & AccountNumber & "' AND dp.status = 'ACTIVE' ORDER BY fb.total DESC"
        'Added 2019-04-16 on behalf of Barbara
        '"WHERE dp.account_number = '" & AccountNumber & "'"

        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _NewDebtor.IDNumber = dr("id_number") & ""
                    _NewDebtor.AccountNumber = dr("account_number") & ""
                    _NewDebtor.FirstName = dr("first_name") & ""
                    _NewDebtor.LastName = dr("last_name") & ""

                    _NewDebtor.ContactNumber = dr("cell_number") & ""
                    _NewDebtor.AltNumber = dr("fax_number") & ""
                    _NewDebtor.HomeNumber1 = dr("home_telephone") & ""
                    _NewDebtor.HomeNumber2 = dr("home_telephone_2") & ""
                    _NewDebtor.NextOfKin = dr("next_of_kin") & ""
                    _NewDebtor.NextOfKinNumber = dr("next_of_kin_contact_number") & ""
                    _NewDebtor.SpouseContactNumber = dr("spouse_cell_number") & ""
                    _NewDebtor.WorkNumber = dr("employer_telephone_1") & ""

                    _NewDebtor.PreferredLanguage = dr("preferred_language") & ""

                    _NewDebtor.SendPromos = dr("send_promos") & ""
                    _NewDebtor.CreditLimit = dr("credit_limit") & ""
                    _NewDebtor.Balance = dr("total") & ""
                    _NewDebtor.Overdue = Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & "")

                    _NewDebtor.ConsumerRating = dr("itc_rating") & ""

                    _NewAgeAnalysis.aaTotal = dr("total") & ""
                    _NewAgeAnalysis.aaCurrent = dr("current_balance") & ""
                    _NewAgeAnalysis.aa30Days = dr("p30") & ""
                    _NewAgeAnalysis.aa60Days = dr("p60") & ""
                    _NewAgeAnalysis.aa90Days = dr("p90") & ""
                    _NewAgeAnalysis.aa120Days = dr("p120") & ""
                    _NewAgeAnalysis.aa150Days = dr("p150") & ""
                    _NewAgeAnalysisList.Add(_NewAgeAnalysis)

                    _NewDebtor.FirstContactInvestigationFailed = dr("first_contact_investigation_failed") & ""

                Next
                _NewContactHistory = GetContactHistory(AccountNumber)
                _NewLastTransactions = GetLastTransactions(AccountNumber)
                _NewChanges = GetChanges(AccountNumber)
                _AltNumbers = GetAlternativeNumbers(AccountNumber)
                _NewTransactions = GetTransactions(AccountNumber)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        If Not IsNothing(_NewLastTransactions) Then
            _NewDebtor.LastPaymentAmount = _NewLastTransactions.LastPaymentAmount
            _NewDebtor.LastPaymentDate = _NewLastTransactions.LastPaymentDate
            _NewDebtor.LastSaleAmount = _NewLastTransactions.LastSaleAmount
            _NewDebtor.LastSaleDate = _NewLastTransactions.LastSaleDate
        End If

        _NewDebtor.AgeAnalysis = _NewAgeAnalysisList
        _NewDebtor.ContactHistory = _NewContactHistory
        _NewDebtor.ChangeHistory = _NewChanges
        _NewDebtor.AlternativeContactNumbers = _AltNumbers
        _NewDebtor.Transactions = _NewTransactions

        Return _NewDebtor

    End Function

    Public Function GetDebitOrders() As DataTable
        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT debtor_banking.bank_name,debtor_banking.branch_name,debtor_banking.branch_number," &
             "debtor_banking.bank_account_number,debtor_personal.account_number,debtor_personal.title,debtor_personal.initials,debtor_personal.last_name," &
             "financial_balances.current_balance,financial_balances.p30,financial_balances.p90,financial_balances.p60," &
             "financial_balances.p120,financial_balances.p150, " &
             "replace(cast(COALESCE(financial_balances.current_balance, 0) + COALESCE(financial_balances.p30, 0) + COALESCE(financial_balances.p90, 0) + COALESCE(financial_balances.p120, 0) + COALESCE(financial_balances.p150, 0) as money)::text,'$', '') AS owning " &
             "From debtor_banking " &
             "Inner Join debtor_personal ON debtor_banking.account_number = debtor_personal.account_number " &
             "Inner Join financial_balances ON debtor_banking.account_number = financial_balances.account_number AND " &
             "(financial_balances.current_balance + financial_balances.p30 + financial_balances.p60 " &
             "+ financial_balances.p90 + + financial_balances.p120 + financial_balances.p150) > 0 " &
             "Where debtor_banking.payment_type = 'DEBIT ORDER' ORDER BY debtor_personal.account_number ASC"

        Try
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                Return ds.Tables(0)
            Else
                Return Nothing
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
    End Function

    Public Function getDebtorsSumDetails() As DebtorsSumResponse
        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")

        Dim _DebtorsSumResponse As New DebtorsSumResponse
        Try
            'REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00',''	) AS number_of_accounts
            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts FROM debtor_personal"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblTA = ds.Tables(0).Rows(0)("number_of_accounts")
            End If



            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE card_protection = True"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblLCP = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblLCPP = Format(Val(_DebtorsSumResponse.lblLCP) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If


            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE status = 'ACTIVE'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblAD = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblAp = Format(Val(_DebtorsSumResponse.lblAD) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If

            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE status = 'PENDING'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblPend = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblPendP = Format(Val(_DebtorsSumResponse.lblPendP) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If

            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE status = 'FRAUD'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblFraud = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblFraudP = Format(Val(_DebtorsSumResponse.lblFraud) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If


            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE status = 'SUSPENDED'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblSusp = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblSuspP = Format(Val(_DebtorsSumResponse.lblSusp) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If

            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE status = 'WRITE-OFF'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblWO = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblWOP = Format(Val(_DebtorsSumResponse.lblWO) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If

            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE status = 'BLOCKED'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblBlock = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblBlockP = Format(Val(_DebtorsSumResponse.lblBlock) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If

            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE status = 'DECLINED'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblDD = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblDDP = Format(Val(_DebtorsSumResponse.lblDD) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If


            tmpSQL = "SELECT REPLACE(REPLACE(CAST(COUNT(account_number) AS money)::text,'$', ''),'.00','') AS number_of_accounts " &
                     "FROM debtor_personal WHERE status = 'LEGAL'"
            'ds = objDBRead.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                _DebtorsSumResponse.lblLeg = ds.Tables(0).Rows(0)("number_of_accounts")
                If Val(_DebtorsSumResponse.lblTA) <> 0 Then
                    _DebtorsSumResponse.lblLegP = Format(Val(_DebtorsSumResponse.lblLeg) / Val(_DebtorsSumResponse.lblTA) * 100, "0")
                End If
            End If

        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            _DebtorsSumResponse.Success = False
            _DebtorsSumResponse.Message = "Somthing went wrong. Pleast try again."
        Finally
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
        End Try
        _DebtorsSumResponse.Success = True
        _DebtorsSumResponse.Message = Nothing
        Return _DebtorsSumResponse
    End Function


    Public Function GetDebtorInvestigationData(ByVal AccountNumber As String) As Debtor

        Dim _NewDebtor As New Debtor
        Dim _NewAgeAnalysis As New Debtor_AgeAnalysis
        Dim _NewAgeAnalysisList As New List(Of Debtor_AgeAnalysis)
        Dim _NewPaymentPlan As New List(Of Debtor_PaymentPlan)
        Dim _NewTransactions As New List(Of Transactions)
        Dim _NewClosingBalances As New List(Of Debtor_ClosingBalances)
        Dim _NewContactHistory As New List(Of Debtor_ContactHistory)
        Dim _NewLastTransactions As New Debtor_LastTransactions
        Dim _NewChanges As New List(Of DebtorChangeLog)
        Dim _AltNumbers As New List(Of AlternativeNumbers)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT dp.account_number,dp.status,dp.first_name,dp.itc_rating,dp.last_name,dp.cell_number,dp.preferred_language," &
                 "fb.credit_limit,fb.total,fb.current_balance,fb.p30,fb.p60," &
                 "fb.p90,fb.p120,fb.p150,fb.current_contact_level,fb.next_contact_time,dp.home_telephone,dp.home_telephone_2,dp.fax_number," &
                 "dp.spouse_cell_number,dp.next_of_kin,dp.next_of_kin_contact_number,dp.send_promos,de.employer_telephone_1 " &
                 "FROM debtor_personal dp " &
                 "INNER JOIN financial_balances fb ON dp.account_number = fb.account_number " &
                 "INNER JOIN debtor_employment de ON dp.account_number = de.account_number " &
                 "WHERE dp.account_number = '" & AccountNumber & "'"

        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _NewDebtor.AccountNumber = dr("account_number") & ""
                    _NewDebtor.CurrentStatus = dr("status") & ""
                    _NewDebtor.FirstName = dr("first_name") & ""
                    _NewDebtor.LastName = dr("last_name") & ""

                    _NewDebtor.ContactNumber = dr("cell_number") & ""
                    _NewDebtor.AltNumber = dr("fax_number") & ""
                    _NewDebtor.HomeNumber1 = dr("home_telephone") & ""
                    _NewDebtor.HomeNumber2 = dr("home_telephone_2") & ""
                    _NewDebtor.NextOfKin = dr("next_of_kin") & ""
                    _NewDebtor.NextOfKinNumber = dr("next_of_kin_contact_number") & ""
                    _NewDebtor.SpouseContactNumber = dr("spouse_cell_number") & ""
                    _NewDebtor.WorkNumber = dr("employer_telephone_1") & ""

                    _NewDebtor.PreferredLanguage = dr("preferred_language") & ""

                    _NewDebtor.ConsumerRating = dr("itc_rating") & ""

                    _NewDebtor.CurrentContactLevel = dr("current_contact_level") & ""

                    _NewDebtor.SendPromos = dr("send_promos") & ""
                    _NewDebtor.CreditLimit = dr("credit_limit") & ""
                    _NewDebtor.Balance = dr("total") & ""
                    _NewDebtor.Overdue = Val(dr("p60") & "") + Val(dr("p90") & "") + Val(dr("p120") & "") + Val(dr("p150") & "")

                    _NewAgeAnalysis.aaTotal = dr("total") & ""
                    _NewAgeAnalysis.aaCurrent = dr("current_balance") & ""
                    _NewAgeAnalysis.aa30Days = dr("p30") & ""
                    _NewAgeAnalysis.aa60Days = dr("p60") & ""
                    _NewAgeAnalysis.aa90Days = dr("p90") & ""
                    _NewAgeAnalysis.aa120Days = dr("p120") & ""
                    _NewAgeAnalysis.aa150Days = dr("p150") & ""
                    _NewAgeAnalysisList.Add(_NewAgeAnalysis)

                Next
                _NewPaymentPlan = GetPaymentsPlans(AccountNumber)
                _NewTransactions = GetTransactions(AccountNumber)
                _NewClosingBalances = GetClosingBalances(AccountNumber)
                _NewContactHistory = GetContactHistory(AccountNumber)
                _NewLastTransactions = GetLastTransactions(AccountNumber)
                _NewChanges = GetChanges(AccountNumber)
                _AltNumbers = GetAlternativeNumbers(AccountNumber)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        If Not IsNothing(_NewLastTransactions) Then
            _NewDebtor.LastPaymentAmount = _NewLastTransactions.LastPaymentAmount
            _NewDebtor.LastPaymentDate = _NewLastTransactions.LastPaymentDate
            _NewDebtor.LastSaleAmount = _NewLastTransactions.LastSaleAmount
            _NewDebtor.LastSaleDate = _NewLastTransactions.LastSaleDate
        End If

        _NewDebtor.AgeAnalysis = _NewAgeAnalysisList
        _NewDebtor.PaymentPlans = _NewPaymentPlan
        _NewDebtor.Transactions = _NewTransactions
        _NewDebtor.AlternativeContactNumbers = _AltNumbers
        _NewDebtor.ClosingBalances = _NewClosingBalances
        _NewDebtor.ContactHistory = _NewContactHistory
        _NewDebtor.ChangeHistory = _NewChanges

        Return _NewDebtor

    End Function


    Public Function GetPaymentsPlans(ByVal AccountNumber As String) As List(Of Debtor_PaymentPlan)

        Dim PaymentPlans As New List(Of Debtor_PaymentPlan)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD HH24:MI:SS') AS ppDate,sale_time ppTime,reference_number ppReference,total_amount ppTotal,period_1 ppPeriod1,amount_1 ppAmount1,period_2 ppPeriod2," &
                  "amount_2 ppAmount2,period_3 ppPeriod3," &
                  "amount_3 ppAmount3,period_4 ppPeriod4,amount_4 ppAmount4,period_5 ppPeriod5,amount_5 ppAmount5,period_6 ppPeriod6,amount_6 ppAmount6 " &
                  "FROM financial_payment_plans WHERE account_number = '" & AccountNumber & "' ORDER BY financial_payment_plans_id DESC"
        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _PaymentPlan As New Debtor_PaymentPlan

                    _PaymentPlan.ppDate = dr("ppDate") & ""
                    _PaymentPlan.ppTime = dr("ppTime") & ""
                    _PaymentPlan.ppReference = dr("ppReference") & ""

                    _PaymentPlan.ppAmount1 = dr("ppAmount1") & ""
                    _PaymentPlan.ppAmount2 = dr("ppAmount2") & ""
                    _PaymentPlan.ppAmount3 = dr("ppAmount3") & ""
                    _PaymentPlan.ppAmount4 = dr("ppAmount4") & ""
                    _PaymentPlan.ppAmount5 = dr("ppAmount5") & ""
                    _PaymentPlan.ppAmount6 = dr("ppAmount6") & ""

                    _PaymentPlan.ppPeriod1 = dr("ppPeriod1") & ""
                    _PaymentPlan.ppPeriod2 = dr("ppPeriod2") & ""
                    _PaymentPlan.ppPeriod3 = dr("ppPeriod3") & ""
                    _PaymentPlan.ppPeriod4 = dr("ppPeriod4") & ""
                    _PaymentPlan.ppPeriod5 = dr("ppPeriod5") & ""
                    _PaymentPlan.ppPeriod6 = dr("ppPeriod6") & ""

                    _PaymentPlan.ppTotal = dr("ppTotal") & ""

                    PaymentPlans.Add(_PaymentPlan)
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return PaymentPlans

    End Function

    Public Function GetClosingBalances(ByVal AccountNumber As String) As List(Of Debtor_ClosingBalances)

        Dim _ClosingBalances As New List(Of Debtor_ClosingBalances)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT total cbTotal,current_balance cbCurrent,p30 cb30Days,p60 cb60Days,p90 cb90Days,p120 cb120Days,p150 cb150Days,current_period cbPeriod " &
                 "FROM financial_closing_balances " &
                 "WHERE account_number = '" & AccountNumber & "' ORDER BY financial_closing_balances_id DESC"

        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _ClosingBalance As New Debtor_ClosingBalances

                    _ClosingBalance.cbPeriod = dr("cbPeriod") & ""
                    _ClosingBalance.cbCurrent = dr("cbCurrent") & ""
                    _ClosingBalance.cb30Days = dr("cb30Days") & ""
                    _ClosingBalance.cb60Days = dr("cb60Days") & ""
                    _ClosingBalance.cb90Days = dr("cb90Days") & ""
                    _ClosingBalance.cb120Days = dr("cb120Days") & ""
                    _ClosingBalance.cb150Days = dr("cb150Days") & ""
                    _ClosingBalance.cbTotal = dr("cbTotal") & ""

                    _ClosingBalances.Add(_ClosingBalance)
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return _ClosingBalances

    End Function

    Public Function GetChanges(ByVal AccountNumber As String) As List(Of DebtorChangeLog)

        Dim _Changes As New List(Of DebtorChangeLog)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT TO_CHAR(change_date, 'YYYY-MM-DD') AS change_date," &
                 "change_time,description,old_value,new_value,username " &
                 "FROM debtor_change_log " &
                 "WHERE account_number = '" & AccountNumber & "' ORDER BY debtor_change_log_id DESC"

        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _Change As New DebtorChangeLog

                    _Change.ChangeDate = dr("change_date") & ""
                    _Change.ChangeTime = dr("change_time") & ""
                    _Change.ChangeDescription = dr("description") & ""
                    _Change.NewValue = dr("new_value") & ""
                    _Change.OldValue = dr("old_value") & ""
                    _Change.Username = dr("username") & ""

                    _Changes.Add(_Change)
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return _Changes

    End Function

    Public Function GetContactHistory(ByVal AccountNumber As String) As List(Of Debtor_ContactHistory)

        Dim _ContactHistoryList As New List(Of Debtor_ContactHistory)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT TO_CHAR(timestamp_of_contact, 'YYYY-MM-DD HH24:MI:SS') AS timestamp_of_contact,type_of_contact,result_of_action,ptp_amount,action_notes,ptp_date,username " &
                 "FROM debtor_contact_history " &
                 "WHERE account_number = '" & AccountNumber & "' ORDER BY timestamp_of_contact DESC"
        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _ContactHistory As New Debtor_ContactHistory

                    _ContactHistory.ActionNotes = dr("action_notes") & ""
                    _ContactHistory.PTPAmount = dr("ptp_amount") & ""
                    _ContactHistory.PTPDate = dr("ptp_date") & ""
                    _ContactHistory.ResultOfAction = dr("result_of_action") & ""
                    _ContactHistory.TimeStampOfAction = dr("timestamp_of_contact") & ""
                    _ContactHistory.TypeOfContact = dr("type_of_contact") & ""
                    _ContactHistory.ActionUser = dr("username") & ""

                    _ContactHistoryList.Add(_ContactHistory)
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return _ContactHistoryList

    End Function

    Public Function GetAlternativeNumbers(ByVal AccountNumber As String) As List(Of AlternativeNumbers)

        Dim _AlternativeNumbersList As New List(Of AlternativeNumbers)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT * FROM debtor_alternative_contact_numbers WHERE account_number = '" & AccountNumber & "' ORDER BY contact_number DESC"
        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _AltNumber As New AlternativeNumbers
                    _AltNumber.NumberNumber = dr("contact_number") & ""
                    _AltNumber.NumberUpdateDate = dr("date_of_number") & ""
                    _AltNumber.DateRecordInserted = dr("date_record_inserted") & ""
                    _AlternativeNumbersList.Add(_AltNumber)
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return _AlternativeNumbersList

    End Function

    Public Function GetLastTransactions(ByVal AccountNumber As String) As Debtor_LastTransactions

        Dim _LastTransactions As New Debtor_LastTransactions
        Dim LastSaleDate As Date
        Dim LastSaleDateString As String
        Dim LastPaymentDate As Date
        Dim LastPaymentDateString As String

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT * FROM " &
                    "(SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date, transaction_amount AS sale_amount " &
                        "FROM financial_transactions WHERE account_number = '" & AccountNumber & "' AND transaction_type = 'SALE' ORDER BY sale_date desc limit 1) AS s1 " &
                    "FULL JOIN " &
                    "(SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS pay_date, transaction_amount AS pay_amount " &
                        "FROM financial_transactions WHERE account_number = '" & AccountNumber & "' AND transaction_type = 'PAY' ORDER BY sale_date desc limit 1) s2 ON True;"

        'SELECT * FROM 
        '(SELECT sale_date as sale_date, transaction_amount AS sale_amount FROM financial_transactions 
        'WHERE account_number = '100126232' 
        'AND transaction_type='SALE' ORDER BY sale_date desc limit 1) AS s1
        'full join
        '(SELECT sale_date as pay_date, transaction_amount AS pay_amount FROM financial_transactions 
        'WHERE account_number = '100126232' 
        'AND transaction_type='PAY' ORDER BY sale_date desc limit 1) s2 on (true);

        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _LastTransactions.LastSaleAmount = dr("sale_amount") & ""
                    If dr("sale_date") & "" <> "" Then
                        LastSaleDate = dr("sale_date") & ""
                        LastSaleDateString = Format(LastSaleDate, "yyyy-MM-dd")
                        _LastTransactions.LastSaleDate = LastSaleDateString
                    Else
                        _LastTransactions.LastSaleDate = ""
                    End If

                    _LastTransactions.LastPaymentAmount = dr("pay_amount") & ""
                    If dr("pay_date") & "" <> "" Then
                        LastPaymentDate = dr("pay_date") & ""
                        LastPaymentDateString = Format(LastPaymentDate, "yyyy-MM-dd")
                        _LastTransactions.LastPaymentDate = LastPaymentDateString
                    Else
                        _LastTransactions.LastPaymentDate = ""
                    End If
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return _LastTransactions

    End Function

    Public Function SetNextContactDateTime(ByVal AccountNumber As String, ByVal CurrentContactLevel As String,
                                          Optional ByVal NextContactDateTime As String = "",
                                          Optional ByVal NextPTPDate As String = "") As Long

        Dim RecordsReturned As Long = 0
        Dim nextcontactdate As Date
        Dim nextcontactdatestring As String = "2001-01-01 12:00:00 AM"

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        If NextContactDateTime <> "" Then
            nextcontactdate = NextContactDateTime
            nextcontactdatestring = Format(nextcontactdate, "yyyy-MM-dd hh:mm:ss tt")
        End If

        If NextContactDateTime = "" Then
            tmpSQL = "UPDATE financial_balances SET next_contact_time = '2001-01-01 12:00:00 AM', current_contact_level = " & CurrentContactLevel & " WHERE account_number = '" & AccountNumber & "'"
        Else
            If NextPTPDate = "" Then
                tmpSQL = "UPDATE financial_balances SET next_contact_time = to_timestamp('" & nextcontactdatestring & "','YYYY-MM-DD HH:MI:SS AM')," &
                         "current_contact_level = '" & CurrentContactLevel & "' WHERE account_number = '" & AccountNumber & "'"
            Else
                tmpSQL = "UPDATE financial_balances SET next_contact_time = to_timestamp('" & nextcontactdatestring & "','YYYY-MM-DD HH:MI:SS AM')," &
                         "current_contact_level = '" & CurrentContactLevel & "', next_ptp_date = '" & NextPTPDate & "' WHERE account_number = '" & AccountNumber & "'"
            End If
        End If

        Try
            'RecordsReturned = objDB.ExecuteQuery(tmpSQL)
            RecordsReturned = usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return 0
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return RecordsReturned

    End Function

    Public Function SetFirstContactInvestigationFailed(ByVal AccountNumber As String,
                                                       Optional ByVal FirstContactInvestigationFailed As String = "") As Long

        Dim RecordsReturned As Long = 0

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        If FirstContactInvestigationFailed = "" Then
            FirstContactInvestigationFailed = "2100-01-01"
        End If

        tmpSQL = "UPDATE financial_balances SET first_contact_investigation_failed = '" & FirstContactInvestigationFailed & "' WHERE account_number = '" & AccountNumber & "'"

        Try
            'RecordsReturned = objDB.ExecuteQuery(tmpSQL)
            RecordsReturned = usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return 0
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return RecordsReturned

    End Function

    Public Function UpdateNextTimeToCall(ByVal AccountNumber As String, ByVal NextContactTime As String) As Long

        If IsNothing(AccountNumber) Then
            Return 0
        End If

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        Dim RecordsReturned As Long = 0

        'Time in 2 hours
        Dim nextcontactdate As Date = Format(Now, "yyyy-MM-dd hh:mm:ss tt")

        nextcontactdate = nextcontactdate.AddHours(2)

        Dim nextcontactdatestring As String = Format(nextcontactdate, "yyyy-MM-dd hh:mm:ss tt")


        'Current next time to contact
        Dim currentcontactdate As Date = CDate(NextContactTime)

        Dim currentcontactdatestring As String = Format(currentcontactdate, "yyyy-MM-dd hh:mm:ss tt")


        If NextContactTime = "" Then
            tmpSQL = "UPDATE financial_balances SET next_contact_time = to_timestamp('" & nextcontactdatestring & "','YYYY-MM-DD HH:MI:SS AM') " &
                            "WHERE account_number = '" & AccountNumber & "' AND next_contact_time = '2001-01-01'"
        Else
            tmpSQL = "UPDATE financial_balances SET next_contact_time = to_timestamp('" & nextcontactdatestring & "','YYYY-MM-DD HH:MI:SS AM') " &
                             "WHERE account_number = '" & AccountNumber & "' AND next_contact_time = to_timestamp('" & currentcontactdatestring & "','YYYY-MM-DD HH:MI:SS AM')"
        End If



        Try
            'RecordsReturned = Val(objDB.ExecuteQuery(tmpSQL))
            RecordsReturned = Val(usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL))

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return 0
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return RecordsReturned

    End Function

    Private Function WRequest(URL As String, method As String, POSTdata As String) As String
        Dim responseData As String = ""
        Try
            Dim hwrequest As Net.HttpWebRequest = Net.WebRequest.Create(URL)
            hwrequest.Accept = "*/*"
            hwrequest.AllowAutoRedirect = True
            hwrequest.UserAgent = "http_requester/0.1"
            hwrequest.Timeout = 60000

            hwrequest.Method = method
            If hwrequest.Method = "POST" Then
                hwrequest.ContentType = "application/x-www-form-urlencoded"
                Dim encoding As New ASCIIEncoding() 'Use UTF8Encoding for XML requests
                Dim postByteArray() As Byte = encoding.GetBytes(POSTdata)
                hwrequest.ContentLength = postByteArray.Length
                Dim postStream As IO.Stream = hwrequest.GetRequestStream()
                postStream.Write(postByteArray, 0, postByteArray.Length)
                postStream.Close()
            End If
            Dim hwresponse As Net.HttpWebResponse = hwrequest.GetResponse()
            If hwresponse.StatusCode = Net.HttpStatusCode.OK Then
                Dim responseStream As IO.StreamReader =
                  New IO.StreamReader(hwresponse.GetResponseStream())
                responseData = responseStream.ReadToEnd()
            End If
            hwresponse.Close()
        Catch e As Exception
            responseData = "An error occurred: " & e.Message
        End Try
        Return responseData
    End Function

    Public Function SendSMSMessage(ByVal SendSMSToNumber As String, ByVal tmpMessage As String) As String

        Dim URL As String

        Try
            URL = "http://bli.panaceamobile.com/json?action=message_send&username=rage&password=4bnlwk0wjg94fj9r3ftea48btit932m_gvfo9qu78gu5ihjl"

            '4bnlwk0wjg94fj9r3ftea48btit932m_gvfo9qu78gu5ihjl
            Dim NumberToSend As String = SendSMSToNumber.Replace("-", "")
            NumberToSend = "27" & Mid$(NumberToSend, 2)

            URL &= "&to=" & Trim(NumberToSend)
            URL &= "&text=" & tmpMessage
            URL &= "&from=Rage"

            Return WRequest(URL, "GET", "")

        Catch ex As Exception
            Return ex.Message
        End Try

        Return ""

    End Function

    Public Function SendSMS(ByVal AccountNumber As String, ByVal NumberToSendTo As String,
                            ByVal Message As String, ByVal user_name As String,
                            Optional ByVal MessageType As String = "") As String

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        If MessageType = "" Then
            MessageType = "Collections Call"
        End If

        tmpSQL = "INSERT INTO sms_sending (guid,account_number,sms_date,sms_time,cellphone_number,sms_message,sms_reply,type_of_sms,user_name) " &
                 "VALUES ('" & Guid.NewGuid.ToString & "','" & AccountNumber & "','" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" &
                  NumberToSendTo & "','" & Message & "'," &
                 "'" & SendSMSMessage(NumberToSendTo, Message) & "','" & MessageType & "','" & user_name & "')"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function


    Public Function UpdateDebtorSMSStatus(ByVal AccountNumber As String, ByVal DoNotSendSMS As Boolean) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "UPDATE debtor_personal SET dont_send_sms = '" & DoNotSendSMS & "' WHERE account_number = '" & AccountNumber & "'"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function

    Public Function UpdateDebtorPreferredLanguage(ByVal AccountNumber As String, ByVal Language As String) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "UPDATE debtor_personal SET preferred_language = '" & Language & "' WHERE account_number = '" & AccountNumber & "'"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function

    Public Function UpdateDebtorContactInvestigation(ByVal AccountNumber As String, ByVal bStatus As Boolean) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "UPDATE debtor_personal SET contact_investigation = " & bStatus & " WHERE account_number = '" & AccountNumber & "'"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function

    Public Function UpdateDebtorContactNumberStatus(ByVal NewLog As ContactInvestigationResult) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        Dim tmpSQL As String
        tmpSQL = "UPDATE debtor_personal SET "

        If Trim(NewLog.AltNumber) <> Trim(NewLog.OriginalAltNumber) Then
            tmpSQL &= "fax_number = '" & NewLog.AltNumber & "' "
        End If

        If Trim(NewLog.ContactNumber) <> Trim(NewLog.OriginalContactNumber) Then
            If tmpSQL <> "UPDATE debtor_personal SET " Then 'Check whether the query is updating multiple fields
                tmpSQL &= ","
            End If
            tmpSQL &= "cell_number = '" & NewLog.ContactNumber & "' "
        End If

        If Trim(NewLog.HomeNumber1) <> Trim(NewLog.OriginalHomeNumber1) Then
            If tmpSQL <> "UPDATE debtor_personal SET " Then
                tmpSQL &= ","
            End If
            tmpSQL &= "home_telephone = '" & NewLog.HomeNumber1 & "' "
        End If

        If Trim(NewLog.HomeNumber2) <> Trim(NewLog.OriginalHomeNumber2) Then
            If tmpSQL <> "UPDATE debtor_personal SET " Then
                tmpSQL &= ","
            End If
            tmpSQL &= "home_telephone_2 = '" & NewLog.HomeNumber2 & "' "
        End If

        If Trim(NewLog.NextOfKin) <> Trim(NewLog.OriginalNextOfKin) Then
            If tmpSQL <> "UPDATE debtor_personal SET " Then
                tmpSQL &= ","
            End If
            tmpSQL &= "next_of_kin = '" & NewLog.NextOfKin & "' "
        End If

        If Trim(NewLog.NextOfKinNumber) <> Trim(NewLog.OriginalNextOfKinNumber) Then
            If tmpSQL <> "UPDATE debtor_personal SET " Then
                tmpSQL &= ","
            End If
            tmpSQL &= "next_of_kin_contact_number = '" & NewLog.NextOfKinNumber & "' "
        End If

        If Trim(NewLog.SpouseContactNumber) <> Trim(NewLog.OriginalSpouseContactNumber) Then
            If tmpSQL <> "UPDATE debtor_personal SET " Then
                tmpSQL &= ","
            End If
            tmpSQL &= "spouse_cell_number = '" & NewLog.SpouseContactNumber & "' "
        End If

        If Trim(NewLog.SendPromos) <> Trim(NewLog.OriginalSendPromos) Then
            If tmpSQL <> "UPDATE debtor_personal SET " Then
                tmpSQL &= ","
            End If
            tmpSQL &= "send_promos = '" & NewLog.SendPromos & "' "
        End If

        If tmpSQL = "UPDATE debtor_personal SET " Then 'Check if the query needs to be run
            Return False
        End If

        tmpSQL &= "WHERE account_number = '" & NewLog.AccountNumber & "'"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        'Update the work number which is in a different table
        If Trim(NewLog.WorkNumber) <> Trim(NewLog.OriginalWorkNumber) Then
            tmpSQL = "UPDATE debtor_employment SET employer_telephone_1 = '" & NewLog.WorkNumber & "' WHERE account_number = '" & NewLog.AccountNumber & "'"
            Try
                'objDB.ExecuteQuery(tmpSQL)
                usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

            Catch ex As Exception
                'If (objDB IsNot Nothing) Then
                '    objDB.CloseConnection()
                'End If
                Return False
            Finally
                'If (objDB IsNot Nothing) Then
                '    objDB.CloseConnection()
                'End If
            End Try
        End If

        Return True

    End Function

    Public Function UpdateDebtorUnderInvestigation(ByVal AccountNumber As String, ByVal UnderInvestigation As Boolean) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "UPDATE debtor_personal SET under_investigation = '" & UnderInvestigation & "' WHERE account_number = '" & AccountNumber & "'"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function

    Public Function UpdateDebtorLegalStatus(ByVal AccountNumber As String, ByVal IsLegal As Boolean) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "UPDATE debtor_personal SET is_legal = '" & IsLegal & "' WHERE account_number = '" & AccountNumber & "'"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        If IsLegal = True Then
            tmpSQL = "UPDATE debtor_personal SET status = 'LEGAL' WHERE account_number = '" & AccountNumber & "'"
        Else
            tmpSQL = "UPDATE debtor_personal SET status = 'ACTIVE' WHERE account_number = '" & AccountNumber & "'"
        End If

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function

    Public Function UpdateDebtorFraudStatus(ByVal AccountNumber As String, ByVal IsFraud As Boolean) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "UPDATE debtor_personal SET is_legal = 'False' WHERE account_number = '" & AccountNumber & "'"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        If IsFraud = True Then
            tmpSQL = "UPDATE debtor_personal SET status = 'FRAUD' WHERE account_number = '" & AccountNumber & "'"
        Else
            tmpSQL = "UPDATE debtor_personal SET status = 'ACTIVE' WHERE account_number = '" & AccountNumber & "'"
        End If

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function

    Public Function UpdateDebtorDebtReviewStatus(ByVal AccountNumber As String) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "UPDATE debtor_personal SET status = 'DEBT REVIEW' WHERE account_number = '" & AccountNumber & "'"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function

    Public Function AddToDebtorChangeLog(ByVal UserName As String, ByVal AccountNumber As String,
                                         ByVal ChangeDescription As String, ByVal OldData As String, ByVal NewData As String) As Boolean

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " &
            "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & UserName & "','" & AccountNumber & "','" & ChangeDescription & "'," &
            "'" & OldData & "','" & NewData & "')"

        Try
            'objDB.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_PCMWriteConnectionString, tmpSQL)

        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return False
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return True

    End Function

    Public Function GetTransactions(ByVal AccountNumber As String) As List(Of Transactions)

        Dim _ListOfTransactions As New List(Of Transactions)

        Dim runningBalance As Double = 0

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date,sale_time,current_period,username,reference_number,transaction_type,transaction_amount " &
                 "FROM financial_transactions " &
                 "WHERE account_number = '" & AccountNumber & "' ORDER BY financial_transactions_id"

        Try
            'ds = objDB.GetDataSet(tmpSQL)
            ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)
            If usingObjDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _Transaction As New Transactions

                    If dr("transaction_type").ToString() <> "LEDDN" Then
                        If dr("transaction_type").ToString() <> "LEDCN" Then
                            Select Case dr("transaction_type").ToString()
                                Case "SALE"
                                    runningBalance = runningBalance + Convert.ToDouble(dr("transaction_amount"))
                                Case "INT"
                                    runningBalance = runningBalance + Convert.ToDouble(dr("transaction_amount"))
                                Case "LCP"
                                    runningBalance = runningBalance + Convert.ToDouble(dr("transaction_amount"))
                                Case "PAY"
                                    runningBalance = runningBalance + Convert.ToDouble(dr("transaction_amount"))
                                Case "CN"
                                    runningBalance = runningBalance + Convert.ToDouble(dr("transaction_amount"))

                                'The positive / negative records for these 2 have been inserted the wrong way round
                                Case "LEDD"
                                    If Val(dr("transaction_amount")) > 0 Then
                                        runningBalance = runningBalance + Convert.ToDouble(dr("transaction_amount"))
                                    Else
                                        runningBalance = runningBalance - Convert.ToDouble(dr("transaction_amount"))
                                    End If

                                Case "LEDC"
                                    If Val(dr("transaction_amount")) < 0 Then
                                        runningBalance = runningBalance + Convert.ToDouble(dr("transaction_amount"))
                                    Else
                                        runningBalance = runningBalance - Convert.ToDouble(dr("transaction_amount"))
                                    End If

                            End Select


                        End If
                    End If

                    _Transaction.tDate = dr("sale_date") & ""
                    _Transaction.tTime = dr("sale_time") & ""
                    _Transaction.tUser = dr("username") & ""
                    _Transaction.tReference = dr("reference_number") & ""
                    _Transaction.tType = dr("transaction_type") & ""
                    _Transaction.tAmount = Format(Val(dr("transaction_amount") & ""), "0.00")
                    _Transaction.tPeriod = dr("current_period") & ""
                    _Transaction.tRunningBalance = Format(Val(runningBalance & ""), "0.00")

                    _ListOfTransactions.Add(_Transaction)
                Next
            Else
                Return Nothing
            End If
        Catch ex As Exception
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDB IsNot Nothing) Then
            '    objDB.CloseConnection()
            'End If
        End Try

        Return _ListOfTransactions

    End Function
End Class
