Imports Npgsql
Imports Microsoft.VisualBasic
Imports System.Data

Public Class BuyingDL
    Inherits DataAccessLayerBase

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim objDBPCMWrite As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing
    Dim connectionPCM As Npgsql.NpgsqlConnection = Nothing

    'Public Sub New(ByVal CompanyCode As String)
    '    objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
    '    objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
    '    connection = Me.DataBase("PostgreConnectionStringPositiveRead")

    '    objDBPCMWrite = New dlNpgSQL("PostgreConnectionStringPCMWrite")
    '    connectionPCM = Me.DataBase("PostgreConnectionStringPCMRead")

    'End Sub

    Public Sub New()
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = Me.DataBase("PostgreConnectionStringPCMRead")

        objDBPCMWrite = New dlNpgSQL("PostgreConnectionStringPCMWrite")
        connectionPCM = Me.DataBase("PostgreConnectionStringPCMRead")

    End Sub

    Public Function ResetPassword(ByVal UserID As String, ByVal NewPassword As String) As String

        tmpSQL = "UPDATE buying_customer_details SET password = '" & RG.Apos(NewPassword) & "' WHERE user_id = '" & UserID & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function GetRankers() As DataTable
        Dim xData As New DataTable

        tmpSQL = "SELECT buying_customer_details.user_id, buying_customer_details.first_name, buying_customer_details.last_name, buying_customer_details.id_number," & _
                "buying_customer_details.state, buying_customer_details.is_female, buying_customer_details.email_address, buying_customer_details.tier, buying_customer_analysis.user_ranking," & _
                "buying_customer_analysis.user_ranking_clothing, buying_customer_analysis.shoes_count, buying_customer_analysis.clothing_count " & _
                "FROM buying_customer_details " & _
                "INNER JOIN buying_customer_analysis ON buying_customer_details.user_id = buying_customer_analysis.user_id " & _
                "WHERE shoes_count > 0 " & _
                "ORDER BY buying_customer_details.user_id"
        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xData)
        Catch ex As Exception
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetRankersRankings() As DataTable
        Dim xData As New DataTable

        tmpSQL = "SELECT buying_stockcodes_rating.item_code, " & _
                "buying_stockcodes_rating.rating," & _
                "buying_stockcodes_rating.price_range," & _
                "buying_stockcodes_rating.user_id, " & _
                "buying_stockcodes_rating.timestamp_of_rating," & _
                "buying_stockcodes_rating.comments," & _
                "buying_stockcodes_rating.time_spent_ranking," & _
                "buying_stockcodes_analysis.rating AS our_rating " & _
                "FROM buying_stockcodes_rating " & _
                "INNER JOIN buying_stockcodes_analysis ON buying_stockcodes_rating.item_code = buying_stockcodes_analysis.item_code " & _
                "WHERE user_id IN " & _
                "(SELECT user_id FROM buying_customer_analysis WHERE shoes_count > 0) " & _
                "ORDER BY item_code"

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xData)
        Catch ex As Exception
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function
    Private Function CardToEncrypt(ByVal cString As String) As String
        Dim strConvert(16) As String
        Dim strSwap(16) As String


        'CONVERT THEM TO ENCRYPTED NUMBERS
        For oLoop = 1 To 16
            Select Case Mid$(cString, oLoop, 1)
                Case "1" : strConvert(oLoop) = "4"
                Case "2" : strConvert(oLoop) = "9"
                Case "3" : strConvert(oLoop) = "0"
                Case "4" : strConvert(oLoop) = "3"
                Case "5" : strConvert(oLoop) = "6"
                Case "6" : strConvert(oLoop) = "2"
                Case "7" : strConvert(oLoop) = "5"
                Case "8" : strConvert(oLoop) = "1"
                Case "9" : strConvert(oLoop) = "7"
                Case "0" : strConvert(oLoop) = "8"
            End Select
        Next oLoop

        'WE SWAP THE ENCRYPTED NUMBERS AROUND (POSITION SWAP)
        'eg position 4 becomes 1 (first line)
        strSwap(1) = strConvert(4)
        strSwap(2) = strConvert(6)
        strSwap(3) = strConvert(10)
        strSwap(4) = strConvert(5)
        strSwap(5) = strConvert(8)
        strSwap(6) = strConvert(1)
        strSwap(7) = strConvert(11)
        strSwap(8) = strConvert(15)
        strSwap(9) = strConvert(3)
        strSwap(10) = strConvert(16)
        strSwap(11) = strConvert(14)
        strSwap(12) = strConvert(7)
        strSwap(13) = strConvert(2)
        strSwap(14) = strConvert(13)
        strSwap(15) = strConvert(9)
        strSwap(16) = strConvert(12)

        CardToEncrypt = ""

        For oLoop = 1 To 16
            CardToEncrypt = CardToEncrypt & strSwap(oLoop)
        Next oLoop

    End Function

    Public Function GetVouchers(ByVal UserID As String)
        Dim xData As New DataTable

        tmpSQL = "SELECT row_number() OVER (ORDER BY card_gift_cards.card_number) AS id, " & _
                 "card_gift_cards.card_number AS voucher_number,card_gift_cards.balance AS balance " & _
                 "FROM buying_customer_vouchers " & _
                 "INNER JOIN card_gift_cards ON buying_customer_vouchers.voucher_id = card_gift_cards.card_number " & _
                 "WHERE buying_customer_vouchers.user_id = '" & UserID & "'"

        Try

            Dim command As New NpgsqlCommand()
            connectionPCM.Open()
            command.Connection = connectionPCM
            command.CommandType = CommandType.Text

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connectionPCM)
            reader.Fill(xData)

        Catch ex As Exception
            Return Nothing
        Finally
            If (connectionPCM IsNot Nothing) Then
                connectionPCM.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function RedeemVoucher(ByVal UserID As String, ByVal Tier As Integer) As String

        Dim xData As New DataTable
        Dim zData As New DataTable

        tmpSQL = "SELECT points " & _
                 "FROM buying_customer_points " & _
                 "WHERE user_id = '" & RG.Apos(UserID) & "'"

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(zData)

            If Val(zData.Rows(0)("points")) < 20 Then
                Return "NotEnoughPoints"

                If (connection IsNot Nothing) Then
                    connection.Close()
                End If
            End If

        Catch ex As Exception
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

      
        tmpSQL = "UPDATE buying_customer_points SET " & _
                 "points = points - " & Val(zData.Rows(0)("points")) & " WHERE user_id = '" & RG.Apos(UserID) & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            objDBWrite.CloseConnection()
        End Try

        Dim gCardNum As String

        Dim Generator As System.Random = New System.Random()

