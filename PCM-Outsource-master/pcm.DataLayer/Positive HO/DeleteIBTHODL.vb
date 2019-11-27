Imports Entities

Public Class DeleteIBTHODL
    Inherits DataAccessLayerBase
    Dim baseResponse As New BaseResponse

    Public Function DeleteIBT(ByVal SentFrom As String, ByVal SentTo As String,
                             ByVal IBTNumber As String, ByVal Username As String,
                             ByVal Reason As String, ByVal AuthorisedBy As String) As BaseResponse

        Dim xData As New DataTable
        Dim strQuery As String

        Try

            strQuery = "SELECT sending_branch_code," &
                       "generated_code," &
                       "sending_date," &
                       "receiving_branch_code," &
                       "sent_qty," &
                       "SUM(sent_qty) OVER() as sum_sent_qty," &
                       "SUM(received_qty) OVER() as sum_received_qty " &
                       "FROM ibt_transactions " &
                       "WHERE sending_branch_code = '" & RG.Apos(SentFrom) & "' " &
                       "AND receiving_branch_code = '" & RG.Apos(SentTo) & "' " &
                       "AND transaction_number = '" & RG.Apos(IBTNumber) & "'"

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)
        Catch ex As Exception
            Throw ex
            baseResponse.Success = False
            baseResponse.Message = ex.Message
        End Try

        'Check that the IBT exists
        If xData.Rows.Count = 0 Then
            baseResponse.Message = "An IBT with the details you selected / entered does not exists."
            baseResponse.Success = False
            Return baseResponse
        End If

        'Check that the IBT hasn't been received
        If Val(xData.Rows(0)("sum_received_qty") & "") <> 0 Then
            baseResponse.Message = "This IBT has already been accepted. You cannot delete it."
            baseResponse.Success = False
            Return baseResponse

        End If

        'Put the stock back into the branch that sent the stock out
        For i As Integer = 0 To xData.Rows.Count - 1
            tmpSQL = "UPDATE stock_on_hand " &
                     "SET qty_on_hand = qty_on_hand + '" & Val(xData.Rows(i)("sent_qty")) & "'," &
                     "updated = '" & Format(Now, "yyyy-MM-dd") & "' " &
                     "WHERE branch_code = '" & xData.Rows(i)("sending_branch_code") & "' " &
                     "AND generated_code = '" & RG.Apos(xData.Rows(i)("generated_code")) & "'"
            Try
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            Catch ex As Exception
                Throw ex
                baseResponse.Success = False
                baseResponse.Message = ex.Message

            End Try

            tmpSQL = "INSERT INTO deleted_ibts (guid,time_stamp,username," &
                      "sending_branch_code,receiving_branch_code,sent_qty,transaction_number,generated_code,sent_date," &
                      "reason_for_delete,authorised_by) " &
                      "VALUES ('" & Guid.NewGuid.ToString & "','" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "'," &
                      "'" & Username & "','" & xData.Rows(i)("sending_branch_code") & "','" & xData.Rows(i)("receiving_branch_code") & "'," &
                      "'" & Val(xData.Rows(i)("sent_qty")) & "','" & RG.Apos(IBTNumber) & "','" & RG.Apos(xData.Rows(i)("generated_code")) & "'," &
                      "'" & xData.Rows(i)("sending_date") & "','" & Mid$(RG.Apos(Reason), 1, 150) & "','" & Mid$(RG.Apos(AuthorisedBy), 1, 99) & "')"
            Try
                usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
            Catch ex As Exception
                Throw ex
                baseResponse.Success = False
                baseResponse.Message = ex.Message

            End Try
        Next

        'Delete from the IBT table
        tmpSQL = "DELETE FROM ibt_transactions " &
                 "WHERE sending_branch_code = '" & RG.Apos(SentFrom) & "' " &
                 "AND receiving_branch_code = '" & RG.Apos(SentTo) & "' " &
                 "AND transaction_number = '" & RG.Apos(IBTNumber) & "'"
        Try
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
            baseResponse.Success = False
            baseResponse.Message = ex.Message

        End Try

        'Delete from the transaction tables
        tmpSQL = "DELETE " &
                 "FROM transaction_line_items " &
                 "WHERE link_guid IN " &
                 "(SELECT guid " &
                 "FROM transaction_master " &
                 "WHERE branch_code = '" & RG.Apos(SentFrom) & "' " &
                 "AND transaction_type = 'IBTOUT'" &
                 "AND transaction_number = '" & RG.Apos(IBTNumber) & "')"
        Try
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
            baseResponse.Success = False
            baseResponse.Message = ex.Message

        End Try

        tmpSQL = "DELETE " &
                 "FROM transaction_times " &
                 "WHERE guid IN " &
                 "(SELECT guid " &
                 "FROM transaction_master " &
                 "WHERE branch_code = '" & RG.Apos(SentFrom) & "' " &
                 "AND transaction_type = 'IBTOUT'" &
                 "AND transaction_number = '" & RG.Apos(IBTNumber) & "')"
        Try
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
            baseResponse.Success = False
            baseResponse.Message = ex.Message

        End Try

        tmpSQL = "DELETE " &
                 "FROM transaction_master " &
                 "WHERE branch_code = '" & RG.Apos(SentFrom) & "' " &
                 "AND transaction_type = 'IBTOUT'" &
                 "AND transaction_number = '" & RG.Apos(IBTNumber) & "'"
        Try
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
            baseResponse.Success = False
            baseResponse.Message = ex.Message

        Finally

        End Try

        baseResponse.Success = True
        baseResponse.Message = "IBT deleted successfully"
        Return baseResponse

    End Function

    Public Function ManageIBT(ByVal SentFrom As String, ByVal SentTo As String,
                            ByVal IBTNumber As String, ByVal Notes As String,
                            ByVal Username As String) As BaseResponse

        Dim xData As New DataTable
        Dim strQuery As String

        Try

            strQuery = "SELECT sending_branch_code,notes " &
                       "FROM ibt_transactions " &
                       "WHERE sending_branch_code = '" & RG.Apos(SentFrom) & "' " &
                       "AND receiving_branch_code = '" & RG.Apos(SentTo) & "' " &
                       "AND transaction_number = '" & RG.Apos(IBTNumber) & "'"

            xData = usingObjDB.GetDataTable(_POSReadConnectionString, strQuery)
        Catch ex As Exception
            Throw ex
        End Try

        'Check that the IBT exists
        If xData.Rows.Count = 0 Then
            baseResponse.Message = "An IBT with the details you selected / entered does not exists."
            baseResponse.Success = False
            Return baseResponse
        End If


        'Update the notes
        If xData.Rows(0)("notes") & "" = "" Then
            tmpSQL = "UPDATE ibt_transactions SET username = '" & RG.Apos(Username) & "', notes = '" & Format(Now, "yyyy-MM-dd") & " : " & RG.Apos(Notes) & "' " &
                     "WHERE sending_branch_code = '" & RG.Apos(SentFrom) & "' " &
                     "AND receiving_branch_code = '" & RG.Apos(SentTo) & "' " &
                     "AND transaction_number = '" & RG.Apos(IBTNumber) & "'"
        Else
            tmpSQL = "UPDATE ibt_transactions SET username = '" & RG.Apos(Username) & "', notes = notes || ' " & Format(Now, "yyyy-MM-dd") & " : " & RG.Apos(Notes) & "' " &
                     "WHERE sending_branch_code = '" & RG.Apos(SentFrom) & "' " &
                     "AND receiving_branch_code = '" & RG.Apos(SentTo) & "' " &
                     "AND transaction_number = '" & RG.Apos(IBTNumber) & "'"
        End If

        Try
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        End Try

        baseResponse.Success = True
        baseResponse.Message = "IBT updated successfully"
        Return baseResponse

    End Function
End Class
