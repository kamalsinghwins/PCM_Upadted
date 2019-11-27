Imports Npgsql

Public Class IncomingRageSMSDataLayer
    Inherits DataAccessLayerBase

    Dim objDBWrite As New dlNpgSQL("PostgreConnectionStringPCMWrite")
    Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPCMRead")
    Dim dsInner As DataSet

    Dim connection As Npgsql.NpgsqlConnection = Me.DataBase("PostgreConnectionStringPCMRead")

    Public Sub InsertRageSMS(ByVal ReceivedFromNumber As String, ByVal SentToNumber As String, ByVal Message As String)

        tmpSQL = "INSERT INTO sms_incoming_rage (time_stamp,from_number,message,to_number) VALUES (" & _
                 "now(),'" & Mid$(RG.Apos(ReceivedFromNumber), 1, 13) & "'," & _
                 "'" & Mid$(RG.Apos(Message), 1, 300) & "','" & Mid$(RG.Apos(SentToNumber), 1, 300) & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        Finally
            objDBWrite.CloseConnection()
        End Try


    End Sub

    Public Sub DoLimitIncrease(ByVal PhoneNumber As String)

        '+27823773773
        '123456789012

        Dim account_number As String = ""
        Dim phone_number As String = "0" & Mid$(PhoneNumber, 4, 2) & "-" & Mid$(PhoneNumber, 6, 3) & "-" & Mid$(PhoneNumber, 9, 4)

        tmpSQL = "SELECT debtor_personal.account_number,COUNT(debtor_personal.account_number) OVER (PARTITION BY debtor_personal.cell_number) as zee_count," &
                 "auto_increase,financial_balances.credit_limit,first_name " &
                 "FROM debtor_personal " &
                 "LEFT OUTER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
                 "WHERE cell_number = '" & phone_number & "'"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows

                    'More than 1 account with this telephone number
                    If Val(dr("zee_count") & "") <> 1 Then
                        If (objDBRead IsNot Nothing) Then
                            objDBRead.CloseConnection()
                        End If
                        Exit Sub
                    End If

                    'Call centre has already dealt with the customer (most likely)
                    If dr("auto_increase") & "" = "True" Then
                        If (objDBRead IsNot Nothing) Then
                            objDBRead.CloseConnection()
                        End If
                        Exit Sub
                    End If

                    tmpSQL = "UPDATE debtor_personal SET auto_increase = 'True' WHERE account_number = '" & dr("account_number") & "" & "'"
                    Try
                        objDBWrite.ExecuteQuery(tmpSQL)
                    Catch ex As Exception
                        If (objDBWrite IsNot Nothing) Then
                            objDBWrite.CloseConnection()
                        End If
                        Exit Sub
                    End Try

                    tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " & _
                             "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','WEB - Reply Yes SMS','" & dr("account_number") & "','Auto Increase Updated'," & _
                             "'False','True')"
                    Try
                        objDBWrite.ExecuteQuery(tmpSQL)
                    Catch ex As Exception
                        If (objDBWrite IsNot Nothing) Then
                            objDBWrite.CloseConnection()
                        End If
                        Exit Sub
                    End Try

                    Dim FourteenDaysAgo As String
                    FourteenDaysAgo = Format(Now.Date.AddDays(-14), "yyyy-MM-dd")

                    tmpSQL = "SELECT guid,new_limit,current_balance FROM debtor_limit_increase WHERE account_number = '" & dr("account_number") & "' AND date_of_run >= '" & FourteenDaysAgo & "' " & _
                             "AND additional_notes = 'AUTO INCREASE FALSE' AND limit_increased = 'False'"
                    Try
                        dsInner = objDBRead.GetDataSet(tmpSQL)
                        If objDBRead.isR(dsInner) Then
                            For Each drInner As DataRow In dsInner.Tables(0).Rows
                                If Val(drInner("new_limit") & "") <= Val(dr("credit_limit") & "") Then
                                    'Increase has already been done
                                    If (objDBRead IsNot Nothing) Then
                                        objDBRead.CloseConnection()
                                    End If
                                    Exit Sub
                                End If

                                tmpSQL = "UPDATE financial_balances SET credit_limit = '" & Val(drInner("new_limit") & "") & "' WHERE account_number = '" & dr("account_number") & "'"
                                Try
                                    objDBWrite.ExecuteQuery(tmpSQL)
                                Catch ex As Exception
                                    If (objDBWrite IsNot Nothing) Then
                                        objDBWrite.CloseConnection()
                                    End If
                                    Exit Sub
                                End Try

                                
                                tmpSQL = "INSERT INTO debtor_change_log (change_date,change_time,username,account_number,description,old_value,new_value) " & _
                                         "VALUES ('" & Format(Now, "yyyy-MM-dd") & "','" & Format(Now, "HH:mm:ss") & "','CI','" & dr("account_number") & "','CI Request'," & _
                                         "'" & Val(dr("credit_limit") & "") & "','" & Val(drInner("new_limit") & "") & "')"
                                Try
                                    objDBWrite.ExecuteQuery(tmpSQL)
                                Catch ex As Exception
                                    If (objDBWrite IsNot Nothing) Then
                                        objDBWrite.CloseConnection()
                                    End If
                                    Exit Sub
                                End Try

                                Dim amountcanspend As Double = Val(drInner("new_limit")) - Val(drInner("current_balance"))

                                tmpSQL = "UPDATE debtor_limit_increase SET limit_increased = 'True'," & _
                                         "auto_increase = 'True',additional_notes = 'AUTO INCREASE DONE'," & _
                                         "sms_reply = '" & SendSMS(PhoneNumber, StrConv(dr("first_name"), VbStrConv.ProperCase), True, Val(drInner("new_limit")), amountcanspend) & "' " & _
                                         "WHERE guid = '" & drInner("guid") & "'"
                                Try
                                    objDBWrite.ExecuteQuery(tmpSQL)
                                Catch ex As Exception
                                    If (objDBWrite IsNot Nothing) Then
                                        objDBWrite.CloseConnection()
                                    End If
                                    Exit Sub
                                End Try

                            Next
                        End If
                    Catch ex As Exception
                        If (objDBRead IsNot Nothing) Then
                            objDBRead.CloseConnection()
                        End If
                        Exit Sub
                    End Try



                Next
            End If
        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If

            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If
        Finally
            If (objDBWrite IsNot Nothing) Then
                objDBWrite.CloseConnection()
            End If

            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try

    End Sub

    Public Function SendSMS(ByVal SendSMSToNumber As String, ByVal CustomerName As String, ByVal AutoIncrease As Boolean, _
                            ByVal NewLimit As String, ByVal AmountCanSpend As String) As String

        Dim theMessage As String

        If AutoIncrease = True Then
            'theMessage = "Congrats, your new Rage credit limit is R" & NewLimit & " which means you have R" & AmountCanSpend & " available " & _
            '              "to spend immediately. " & txtSMSAutoIncrease.Text
            theMessage = "Yay! Your Rage credit limit has been increased to R" & NewLimit & " which means you have R" & AmountCanSpend & " available to spend immediately. See you in-store soon!"
        Else
            theMessage = "We dont want u to miss out! Reply YES if you want your Rage credit limit raised to R" & NewLimit & " and you will " & _
                         "have R" & AmountCanSpend & " to spend now! Call 0116086800 for queries"
        End If

        'theMessage = System.Web.HttpUtility.UrlEncode(theMessage)


        Dim URL As String
        'Dim requestWeb As WebRequest
        'Dim responseWeb As WebResponse

        Try
            Dim toNumbers() As String = SendSMSToNumber.Split(",")

            For x As Integer = 0 To toNumbers.Length - 1

                'New SMS service 2013-04-26
                URL = "http://bli.panaceamobile.com/json?action=message_send&username=rage&password=4bnlwk0wjg94fj9r3ftea48btit932m_gvfo9qu78gu5ihjl"

                '4bnlwk0wjg94fj9r3ftea48btit932m_gvfo9qu78gu5ihjl

                URL &= "&to=27" & Trim(toNumbers(x))
                URL &= "&text=" & theMessage
                URL &= "&from=Rage"


                'URL = "http://submitxml.integrat.co.za/higate/get2xhg.php?"
                'URL = URL & "toaddress=" & Trim(toNumbers(x))
                'URL = URL & "&content=" & theMessage
                'URL = URL & "&username=RageVB&password=J87VwV&servicecode=RAGAPI&tag=125&refno=125&value=0"

                'URL = System.Web.HttpUtility.UrlEncode(URL)

                'Comment to send
                Return WRequest(URL, "GET", "")

            Next


        Catch ex As Exception
            Return ex.Message
        End Try

        Return ""

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
                Dim responseStream As IO.StreamReader = _
                  New IO.StreamReader(hwresponse.GetResponseStream())
                responseData = responseStream.ReadToEnd()
            End If
            hwresponse.Close()
        Catch e As Exception
            responseData = "An error occurred: " & e.Message
        End Try
        Return responseData
    End Function

    Public Function GetRageSMSDetails(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            tmpSQL = "SELECT time_stamp,to_number,from_number,message FROM sms_incoming_rage " &
                     "WHERE time_stamp BETWEEN '" & FromDate & " 00:00:00' AND '" & ToDate & " 23:59:59' ORDER BY time_stamp"
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

    Public Function GetManualSaleSMSDetails(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            tmpSQL = "SELECT * FROM sms_incoming_manual_sale " &
                     "WHERE time_stamp BETWEEN '" & FromDate & " 00:00:00' AND '" & ToDate & " 23:59:59' ORDER BY time_stamp"
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

End Class
