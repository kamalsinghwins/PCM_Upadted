Imports System.IO
Imports Npgsql

Public Class GeneralHODL
    Inherits DataAccessLayerBase

    Dim ds2 As DataSet

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing

    'Public Sub New(ByVal CompanyCode As String)
    '    objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite", "pos_" & CompanyCode)
    '    objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead", "pos_" & CompanyCode)
    '    connection = Me.DataBase("PostgreConnectionStringPositiveRead", "pos_" & CompanyCode)

    'End Sub

    Public Sub New()
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = Me.DataBase("PostgreConnectionStringPositiveRead")

    End Sub

    Public Function GenerateSKU(Optional CompanyCode As String = "") As String
TryAgain:

        Dim nSKUNumber As String = ""

        'If Len("600" & nSKUNum & "00" & getEAN13("600" & nSKUNum & "00")) = 14 Then
        '    GoTo TryAgain:
        'End If

        tmpSQL = "SELECT nextval('barcodes_seq')"
        Try
            'Ds = objDBWrite.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSWriteConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    nSKUNumber = dr("nextval") & ""
                Next
            End If
        Catch ex As Exception
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
            Return Nothing
        End Try

        GenerateSKU = nSKUNumber & FillString("0", 12 - Len(nSKUNumber)) & getEAN13(nSKUNumber & FillString("0", 12 - Len(nSKUNumber)))
        'Debug.Print GenSKU

        'Check if exists
        tmpSQL = "SELECT sku_number FROM stockcodes_master WHERE sku_number = '" & GenerateSKU & "'"
        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    GoTo TryAgain
                Next
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return Nothing
        End Try

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

    End Function

    Public Function getEAN13(ByRef sCodice As String) As String

        Dim EANLOOP As Long
        Dim SKUArray(12) As String
        Dim tmpEven As String
        Dim tmpOdd As String
        Dim tmpTotal As String

        For EANLOOP = 1 To 12
            SKUArray(EANLOOP) = Mid$(sCodice, EANLOOP, 1)
        Next EANLOOP

        tmpOdd = Val(SKUArray(12)) + Val(SKUArray(10)) + Val(SKUArray(8)) + Val(SKUArray(6)) + Val(SKUArray(4)) + Val(SKUArray(2))
        tmpOdd = Val(tmpOdd) * 3

        tmpEven = Val(SKUArray(11)) + Val(SKUArray(9)) + Val(SKUArray(7)) + Val(SKUArray(5)) + Val(SKUArray(3)) + Val(SKUArray(1))

        tmpTotal = Val(tmpOdd) + Val(tmpEven)
        tmpTotal = Mid$(tmpTotal, Len(tmpTotal), 1)

        getEAN13 = ""

        If Val(tmpTotal) = 1 Then getEAN13 = "9"
        If Val(tmpTotal) = 2 Then getEAN13 = "8"
        If Val(tmpTotal) = 3 Then getEAN13 = "7"
        If Val(tmpTotal) = 4 Then getEAN13 = "6"
        If Val(tmpTotal) = 5 Then getEAN13 = "5"
        If Val(tmpTotal) = 6 Then getEAN13 = "4"
        If Val(tmpTotal) = 7 Then getEAN13 = "3"
        If Val(tmpTotal) = 8 Then getEAN13 = "2"
        If Val(tmpTotal) = 9 Then getEAN13 = "1"
        If Val(tmpTotal) = 0 Then getEAN13 = "0"

        Return getEAN13
        'Return "Error in getEAN13 " & vbCrLf & Err.Number & vbCrLf & Err.Description

    End Function

    Public Function FillString(
    ByVal strChar As String,
    ByVal lngCount As Long) _
    As String
        ' Comments  : Generates a string of repeated characters
        ' Parameters: strChar - Character to repeat
        '             lngCount - Number of characters
        ' Returns   : String of repeated characters
        ' Source    : Total VB SourceBook 6
        '
        Dim lngCounter As Long
        Dim strTmp As String

        ' Add the specified character x times
        For lngCounter = 1 To lngCount
            strTmp = strTmp & strChar
        Next lngCounter

        ' Return the value
        FillString = strTmp

