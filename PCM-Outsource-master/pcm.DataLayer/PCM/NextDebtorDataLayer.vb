Imports Entities
Imports Npgsql
Imports NpgsqlTypes

Public Class NextDebtorDataLayer

    Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMCollectionsRead")
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Dim _dlDebtors As New DebtorsDataLayer

    Public Function UpdateNextDebtorContactTime(ByVal AccountNumber As Debtor) As Long

        Dim RecordsReturned As Long

        RecordsReturned = _dlDebtors.UpdateNextTimeToCall(AccountNumber.AccountNumber, AccountNumber.NextContactTime)

        Return RecordsReturned

    End Function

    Public Function GetNextInvestigationDebtor() As Debtor

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim NextDebtorAccount As New Debtor

        tmpSQL = "SELECT * FROM (" &
                 "SELECT fb.next_contact_time,fb.account_number " &
                 "FROM financial_balances fb " &
                 "INNER JOIN debtor_personal dp ON fb.account_number = dp.account_number " &
                 "WHERE under_investigation = True " &
                 "ORDER BY total DESC LIMIT 20) s " &
                 "ORDER BY RANDOM() LIMIT 1"
        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    NextDebtorAccount.AccountNumber = dr("account_number") & ""
                    NextDebtorAccount.NextContactTime = dr("next_contact_time") & ""
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

        Return NextDebtorAccount

    End Function

    Public Sub CollectionsLog(ByVal Username As String, ByVal CollectionPeriod As String, ByVal Details As String)

        Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")

        tmpSQL = "INSERT INTO collections_log (details,user_name,collection_period) VALUES " &
                            "('" & Details & "','" & Username & "','" & CollectionPeriod & "')"


        Try
            objDB.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If

        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

    End Sub

    Public Function GetNextContactInvestigationDebtor() As DebtorContactInvestigation

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim NextDebtorAccount As New DebtorContactInvestigation

        tmpSQL = "SELECT fb.next_contact_time,fb.account_number " &
                 "FROM financial_balances fb " &
                 "INNER JOIN debtor_personal dp ON fb.account_number = dp.account_number " &
                 "WHERE fb.next_contact_time < now() AND under_investigation = False " &
                 "AND dp.contact_investigation = True ORDER BY total DESC LIMIT 1"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    NextDebtorAccount.AccountNumber = dr("account_number") & ""
                    NextDebtorAccount.NextContactTime = dr("next_contact_time") & ""
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

        Return NextDebtorAccount

    End Function

    Public Function GetNextPTPDebtor(ByVal Period As String) As Debtor

        If Period = "" Then
            Return Nothing
        End If

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim NextDebtorAccount As New Debtor

        'select * from (select ... order by ... limit 10) s order by random() limit 1;

        'current_contact_level = 5 is PTP
        tmpSQL = "SELECT * FROM (" &
                    "SELECT fb.next_contact_time,fb.account_number " &
                    "FROM financial_balances fb " &
                    "WHERE fb.current_contact_level = 5 "

        Select Case Period
            Case "30"
                tmpSQL &= "AND fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 = 0 AND fb.p60 = 0 AND p30 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p30 DESC LIMIT 500 "
            Case "60"
                tmpSQL &= "AND fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 = 0 AND fb.p60 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p60 DESC LIMIT 500 "
            Case "90"
                tmpSQL &= "AND fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p90 DESC LIMIT 500 "
            Case "120"
                tmpSQL &= "AND fb.p150 = 0 AND fb.p120 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p120 DESC LIMIT 500 "
            Case "150"
                tmpSQL &= "AND fb.p150 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p150 DESC LIMIT 500 "

        End Select

        tmpSQL &= ") s ORDER BY RANDOM() LIMIT 1"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    NextDebtorAccount.AccountNumber = dr("account_number") & ""
                    NextDebtorAccount.NextContactTime = dr("next_contact_time") & ""
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

        Return NextDebtorAccount

    End Function

    Public Function GetNextSnoozedDebtor(ByVal Period As String) As Debtor

        If Period = "" Then
            Return Nothing
        End If

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim NextDebtorAccount As New Debtor

        'current_contact_level 1-3 is "Snoozed"
        tmpSQL = "SELECT * FROM (" &
                 "SELECT fb.next_contact_time,fb.account_number " &
                    "FROM financial_balances fb " &
                    "WHERE fb.current_contact_level BETWEEN 1 AND 4 "

        Select Case Period
            Case "30"
                tmpSQL &= "AND fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 = 0 AND fb.p60 = 0 AND p30 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p30 DESC LIMIT 200 "
            Case "60"
                tmpSQL &= "AND fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 = 0 AND fb.p60 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p60 DESC LIMIT 200 "
            Case "90"
                tmpSQL &= "AND fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p90 DESC LIMIT 200 "
            Case "120"
                tmpSQL &= "AND fb.p150 = 0 AND fb.p120 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p120 DESC LIMIT 200 "
            Case "150"
                tmpSQL &= "AND fb.p150 > " & Val(MinimumCollectionAmount) & " AND fb.next_contact_time < now() ORDER BY fb.p150 DESC LIMIT 200 "

        End Select

        tmpSQL &= ") s ORDER BY RANDOM() LIMIT 1"

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    NextDebtorAccount.AccountNumber = dr("account_number") & ""
                    NextDebtorAccount.NextContactTime = dr("next_contact_time") & ""
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

        Return NextDebtorAccount

    End Function


    Public Function GetNextDebtorByOutstandingAmount(ByVal Period As String) As Debtor

        If Period = "" Then
            Return Nothing
        End If

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim NextDebtorAccount As New Debtor

        '2013-08-13: Should really be checking where contact_investigation = False and not dont_send_sms = False. Changed.

        tmpSQL = "SELECT * FROM (" &
                 "SELECT " &
                        "fb.next_contact_time, " &
                        "fb.account_number " &
                    "FROM " &
                         "financial_balances AS fb "

        Select Case Period
            Case "30"
                tmpSQL &= "WHERE fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 = 0 AND fb.p60 = 0 AND p30 > 10 AND fb.total >= " & Val(MinimumCollectionAmount) & " AND " &
                          "fb.current_contact_level = 0 AND fb.next_contact_time < now() "

            Case "60"
                tmpSQL &= "WHERE fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 = 0 AND fb.p60 > 10 AND fb.total >= " & Val(MinimumCollectionAmount) & " AND " &
                          "fb.current_contact_level = 0 AND fb.next_contact_time < now() "

            Case "90"
                tmpSQL &= "WHERE fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 > 10 AND fb.total >= " & Val(MinimumCollectionAmount) & " AND " &
                          "fb.current_contact_level = 0 AND fb.next_contact_time < now() "

            Case "120"
                tmpSQL &= "WHERE fb.p150 = 0 AND fb.p120 > 10 AND fb.total >= " & Val(MinimumCollectionAmount) & " AND " &
                          "fb.current_contact_level = 0 AND fb.next_contact_time < now() "

            Case "150"
                tmpSQL &= "WHERE fb.p150 > 10 AND fb.total >= " & Val(MinimumCollectionAmount) & " AND " &
                          "fb.current_contact_level = 0 AND fb.next_contact_time < now() "

        End Select

        tmpSQL &= "AND EXISTS ( " &
                     "SELECT " &
                         "1 " &
                     "FROM " &
                         "debtor_personal AS dp " &
                     "WHERE " &
                         "dp.account_number = fb.account_number " &
                     "AND dp.status = 'ACTIVE' " &
                     "AND dp.dont_send_sms = false " &
                     "AND NOT dp.contact_investigation " &
                     "AND NOT dp.is_legal " &
                     "AND NOT dp.under_investigation " &
                   ") "

        Select Case Period
            Case "30"
                tmpSQL &= "ORDER BY " &
                          "fb.p30 DESC " &
                           "LIMIT 100 "
            Case "60"
                tmpSQL &= "ORDER BY " &
                          "fb.p60 DESC " &
                          "LIMIT 100 "
            Case "90"
                tmpSQL &= "ORDER BY " &
                          "fb.p90 DESC " &
                          "LIMIT 100 "
            Case "120"
                tmpSQL &= "ORDER BY " &
                          "fb.p120 DESC " &
                          "LIMIT 100 "
            Case "150"
                tmpSQL &= "ORDER BY " &
                          "fb.p150 DESC " &
                          "LIMIT 100 "

        End Select

        tmpSQL &= ") s ORDER BY RANDOM() LIMIT 1"

        'tmpSQL = "SELECT fb.next_contact_time,fb.account_number " &
        '         "FROM financial_balances fb " &
        '         "INNER JOIN debtor_personal dp ON fb.account_number = dp.account_number "

        'Select Case Period
        '    Case "30"
        '        tmpSQL &= "WHERE fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 = 0 AND fb.p60 = 0 AND p30 > 10 AND total >= " & Val(MinimumCollectionAmount) & " AND " &
        '                  "fb.current_contact_level = 0 AND fb.next_contact_time < now() " &
        '                  "AND dp.status <> 'DEBT REVIEW' " &
        '                  "AND contact_investigation = False AND is_legal = False AND under_investigation = False ORDER BY fb.p30 DESC LIMIT 1"
        '    Case "60"
        '        tmpSQL &= "WHERE fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 = 0 AND fb.p60 > 10 AND total >= " & Val(MinimumCollectionAmount) & " AND " &
        '                  "fb.current_contact_level = 0 AND fb.next_contact_time < now() " &
        '                  "AND dp.status <> 'DEBT REVIEW' " &
        '                  "AND contact_investigation = False AND is_legal = False AND under_investigation = False ORDER BY fb.p60 DESC LIMIT 1"
        '    Case "90"
        '        tmpSQL &= "WHERE fb.p150 = 0 AND fb.p120 = 0 AND fb.p90 > 10 AND total >= " & Val(MinimumCollectionAmount) & " AND " &
        '                  "fb.current_contact_level = 0 AND fb.next_contact_time < now() " &
        '                  "AND dp.status <> 'DEBT REVIEW' " &
        '                  "AND contact_investigation = False AND is_legal = False AND under_investigation = False ORDER BY fb.p90 DESC LIMIT 1"
        '    Case "120"
        '        tmpSQL &= "WHERE fb.p150 = 0 AND fb.p120 > 10 AND total >= " & Val(MinimumCollectionAmount) & " AND " &
        '                  "fb.current_contact_level = 0 AND fb.next_contact_time < now() " &
        '                  "AND dp.status <> 'DEBT REVIEW' " &
        '                  "AND contact_investigation = False AND is_legal = False AND under_investigation = False ORDER BY fb.p120 DESC LIMIT 1"
        '    Case "150"
        '        tmpSQL &= "WHERE fb.p150 > 10 AND total >= " & Val(MinimumCollectionAmount) & " AND " &
        '                  "fb.current_contact_level = 0 AND fb.next_contact_time < now() " &
        '                  "AND dp.status <> 'DEBT REVIEW' " &
        '                  "AND contact_investigation = False AND is_legal = False AND under_investigation = False ORDER BY fb.p150 DESC LIMIT 1"

        'End Select

        Try
            ds = objDB.GetDataSet(tmpSQL)
            If objDB.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    NextDebtorAccount.AccountNumber = dr("account_number") & ""
                    NextDebtorAccount.NextContactTime = dr("next_contact_time") & ""
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

        Return NextDebtorAccount

    End Function

End Class


