Imports Npgsql
Imports Entities
Imports System.IO
Imports Entities.LoyaltyDiscount

Public Class LoyaltyDiscountHODL
    Inherits DataAccessLayerBase
    Dim getDiscountResponse As New GetDiscountResponse
    Dim baseResponse As New BaseResponse


    Public Function GetDiscountList() As DataSet

        tmpSQL = "  Select 
                    guid,
                    discount_name,
                    discount_date,
                    discount_percentage,
                    is_enabled
                    from
                    loyalty_discounts
                    order by discount_name desc"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                Return Ds
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SaveDiscount(discountRequest As SaveDiscount
                                       ) As BaseResponse

        If discountRequest.LoyaltyDiscountId <> "" Then
            discountRequest.IsUpdate = True
        Else
            discountRequest.IsInsert = True

        End If


        Dim RecordExists As Boolean

        RecordExists = CheckRecordExists(discountRequest.DiscountName, discountRequest.LoyaltyDiscountId)

        If RecordExists = False And discountRequest.IsInsert = True Then


            Dim discount_id As String = System.Guid.NewGuid.ToString()

            tmpSQL = "Insert into loyalty_discounts
                                (
                                discount_name, 
                                discount_date,
                                discount_percentage, 
                                is_enabled,update_date,
                                guid
                                ) 
                                values
                                (
                                '" & RG.Apos(discountRequest.DiscountName) & "',
                                '" & RG.Apos(discountRequest.DiscountDate) & "',
                                '" & discountRequest.DiscountPercentage & "',
                                '" & discountRequest.IsActive & "',
                                '" & Format(Now, "yyyy-MM-dd") & "',
                                  '" & discount_id & "'
                                )"

            baseResponse.Success = True
            baseResponse.Message = "The new loyalty discount created successfully"


        ElseIf (RecordExists = True And discountRequest.IsUpdate = True) Or (RecordExists = False And discountRequest.IsUpdate = True) Then



            tmpSQL = "update loyalty_discounts set 
                                            discount_date ='" & discountRequest.DiscountDate & "',
                                            discount_percentage='" & discountRequest.DiscountPercentage & "',
                                            is_enabled='" & discountRequest.IsActive & "',
                                            update_date='" & Format(Now, "yyyy-MM-dd") & "',
                                            discount_name='" & discountRequest.DiscountName & "'
                                            where guid='" & discountRequest.LoyaltyDiscountId & "'"


            baseResponse.Success = True
            baseResponse.Message = "The discount updated successfully"

        Else
            baseResponse.Success = False
            baseResponse.Message = "This discount already exists"
            Return baseResponse

        End If



        Try
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)

        Catch ex As Exception
            If ex.Message.ToString.Contains("duplicate key value violates unique constraint ") Then
                baseResponse.Success = False
                baseResponse.Message = "This discount already exists"
                Return baseResponse
            Else
                Throw ex
            End If
        End Try


        Return baseResponse


    End Function

    Public Function GetSelectedDiscountDetails(strDiscountName As String) As GetDiscountResponse
        If strDiscountName <> "" Then

            tmpSQL = "SELECT 
                            guid,
                            discount_name,
                            discount_date,
                            discount_percentage,
                            is_enabled
                            from
                            loyalty_discounts
                            WHERE discount_name = '" & strDiscountName & "'"

            Try
                Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                If usingObjDB.isR(Ds) Then
                    getDiscountResponse.GetSelectedDiscountResponse = Ds.Tables(0)
                    getDiscountResponse.Success = True
                Else
                    getDiscountResponse.Message = "Some issue occured. Please try again."
                    getDiscountResponse.Success = False
                End If
            Catch ex As Exception

                getDiscountResponse.Message = "Some issue occured. Please try again."
                getDiscountResponse.Success = False

            End Try
        Else
            getDiscountResponse.Message = "Discount is missing. Please try again."
            getDiscountResponse.Success = False
        End If

        Return getDiscountResponse
    End Function

    Public Function CheckRecordExists(ByVal discountName As String, Optional ByVal discountId As String = "") As Boolean

        tmpSQL = "SELECT discount_name from loyalty_discounts WHERE discount_name = '" & discountName & "'"

        If discountId <> "" Then
            tmpSQL += " and guid <>'" & discountId & "'"
        End If

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

            baseResponse.Message = "Some issue occured. Please try again."
            baseResponse.Success = False

        End Try


    End Function

    Public Function GetLoyaltyDiscounts(ByVal StartDate As String, ByVal EndDate As String) As DataSet

        tmpSQL = "  Select 
                    guid,
                    discount_name,
                    discount_date,
                    discount_percentage,
                    is_enabled
                    from
                    loyalty_discounts
                    where
                    discount_date between  '" & StartDate & "'   and  '" & EndDate & "'
                    order by discount_name desc"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                Return Ds
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
