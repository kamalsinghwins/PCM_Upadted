Imports pcm.DataLayer
Imports Entities

Public Class NextDebtorBusinessLayer

    Private _dlDebtors As New DebtorsDataLayer
    Private _dlNextDebtor As New NextDebtorDataLayer

    Private Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        ' by making Generator static, we preserve the same instance '
        ' (i.e., do not create new instances with the same seed over and over) '
        ' between calls '
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Public Function ReturnNextDebtor(ByVal Period As String, ByVal Username As String) As Debtor

        If Period = "" Then
            Return Nothing
        End If

        Dim tmpAccount As New Debtor

        '2017-12-20
        'We're having issues with the queries to fetch a new debtor using 100% of the read server CPU
        'As there are thousands (perhaps tens of) call being made a day, all levels should be contacted
        'even when splitting the calls
        'Current calls in queue:
        '30 days - ptp:1 174 snoozed:12 596 byamount:207 423
        '60 days - ptp:278 snoozed:3 byamount:21 482
        '90 days - ptp:6 886 snoozed:7 277 byamount:61
        '120 days - ptp:1 321 snoozed:2772 byamount:28
        '150 days - ptp:25 461 snoozed:55 097 byamount:249 445
        'Seems the one to concentrate on getting through is the byamount,
        'however the PTPs should be the easiest to collect?

        'The 30 and 150 periods are the ones that should be split

        Dim _HasLooped As Boolean

        Dim tmpDecider As Integer = GetRandom(1, 10)

        _dlNextDebtor.CollectionsLog(Username, Period, "Selected from random: " & tmpDecider)


        '===================================================================================
        '2018-10-05
        'WE NOW HAVE ENOUGH AGENTS TO COLLECT ON MOST LIKELY TO PAY - REMOVING THE DECIDER
        '2019-05-29
        'TOO MANY ACCOUNTS, PUTTING DECIDER BACK IN
        '===================================================================================
        If Period = 30 Then
            If tmpDecider > 8 Then
                '9, 10
                GoTo GetNewRecordSnoozed
                'Snoozed -> ByAmount
            ElseIf tmpDecider < 3 Then
                '1,2
                GoTo GetNewRecordOutstanding
            End If
            '3,4,5,6,7,8
            'rest will just fall through, hitting PTP first
            'and then going Snoozed -> ByAmount
        End If

        If Period = 150 Then
            If tmpDecider > 7 Then
                '8,9,10
                GoTo GetNewRecordSnoozed
                'Snoozed -> ByAmount
            ElseIf tmpDecider < 6 Then
                '1,2,3,4,5
                GoTo GetNewRecordOutstanding
            End If
            'rest will just fall through, hitting PTP first
            'and then going Snoozed -> ByAmount
        End If

        Dim NoPTPRecordsLeft As Boolean = False
        Dim NoSnoozedRecordsLeft As Boolean = False
        '===================================================================================

        'First call PTP who are over 
GetNewRecord:
        tmpAccount = _dlNextDebtor.GetNextPTPDebtor(Period)
        If Not IsNothing(tmpAccount) Then
            '_dlNextDebtor.CollectionsLog(Username, Period, "PTP Got account to try: " & tmpAccount.AccountNumber)
            If _dlNextDebtor.UpdateNextDebtorContactTime(tmpAccount) = 0 Then
                '_dlNextDebtor.CollectionsLog(Username, Period, "PTP Account already taken: " & tmpAccount.AccountNumber)
                'The selected record has already been updated by another user
                GoTo GetNewRecord
            End If
            '_dlNextDebtor.CollectionsLog(Username, Period, "PTP Got account successfully: " & tmpAccount.AccountNumber)
            Return tmpAccount
        Else
            NoPTPRecordsLeft = True
        End If


        '2nd, call "snoozed" customers
GetNewRecordSnoozed:
        tmpAccount = _dlNextDebtor.GetNextSnoozedDebtor(Period)
        If Not IsNothing(tmpAccount) Then
            '_dlNextDebtor.CollectionsLog(Username, Period, "Snoozed account to try: " & tmpAccount.AccountNumber)
            If _dlNextDebtor.UpdateNextDebtorContactTime(tmpAccount) = 0 Then
                '_dlNextDebtor.CollectionsLog(Username, Period, "Snoozed Account already taken: " & tmpAccount.AccountNumber)
                'The selected record has already been updated by another user
                GoTo GetNewRecordSnoozed
            End If
            '_dlNextDebtor.CollectionsLog(Username, Period, "Snoozed Got account successfully: " & tmpAccount.AccountNumber)
            Return tmpAccount
        Else
            NoSnoozedRecordsLeft = True
            '_dlNextDebtor.CollectionsLog(Username, Period, "No Snoozed accounts left")
        End If


        '3rd, call customers in order of most outstanding
GetNewRecordOutstanding:
        tmpAccount = _dlNextDebtor.GetNextDebtorByOutstandingAmount(Period)
        If Not IsNothing(tmpAccount) Then
            '_dlNextDebtor.CollectionsLog(Username, Period, "Outstanding account to try: " & tmpAccount.AccountNumber)
            If _dlNextDebtor.UpdateNextDebtorContactTime(tmpAccount) = 0 Then
                '_dlNextDebtor.CollectionsLog(Username, Period, "Snoozed Account already taken: " & tmpAccount.AccountNumber)
                'The selected record has already been updated by another user
                GoTo GetNewRecord
            End If
            '_dlNextDebtor.CollectionsLog(Username, Period, "Outstanding Got account successfully: " & tmpAccount.AccountNumber)
            Return tmpAccount
        Else
            'Added: 2018-10-02
            'Call centre was getting no results when all outstanding were called
            'Now start again from PTP
            '_dlNextDebtor.CollectionsLog(Username, Period, "No Outstanding left. Going to PTP")

            'If PTP and Snoozed are also empty there are no records for this period
            If NoPTPRecordsLeft = True Then
                If NoSnoozedRecordsLeft = True Then
                    Dim NextDebtorAccount As New Debtor
                    NextDebtorAccount.AccountNumber = ""
                    Return NextDebtorAccount
                End If
            End If

            GoTo GetNewRecord
        End If

        Return Nothing

    End Function

    Public Function ReturnNextContactInvestigationDebtor() As DebtorContactInvestigation

        Dim tmpAccount As New DebtorContactInvestigation

GetNewRecord:
        tmpAccount = _dlNextDebtor.GetNextContactInvestigationDebtor
        If Not IsNothing(tmpAccount) Then
            If _dlDebtors.UpdateNextTimeToCall(tmpAccount.AccountNumber, tmpAccount.NextContactTime) = 0 Then
                'The selected record has already been updated by another user
                GoTo GetNewRecord
            End If
            Return tmpAccount
        Else
            'All records have been dealt with
            tmpAccount.AccountNumber = ""
            Return tmpAccount
        End If

        Return Nothing

    End Function

    Public Function ReturnNextInvestigationDebtor() As Debtor

        Dim tmpAccount As New Debtor

GetNewRecord:
        tmpAccount = _dlNextDebtor.GetNextInvestigationDebtor
        If Not IsNothing(tmpAccount) Then
            If _dlDebtors.UpdateNextTimeToCall(tmpAccount.AccountNumber, tmpAccount.NextContactTime) = 0 Then
                'The selected record has already been updated by another user
                GoTo GetNewRecord
            End If
            Return tmpAccount
        Else
            'All records have been dealt with
            'All records have been dealt with
            tmpAccount.AccountNumber = ""
            Return tmpAccount
        End If

        Return Nothing

    End Function

End Class