TryAgain:

        gCardNum = "6502" & Generator.Next(2000, 9999) & Generator.Next(3000, 9999) & Generator.Next(6000, 9999)

        tmpSQL = "SELECT card_number FROM card_details WHERE card_number = '" & gCardNum & "'"
        Try

            Dim command As New NpgsqlCommand()
            connectionPCM.Open()
            command.Connection = connectionPCM
            command.CommandType = CommandType.Text

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connectionPCM)
            reader.Fill(xData)

            If xData.Rows.Count > 0 Then
                GoTo TryAgain
            End If

            tmpSQL = "INSERT INTO card_details (card_number,encrypted_card_number,created_by,type_of_card,current_status) VALUES " & _
                     "('" & gCardNum & "','" & CardToEncrypt(gCardNum) & "','BUY','GIFT','ACTIVE')"
            Try
                objDBPCMWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                objDBPCMWrite.CloseConnection()
                Return ex.Message
            Finally
                objDBPCMWrite.CloseConnection()
            End Try

            tmpSQL = "INSERT INTO card_dates (card_number,date_created) VALUES " & _
                      "('" & gCardNum & "','" & Format(Now, "yyyy-MM-dd") & "')"
            Try
                objDBPCMWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                objDBPCMWrite.CloseConnection()
                Return ex.Message
            Finally
                objDBPCMWrite.CloseConnection()
            End Try

            'Tier 1 gets 50c per point, Tier 2 gets R1
            If Tier = 1 Then
                tmpSQL = "INSERT INTO card_gift_cards (card_number,balance) VALUES " & _
                    "('" & gCardNum & "','" & RG.Numb(Val(zData.Rows(0)("points")) / 2) & "')"
            ElseIf Tier = 2 Then
                tmpSQL = "INSERT INTO card_gift_cards (card_number,balance) VALUES " & _
                    "('" & gCardNum & "','" & RG.Numb(Val(zData.Rows(0)("points"))) & "')"
            End If

            Try
                objDBPCMWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                objDBPCMWrite.CloseConnection()
                Return ex.Message
            Finally
                objDBPCMWrite.CloseConnection()
            End Try

            tmpSQL = "INSERT INTO buying_customer_vouchers (user_id,voucher_id) VALUES ('" & UserID & "','" & gCardNum & "')"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                objDBWrite.CloseConnection()
                Return ex.Message
            Finally
                objDBWrite.CloseConnection()
            End Try

            tmpSQL = "INSERT INTO buying_customer_vouchers (user_id,voucher_id) VALUES ('" & UserID & "','" & gCardNum & "')"
            Try
                objDBPCMWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                objDBPCMWrite.CloseConnection()
                Return ex.Message
            Finally
                objDBPCMWrite.CloseConnection()
            End Try

        Catch ex As Exception
            Return Nothing
        Finally
            If (connectionPCM IsNot Nothing) Then
                connectionPCM.Close()
            End If

        End Try

        Return "Success"

    End Function

    Public Function CheckResetPassword(ByVal EMailAddress As String, ByVal GuID As String) As DataTable

        Dim xData As New DataTable

        If EMailAddress <> "" Then
            tmpSQL = "SELECT bcd.user_id,bcda.is_first_login " & _
                     "FROM buying_customer_details bcd " & _
                     "LEFT OUTER JOIN buying_customer_dates bcda ON bcd.user_id = bcda.user_id " & _
                     "WHERE lower(email_address) = '" & RG.Apos(EMailAddress.ToString.ToLower) & "'"
        ElseIf GuID <> "" Then
            tmpSQL = "SELECT bcd.user_id,bcda.is_first_login " & _
                      "FROM buying_customer_details bcd " & _
                      "LEFT OUTER JOIN buying_customer_dates bcda ON bcd.user_id = bcda.user_id " & _
                      "WHERE temp_unlock = '" & RG.Apos(GuID) & "'"
        End If

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xData)

            'Can only reset with email address on first login
            If EMailAddress <> "" Then
                If xData.Rows(0)("is_first_login") = True Then
                    xData.Clear()
                End If
            End If
        Catch ex As Exception
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function CheckViewClothing(ByVal UserID As String) As String

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT view_clothing " & _
                     "FROM buying_customer_details bcd " & _
                     "WHERE user_id = " & UserID & ""


            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xData)

        Catch ex As Exception
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        'Credentials are incorrect
        If xData.Rows.Count = 0 Then
            Return Nothing
        End If

        Return xData.Rows(0)("view_clothing") & ""
        
    End Function


    Public Function ProcessLogin(ByVal EMailAddress As String, ByVal Password As String, ByVal IPAddress As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT bcd.user_id,bcd.first_name,bcda.is_first_login,active,tier,view_clothing " & _
                     "FROM buying_customer_details bcd " & _
                     "LEFT OUTER JOIN buying_customer_dates bcda ON bcd.user_id = bcda.user_id " & _
                     "WHERE lower(email_address) = '" & RG.Apos(EMailAddress.ToString.ToLower) & "' AND password = '" & RG.Apos(Password) & "'"


            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xData)

        Catch ex As Exception
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        'Credentials are incorrect
        If xData.Rows.Count = 0 Then
            Return Nothing
        End If

        'Account was disabled
        If xData.Rows(0)("active") = "False" Then
            Return Nothing
        End If

        'Check for first login
        If xData.Rows(0)("is_first_login") = False Then
            tmpSQL = "UPDATE buying_customer_dates SET is_first_login = True,first_login = '" & Format(Now, "yyyy-MM-dd") & "' " & _
                     "WHERE user_id = '" & xData.Rows(xData.Rows.Count - 1)("user_id") & "'"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                objDBWrite.CloseConnection()
                Return Nothing
            Finally
                objDBWrite.CloseConnection()
            End Try

            tmpSQL = "UPDATE buying_customer_details SET temp_unlock = '" & Guid.NewGuid.ToString & "' WHERE user_id = '" & xData.Rows(xData.Rows.Count - 1)("user_id") & "'"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                objDBWrite.CloseConnection()
                Return Nothing
            Finally
                objDBWrite.CloseConnection()
            End Try
        End If

        'Insert login
        tmpSQL = "INSERT INTO buying_customer_logins (user_id,login_ts,ip_address) VALUES ('" & xData.Rows(xData.Rows.Count - 1)("user_id") & "'," & _
                 "now(),'" & IPAddress & "') "
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return Nothing
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return xData

    End Function

    Public Function GetStockcodeDetail(ByVal Stockcode As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT bsm.material,bsm.qty_ordered,bsm.description,bsm.price,bsm.category,bsi.image_location,bsi.display_order " & _
                     "FROM buying_stockcodes_master bsm " & _
                     "LEFT OUTER JOIN buying_stockcodes_images bsi ON bsm.item_code = bsi.item_code " & _
                     "WHERE upper(bsm.item_code) = '" & RG.Apos(Stockcode).ToString.ToUpper & "'"

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

    Public Function GetStockcodes(ByVal SearchString As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT DISTINCT ON (master_code,item_colour) master_code||item_colour AS stockcode FROM stockcodes_master " & _
                     "WHERE upper(master_code) LIKE '" & RG.Apos(SearchString).ToString.ToUpper & "%' LIMIT 50"

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

    Public Function UpdateProduct(ByVal Stockcode As String, ByVal Description As String, ByVal Material As String, _
                                  ByVal QTYOrdered As String, ByVal ImageLocation As String, ByVal Price As String,
                                  ByVal DisplayOrder As String, ByVal Category As String) As String

        tmpSQL = "UPDATE buying_stockcodes_master SET material = '" & RG.Apos(Material.ToUpper) & "',description = '" & RG.Apos(Description.ToUpper) & "'," & _
                 "qty_ordered = '" & RG.Apos(Val(QTYOrdered)) & "',price = '" & Val(Price) & "',category = '" & Category & "' WHERE upper(item_code) = '" & RG.Apos(Stockcode.ToUpper) & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            objDBWrite.CloseConnection()
        End Try

        tmpSQL = "INSERT INTO buying_stockcodes_images (item_code,image_location,display_order) VALUES ('" & RG.Apos(Stockcode.ToUpper) & "', " & _
                 "'" & RG.Apos(ImageLocation) & "','" & Val(DisplayOrder) & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function InsertProduct(ByVal Stockcode As String, ByVal Description As String, ByVal Material As String, _
                                  ByVal QTYOrdered As String, ByVal ImageLocation As String, ByVal Price As String, ByVal DisplayOrder As String,
                                  ByVal Category As String) As String

        tmpSQL = "INSERT INTO buying_stockcodes_master (item_code,material,description,qty_ordered,price,date_inserted,category) VALUES " & _
                 "('" & RG.Apos(Stockcode.ToUpper) & "','" & RG.Apos(Material.ToUpper) & "','" & RG.Apos(Description.ToUpper) & "'," & _
                 "'" & RG.Apos(Val(QTYOrdered)) & "','" & Val(Price) & "','" & Format(Now, "yyyy-MM-dd") & "','" & Category & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            objDBWrite.CloseConnection()
        End Try

        tmpSQL = "INSERT INTO buying_stockcodes_images (item_code,image_location,display_order) VALUES ('" & RG.Apos(Stockcode.ToUpper) & "', " & _
                 "'" & RG.Apos(ImageLocation) & "','" & Val(DisplayOrder) & "')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function GetGUIDForEmail(ByVal EMailAddress As String) As String

        Dim xdata As New DataTable
        
        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT temp_unlock FROM buying_customer_details WHERE email_address = '" & RG.Apos(EMailAddress) & "'"

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xData)

        Catch ex As Exception
            If (connection IsNot Nothing) Then
                connection.Close()
            End If
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        'Credentials are incorrect
        If xData.Rows.Count = 0 Then
            Return ""
        Else
            Return xdata.Rows(0)("temp_unlock")
        End If

    End Function

    Public Function GetItemCodeRankings(ByVal StartDate As String, ByVal EndDate As String, ByVal isClothing As Boolean) As DataTable

        Dim xdata As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT bsm.item_code, MAX(bsm.description) as description, MAX(bsm.price) AS price, MAX(qty_ordered) AS qty_ordered, " & _
                     "MAX(ba.rating) as our_rating, MAX(ba.rankers_rating) * 100 AS normalized, " & _
                     "MAX(category_1) AS cat1, MAX(category_2) AS cat2, MAX(category_3) AS cat3, " & _
                     "(SELECT COUNT(item_code) FROM buying_stockcodes_rating WHERE price_range = '1' AND item_code = bsm.item_code) AS ones," & _
                     "(SELECT COUNT(item_code) FROM buying_stockcodes_rating WHERE price_range = '2' AND item_code = bsm.item_code) AS twos " & _
                     "FROM buying_stockcodes_master bsm " & _
                     "LEFT OUTER JOIN buying_stockcodes_analysis ba ON bsm.item_code = ba.item_code " & _
                     "LEFT OUTER JOIN stockcodes_categories sc ON master_code = left(bsm.item_code,length(bsm.item_code) -2) " & _
                     "LEFT OUTER JOIN buying_stockcodes_rating sr ON sr.item_code = bsm.item_code " & _
                     "WHERE sr.timestamp_of_rating BETWEEN '" & StartDate & "' AND '" & EndDate & "' "

            If isClothing = True Then
                tmpSQL &= "AND category = 'Clothing' "
            Else
                tmpSQL &= "AND category = 'Shoes' "
            End If

            tmpSQL &= "GROUP BY bsm.item_code ORDER BY bsm.item_code"

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xdata)

        Catch ex As Exception
            If (connection IsNot Nothing) Then
                connection.Close()
            End If
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xdata

    End Function

    Public Function GetItemCodeRankingsTransactions(ByVal StartDate As String, ByVal EndDate As String, ByVal isClothing As Boolean) As DataTable

        Dim xdata As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT bsm.item_code, bsr.rating AS rating, bsr.price_range AS reason, bsr.timestamp_of_rating AS timestamp, " & _
                     "bsr.time_spent_ranking AS seconds, bsr.comments AS comments " & _
                     "FROM buying_stockcodes_master bsm " & _
                     "INNER JOIN buying_stockcodes_rating bsr ON bsm.item_code = bsr.item_code " & _
                     "WHERE bsr.timestamp_of_rating BETWEEN '" & StartDate & "' AND '" & EndDate & "'"

            If isClothing = True Then
                tmpSQL &= "AND category = 'Clothing'"
            Else
                tmpSQL &= "AND category = 'Shoes'"
            End If

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xdata)

        Catch ex As Exception
            If (connection IsNot Nothing) Then
                connection.Close()
            End If
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        Return xdata

    End Function

    Public Function InsertStockCodeRating(ByVal Stockcode As String, ByVal Rating As Integer,
                                          ByVal PriceRange As String, ByVal UserId As String,
                                          ByVal LengthOfAction As String, ByVal Comments As String) As String

        Dim xdata As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT item_code FROM buying_stockcodes_rating WHERE user_id = " & UserId & " AND item_code = '" & RG.Apos(Stockcode) & "'"

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connection)
            reader.Fill(xdata)

        Catch ex As Exception
            If (connection IsNot Nothing) Then
                connection.Close()
            End If
            Return Nothing
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        'Has not yet been inserted
        If xdata.Rows.Count = 0 Then
            tmpSQL = "INSERT INTO buying_stockcodes_rating (item_code,rating,price_range,user_id,timestamp_of_rating,time_spent_ranking,comments) VALUES " & _
                "('" & RG.Apos(Stockcode.ToUpper) & "','" & Rating & "','" & RG.Apos(PriceRange) & "','" & UserId & "',now()," & _
                "'" & RG.Numb(Val(LengthOfAction)) & "','" & RG.Apos(Mid(Comments, 1, 1000)) & "')"
            Try
                objDBWrite.ExecuteQuery(tmpSQL)
            Catch ex As Exception
                objDBWrite.CloseConnection()
                Return ex.Message
            Finally
                objDBWrite.CloseConnection()
            End Try
        End If

        Return "Success"

    End Function

    Public Sub UpdateUserForClothing(ByVal UserID As String, ByVal ViewClothing As Boolean)

        tmpSQL = "UPDATE buying_customer_details SET view_clothing = " & ViewClothing & " WHERE user_id = " & UserID & ""
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
        Finally
            objDBWrite.CloseConnection()
        End Try


    End Sub

    Public Function UpdateUserPoints(ByVal Points As Integer, ByVal UserId As String) As String

        tmpSQL = "UPDATE buying_customer_points SET number_of_styles_rated = number_of_styles_rated + " & Points & "," & _
                 "points = points+ " & Points & " WHERE user_id = '" & RG.Apos(UserId) & "'"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function InsertApplication(ByVal FirstName As String, ByVal LastName As String, ByVal IDNumber As String, ByVal EMailAddress As String,
                                      ByVal ContactNumber As String, ByVal Province As String, ByVal Kids As String) As String

        tmpSQL = "INSERT INTO buying_customer_applications (first_name,last_name,id_number,email,contact_number,province,kids,date,approved) VALUES " & _
                 "('" & RG.Apos(FirstName) & "','" & RG.Apos(LastName) & "','" & RG.Apos(IDNumber) & "','" & RG.Apos(EMailAddress) & "'," & _
                 "'" & RG.Apos(ContactNumber) & "','" & RG.Apos(Province) & "','" & RG.Apos(Kids) & "',now(),'False')"
        Try
            objDBWrite.ExecuteQuery(tmpSQL)
        Catch ex As Exception
            objDBWrite.CloseConnection()
            Return ex.Message
        Finally
            objDBWrite.CloseConnection()
        End Try

        Return "Success"

    End Function

    Public Function GetAllBuyingStockCodes(ByVal UserID As String, ByVal ShowClothing As Boolean) As DataTable
        Dim xData As New DataTable
        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            If ShowClothing = True Then
                tmpSQL = "SELECT item_code from buying_stockcodes_master " & _
                     "WHERE item_code NOT IN " & _
                     "(SELECT item_code FROM buying_stockcodes_rating WHERE user_id = " & UserID & ") " & _
                     "ORDER BY RANDOM()"
            Else
                tmpSQL = "SELECT item_code from buying_stockcodes_master " & _
                     "WHERE item_code NOT IN " & _
                     "(SELECT item_code FROM buying_stockcodes_rating WHERE user_id = " & UserID & ") " & _
                     "AND category = 'Shoes' " & _
                     "ORDER BY RANDOM()"
            End If
            

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

    Public Function GetAllStockCodeImages(ByVal Stockcode As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT image_location " & _
                     "FROM buying_stockcodes_images " & _
                     "WHERE upper(item_code) = '" & RG.Apos(Stockcode).ToString.ToUpper & "' ORDER by display_order NULLS FIRST"

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

    Public Function GetUserDataValues(ByVal UserId As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT number_of_styles_rated,points " & _
                     "FROM buying_customer_points " & _
                     "WHERE user_id = '" & RG.Apos(UserId) & "'"

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

