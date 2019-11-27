Imports Npgsql
Imports Microsoft.VisualBasic
Imports System.Data
Imports Entities
Imports System.IO
Imports Newtonsoft.Json
Imports Entities.Stock

Public Class ShopTransactionsDL
    Inherits DataAccessLayerBase

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing

    Dim _database As String

    'Public Sub New(ByVal CompanyCode As String)

    '    If Not HttpContext.Current.IsDebuggingEnabled Then
    '        'Not debugging
    '        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite", CompanyCode)
    '        _database = CompanyCode
    '        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead", CompanyCode)
    '        connection = Me.DataBase("PostgreConnectionStringPositiveRead", CompanyCode)
    '    Else
    '        _database = CompanyCode
    '        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWriteTesting", CompanyCode)
    '        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting", CompanyCode)
    '        connection = Me.DataBase("PostgreConnectionStringPositiveReadTesting", CompanyCode)
    '    End If

    'End Sub

    Public Sub New()
        _database = ConfigurationManager.AppSettings("CurrentDatabase")
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = Me.DataBase("PostgreConnectionStringPCMRead")

    End Sub

    Public Function ProcessCashUp(ByVal TransactionData As CashUpData) As String

        tmpSQL = "INSERT INTO cash_ups (guid,branch_code,username,total_cash,total_cheque,total_credit_card,total_voucher," &
                 "total_credit_note,total_system_cash,total_system_cheque," &
                 "total_system_credit_card,total_system_voucher,total_system_credit_note,date_of_cashup," &
                 "time_of_cashup,till_number) VALUES " &
                 "('" & TransactionData.Guid & "','" & TransactionData.branch_code & "','" & TransactionData.username & "'," &
                 "'" & RG.Numb(TransactionData.total_cash) & "','" & RG.Numb(TransactionData.total_cheque) & "'," &
                 "'" & RG.Numb(TransactionData.total_credit_note) & "','" & RG.Numb(TransactionData.total_voucher) & "'," &
                 "'" & RG.Numb(TransactionData.total_credit_card) & "','" & TransactionData.total_system_cash & "'," &
                 "'" & TransactionData.total_system_cheque & "','" & TransactionData.total_system_credit_card & "'," &
                 "'" & TransactionData.total_system_voucher & "','" & TransactionData.total_system_credit_note & "'," &
                 "'" & TransactionData.date_of_cashup & "','" & TransactionData.time_of_cashup & "','" & TransactionData.till_number & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, TransactionData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & TransactionData.Guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If ex.Message.ToString.Contains("duplicate key value") Then
            '    Return TransactionData.Guid
            'End If

            'If ex.Message.ToString.Contains("violates foreign key constraint") Then
            '    Return TransactionData.Guid
            'End If

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            'Return ""
        End Try


        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        'Return the GuID either way. Data not critical.
        Return TransactionData.Guid


    End Function

    Public Function ProcessReprint(ByVal ReprintTransaction As ReprintTransaction, ByVal ServerPath As String) As String

        'Dim tmpGUID As String
        'tmpGUID = Guid.NewGuid.ToString

        tmpSQL = "INSERT INTO reprints " &
                 "(reprint_timestamp,branch_code,username,transaction_type,transaction_number)" &
                 " VALUES('" & RG.Apos(ReprintTransaction.ReprintTimestamp) & "','" & Mid$(RG.Apos(ReprintTransaction.BranchCode), 1, 5) & "'," &
                 "'" & Mid$(RG.Apos(ReprintTransaction.Username), 1, 45) & "','" & Mid$(RG.Apos(ReprintTransaction.TransactionType), 1, 18) & "'," &
                 "'" & Mid$(RG.Apos(ReprintTransaction.TransactionNumber), 1, 28) & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, ReprintTransaction.BranchCode)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & ReprintTransaction.Guid & "','" & RG.Apos(tmpSQL) & "','" & ReprintTransaction.BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return ReprintTransaction.Guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                Return ReprintTransaction.Guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return ReprintTransaction.Guid


    End Function

    Public Function NewVAT(ByVal BranchCode As String, ByVal RecordsUpdated As String,
                           ByVal TimeStampCompleted As String) As String

        tmpSQL = "INSERT INTO new_vat " &
                 "(branch_code,time_stamp,records_updated)" &
                 " VALUES ('" & RG.Apos(BranchCode) & "','" & TimeStampCompleted & "','" & RecordsUpdated & "')"

        Try
            objDBWrite.ExecuteQuery(tmpSQL, BranchCode)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
            '      "('" & CashTransaction.transaction_guid & "','" & RG.Apos(tmpSQL) & "','" & CashTransaction.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        Return ""

    End Function

    Public Function ProcessCashTransaction(ByVal CashTransaction As CashTransaction, ByVal ServerPath As String) As String

        'Dim tmpGUID As String
        'tmpGUID = Guid.NewGuid.ToString

        tmpSQL = "INSERT INTO cash_transactions " &
                 "(guid,sale_date,sale_time,branch_code,user_name,till_number,transaction_type,transaction_number,sale_total,cash,cheque," &
                 "credit_card,voucher,account,payment_details)" &
                 " VALUES('" & RG.Apos(CashTransaction.transaction_guid) & "','" & CashTransaction.sale_date & "','" & CashTransaction.sale_time & "','" & Mid$(RG.Apos(CashTransaction.branch_code), 1, 5) & "'," &
                 "'" & Mid$(RG.Apos(CashTransaction.user_name), 1, 19) & "','" & Mid$(RG.Apos(CashTransaction.till_number), 1, 3) & "','" & Mid$(RG.Apos(CashTransaction.transaction_type), 1, 20) & "'," &
                 "'" & Mid$(RG.Apos(CashTransaction.transaction_number), 1, 15) & "'," &
                 "'" & RG.Apos(RG.Numb(CashTransaction.sale_total)) & "','" & RG.Apos(RG.Numb(CashTransaction.cash)) & "','" & RG.Apos(RG.Numb(CashTransaction.cheque)) & "'," &
                 "'" & RG.Apos(RG.Numb(CashTransaction.credit_card)) & "'," &
                 "'" & RG.Apos(RG.Numb(CashTransaction.voucher)) & "','" & RG.Apos(RG.Numb(CashTransaction.account)) & "','" & Mid$(RG.Apos(CashTransaction.payment_details), 1, 48) & "')"

        Try
            objDBWrite.ExecuteQuery(tmpSQL, CashTransaction.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
            '      "('" & CashTransaction.transaction_guid & "','" & RG.Apos(tmpSQL) & "','" & CashTransaction.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return CashTransaction.transaction_guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                Return CashTransaction.transaction_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If CashTransaction.card_number <> "" Then
            tmpSQL = "INSERT INTO card_to_cash (guid,card_number) VALUES ('" & CashTransaction.transaction_guid & "','" & CashTransaction.card_number & "')"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception

                'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
                '      "('" & CashTransaction.transaction_guid & "','" & RG.Apos(tmpSQL) & "','" & CashTransaction.branch_code & "','" & RG.Apos(ex.Message) & "')"
                'objDBWrite.ExecuteQuery(tmpSQL)

                If ex.Message.ToString.Contains("duplicate key value") Then
                    Return CashTransaction.transaction_guid
                End If

                If ex.Message.ToString.Contains("violates foreign key constraint") Then
                    Return CashTransaction.transaction_guid
                End If

                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If

                Return ""
            End Try
        End If

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return CashTransaction.transaction_guid


    End Function

    Public Function InsertVoid(ByVal _void As Void) As String

        tmpSQL = "INSERT INTO voids (guid,void_date,void_time,branch_code,user_name,generated_code,quantity) VALUES " &
                 "('" & _void.guid & "','" & _void.void_date & "','" & _void.void_time & "','" & _void.branch_code & "', " &
                 "'" & RG.Apos(_void.user_name.ToUpper) & "','" & RG.Apos(_void.generated_code.ToUpper) & "','" & Val(_void.quantity) & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, _void.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & _void.guid & "','" & RG.Apos(tmpSQL) & "','" & _void.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return _void.guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return _void.guid

    End Function

    Public Function ProcessHeader(ByVal TransactionData As TransactionMaster, ByVal ServerPath As String) As String


        tmpSQL = "INSERT INTO transaction_master " &
                "(guid,sale_date,sale_time,branch_code,customer_code,till_number,user_name,rep_code,number_of_items,address_line_1,address_line_2," &
                "address_line_3,address_line_4,address_line_5,reference_number,transaction_type,transaction_number,transaction_total," &
                "transaction_total_taxable,transaction_total_non_taxable,transaction_total_tax,transaction_total_discount,account_number,price_level,positive_pc_id)" &
                " VALUES('" & RG.Apos(TransactionData.transaction_guid) & "','" & TransactionData.sale_date & "','" & TransactionData.sale_time & "','" & Mid$(RG.Apos(TransactionData.branch_code), 1, 5) & "'," &
                "'" & Mid$(RG.Apos(TransactionData.customer_code), 1, 29) & "','" & Mid$(RG.Apos(TransactionData.till_number), 1, 3) & "','" & Mid$(RG.Apos(TransactionData.user_name), 1, 19) & "'," &
                "'" & Mid$(RG.Apos(TransactionData.rep_code), 1, 5) & "'," &
                "'" & RG.Numb(RG.Apos(TransactionData.number_of_items)) & "','" & Mid$(RG.Apos(TransactionData.address_line_1), 1, 48) & "','" & Mid$(RG.Apos(TransactionData.address_line_2), 1, 48) & "'," &
                "'" & Mid$(RG.Apos(TransactionData.address_line_3), 1, 48) & "'," &
                "'" & Mid$(RG.Apos(TransactionData.address_line_4), 1, 48) & "','" & Mid$(RG.Apos(TransactionData.address_line_5), 1, 48) & "','" & Mid$(RG.Apos(TransactionData.reference_number), 1, 48) & "'," &
                "'" & Mid$(RG.Apos(TransactionData.transaction_type), 1, 20) & "'," &
                "'" & Mid$(RG.Apos(TransactionData.transaction_number), 1, 15) & "','" & RG.Numb(TransactionData.transaction_total) & "','" & RG.Numb(TransactionData.transaction_total_taxable) & "'," &
                "'0','" & RG.Numb(TransactionData.transaction_total_tax) & "'," &
                "'" & RG.Numb(TransactionData.transaction_total_discount) & "','" & Mid$(RG.Apos(TransactionData.account_number), 1, 12) & "'," &
                "NULLIF('" & RG.Apos(TransactionData.price_level) & "', '')::numeric,'" & TransactionData.positive_pc_id & "')"

        Try
            objDBWrite.ExecuteQuery(tmpSQL, TransactionData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
            '      "('" & TransactionData.transaction_guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return TransactionData.transaction_guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                Return TransactionData.transaction_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        tmpSQL = "INSERT INTO transaction_times (guid,period,currrent_hour,current_weekday,current_week) VALUES " &
                "('" & RG.Apos(TransactionData.transaction_guid) & "','1" & TransactionData.current_month & "','" & TransactionData.current_hour & "','" & TransactionData.current_weekday & "'," &
                "'" & TransactionData.current_week & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, TransactionData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
            '      "('" & TransactionData.transaction_guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return TransactionData.transaction_guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                Return TransactionData.transaction_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return TransactionData.transaction_guid

    End Function

    Public Sub RemStock(ByVal generated_code As String,
                         ByVal quantity As String, ByVal branch_code As String,
                         Optional ByVal INVorIBT As String = "")

        '        'Check if the code was originally inserted
        '        tmpSQL = "SELECT generated_code FROM stock_on_hand WHERE generated_code = '" & RG.Apos(generated_code) & "' " &
        '                 "AND branch_code = '" & RG.Apos(branch_code) & "'"
        '        Try
        '            ds = objDBRead.GetDataSet(tmpSQL)
        '            If objDBRead.isR(ds) Then
        '                'Record exists
        '                GoTo RecordExists
        '            End If

        '        Catch ex As Exception
        '            tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
        '                  "('NO GUID','" & RG.Apos(tmpSQL) & "','" & branch_code & "','" & RG.Apos(ex.Message) & "')"
        '            objDBWrite.ExecuteQuery(tmpSQL)

        '            If (objDBWrite IsNot Nothing) Then
        '                objDBWrite.CloseConnection()
        '            End If

        '            Exit Sub
        '        End Try

        '        tmpSQL = "INSERT INTO stock_on_hand (generated_code,branch_code) VALUES ('" & RG.Apos(Mid$(generated_code.ToUpper, 1, 30)) & "'," &
        '                                  "'" & branch_code.ToString.ToUpper & "')"
        '        Try
        '            objDBWrite.ExecuteQuery(tmpSQL)
        '        Catch ex As Exception
        '            objDBWrite.CloseConnection()

        '            tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
        '                  "('NO GUID','" & RG.Apos(tmpSQL) & "','" & branch_code & "','" & RG.Apos(ex.Message) & "')"
        '            objDBWrite.ExecuteQuery(tmpSQL)

        '            If (objDBWrite IsNot Nothing) Then
        '                objDBWrite.CloseConnection()
        '            End If

        '            Exit Sub
        '        End Try

        'RecordExists:

        tmpSQL = "UPDATE stock_on_hand SET qty_on_hand = qty_on_hand - " & quantity & ",updated = '" & Format(Now, "yyyy-MM-dd") & "' WHERE generated_code = '" & RG.Apos(generated_code.ToUpper) & "' AND branch_code = '" & RG.Apos(branch_code.ToUpper) & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('NO GUID','" & RG.Apos(tmpSQL) & "','" & branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Exit Sub
        End Try

        If INVorIBT = "INV" Then
            tmpSQL = "UPDATE stockcodes_dates SET last_sale = '" & Format(Now, "yyyy-MM-dd") & "' WHERE generated_code = '" & RG.Apos(generated_code.ToUpper) & "'"
        ElseIf INVorIBT = "IBT" Then
            tmpSQL = "UPDATE stockcodes_dates SET last_ibt = '" & Format(Now, "yyyy-MM-dd") & "' WHERE generated_code = '" & RG.Apos(generated_code.ToUpper) & "'"
        End If

        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('NO GUID','" & RG.Apos(tmpSQL) & "','" & branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Exit Sub
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

    End Sub

    Public Sub AddHistory(ByVal SALEorRETURN As String,
                         ByVal HistoryQtyPositiveOnly As String,
                         ByVal GeneratedCode As String)


        'Jacques Steenberg 02/04/2008
        'Daniel Gochin 03/06/2008
        'Changed weeks to match Live reporting 2011-07-31
        'Gil Barazani copied it from the shop just for fun 19/03/2014
        Select Case SALEorRETURN
            Case "SALE"
                tmpSQL = "UPDATE stock_bestsellers_monthly SET p1" & Format(Now, "MM") & " = p1" & Format(Now, "MM") & " + " & HistoryQtyPositiveOnly & " WHERE generated_code = '" & GeneratedCode.ToUpper & "'"
                Try
                    objDBWrite.ExecuteQuery(tmpSQL)
                Catch ex As Exception
                    'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
                    '      "('NO GUID','" & RG.Apos(tmpSQL) & "','IN SQL','" & RG.Apos(ex.Message) & "')"
                    'objDBWrite.ExecuteQuery(tmpSQL)

                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If

                    Exit Sub
                End Try

                tmpSQL = "UPDATE stock_bestsellers_weekly SET w" & DatePart("ww", Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) & " = w" & DatePart("ww", Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) & " + " & Val(HistoryQtyPositiveOnly) & " WHERE generated_code = '" & GeneratedCode.ToUpper & "'"
                Try
                    objDBWrite.ExecuteQuery(tmpSQL)
                Catch ex As Exception
                    'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
                    '      "('NO GUID','" & RG.Apos(tmpSQL) & "','IN SQL','" & RG.Apos(ex.Message) & "')"
                    'objDBWrite.ExecuteQuery(tmpSQL)

                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If

                    Exit Sub
                End Try

            Case "RETURN"
                tmpSQL = "UPDATE stock_bestsellers_monthly SET p1" & Format(Now, "MM") & " = p1" & Format(Now, "MM") & " - " & HistoryQtyPositiveOnly & " WHERE generated_code = '" & GeneratedCode.ToUpper & "'"
                Try
                    objDBWrite.ExecuteQuery(tmpSQL)
                Catch ex As Exception
                    'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
                    '      "('NO GUID','" & RG.Apos(tmpSQL) & "','IN SQL','" & RG.Apos(ex.Message) & "')"
                    'objDBWrite.ExecuteQuery(tmpSQL)

                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If

                    Exit Sub
                End Try

                tmpSQL = "UPDATE stock_bestsellers_weekly SET w" & DatePart("ww", Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) & " = w" & DatePart("ww", Now, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) & " - " & Val(HistoryQtyPositiveOnly) & " WHERE generated_code = '" & GeneratedCode.ToUpper & "'"
                Try
                    objDBWrite.ExecuteQuery(tmpSQL)
                Catch ex As Exception
                    'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
                    '      "('NO GUID','" & RG.Apos(tmpSQL) & "','IN SQL','" & RG.Apos(ex.Message) & "')"
                    'objDBWrite.ExecuteQuery(tmpSQL)

                    'If (objDBWrite IsNot Nothing) Then
                    '    objDBWrite.CloseConnection()
                    'End If

                    Exit Sub
                End Try
        End Select

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

    End Sub

    Public Sub AddStock(ByVal generated_code As String,
                         ByVal quantity As String, ByVal branch_code As String, Optional ByVal GRVorIBT As String = "")

        '        'Check if the code was originally inserted
        '        tmpSQL = "SELECT generated_code FROM stock_on_hand WHERE generated_code = '" & RG.Apos(generated_code) & "' " &
        '                 "AND branch_code = '" & RG.Apos(branch_code) & "'"
        '        Try
        '            ds = objDBRead.GetDataSet(tmpSQL)
        '            If objDBRead.isR(ds) Then
        '                'Record exists
        '                GoTo RecordExists
        '            End If

        '        Catch ex As Exception
        '            tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
        '                  "('NO GUID','" & RG.Apos(tmpSQL) & "','" & branch_code & "','" & RG.Apos(ex.Message) & "')"
        '            objDBWrite.ExecuteQuery(tmpSQL)

        '            If (objDBWrite IsNot Nothing) Then
        '                objDBWrite.CloseConnection()
        '            End If

        '            Exit Sub
        '        End Try

        '        tmpSQL = "INSERT INTO stock_on_hand (generated_code,branch_code) VALUES ('" & RG.Apos(Mid$(generated_code.ToUpper, 1, 30)) & "'," &
        '                                  "'" & branch_code.ToString.ToUpper & "')"
        '        Try
        '            objDBWrite.ExecuteQuery(tmpSQL)
        '        Catch ex As Exception
        '            objDBWrite.CloseConnection()

        '            tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
        '                  "('NO GUID','" & RG.Apos(tmpSQL) & "','" & branch_code & "','" & RG.Apos(ex.Message) & "')"
        '            objDBWrite.ExecuteQuery(tmpSQL)

        '            If (objDBWrite IsNot Nothing) Then
        '                objDBWrite.CloseConnection()
        '            End If

        '            Exit Sub
        '        End Try

        'RecordExists:


        tmpSQL = "UPDATE stock_on_hand SET qty_on_hand = qty_on_hand + " & quantity & ",updated = '" & Format(Now, "yyyy-MM-dd") & "' WHERE generated_code = '" & RG.Apos(generated_code.ToUpper) & "' AND branch_code = '" & RG.Apos(branch_code.ToUpper) & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
            '         "('NO GUID','" & RG.Apos(tmpSQL) & "','" & RG.Apos(branch_code.ToUpper) & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Exit Sub
        End Try

        If GRVorIBT = "GRV" Then
            tmpSQL = "UPDATE stockcodes_dates SET last_grv = '" & Format(Now, "yyyy-MM-dd") & "' WHERE generated_code = '" & RG.Apos(generated_code.ToUpper) & "'"
        ElseIf GRVorIBT = "IBT" Then
            tmpSQL = "UPDATE stockcodes_dates SET last_ibt = '" & Format(Now, "yyyy-MM-dd") & "' WHERE generated_code = '" & RG.Apos(generated_code.ToUpper) & "'"
        Else
            Exit Sub
        End If

        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
            '      "('NO GUID','" & RG.Apos(tmpSQL) & "','IN SQL','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Exit Sub
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

    End Sub

    Public Function StockEx(ByVal BranchCode As String, ByVal GeneratedCode As String,
                            ByVal ipAddress As String, ByVal ServerPath As String) As String

        Dim ReturnQTY As String = ""

        tmpSQL = "SELECT qty_on_hand FROM stock_on_hand WHERE generated_code = '" & RG.Apos(GeneratedCode) & "' " &
                 "AND branch_code = '" & RG.Apos(BranchCode) & "'"
        Try
            Ds = objDBRead.GetDataSet(tmpSQL, BranchCode)
            If objDBRead.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    ReturnQTY = dr("qty_on_hand") & ""
                Next
            Else
                'no record
                ReturnQTY = "Stockcode not found."
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",StockEx," & ex.Message & "," &
                                          tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            ReturnQTY = ex.Message
        End Try


        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return ReturnQTY

    End Function


    Public Function GetIBTOut(ByVal receiving_branch_code As String,
                              ByVal sending_branch_code As String,
                              ByVal IBTOutNumber As String,
                              ByVal ServerPath As String) As IBTOut

        Dim _IBT As New IBTOut
        Dim _LineItems As New List(Of IBTOutLineItems)

        tmpSQL = "SELECT " &
                 "SUM(received_qty) OVER () AS total_received," &
                 "stockcodes_master.generated_code," &
                 "stockcodes_master.description," &
                 "stockcodes_prices.selling_price_1 AS selling_price," &
                 "stockcodes_master.master_code," &
                 "stockcodes_master.is_service_item," &
                 "stockcodes_master.item_size," &
                 "stockcodes_master.item_colour," &
                 "stockcodes_master.size_matrix," &
                 "stockcodes_master.colour_matrix," &
                 "stockcodes_master.supplier," &
                 "stockcodes_master.suppliers_code," &
                 "stockcodes_prices.cost_price," &
                 "stockcodes_prices.estimated_cost AS average_cost_price," &
                 "stockcodes_categories.category_1," &
                 "stockcodes_categories.category_2," &
                 "stockcodes_categories.category_3," &
                 "ibt_transactions.sent_qty," &
                 "ibt_transactions.sending_branch_code," &
                 "ibt_transactions.selling_tax_group " &
                 "FROM ibt_transactions " &
                 "INNER JOIN stockcodes_master ON ibt_transactions.generated_code = stockcodes_master.generated_code " &
                 "INNER JOIN stockcodes_prices ON stockcodes_master.generated_code = stockcodes_prices.generated_code " &
                 "INNER JOIN stockcodes_categories ON stockcodes_master.generated_code = stockcodes_categories.generated_code " &
                 "WHERE ibt_transactions.sending_branch_code = '" & RG.Apos(sending_branch_code.ToUpper) & "' " &
                 "AND ibt_transactions.receiving_branch_code = '" & RG.Apos(receiving_branch_code.ToUpper) & "' " &
                 "AND ibt_transactions.transaction_number = '" & RG.Apos(IBTOutNumber) & "'"
        Try
            Ds = objDBRead.GetDataSet(tmpSQL, sending_branch_code)
            If objDBRead.isR(Ds) Then
                If Val(Ds.Tables(0).Rows(0)("total_received") & "") <> 0 Then
                    _IBT.strMessage = "This IBT has already been accepted"
                    If (objDBRead IsNot Nothing) Then
                        objDBRead.CloseConnection()
                    End If
                    Return _IBT
                End If
                For Each dr As DataRow In Ds.Tables(0).Rows
                    Dim _LineItem As New IBTOutLineItems
                    _LineItem.average_cost_price = dr("average_cost_price") & ""
                    _LineItem.category_1 = dr("category_1") & ""
                    _LineItem.category_2 = dr("category_2") & ""
                    _LineItem.category_3 = dr("category_3") & ""
                    _LineItem.colour_matrix = dr("colour_matrix") & ""
                    _LineItem.cost_price = dr("cost_price") & ""
                    _LineItem.description = dr("description") & ""
                    _LineItem.generated_code = dr("generated_code") & ""
                    _LineItem.is_service_item = dr("is_service_item") & ""
                    _LineItem.item_colour = dr("item_colour") & ""
                    _LineItem.item_size = dr("item_size") & ""
                    _LineItem.master_code = dr("master_code") & ""
                    _LineItem.selling_price = dr("selling_price") & ""
                    _LineItem.selling_tax_group = dr("selling_tax_group") & ""
                    _LineItem.sending_branch_code = dr("sending_branch_code") & ""
                    _LineItem.sent_qty = dr("sent_qty") & ""
                    _LineItem.size_matrix = dr("size_matrix") & ""
                    _LineItem.supplier = dr("supplier") & ""
                    _LineItem.suppliers_code = dr("suppliers_code") & ""
                    _LineItems.Add(_LineItem)
                Next
            Else
                'no record
                _IBT.strMessage = "IBT Out Not Found"
                Return _IBT
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetIBTOut," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & receiving_branch_code)
            End Using

            _IBT.strMessage = ex.Message
            Return _IBT
        End Try

        _IBT.LineItems = _LineItems
        _IBT.strMessage = "success"

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _IBT

    End Function

    Public Function EmployeeNoClockOut(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        tmpSQL = "UPDATE employee_details SET is_logged_in = False,last_guid = '', " &
                 "last_logout = '" & EmployeeData.last_logout & "' " &
                 "WHERE employee_number = '" & RG.Apos(EmployeeData.employee_number) & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, EmployeeData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & EmployeeData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""

        End Try

        tmpSQL = "UPDATE employee_transactions " &
                 "SET time_worked = 0 WHERE " &
                 "guid = '" & EmployeeData.last_guid & "" & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, EmployeeData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & EmployeeData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""

        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return EmployeeData.upload_guid

    End Function

    Public Function EmployeeLogOut(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String


        tmpSQL = "UPDATE employee_transactions SET clocking_time_out = '" & EmployeeData.clocking_time_out & "'," &
                 "clocking_date_out = '" & EmployeeData.clocking_date_out & "'," &
                 "clocking_hour_out = '" & EmployeeData.clocking_hour_out & "',is_sunday = " & EmployeeData.is_sunday & "," &
                 "is_public_holiday = " & EmployeeData.is_public_holiday & ",time_worked = " & RG.Numb(EmployeeData.time_worked) & " " &
                 "WHERE guid = '" & EmployeeData.last_guid & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, EmployeeData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & EmployeeData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""

        End Try

        tmpSQL = "UPDATE employee_details SET is_logged_in = False,last_guid = '', " &
                 "last_logout = '" & EmployeeData.last_logout & "' " &
                 "WHERE employee_number = '" & RG.Apos(EmployeeData.employee_number) & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, EmployeeData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & EmployeeData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return EmployeeData.upload_guid

    End Function

    Public Function LastUpdate(ByVal ShopVersion As String, ByVal HOVersion As String,
                               ByVal UploaderVersion As String, ByVal ConfigVersion As String,
                               ByVal LastError As String, ByVal Queries As String,
                               ByVal IPAddess As String, ByVal BranchCode As String) As String

        tmpSQL = "UPDATE version_numbers SET last_update_time = '" & Format(Now, "HH:mm:ss") & "', last_update_date = '" & Format(Now, "yyyy-MM-dd") & "'," &
                 "positive_shop_version = '" & ShopVersion & "',positive_ho_version = '" & HOVersion & "'," &
                 "uploader_version = '" & UploaderVersion & "',config_version = '" & ConfigVersion & "'," &
                 "last_error = '" & LastError & "',queries_uploaded = '" & Queries & "',ip_address = '" & IPAddess & "' WHERE branch_code = '" & BranchCode & "'"

        Try
            objDBWrite.ExecuteQuery(tmpSQL, BranchCode)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return "success"

    End Function

    Public Function EmployeeLogIn(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        tmpSQL = "INSERT INTO employee_transactions (guid,employee_number,branch_code,clocking_time_in,clocking_date_in," &
                 "clocking_hour_in,user_name) VALUES ('" & EmployeeData.guid & "','" & RG.Apos(EmployeeData.employee_number) & "'," &
                 "'" & EmployeeData.branch_code & "','" & EmployeeData.clocking_time_in & "'," &
                 "'" & EmployeeData.clocking_date_in & "','" & EmployeeData.clocking_hour_in & "'," &
                 "'" & EmployeeData.user_name & "')"

        Try
            objDBWrite.ExecuteQuery(tmpSQL, EmployeeData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & EmployeeData.guid & "','" & RG.Apos(tmpSQL) & "','" & EmployeeData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        tmpSQL = "UPDATE employee_details SET is_logged_in = True,last_guid = '" & EmployeeData.guid & "', " &
                 "last_login = '" & EmployeeData.last_login & "' WHERE employee_number = '" & RG.Apos(EmployeeData.employee_number) & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, EmployeeData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & EmployeeData.guid & "','" & RG.Apos(tmpSQL) & "','" & EmployeeData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return EmployeeData.upload_guid

    End Function

    Public Function EmployeeUpdateTimeWorked(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        tmpSQL = "UPDATE employee_transactions SET " &
                 "time_worked = " & EmployeeData.time_worked & "' " &
                 "WHERE guid = '" & EmployeeData.last_guid & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, EmployeeData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & EmployeeData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return EmployeeData.upload_guid

    End Function

    Public Function EmployeeAbsent(ByVal EmployeeData As Employee, ByVal ServerPath As String) As String

        tmpSQL = "INSERT INTO employee_transactions (guid,employee_number,branch_code,clocking_time_in,clocking_date_in," &
                 "clocking_hour_in,clocking_time_out,clocking_date_out,clocking_hour_out,is_sunday,is_public_holiday," &
                 "time_worked,reason,user_name) VALUES ('" & EmployeeData.last_guid & "','" & RG.Apos(EmployeeData.employee_number) & "'," &
                 "'" & EmployeeData.branch_code & "',now(),'" & EmployeeData.clocking_date_in & "','" & EmployeeData.clocking_hour_in & "'," &
                 "now(),'" & EmployeeData.clocking_date_out & "','" & EmployeeData.clocking_hour_out & "'," & EmployeeData.is_sunday & "," &
                 EmployeeData.is_public_holiday & ",'0','" & RG.Apos(EmployeeData.reason) & "','" & EmployeeData.user_name & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, EmployeeData.branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & EmployeeData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If
                Return EmployeeData.upload_guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return EmployeeData.upload_guid

    End Function


    Public Function UpdateStationaryCount(ByVal TransactionData As StationaryTransactionData, ByVal ServerPath As String) As String


        tmpSQL = "INSERT INTO stationary_transactions (item_code,quantity,transaction_type,branch_code) VALUES " &
                     "('" & TransactionData.item_code & "','" & TransactionData.received_qty & "','COUNT','" & TransactionData.BranchCode & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, TransactionData.BranchCode)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '      "('" & TransactionData.Guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return TransactionData.Guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                Return TransactionData.Guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return TransactionData.Guid

    End Function

    Public Function UpdateIBTTransactions(ByVal TransactionData As IBTTransactionData, ByVal ServerPath As String) As String

        tmpSQL = "UPDATE ibt_transactions SET received_qty = '" & TransactionData.received_qty & "',receiving_date = '" & TransactionData.receiving_date & "' " &
                 "WHERE transaction_number = '" & TransactionData.transaction_number & "' AND receiving_branch_code = '" & TransactionData.receiving_branch_code & "' " &
                 "AND generated_code = '" & TransactionData.generated_code & "' AND sending_branch_code = '" & TransactionData.sending_branch_code.ToUpper & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL, TransactionData.sending_branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '       "('" & TransactionData.Guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.receiving_branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return TransactionData.Guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                Return TransactionData.Guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If Mid$(TransactionData.generated_code, 1, 2) = "##" Then
            tmpSQL = "INSERT INTO stationary_transactions (item_code,quantity,transaction_type,branch_code) VALUES " &
                     "('" & TransactionData.generated_code & "','" & TransactionData.received_qty & "','IBTIN','" & TransactionData.receiving_branch_code & "')"
            Try
                objDBWrite.ExecuteQuery(tmpSQL, TransactionData.sending_branch_code)
            Catch ex As Exception

                'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
                '    "('" & TransactionData.Guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.receiving_branch_code & "','" & RG.Apos(ex.Message) & "')"
                'objDBWrite.ExecuteQuery(tmpSQL)

                If ex.Message.ToString.Contains("duplicate key value") Then
                    Return TransactionData.Guid
                End If

                If ex.Message.ToString.Contains("violates foreign key constraint") Then
                    Return TransactionData.Guid
                End If

                If (objDBWrite IsNot Nothing) Then
                    objDBWrite.CloseConnection()
                End If

                Return ""
            End Try
        End If

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return TransactionData.Guid

    End Function

    Public Function InsertIBTTransactions(ByVal TransactionData As IBTTransactionData, ByVal ServerPath As String) As String

        tmpSQL = "INSERT INTO ibt_transactions (guid,sending_branch_code,sending_date,transaction_number,receiving_branch_code,generated_code," &
                 "sent_qty,selling_tax_group) VALUES " &
                 "('" & TransactionData.Guid & "','" & RG.Apos(TransactionData.sending_branch_code.ToUpper) & "','" & TransactionData.sending_date & "'," &
                 "'" & TransactionData.transaction_number & "','" & RG.Apos(TransactionData.receiving_branch_code.ToUpper) & "'," &
                 "'" & RG.Apos(TransactionData.generated_code) & "','" & RG.Numb(TransactionData.sent_qty) & "'," &
                 "'" & TransactionData.selling_tax_group & "')"

        Try
            objDBWrite.ExecuteQuery(tmpSQL, TransactionData.sending_branch_code)
        Catch ex As Exception

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '       "('" & TransactionData.Guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.receiving_branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return TransactionData.Guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                Return TransactionData.Guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

        Return TransactionData.Guid

    End Function

    Public Function ProcessLineItems(ByVal TransactionData As TransactionLineItems, ByVal ServerPath As String) As String

        '===========================================================================================================================================
        'UPDATE COST AVERAGE
        '===========================================================================================================================================
        If Val(TransactionData.updated_cost) <> 0 Then 'This should never be run at the store version. Using the same Module as per HO
            If Val(TransactionData.updated_cost) <> Val(TransactionData.cost_exclusive_average) Then
                Dim tmpTotalQtyDBL As Double
                tmpSQL = "SELECT sum(qty_on_hand) FROM stock_on_hand WHERE generated_code = '" & RG.Apos(TransactionData.generated_code.ToUpper) & "'"
                Try
                    Ds = objDBRead.GetDataSet(tmpSQL, TransactionData.branch_code)
                    If objDBRead.isR(Ds) Then
                        For Each dr As DataRow In Ds.Tables(0).Rows
                            tmpTotalQtyDBL = Ds.Tables(0).Rows(0)("sum") & ""
                        Next
                    End If

                Catch ex As Exception
                    Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                        tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",ProcessLineItems," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & TransactionData.branch_code)
                    End Using

                    'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
                    '         "('" & TransactionData.Guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.branch_code & "','" & RG.Apos(ex.Message) & "')"
                    'objDBWrite.ExecuteQuery(tmpSQL)

                    If ex.Message.ToString.Contains("duplicate key value") Then
                        Return TransactionData.Guid
                    End If

                    If ex.Message.ToString.Contains("violates foreign key constraint") Then
                        Return TransactionData.Guid
                    End If

                    If (objDBRead IsNot Nothing) Then
                        objDBRead.CloseConnection()
                    End If

                    Return ""
                End Try

                Dim CostAverage As Double
                Dim NewCost As Double
                'Don't need to worry about getting a negative value for the tQtyPositiveOrNegative here, as the only time
                'tQtyPositiveOrNegative will  be Negative is with POS Refunds and this will only run for GRVs
                CostAverage = TransactionData.updated_cost * Val(TransactionData.quantity)
                NewCost = Val(TransactionData.cost_exclusive_average) * tmpTotalQtyDBL
                NewCost = CostAverage + Val(NewCost)
                NewCost = Val(NewCost) / Val(Val(TransactionData.quantity) + tmpTotalQtyDBL)
                'Do the update
                'Not getting the original estimated cost, so no way to compare if the estimated cost has been updated.
                'So, only updating when average cost has been changed.
                tmpSQL = "UPDATE stockcodes_prices SET cost_price = '" & RG.Numb(NewCost) & "',estimated_cost = '" & TransactionData.updated_cost & "'" &
                          "WHERE generated_code = '" & RG.Apos(TransactionData.generated_code.ToUpper) & "'"
                Try
                    objDBWrite.ExecuteQuery(tmpSQL, TransactionData.branch_code)
                Catch ex As Exception
                    Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                        tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",ProcessLineItems," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & TransactionData.branch_code)
                    End Using

                    'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
                    '         "('" & TransactionData.Guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.branch_code & "','" & RG.Apos(ex.Message) & "')"
                    'objDBWrite.ExecuteQuery(tmpSQL)

                    If ex.Message.ToString.Contains("duplicate key value") Then
                        Return TransactionData.Guid
                    End If

                    If ex.Message.ToString.Contains("violates foreign key constraint") Then
                        Return TransactionData.Guid
                    End If

                    If (objDBRead IsNot Nothing) Then
                        objDBRead.CloseConnection()
                    End If

                    If (objDBWrite IsNot Nothing) Then
                        objDBWrite.CloseConnection()
                    End If

                    Return ""
                End Try

                'For GRV purpose we need to insert the current cost for the transaction
                TransactionData.cost_exclusive_average = TransactionData.updated_cost
            End If

        End If
        '==============================================================


        '****************************************************************************************************
        'NORMAL CODES
        '****************************************************************************************************
        tmpSQL = "INSERT INTO transaction_line_items " &
                 "(guid,
                 link_guid,
                 master_code,
                 generated_code,
                 description,
                 tax_group,
                 tax_percentage,
                 supplier,
                 suppliers_code,
                 category_1,
                 category_2," &
                 "category_3,
                 estimated_cost_price,
                 average_cost_price,
                 selling_price,
                 discount_per_item,
                 discount_reason,
                 quantity,
                 item_size," &
                 "item_size_grid,
                 item_colour,
                 item_colour_grid,
                 is_service_item,
                 branch_code,
                 transaction_number,
                 transaction_type) VALUES " &
                 "('" & TransactionData.Guid & "','" & TransactionData.Link_GuID & "','" & RG.Apos(TransactionData.master_code.ToUpper) & "'," &
                 "'" & RG.Apos(TransactionData.generated_code.ToUpper) & "','" & RG.Apos(TransactionData.description) & "'," &
                 "'" & TransactionData.tax_group & "',NULLIF('" & RG.FormatCurrency(TransactionData.tax_percentage) & "','')::numeric,'" & RG.Apos(TransactionData.supplier_account.ToUpper) & "'," &
                 "'" & RG.Apos(TransactionData.supplier_item_code) & "'," &
                 "'" & RG.Apos(TransactionData.category_1.ToUpper) & "','" & RG.Apos(TransactionData.category_2.ToUpper) & "'," &
                 "'" & RG.Apos(TransactionData.category_3.ToUpper) & "',NULLIF('" & RG.FormatCurrency(TransactionData.cost_exclusive_estimated) & "','')::numeric,NULLIF(" &
                 "'" & RG.FormatCurrency(TransactionData.cost_exclusive_average) & "','')::numeric,NULLIF('" & RG.FormatCurrency(TransactionData.selling_exclusive) & "','')::numeric,NULLIF(" &
                 "'" & RG.Numb(TransactionData.discount_value) & "','')::numeric,'" & Mid$(RG.Apos(TransactionData.discount_reason), 1, 150) & "',NULLIF(" &
                 "'" & RG.FormatCurrency(TransactionData.quantity) & "','')::numeric,'" & TransactionData.size & "','" & TransactionData.size_grid & "'," &
                 "'" & TransactionData.colour & "','" & TransactionData.colour_grid & "',NULLIF('" & TransactionData.is_service_item & "','')::bool," &
                 "'" & TransactionData.branch_code & "','" & TransactionData.transaction_number & "','" & TransactionData.transsaction_type & "')"

        Try
            objDBWrite.ExecuteQuery(tmpSQL, TransactionData.branch_code)
        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",ProcessLineItems," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & TransactionData.branch_code)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
            '         "('" & TransactionData.Guid & "','" & RG.Apos(tmpSQL) & "','" & TransactionData.branch_code & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            If ex.Message.ToString.Contains("duplicate key value") Then
                Return TransactionData.Guid
            End If

            If ex.Message.ToString.Contains("violates foreign key constraint") Then
                Return TransactionData.Guid
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If

            Return ""
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If
        'Add and subtract qtys. Add / subtract to bestsellers
        'For service items, must add to bestsellers but don't add / subtract stock qtys

        'NB: The only time tQtyPositiveOrNegative will be negative is with POS Refunds, so we need to take care of it


        '=============================================================================================================
        '2017-08-07
        'The adding and removing of stock quantities are now done by a database trigger
        '=============================================================================================================

        'If TransactionData.affect_quantities = True Then
        '    If TransactionData.transsaction_type = "POSSALE" Then
        '        'Add Stock to Qty
        '        If TransactionData.is_service_item = False Then
        '           RemStock(TransactionData.generated_code, TransactionData.quantity, TransactionData.branch_code, "INV")
        '        End If
        '        'Add the history
        '        AddHistory("SALE", TransactionData.quantity, TransactionData.generated_code)
        '    ElseIf TransactionData.transsaction_type = "POSCN" Then
        '        'Add the stk qty
        '        If TransactionData.is_service_item = False Then
        '            AddStock(TransactionData.generated_code, Val(TransactionData.quantity) * -1, TransactionData.branch_code, "RETURN")
        '        End If
        '        'Add the history
        '        AddHistory("RETURN", Val(TransactionData.quantity) * -1, TransactionData.generated_code)
        '    ElseIf TransactionData.transsaction_type = "POSREF" Then
        '        'Add the stk qty
        '        If TransactionData.is_service_item = False Then
        '            AddStock(TransactionData.generated_code, Val(TransactionData.quantity) * -1, TransactionData.branch_code, "RETURN")
        '        End If
        '        'Add the history
        '        AddHistory("RETURN", Val(TransactionData.quantity) * -1, TransactionData.generated_code)
        '    ElseIf TransactionData.transsaction_type = "IBTIN" Then
        '        'Add the stk qty
        '        If TransactionData.is_service_item = False Then
        '            AddStock(TransactionData.generated_code, TransactionData.quantity, TransactionData.branch_code, "IBT")
        '        End If
        '    ElseIf TransactionData.transsaction_type = "IBTOUT" Then
        '        'Remove the stk qty
        '        If TransactionData.is_service_item = False Then
        '            RemStock(TransactionData.generated_code, TransactionData.quantity, TransactionData.branch_code, "IBT")
        '        End If
        '    ElseIf TransactionData.transsaction_type = "GRV" Then
        '        'Add the stk qty
        '        If TransactionData.is_service_item = False Then
        '            AddStock(TransactionData.generated_code, TransactionData.quantity, TransactionData.branch_code, "GRV")
        '        End If
        '        'No history. Only sales and returns
        '    ElseIf TransactionData.transsaction_type = "STOCKTAKE+" Then 'Stocktake can go either was do we'll need to mid$ later on
        '        'Codes must be zero'd before this procedure can work
        '        'Add the stk qty
        '        If TransactionData.is_service_item = False Then
        '            AddStock(TransactionData.generated_code, TransactionData.quantity, TransactionData.branch_code)
        '        End If
        '    ElseIf TransactionData.transsaction_type = "STOCKTAKE-" Then 'Stocktake can go either was do we'll need to mid$ later on
        '        'Codes must be zero'd before this procedure can work
        '        'Remove the stk qty
        '        If TransactionData.is_service_item = False Then
        '            RemStock(TransactionData.generated_code, TransactionData.quantity, TransactionData.branch_code)
        '        End If
        '    ElseIf TransactionData.transsaction_type = "STKADJ+" Then 'Stock adjustments can go either was do we'll need to mid$ later on
        '        'Add the stk qty
        '        If TransactionData.is_service_item = False Then
        '            AddStock(TransactionData.generated_code, TransactionData.quantity, TransactionData.branch_code)
        '        End If
        '    ElseIf TransactionData.transsaction_type = "STKADJ-" Then 'Stock adjustments can go either was do we'll need to mid$ later on
        '        'Remove the stk qty
        '        If TransactionData.is_service_item = False Then
        '            RemStock(TransactionData.generated_code, TransactionData.quantity, TransactionData.branch_code)
        '        End If
        '    End If
        'End If

        Return TransactionData.Guid

    End Function

    Public Function CreditCardAuth(ByVal CreditCardAuthoristaion As CreditCardAuth, ByVal ServerPath As String) As String

        'Dim dataSerialized = JsonConvert.SerializeObject(CreditCardAuthoristaion.data)
        'Dim recieptSerialized = JsonConvert.SerializeObject(CreditCardAuthoristaion.receipt)

        'tmpSQL = "INSERT INTO credit_card_auth (" &
        '            "transaction_guid,sale_date,sale_time,branch_code,tran,transaction_type,transaction_number,amount,response_code,response_text,reference," &
        '            "seq,rrn,pos,store,card,card_name,card_entry,sign,cryptogram_type,cryptogram,apl,tvr,tsi,cvm_results," &
        '            "iad,pin_statement,data,receipt,merchant_id,aid) " &
        '            " VALUES('" & CreditCardAuthoristaion.transaction_guid & "','" & CreditCardAuthoristaion.sale_date & "','" & CreditCardAuthoristaion.sale_time & "'," &
        '            "'" & Mid$(RG.Apos(CreditCardAuthoristaion.branch_code), 1, 5) & "'," &
        '            "'" & Mid$(RG.Apos(CreditCardAuthoristaion.tran), 1, 5) & "','" & Mid$(RG.Apos(CreditCardAuthoristaion.transaction_type), 1, 8) & "'," &
        '            "'" & Mid$(RG.Apos(CreditCardAuthoristaion.transaction_number), 1, 15) & "'," &
        '            "'" & RG.Apos(RG.Numb(CreditCardAuthoristaion.amount)) & "','" & RG.Apos(Mid$(CreditCardAuthoristaion.response_code, 1, 5)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.response_text, 1, 250)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.reference, 1, 55)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.seq, 1, 18)) & "','" & RG.Apos(Mid(CreditCardAuthoristaion.rrn, 1, 48)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.pos, 1, 18)) & "','" & RG.Apos(Mid(CreditCardAuthoristaion.store, 1, 48)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.card, 1, 38)) & "','" & RG.Apos(Mid(CreditCardAuthoristaion.card_name, 1, 98)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.card_entry, 1, 5)) & "','" & CreditCardAuthoristaion.sign & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.cryptogram_type, 1, 10)) & "','" & RG.Apos(Mid(CreditCardAuthoristaion.cryptogram, 1, 18)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.apl, 1, 48)) & "','" & RG.Apos(Mid(CreditCardAuthoristaion.tvr, 1, 48)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.tsi, 1, 10)) & "','" & RG.Apos(Mid(CreditCardAuthoristaion.cvm_results, 1, 18)) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.iad, 1, 98)) & "','" & RG.Apos(Mid(CreditCardAuthoristaion.pin_statement, 1, 48)) & "'," &
        '            "'" & RG.Apos(dataSerialized) & "','" & RG.Apos(recieptSerialized) & "'," &
        '            "'" & RG.Apos(Mid(CreditCardAuthoristaion.merchant_id, 1, 18)) & "','" & RG.Apos(Mid(CreditCardAuthoristaion.aid, 1, 48)) & "')"

        'Try
        '    objDBWrite.ExecuteQuery(tmpSQL, CreditCardAuthoristaion.branch_code)
        'Catch ex As Exception

        '    'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " & _
        '    '      "('" & CashTransaction.transaction_guid & "','" & RG.Apos(tmpSQL) & "','" & CashTransaction.branch_code & "','" & RG.Apos(ex.Message) & "')"
        '    'objDBWrite.ExecuteQuery(tmpSQL)

        '    If ex.Message.ToString.Contains("duplicate key value") Then
        '        Return CreditCardAuthoristaion.transaction_guid
        '    End If

        '    If ex.Message.ToString.Contains("violates foreign key constraint") Then
        '        Return CreditCardAuthoristaion.transaction_guid
        '    End If

        '    If (objDBWrite IsNot Nothing) Then
        '        objDBWrite.CloseConnection()
        '    End If

        '    Return ""
        'End Try

        'If (objDBWrite IsNot Nothing) Then
        '    objDBWrite.CloseConnection()
        'End If

        Return CreditCardAuthoristaion.transaction_guid

    End Function

    '' Do Stock Adjustments
    Public Function StockAdjustments(ByVal transactionLineItems As StockTransactions)
        Dim qty As String = String.Empty
        Dim tmpGUID As String
        tmpGUID = Guid.NewGuid.ToString()

        Dim tmpTaxGroup As String       'Retrieved from tmpTaxArray (split from TaxGroupForNonValidStock)
        Dim tmpTaxPercentage As String  'Retrieved from tmpTaxArray (split from TaxGroupForNonValidStock)
        Dim tmpTaxArray() As String

        Try
            'If transactionLineItems.TransType = "STKADJ+" Then
            '    AddStock(transactionLineItems.GCode, transactionLineItems.tQtyPositiveOnly, transactionLineItems.BranchCode)
            'Else
            '    RemStock(transactionLineItems.GCode, transactionLineItems.tQtyPositiveOnly, transactionLineItems.BranchCode)
            'End If

            If Mid$(transactionLineItems.TransType, Len(transactionLineItems.TransType), 1) = "-" Then
                qty = Val(Val(transactionLineItems.tQtyPositiveOnly) * -1)
            Else
                qty = transactionLineItems.tQtyPositiveOnly
            End If

            tmpTaxArray = Split(transactionLineItems.Tax, " - ")
            tmpTaxGroup = tmpTaxArray(0)
            tmpTaxArray = Split(transactionLineItems.Tax, "(")
            tmpTaxPercentage = Mid(tmpTaxArray(1), 1, Len(tmpTaxArray(1)) - 1)

            If transactionLineItems.OriginalEstimatedCost = "" Then
                transactionLineItems.OriginalEstimatedCost = transactionLineItems.OriginalAvgCost
            End If


            tmpSQL = "INSERT INTO transaction_line_items " &
                 "(
                  guid,
                 link_guid,
                 master_code,
                 generated_code,
                 description,
                 tax_group,
                 tax_percentage,
                 supplier,
                 suppliers_code,
                 category_1,
                 category_2,
                 " &
                 "category_3,
                 estimated_cost_price,
                 average_cost_price,
                 selling_price,
                 discount_per_item,
                 discount_reason,
                 quantity,
                 item_size,
                 " &
                 "item_size_grid,
                 item_colour,
                 item_colour_grid,
                 is_service_item,
                  branch_code,
                 transaction_number,
                 transaction_type
                 ) 
                 VALUES
                 " &
                 "('" & RG.Apos(tmpGUID) & "',
                 '" & RG.Apos(transactionLineItems.tGuID) & "',
                 '" & RG.Apos(transactionLineItems.MCode.ToUpper) & "',
                 '" & RG.Apos(transactionLineItems.GCode.ToUpper) & "',
                 '" & RG.Apos(transactionLineItems.DDescription) & "',
                 " &
                 "'" & tmpTaxGroup & "',
                 '" & tmpTaxPercentage & "',
                 '" & RG.Apos(transactionLineItems.DSupplier.ToUpper) & "',
                 '" & RG.Apos(transactionLineItems.DSupplierItemCode) & "',
                 " &
                 "'" & RG.Apos(transactionLineItems.DCategory1.ToUpper) & "',
                 '" & RG.Apos(transactionLineItems.DCategory2.ToUpper) & "',
                 '" & RG.Apos(transactionLineItems.DCategory3.ToUpper) & "',
                 '" & RG.Numb(transactionLineItems.OriginalEstimatedCost) & "',
                 " &
                 "'" & RG.Numb(transactionLineItems.OriginalAvgCost) & "',
                 '" & RG.Numb(transactionLineItems.SellEx) & "',
                 '" & RG.Numb(transactionLineItems.DiscountAmount) & "',
                 '" & Mid$(RG.Apos(transactionLineItems.DiscountReason), 1, 150) & "'," &
                 "'" & RG.Numb(qty) & "',
                 '" & transactionLineItems.DItemSize & "',
                 '" & transactionLineItems.DItemSizeGrid & "',
                 '" & transactionLineItems.DItemColour & "',
                 '" & transactionLineItems.DItemColourGrid & "',
                 '" & transactionLineItems.DisServiceItem & "',
                 '" & transactionLineItems.BranchCode & "',
                 '" & transactionLineItems.TransactionNumber & "',
                 '" & transactionLineItems.TransType & "'
                 )"

            objDBWrite.ExecuteQuery(tmpSQL, transactionLineItems.BranchCode)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return ""
        End Try
        Return transactionLineItems.tGuID
    End Function
End Class

