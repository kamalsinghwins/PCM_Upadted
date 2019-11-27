Imports System.IO
Imports Entities
Imports Newtonsoft.Json
Imports Npgsql
Imports Npgsql.Logging
'Imports pcm.DataLayer.dlLoggingNpgSQL
Public Class PCMReportingDataLayer
    Inherits DataAccessLayerBase

    Dim DebtorDataLayer As New DebtorsDataLayer
    Dim getDetailsResponse As New GetDetailsResponse
    Dim queryResponse As New QueryResponse
    Dim reportResponse As New ReportResponse
    Dim getPeriodResponse As New GetPeriodResponse
    Dim getAgeAnalysisDetailResponse As New GetAgeAnalysisDetailResponse
    Dim giftCardDetailsResponse As New GiftCardDetailsResponse
    Dim transactionListResponse As New TransactionListResponse
    Dim printTransactionDetailsResponse As New PrintTransactionDetailsResponse
    Dim _GetQueryResponse As New GetQueryResponse
    Dim cardIssuedResponse As New CardIssuedResponse

    Public Function GetDetails(_getDetailsRequest As GetDetailsRequest) As GetDetailsResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        tmpSQL = "Select " &
             "REPLACE(REPLACE(CAST(COUNT(CASE WHEN f.current_balance <> 0 AND p30 = 0 AND p60 = 0 AND p90 = 0	AND p120 = 0 AND p150 = 0 THEN P .account_number END) AS money)::text,'$', ''),'.00',''	) AS number_of_current," &
             "REPLACE(REPLACE(CAST(COUNT(Case When p30 <> 0 And p60 = 0 And p90 = 0 And p120 = 0 And p150 = 0 Then p.account_number End) AS money)::text,'$', ''),'.00',''	) AS number_of_30," &
             "REPLACE(REPLACE(CAST(COUNT(Case When p60 <> 0 And p90 = 0 And p120 = 0 And p150 = 0 Then p.account_number End) AS money)::text,'$', ''),'.00',''	) AS number_of_60," &
             "REPLACE(REPLACE(CAST(COUNT(Case When p90 <> 0 And p120 = 0 And p150 = 0 Then p.account_number End) AS money)::text,'$', ''),'.00',''	) AS number_of_90," &
             "REPLACE(REPLACE(CAST(COUNT(Case When p120 <> 0 And p150 = 0 Then p.account_number End) AS money)::text,'$', ''),'.00',''	) AS number_of_120," &
             "REPLACE(REPLACE(CAST(COUNT(Case When p150 <> 0 Then p.account_number End) AS money)::text,'$', ''),'.00',''	) AS number_of_150," &
             "REPLACE(REPLACE(CAST(COUNT(p.account_number) AS money)::text,'$', ''),'.00',''	) AS number_of_accounts," &
             "replace(replace(cast(ceil(SUM(f.total)) as money)::text,'$', ''), '.00', '') As total,replace(replace(cast(ceil(SUM(f.current_balance)) as money)::text,'$', ''), '.00', '') As balance,replace(replace(cast(ceil(SUM(f.p30)) as money)::text,'$', ''), '.00', '') As p30," &
             "replace(replace(cast(ceil(SUM(f.p60)) as money)::text,'$', ''), '.00', '') As p60,replace(replace(cast(ceil(SUM(f.p90)) as money)::text,'$', ''), '.00', '') As p90,replace(replace(cast(ceil(SUM(f.p120)) as money)::text,'$', ''), '.00', '') As p120,replace(replace(cast(ceil(SUM(f.p150)) as money)::text,'$', ''), '.00', '') As p150, " &
             "replace(replace(cast(ceil(SUM(f.total_spent)) as money)::text,'$', ''), '.00', '') As total_spent " &
             "FROM financial_balances f " &
             "INNER JOIN debtor_personal p On f.account_number = p.account_number "

        Dim AddedJoin As Boolean
        AddedJoin = False

        If _getDetailsRequest.AccountsOpenedBetween = True Then
            tmpSQL = tmpSQL & "INNER JOIN debtor_dates dd On dd.account_number = p.account_number "
            AddedJoin = True
        End If

        If AddedJoin = False Then
            If _getDetailsRequest.LastTransaction = True Then
                tmpSQL = tmpSQL & "INNER JOIN debtor_dates dd On dd.account_number = p.account_number "
            End If
        End If

        tmpSQL = tmpSQL & "WHERE p.account_number <> ''"

        If _getDetailsRequest.ActiveOnly = True Then 'Active Customers Only
            tmpSQL = tmpSQL & " AND p.status = 'ACTIVE'"
        Else 'Option of a Status
            If _getDetailsRequest.Other <> "ALL" Then 'This will be a huge report
                tmpSQL = tmpSQL & " AND p.status = '" & _getDetailsRequest.Other & "'"
            End If
        End If

        'If _getDetailsRequest.CheckAll = False Then
        '    tmpSQL = tmpSQL & " AND p.account_number >= '" & _getDetailsRequest.FromAccount & "' AND p.account_number <= '" & _getDetailsRequest.ToAccount & "'"
        'End If

        'If _getDetailsRequest.TickOn = True Then
        '    If _getDetailsRequest.TickOff = False Then
        '        tmpSQL = tmpSQL & " AND p.show_on_age_analysis = 'True'"
        '    End If
        'Else
        '    If _getDetailsRequest.TickOff = True Then
        '        tmpSQL = tmpSQL & " AND p.show_on_age_analysis = 'False'"
        '    End If
        'End If

        If _getDetailsRequest.CheckCurrentUse = True Then

            tmpSQL = tmpSQL & " AND current_balance " & _getDetailsRequest.CboCurrent & " " & _getDetailsRequest.WhereCurrent
        End If

        If _getDetailsRequest.CheckUse30 = True Then
            tmpSQL = tmpSQL & " AND p30 " & _getDetailsRequest.Cbo30Days & " " & _getDetailsRequest.Where30Days
        End If

        If _getDetailsRequest.CheckUse60 = True Then
            tmpSQL = tmpSQL & " AND p60 " & _getDetailsRequest.Cbo60Days & " " & _getDetailsRequest.Where60Days
        End If

        If _getDetailsRequest.CheckUse90 = True Then
            tmpSQL = tmpSQL & " AND p90 " & _getDetailsRequest.Cbo90Days & " " & _getDetailsRequest.Where90Days
        End If

        If _getDetailsRequest.CheckUse120 = True Then
            tmpSQL = tmpSQL & " AND p120 " & _getDetailsRequest.Cbo120Days & " " & _getDetailsRequest.Where120Days
        End If

        If _getDetailsRequest.CheckUse150 = True Then
            tmpSQL = tmpSQL & " AND p150 " & _getDetailsRequest.Cbo150Days & " " & _getDetailsRequest.Where150Days
        End If

        If _getDetailsRequest.CheckUsetotal = True Then
            If _getDetailsRequest.CboTotal = "BETWEEN" Then
                tmpSQL = tmpSQL & " AND total BETWEEN " & _getDetailsRequest.Wheretotal & " AND " & _getDetailsRequest.ToTotal
            Else
                tmpSQL = tmpSQL & " And total " & _getDetailsRequest.CboTotal & " " & _getDetailsRequest.Wheretotal
            End If
        End If

        If _getDetailsRequest.AccountsOpenedBetween = True Then
            tmpSQL = tmpSQL & " AND date_of_creation BETWEEN '" & _getDetailsRequest.StartDate & "' AND '" & _getDetailsRequest.EndDate & "'"
        End If

        If _getDetailsRequest.LastTransaction = True Then
            tmpSQL = tmpSQL & " AND date_of_last_transaction BETWEEN '" & _getDetailsRequest.LastDateTransactionStartDate & "' AND '" & _getDetailsRequest.LastDateTransactionEndDate & "'"
        End If

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                getDetailsResponse.GetSelectedResponse = ds.Tables(0)
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
        getDetailsResponse.Success = True
        Return getDetailsResponse

    End Function

    Public Function GetReport() As GetPeriodResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Try
            tmpSQL = "SELECT current_period FROM general_settings"
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                Dim CurrentPeriod As Integer
                CurrentPeriod = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("current_period")
                getPeriodResponse.Success = True
                getPeriodResponse.Period = CurrentPeriod
            Else
                getPeriodResponse.Success = False
                getPeriodResponse.Message = "No Period set in Database."
            End If
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            queryResponse.Message = ""
            queryResponse.Success = False
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        Return getPeriodResponse

    End Function

    Public Function GetEMailAddresses() As String
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim csv As String = String.Empty

        Dim FileName As String

        Dim tGuID As String = Guid.NewGuid.ToString

        FileName = HttpContext.Current.Server.MapPath("~\Docs\" & tGuID & ".csv")

        Dim CreateFile As New FileStream(FileName, FileMode.Append)
        Dim strStreamWriter As New StreamWriter(CreateFile)

        tmpSQL = "SELECT email_address,INITCAP(first_name) as first_name FROM debtor_personal WHERE email_address Like '%@%'"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(Ds) Then
                strStreamWriter.WriteLine("FirstName,Email")
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'csv &= dr("email_address") & vbCrLf
                    strStreamWriter.WriteLine(dr("first_name") & "," & dr("email_address"))
                Next
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

        tmpSQL = "SELECT email_address,
                         INITCAP(first_name) as first_name
                         FROM customer_contact_details ccd
                         inner join customer_personal cp
                         on cp.account_number=ccd.account_number
                         WHERE email_address like '%@%'"

        Try
            ds = objDBPositive.GetDataSet(tmpSQL)
            If objDB.isR(Ds) Then

                For Each dr As DataRow In Ds.Tables(0).Rows
                    'csv &= dr("email_address") & vbCrLf
                    strStreamWriter.WriteLine(dr("first_name") & "," & dr("email_address"))
                Next
            End If
        Catch ex As Exception
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
        End Try

        strStreamWriter.Close()
        strStreamWriter.Dispose()

        CreateFile.Close()
        CreateFile.Dispose()

        Return tGuID


    End Function

    Public Function GetAllCellphoneNumbers() As String
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim csv As String = String.Empty
        Dim FileName As String

        Dim tGuID As String = Guid.NewGuid.ToString

        FileName = HttpContext.Current.Server.MapPath("~\Docs\" & tGuID & ".csv")

        Dim CreateFile As New FileStream(FileName, FileMode.Append)
        Dim strStreamWriter As New StreamWriter(CreateFile)

        'tmpSQL = "Select " &
        '         "27 || substr(cell_number,2,2) || substr(cell_number,5,3) || substr(cell_number,9,4) As cell_number " &
        '         "FROM " &
        '         "financial_balances " &
        '         "INNER JOIN debtor_personal On financial_balances.account_number = debtor_personal.account_number " &
        '         "INNER JOIN debtor_dates On financial_balances.account_number = debtor_dates.account_number " &
        '         "INNER JOIN card_details On financial_balances.account_number = card_details.account_number " &
        '         "WHERE " &
        '         "dont_send_sms = False " &
        '         "And length(cell_number) = 12"

        'tmpSQL = "SELECT " &
        '         "27 || substr(cell_number,2,2) || substr(cell_number,5,3) || substr(cell_number,9,4) As cell_number " &
        '         "FROM " &
        '         "debtor_personal " &
        '         "WHERE " &
        '         "dont_send_sms = False " &
        '         "AND cell_number <> '000-000-0000' " &
        '         "AND length(cell_number) = 12"

        ''  Added firstname and available amount to buy
        tmpSQL = "SELECT " &
                 "27 || substr(cell_number,2,2) || substr(cell_number,5,3) || substr(cell_number,9,4) As cell_number, " &
                 "INITCAP(first_name) AS first_name," &
                 "financial_balances.credit_limit - financial_balances.total as available_amount " &
                 "FROM " &
                 "debtor_personal " &
                 "INNER JOIN financial_balances  ON financial_balances.account_number = debtor_personal.account_number " &
                 "WHERE " &
                 "dont_send_sms = False " &
                 "AND cell_number <> '000-000-0000' " &
                 "AND length(cell_number) = 12"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(Ds) Then
                strStreamWriter.WriteLine("CellphoneNumber,FirstName,Available Amount To Pay")
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'csv &= dr("cell_number") & vbCrLf
                    strStreamWriter.WriteLine(dr("cell_number") & "," & dr("first_name") & "," & dr("available_amount"))

                Next
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



        strStreamWriter.Close()
        strStreamWriter.Dispose()

        CreateFile.Close()
        CreateFile.Dispose()

        Return tGuID


    End Function

    Public Function GetCellphoneNumbers() As String
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim csv As String = String.Empty

        Dim FileName As String

        Dim tGuID As String = Guid.NewGuid.ToString

        FileName = HttpContext.Current.Server.MapPath("~\Docs\" & tGuID & ".csv")

        Dim CreateFile As New FileStream(FileName, FileMode.Append)
        Dim strStreamWriter As New StreamWriter(CreateFile)

        ''Added firstname and available amount to buy

        tmpSQL = "SELECT " &
                 "27 || substr(cell_number,2,2) || substr(cell_number,5,3) || substr(cell_number,9,4) As cell_number, " &
                 "INITCAP(first_name) AS first_name," &
                 "financial_balances.credit_limit - financial_balances.total as available_amount " &
                 "FROM " &
                 "financial_balances " &
                 "INNER JOIN debtor_personal ON financial_balances.account_number = debtor_personal.account_number " &
                 "INNER JOIN debtor_dates ON financial_balances.account_number = debtor_dates.account_number " &
                 "INNER JOIN card_details ON financial_balances.account_number = card_details.account_number " &
                 "WHERE " &
                 "financial_balances.p90 = 0 And " &
                 "financial_balances.p120 = 0 AND " &
                 "financial_balances.p150 = 0 And " &
                 "debtor_personal.status = 'ACTIVE' " &
                 "And current_status = 'ACTIVE' " &
                 "and card_details.card_number <> '' " &
                 "And dont_send_sms = FALSE AND temp_send_sms = TRUE " &
                 "And cell_number <> '000-000-0000' " &
                 "and length(cell_number) = 12"

        Try
            Ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(Ds) Then
                strStreamWriter.WriteLine("CellphoneNumber,FirstName,Available Amount To Pay")
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'csv &= dr("cell_number") & vbCrLf
                    strStreamWriter.WriteLine(dr("cell_number") & "," & dr("first_name") & "," & dr("available_amount"))
                Next
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

        strStreamWriter.Close()
        strStreamWriter.Dispose()

        CreateFile.Close()
        CreateFile.Dispose()

        Return tGuID


    End Function

    Public Function GetCellphonesByBranch(ByVal BranchCode As String) As String
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        Dim csv As String = String.Empty

        Dim FileName As String

        Dim tGuID As String = Guid.NewGuid.ToString

        FileName = HttpContext.Current.Server.MapPath("~\Docs\" & tGuID & ".csv")

        Dim CreateFile As New FileStream(FileName, FileMode.Append)
        Dim strStreamWriter As New StreamWriter(CreateFile)

        ''Added firstname and available amount to buy

        tmpSQL = "SELECT " &
                 "27 || substr(cell_number,2,2) || substr(cell_number,5,3) || substr(cell_number,9,4) As cell_number, " &
                 "INITCAP(first_name) AS first_name," &
                 "financial_balances.credit_limit - financial_balances.total as available_amount " &
                 "FROM debtor_personal " &
                 "INNER JOIN financial_balances ON financial_balances.account_number = debtor_personal.account_number " &
                 "where branch_code = '" & BranchCode & "' " &
                 "And dont_send_sms = FALSE " &
                 "and length(cell_number) = 12"


        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(Ds) Then
                strStreamWriter.WriteLine("CellphoneNumber,FirstName,Available Amount To Pay")
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'csv &= dr("cell_number") & vbCrLf
                    strStreamWriter.WriteLine(dr("cell_number") & "," & dr("first_name") & "," & dr("available_amount"))
                Next
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

        strStreamWriter.Close()
        strStreamWriter.Dispose()

        CreateFile.Close()
        CreateFile.Dispose()

        Return tGuID


    End Function

    Public Function GetAgeAnalysisDetails(_getAgeAnalysisDetailRequest As GetAgeAnalysisDetailRequest) As GetAgeAnalysisDetailResponse

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        If Val(_getAgeAnalysisDetailRequest.FromAccount) > Val(_getAgeAnalysisDetailRequest.ToAccount) Then
            getAgeAnalysisDetailResponse.Message = "Please select a proper account range."
            getAgeAnalysisDetailResponse.Success = False
            Return getAgeAnalysisDetailResponse
        End If

        If _getAgeAnalysisDetailRequest.ActiveAccount = False And _getAgeAnalysisDetailRequest.CheckOtherStatus = False Then
            getAgeAnalysisDetailResponse.Message = "Please select a Status option."
            getAgeAnalysisDetailResponse.Success = False
            Return getAgeAnalysisDetailResponse
        End If

        If _getAgeAnalysisDetailRequest.CheckOtherStatus = True And _getAgeAnalysisDetailRequest.OtherStatus = "" Then
            getAgeAnalysisDetailResponse.Message = "Please select an option for ""Other Status"" "
            getAgeAnalysisDetailResponse.Success = False
            Return getAgeAnalysisDetailResponse
        End If

        'If _getAgeAnalysisDetailRequest.DoubleLine = "" Then
        '    getAgeAnalysisDetailResponse.Message = "Please select a valid file."
        '    getAgeAnalysisDetailResponse.Success = False
        '    Return getAgeAnalysisDetailResponse
        'End If

        If _getAgeAnalysisDetailRequest.Period = "" Then
            _getAgeAnalysisDetailRequest.Period = "CURRENT"
        End If

        'Dim strP(1 To 7) As String
        'cmdOk.Enabled = False
        'pB.Visible = True

        Dim isRageEmployee As Boolean

        'TotalDebtors = 0

        'Strm.WriteLine "Name,Account Number,Current,30 Days,60 Days,90 Days,120 Days,150 Days,Total,Cellphone,Last Transaction,Last Payment"

        'strP(1) = "0"
        'strP(2) = "0"
        'strP(3) = "0"
        'strP(4) = "0"
        'strP(5) = "0"
        'strP(6) = "0"
        'strP(7) = "0"

        'Dim strTmpSQL As String

        If _getAgeAnalysisDetailRequest.Period = "CURRENT" Then
            tmpSQL = "SELECT debtor_personal.account_number,debtor_personal.first_name,debtor_personal.last_name,debtor_personal.cell_number," &
                       "debtor_personal.id_number,debtor_personal.clock_number,financial_balances.total," &
                       "financial_balances.current_balance,financial_balances.p30,financial_balances.p60,financial_balances.p90," &
                       "financial_balances.p120," &
                       "financial_balances.p150,debtor_dates.date_of_last_payment,debtor_dates.date_of_last_transaction," &
                       "debtor_first_purchase.first_purchase as branch_code " &
                       "FROM " &
                       "debtor_personal " &
                       "LEFT OUTER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
                       "LEFT OUTER JOIN debtor_dates ON debtor_personal.account_number = debtor_dates.account_number " &
                       "LEFT OUTER JOIN debtor_first_purchase ON debtor_first_purchase.account_number = debtor_personal.account_number " &
                       "WHERE debtor_personal.account_number <> ''" 'This is some nonsense to avoid the problem with where to use the AND
        Else
            tmpSQL = "SELECT debtor_personal.account_number,debtor_personal.first_name,debtor_personal.last_name,debtor_personal.cell_number," &
                      "debtor_personal.id_number,debtor_personal.clock_number,financial_closing_balances.total," &
                      "financial_closing_balances.p150,debtor_dates.date_of_last_payment,debtor_dates.date_of_last_transaction " &
                      "financial_closing_balances.current_balance,financial_closing_balances.p30,financial_closing_balances.p60," &
                      "financial_closing_balances.p90,financial_closing_balances.p120,debtor_first_purchase.first_purchase as branch_code " &
                      "FROM " &
                      "debtor_personal " &
                      "LEFT OUTER JOIN financial_closing_balances " &
                      "ON debtor_personal.account_number = financial_closing_balances.account_number " &
                      "LEFT OUTER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
                      "LEFT OUTER JOIN debtor_dates ON debtor_personal.account_number = debtor_dates.account_number " &
                      "LEFT OUTER JOIN debtor_first_purchase ON debtor_first_purchase.account_number = debtor_personal.account_number " &
                      "WHERE financial_closing_balances.current_period = '" & _getAgeAnalysisDetailRequest.Period & "'"
        End If

        If _getAgeAnalysisDetailRequest.ActiveAccount = True Then 'Active Customers Only
            tmpSQL = tmpSQL & " AND status = 'ACTIVE'"
        Else 'Option of a Status
            If _getAgeAnalysisDetailRequest.OtherStatus <> "ALL" Then
                tmpSQL = tmpSQL & " AND status = '" & _getAgeAnalysisDetailRequest.OtherStatus & "'"
            End If
        End If

        If _getAgeAnalysisDetailRequest.AllAccounts = False Then
            tmpSQL = tmpSQL & " AND debtor_personal.account_number >= '" & _getAgeAnalysisDetailRequest.FromAccount & "' AND debtor_personal.account_number <= '" & _getAgeAnalysisDetailRequest.ToAccount & "'"
        End If

        If _getAgeAnalysisDetailRequest.BranchCode <> "" Then
            tmpSQL = tmpSQL & " AND debtor_first_purchase.first_purchase = '" & RG.Apos(_getAgeAnalysisDetailRequest.BranchCode) & "' "
        End If
        If _getAgeAnalysisDetailRequest.PrintZero = True And _getAgeAnalysisDetailRequest.PrintCredit = False And _getAgeAnalysisDetailRequest.PrintDebit = False Then
            tmpSQL = tmpSQL & " AND financial_balances.total = 0"
        ElseIf _getAgeAnalysisDetailRequest.PrintZero = True And _getAgeAnalysisDetailRequest.PrintCredit = True And _getAgeAnalysisDetailRequest.PrintDebit = False Then
            tmpSQL = tmpSQL & " AND financial_balances.total >= 0"
        ElseIf _getAgeAnalysisDetailRequest.PrintZero = False And _getAgeAnalysisDetailRequest.PrintCredit = True And _getAgeAnalysisDetailRequest.PrintDebit = False Then
            tmpSQL = tmpSQL & " AND financial_balances.total > 0"
        ElseIf _getAgeAnalysisDetailRequest.PrintZero = True And _getAgeAnalysisDetailRequest.PrintCredit = False And _getAgeAnalysisDetailRequest.PrintDebit = True Then
            tmpSQL = tmpSQL & " AND financial_balances.total <= 0"
        ElseIf _getAgeAnalysisDetailRequest.PrintZero = False And _getAgeAnalysisDetailRequest.PrintCredit = True And _getAgeAnalysisDetailRequest.PrintDebit = True Then
            tmpSQL = tmpSQL & " AND financial_balances.total <> 0"
        ElseIf _getAgeAnalysisDetailRequest.PrintZero = False And _getAgeAnalysisDetailRequest.PrintCredit = False And _getAgeAnalysisDetailRequest.PrintDebit = True Then
            tmpSQL = tmpSQL & " AND financial_balances.total < 0"
        End If

        tmpSQL = tmpSQL & " AND is_rage_employee = '" & _getAgeAnalysisDetailRequest.RageEmployee & "'"

        tmpSQL = tmpSQL & " AND debtor_personal.show_on_age_analysis = True ORDER BY debtor_personal.account_number ASC"

        'tmpSQL &= " LIMIT 1000"



        Try
            'ds = objDB.GetDataSet(tmpSQL)
            'If objDB.isR(ds) Then
            '    getAgeAnalysisDetailResponse.LongTotalCount = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("count")
            'End If


            'tmpSQL = strTmpSQL
            ds = objDB.GetDataSet(tmpSQL)

            If objDB.isR(ds) Then
                getAgeAnalysisDetailResponse.AgeAnalysisDetails = ds.Tables(0)
            End If
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            getAgeAnalysisDetailResponse.Success = False
            getAgeAnalysisDetailResponse.Message = "Something Went Wrong"
            Return getAgeAnalysisDetailResponse
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        getAgeAnalysisDetailResponse.Success = True
        Return getAgeAnalysisDetailResponse

    End Function

    Public Function GetQuery(_queryRequest As QueryRequest) As QueryResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        'If Not RG.ValidDate(_queryRequest.AccountStartDate) Then
        '    queryResponse.Message = "Invalid Account Start Date."
        '    queryResponse.Success = False
        '    Return queryResponse
        'End If

        'If Not RG.ValidDate(_queryRequest.AccountEndDate) Then
        '    queryResponse.Message = "Invalid Account End Date."
        '    queryResponse.Success = False
        '    Return queryResponse
        'End If

        'If Not RG.ValidDate(_queryRequest.SalesStartDate) Then
        '    queryResponse.Message = "Invalid Sales Start Date."
        '    queryResponse.Success = False
        '    Return queryResponse
        'End If

        'If Not RG.ValidDate(_queryRequest.SalesEndDate) Then
        '    queryResponse.Message = "Invalid Sales End Date."
        '    queryResponse.Success = False
        '    Return queryResponse
        'End If

        'If Not RG.ValidDate(_queryRequest.PaymentStartDate) Then
        '    queryResponse.Message = "Invalid Payment Start Date."
        '    queryResponse.Success = False
        '    Return queryResponse
        'End If

        'If Not RG.ValidDate(_queryRequest.PaymentEndDate) Then
        '    queryResponse.Message = "Invalid Payment End Date."
        '    queryResponse.Success = False
        '    Return queryResponse
        'End If

        If _queryRequest.NeverPaid = False Then
            If _queryRequest.Amount = "" Then
                queryResponse.Message = "Please fill in an amount of payment."
                queryResponse.Success = False
                Return queryResponse
            End If

            If _queryRequest.MoreLessThan = "" Then
                queryResponse.Message = "Please select >= Or <= from the dropdown."
                queryResponse.Success = False
                Return queryResponse
            End If
        End If

        If _queryRequest.File = "" Then
            queryResponse.Message = "Please select a valid file."
            queryResponse.Success = False
            Return queryResponse
        End If


        If _queryRequest.NeverPaid = True Then
            tmpSQL = "SELECT * FROM (" &
                     "SELECT p.account_number,MAX(p.cell_number) AS cell_number," &
                     "MAX(t.credit_limit) AS credit_limit,MAX(p.itc_rating) AS rating," &
                     "SUM(CASE WHEN t.transaction_type = 'SALE' THEN t.transaction_amount end) AS sales," &
                     "SUM(CASE WHEN t.transaction_type = 'PAY' THEN t.transaction_amount end) as payments " &
                     "FROM debtor_personal p " &
                     "LEFT OUTER JOIN financial_transactions t ON p.account_number = t.account_number " &
                     "LEFT OUTER JOIN debtor_dates d ON p.account_number = d.account_number " &
                     "WHERE ((t.transaction_type = 'SALE' AND t.sale_date " &
                     "BETWEEN '" & _queryRequest.SalesStartDate & "' AND '" & _queryRequest.SalesEndDate & "' " &
                     "OR (t.transaction_type = 'PAY' AND t.sale_date " &
                     "BETWEEN '" & _queryRequest.PaymentStartDate & "' AND '" & _queryRequest.PaymentEndDate & "')) " &
                     "AND d.date_of_creation BETWEEN '" & _queryRequest.AccountStartDate & "' AND " &
                     "'" & _queryRequest.AccountEndDate & "' " &
                     "GROUP BY p.account_number) foo where foo.payments is null ORDER BY foo.account_number ASC;"

        Else
            tmpSQL = "SELECT * FROM (" &
                     "SELECT p.account_number,MAX(p.cell_number)  AS cell_number," &
                     "MAX(t.credit_limit) AS credit_limit,MAX(p.itc_rating) AS rating," &
                     "SUM(CASE WHEN t.transaction_type = 'SALE' THEN t.transaction_amount end) AS sales," &
                     "SUM(CASE WHEN t.transaction_type = 'PAY' THEN t.transaction_amount end) as payments " &
                     "FROM debtor_personal p " &
                     "LEFT OUTER JOIN financial_transactions t ON p.account_number = t.account_number " &
                     "LEFT OUTER JOIN debtor_dates d ON p.account_number = d.account_number " &
                     "WHERE ((t.transaction_type = 'SALE' AND t.sale_date " &
                     "BETWEEN '" & _queryRequest.SalesStartDate & "' AND '" & _queryRequest.SalesEndDate & "') " &
                     "OR (t.transaction_type = 'PAY' AND t.sale_date " &
                     "BETWEEN '" & _queryRequest.PaymentStartDate & "' AND '" & _queryRequest.PaymentEndDate & "')) " &
                     "AND d.date_of_creation BETWEEN '" & _queryRequest.AccountStartDate & "' AND " &
                     "'" & _queryRequest.AccountEndDate & "' " &
                     "GROUP BY p.account_number) foo WHERE " &
                     "foo.payments " & _queryRequest.MoreLessThan & " " & _queryRequest.Amount & " ORDER BY foo.account_number ASC;"
        End If
        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                queryResponse.GetQueryDetails = ds.Tables(0)
                queryResponse.Message = "Record  found."
                queryResponse.Success = True
            Else
                queryResponse.Message = "Record not found."
                queryResponse.Success = False
            End If

        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            queryResponse.Message = "Report Completed"
            queryResponse.Success = False
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        Return queryResponse
    End Function

    Public Function GetCardDetails(_giftCardDetailsRequest As GiftCardDetailsRequest) As GiftCardDetailsResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        If _giftCardDetailsRequest.CardNumber = "" Then
            giftCardDetailsResponse.Message = "Please specify an Card Number before you continue."
            giftCardDetailsResponse.Success = False
            Return giftCardDetailsResponse
        End If

        tmpSQL = "SELECT card_dates.date_created,card_dates.date_modified,card_dates.date_last_used,card_details.current_status," &
                 "card_gift_cards.balance,card_gift_cards.total_spent,card_details.created_by " &
                 "From card_dates " &
                 "Inner Join card_details ON card_details.card_number = card_dates.card_number " &
                 "Inner Join card_gift_cards ON card_details.card_number = card_gift_cards.card_number " &
                 "Where card_details.card_number = '" & RG.Apos(_giftCardDetailsRequest.CardNumber) & "'"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                giftCardDetailsResponse.GiftCardDetails = ds.Tables(0)
            End If
            tmpSQL = "SELECT * FROM financial_transactions WHERE account_number = '" & RG.Apos(_giftCardDetailsRequest.CardNumber) & "' ORDER BY financial_transactions_id ASC"

            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                giftCardDetailsResponse.CardTransactions = ds.Tables(0)
            End If
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            giftCardDetailsResponse.Success = False
            giftCardDetailsResponse.Message = "Something Went Wrong"
            Return giftCardDetailsResponse
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        giftCardDetailsResponse.Success = True
        Return giftCardDetailsResponse

    End Function

    Public Function GetPaymentTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        Dim _dt As New DataTable

        tmpSQL = "SELECT " &
                 "TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date,sale_time,account_number,transaction_number,purchase_amount,interest_amount " &
                 "FROM financial_interest_transactions WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' ORDER BY sale_time"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                _dt = ds.Tables(0)
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

        Return _dt

    End Function


    Public Function GetTransactionListDetails(ByVal TransactionListRequest As TransactionListRequest) As TransactionListResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        If TransactionListRequest.StartDate = "" Then
            transactionListResponse.Success = False
            transactionListResponse.Message = "Please Input a Start Date."
            Return transactionListResponse
        End If

        If TransactionListRequest.EndDate = "" Then
            transactionListResponse.Success = False
            transactionListResponse.Message = "Please Input an End Date."
            Return transactionListResponse
        End If

        'If Not RG.ValidDate(TransactionListRequest.StartDate) Then
        '    TransactionListResponse.Success = False
        '    TransactionListResponse.Message = "Invalid Start Date."
        '    Return TransactionListResponse
        'End If

        'If Not RG.ValidDate(TransactionListRequest.EndDate) Then
        '    TransactionListResponse.Success = False
        '    TransactionListResponse.Message = "Invalid End Date."
        '    Return TransactionListResponse
        'End If

        TransactionListRequest.TransactionType = ""

        If TransactionListRequest.CheckPurchase = True Then
            TransactionListRequest.TransactionType &= "'SALE',"
            TransactionListRequest.FullTransactionType = "Account Sale"
        End If

        If TransactionListRequest.CheckGiftCardPurchase = True Then
            TransactionListRequest.TransactionType &= "'GCS',"
            TransactionListRequest.FullTransactionType = "Gift Card Sale"
        End If

        If TransactionListRequest.CheckPayments = True Then
            TransactionListRequest.TransactionType &= "'PAY',"
            TransactionListRequest.FullTransactionType = "Account Payment"
        End If

        If TransactionListRequest.CheckGiftCardPayments = True Then
            TransactionListRequest.TransactionType &= "'GCP',"
            TransactionListRequest.FullTransactionType = "Gift Card Payment"
        End If

        If TransactionListRequest.CheckCreditNotes = True Then
            TransactionListRequest.TransactionType &= "'CN',"
            TransactionListRequest.FullTransactionType = "Account Credit"
        End If

        If TransactionListRequest.CheckGiftCardCreditNotes = True Then
            TransactionListRequest.TransactionType &= "'GCC',"
            TransactionListRequest.FullTransactionType = "Gift Card Credit"
        End If

        If TransactionListRequest.CheckLostCardProtection = True Then
            TransactionListRequest.TransactionType &= "'LCP',"
            TransactionListRequest.FullTransactionType = "Lost Card Protection"
        End If

        If TransactionListRequest.CheckInterest = True Then
            TransactionListRequest.TransactionType &= "'INT',"
            TransactionListRequest.FullTransactionType = "Interest"
        End If

        If TransactionListRequest.CheckCreditBalanceAffected = True Then
            TransactionListRequest.TransactionType &= "'LEDC',"
            TransactionListRequest.FullTransactionType = "Ledger Credit"
        End If

        If TransactionListRequest.CheckDebitBalanceAffected = True Then
            TransactionListRequest.TransactionType &= "'LEDD',"
            TransactionListRequest.FullTransactionType = "Ledger Debit"
        End If

        If TransactionListRequest.CheckCreditBalanceNotAffected = True Then
            TransactionListRequest.TransactionType &= "'LEDCN',"
            TransactionListRequest.FullTransactionType = "Ledger Credit BNA"
        End If

        If TransactionListRequest.CheckDebitBalanceNotAffected = True Then
            TransactionListRequest.TransactionType &= "'LEDDN',"
            TransactionListRequest.FullTransactionType = "Ledger Debit BNA"
        End If


        If TransactionListRequest.BadDebtWriteOff = True Then
            TransactionListRequest.TransactionType &= "'LEDDN',"
            TransactionListRequest.FullTransactionType = "Bad Debit Write Off"
        End If

        If TransactionListRequest.TransactionType <> "" Then
            TransactionListRequest.TransactionType = TransactionListRequest.TransactionType.Trim().Substring(0,
                                                     TransactionListRequest.TransactionType.Length - 1)
        End If

        PrintTransactionDetails(TransactionListRequest)
        If printTransactionDetailsResponse.Success = True Then
            transactionListResponse.TransactionList = printTransactionDetailsResponse.FinancialTransactionDetails
            transactionListResponse.Success = printTransactionDetailsResponse.Success
        End If

        Return transactionListResponse

    End Function

    Private Function PrintTransactionDetails(ByVal TransactionListRequest As TransactionListRequest)
        Dim _ListOfTransactions As New List(Of Transactions)

        Dim runningBalance As Double = 0

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        'If TransactionListRequest.TransactionType = "" Then
        '    tmpSQL = "SELECT COUNT(sale_date) FROM financial_transactions WHERE sale_date >= '" & TransactionListRequest.StartDate & "' AND sale_date <= '" & TransactionListRequest.EndDate & "'"
        'Else
        '    tmpSQL = "SELECT COUNT(sale_date) FROM financial_transactions WHERE transaction_type IN (" & TransactionListRequest.TransactionType & ") AND sale_date >= '" & TransactionListRequest.StartDate & "' AND sale_date <= '" & TransactionListRequest.EndDate & "'"
        'End If

        'Try
        '    ds = objDB.GetDataSet(tmpSQL)
        '    If objDB.isR(ds) Then
        '        PrintTransactionDetailsResponse.Count = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("count")
        '    End If

        Try
            If TransactionListRequest.TransactionType = "" Then
                tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date," &
                         "sale_time,reference_number,auth_code,username,account_number,transaction_type," &
                         "transaction_amount,current_period,notes " &
                         "FROM financial_transactions " &
                         "WHERE sale_date BETWEEN '" & TransactionListRequest.StartDate & "' AND '" & TransactionListRequest.EndDate & "' " &
                         "ORDER BY sale_date,sale_time ASC"

            Else
                tmpSQL = "SELECT TO_CHAR(sale_date, 'YYYY-MM-DD') AS sale_date," &
                         "sale_time,reference_number,auth_code,username,account_number,transaction_amount," &
                         "transaction_type,current_period,notes " &
                         "FROM financial_transactions " &
                         "WHERE transaction_type In (" & TransactionListRequest.TransactionType & ") And sale_date BETWEEN " &
                         "'" & TransactionListRequest.StartDate & "' AND '" & TransactionListRequest.EndDate & "' " &
                         "ORDER BY sale_date,sale_time ASC"
            End If

            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then

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
                    _Transaction.tAccount_Number = dr("account_number") & ""
                    _Transaction.tNotes = dr("notes") & ""
                    _Transaction.tAuth_Code = Convert.ToString(dr("auth_code"))
                    _ListOfTransactions.Add(_Transaction)
                Next
                printTransactionDetailsResponse.FinancialTransactionDetails = _ListOfTransactions
            End If

        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            printTransactionDetailsResponse.Success = False
            printTransactionDetailsResponse.Message = "Something Went Wrong"
            Return printTransactionDetailsResponse
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        printTransactionDetailsResponse.Success = True
        Return printTransactionDetailsResponse

    End Function

    'Public Function GetReport(_reportRequest As ReportRequest) As ReportResponse
    '    Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")
    '    Dim tmpMonthInt As String
    '    Dim NextMonth As String
    '    Dim NextYear As String
    '    Dim csv As String = String.Empty
    '    If _reportRequest.FileName = "" Then
    '        reportResponse.Message = "Please select a valid file."
    '        reportResponse.Success = False
    '        Return reportResponse
    '    End If

    '    If _reportRequest.Month = "" Then
    '        reportResponse.Message = "Please select a Month."
    '        reportResponse.Success = False
    '        Return reportResponse
    '    End If


    '    If _reportRequest.Year = "" Then
    '        reportResponse.Message = "Please select a Year."
    '        reportResponse.Success = False
    '        Return reportResponse
    '    End If


    '    If _reportRequest.Status = "" Then
    '        reportResponse.Message = "Please select a Status."
    '        reportResponse.Success = False
    '        Return reportResponse
    '    End If

    '    Select Case _reportRequest.Month

    '        Case "January"
    '            tmpMonthInt = "01"
    '            NextMonth = "02"
    '            NextYear = _reportRequest.Year
    '        Case "February"
    '            tmpMonthInt = "02"
    '            NextMonth = "03"
    '            NextYear = _reportRequest.Year
    '        Case "March"
    '            tmpMonthInt = "03"
    '            NextMonth = "04"
    '            NextYear = _reportRequest.Year
    '        Case "April"
    '            tmpMonthInt = "04"
    '            NextMonth = "05"
    '            NextYear = _reportRequest.Year
    '        Case "May"
    '            tmpMonthInt = "05"
    '            NextMonth = "06"
    '            NextYear = _reportRequest.Year
    '        Case "June"
    '            tmpMonthInt = "06"
    '            NextMonth = "07"
    '            NextYear = _reportRequest.Year
    '        Case "July"
    '            tmpMonthInt = "07"
    '            NextMonth = "08"
    '            NextYear = _reportRequest.Year
    '        Case "August"
    '            tmpMonthInt = "08"
    '            NextMonth = "09"
    '            NextYear = _reportRequest.Year
    '        Case "September"
    '            tmpMonthInt = "09"
    '            NextMonth = "10"
    '            NextYear = _reportRequest.Year
    '        Case "October"
    '            tmpMonthInt = "10"
    '            NextMonth = "11"
    '            NextYear = _reportRequest.Year
    '        Case "November"
    '            tmpMonthInt = "11"
    '            NextMonth = "12"
    '            NextYear = _reportRequest.Year
    '        Case "December"
    '            tmpMonthInt = "12"
    '            NextMonth = "01"
    '            NextYear = Val(_reportRequest.Year) + 1
    '    End Select

    '    Try
    '        'Take care of the blanks which would cause a numeric error
    '        tmpSQL = "  "
    '        objDB.ExecuteQuery(tmpSQL)

    '        'Get the internal period for the month
    '        Dim internal_period As Integer

    '        tmpSQL = "SELECT internal_period FROM internal_period_to_date WHERE real_month = '" & _reportRequest.Month & "' " &
    '                 "AND real_year = '" & _reportRequest.Year & "'"
    '        Ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                internal_period = Val(dr("internal_period") & "")
    '            Next
    '        End If

    '        Dim number_of_accounts As String
    '        Dim credit_limit_average As String
    '        Dim average_rating As String
    '        Dim zero_count As String

    '        ''lblStatus = "Getting Header"


    '        'Get the heading counts
    '        tmpSQL = "SELECT COUNT(dp.account_number) AS number_of_accounts," &
    '                 "AVG(financial_balances.credit_limit) AS credit_limit_average," &
    '                 "AVG(CASE WHEN cast(itc_rating as integer) > 100 THEN cast(itc_rating as integer) END) AS average_rating, "

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & "COUNT(CASE WHEN cast(itc_rating as integer) > 5 THEN itc_rating END) AS zero_count "
    '        Else
    '            If _reportRequest.Score = "" Then
    '                tmpSQL = tmpSQL & "COUNT(CASE WHEN cast(itc_rating as integer) < 100 THEN itc_rating END) AS zero_count "
    '            Else
    '                tmpSQL = tmpSQL & "COUNT(CASE WHEN itc_rating = '" & _reportRequest.Score & "' THEN itc_rating END) AS zero_count "
    '            End If
    '        End If

    '        tmpSQL = tmpSQL & "FROM debtor_personal dp " &
    '                          "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "INNER JOIN financial_balances ON dp.account_number = financial_balances.account_number "


    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON dp.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11' " &
    '                          "AND total_spent > 0 "

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                              "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckZeroes = True Then
    '            tmpSQL = tmpSQL & " AND itc_rating = '0'"
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows

    '                number_of_accounts = RG.Num(dr("number_of_accounts").ToString)
    '                credit_limit_average = RG.Num(dr("credit_limit_average").ToString)
    '                average_rating = RG.Num(dr("average_rating").ToString)
    '                zero_count = RG.Num(dr("zero_count").ToString)
    '            Next
    '        End If

    '        csv &= "Vintage Report"
    '        csv &= vbCr & vbLf

    '        csv &= "Date Of Report:," & Format(Now, "yyyy-MM-dd")
    '        csv &= vbCr & vbLf

    '        csv &= "For Period:," & _reportRequest.Month & " " & _reportRequest.Year
    '        csv &= vbCr & vbLf

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        csv &= "Accounts Opened:," & number_of_accounts
    '        csv &= vbCr & vbLf

    '        csv &= "Average Score (> 100):," & average_rating
    '        csv &= vbCr & vbLf

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            csv &= "# of Accounts Score > 5 :," & zero_count
    '        Else
    '            If _reportRequest.Score = "" Then
    '                csv &= "# of Accounts < 100:," & zero_count
    '            Else
    '                csv &= "# of Accounts Score: " & _reportRequest.Score & "," & zero_count
    '            End If
    '        End If
    '        csv &= vbCr & vbLf

    '        csv &= "Average Credit Limit:," & credit_limit_average
    '        csv &= vbCr & vbLf

    '        If _reportRequest.Score <> "" Then
    '            csv &= "Score:," & _reportRequest.Score
    '            csv &= vbCr & vbLf
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf








    '        'CURRENT
    '        '==========================================================================================================================================
    '        '' lblStatus = "CURRENT"

    '        tmpSQL = "SELECT " &
    '                     "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_balances.account_number END) AS p60_overdue," &
    '                     "COUNT(CASE WHEN p90 > 15 AND p120 + p150 < 15 THEN financial_balances.account_number END) AS p90_overdue," &
    '                     "COUNT(CASE WHEN p120 > 15 AND p150 < 15 THEN financial_balances.account_number END) AS p120_overdue," &
    '                     "COUNT(CASE WHEN p150 > 15 THEN financial_balances.account_number END) AS p150_overdue," &
    '                     "SUM(total) AS total_balance," &
    '                     "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '                     "SUM(CASE WHEN p90 > 15 AND p120 + p150 < 15 THEN total END) AS p90_overdue_balance, " &
    '                     "SUM(CASE WHEN p120 > 15 AND p150 < 15 THEN total END) AS p120_overdue_balance, " &
    '                     "SUM(CASE WHEN p150 > 15 THEN total END) AS p150_overdue_balance " &
    '                     "FROM financial_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE financial_balances.account_number IN " &
    '                 "(SELECT dp.account_number " &
    '                 "FROM debtor_personal dp "

    '        tmpSQL = tmpSQL & "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND " &
    '                          "'" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                              "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        'If chkZeroes.value = 1 Then
    '        '    tmpSQL = tmpSQL & " AND itc_rating = '0'"
    '        'End If

    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows

    '                csv &= "#A/cs 60+ dpd  @ CURRENT:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ CURRENT:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 120+ dpd  @ CURRENT:," & RG.Num(dr("p120_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 150+ dpd  @ CURRENT:," & RG.Num(dr("p150_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ CURRENT:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ CURRENT:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ CURRENT:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 120+dpd @ CURRENT:," & RG.Num(dr("p120_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 150+dpd @ CURRENT:," & RG.Num(dr("p150_overdue_balance").ToString & "") & vbCr & vbLf
    '            Next
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf


    '        If _reportRequest.CheckIncludeAllPeriods = True Then
    '            '==========================================================================================================================================
    '            '2 MOB
    '            '==========================================================================================================================================
    '            ''lblStatus = "2 MOB"

    '            tmpSQL = "SELECT " &
    '                     "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '                     "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '                     "SUM(total) AS total_balance," &
    '                     "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '                     "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '                     "FROM financial_closing_balances "

    '            'If _reportRequest.CheckBadDebtStoresonly = True Then
    '            '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '            'End If

    '            tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 2 & "' " &
    '                              "AND financial_closing_balances.account_number IN " &
    '                              "(SELECT dp.account_number " &
    '                              "FROM debtor_personal dp " &
    '                              "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                              "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '            If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '                tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '            Else
    '                If _reportRequest.Status <> "ALL" Then
    '                    tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '                End If
    '            End If

    '            If _reportRequest.CheckThickFilesOnly = True Then
    '                tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '            Else

    '                If _reportRequest.Score <> "" Then
    '                    tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '                End If
    '            End If

    '            If _reportRequest.CheckMaleOnly = True Then
    '                tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '            End If


    '            tmpSQL = tmpSQL & ")"

    '            'If _reportRequest.CheckBadDebtStoresonly = True Then
    '            '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '            'End If

    '            ds = objDB.GetDataSet(tmpSQL)
    '            If objDB.isR(ds) Then
    '                For Each dr As DataRow In ds.Tables(0).Rows


    '                    csv &= "#A/cs 60+ dpd  @ 2MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                    csv &= "#A/cs 90+ dpd  @ 2MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance @ 2MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance 60+dpd @ 2MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance 90+dpd @ 2MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '                Next
    '            End If

    '            csv &= vbCr & vbLf
    '            csv &= vbCr & vbLf

    '            '==========================================================================================================================================
    '        End If











    '        '==========================================================================================================================================
    '        '3 MOB
    '        '==========================================================================================================================================
    '        ''lblStatus = "3 MOB"

    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '                 "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '                 "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 3 & "' " &
    '                          "AND financial_closing_balances.account_number IN " &
    '                          "(SELECT dp.account_number " &
    '                          "FROM debtor_personal dp " &
    '                          "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                              "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If

    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows

    '                csv &= "#A/cs 60+ dpd  @ 3MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 3MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 3MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 3MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 3MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf
    '            Next
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        If _reportRequest.CheckIncludeAllPeriods = True Then
    '            '==========================================================================================================================================
    '            '4 MOB
    '            '==========================================================================================================================================
    '            ''lblStatus = "4 MOB"

    '            tmpSQL = "SELECT " &
    '                     "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '                     "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '                     "SUM(total) AS total_balance," &
    '                     "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '                     "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '                     "FROM financial_closing_balances "

    '            'If _reportRequest.CheckBadDebtStoresonly = True Then
    '            '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '            'End If

    '            tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 4 & "' " &
    '                              "AND financial_closing_balances.account_number IN " &
    '                              "(SELECT dp.account_number " &
    '                              "FROM debtor_personal dp " &
    '                              "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                              "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '            If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '                tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '            Else
    '                If _reportRequest.Status <> "ALL" Then
    '                    tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '                End If
    '            End If

    '            If _reportRequest.CheckThickFilesOnly = True Then
    '                tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '            Else
    '                If _reportRequest.Score <> "" Then
    '                    tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '                End If
    '            End If

    '            If _reportRequest.CheckMaleOnly = True Then
    '                tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '            End If


    '            tmpSQL = tmpSQL & ")"

    '            'If _reportRequest.CheckBadDebtStoresonly = True Then
    '            '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '            'End If

    '            ds = objDB.GetDataSet(tmpSQL)
    '            If objDB.isR(ds) Then
    '                For Each dr As DataRow In ds.Tables(0).Rows
    '                    csv &= "#A/cs 60+ dpd  @ 4MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                    csv &= "#A/cs 90+ dpd  @ 4MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance @ 4MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance 60+dpd @ 4MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance 90+dpd @ 4MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '                Next
    '            End If

    '            csv &= vbCr & vbLf
    '            csv &= vbCr & vbLf

    '            '==========================================================================================================================================

    '            '==========================================================================================================================================
    '            '5 MOB
    '            '==========================================================================================================================================
    '            'lblStatus = "5 MOB"

    '            tmpSQL = "SELECT " &
    '                     "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '                     "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '                     "SUM(total) AS total_balance," &
    '                     "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '                     "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '                     "FROM financial_closing_balances "

    '            'If _reportRequest.CheckBadDebtStoresonly = True Then
    '            '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '            'End If

    '            tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 5 & "' " &
    '                              "AND financial_closing_balances.account_number IN " &
    '                              "(SELECT dp.account_number " &
    '                              "FROM debtor_personal dp " &
    '                              "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                              "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '            If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '                tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '            Else
    '                If _reportRequest.Status <> "ALL" Then
    '                    tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '                End If
    '            End If

    '            'If _reportRequest.CheckThickFilesOnly = True Then
    '            '    tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '            'Else
    '            '    If _reportRequest.Score = "" Then
    '            '        tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            '    End If
    '            'End If

    '            If _reportRequest.CheckThickFilesOnly = True Then
    '                tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '            Else
    '                If _reportRequest.Score <> "" Then
    '                    tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '                End If
    '            End If


    '            If _reportRequest.CheckMaleOnly = True Then
    '                tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '            End If


    '            tmpSQL = tmpSQL & ")"

    '            'If _reportRequest.CheckBadDebtStoresonly = True Then
    '            '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '            'End If

    '            ds = objDB.GetDataSet(tmpSQL)
    '            If objDB.isR(ds) Then
    '                For Each dr As DataRow In ds.Tables(0).Rows
    '                    csv &= "#A/cs 60+ dpd  @ 5MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                    csv &= "#A/cs 90+ dpd  @ 5MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance @ 5MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance 60+dpd @ 5MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance 90+dpd @ 5MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '                Next
    '            End If

    '            csv &= vbCr & vbLf
    '            csv &= vbCr & vbLf


    '            '==========================================================================================================================================

    '        End If


    '        '==========================================================================================================================================
    '        '6 MOB
    '        '==========================================================================================================================================
    '        ''lblStatus = "6 MOB"

    '        tmpSQL = "SELECT " &
    '         "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '         "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '         "SUM(total) AS total_balance," &
    '         "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '         "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '         "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 6 & "' " &
    '                  "AND financial_closing_balances.account_number IN " &
    '                  "(SELECT dp.account_number " &
    '                  "FROM debtor_personal dp " &
    '                  "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                  "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                      "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 6MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 6MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 6MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 6MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 6MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf
    '            Next
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf


    '        '==========================================================================================================================================

    '        If _reportRequest.CheckIncludeAllPeriods = True Then

    '            '==========================================================================================================================================
    '            '7 MOB
    '            '==========================================================================================================================================
    '            ''lblStatus = "7 MOB"

    '            tmpSQL = "SELECT " &
    '             "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '             "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '             "SUM(total) AS total_balance," &
    '             "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '             "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '             "FROM financial_closing_balances "

    '            'If _reportRequest.CheckBadDebtStoresonly = True Then
    '            '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '            'End If

    '            tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 7 & "' " &
    '                      "AND financial_closing_balances.account_number IN " &
    '                      "(SELECT dp.account_number " &
    '                      "FROM debtor_personal dp " &
    '                      "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                      "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '            If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '                tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                          "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '            Else
    '                If _reportRequest.Status <> "ALL" Then
    '                    tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '                End If
    '            End If

    '            If _reportRequest.CheckThickFilesOnly = True Then
    '                tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '            Else
    '                If _reportRequest.Score <> "" Then
    '                    tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '                End If
    '            End If

    '            If _reportRequest.CheckMaleOnly = True Then
    '                tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '            End If


    '            tmpSQL = tmpSQL & ")"

    '            If _reportRequest.CheckBadDebtStoresonly = True Then
    '                tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '            End If
    '            ds = objDB.GetDataSet(tmpSQL)
    '            If objDB.isR(ds) Then
    '                For Each dr As DataRow In ds.Tables(0).Rows
    '                    csv &= "#A/cs 60+ dpd  @ 7MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                    csv &= "#A/cs 90+ dpd  @ 7MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance @ 7MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance 60+dpd @ 7MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                    csv &= "Total balance 90+dpd @ 7MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf
    '                Next
    '            End If

    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        '==========================================================================================================================================

    '        '==========================================================================================================================================
    '        '8 MOB
    '        '==========================================================================================================================================
    '        'lblStatus = "8 MOB"

    '        tmpSQL = "SELECT " &
    '             "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '             "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '             "SUM(total) AS total_balance," &
    '             "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '             "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '             "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 8 & "' " &
    '                  "AND financial_closing_balances.account_number IN " &
    '                  "(SELECT dp.account_number " &
    '                  "FROM debtor_personal dp " &
    '                  "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                  "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                      "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 8MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 8MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 8MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 8MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 8MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '            Next

    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        '==========================================================================================================================================

    '        '==========================================================================================================================================
    '        '9 MOB
    '        '==========================================================================================================================================
    '        ''lblStatus = "9 MOB"

    '        tmpSQL = "SELECT " &
    '         "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '         "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '         "SUM(total) AS total_balance," &
    '         "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '         "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '         "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 9 & "' " &
    '                  "AND financial_closing_balances.account_number IN " &
    '                  "(SELECT dp.account_number " &
    '                  "FROM debtor_personal dp " &
    '                  "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                  "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                      "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If


    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 9MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 9MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 9MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 9MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 9MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '            Next
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf


    '        '==========================================================================================================================================

    '        '==========================================================================================================================================
    '        '10 MOB
    '        '==========================================================================================================================================
    '        '' lblStatus = "10 MOB"


    '        tmpSQL = "SELECT " &
    '         "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '         "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '         "SUM(total) AS total_balance," &
    '         "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '         "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '         "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 10 & "' " &
    '                  "AND financial_closing_balances.account_number IN " &
    '                  "(SELECT dp.account_number " &
    '                  "FROM debtor_personal dp " &
    '                  "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                  "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                      "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 10MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 10MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 10MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 10MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 10MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '            Next

    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        '==========================================================================================================================================

    '        '==========================================================================================================================================
    '        '11 MOB
    '        '==========================================================================================================================================
    '        ''lblStatus = "11 MOB"


    '        tmpSQL = "SELECT " &
    '         "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '         "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '         "SUM(total) AS total_balance," &
    '         "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 < 15 THEN total END) AS p60_overdue_balance, " &
    '         "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '         "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 11 & "' " &
    '                  "AND financial_closing_balances.account_number IN " &
    '                  "(SELECT dp.account_number " &
    '                  "FROM debtor_personal dp " &
    '                  "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                  "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                      "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 11MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 11MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 11MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 11MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 11MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '            Next

    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        ''==========================================================================================================================================
    '        '    End If

    '        '    '==========================================================================================================================================
    '        '    '12 MOB
    '        '    '==========================================================================================================================================
    '        '    lblStatus = "12 MOB"


    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 <15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '                 "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 <15 THEN total END) AS p60_overdue_balance, " &
    '                 "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 12 & "' " &
    '                          "AND financial_closing_balances.account_number IN " &
    '                          "(SELECT dp.account_number " &
    '                          "FROM debtor_personal dp " &
    '                          "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 12MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 12MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 12MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 12MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 12MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '            Next

    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        '13 MOB

    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p150 > 30 THEN financial_closing_balances.account_number END) AS number_of_overdue_accounts," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p150 > 30 THEN total END) AS overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 12 & "' " &
    '              "AND financial_closing_balances.account_number IN " &
    '              "(SELECT dp.account_number " &
    '              "FROM debtor_personal dp "

    '        tmpSQL = tmpSQL & "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND " &
    '                          "'" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If
    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If
    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If

    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 30+ dpd  @ 12MOB:," & RG.Num(dr("number_of_overdue_accounts").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 12MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 150+dpd @ 12MOB:," & RG.Num(dr("overdue_balance").ToString & "") & vbCr & vbLf
    '            Next

    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        ''==========================================================================================================================================

    '        '        '==========================================================================================================================================

    '        '        '==========================================================================================================================================
    '        '        '18 MOB
    '        '        '==========================================================================================================================================
    '        '        lblStatus = "18 MOB"


    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 <15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '                 "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 <15 THEN total END) AS p60_overdue_balance, " &
    '                 "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 18 & "' " &
    '              "AND financial_closing_balances.account_number IN " &
    '              "(SELECT dp.account_number " &
    '              "FROM debtor_personal dp " &
    '              "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '              "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 18MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 18MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 18MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 18MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 18MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '            Next
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        ''
    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p150 > 30 THEN financial_closing_balances.account_number END) AS number_of_overdue_accounts," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p150 > 30 THEN total END) AS overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 18 & "' " &
    '              "AND financial_closing_balances.account_number IN " &
    '              "(SELECT dp.account_number " &
    '              "FROM debtor_personal dp "

    '        tmpSQL = tmpSQL & "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND " &
    '                          "'" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If
    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If
    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If

    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 30+ dpd  @ 18MOB:," & RG.Num(dr("number_of_overdue_accounts").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 18MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 150+dpd @ 18MOB:," & RG.Num(dr("overdue_balance").ToString & "") & vbCr & vbLf
    '            Next
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        ''==========================================================================================================================================

    '        '        '==========================================================================================================================================
    '        '        '24 MOB
    '        '        '==========================================================================================================================================
    '        '        lblStatus = "24 MOB"


    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 <15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '                 "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 <15 THEN total END) AS p60_overdue_balance, " &
    '                 "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 24 & "' " &
    '              "AND financial_closing_balances.account_number IN " &
    '              "(SELECT dp.account_number " &
    '              "FROM debtor_personal dp " &
    '              "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '              "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 24MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 24MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 24MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 24MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 24MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf

    '            Next
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        ''
    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p150 > 30 THEN financial_closing_balances.account_number END) AS number_of_overdue_accounts," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p150 > 30 THEN total END) AS overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        If _reportRequest.CheckBadDebtStoresonly = True Then
    '            tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 24 & "' " &
    '                          "AND financial_closing_balances.account_number IN " &
    '                          "(SELECT dp.account_number " &
    '                          "FROM debtor_personal dp "

    '        tmpSQL = tmpSQL & "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND " &
    '                          "'" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If
    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If
    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If

    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 30+ dpd  @ 24MOB:," & RG.Num(dr("number_of_overdue_accounts").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 24MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 150+dpd @ 24MOB:," & RG.Num(dr("overdue_balance").ToString & "") & vbCr & vbLf

    '            Next
    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '        '==========================================================================================================================================


    '        '==========================================================================================================================================
    '        '36 MOB
    '        '==========================================================================================================================================
    '        ''lblStatus = "36 MOB"


    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p60 > 15 AND p90 + p120 + p150 <15 THEN financial_closing_balances.account_number END) AS p60_overdue," &
    '                 "COUNT(CASE WHEN p90 > 15 THEN financial_closing_balances.account_number END) AS p90_overdue," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p60 > 15 AND p90 + p120 + p150 <15 THEN total END) AS p60_overdue_balance, " &
    '                 "SUM(CASE WHEN p90 > 15 THEN total END) AS p90_overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 36 & "' " &
    '                          "AND financial_closing_balances.account_number IN " &
    '                          "(SELECT dp.account_number " &
    '                          "FROM debtor_personal dp " &
    '                          "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND '" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If

    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If


    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 60+ dpd  @ 36MOB:," & RG.Num(dr("p60_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "#A/cs 90+ dpd  @ 36MOB:," & RG.Num(dr("p90_overdue").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 36MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 60+dpd @ 36MOB:," & RG.Num(dr("p60_overdue_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 90+dpd @ 36MOB:," & RG.Num(dr("p90_overdue_balance").ToString & "") & vbCr & vbLf
    '            Next

    '        End If

    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf


    '        tmpSQL = "SELECT " &
    '                 "COUNT(CASE WHEN p150 > 30 THEN financial_closing_balances.account_number END) AS number_of_overdue_accounts," &
    '                 "SUM(total) AS total_balance," &
    '                 "SUM(CASE WHEN p150 > 30 THEN total END) AS overdue_balance " &
    '                 "FROM financial_closing_balances "

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & "INNER JOIN debtor_first_purchase dfp ON financial_closing_balances.account_number = dfp.account_number "
    '        'End If

    '        tmpSQL = tmpSQL & "WHERE current_period = '" & Val(internal_period) + 36 & "' " &
    '              "AND financial_closing_balances.account_number IN " &
    '              "(SELECT dp.account_number " &
    '              "FROM debtor_personal dp "

    '        tmpSQL = tmpSQL & "INNER JOIN debtor_dates ON dp.account_number = debtor_dates.account_number " &
    '                          "WHERE date_of_creation BETWEEN '" & _reportRequest.Year & "-" & tmpMonthInt & "-12' AND " &
    '                          "'" & NextYear & "-" & NextMonth & "-11'"

    '        If _reportRequest.Status = "A,L,D,DR,C,F,WO,B" Then
    '            tmpSQL = tmpSQL & " AND (status = 'ACTIVE' OR status = 'LEGAL' OR status = 'DECEASED' OR status = 'DEBT REVIEW' " &
    '                  "OR status = 'CLOSED' OR status = 'FRAUD' OR status = 'WRITE-OFF' OR status = 'BLOCKED')"
    '        Else
    '            If _reportRequest.Status <> "ALL" Then
    '                tmpSQL = tmpSQL & " AND status = '" & _reportRequest.Status & "'"
    '            End If
    '        End If
    '        If _reportRequest.CheckThickFilesOnly = True Then
    '            tmpSQL = tmpSQL & " AND cast(itc_rating as integer) > 5 "
    '        Else
    '            If _reportRequest.Score <> "" Then
    '                tmpSQL = tmpSQL & " AND itc_rating = '" & _reportRequest.Score & "'"
    '            End If
    '        End If
    '        If _reportRequest.CheckMaleOnly = True Then
    '            tmpSQL = tmpSQL & " AND gender = 'MALE'"
    '        End If

    '        tmpSQL = tmpSQL & ")"

    '        'If _reportRequest.CheckBadDebtStoresonly = True Then
    '        '    tmpSQL = tmpSQL & " AND first_purchase IN (SELECT branch_code FROM no_self_activate)"
    '        'End If

    '        ds = objDB.GetDataSet(tmpSQL)
    '        If objDB.isR(ds) Then
    '            For Each dr As DataRow In ds.Tables(0).Rows
    '                csv &= "#A/cs 30+ dpd  @ 36MOB:," & RG.Num(dr("number_of_overdue_accounts").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance @ 36MOB:," & RG.Num(dr("total_balance").ToString & "") & vbCr & vbLf
    '                csv &= "Total balance 150+dpd @ 36MOB:," & RG.Num(dr("overdue_balance").ToString & "") & vbCr & vbLf
    '            Next

    '        End If
    '        csv &= vbCr & vbLf
    '        csv &= vbCr & vbLf

    '    Catch ex As Exception
    '        If (objDB IsNot Nothing) Then
    '            objDB.CloseConnection()
    '        End If
    '        queryResponse.Message = "Report Completed"
    '        queryResponse.Success = False
    '    Finally
    '        If (objDB IsNot Nothing) Then
    '            objDB.CloseConnection()
    '        End If
    '    End Try
    '    reportResponse.CSV = csv
    '    reportResponse.Success = True
    '    Return reportResponse

    'End Function

    Public Function GetQuery(ByVal NewAccountRequest As NewAccountRequest) As GetQueryResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        'If Not RG.ValidDate(NewAccountRequest.StartDate) Then
        '    queryResponse.Message = "Invalid Start Date."
        '    queryResponse.Success = False
        '    Return queryResponse

        'End If
        'If Not RG.ValidDate(NewAccountRequest.EndDate) Then
        '    queryResponse.Message = "Invalid End Date."
        '    queryResponse.Success = False
        '    Return queryResponse
        'End If

        If NewAccountRequest.Status = "" Then
            _GetQueryResponse.Message = "Please select a Status."
            _GetQueryResponse.Success = False
            Return _GetQueryResponse
        End If


        tmpSQL = "SELECT COUNT (debtor_personal.account_number) FROM debtor_personal " &
                 "LEFT OUTER JOIN debtor_dates ON debtor_personal.account_number = debtor_dates.account_number " &
                 "LEFT OUTER JOIN card_details ON debtor_personal.account_number = card_details.account_number " &
                 "WHERE debtor_dates.date_of_creation >= '" & NewAccountRequest.StartDate & "' AND " &
                 "debtor_dates.date_of_creation <= '" & NewAccountRequest.EndDate & "'"

        If NewAccountRequest.Status <> "ALL" Then
            tmpSQL = tmpSQL & " AND debtor_personal.status = '" & NewAccountRequest.Status & "'"
        End If

        If NewAccountRequest.CheckCardIssued = True Then
            tmpSQL = tmpSQL & " AND card_details.card_number <> ''"
        End If
        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                _GetQueryResponse.Count = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("count")
            Else
                _GetQueryResponse.Count = 0
            End If


            'tmpSQL = "SELECT COUNT (debtor_personal.account_number) FROM debtor_personal " &
            '"LEFT OUTER JOIN debtor_dates ON debtor_personal.account_number = debtor_dates.account_number " &
            '"LEFT OUTER JOIN card_details ON debtor_personal.account_number = card_details.account_number " &
            '"WHERE debtor_dates.date_of_creation >= '" & NewAccountRequest.StartDate & "' AND " &
            '"debtor_dates.date_of_creation <= '" & NewAccountRequest.EndDate & "' AND " &
            '"debtor_personal.preferred_name = 'TU'"

            'If NewAccountRequest.Status <> "ALL" Then
            '    tmpSQL = tmpSQL & " AND debtor_personal.status = '" & NewAccountRequest.Status & "'"
            'End If

            'If NewAccountRequest.CheckCardIssued = True Then
            '    tmpSQL = tmpSQL & " AND card_details.card_number <> ''"
            'End If

            'ds = objDB.GetDataSet(tmpSQL)
            'If objDB.isR(ds) Then
            '    _GetQueryResponse.PreferredCount = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("count")
            'Else
            '    _GetQueryResponse.PreferredCount = 0
            'End If

        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            _GetQueryResponse.Success = False
            _GetQueryResponse.Message = "Something Went Wrong"
            Return _GetQueryResponse
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        _GetQueryResponse.Success = True
        Return _GetQueryResponse
    End Function

    Public Function CardIssued(ByVal NewAccountRequest As NewAccountRequest) As CardIssuedResponse
        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")

        'If Not RG.ValidDate(NewAccountRequest.StartDate) Then
        '    CardIssuedResponse.Message = "Invalid Start Date."
        '    CardIssuedResponse.Success = False
        '    Return CardIssuedResponse
        'End If

        'If Not RG.ValidDate(NewAccountRequest.EndDate) Then
        '    CardIssuedResponse.Message = "Invalid End Date."
        '    CardIssuedResponse.Success = False
        '    Return CardIssuedResponse
        'End If


        tmpSQL = "SELECT COUNT(card_number) FROM card_dates WHERE date_assigned BETWEEN '" & NewAccountRequest.StartDate & "' " &
                 "AND '" & NewAccountRequest.EndDate & "'"
        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                cardIssuedResponse.Count = ds.Tables(0).Rows(ds.Tables(0).Rows.Count - 1)("count")
            Else
                cardIssuedResponse.Count = 0
            End If
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
            cardIssuedResponse.Success = False
            cardIssuedResponse.Message = "Something Went Wrong"
            Return cardIssuedResponse
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try
        cardIssuedResponse.Success = True
        Return cardIssuedResponse
    End Function

    Public Function GetNumbersForSMS(ByVal Type As String, ByVal BranchCode As String) As DataTable

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMRead")
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")


        Dim _tempds As New DataSet
        Dim _dt As New DataTable("entries")

        Select Case Type
            Case "PhoneNumbersAll"
                tmpSQL = "Select " &
                 "27 || substr(cell_number,2,2) || substr(cell_number,5,3) || substr(cell_number,9,4) As numto, financial_balances.account_number as customerid " &
                 "FROM " &
                 "financial_balances " &
                 "INNER JOIN debtor_personal On financial_balances.account_number = debtor_personal.account_number " &
                 "INNER JOIN debtor_dates On financial_balances.account_number = debtor_dates.account_number " &
                 "INNER JOIN card_details On financial_balances.account_number = card_details.account_number " &
                 "WHERE " &
                 "dont_send_sms = False " &
                 "And length(cell_number) = 12 "


                Try
                    _tempds = objDB.GetDataSet(tmpSQL)
                    If objDB.isR(_tempds) Then
                        _dt = _tempds.Tables(0)
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

                tmpSQL = "SELECT 27 || substr(contact_number,2,2) || substr(contact_number,5,3) || substr(contact_number,9,4) AS numto, account_number as customerid " &
                         "FROM customer_contact_details " &
                         "where opted_out = FALSE and " &
                         "number_failed = FALSE " &
                         "and length(contact_number) = 12"

                Try
                    _tempds = objDBPositive.GetDataSet(tmpSQL)
                    If objDBPositive.isR(_tempds) Then
                        _dt.Merge(_tempds.Tables(0))
                    End If

                Catch ex As Exception
                    If (objDBPositive IsNot Nothing) Then
                        objDBPositive.CloseConnection()
                    End If
                    Return Nothing
                Finally
                    If (objDBPositive IsNot Nothing) Then
                        objDBPositive.CloseConnection()
                    End If
                End Try


            Case "PhoneNumbersBuy"
                tmpSQL = "SELECT " &
                 "27 || substr(cell_number,2,2) || substr(cell_number,5,3) || substr(cell_number,9,4) As numto, financial_balances.account_number as customerid " &
                 "FROM " &
                 "financial_balances " &
                 "INNER JOIN debtor_personal ON financial_balances.account_number = debtor_personal.account_number " &
                 "INNER JOIN debtor_dates ON financial_balances.account_number = debtor_dates.account_number " &
                 "INNER JOIN card_details ON financial_balances.account_number = card_details.account_number " &
                 "WHERE " &
                 "financial_balances.p90 = 0 And " &
                 "financial_balances.p120 = 0 AND " &
                 "financial_balances.p150 = 0 And " &
                 "debtor_personal.status = 'ACTIVE' " &
                 "And current_status = 'ACTIVE' " &
                 "and card_details.card_number <> '' " &
                 "And dont_send_sms = FALSE " &
                 "And cell_number <> '000-000-0000' " &
                 "and length(cell_number) = 12"



                Try
                    _tempds = objDB.GetDataSet(tmpSQL)
                    If objDB.isR(_tempds) Then
                        _dt = _tempds.Tables(0)
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

                tmpSQL = "SELECT 27 || substr(contact_number,2,2) || substr(contact_number,5,3) || substr(contact_number,9,4) AS numto, account_number as customerid " &
                 "FROM customer_contact_details " &
                 "where opted_out = FALSE and " &
                 "number_failed = FALSE " &
                 "and length(contact_number) = 12"


                Try
                    _tempds = objDBPositive.GetDataSet(tmpSQL)
                    If objDBPositive.isR(_tempds) Then
                        _dt.Merge(_tempds.Tables(0))
                    End If

                Catch ex As Exception
                    If (objDBPositive IsNot Nothing) Then
                        objDBPositive.CloseConnection()
                    End If
                    Return Nothing
                Finally
                    If (objDBPositive IsNot Nothing) Then
                        objDBPositive.CloseConnection()
                    End If
                End Try

            Case "Branch"
                tmpSQL = "SELECT " &
                 "27 || substr(cell_number,2,2) || substr(cell_number,5,3) || substr(cell_number,9,4) As numto, account_number as customerid " &
                 "FROM debtor_personal " &
                 "where branch_code = '" & BranchCode & "' " &
                 "And dont_send_sms = FALSE " &
                 "and length(cell_number) = 12"


                Try
                    ds = objDB.GetDataSet(tmpSQL)
                    If objDB.isR(ds) Then
                        _dt = ds.Tables(0)
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
        End Select

        Return _dt

    End Function


    Public Function RunVintageReport(ByVal Email As String, ByVal Username As String, ByVal Json As String) As String
        Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPositiveWrite")

        tmpSQL = "INSERT INTO tasks (task_type,
                                     file_to_run,
                                    email_addresses,
                                     timestamp_created,
                                     username,
                                     additional_options) VALUES " &
                                    "('vintage_report',
                                    '',
                                    '" & Email & "',
                                    '" & Format(Now, "yyyy-MM-dd HH:mm") & "',
                                     '" & Username & "',
                                    '" & Json & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Throw ex
            Return "False"
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function


    Public Function GetCellphoneNumbersByLoayaltyAccounts() As String
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim csv As String = String.Empty

        Dim FileName As String

        Dim tGuID As String = Guid.NewGuid.ToString

        FileName = HttpContext.Current.Server.MapPath("~\Docs\" & tGuID & ".csv")

        Dim CreateFile As New FileStream(FileName, FileMode.Append)
        Dim strStreamWriter As New StreamWriter(CreateFile)

        ''Added firstname to the report
        tmpSQL = "SELECT 27 || substr(contact_number,2,2) || substr(contact_number,5,3) || substr(contact_number,9,4) AS cell_number, " &
                 "INITCAP(first_name) as first_name " &
                 "FROM customer_contact_details ccd " &
                 "inner join customer_personal cc on cc.account_number=ccd.account_number " &
                 "where opted_out = FALSE and " &
                 "number_failed = FALSE " &
                 "and length(contact_number) = 12"

        Try
            Ds = objDBPositive.GetDataSet(tmpSQL)
            If objDBPositive.isR(Ds) Then
                strStreamWriter.WriteLine("CellphoneNumber,FirstName")
                For Each dr As DataRow In Ds.Tables(0).Rows
                    'csv &= dr("email_address") & vbCrLf
                    strStreamWriter.WriteLine(dr("cell_number") & "," & dr("first_name"))
                Next
            End If
        Catch ex As Exception
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
        End Try

        strStreamWriter.Close()
        strStreamWriter.Dispose()

        CreateFile.Close()
        CreateFile.Dispose()

        Return tGuID


    End Function

End Class


