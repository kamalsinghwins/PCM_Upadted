Imports Npgsql
Imports Entities
Imports NpgsqlTypes

Public Class AccountsDL

    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil
    Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")
    Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")
    Dim checkCardResponse As New CheckCardResponse
    Dim processTransactionResponse As New ProcessTransactionResponse

    'Public Function ProcessMultipleTransaction(ByVal ProcessTransactionRequest As ProcessTransactionRequest) As ProcessTransactionResponse
    '    Dim objDB As New dlNpgSQL("PostgreConnectionStringPositiveRead")


    '    'If ProcessTransactionRequest.PaymentType = "" Then

    '    '    processTransactionResponse.Message = "Please select a type of payment."
    '    '    processTransactionResponse.Success = False
    '    '    Return processTransactionResponse
    '    'End If

    '    'tmpSQL = "SELECT nextval('enum_manual_purchase_seq')"
    '    'NEX 1009
    '    '    If isR(1009) Then
    '    '    strTransactionNumber = rS(1009).Fields("nextval") & ""
    '    'End If
    '    'NCL 1009

    '    Dim _Returned As New PCMAccount

    '    Dim pcmAccount As New PCMAccount

    '    If ProcessTransactionRequest.TransactionType = "CN" Or ProcessTransactionRequest.TransactionType = "PAY" Then
    '        ProcessTransactionRequest.Amount = "-" & ProcessTransactionRequest.Amount
    '    End If

    '    pcmAccount.CardNumber = ProcessTransactionRequest.CardNumber
    '    pcmAccount.TransactionAmount = ProcessTransactionRequest.Amount
    '    pcmAccount.TransactionType = ProcessTransactionRequest.TransactionType
    '    pcmAccount.UserName = ProcessTransactionRequest.ShopAssistant
    '    pcmAccount.PayType = ProcessTransactionRequest.PaymentType
    '    pcmAccount.TransactionNumber = ProcessTransactionRequest.ShopCode
    '    pcmAccount.GuID = Guid.NewGuid.ToString 'Generate a random GuID to post the transaction
    '    'pcmAccount.CurrentAccountPeriod = CurrentAccountPeriod
    '    pcmAccount.skip_condition = True

    '    'First Check for GiftCard
    '    If Mid$(ProcessTransactionRequest.CardNumber, 1, 4) = "6501" Then 'Account Card
    '        _Returned = ProcessAccountCard(pcmAccount)
    '    ElseIf Mid$(ProcessTransactionRequest.CardNumber, 1, 4) = "6502" Then
    '        'Gift Card
    '        _Returned = ProcessGiftCard(pcmAccount)
    '    End If

    '    If _Returned.ReturnMessage = "PASSED" Then
    '        Dim AuthCode As String
    '        AuthCode = GenerateAuthCode(ProcessTransactionRequest.CardNumber, ProcessTransactionRequest.Amount,
    '                                    ProcessTransactionRequest.ShopCode, ProcessTransactionRequest.TransactionType)
    '        processTransactionResponse.ProcessTransaction = AuthCode
    '        processTransactionResponse.Success = True
    '    Else
    '        processTransactionResponse.Message = _Returned.ReturnMessage
    '        processTransactionResponse.Success = False
    '    End If

    '    Return processTransactionResponse

    'End Function

    Public Function GetRandomInt(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Public Function GenerateAuthCode(ByVal CardNumber As String,
                                    ByVal TransactionAmount As String, ByVal BranchCode As String,
                                    ByVal TransactionType As String, ByVal RequiresDeposit As Boolean) As String

        Dim CardSeq As Integer
        Dim CardCheck As String
        Dim nnLoop As Integer
        Dim TransactionSeq As Integer
        Dim TransactionCheck As String
        Dim TransacionTypeCheck As String
        Dim BranchCodeSeq As Integer
        Dim BranchCodeCheck As String
        Dim RandomCode As String

StartAgain:

        CardSeq = 0

        For nnLoop = 1 To 16
            CardSeq = CardSeq + Mid$(CardNumber, nnLoop, 1)
        Next nnLoop

        CardCheck = CardSeq * 3

        CardCheck = Mid$(CardCheck, Len(CardCheck), 1)

        If Mid$(TransactionAmount, 1, 1) = "-" Then
            TransactionAmount = Val(TransactionAmount) * -1
        End If

        TransactionAmount = Replace(TransactionAmount, "R", "")
        TransactionAmount = Replace(TransactionAmount, "r", "")

        TransactionAmount = Replace(TransactionAmount, ".", "0")

        For nnLoop = 1 To Len(TransactionAmount)
            TransactionSeq = TransactionSeq + Mid$(TransactionAmount, nnLoop, 1)
        Next nnLoop

        TransactionCheck = TransactionSeq * 3

        TransactionCheck = Mid$(TransactionCheck, Len(TransactionCheck), 1)

        Select Case TransactionType
            Case "SALE"
                TransacionTypeCheck = "1"
            Case "GCS"
                TransacionTypeCheck = "2"
            Case "CN"
                TransacionTypeCheck = "3"
            Case "PAY"
                TransacionTypeCheck = "4"
            Case "GCC"
                TransacionTypeCheck = "5"
            Case "GCP"
                TransacionTypeCheck = "6"
        End Select

        For nnLoop = 1 To Len(BranchCode)
            Select Case Mid$(BranchCode, nnLoop, 1)
                Case "A"
                    BranchCodeSeq = BranchCodeSeq + 5
                Case "B"
                    BranchCodeSeq = BranchCodeSeq + 6
                Case "C"
                    BranchCodeSeq = BranchCodeSeq + 7
                Case "D"
                    BranchCodeSeq = BranchCodeSeq + 8
                Case "E"
                    BranchCodeSeq = BranchCodeSeq + 9
                Case "F"
                    BranchCodeSeq = BranchCodeSeq + 10
                Case "G"
                    BranchCodeSeq = BranchCodeSeq + 11
                Case "H"
                    BranchCodeSeq = BranchCodeSeq + 12
                Case "I"
                    BranchCodeSeq = BranchCodeSeq + 13
                Case "J"
                    BranchCodeSeq = BranchCodeSeq + 14
                Case "K"
                    BranchCodeSeq = BranchCodeSeq + 15
                Case "L"
                    BranchCodeSeq = BranchCodeSeq + 16
                Case "M"
                    BranchCodeSeq = BranchCodeSeq + 17
                Case "N"
                    BranchCodeSeq = BranchCodeSeq + 18
                Case "O"
                    BranchCodeSeq = BranchCodeSeq + 19
                Case "P"
                    BranchCodeSeq = BranchCodeSeq + 20
                Case "Q"
                    BranchCodeSeq = BranchCodeSeq + 21
                Case "R"
                    BranchCodeSeq = BranchCodeSeq + 22
                Case "S"
                    BranchCodeSeq = BranchCodeSeq + 23
                Case "T"
                    BranchCodeSeq = BranchCodeSeq + 24
                Case "U"
                    BranchCodeSeq = BranchCodeSeq + 25
                Case "V"
                    BranchCodeSeq = BranchCodeSeq + 26
                Case "W"
                    BranchCodeSeq = BranchCodeSeq + 1
                Case "X"
                    BranchCodeSeq = BranchCodeSeq + 2
                Case "Y"
                    BranchCodeSeq = BranchCodeSeq + 3
                Case "Z"
                    BranchCodeSeq = BranchCodeSeq + 4
                Case "0"
                    BranchCodeSeq = BranchCodeSeq + 0
                Case "1"
                    BranchCodeSeq = BranchCodeSeq + 1
                Case "2"
                    BranchCodeSeq = BranchCodeSeq + 2
                Case "3"
                    BranchCodeSeq = BranchCodeSeq + 3
                Case "4"
                    BranchCodeSeq = BranchCodeSeq + 4
                Case "5"
                    BranchCodeSeq = BranchCodeSeq + 5
                Case "6"
                    BranchCodeSeq = BranchCodeSeq + 6
                Case "7"
                    BranchCodeSeq = BranchCodeSeq + 7
                Case "8"
                    BranchCodeSeq = BranchCodeSeq + 8
                Case "9"
                    BranchCodeSeq = BranchCodeSeq + 9

            End Select
        Next nnLoop

        BranchCodeCheck = BranchCodeSeq * 5

        BranchCodeCheck = Mid$(BranchCodeCheck, Len(BranchCodeCheck), 1)

        If RequiresDeposit = True Then
            RandomCode = "7" & GetRandomInt(1, 9) & "1" & GetRandomInt(1, 9) & "8"

            'RandomCode = Format(RandomCode, "00000")
        Else
            RandomCode = GetRandomInt(1000, 9999) & GetRandomInt(1, 7)

            'RandomCode = Format(RandomCode, "00000")
        End If


        '1: Check digit
        '2: Card Number check
        '3: Transaction Amount check
        '4: Branch Code check
        '5: Trancaction Type check
        '6 - 10: Random

        Dim WithoutCheck As String

        WithoutCheck = CardCheck & TransactionCheck & BranchCodeCheck & TransacionTypeCheck & RandomCode

        Dim CheckDigits(9) As Integer

        CheckDigits(1) = Mid$(WithoutCheck, 1, 1)
        CheckDigits(2) = Mid$(WithoutCheck, 2, 1)
        CheckDigits(3) = Mid$(WithoutCheck, 3, 1)
        CheckDigits(4) = Mid$(WithoutCheck, 4, 1)
        CheckDigits(5) = Mid$(WithoutCheck, 5, 1)
        CheckDigits(6) = Mid$(WithoutCheck, 6, 1)
        CheckDigits(7) = Mid$(WithoutCheck, 7, 1)
        CheckDigits(8) = Mid$(WithoutCheck, 8, 1)
        CheckDigits(9) = Mid$(WithoutCheck, 9, 1)

        Dim Odds As Integer
        Dim Evens As Integer
        Dim FinalCheck As String
        Dim WithCheckDigit As String


        Odds = CheckDigits(1) + CheckDigits(3) + CheckDigits(5) + CheckDigits(7) + CheckDigits(9)
        Evens = CheckDigits(2) + CheckDigits(4) + CheckDigits(6) + CheckDigits(8)
        Evens = Evens * 2

        FinalCheck = Odds + Evens

        FinalCheck = Mid$(FinalCheck, Len(FinalCheck), 1)

        WithCheckDigit = FinalCheck & WithoutCheck

        'Debug.Print WithCheckDigit

        tmpSQL = "SELECT auth_code FROM financial_transactions WHERE auth_code = '" & WithCheckDigit & "'"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                'Auth code already exists
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                GoTo StartAgain
            End If
        Catch ex As Exception
            '_SendEmail.WriteToLog(MapPath, "dlManualSale :: GenerateAuthCode", "", "Error: " & ex.Message & " " & tmpSQL)

            '_SendEmail.SendEMail("dlIncomingSMS: IsExistingCustomer", ex.Message & vbCrLf & "SQL: " & tmpSQL)
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return Nothing
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        GenerateAuthCode = WithCheckDigit

        GenerateAuthCode = Mid$(WithCheckDigit, 1, 3) & "-" & Mid$(WithCheckDigit, 4, 3) & "-" &
                           Mid$(WithCheckDigit, 7, 3) & "-" & Mid$(WithCheckDigit, 10, 1)


    End Function

    Public Function CheckCardnumber(ByVal CheckCardRequest As CheckCardRequest) As CheckCardResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")



        If Mid$(CheckCardRequest.CardNumber, 1, 4) = "6501" Then
            tmpSQL = "SELECT debtor_personal.account_number,card_details.card_number,debtor_personal.title,debtor_personal.initials,debtor_personal.last_name," &
                     "debtor_personal.id_number " &
                     "FROM debtor_personal " &
                     "INNER JOIN card_details ON debtor_personal.account_number = card_details.account_number " &
                     "WHERE card_details.card_number = '" & RG.Apos(CheckCardRequest.CardNumber) & "'"
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                checkCardResponse.AccountNumber = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("account_number")
                checkCardResponse.ID = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("id_number")
                checkCardResponse.Customer = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("title") &
                    " " & ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("initials") &
                    " " & ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("last_name")
                checkCardResponse.Success = True
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                checkCardResponse.Message = "There is no Account Assigned to this Card or the Card Number does Not Exist."
                checkCardResponse.Success = False
                Return checkCardResponse
            End If

        ElseIf Mid$(CheckCardRequest.CardNumber, 1, 4) = "6502" Then
            tmpSQL = "SELECT card_number FROM card_gift_cards WHERE card_number = '" & RG.Apos(CheckCardRequest.CardNumber) & "'"
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                checkCardResponse.AccountNumber = CheckCardRequest.CardNumber
                checkCardResponse.Customer = "GIFTCARD"
                checkCardResponse.ID = "GIFTCARD"
                checkCardResponse.Success = True
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                checkCardResponse.Message = "This Card Number does Not Exist."
                checkCardResponse.Success = False
                Return checkCardResponse
            End If
        End If

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return checkCardResponse
    End Function

    Public Function CheckAccountNumber(ByVal CheckCardRequest As CheckCardRequest) As CheckCardResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        Try
            tmpSQL = "SELECT debtor_personal.account_number,card_details.card_number,debtor_personal.title,debtor_personal.initials,debtor_personal.last_name," &
                 "debtor_personal.id_number " &
                 "FROM debtor_personal " &
                 "INNER JOIN card_details ON debtor_personal.account_number = card_details.account_number " &
                 "WHERE debtor_personal.account_number = '" & RG.Apos(CheckCardRequest.AccountNumber) & "'"
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                checkCardResponse.CardNumber = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("card_number")
                checkCardResponse.AccountNumber = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("account_number")
                checkCardResponse.ID = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("id_number")
                checkCardResponse.Customer = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("title") &
                    " " & ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("initials") &
                    " " & ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("last_name")
                checkCardResponse.Success = True
            Else
                checkCardResponse.Message = "There is no Card Assigned to this Account or the Account Number does Not Exist."
                checkCardResponse.Success = False
                Return checkCardResponse
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If

            checkCardResponse.Message = ex.Message
            checkCardResponse.Success = False
            Return checkCardResponse

        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return checkCardResponse

    End Function


    Public Function ProcessGiftCard(ByVal AccountDetails As PCMAccount) As PCMAccount

        Dim _Return As New PCMAccount

        Dim ds As DataSet

        Dim CurrentPeriod As Integer

        'Check if month end is running
        tmpSQL = "SELECT is_shut_down FROM shut_down"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                _Return.ReturnMessage = "System has been shut down for maintenance." & vbCrLf & "This maintenance should not take longer than 15 minutes."
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return _Return
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Return.ReturnMessage = ex.Message
            Return _Return
            'Finally
            '    If (objDBRead IsNot Nothing) Then
            '        objDBRead.CloseConnection()
            '    End If
        End Try

        tmpSQL = "SELECT current_period FROM general_settings"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    CurrentPeriod = Val(dr("current_period") & "")
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return Nothing
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Return.ReturnMessage = ex.Message
            Return _Return
            'Finally
            '    If (objDBRead IsNot Nothing) Then
            '        objDBRead.CloseConnection()
            '    End If
        End Try

        If Val(CurrentPeriod) = 0 Then
            _Return.ReturnMessage = "System Failure. No Current Period Set."
            Return _Return
        End If

        'Check if the GuID has been processed previously
        If AccountDetails.GuID <> "" Then
            tmpSQL = "SELECT guid FROM processed_guids WHERE guid = '" & AccountDetails.GuID & "'"
            Try
                ds = objDBRead.GetDataSet(tmpSQL)
                If objDBRead.isR(ds) Then
                    'If the guid exists it has already been processed
                    If (objDBRead IsNot Nothing) Then
                        objDBRead.CloseConnection()
                    End If

                    _Return.ReturnMessage = "PASSED"
                    Return _Return

                End If
            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                _Return.ReturnMessage = ex.Message
                Return _Return
                'Finally
                '    If (objDBRead IsNot Nothing) Then
                '        objDBRead.CloseConnection()
                '    End If
            End Try
        End If

        tmpSQL = "SELECT " &
                 "card_details.card_number," &
                 "card_details.current_status AS card_status," &
                 "card_gift_cards.balance " &
                 "FROM " &
                 "card_details " &
                 "LEFT OUTER JOIN card_gift_cards ON card_details.card_number = card_gift_cards.card_number " &
                 "WHERE card_details.card_number = '" & AccountDetails.CardNumber & "'"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    _Return.AccountNumber = Mid$(AccountDetails.CardNumber, 1, 12) & "****"
                    _Return.CreditAvailable = RG.Numb(Val(dr("balance") & "") - AccountDetails.TransactionAmount)
                    _Return.CustomerName = "GIFT CARD"

                    Select Case AccountDetails.TransactionType
                        Case "SALE"
                            'Check the card hasn't been blacklisted etc
                            If dr("card_status") & "" <> "ACTIVE" Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "There is an issue with this card."
                                Return _Return
                            End If

                            'Check Credit Limit will not / is not exceeded
                            If Val(dr("balance") & "") - AccountDetails.TransactionAmount < 0 Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "There is not enough Credit on this card to complete the sale."
                                Return _Return
                            End If
                        Case "CN"
                            'Check the card hasn't been blacklisted etc
                            If dr("card_status") & "" <> "ACTIVE" Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "There is an issue with this card."
                                Return _Return
                            End If
                        Case "PAY"
                            'Check the card hasn't been blacklisted etc
                            If dr("card_status") & "" <> "ACTIVE" Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "There is an issue with this card."
                                Return _Return
                            End If

                    End Select
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                _Return.ReturnMessage = "This card does not exist on this system."
                Return _Return
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Return.ReturnMessage = ex.Message
            Return _Return
            'Finally
            '    If (objDBRead IsNot Nothing) Then
            '        objDBRead.CloseConnection()
            '    End If
        End Try

        Select Case AccountDetails.TransactionType
            Case "SALE"
                AccountDetails.TransactionType = "GCS"
                _Return.ReturnMessage = ProcessGiftCardTransaction(CurrentPeriod, AccountDetails)

            Case "CN"
                AccountDetails.TransactionType = "GCC"
                _Return.ReturnMessage = ProcessGiftCardTransaction(CurrentPeriod, AccountDetails)

            Case "PAY"
                AccountDetails.TransactionType = "GCP"
                _Return.ReturnMessage = ProcessGiftCardTransaction(CurrentPeriod, AccountDetails)

        End Select

        If AccountDetails.GuID <> "" Then
            tmpSQL = "INSERT INTO processed_guids (guid) VALUES ('" & AccountDetails.GuID & "')"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                _Return.ReturnMessage = ex.Message

                Return _Return
                'Finally
                '    If (objDBWrite IsNot Nothing) Then
                '        objDBWrite.CloseConnection()
                '    End If
            End Try
        End If

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return _Return

    End Function

    Public Function ProcessAccountCard(ByVal AccountDetails As PCMAccount,
                                       Optional ByVal WasManuel As Boolean = False) As PCMAccount
        Dim _Return As New PCMAccount

        Dim CurrentPeriod As Integer

        'Check if month end is running

        tmpSQL = "SELECT is_shut_down FROM shut_down"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                _Return.ReturnMessage = "System has been shut down for maintenance." & vbCrLf & "This maintenance should not take longer than 15 minutes."
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return _Return
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Return.ReturnMessage = ex.Message
            Return _Return
        End Try

        Dim BirthdayDiscount As Integer

        tmpSQL = "SELECT current_period,birthday_discount FROM general_settings"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    CurrentPeriod = Val(dr("current_period") & "")
                    BirthdayDiscount = Val(dr("birthday_discount") & "")
                Next
            Else
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return Nothing
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Return.ReturnMessage = ex.Message
            Return _Return
        End Try

        If Val(CurrentPeriod) = 0 Then
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Return.ReturnMessage = "System Failure. No Current Period Set."
            Return _Return
        End If

        'Check if the GuID has been processed previously
        'Will have no GuID with online call centre manual transactions
        If AccountDetails.GuID <> "" Then
            tmpSQL = "SELECT guid FROM processed_guids WHERE guid = '" & AccountDetails.GuID & "'"
            Try
                ds = objDBRead.GetDataSet(tmpSQL)
                If objDBRead.isR(ds) Then
                    'If the guid exists it has already been processed
                    If (objDBRead IsNot Nothing) Then
                        objDBRead.CloseConnection()
                    End If

                    _Return.ReturnMessage = "PASSED"
                    Return _Return

                End If
            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                _Return.ReturnMessage = ex.Message
                Return _Return
            End Try
        End If

        'Customer paid using ID Number and not with a card
        If AccountDetails.IDNumber <> "" Then
            tmpSQL = "SELECT card_details.card_number " &
                 "FROM debtor_personal " &
                 "INNER JOIN card_details ON debtor_personal.account_number = card_details.account_number " &
                 "WHERE id_number = '" & AccountDetails.IDNumber & "'"
            Try
                ds = objDBRead.GetDataSet(tmpSQL)
                If objDBRead.isR(ds) Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        AccountDetails.CardNumber = dr("card_number") & ""
                    Next
                End If
            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                _Return.ReturnMessage = ex.Message
                Return _Return
            End Try
        End If

        '2014-07-15
        Dim isFirstPurchase As Boolean = False
        Dim isCallThinFiles As Boolean = False
        Dim DoFirstPurchaseCredit As String = ConfigurationManager.AppSettings("DoFirstPurchaseCredit")
        Dim IDNumber As String
        Dim CellphoneNumber As String

        Dim is150 As Boolean = False
        AccountDetails.isSpecialPayment = False

        tmpSQL = "SELECT " &
                 "card_details.card_number," &
                 "card_details.current_status AS card_status," &
                 "card_details.account_number," &
                 "dp.first_name," &
                 "dp.last_name," &
                 "dp.status AS account_status," &
                 "dp.cell_number AS cellphone," &
                 "dp.itc_rating," &
                 "dp.id_number," &
                 "dp.email_address," &
                 "dp.dont_send_sms," &
                 "dp.show_on_age_analysis," &
                 "debtor_policyfilters.policyfilter3," &
                 "financial_balances.current_balance AS current," &
                 "financial_balances.p30 AS p30," &
                 "financial_balances.p60 AS p60," &
                 "financial_balances.p90 AS p90," &
                 "financial_balances.p120 AS p120," &
                 "financial_balances.p150 AS p150," &
                 "financial_balances.total AS total," &
                 "financial_balances.total_spent AS total_spent," &
                 "financial_balances.credit_limit," &
                 "card_dates.date_assigned,card_dates.date_last_used, " &
                 "dd.date_of_creation, " &
                 "recovery_5000.date_recovered, " &
                 "(SELECT MIN(sale_date) FROM financial_transactions WHERE transaction_type = 'SALE' AND account_number = dp.account_number) as mindate " &
                 "FROM " &
                 "card_details " &
                 "LEFT OUTER JOIN debtor_personal dp ON card_details.account_number = dp.account_number " &
                 "LEFT OUTER JOIN debtor_dates dd ON dd.account_number = dp.account_number " &
                 "LEFT OUTER JOIN financial_balances ON card_details.account_number = financial_balances.account_number " &
                 "LEFT OUTER JOIN card_dates ON card_dates.card_number = card_details.card_number " &
                 "LEFT OUTER JOIN debtor_policyfilters ON dp.account_number = debtor_policyfilters.account_number " &
                 "LEFT OUTER JOIN recovery_5000 ON dp.account_number = recovery_5000.account_number " &
                 "WHERE card_details.card_number = '" & AccountDetails.CardNumber & "'"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    'Check for the account number associated with the card
                    'If there's no account number, we can't even accept a payment
                    If dr("account_number") & "" = "" Then
                        If (objDBRead IsNot Nothing) Then
                            objDBRead.CloseConnection()
                        End If
                        _Return.ReturnMessage = "No Account assigned to this Card."
                        Return _Return
                    End If

                    AccountDetails.AccountNumber = dr("account_number") & ""
                    AccountDetails.Total = Val(dr("total") & "")

                    IDNumber = dr("id_number") & ""
                    CellphoneNumber = dr("cellphone") & ""

                    _Return.AccountNumber = dr("account_number") & ""
                    _Return.CustomerName = dr("first_name") & " " & dr("last_name") & ""

                    '2014-07-15
                    'Give the customer a R50 credit if this is their first purchase, the account was opened within the last 30 days and the transaction is >= R350
                    If DoFirstPurchaseCredit = "True" Then
                        If AccountDetails.TransactionType = "SALE" Then
                            If dr("mindate") & "" = "" Then
                                'First sale
                                Try
                                    If DateDiff("d", dr("date_of_creation") & "", Format(Now, "yyyy-MM-dd")) < 31 Then
                                        If Val(AccountDetails.TransactionAmount) > 349 Then
                                            isFirstPurchase = True
                                            AccountDetails.isFirstPurchase = True
                                        Else
                                            isFirstPurchase = False
                                            AccountDetails.isFirstPurchase = False
                                        End If
                                    End If
                                Catch ex As Exception
                                    If (objDBRead IsNot Nothing) Then
                                        objDBRead.CloseConnection()
                                    End If
                                    _Return.ReturnMessage = "Error on Account Creation Date."
                                    Return _Return
                                End Try
                            End If
                        End If
                    End If

                    'don't let the user do a credit note if there hasn't been a sale
                    If AccountDetails.TransactionType = "CN" Then
                        If Val(dr("total_spent") & "") = 0 Then
                            If (objDBRead IsNot Nothing) Then
                                objDBRead.CloseConnection()
                            End If
                            _Return.ReturnMessage = "You cannot process a credit note on an account before a sale has been done."
                            Return _Return
                        End If
                    End If

                    'We ran a "special" in Dec 2014 that customers in 150 days who paid 
                    'before the 25th would get double the amount credited
                    'Ran again in 01-2016
                    'Ran again in 04-2016
                    'Ran again in 08-2016
                    'Ran again in 11-2016
                    'Dim startDate As Date = "2016-11-21"
                    'Dim xDate As Date = "2016-12-25"

                    'If Val(dr("p150") & "") > 0 Then
                    '    If DateDiff("d", startDate, Format(Now, "yyyy-MM-dd")) >= 0 Then
                    '        If DateDiff("d", xDate, Format(Now, "yyyy-MM-dd")) <= 0 Then
                    '            If AccountDetails.TransactionType = "PAY" Then
                    '                'If dr("show_on_age_analysis") & "" = False Then
                    '                is150 = True
                    '                AccountDetails.isSpecialPayment = True
                    '                'End If
                    '            End If
                    '        End If
                    '    End If
                    'End If

                    Dim nAmount As Double

                    If Val(AccountDetails.TransactionAmount) * -2 > Val(dr("total") & "") Then
                        nAmount = Val(dr("total") & "") * -1
                    Else
                        nAmount = Val(AccountDetails.TransactionAmount) + Val(AccountDetails.TransactionAmount)
                    End If

                    If is150 Then
                        _Return.CreditAvailable = RG.Numb(Val(dr("credit_limit") & "") - Val(nAmount) _
                                                         - Val(dr("total") & ""))

                        _Return.CurrentBalance = RG.Numb(Val(dr("total") & "") + Val(nAmount))

                        'Using this for payments (getting rid of the negative)
                        If RG.Numb(Val(dr("p30") & "") + Val(dr("p60") & "") + Val(dr("p90") & "") _
                                                + Val(dr("p120") & "") + Val(dr("p150") & "") - (Val(AccountDetails.TransactionAmount) * 2)) > 0 Then
                            _Return.AmountDueAfterPayment = RG.Numb(Val(dr("p30") & "") _
                                                            + Val(dr("p60") & "") + Val(dr("p90") & "") _
                                                            + Val(dr("p120") & "") + Val(dr("p150") & "") _
                                                            - (Val(nAmount) * -1))
                        Else
                            _Return.AmountDueAfterPayment = "0"
                        End If

                    Else
                        _Return.CreditAvailable = RG.Numb(Val(dr("credit_limit") & "") - Val(AccountDetails.TransactionAmount) _
                                                         - Val(dr("total") & ""))

                        _Return.CurrentBalance = RG.Numb(Val(dr("total") & "") + Val(AccountDetails.TransactionAmount))

                        'Using this for payments (getting rid of the negative)
                        If RG.Numb(Val(dr("p30") & "") + Val(dr("p60") & "") + Val(dr("p90") & "") _
                                                + Val(dr("p120") & "") + Val(dr("p150") & "") - (Val(AccountDetails.TransactionAmount) * -1)) > 0 Then
                            _Return.AmountDueAfterPayment = RG.Numb(Val(dr("p30") & "") _
                                                            + Val(dr("p60") & "") + Val(dr("p90") & "") _
                                                            + Val(dr("p120") & "") + Val(dr("p150") & "") _
                                                            - (Val(AccountDetails.TransactionAmount) * -1))
                        Else
                            _Return.AmountDueAfterPayment = "0"
                        End If

                    End If

                    _Return.ContactNumber = dr("cellphone") & ""
                    _Return.EMailAddress = dr("email_address") & ""
                    _Return.Total = Val(dr("total") & "")
                    _Return.isDontSendSMS = dr("dont_send_sms") & ""

                    'don't let the user do a credit note if there hasn't been a sale
                    'If AccountDetails.TransactionType = "CN" Then
                    '    If (objDBRead IsNot Nothing) Then
                    '        objDBRead.CloseConnection()
                    '    End If
                    '    _Return.ReturnMessage = "System Error. No Last Transaction with Balance."
                    '    Return _Return
                    'End If


                    Select Case AccountDetails.TransactionType
                        Case "SALE"
                            'Check Card is Active
                            If dr("card_status") & "" <> "ACTIVE" Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "There is a problem with this card."
                                Return _Return
                            End If

                            'Check Account is Active
                            If dr("account_status") & "" <> "ACTIVE" Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "This Account is Not Active."
                                Return _Return
                            End If

                            'Check Credit Limit will not / is not exceeded
                            If Val(dr("total") & "") + Val(AccountDetails.TransactionAmount) > Val(dr("credit_limit") & "") Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "The customer does not have enough Credit to complete this Sale."
                                Return _Return
                            End If

                            'Check there are no overdue amounts
                            If Val(dr("p60") & "") + Val(dr("p90") & "") _
                                + Val(dr("p120") & "") + Val(dr("p150") & "") > 5 Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "Ths customer needs to make a payment before they can purchase."
                                Return _Return
                            End If

                            '2012-01-31
                            'New check for fraud
                            'People who use fake ID books generally spend close to 100% of the credit limit within the first few days of getting the card
                            If dr("date_assigned") & "" = "" Then
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "There is an issue with the Assigned Date on this Card."
                                Return _Return
                            End If

                            Dim DateCardAssigned As Date
                            DateCardAssigned = dr("date_assigned")

                            Dim strDateCardAssigned As String
                            strDateCardAssigned = Format(DateCardAssigned, "yyyy-MM-dd")


                            Try
                                'Updated to 31 days from 4 days :: 2014-03-17
                                If DateDiff("d", strDateCardAssigned, Format(Now, "yyyy-MM-dd")) < 31 Then
                                    'Check that they're not trying to take the limit within the last 10% of the limit
                                    If Val(dr("credit_limit") & "") > 1400 Then
                                        If (Val(dr("total") & "") + Val(AccountDetails.TransactionAmount)) _
                                            / Val(dr("credit_limit") & "") > 0.88 Then
                                            'Mark the account as Suspended
                                            tmpSQL = "UPDATE debtor_personal SET status = 'SUSPENDED' WHERE account_number = '" & _Return.AccountNumber & "'"
                                            Try
                                                objDBWrite.ExecuteQuery(tmpSQL)
                                            Catch ex As Exception
                                                If (objDBWrite IsNot Nothing) Then
                                                    objDBWrite.CloseConnection()
                                                End If
                                                _Return.ReturnMessage = ex.Message
                                                Return _Return
                                            End Try

                                            tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " &
                                                      "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','WEB','" & _Return.AccountNumber & "','ERROR 577 - ACCOUNT SUSPENDED'," &
                                                      "'ACTIVE','SUSPENDED')"
                                            Try
                                                objDBWrite.ExecuteQuery(tmpSQL)
                                            Catch ex As Exception
                                                If (objDBWrite IsNot Nothing) Then
                                                    objDBWrite.CloseConnection()
                                                End If
                                                _Return.ReturnMessage = ex.Message
                                                Return _Return
                                            End Try

                                            'Close read
                                            If (objDBRead IsNot Nothing) Then
                                                objDBRead.CloseConnection()
                                            End If
                                            _Return.ReturnMessage = "Security Error. Please contact the Accounts Department."
                                            Return _Return

                                        End If
                                    End If
                                End If
                            Catch ex As Exception
                                If (objDBRead IsNot Nothing) Then
                                    objDBRead.CloseConnection()
                                End If
                                _Return.ReturnMessage = "General System Error On Security Check."
                                Return _Return
                            End Try


                            '2014-08-04
                            'On the first purchase from a bad debt store, the customer has to pay a 1/6 "deposit"
                            'Check if this transaction comes from a bad debt store

                            '2014-12-22
                            'From 2014-12-26 all debtors with a score of < 601 and/or IDV3 will need to pay a 1/6 deposit
                            'for a period of 3 months (from date_of_creation)
                            If AccountDetails.SpecialCondition = "" Then 'Special condition <> "" means there was already a payment
                                If Val(dr("total_spent") & "") = 0 Then
                                    'If they've never transacted before
                                    '2015-04-23
                                    'All customers now need to pay the deposit

                                    _Return.ReturnMessage = "Require Payment"
                                    'Return _Return 'Returns later one
                                Else
                                    'If their first transaction was in the past 30 days
                                    'Recovered accounts
                                    Try
                                        If DateDiff("d", dr("date_recovered") & "", Format(Now, "yyyy-MM-dd")) < 30 Then
                                            '2015-04-23
                                            'All customers now need to pay the deposit

                                            _Return.ReturnMessage = "Require Payment"
                                            'Return _Return 'Returns later one
                                        End If
                                    Catch ex As Exception
                                        'No error. Accounts not recovered will have no date_recovered

                                    End Try

                                    Try
                                        If DateDiff("d", dr("mindate") & "", Format(Now, "yyyy-MM-dd")) < 30 Then
                                            '2015-04-23
                                            'All customers now need to pay the deposit

                                            _Return.ReturnMessage = "Require Payment"
                                            'Return _Return 'Returns later one
                                        End If
                                    Catch ex As Exception
                                        If (objDBRead IsNot Nothing) Then
                                            objDBRead.CloseConnection()
                                        End If
                                        _Return.ReturnMessage = "System Error. No Last Transaction with Balance."
                                        Return _Return
                                    End Try

                                End If
                            End If

                            If Format(Now, "dd") < 12 Then
                                _Return.CurrentPayment = RG.Numb(Val(AccountDetails.TransactionAmount) / 6 + Val(dr("current") & "") _
                                                     + Val(dr("p30") & "") + Val(dr("p60") & "") _
                                                     + Val(dr("p90") & "") + Val(dr("p120") & "") _
                                                     + Val(dr("p150") & ""))

                            Else
                                _Return.CurrentPayment = RG.Numb(Val(dr("p30") & "") + Val(dr("p60") & "") _
                                                     + Val(dr("p90") & "") + Val(dr("p120") & "") _
                                                     + Val(dr("p150") & ""))
                            End If
                        Case "CN"
                            'Always accept a CN

                            If Format(Now, "dd") < 12 Then
                                _Return.CurrentPayment = RG.Numb(Val(AccountDetails.TransactionAmount) / 6 + Val(dr("current") & "") _
                                                         + Val(dr("p30") & "") + Val(dr("p60") & "") _
                                                         + Val(dr("p90") & "") + Val(dr("p120") & "") _
                                                         + Val(dr("p150") & ""))
                            Else
                                _Return.CurrentPayment = RG.Numb(Val(dr("p30") & "") + Val(dr("p60") & "") _
                                                         + Val(dr("p90") & "") + Val(dr("p120") & "") _
                                                         + Val(dr("p150") & ""))
                            End If
                        Case "PAY"
                            'Always accept a payment
                            AccountDetails.pCurrent = Val(dr("current") & "")
                            AccountDetails.p30 = Val(dr("p30") & "")
                            AccountDetails.p60 = Val(dr("p60") & "")
                            AccountDetails.p90 = Val(dr("p90") & "")
                            AccountDetails.p120 = Val(dr("p120") & "")
                            AccountDetails.p150 = Val(dr("p150") & "")

                            If Format(Now, "dd") < 12 Then
                                'Payments come through as a negative and therefore need to be taken off the current amount
                                _Return.CurrentPayment = RG.Numb(Val(AccountDetails.TransactionAmount) / 6 + Val(dr("current") & "") _
                                                         + Val(dr("p30") & "") + Val(dr("p60") & "") _
                                                         + Val(dr("p90") & "") + Val(dr("p120") & "") _
                                                         + Val(dr("p150") & ""))
                            Else
                                _Return.CurrentPayment = RG.Numb(Val(dr("p30") & "") + Val(dr("p60") & "") _
                                                         + Val(dr("p90") & "") + Val(dr("p120") & "") _
                                                         + Val(dr("p150") & ""))
                            End If
                    End Select

                Next
            Else

                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If

                '2014-06-26
                'Put this email sending in to see when a store puts through a manual payment with an incorrect card number
                If WasManuel = True Then

                    Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
                    Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

                    Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "WebService Query")
                    Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

                    Msg3.IsBodyHtml = "False"
                    Msg3.Subject = "PCM Website Error : ProcessManualPayment"

                    Msg3.Body = "This card is not a valid Rage card" & vbCrLf & AccountDetails.CardNumber & vbCrLf & AccountDetails.TransactionType & vbCrLf & AccountDetails.TransactionNumber

                    MailObj3.UseDefaultCredentials = False
                    MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
                    'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
                    MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

                    MailObj3.Send(Msg3)

                    'Delete the line at the store to avoid the line being sent continiously
                    _Return.ReturnMessage = "PASSED"
                    Return _Return

                End If

                _Return.ReturnMessage = "This card is not a valid Rage card"
                Return _Return
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            _Return.ReturnMessage = ex.Message
            Return _Return
            'Finally
            '    If (objDBRead IsNot Nothing) Then
            '        objDBRead.CloseConnection()
            '    End If
        End Try

        If _Return.ReturnMessage = "Require Payment" Then
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return _Return
        End If

        If AccountDetails.isOnlineTransaction Then
            'Online call centre manual transaction
            If AccountDetails.SpecialCondition = "Paid" Then
                _Return.AuthCode = GenerateAuthCode(AccountDetails.CardNumber, AccountDetails.TransactionAmount,
                                            AccountDetails.TransactionNumber, AccountDetails.TransactionType, True)
                AccountDetails.AuthCode = _Return.AuthCode
            Else
                _Return.AuthCode = GenerateAuthCode(AccountDetails.CardNumber, AccountDetails.TransactionAmount,
                                            AccountDetails.TransactionNumber, AccountDetails.TransactionType, False)
                AccountDetails.AuthCode = _Return.AuthCode
            End If

            Select Case AccountDetails.TransactionType
                Case "SALE"
                    tmpSQL = "SELECT nextval('enum_manual_purchase_seq') transaction_number"
                Case "CN"
                    tmpSQL = "SELECT nextval('enum_manual_purchase_seq') transaction_number"
                Case "PAY"
                    tmpSQL = "SELECT nextval('enum_manual_purchase_seq') transaction_number"
            End Select



            Try
                Dim dsT As DataSet
                dsT = objDBWrite.GetDataSet(tmpSQL)
                If objDBWrite.isR(dsT) Then
                    'Branch code was sent as the transaction number previously
                    '-CC is to show Call Centre processing
                    AccountDetails.TransactionNumber &= dsT.Tables(0).Rows(0)("transaction_number") & "-CC"
                End If
            Catch ex As Exception
                _Return.ReturnMessage = ex.Message
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return _Return
            End Try

        End If

        If AccountDetails.TransactionType = "PAY" Then
            _Return.ReturnMessage = ProcessPayment(CurrentPeriod, AccountDetails)
        Else
            _Return.ReturnMessage = ProcessTransaction(CurrentPeriod, AccountDetails)
        End If

        '===========================================================================================
        'ALLOCATE BIRTHDAY POINTS
        '===========================================================================================
        If AccountDetails.TransactionType = "SALE" Then
            Dim tmpCurrentYear As String
            'Account for year rollover
            Try
                'First 7 days of the year
                If DateDiff("d", Format(Now, "yyyy") & "-01-01", Format(Now, "yyyy-MM-dd")) < 8 Then
                    If Mid(IDNumber, 3, 2) = "12" Then
                        'Customer was born in December. Use previous year
                        tmpCurrentYear = Val(Format(Now, "yyyy")) - 1
                    Else
                        'Customer was born in January. Use current year
                        tmpCurrentYear = Format(Now, "yyyy")
                    End If
                Else
                    tmpCurrentYear = Format(Now, "yyyy")
                End If
            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                _Return.ReturnMessage = "System Error On Leap Year Calculation."
                Return _Return
            End Try

            'For customers born on 29/02
            Dim tmpDate As String
            If Mid(IDNumber, 3, 4) = "0229" Then
                tmpDate = tmpCurrentYear & "-03-01"
            Else
                tmpDate = tmpCurrentYear & "-" & Mid(IDNumber, 3, 2) & "-" & Mid(IDNumber, 5, 2)
            End If

            Try
                'If the birthday was in the last 7 days, give the birthday credit
                If DateDiff("d", tmpDate, Format(Now, "yyyy-MM-dd")) >= 0 Then
                    If DateDiff("d", tmpDate, Format(Now, "yyyy-MM-dd")) < 8 Then
                        'Is birthday, yay!
                        'Turn amount into negative

                        'Check if the person has already redeemed for this year
                        Dim _IsUsed As String = CheckBirthdayRedemption(AccountDetails.AccountNumber)
                        If _IsUsed = "UNUSED" Then
                            AccountDetails.TransactionAmount = BirthdayDiscount * -1
                            AccountDetails.TransactionType = "LEDC"
                            AccountDetails.TransactionNumber = "bv" & AccountDetails.TransactionNumber

                            'First insert redemption
                            Dim _redemptionresponse As String = InsertBirthdayRedemption(AccountDetails.AccountNumber)

                            If _redemptionresponse = "PASSED" Then
                                _Return.ReturnMessage = ProcessTransaction(CurrentPeriod, AccountDetails)

                                'Send SMS
                                Dim _dlDebtors As New DebtorsDataLayer

                                Dim message As String = "Congratulations! You have redeemed your R" & BirthdayDiscount & " Rage voucher."
                                _dlDebtors.SendSMS(AccountDetails.AccountNumber, CellphoneNumber, message, "WS", "Birthday Voucher Redeemed")
                            Else
                                _Return.ReturnMessage = _redemptionresponse
                            End If
                        End If



                    End If
                End If
            Catch ex As Exception
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                _Return.ReturnMessage = "General System Error."
                Return _Return
            End Try

        End If

        '2018-03-19 bv is added to birthday voucher to indicate what it is
        Dim OriginalTransactionNumber As String
        OriginalTransactionNumber = AccountDetails.TransactionNumber.ToString.Replace("bv", "")

        '2014-08-04
        'On the first purchase from a bad debt store, the customer has to pay a 1/6 "deposit"
        If AccountDetails.SpecialCondition = "Paid" Then

            If AccountDetails.isOnlineTransaction Then
                'Deposit can only be exact
                AccountDetails.SpecialPaymentAmount = RG.Numb(Val(AccountDetails.TransactionAmount) / 6)
            End If
            _Return.CurrentPayment = (Val(AccountDetails.TransactionAmount) / 6) - Val(AccountDetails.SpecialPaymentAmount) / 6

            AccountDetails.TransactionType = "PAY"
            AccountDetails.TransactionAmount = "-" & Val(AccountDetails.SpecialPaymentAmount)
            AccountDetails.TransactionNumber = "xp" & OriginalTransactionNumber
            ProcessSpecialPayment(CurrentPeriod, AccountDetails)

            _Return.CreditAvailable = Val(_Return.CreditAvailable) - Val(AccountDetails.TransactionAmount)

            _Return.CurrentBalance = Val(_Return.CurrentBalance) + Val(AccountDetails.TransactionAmount)

        End If

        '2014-07-15
        'Post a R50 credit note if it's the customer's first purchase

        Dim FirstPurchaseCreditAmount As Double = Val(ConfigurationManager.AppSettings("FirstPurchaseCreditAmount"))

        If AccountDetails.isFirstPurchase = True Then
            AccountDetails.TransactionType = "LEDC"
            AccountDetails.TransactionAmount = FirstPurchaseCreditAmount
            AccountDetails.TransactionNumber = "xx" & OriginalTransactionNumber
            ProcessTransaction(CurrentPeriod, AccountDetails)
        End If

        If AccountDetails.GuID <> "" Then
            tmpSQL = "INSERT INTO processed_guids (guid) VALUES ('" & AccountDetails.GuID & "')"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                _Return.ReturnMessage = ex.Message

                Return _Return
                'Finally
                '    If (objDBWrite IsNot Nothing) Then
                '        objDBWrite.CloseConnection()
                '    End If
            End Try
        End If

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        If AccountDetails.isOnlineTransaction = True Then
            If _Return.ReturnMessage = "PASSED" Then
                _Return.Success = True
            End If
        End If

        Return _Return

    End Function

    Private Function ProcessPayment(ByVal CurrentPeriod As Integer, ByVal tmpAccount As PCMAccount) As String

        '===========================================================================================
        'POST TRANSACTION
        '===========================================================================================
        Dim SevenDaysIntoThePast As String
        SevenDaysIntoThePast = Format(Now.Date.AddDays(-7), "yyyy-MM-dd")

        Dim FortyDaysAgo As String
        FortyDaysAgo = Format(Now.Date.AddDays(-40), "yyyy-MM-dd")

        Dim PTPUser As String = ""

        'If there was a PTP, allocate the payment to the collection agent
        tmpSQL = "SELECT username FROM debtor_contact_history WHERE timestamp_of_contact >= '" & FortyDaysAgo & "' AND ptp_date >= '" & Format(Now, "yyyy-MM-dd") & "' " & _
                 "AND account_number = '" & tmpAccount.AccountNumber & "' ORDER BY ptp_date DESC LIMIT 1"
        ds = objDBRead.GetDataSet(tmpSQL)
        If objDBRead.isR(ds) Then
            For Each dr As DataRow In ds.Tables(0).Rows
                If dr("username") & "" <> "" Then
                    PTPUser = dr("username") & ""
                End If
            Next
        End If

        If PTPUser = "" Then
            tmpSQL = "SELECT username FROM debtor_contact_history WHERE timestamp_of_contact >= '" & FortyDaysAgo & "' AND ptp_date >= '" & SevenDaysIntoThePast & "' " & _
                     "AND account_number = '" & tmpAccount.AccountNumber & "' ORDER BY ptp_date DESC LIMIT 1"
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If dr("username") & "" <> "" Then
                        PTPUser = dr("username") & ""
                    End If
                Next
            End If

        End If

        tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," &
                 "transaction_type,transaction_amount,pay_type,ptp_user,branch_code,auth_code,notes) VALUES " &
                 "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & CurrentPeriod & "'," &
                 "'" & RG.Apos(tmpAccount.UserName) & "','" & tmpAccount.AccountNumber & "','" & tmpAccount.TransactionNumber & "'" &
                 ",'" & tmpAccount.TransactionType & "','" & tmpAccount.TransactionAmount & "','" & tmpAccount.PayType & "'," &
                 "'" & RG.Apos(Mid$(PTPUser, 1, 30)) & "','" & Mid$(tmpAccount.TransactionNumber, 1, 3) & "','" & tmpAccount.AuthCode & "'," &
                 "'" & RG.Apos(tmpAccount.Notes) & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return ex.Message
            'Finally
            '    If (objDBWrite IsNot Nothing) Then
            '        objDBWrite.CloseConnection()
            '    End If
        End Try

        '====================================================================================================================================================================================
        'We ran a "special" in Dec 2014 that customers in 150 days who paid before the 25th would get double the amount credited
        'Again in 01-2016
        'Again in 04-2016
        'Again in 08-2016
        '====================================================================================================================================================================================
        'Dim xDate As Date = "2015-05-11"
        'Dim is150 As Boolean = False

        'If DateDiff("d", xDate, Format(Now, "yyyy-MM-dd")) < 0 Then
        '    If tmpAccount.isSpecialPayment = True Then
        '        is150 = True
        '    End If
        'End If

        Dim startDate As Date = "2016-11-21"
        Dim xDate As Date = "2016-12-25"

        Dim is150 As Boolean = False

        If DateDiff("d", startDate, Format(Now, "yyyy-MM-dd")) >= 0 Then
            If DateDiff("d", xDate, Format(Now, "yyyy-MM-dd")) <= 0 Then
                If tmpAccount.isSpecialPayment = True Then
                    is150 = True
                End If
            End If
        End If

        Dim nAmount As Double

        If Val(tmpAccount.TransactionAmount) * -2 > Val(tmpAccount.Total) Then
            nAmount = (Val(tmpAccount.Total) + Val(tmpAccount.TransactionAmount)) * -1
        Else
            nAmount = Val(tmpAccount.TransactionAmount)
        End If

        If is150 Then
            tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," & _
                 "transaction_type,transaction_amount,pay_type,ptp_user,branch_code) VALUES " & _
                 "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & CurrentPeriod & "'," & _
                 "'" & RG.Apos(tmpAccount.UserName) & "','" & tmpAccount.AccountNumber & "','hs-" & tmpAccount.TransactionNumber & "'" & _
                 ",'LEDC','" & nAmount & "','" & tmpAccount.PayType & "'," & _
                 "'" & RG.Apos(Mid$(PTPUser, 1, 30)) & "','" & Mid$(tmpAccount.TransactionNumber, 1, 3) & "'); "
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return ex.Message
            End Try
        End If
        '====================================================================================================================================================================================

        '===========================================================================================
        'TRANSACTION POSTED
        '===========================================================================================

        '===========================================================================================
        'UPDATE BALANCES
        '===========================================================================================
        Dim RunningAmount As Double
        Dim OriginalTransactionAmount As Double

        RunningAmount = 0

        '===========================================================================================
        'Part of the 150 special
        '===========================================================================================
        If Val(tmpAccount.TransactionAmount) * -2 > Val(tmpAccount.Total) Then
            nAmount = Val(tmpAccount.Total) * -1
        Else
            nAmount = Val(tmpAccount.TransactionAmount) + Val(tmpAccount.TransactionAmount)
        End If
        '===========================================================================================

        If is150 Then
            OriginalTransactionAmount = nAmount
        Else
            OriginalTransactionAmount = Val(tmpAccount.TransactionAmount)
        End If

        'Change the amount from a negative into a positive. (Payments are negative transactions.)
        'Just easier to work with a positive here
        Dim TransactionAmount As Double
        If is150 Then
            TransactionAmount = nAmount * -1
        Else
            TransactionAmount = Val(tmpAccount.TransactionAmount) * -1
        End If

        'p150
        If Val(tmpAccount.p150) <> 0 Then
            If Val(tmpAccount.p150) >= TransactionAmount Then
                tmpAccount.p150 = Val(tmpAccount.p150) - TransactionAmount
            ElseIf Val(tmpAccount.p150) < TransactionAmount Then
                RunningAmount = TransactionAmount - Val(tmpAccount.p150)
                tmpAccount.p150 = 0
            End If
        Else
            RunningAmount = TransactionAmount
        End If

        'p120
        If RunningAmount > 0 Then
            If Val(tmpAccount.p120) <> 0 Then
                If Val(tmpAccount.p120) >= RunningAmount Then
                    tmpAccount.p120 = Val(tmpAccount.p120) - RunningAmount
                    RunningAmount = 0
                ElseIf Val(tmpAccount.p120) < RunningAmount Then
                    RunningAmount = RunningAmount - Val(tmpAccount.p120)
                    tmpAccount.p120 = 0
                End If
            End If
        End If

        'p90
        If RunningAmount > 0 Then
            If Val(tmpAccount.p90) <> 0 Then
                If Val(tmpAccount.p90) >= RunningAmount Then
                    tmpAccount.p90 = Val(tmpAccount.p90) - RunningAmount
                    RunningAmount = 0
                ElseIf Val(tmpAccount.p90) < RunningAmount Then
                    RunningAmount = RunningAmount - Val(tmpAccount.p90)
                    tmpAccount.p90 = 0
                End If
            End If
        End If

        'p60
        If RunningAmount > 0 Then
            If Val(tmpAccount.p60) <> 0 Then
                If Val(tmpAccount.p60) >= RunningAmount Then
                    tmpAccount.p60 = Val(tmpAccount.p60) - RunningAmount
                    RunningAmount = 0
                ElseIf Val(tmpAccount.p60) < RunningAmount Then
                    RunningAmount = RunningAmount - Val(tmpAccount.p60)
                    tmpAccount.p60 = 0
                End If
            End If
        End If

        'p30
        If RunningAmount > 0 Then
            If Val(tmpAccount.p30) <> 0 Then
                If Val(tmpAccount.p30) >= RunningAmount Then
                    tmpAccount.p30 = Val(tmpAccount.p30) - RunningAmount
                    RunningAmount = 0
                ElseIf Val(tmpAccount.p30) < RunningAmount Then
                    RunningAmount = RunningAmount - Val(tmpAccount.p30)
                    tmpAccount.p30 = 0
                End If
            End If
        End If

        'current
        If RunningAmount > 0 Then
            If Val(tmpAccount.pCurrent) <> 0 Then
                If Val(tmpAccount.pCurrent) >= RunningAmount Then
                    tmpAccount.pCurrent = Val(tmpAccount.pCurrent) - RunningAmount
                    RunningAmount = 0
                ElseIf Val(tmpAccount.pCurrent) < RunningAmount Then
                    RunningAmount = RunningAmount - Val(tmpAccount.pCurrent)
                    tmpAccount.pCurrent = 0
                End If
            End If
        End If

        'Put whatever is left in current
        tmpAccount.pCurrent = Val(tmpAccount.pCurrent) - RunningAmount


        '============================================================================================================================================================
        '============================================================================================================================================================
        '2015-05-27
        'Credits have been an issue for some time.
        'The new theory is if a payment takes an account into credit, the 30-150 periods get zeroed, 
        'the current_balance will equal the credit and the payment plans are deleted

        '2012-11-09
        'Updating payments to deal with credits.
        'There were issues with credits in the age analysis
        If Val(tmpAccount.Total) - TransactionAmount <= 0 Then

            'Make current equal to the zero / credit
            tmpAccount.pCurrent = Val(tmpAccount.Total) - TransactionAmount

            tmpAccount.p30 = 0
            tmpAccount.p60 = 0
            tmpAccount.p90 = 0
            tmpAccount.p120 = 0
            tmpAccount.p150 = 0

            'Delete all (upcoming) records from the payment plans
            tmpSQL = "DELETE FROM financial_payment_plans WHERE account_number = '" & tmpAccount.AccountNumber & "'; "
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                If (objDBRead IsNot Nothing) Then
                    objDBRead.CloseConnection()
                End If
                Return ex.Message
            End Try
        End If
        '============================================================================================================================================================
        '============================================================================================================================================================


        tmpSQL = "UPDATE financial_balances SET total = total - " & TransactionAmount & ",current_balance = '" & tmpAccount.pCurrent & "'," & _
                 "p30 = '" & tmpAccount.p30 & "',p60 = '" & tmpAccount.p60 & "',p90 = '" & tmpAccount.p90 & "'," & _
                 "p120 = '" & tmpAccount.p120 & "',p150 = '" & tmpAccount.p150 & "' WHERE account_number = '" & tmpAccount.AccountNumber & "'; "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try

        '===========================================================================================
        'COMPLETED UPDATING BALANCES
        '===========================================================================================

        '===========================================================================================
        'UPDATE DATES
        '===========================================================================================
        'Account Card
        tmpSQL = "UPDATE debtor_dates SET date_of_last_transaction = '" & Format(Now, "yyyy-MM-dd") & "'," & _
                 "date_of_last_payment = '" & Format(Now, "yyyy-MM-dd") & "' WHERE account_number = '" & tmpAccount.AccountNumber & "'; "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return ex.Message
        End Try

        tmpSQL = "UPDATE card_dates SET date_last_used = '" & Format(Now, "yyyy-MM-dd") & "' " & _
                 "WHERE card_number = '" & tmpAccount.CardNumber & "'; "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return ex.Message
        End Try
        '===========================================================================================
        'FINISHED UPDATING DATES
        '===========================================================================================

        'Close the DB connection
        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return "PASSED"

    End Function

    Private Function ProcessGiftCardTransaction(ByVal CurrentPeriod As Integer, ByVal tmpAccount As PCMAccount) As String

        '===========================================================================================
        tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," &
                 "transaction_type,transaction_amount,pay_type,branch_code,notes) VALUES " &
                 "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & CurrentPeriod & "'," &
                 "'" & RG.Apos(tmpAccount.UserName) & "','" & tmpAccount.CardNumber & "','" & tmpAccount.TransactionNumber & "'" &
                 ",'" & tmpAccount.TransactionType & "','" & tmpAccount.TransactionAmount & "','" & tmpAccount.PayType & "'," &
                 "'" & Mid$(tmpAccount.TransactionNumber, 1, 3) & "','" & RG.Apos(tmpAccount.Notes) & "'); "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try

        '===========================================================================================
        'TRANSACTION POSTED
        '===========================================================================================

        '===========================================================================================
        'UPDATE BALANCES
        '===========================================================================================
        'Don't update the total unless the transaction is a sale
        If tmpAccount.TransactionType = "SALE" Then
            tmpSQL = "UPDATE card_gift_cards SET balance = balance - " & tmpAccount.TransactionAmount & "," & _
                "total_spent = total_spent + '" & tmpAccount.TransactionAmount & "' WHERE card_number = '" & tmpAccount.CardNumber & "'; "
        Else
            tmpSQL = "UPDATE card_gift_cards SET balance = balance - " & tmpAccount.TransactionAmount & _
                " WHERE card_number = '" & tmpAccount.CardNumber & "'; "
        End If
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try
        '===========================================================================================
        'COMPLETED UPDATING BALANCES
        '===========================================================================================

        '===========================================================================================
        'UPDATE DATES
        '===========================================================================================
        tmpSQL = "UPDATE card_dates SET date_last_used = '" & Format(Now, "yyyy-MM-dd") & "' WHERE card_number = '" & tmpAccount.CardNumber & "';"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try
        '===========================================================================================
        'FINISHED UPDATING DATES
        '===========================================================================================
        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return "PASSED"

    End Function

    Private Function InsertBirthdayRedemption(ByVal AccountNumber As String) As String

        tmpSQL = "INSERT INTO birthday_voucher_redemption (account_number,year_redeemed) VALUES " &
                 "('" & AccountNumber & "','" & Format(Now, "yyyy") & "'); "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return "PASSED"

    End Function

    Private Function CheckBirthdayRedemption(ByVal AccountNumber As String) As String

        Dim isUsed As String = "UNUSED"

        Try
            tmpSQL = "SELECT * FROM birthday_voucher_redemption WHERE account_number = '" & AccountNumber & "' AND " &
                     "year_redeemed = '" & Format(Now, "yyyy") & "'"
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                Return "USED"
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return ex.Message
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return isUsed

    End Function

    Private Function ProcessTransaction(ByVal CurrentPeriod As Integer, ByVal tmpAccount As PCMAccount) As String

        Dim dblAmountForCurrentBalance As Double

        '===========================================================================================
        'POST TRANSACTION
        '===========================================================================================
        tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," &
                 "transaction_type,transaction_amount,pay_type,branch_code,auth_code,notes) VALUES " &
                 "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & CurrentPeriod & "'," &
                 "'" & RG.Apos(tmpAccount.UserName) & "','" & tmpAccount.AccountNumber & "','" & tmpAccount.TransactionNumber & "'" &
                 ",'" & tmpAccount.TransactionType & "','" & tmpAccount.TransactionAmount & "'," &
                 "'" & tmpAccount.PayType & "','" & Mid$(tmpAccount.TransactionNumber, 1, 3) & "','" & tmpAccount.AuthCode & "'," &
                 "'" & RG.Apos(tmpAccount.Notes) & "'); "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try

        '===========================================================================================
        'TRANSACTION POSTED
        '===========================================================================================

        '===========================================================================================
        'POST PAYMENT PLAN
        '===========================================================================================
        If tmpAccount.TransactionType <> "PAY" Then
            Dim dblPeriod1 As Double
            Dim dblPeriod2 As Double
            Dim dblPeriod3 As Double
            Dim dblPeriod4 As Double
            Dim dblPeriod5 As Double
            Dim dblPeriod6 As Double

            Dim NewTransactionAmount As String

            'The transaction amount can be positive or negative (sales / credit notes)
            'Always easier to times it out then making 2 sets of functions
            Dim wasNegative As Boolean
            If Val(tmpAccount.TransactionAmount) > 0 Then 'Transaction is positive, do nothing
                NewTransactionAmount = tmpAccount.TransactionAmount
                wasNegative = False
            Else
                NewTransactionAmount = Val(tmpAccount.TransactionAmount) * -1
                wasNegative = True
            End If

            'Amount per period is over the minimum. Just divide it through
            'If Val(TransactionAmount) / 6 >= Val(strMinimumPayment) Then
            dblAmountForCurrentBalance = RG.Numb(Val(NewTransactionAmount) / 6)
            dblPeriod1 = RG.Numb(Val(NewTransactionAmount) / 6)
            dblPeriod2 = RG.Numb(Val(NewTransactionAmount) / 6)
            dblPeriod3 = RG.Numb(Val(NewTransactionAmount) / 6)
            dblPeriod4 = RG.Numb(Val(NewTransactionAmount) / 6)
            dblPeriod5 = RG.Numb(Val(NewTransactionAmount) / 6)
            dblPeriod6 = RG.Numb(Val(NewTransactionAmount) / 6)

            If wasNegative = True Then
                dblAmountForCurrentBalance = RG.Numb(dblAmountForCurrentBalance * -1)
                dblPeriod1 = RG.Numb(dblPeriod1 * -1)
                dblPeriod2 = RG.Numb(dblPeriod2 * -1)
                dblPeriod3 = RG.Numb(dblPeriod3 * -1)
                dblPeriod4 = RG.Numb(dblPeriod4 * -1)
                dblPeriod5 = RG.Numb(dblPeriod5 * -1)
                dblPeriod6 = RG.Numb(dblPeriod6 * -1)
                NewTransactionAmount = RG.Numb(Val(NewTransactionAmount) * -1)
            End If

            tmpSQL = "INSERT INTO financial_payment_plans (sale_date,sale_time,account_number,reference_number,total_amount,current_period," &
                     "period_1,amount_1,period_2,amount_2,period_3,amount_3,period_4,amount_4,period_5,amount_5,period_6,amount_6) " &
                     "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & tmpAccount.AccountNumber & "'," &
                     "'" & tmpAccount.TransactionNumber & "','" & NewTransactionAmount & "'," &
                     "'" & CurrentPeriod & "','" & CurrentPeriod & "','" & dblPeriod1 & "','" & CurrentPeriod + 1 & "'," &
                     "'" & dblPeriod2 & "'," &
                     "'" & CurrentPeriod + 2 & "','" & dblPeriod3 & "','" & CurrentPeriod + 3 & "','" & dblPeriod4 & "'," &
                     "'" & CurrentPeriod + 4 & "','" & dblPeriod5 & "','" & CurrentPeriod + 5 & "','" & dblPeriod6 & "'); "
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return ex.Message
            End Try
        End If

        '===========================================================================================
        'FINISHED POSTING PAYMENT PLAN
        '===========================================================================================

        '===========================================================================================
        'UPDATE BALANCES
        '===========================================================================================
        If tmpAccount.TransactionType <> "PAY" Then
            tmpSQL = "UPDATE financial_balances SET total = total + " & tmpAccount.TransactionAmount & "," &
                     "current_balance = current_balance + " & dblAmountForCurrentBalance & "," &
                     "total_spent = total_spent + " & tmpAccount.TransactionAmount &
                     " WHERE account_number = '" & tmpAccount.AccountNumber & "'; "
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return ex.Message
            End Try
        End If
        '===========================================================================================
        'FINISH UPDATING BALANCES
        '===========================================================================================


        '===========================================================================================
        'UPDATE DATES
        '===========================================================================================
        'Account Card
        If tmpAccount.TransactionType = "PAY" Then
            tmpSQL = "UPDATE debtor_dates SET date_of_last_transaction = '" & Format(Now, "yyyy-MM-dd") & "'," &
                "date_of_last_payment = '" & Format(Now, "yyyy-MM-dd") & "' WHERE account_number = '" & tmpAccount.AccountNumber & "'; "
        ElseIf tmpAccount.TransactionType <> "PAY" Then
            tmpSQL = "UPDATE debtor_dates SET date_of_last_transaction = '" & Format(Now, "yyyy-MM-dd") & "' " &
                     "WHERE account_number = '" & tmpAccount.AccountNumber & "'; "
        End If
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try

        tmpSQL = "UPDATE card_dates SET date_last_used = '" & Format(Now, "yyyy-MM-dd") & "' " &
                 "WHERE card_number = '" & tmpAccount.CardNumber & "'; "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try
        '===========================================================================================
        'FINISHED UPDATING DATES
        '===========================================================================================

        tmpSQL = "SELECT account_number FROM debtor_first_purchase WHERE account_number = '" & tmpAccount.AccountNumber & "'"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If Not objDBRead.isR(ds) Then
                tmpSQL = "INSERT INTO debtor_first_purchase (account_number,first_purchase) VALUES ('" & tmpAccount.AccountNumber & "','" & Mid$(tmpAccount.TransactionNumber, 1, 3) & "')"
                Try
                    objDBWrite.ExecuteQuery(tmpSQL)
                Catch ex As Exception
                    If (objDBWrite IsNot Nothing) Then
                        objDBWrite.CloseConnection()
                    End If
                    Return ex.Message
                End Try
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return ex.Message
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return "PASSED"

    End Function

    Private Function ProcessSpecialPayment(ByVal CurrentPeriod As Integer, ByVal tmpAccount As PCMAccount) As String

        Dim dblAmountForCurrentBalance As Double

        '===========================================================================================
        'POST TRANSACTION
        '===========================================================================================
        tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," & _
                 "transaction_type,transaction_amount,pay_type,branch_code) VALUES " & _
                 "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & CurrentPeriod & "'," & _
                 "'" & RG.Apos(tmpAccount.UserName) & "','" & tmpAccount.AccountNumber & "','" & tmpAccount.TransactionNumber & "'" & _
                 ",'" & tmpAccount.TransactionType & "','" & tmpAccount.TransactionAmount & "','" & tmpAccount.PayType & "','" & Mid$(tmpAccount.TransactionNumber, 1, 3) & "'); "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try

        '===========================================================================================
        'TRANSACTION POSTED
        '===========================================================================================

        '===========================================================================================
        'POST PAYMENT PLAN
        '===========================================================================================
        Dim dblPeriod1 As Double
        Dim dblPeriod2 As Double
        Dim dblPeriod3 As Double
        Dim dblPeriod4 As Double
        Dim dblPeriod5 As Double
        Dim dblPeriod6 As Double

        Dim NewTransactionAmount As String

        'The transaction amount can be positive or negative (sales / credit notes)
        'Always easier to times it out then making 2 sets of functions
        Dim wasNegative As Boolean
        If Val(tmpAccount.TransactionAmount) > 0 Then 'Transaction is positive, do nothing
            NewTransactionAmount = tmpAccount.TransactionAmount
            wasNegative = False
        Else
            NewTransactionAmount = Val(tmpAccount.TransactionAmount) * -1
            wasNegative = True
        End If

        'Amount per period is over the minimum. Just divide it through
        'If Val(TransactionAmount) / 6 >= Val(strMinimumPayment) Then
        dblAmountForCurrentBalance = RG.Numb(Val(NewTransactionAmount) / 6)
        dblPeriod1 = RG.Numb(Val(NewTransactionAmount) / 6)
        dblPeriod2 = RG.Numb(Val(NewTransactionAmount) / 6)
        dblPeriod3 = RG.Numb(Val(NewTransactionAmount) / 6)
        dblPeriod4 = RG.Numb(Val(NewTransactionAmount) / 6)
        dblPeriod5 = RG.Numb(Val(NewTransactionAmount) / 6)
        dblPeriod6 = RG.Numb(Val(NewTransactionAmount) / 6)

        If wasNegative = True Then
            dblAmountForCurrentBalance = RG.Numb(dblAmountForCurrentBalance * -1)
            dblPeriod1 = RG.Numb(dblPeriod1 * -1)
            dblPeriod2 = RG.Numb(dblPeriod2 * -1)
            dblPeriod3 = RG.Numb(dblPeriod3 * -1)
            dblPeriod4 = RG.Numb(dblPeriod4 * -1)
            dblPeriod5 = RG.Numb(dblPeriod5 * -1)
            dblPeriod6 = RG.Numb(dblPeriod6 * -1)
            NewTransactionAmount = RG.Numb(Val(NewTransactionAmount) * -1)
        End If

        tmpSQL = "INSERT INTO financial_payment_plans (sale_date,sale_time,account_number,reference_number,total_amount,current_period," & _
                 "period_1,amount_1,period_2,amount_2,period_3,amount_3,period_4,amount_4,period_5,amount_5,period_6,amount_6) " & _
                 "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','" & tmpAccount.AccountNumber & "'," & _
                 "'" & tmpAccount.TransactionNumber & "','" & NewTransactionAmount & "'," & _
                 "'" & CurrentPeriod & "','" & CurrentPeriod & "','" & dblPeriod1 & "','" & CurrentPeriod + 1 & "'," & _
                 "'" & dblPeriod2 & "'," & _
                 "'" & CurrentPeriod + 2 & "','" & dblPeriod3 & "','" & CurrentPeriod + 3 & "','" & dblPeriod4 & "'," & _
                 "'" & CurrentPeriod + 4 & "','" & dblPeriod5 & "','" & CurrentPeriod + 5 & "','" & dblPeriod6 & "'); "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try
        '===========================================================================================
        'FINISHED POSTING PAYMENT PLAN
        '===========================================================================================

        '===========================================================================================
        'UPDATE BALANCES
        '===========================================================================================
        tmpSQL = "UPDATE financial_balances SET total = total + " & tmpAccount.TransactionAmount & "," & _
                     "current_balance = current_balance + " & dblAmountForCurrentBalance & " " & _
                     " WHERE account_number = '" & tmpAccount.AccountNumber & "'; "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            Return ex.Message
        End Try
        '===========================================================================================
        'FINISH UPDATING BALANCES
        '===========================================================================================

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return "PASSED"

    End Function


    Public Function ProcessAccountTransaction(ByVal AccountDetails As PCMAccount) As PCMAccount

        Dim _Return As New PCMAccount

        If Mid$(AccountDetails.CardNumber, 1, 4) = "6501" Then 'Account Card
            _Return = ProcessAccountCard(AccountDetails)
        ElseIf Mid$(AccountDetails.CardNumber, 1, 4) = "6502" Then
            'Gift Card
            _Return = ProcessGiftCard(AccountDetails)
        Else
            _Return.ReturnMessage = "This card is not a valid Rage card"
            Return _Return
        End If

        Return _Return

    End Function

    Public Function ProcessManualPayment(ByVal AccountDetails As PCMAccount) As PCMAccount

        Dim _Return As New PCMAccount

        If Mid$(AccountDetails.CardNumber, 1, 4) = "6501" Then 'Account Card
            _Return = ProcessAccountCard(AccountDetails, True)
        ElseIf Mid$(AccountDetails.CardNumber, 1, 4) = "6502" Then
            'Gift Card
            _Return = ProcessGiftCard(AccountDetails)
        Else
            Dim Msg3 As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
            Dim MailObj3 As New System.Net.Mail.SmtpClient("mail.ragesa.co.za")

            Msg3.From = New System.Net.Mail.MailAddress("reporting@ragesa.co.za", "WebService Query")
            Msg3.To.Add(New System.Net.Mail.MailAddress("dgochin@gmail.com", "Daniel"))

            Msg3.IsBodyHtml = "False"
            Msg3.Subject = "PCM Website Error : ProcessManualPayment"

            Msg3.Body = "This card is not a valid Rage card" & vbCrLf & AccountDetails.CardNumber & vbCrLf & AccountDetails.TransactionType & vbCrLf & AccountDetails.TransactionNumber

            MailObj3.UseDefaultCredentials = False
            MailObj3.Credentials = New System.Net.NetworkCredential("reporting@ragesa.co.za", "Dgdg76097609")
            'MailObj3.Credentials = New System.Net.NetworkCredential("daniel@pricenet.co.za", "dgdg7609")
            MailObj3.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network

            MailObj3.Send(Msg3)

            'Delete the line at store to avoid the line being sent continuously
            _Return.ReturnMessage = "PASSED"
            Return _Return

        End If

        Return _Return

    End Function

    Public Function GetCardNumber(ByVal IDNumber As String) As String

        Dim tmpIDNumber As String = ""

        tmpSQL = "SELECT card_details.card_number " & _
                 "FROM debtor_personal " & _
                 "INNER JOIN card_details ON debtor_personal.account_number = card_details.account_number " & _
                 "WHERE id_number = '" & IDNumber & "'"

        ds = objDBRead.GetDataSet(tmpSQL)

        If objDBRead.isR(ds) Then
            If ds.Tables(0).Rows(0)("card_number") & "" <> "" Then
                tmpIDNumber = ds.Tables(0).Rows(0)("card_number") & ""
            End If
        End If

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        ds.Clear()

        Return tmpIDNumber

    End Function

    Public Sub UpdateCellPhoneNumber(ByVal strAccountNumber As String,
                                     ByVal strUser As String,
                                     ByVal tOldVal As String, ByVal tNewVal As String,
                                     ByVal tEMailAddress As String)

        If tOldVal <> tNewVal Then
            tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " &
                "VALUES (current_date,'" & Format(Now, "HH:mm:ss") & "','" & strUser & "','" & strAccountNumber & "','Shop Cellphone Update','" & RG.Apos(tOldVal) & "','" & RG.Apos(tNewVal) & "')"

            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
            End Try

            tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " &
                "VALUES (current_date,'" & Format(Now, "HH:mm:ss") & "','" & strUser & "','" & strAccountNumber & "','Marked as not Contact Investigation'," &
                "'True','False')"

            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
            End Try

            tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " &
                "VALUES (current_date,'" & Format(Now, "HH:mm:ss") & "','" & strUser & "','" & strAccountNumber & "','Marked dont_send_sms as False'," &
                "'True','False')"

            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
            End Try

            tmpSQL = "UPDATE debtor_personal SET cell_number = '" & RG.Apos(tNewVal) & "',contact_investigation = False," &
                     "dont_send_sms = false WHERE account_number = '" & strAccountNumber & "'"

            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
            End Try
        End If

        If tEMailAddress <> "" Then
            tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " &
                "VALUES (current_date,'" & Format(Now, "HH:mm:ss") & "','" & strUser & "','" & strAccountNumber & "','Shop Email Address Update','','" & RG.Apos(tEMailAddress) & "')"

            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
            End Try

            tmpSQL = "UPDATE debtor_personal SET email_address = '" & RG.Apos(tEMailAddress) & "' WHERE account_number = '" & strAccountNumber & "'"

            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
            End Try
        End If

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

    End Sub


    Public Function GetSelfActivateDetails(ByVal IDNumber As String, ByVal BranchCode As String) As Debtor

        Dim dlChk As New CashCardDataLayer

        Dim ReturnDebtor As New Debtor

        If dlChk.ValidID(IDNumber) = False Then
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
            dsB = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(dsB) Then
                LostCardCharge = Val(dsB.Tables(0).Rows(0)("lost_card_charge") & "")
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If

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
            ds = objDBRead.GetDataSet(tmpSQL)

            If objDBRead.isR(ds) Then
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
                    ReturnDebtor.SelfActivate = False
                End If


                '2014-08-05
                'Calling to check employment resulted in less than 2% of these customers being declined.
                'Stores will now be able to self activte on < 24 accounts as well.
                'If Val(ds.Tables(0).Rows(0)("itc_rating") & "") < 7 Then

                '    Dim tmpYear As String

                '    If Mid$(IDNumber, 1, 2) > 40 Then
                '        tmpYear = "19" & Mid$(IDNumber, 1, 2)
                '    Else
                '        tmpYear = "20" & Mid$(IDNumber, 1, 2)
                '    End If

                '    'DateTime constructor: parameters year, month, day, hour, min, sec
                '    Dim IDDate As New Date(tmpYear, Mid$(IDNumber, 3, 2), Mid$(IDNumber, 5, 2), 12, 0, 0)

                '    If DateDiff(DateInterval.Year, IDDate, DateTime.Now) < 21 Then
                '        'MsgBox("This account cannot be self activated." & vbCrLf & "Please call the Call Centre to activate.", MsgBoxStyle.Exclamation)
                '        'DoButtons(Me, True)
                '        'ds.Clear()
                '        'Exit Sub
                '        ReturnDebtor.SelfActivate = False
                '    End If

                'End If

                ReturnDebtor.AccountNumber = ds.Tables(0).Rows(0)("account_number") & ""
                ReturnDebtor.FirstName = ds.Tables(0).Rows(0)("first_name") & ""
                ReturnDebtor.LastName = ds.Tables(0).Rows(0)("last_name") & ""
                ReturnDebtor.ContactNumber = ds.Tables(0).Rows(0)("cell_number")
                ReturnDebtor.CreditLimit = ds.Tables(0).Rows(0)("credit_limit") & ""
                ReturnDebtor.CurrentStatus = ds.Tables(0).Rows(0)("status") & ""

                If Val(ds.Tables(0).Rows(0)("credit_limit") & "") > 1500 Then
                    'MsgBox("This account cannot be self activated." & vbCrLf & "Please call the Call Centre to activate.", MsgBoxStyle.Exclamation)
                    'DoButtons(Me, True)
                    'ds.Clear()
                    'Exit Sub
                    If (objDBRead IsNot Nothing) Then
                        objDBRead.CloseConnection()
                    End If
                    ReturnDebtor.SelfActivate = False
                End If

                objDBRead.CloseConnection()
                ds.Clear()
                Return ReturnDebtor

            Else
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If

                ReturnDebtor.ReturnMessage = "ID Number does not exist on the system"

                ReturnDebtor.SelfActivate = False
                Return ReturnDebtor
            End If
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ex.Message

            Return ReturnDebtor
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

    End Function


    Public Function InsertSelfActivated(ByVal DebtorDetails As Debtor) As Debtor

        Dim ReturnDebtor As New Debtor
        ReturnDebtor.SelfActivate = True

        Dim ds As DataSet

        tmpSQL = "SELECT card_number,current_status,account_number FROM card_details WHERE card_number = '" & DebtorDetails.CardNumber & "'"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                If ds.Tables(0).Rows(0)("current_status") = "STOLEN" Or ds.Tables(0).Rows(0)("current_status") = "LOST" _
                    Or ds.Tables(0).Rows(0)("current_status") = "BLACKLISTED" Then
                    ReturnDebtor.SelfActivate = False
                    ReturnDebtor.ReturnMessage = ("The card that you used has been Blocked by Head Office." & vbCrLf & "Please use another card.")
                    objDBRead.CloseConnection()
                    ds.Clear()
                    Return ReturnDebtor
                End If

                'Card is already in use
                If ds.Tables(0).Rows(0)("account_number") & "" <> "" Then
                    ReturnDebtor.SelfActivate = False
                    'I'm guessing we give this message as if we do say that it is use, the staff will try to buy on it.
                    ReturnDebtor.ReturnMessage = ("The card that you used has been Blocked by Head Office." & vbCrLf & "Please use another card.")
                    objDBRead.CloseConnection()
                    ds.Clear()
                    Return ReturnDebtor
                End If


            Else
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Invalid Card Number.")
                objDBRead.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End If
        Catch ex As Exception
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try


        tmpSQL = "SELECT cell_number,account_number FROM debtor_personal WHERE cell_number = '" & DebtorDetails.ContactNumber & "'"
        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                If ds.Tables(0).Rows(0)("account_number") <> DebtorDetails.AccountNumber Then
                    ReturnDebtor.SelfActivate = False
                    ReturnDebtor.ReturnMessage = ("This Cellphone number already exists in the Database for another account. Please call the Call Centre to assign a Card to this Account.")
                    objDBRead.CloseConnection()
                    ds.Clear()
                    Return ReturnDebtor
                End If
            End If
        Catch ex As Exception
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try

        'Delete all previous cards for this customer
        tmpSQL = "UPDATE card_details SET account_number = NULL, current_status = 'LOST'," &
                 "delivered_by = '" & DebtorDetails.EmployeeNumber & "',assigned_by = '" & DebtorDetails.EmployeeNumber & "'," &
                 "assigned_at_branch = '" & DebtorDetails.BranchCode & "' WHERE account_number = '" & DebtorDetails.AccountNumber & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            objDBWrite.CloseConnection()
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

                ds = objDBRead.GetDataSet(tmpSQL)
                If objDBRead.isR(ds) Then
                    current_period = Val(ds.Tables(0).Rows(0)("current_period"))
                End If
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                objDBRead.CloseConnection()
                objDBWrite.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End Try

            Try
                tmpSQL = "SELECT nextval('enum_journal_debit_seq')"

                ds = objDBWrite.GetDataSet(tmpSQL)
                If objDBWrite.isR(ds) Then
                    transaction_number = ds.Tables(0).Rows(0)("nextval")
                End If
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                objDBRead.CloseConnection()
                objDBWrite.CloseConnection()
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

                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                objDBRead.CloseConnection()
                objDBWrite.CloseConnection()
                ds.Clear()
                Return ReturnDebtor
            End Try

            'Update balances
            Try
                tmpSQL = "UPDATE financial_balances SET total = total + '" & Val(DebtorDetails.PayNewCard) & "'," &
                         "current_balance = '" & RG.Numb(Val(DebtorDetails.PayNewCard) / 6) & "' " &
                         "WHERE account_number = '" & DebtorDetails.AccountNumber & "'"

                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                objDBRead.CloseConnection()
                objDBWrite.CloseConnection()
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

                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                ReturnDebtor.SelfActivate = False
                ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
                objDBRead.CloseConnection()
                objDBWrite.CloseConnection()
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
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                If ds.Tables(0).Rows(0)("branch_code") & "" = "" Then
                    'Assign the branch code to the customer
                    tmpSQL = "UPDATE debtor_personal SET branch_code = '" & DebtorDetails.BranchCode & "',auto_increase = '" & autoincrease & "',card_protection = '" & lostcard & "', " &
                         "preferred_language = '" & DebtorDetails.PreferredLanguage & "' WHERE account_number = '" & DebtorDetails.AccountNumber & "'"
                Else
                    tmpSQL = "UPDATE debtor_personal SET auto_increase = '" & autoincrease & "',card_protection = '" & lostcard & "', " &
                         "preferred_language = '" & DebtorDetails.PreferredLanguage & "' WHERE account_number = '" & DebtorDetails.AccountNumber & "'"
                End If
            End If

            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBRead.CloseConnection()
            objDBWrite.CloseConnection()
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

            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBRead.CloseConnection()
            objDBWrite.CloseConnection()
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card not assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try

        Try
            tmpSQL = "UPDATE card_dates SET date_assigned = '" & Format(Now, "yyyy-MM-dd") & "' WHERE card_number = '" & DebtorDetails.CardNumber & "'"

            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBRead.CloseConnection()
            objDBWrite.CloseConnection()
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

            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBRead.CloseConnection()
            objDBWrite.CloseConnection()
            ReturnDebtor.SelfActivate = False
            ReturnDebtor.ReturnMessage = ("Internal Server Error. Card was still assigned." & vbCrLf & "Error: " & ex.Message)
            objDBRead.CloseConnection()
            ds.Clear()
            Return ReturnDebtor
        End Try


        objDBRead.CloseConnection()
        objDBWrite.CloseConnection()

        Return ReturnDebtor

    End Function









    'These funtions are from the old VB6, PCM code.
    'Can't use the regular functions for updating balances, etc, as they all involve updating things like
    'last transactions and card, etc.
    'This code has been used for many years and the bugs have (hopefully) all been sorted out.

    Public Function ProcessJournalEntry(ByVal _JournalEntryRequest As JournalEntryRequest) As String
        Dim CurrentAccountPeriod As Integer
        Dim Function1 As String
        Dim Function2 As String
        Dim Function3 As String

        Dim AccountNumber As String = _JournalEntryRequest.AccountNumber
        Dim Amount As Double = _JournalEntryRequest.Amount
        Dim BalanceAffected As String = _JournalEntryRequest.BalanceAffected
        Dim TransactionType As String = _JournalEntryRequest.TransactionType
        Dim AffectedPeriod As String = _JournalEntryRequest.AffectedPeriod

        Dim _PostTransactionRequest As New PostTransactionRequest

        '_PostTransactionRequest.LoggedInUser = _JournalEntryRequest.LoggedInUser
        _PostTransactionRequest.AccountNumber = RG.Apos(_JournalEntryRequest.AccountNumber)
        _PostTransactionRequest.TransactionAmount = Amount
        _PostTransactionRequest.User = _JournalEntryRequest.User
        _PostTransactionRequest.Notes = _JournalEntryRequest.Notes


        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        tmpSQL = "SELECT current_period FROM general_settings"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                CurrentAccountPeriod = ds.Tables(0).Rows(0)("current_period")
            End If

            If CurrentAccountPeriod <= 0 Then
                ProcessJournalEntry = "System Failure. No Current Period Set."
                Return ProcessJournalEntry
            End If

            _PostTransactionRequest.CurrentAccountPeriod = CurrentAccountPeriod


            If _JournalEntryRequest.TransactionType = "Credit" Then
                If _JournalEntryRequest.BalanceAffected = True Then
                    _PostTransactionRequest.TransactionType = "LEDC"
                    Function1 = PostTransaction(_PostTransactionRequest)
                    If Function1 <> "PASSED" Then
                        ProcessJournalEntry = Function1
                        Return ProcessJournalEntry
                    End If
                ElseIf BalanceAffected = False Then
                    _PostTransactionRequest.TransactionType = "LEDCN"
                    Function1 = PostTransaction(_PostTransactionRequest)
                    If Function1 <> "PASSED" Then
                        ProcessJournalEntry = Function1
                        Return ProcessJournalEntry
                    End If
                End If
            ElseIf TransactionType = "Debit" Then
                If BalanceAffected = True Then
                    _PostTransactionRequest.TransactionType = "LEDD"
                    Function1 = PostTransaction(_PostTransactionRequest)
                    If Function1 <> "PASSED" Then
                        ProcessJournalEntry = Function1
                        Return ProcessJournalEntry
                    End If
                ElseIf BalanceAffected = False Then
                    _PostTransactionRequest.TransactionType = "LEDDN"
                    Function1 = PostTransaction(_PostTransactionRequest)
                    If Function1 <> "PASSED" Then
                        ProcessJournalEntry = Function1
                        Return ProcessJournalEntry
                    End If
                End If
            ElseIf TransactionType = "Bad Debt" Then
                _PostTransactionRequest.TransactionType = "BADD"
                Function1 = PostTransaction(_PostTransactionRequest)
                If Function1 <> "PASSED" Then
                    ProcessJournalEntry = Function1
                    Return ProcessJournalEntry
                End If
            End If

            Function2 = UpdateBalancesJournal(RG.Apos(AccountNumber), Amount, BalanceAffected, AffectedPeriod)
            If Function2 <> "PASSED" Then
                ProcessJournalEntry = Function2
                Return ProcessJournalEntry
            End If

            'If BalanceAffected = True Then
            '    Function3 = FunctionUpdateLimit(RG.Apos(AccountNumber), Amount)
            '    If Function3 <> "PASSED" Then
            '        ProcessJournalEntry = Function3
            '        Return ProcessJournalEntry
            '    End If
            'End If

            ProcessJournalEntry = "PASSED"
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If

            ProcessJournalEntry = "Something Went Wrong"
            Return ProcessJournalEntry
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        Return ProcessJournalEntry
    End Function

    Public Function UpdateBalancesJournal(ByVal AccountNumber As String, ByVal Amount As Double,
                                  ByVal TotalAffected As Boolean, ByVal AffectedPeriod As String) As String
        Dim Function1 As String

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")
        Try

            If TotalAffected = True Then
                tmpSQL = "UPDATE financial_balances SET total = total + '" & Amount & "'"
            Else
                'Bit of garbage to avoid the double 'SET'
                tmpSQL = "UPDATE financial_balances SET total = total + 0"
            End If

            Select Case AffectedPeriod
                Case "CURRENT"
                    tmpSQL = tmpSQL & ",current_balance = current_balance + '" & Amount & "'"
                Case "30 DAYS"
                    tmpSQL = tmpSQL & ",p30 = p30 + '" & Amount & "'"
                Case "60 DAYS"
                    tmpSQL = tmpSQL & ",p60 = p60 + '" & Amount & "'"
                Case "90 DAYS"
                    tmpSQL = tmpSQL & ",p90 = p90 + '" & Amount & "'"
                Case "120 DAYS"
                    tmpSQL = tmpSQL & ",p120 = p120 + '" & Amount & "'"
                Case "150 DAYS"
                    tmpSQL = tmpSQL & ",p150 = p150 + '" & Amount & "'"
                Case "FROM OLDEST PERIOD"
                    'Already have the functionality done for payments
                    tmpSQL = "SELECT financial_balances.current_balance AS current,financial_balances.p30 AS p30,financial_balances.p60 AS p60,financial_balances.p90 AS p90," &
                             "financial_balances.p120 AS p120,financial_balances.p150 AS p150 FROM financial_balances " &
                             "WHERE account_number = '" & AccountNumber & "'"

                    ds = objDB.GetDataSet(tmpSQL)

                    If objDB.isR(ds) Then
                        If TotalAffected = True Then
                            Function1 = FunctionUpdateBalancesPay(AccountNumber, Amount, Val(ds.Tables(0).Rows(0)("current") & ""), Val(ds.Tables(0).Rows(0)("p30") & ""),
                                                        Val(ds.Tables(0).Rows(0)("p60") & ""), Val(ds.Tables(0).Rows(0)("p90") & ""),
                                                        Val(ds.Tables(0).Rows(0)("p120") & ""), Val(ds.Tables(0).Rows(0)("p150") & ""), True)
                        Else
                            Function1 = FunctionUpdateBalancesPay(AccountNumber, Amount, Val(ds.Tables(0).Rows(0)("current") & ""), Val(ds.Tables(0).Rows(0)("p30") & ""),
                                                        Val(ds.Tables(0).Rows(0)("p60") & ""), Val(ds.Tables(0).Rows(0)("p90") & ""),
                                                        Val(ds.Tables(0).Rows(0)("p120") & ""), Val(ds.Tables(0).Rows(0)("p150") & ""), False)
                        End If

                        'Taking from the last period completes the query and needs to exit as to not run twice
                        If Function1 = "PASSED" Then
                            UpdateBalancesJournal = "PASSED"
                            Exit Function
                        Else
                            UpdateBalancesJournal = Function1
                            Exit Function
                        End If
                    End If

                Case "ACROSS ALL PERIODS"
                    tmpSQL = tmpSQL & ",current_balance = current_balance + '" & Amount / 6 & "',p30 = p30 + '" & Amount / 6 & "',p60 = p60 + '" & Amount / 6 & "'," &
                                      "p90 = p90 + '" & Amount / 6 & "',p120 = p120 + '" & Amount / 6 & "',p150 = p150 + '" & Amount / 6 & "'"
            End Select

            tmpSQL = tmpSQL & " WHERE account_number = '" & AccountNumber & "'"
            objDB.ExecuteQuery(tmpSQL)

            UpdateBalancesJournal = "PASSED"

        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If

            UpdateBalancesJournal = "Something Went Wrong"
            Return UpdateBalancesJournal
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        Return UpdateBalancesJournal
    End Function

    Public Function PostTransaction(ByVal _PostTransactionRequest As PostTransactionRequest) As String

        Dim strTransactionNumber As Integer
        Dim strApprovalCode As String = ""

        Dim AccountNumber As String = _PostTransactionRequest.AccountNumber
        Dim CardNumber As String = _PostTransactionRequest.CardNumber
        Dim TransactionAmount As String = _PostTransactionRequest.TransactionAmount
        Dim BranchCode As String = _PostTransactionRequest.BranchCode
        Dim TransactionType As String = _PostTransactionRequest.TransactionType
        Dim LoggedInUser As String = _PostTransactionRequest.User
        Dim CurrentAccountPeriod As String = _PostTransactionRequest.CurrentAccountPeriod
        Dim PayType As String = _PostTransactionRequest.PayType
        Dim Notes As String = _PostTransactionRequest.Notes


        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")
        Try

            '===========================================================================================
            'Getting and updating the numbers
            '===========================================================================================
            'GetNum

            Select Case _PostTransactionRequest.TransactionType
                Case "LEDC" 'Ledger Credit Balance Affected
                    tmpSQL = "SELECT nextval('enum_journal_credit_seq')"
                    ds = objDB.GetDataSet(tmpSQL)
                    If objDB.isR(ds) Then
                        strTransactionNumber = ds.Tables(0).Rows(0)("nextval")
                    End If
                Case "LEDCN" 'Ledger Credit Balance Not Affected
                    tmpSQL = "SELECT nextval('enum_journal_credit_seq')"
                    ds = objDB.GetDataSet(tmpSQL)
                    If objDB.isR(ds) Then
                        strTransactionNumber = ds.Tables(0).Rows(0)("nextval")
                    End If
                Case "LEDD" 'Ledger Debit
                    tmpSQL = "SELECT nextval('enum_journal_debit_seq')"
                    ds = objDB.GetDataSet(tmpSQL)
                    If objDB.isR(ds) Then
                        strTransactionNumber = ds.Tables(0).Rows(0)("nextval")
                    End If
                Case "LEDDN" 'Ledger Debit Balance Not Affected
                    tmpSQL = "SELECT nextval('enum_journal_debit_seq')"
                    ds = objDB.GetDataSet(tmpSQL)
                    If objDB.isR(ds) Then
                        strTransactionNumber = ds.Tables(0).Rows(0)("nextval")
                    End If
                Case "BADD" 'Bad Debt
                    tmpSQL = "SELECT nextval('enum_bad_debt_seq')"
                    ds = objDB.GetDataSet(tmpSQL)
                    If objDB.isR(ds) Then
                        strTransactionNumber = ds.Tables(0).Rows(0)("nextval")
                    End If
            End Select

            '===========================================================================================
            If strApprovalCode = "" Then
                tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," &
                     "transaction_type,transaction_amount,pay_type,branch_code,notes) VALUES " &
                     "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "'," &
                     "'" & CurrentAccountPeriod & "','" & LoggedInUser & "','" & AccountNumber & "'," &
                     "'HO-" & strTransactionNumber & "','" & TransactionType & "'," &
                     "'" & TransactionAmount & "','" & PayType & "','" & BranchCode & "','" & RG.Apos(Notes) & "')"
            Else
                tmpSQL = "INSERT INTO financial_transactions (sale_date,sale_time,current_period,username,account_number,reference_number," &
                     "transaction_type,transaction_amount,pay_type,auth_code,branch_code,notes) VALUES " &
                     "('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "'," &
                     "'" & CurrentAccountPeriod & "','" & LoggedInUser & "','" & AccountNumber & "'," &
                     "'HO-" & strTransactionNumber & "','" & TransactionType & "'," &
                     "'" & TransactionAmount & "','" & PayType & "'," &
                     "'" & strApprovalCode & "','" & BranchCode & "','" & RG.Apos(Notes) & "')"
            End If
            objDB.ExecuteQuery(tmpSQL)
            PostTransaction = "PASSED"


        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If

            PostTransaction = "Something Went Wrong"
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        Return PostTransaction
    End Function

    Public Function FunctionUpdateBalancesPay(ByVal AccountNumber As String,
                                          ByVal TransactionAmount As Double, ByVal Current As Double, ByVal P30 As Double,
                                          ByVal P60 As Double, ByVal P90 As Double, ByVal P120 As Double,
                                          ByVal P150 As Double, ByVal AffectTotal As Boolean,
                                          Optional ByVal CurrentTotal As String = "") As String

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")
        Try

            Dim RunningAmount As Double
            Dim OriginalTransactionAmount As Double

            RunningAmount = 0

            OriginalTransactionAmount = TransactionAmount

            'Change the amount from a negative into a positive. (Payments are negative transactions.)
            'Just easier to work with a positive here
            TransactionAmount = TransactionAmount * -1

            'p150
            If P150 <> 0 Then
                If P150 >= TransactionAmount Then
                    P150 = P150 - TransactionAmount
                ElseIf P150 < TransactionAmount Then
                    RunningAmount = TransactionAmount - P150
                    P150 = 0
                End If
            Else
                RunningAmount = TransactionAmount
            End If

            'p120
            If RunningAmount > 0 Then
                If P120 <> 0 Then
                    If P120 >= RunningAmount Then
                        P120 = P120 - RunningAmount
                        RunningAmount = 0
                    ElseIf P120 < RunningAmount Then
                        RunningAmount = RunningAmount - P120
                        P120 = 0
                    End If
                End If
            End If

            'p90
            If RunningAmount > 0 Then
                If P90 <> 0 Then
                    If P90 >= RunningAmount Then
                        P90 = P90 - RunningAmount
                        RunningAmount = 0
                    ElseIf P90 < RunningAmount Then
                        RunningAmount = RunningAmount - P90
                        P90 = 0
                    End If
                End If
            End If

            'p60
            If RunningAmount > 0 Then
                If P60 <> 0 Then
                    If P60 >= RunningAmount Then
                        P60 = P60 - RunningAmount
                        RunningAmount = 0
                    ElseIf P60 < RunningAmount Then
                        RunningAmount = RunningAmount - P60
                        P60 = 0
                    End If
                End If
            End If

            'p30
            If RunningAmount > 0 Then
                If P30 <> 0 Then
                    If P30 >= RunningAmount Then
                        P30 = P30 - RunningAmount
                        RunningAmount = 0
                    ElseIf P30 < RunningAmount Then
                        RunningAmount = RunningAmount - P30
                        P30 = 0
                    End If
                End If
            End If

            'current
            If RunningAmount > 0 Then
                If Current <> 0 Then
                    If Current >= RunningAmount Then
                        Current = Current - RunningAmount
                        RunningAmount = 0
                    ElseIf Current < RunningAmount Then
                        RunningAmount = RunningAmount - Current
                        Current = 0
                    End If
                End If
            End If

            'Put whatever is left in current
            Current = Current - RunningAmount

            '2012-11-09
            'Updating payments to deal with credits.
            'There were issues with credits in the age analysis
            If CurrentTotal <> "" Then
                If Val(CurrentTotal) - TransactionAmount <= 0 Then
                    'Make all periods zero
                    Current = 0
                    P30 = 0
                    P60 = 0
                    P90 = 0
                    P120 = 0
                    P150 = 0

                    'Delete all (upcoming) records from the payment plans
                    tmpSQL = "DELETE FROM financial_payment_plans WHERE account_number = '" & AccountNumber & "'"
                    objDB.ExecuteQuery(tmpSQL)
                End If
            End If

            If AffectTotal = True Then
                tmpSQL = "UPDATE financial_balances SET total = total - " & TransactionAmount & ",current_balance = '" & Current & "'," &
                         "p30 = '" & P30 & "',p60 = '" & P60 & "',p90 = '" & P90 & "',p120 = '" & P120 & "',p150 = '" & P150 & "' WHERE account_number = '" & AccountNumber & "'"
            Else
                tmpSQL = "UPDATE financial_balances SET current_balance = '" & Current & "'," &
                         "p30 = '" & P30 & "',p60 = '" & P60 & "',p90 = '" & P90 & "',p120 = '" & P120 & "',p150 = '" & P150 & "' WHERE account_number = '" & AccountNumber & "'"
            End If

            objDB.ExecuteQuery(tmpSQL)

            FunctionUpdateBalancesPay = "PASSED"

        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If

            FunctionUpdateBalancesPay = "Something Went Wrong"

        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        Return FunctionUpdateBalancesPay
    End Function

    Public Function FunctionUpdateLimit(ByVal AccountNumber As String, ByVal Amount As Double) As String
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")
        Try
            tmpSQL = "UPDATE financial_balances SET total = total + " & Amount & " WHERE account_number = '" & AccountNumber & "'"
            objDB.ExecuteQuery(tmpSQL)

            FunctionUpdateLimit = "PASSED"

        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If

            FunctionUpdateLimit = "Something Went Wrong"
            Return FunctionUpdateLimit
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        Return FunctionUpdateLimit
    End Function

End Class