PROC_EXIT:
        Exit Function

    End Function

    Public Function BankReconReportDetails(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT " &
                     "sale_date," &
                     "branch_code, " &
                     "branch_name, " &
                     "cash_positive, " &
                     "credit_card_positive, " &
                     "voucher_positive, " &
                     "credit_card_transactions_positive, " &
                     "credit_card_transactions_bank, " &
                     "cash_bank, " &
                     "credit_card_bank, " &
                     "cash_transactions_positive, " &
                     "(cash_positive - cash_bank) cash_diff, " &
                     "(credit_card_positive - credit_card_bank) credit_card_diff, " &
                     "(credit_card_transactions_positive - credit_card_transactions_bank) eft_diff, " &
                     "(cash_positive + credit_card_positive - cash_bank - credit_card_bank) total_diff, " &
                     "comment commentt " &
                     "FROM  " &
                     "cashup_summary " &
                     "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                     "ORDER BY sale_date"

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

    Public Function BankReconReport(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "SELECT " &
                     "cashup_summary.branch_code, " &
                     "MAX(branch_name) branch_name, " &
                     "SUM(cash_positive) cash_positive, " &
                     "SUM(credit_card_positive) credit_card_positive, " &
                     "SUM(voucher_positive) voucher_positive, " &
                     "SUM(credit_card_transactions_positive) credit_card_transactions_positive, " &
                     "SUM(credit_card_transactions_bank) credit_card_transactions_bank, " &
                     "SUM(cash_bank) cash_bank, " &
                     "SUM(credit_card_bank) credit_card_bank, " &
                     "SUM(cash_transactions_positive) cash_transactions_positive, " &
                     "SUM(cash_positive) - SUM(cash_bank) cash_diff, " &
                     "SUM(credit_card_positive) - SUM(credit_card_bank) credit_card_diff, " &
                     "SUM(credit_card_transactions_positive)  - SUM(credit_card_transactions_bank) eft_diff, " &
                     "SUM(cash_positive) + SUM(credit_card_positive) - SUM(cash_bank) - SUM(credit_card_bank) total_diff, " &
                     "MAX(comment) commentt " &
                     "FROM  " &
                     "cashup_summary " &
                     "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                     "GROUP BY branch_code " &
                     "ORDER BY branch_code"

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

    Public Function RunStatementUpload(ByVal FileName As String, ByVal TypeOfFile As String) As String

        Dim isSuccess As Boolean = True
        Dim ErrorMessage As String = ""



        If TypeOfFile = "EFT Statement" Then
            Return ProcessEFTStatement(FileName)
        ElseIf TypeOfFile = "ABSA Statement" Then
            Return ProcessABSAStatement(FileName)
        ElseIf TypeOfFile = "Standard Bank Statement" Then
            Return ProcessStandardBankStatement(FileName)
        ElseIf TypeOfFile = "FNB Statement" Then
            Return ProcessFNBStatement(FileName)
        ElseIf TypeOfFile = "Nedbank Statement" Then
            Return ProcessNedbankStatement(FileName)
        End If

        Return Nothing

    End Function

    Private Function ProcessNedbankStatement(ByVal FileName As String) As String

        Dim isSuccess As Boolean = True
        Dim ErrorMessage As String = ""

        Dim ReturnStatus As String = ""

        Dim line As String

        Dim linenumber As Integer = 0

        tmpSQL = "DELETE FROM merchant_report_working"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        Dim tmpBranchName As String
        Dim tmpDepositAmount As String
        Dim tmpDepositDate As String

        Dim stringdataline1(100) As String


        Try
            Using sr As New StreamReader(FileName)

                ' Read and display lines from the file until the end of
                ' the file is reached.

                Do
                    line = sr.ReadLine()
                    If Not (line Is Nothing) Then
                        '--------------------------------------------
                        'Lines 1 (0 in read) is the heading
                        stringdataline1 = line.Split(",")

                        '   0       1      2     3        4
                        '      1234567890              
                        '70893,02/05/2017,HC1,14630.93,1203477.52
                        If stringdataline1.Count > 3 Then

                            'Don't process fees, etc
                            If Val(stringdataline1(3)) > 0 Then
                                tmpBranchName = Trim(stringdataline1(2).Replace(" ", "")).ToString.ToUpper
                                If CheckRecordExists("branch_details", "branch_code", tmpBranchName) = True Then
                                    'All ok
                                ElseIf CheckRecordExists("branch_details", "branch_code", Mid(tmpBranchName, Len(tmpBranchName) - 2, 3)) = True Then
                                    tmpBranchName = Mid(tmpBranchName, Len(tmpBranchName) - 2, 3)
                                ElseIf CheckRecordExists("branch_details", "branch_code",
                                                         mid$(Trim(stringdataline1(2).Replace(" ", "")).ToString.ToUpper, 1, 3)) = True Then
                                    tmpBranchName = Mid(Trim(stringdataline1(2).Replace(" ", "")).ToString.ToUpper, 1, 3)
                                Else
                                    GoTo DoesNotExist
                                End If

                                tmpDepositAmount = RG.Numb(stringdataline1(3))
                                tmpDepositDate = Mid(stringdataline1(1), 7, 4) & "-" & Mid(stringdataline1(1), 4, 2) & "-" & Mid(stringdataline1(1), 1, 2)

                                Dim tdate As Date = DateTime.Parse(tmpDepositDate)

                                tmpDepositDate = Format(tdate.Date.AddDays(-1), "yyyy-MM-dd")

                                'Check if branch code exists
                                If CheckRecordExists("branch_details", "branch_code", tmpBranchName) = True Then
                                    tmpSQL = "INSERT INTO merchant_report_working (sale_date,branch_code,transaction_amount) " &
                                             "VALUES ('" & tmpDepositDate & "','" & tmpBranchName & "','" & tmpDepositAmount & "')"
                                    Try
                                        'objDBWrite.ExecuteQuery(tmpSQL)
                                        usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                    Catch ex As Exception
                                        'objDBWrite.CloseConnection()
                                        'objDBRead.CloseConnection()
                                        Return ex.Message
                                    End Try
                                End If
                            End If


                        End If
                    End If
DoesNotExist:
                    linenumber += 1
                Loop Until line Is Nothing
            End Using
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            isSuccess = False
            ErrorMessage &= "Error in line " & linenumber & " \n"
        End Try

        tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date FROM merchant_report_working GROUP BY sale_date"
        'Check for mutiple dates
        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'Work through the dates
                    tmpSQL = "SELECT branch_code," &
                             "SUM(transaction_amount) as t_sum," &
                             "COUNT(transaction_amount) as t_count " &
                             "FROM merchant_report_working " &
                             "WHERE sale_date = '" & dr("sale_date") & "' " &
                             "GROUP BY merchant_report_working.branch_code"
                    Try
                        'ds2 = objDBRead.GetDataSet(tmpSQL)
                        ds2 = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                        If usingObjDB.isR(ds2) Then
                            For Each dr2 As DataRow In ds2.Tables(0).Rows
                                'Work through the dates
                                tmpSQL = "UPDATE cashup_summary " &
                                         "SET " &
                                         "cash_bank = " & Val(dr2("t_sum")) & " " &
                                         "WHERE sale_date = '" & dr("sale_date") & "' " &
                                         "AND branch_code = '" & dr2("branch_code") & "'"
                                Try
                                    'objDBWrite.ExecuteQuery(tmpSQL)
                                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                Catch ex As Exception
                                    'objDBWrite.CloseConnection()
                                    'objDBRead.CloseConnection()
                                    Return ex.Message
                                End Try

                            Next
                        End If
                    Catch ex As Exception
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return ex.Message
                    End Try
                Next
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        If isSuccess = False Then
            Return ErrorMessage
        End If

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

        Return "Success"

    End Function

    Private Function ProcessFNBStatement(ByVal FileName As String) As String

        Dim isSuccess As Boolean = True
        Dim ErrorMessage As String = ""

        Dim ReturnStatus As String = ""

        Dim line As String

        Dim linenumber As Integer = 0

        tmpSQL = "DELETE FROM merchant_report_working"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        Dim tmpBranchName As String
        Dim tmpDepositAmount As String
        Dim tmpDepositDate As String

        Dim stringdataline1(100) As String


        Try
            Using sr As New StreamReader(FileName)

                ' Read and display lines from the file until the end of
                ' the file is reached.

                Do
                    line = sr.ReadLine()
                    If Not (line Is Nothing) Then
                        '--------------------------------------------
                        'Lines 1 (0 in read) is the heading
                        stringdataline1 = line.Split(",")

                        '   0         1      2        3
                        '1234567890                123456789012345
                        '28/04/2017,1010,264117.02, ADT CASH DEPOVredenbu VD1
                        If stringdataline1.Count > 3 Then

                            If stringdataline1(3).ToString.ToUpper.Contains("ADT CASH") Then
                                tmpBranchName = Trim(stringdataline1(3).Replace(" ", "")).ToString.ToUpper
                                tmpBranchName = Mid(tmpBranchName, Len(tmpBranchName) - 2, 3)
                                tmpDepositAmount = RG.Numb(stringdataline1(1))
                                tmpDepositDate = Mid(stringdataline1(0), 7, 4) & "-" & Mid(stringdataline1(0), 4, 2) & "-" & Mid(stringdataline1(0), 1, 2)

                                Dim tdate As Date = DateTime.Parse(tmpDepositDate)

                                tmpDepositDate = Format(tdate.Date.AddDays(-1), "yyyy-MM-dd")

                                'Check if branch code exists
                                If CheckRecordExists("branch_details", "branch_code", tmpBranchName) = True Then
                                    tmpSQL = "INSERT INTO merchant_report_working (sale_date,branch_code,transaction_amount) " &
                                             "VALUES ('" & tmpDepositDate & "','" & tmpBranchName & "','" & tmpDepositAmount & "')"
                                    Try
                                        'objDBWrite.ExecuteQuery(tmpSQL)
                                        usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                    Catch ex As Exception
                                        'objDBWrite.CloseConnection()
                                        'objDBRead.CloseConnection()
                                        Return ex.Message
                                    End Try
                                End If
                            End If


                        End If
                    End If

                    linenumber += 1
                Loop Until line Is Nothing
            End Using
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            isSuccess = False
            ErrorMessage &= "Error in line " & linenumber & " \n"
        End Try

        tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date FROM merchant_report_working GROUP BY sale_date"
        'Check for mutiple dates
        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'Work through the dates
                    tmpSQL = "SELECT branch_code," &
                             "SUM(transaction_amount) as t_sum," &
                             "COUNT(transaction_amount) as t_count " &
                             "FROM merchant_report_working " &
                             "WHERE sale_date = '" & dr("sale_date") & "' " &
                             "GROUP BY merchant_report_working.branch_code"
                    Try
                        'ds2 = objDBRead.GetDataSet(tmpSQL)
                        ds2 = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                        If usingObjDB.isR(ds2) Then
                            For Each dr2 As DataRow In ds2.Tables(0).Rows
                                'Work through the dates
                                tmpSQL = "UPDATE cashup_summary " &
                                         "SET " &
                                         "cash_bank = " & Val(dr2("t_sum")) & " " &
                                         "WHERE sale_date = '" & dr("sale_date") & "' " &
                                         "AND branch_code = '" & dr2("branch_code") & "'"
                                Try
                                    'objDBWrite.ExecuteQuery(tmpSQL)
                                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                Catch ex As Exception
                                    'objDBWrite.CloseConnection()
                                    'objDBRead.CloseConnection()
                                    Return ex.Message
                                End Try

                            Next
                        End If
                    Catch ex As Exception
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return ex.Message
                    End Try
                Next
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        If isSuccess = False Then
            Return ErrorMessage
        End If

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

        Return "Success"

    End Function

    Private Function ProcessStandardBankStatement(ByVal FileName As String) As String

        Dim isSuccess As Boolean = True
        Dim ErrorMessage As String = ""

        Dim ReturnStatus As String = ""

        Dim line As String

        Dim linenumber As Integer = 0

        tmpSQL = "DELETE FROM merchant_report_working"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        Dim tmpBranchName As String
        Dim tmpDepositAmount As String
        Dim tmpDepositDate As String

        Dim stringdataline1(100) As String


        Try
            Using sr As New StreamReader(FileName)

                ' Read and display lines from the file until the end of
                ' the file is reached.

                Do
                    line = sr.ReadLine()
                    If Not (line Is Nothing) Then
                        '--------------------------------------------
                        'Lines 1 (0 in read) is the heading
                        stringdataline1 = line.Split(",")

                        '   0     1   2  3      4            5                                6    7
                        'HIST,20170301,,737,CASH DEPOSIT,FC1            FOURWAYS CRO 383 103,1273,0
                        If stringdataline1.Count > 5 Then
                            If stringdataline1(4).Contains("CASH DEPOSIT") Then
                                tmpBranchName = Trim(Mid(stringdataline1(5), 1, 5).Replace(" ", "")).ToString.ToUpper
                                tmpDepositAmount = RG.Numb(stringdataline1(3))
                                tmpDepositDate = Mid(stringdataline1(1), 1, 4) & "-" & Mid(stringdataline1(1), 5, 2) & "-" & Mid(stringdataline1(1), 7, 2)

                                Dim tdate As Date = DateTime.Parse(tmpDepositDate)

                                tmpDepositDate = Format(tdate.Date.AddDays(-1), "yyyy-MM-dd")

                                'Check if branch code exists
                                If CheckRecordExists("branch_details", "branch_code", tmpBranchName) = True Then
                                    tmpSQL = "INSERT INTO merchant_report_working (sale_date,branch_code,transaction_amount) " &
                                             "VALUES ('" & tmpDepositDate & "','" & tmpBranchName & "','" & tmpDepositAmount & "')"
                                    Try
                                        'objDBWrite.ExecuteQuery(tmpSQL)
                                        usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                    Catch ex As Exception
                                        'objDBWrite.CloseConnection()
                                        'objDBRead.CloseConnection()
                                        Return ex.Message
                                    End Try
                                End If
                            End If


                        End If
                    End If

                    linenumber += 1
                Loop Until line Is Nothing
            End Using
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            isSuccess = False
            ErrorMessage &= "Error in line " & linenumber & " \n"
        End Try

        tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date FROM merchant_report_working GROUP BY sale_date"
        'Check for mutiple dates
        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'Work through the dates
                    tmpSQL = "SELECT branch_code," &
                             "SUM(transaction_amount) as t_sum," &
                             "COUNT(transaction_amount) as t_count " &
                             "FROM merchant_report_working " &
                             "WHERE sale_date = '" & dr("sale_date") & "' " &
                             "GROUP BY merchant_report_working.branch_code"
                    Try
                        'ds2 = objDBRead.GetDataSet(tmpSQL)
                        ds2 = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                        If usingObjDB.isR(ds2) Then
                            For Each dr2 As DataRow In ds2.Tables(0).Rows
                                'Work through the dates
                                tmpSQL = "UPDATE cashup_summary " &
                                         "SET " &
                                         "cash_bank = " & Val(dr2("t_sum")) & " " &
                                         "WHERE sale_date = '" & dr("sale_date") & "' " &
                                         "AND branch_code = '" & dr2("branch_code") & "'"
                                Try
                                    'objDBWrite.ExecuteQuery(tmpSQL)
                                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                Catch ex As Exception
                                    'objDBWrite.CloseConnection()
                                    'objDBRead.CloseConnection()
                                    Return ex.Message
                                End Try

                            Next
                        End If
                    Catch ex As Exception
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return ex.Message
                    End Try
                Next
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        If isSuccess = False Then
            Return ErrorMessage
        End If

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

        Return "Success"

    End Function

    Private Function ProcessABSAStatement(ByVal FileName As String) As String

        Dim isSuccess As Boolean = True
        Dim ErrorMessage As String = ""

        Dim ReturnStatus As String = ""

        Dim line As String

        Dim linenumber As Integer = 0

        tmpSQL = "DELETE FROM merchant_report_working"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        Dim tmpBranchName As String
        Dim tmpDepositAmount As String
        Dim tmpDepositDate As String

        Dim stringdataline1(100) As String

        '15 -> 21 = merchant number
        '45 -> 50 = yyMMdd
        '77 -> 82 = amount

        Try
            Using sr As New StreamReader(FileName)

                ' Read and display lines from the file until the end of
                ' the file is reached.

                Do
                    line = sr.ReadLine()
                    If Not (line Is Nothing) Then
                        '--------------------------------------------
                        'Lines 1 (0 in read) is the heading
                        stringdataline1 = line.Split(",")

                        '   0          1            2       3       4  5                6    7     8              9      10 
                        '331155,0000004076282826,20170420,001245373,CT,C,          4198.00,NQUTU,CASH DEP BRANCH,NQ1, ,20170420,      -3269455.54,            30.00,000000000,0000083,CDP,CD, ,                   .
                        If stringdataline1.Count > 5 Then
                            If stringdataline1(8).ToString.ToUpper.Contains("CASH DEP") Then
                                tmpBranchName = Mid(Trim(stringdataline1(9).Replace(" ", "")).ToString.ToUpper, 1, 3)
                                tmpDepositAmount = RG.Numb(stringdataline1(6))
                                tmpDepositDate = Mid(stringdataline1(2), 1, 4) & "-" & Mid(stringdataline1(2), 5, 2) & "-" & Mid(stringdataline1(2), 7, 2)

                                Dim tdate As Date = DateTime.Parse(tmpDepositDate)
                                tmpDepositDate = Format(tdate.Date.AddDays(-1), "yyyy-MM-dd")

                                'Check if branch code exists
                                If CheckRecordExists("branch_details", "branch_code", tmpBranchName) = True Then
                                    tmpSQL = "INSERT INTO merchant_report_working (sale_date,branch_code,transaction_amount) " &
                                         "VALUES ('" & tmpDepositDate & "','" & tmpBranchName & "','" & tmpDepositAmount & "')"
                                    Try
                                        'objDBWrite.ExecuteQuery(tmpSQL)
                                        usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                    Catch ex As Exception
                                        'objDBWrite.CloseConnection()
                                        'objDBRead.CloseConnection()
                                        Return ex.Message
                                    End Try
                                End If
                            End If


                        End If
                    End If

                    linenumber += 1
                Loop Until line Is Nothing
            End Using
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            isSuccess = False
            ErrorMessage &= "Error in line " & linenumber & " \n"
        End Try

        tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date FROM merchant_report_working GROUP BY sale_date"
        'Check for mutiple dates
        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'Work through the dates
                    tmpSQL = "SELECT branch_code," &
                             "SUM(transaction_amount) as t_sum," &
                             "COUNT(transaction_amount) as t_count " &
                             "FROM merchant_report_working " &
                             "WHERE sale_date = '" & dr("sale_date") & "' " &
                             "GROUP BY merchant_report_working.branch_code"
                    Try
                        'ds2 = objDBRead.GetDataSet(tmpSQL)
                        ds2 = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                        If usingObjDB.isR(ds2) Then
                            For Each dr2 As DataRow In ds2.Tables(0).Rows
                                'Work through the dates
                                tmpSQL = "UPDATE cashup_summary " &
                                         "SET " &
                                         "cash_bank = " & Val(dr2("t_sum")) & " " &
                                         "WHERE sale_date = '" & dr("sale_date") & "' " &
                                         "AND branch_code = '" & dr2("branch_code") & "'"
                                Try
                                    'objDBWrite.ExecuteQuery(tmpSQL)
                                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                Catch ex As Exception
                                    'objDBWrite.CloseConnection()
                                    'objDBRead.CloseConnection()
                                    Return ex.Message
                                End Try

                            Next
                        End If
                    Catch ex As Exception
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return ex.Message
                    End Try
                Next
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        If isSuccess = False Then
            Return ErrorMessage
        End If

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

        Return "Success"

    End Function

    Public Function UpdateEFTID(ByVal BranchCode As String, ByVal BranchName As String,
                                ByVal EFTId As String) As String

        'tmpSQL = "SELECT branch_code,branch_name,merchant_number FROM branch_details"
        'Try
        '    ds = objDBRead.GetDataSet(tmpSQL)
        '    If objDBRead.isR(ds) Then
        '        For Each dr As DataRow In ds.Tables(0).Rows
        '            tmpSQL = "SELECT branch_code FROM eft_ids WHERE branch_code = '" & dr("branch_code") & "'"
        '            Try
        '                ds2 = objDBRead.GetDataSet(tmpSQL)
        '                If Not objDBRead.isR(ds2) Then
        '                    'branch not in eft_ids
        '                    tmpSQL = "INSERT INTO eft_ids (branch_code,branch_name,eft_id) " &
        '                             "VALUES " &
        '                             "('" & dr("branch_code") & "','" & dr("branch_name") & "','" & dr("merchant_number") & "')"
        '                    Try
        '                        objDBWrite.ExecuteQuery(tmpSQL)
        '                    Catch ex As Exception
        '                        objDBWrite.CloseConnection()
        '                        objDBRead.CloseConnection()
        '                        Return ex.Message
        '                    End Try
        '                End If
        '            Catch ex As Exception
        '                If (objDBRead IsNot Nothing) Then
        '                    objDBRead.CloseConnection()
        '                End If
        '                Return ex.Message
        '            End Try
        '        Next
        '    End If
        'Catch ex As Exception
        '    If (objDBRead IsNot Nothing) Then
        '        objDBRead.CloseConnection()
        '    End If
        '    Return ex.Message
        'End Try

        'Check that the EFT ID isn't in use by another store
        tmpSQL = "SELECT branch_code,branch_name FROM eft_ids WHERE eft_id = '" & EFTId & "'"
        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'objDBWrite.CloseConnection()
                    'objDBRead.CloseConnection()
                    Return "This EFT ID is already in use by branch " & dr("branch_code") & " - " & dr("branch_name") &
                            " and cannot be used"
                Next
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        tmpSQL = "INSERT INTO eft_ids (branch_code,branch_name,eft_id) VALUES ('" & BranchCode & "','" & BranchName & "','" & EFTId & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

        Return "success"

    End Function

    Public Function DeleteEFTID(ByVal BranchCode As String, ByVal EFTId As String) As String

        tmpSQL = "DELETE FROM eft_ids WHERE eft_id = '" & EFTId & "'"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

        Return "success"

    End Function

    Public Function GetEFTIDS(ByVal BranchCode As String) As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "Select eft_id FROM eft_ids WHERE branch_code = '" & BranchCode & "'"

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

    Public Function GetBranches() As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text

            tmpSQL = "Select branch_code,branch_name FROM branch_details ORDER BY branch_code ASC"

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

    Private Function ProcessEFTStatement(ByVal FileName As String) As String

        Dim isSuccess As Boolean = True
        Dim ErrorMessage As String = ""

        Dim ReturnStatus As String = ""

        Dim line As String

        Dim linenumber As Integer = 0

        tmpSQL = "DELETE FROM merchant_report_working"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        Dim WorkingDate As String
        Dim MerchantNumber As String
        Dim TransactionAmount As String

        '15 -> 21 = merchant number
        '45 -> 50 = yyMMdd
        '77 -> 82 = amount

        Try
            Using sr As New StreamReader(FileName)

                ' Read and display lines from the file until the end of
                ' the file is reached.

                Do
                    line = sr.ReadLine()
                    If Not (line Is Nothing) Then
                        '--------------------------------------------
                        'Lines 1 (0 in read) is the heading
                        If linenumber = 0 Then
                            If Mid(line, 1, 5) <> "1RAGE" Then
                                isSuccess = False
                                ErrorMessage &= "There Is an Error With the file. Please check the file type. \n"
                                Return ErrorMessage
                            End If
                        End If

                        If linenumber >= 1 Then
                            If Mid(line, 35, 1) = "*" Then 'Check for last line
                                WorkingDate = "20" & Mid(line, 45, 2) & "-" & Mid(line, 47, 2) & "-" & Mid(line, 49, 2)
                                MerchantNumber = Mid(line, 15, 7)
                                TransactionAmount = Val(Mid(line, 77, 4)) & "." & Mid(line, 81, 2)

                                tmpSQL = "INSERT INTO merchant_report_working (sale_date,merchant_number,transaction_amount) " &
                                      "VALUES ('" & WorkingDate & "','" & MerchantNumber & "','" & TransactionAmount & "')"
                                Try
                                    'objDBWrite.ExecuteQuery(tmpSQL)
                                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                Catch ex As Exception
                                    'objDBWrite.CloseConnection()
                                    'objDBRead.CloseConnection()
                                    Return ex.Message
                                End Try
                            End If
                        End If
                        linenumber += 1
                    End If
                Loop Until line Is Nothing
            End Using
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            isSuccess = False
            ErrorMessage &= "Error in line " & linenumber & " \n"
        End Try

        tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date FROM merchant_report_working GROUP BY sale_date"
        'Check for mutiple dates
        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'Work through the dates
                    'tmpSQL = "SELECT max(branch_code) AS branch_code,merchant_report_working.merchant_number," &
                    '         "SUM(transaction_amount) as t_sum," &
                    '         "COUNT(transaction_amount) as t_count " &
                    '         "FROM merchant_report_working " &
                    '         "INNER JOIN eft_ids ON merchant_report_working.merchant_number = eft_ids.merchant_number " &
                    '         "WHERE sale_date = '" & dr("sale_date") & "' " &
                    '         "GROUP BY merchant_report_working.merchant_number"

                    tmpSQL = "SELECT " &
                             "eft_ids.branch_code AS branch_code, " &
                             "SUM(transaction_amount) AS t_sum," &
                             "COUNT(transaction_amount) AS t_count " &
                             "FROM merchant_report_working " &
                             "INNER JOIN eft_ids ON merchant_report_working.merchant_number = eft_ids.eft_id " &
                             "WHERE sale_date = '" & dr("sale_date") & "' " &
                             "GROUP BY eft_ids.branch_code " &
                             "ORDER BY eft_ids.branch_code"
                    Try
                        'ds2 = objDBRead.GetDataSet(tmpSQL)
                        ds2 = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                        If usingObjDB.isR(ds2) Then
                            For Each dr2 As DataRow In ds2.Tables(0).Rows
                                'Work through the dates
                                tmpSQL = "UPDATE cashup_summary " &
                                         "SET credit_card_transactions_bank = " & Val(dr2("t_count")) & "," &
                                         "credit_card_bank = " & Val(dr2("t_sum")) & " " &
                                         "WHERE sale_date = '" & dr("sale_date") & "' " &
                                         "AND branch_code = '" & dr2("branch_code") & "'"
                                Try
                                    'objDBWrite.ExecuteQuery(tmpSQL)
                                    usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
                                Catch ex As Exception
                                    'objDBWrite.CloseConnection()
                                    'objDBRead.CloseConnection()
                                    Return ex.Message
                                End Try

                            Next
                        End If
                    Catch ex As Exception
                        'If (objDBRead IsNot Nothing) Then
                        '    objDBRead.CloseConnection()
                        'End If
                        Return ex.Message
                    End Try
                Next
            End If
        Catch ex As Exception
            'If (objDBRead IsNot Nothing) Then
            '    objDBRead.CloseConnection()
            'End If
            Return ex.Message
        End Try

        If isSuccess = False Then
            Return ErrorMessage
        End If

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

        Return "Success"

    End Function

    Public Function InsertCashupComment(ByVal SaleDate As String, ByVal BranchCode As String,
                                         ByVal Comments As String, ByVal Username As String) As String

        tmpSQL = "UPDATE cashup_summary SET comment = '" & Mid(RG.Apos(Comments), 1, 1000) & "' " &
                 "WHERE branch_code = '" & BranchCode & "' AND sale_date = '" & SaleDate & "'"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        tmpSQL = "INSERT INTO cashup_summary_log (time_stamp,username,comment,branch_code,sale_date) " &
                 "VALUES (now(),'" & Username & "','" & Mid(RG.Apos(Comments), 1, 400) & "','" & BranchCode & "','" & SaleDate & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            'objDBWrite.CloseConnection()
            'objDBRead.CloseConnection()
            Return ex.Message
        End Try

        'objDBWrite.CloseConnection()
        'objDBRead.CloseConnection()

        Return "Success"

    End Function

    Public Function CheckRecordExists(ByVal TableName As String, ByVal ColumnName As String, ByVal RecordToCheck As String,
                                      Optional ByVal ColumnName2 As String = "", Optional ByVal RecordToCheck2 As String = "",
                                      Optional ByVal ColumnName3 As String = "", Optional ByVal RecordToCheck3 As String = "") As Boolean

        Dim RecordExists As Boolean = False

        tmpSQL = "SELECT " & ColumnName & " FROM " & TableName & " WHERE " & ColumnName & " ILIKE '" & RG.Apos(RecordToCheck) & "'"
        If ColumnName2 <> "" Then
            tmpSQL = tmpSQL & " AND " & ColumnName2 & " = '" & RG.Apos(RecordToCheck2) & "'"
        End If
        If ColumnName3 <> "" Then
            tmpSQL = tmpSQL & " AND " & ColumnName3 & " = '" & RG.Apos(RecordToCheck3) & "'"
        End If

        Try
            'Ds = objDBRead.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                RecordExists = True
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

        Return RecordExists

    End Function

    Public Function GetTaxGroups() As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT tax_group|| ' - ' ||tax_description|| ' (' ||tax_value|| ')' AS taxgroups FROM tax_groups ORDER BY tax_group"
            '01 - Standard (10.00)

            'Dim reader As New NpgsqlDataAdapter(strQuery, _POSReadConnectionString)
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

    Public Function GetCompanySettings() As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT * FROM company_details"
            '01 - Standard (10.00)

            'Dim reader As New NpgsqlDataAdapter(strQuery, _POSReadConnectionString)
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


End Class
