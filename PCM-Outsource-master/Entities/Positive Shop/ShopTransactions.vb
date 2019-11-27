
<Serializable()> _
Public Class CashTransaction

    Public Property Password As String
    Public Property DataBase As String
    Public Property transaction_guid As String
    Public Property sale_date As String
    Public Property sale_time As String
    Public Property branch_code As String
    Public Property user_name As String
    Public Property till_number As String
    Public Property transaction_type As String
    Public Property transaction_number As String
    Public Property sale_total As String
    Public Property cash As String
    Public Property cheque As String
    Public Property credit_card As String
    Public Property voucher As String
    Public Property account As String
    Public Property payment_details As String
    Public Property card_number As String

End Class

<Serializable()> _
Public Class TransactionMaster

    Public Property Password As String
    Public Property DataBase As String
    Public Property transaction_guid As String
    Public Property sale_date As String
    Public Property sale_time As String
    Public Property branch_code As String
    Public Property customer_code As String
    Public Property till_number As String
    Public Property user_name As String
    Public Property rep_code As String
    Public Property number_of_items As String
    Public Property address_line_1 As String
    Public Property address_line_2 As String
    Public Property address_line_3 As String
    Public Property address_line_4 As String
    Public Property address_line_5 As String
    Public Property reference_number As String
    Public Property transaction_type As String
    Public Property transaction_number As String
    Public Property transaction_total As String
    Public Property transaction_total_taxable As String
    Public Property transaction_total_non_taxable As String
    Public Property transaction_total_tax As String
    Public Property transaction_total_discount As String
    Public Property account_number As String
    Public Property price_level As String
    Public Property positive_pc_id As String
    Public Property current_hour As String
    Public Property current_month As String
    Public Property current_weekday As String
    Public Property current_week As String
    
End Class

<Serializable()> _
Public Class TransactionLineItems

    Public Property Password As String
    Public Property DataBase As String

    Public Property Guid As String
    Public Property Link_GuID As String

    Public Property user_name As String
    Public Property shift As String
    Public Property rep_code As String
    Public Property transsaction_type As String
    Public Property transaction_number As String
    Public Property is_service_item As String
    Public Property master_code As String
    Public Property generated_code As String
    Public Property description As String
    Public Property cost_exclusive_estimated As String
    Public Property cost_exclusive_average As String
    Public Property selling_exclusive As String
    Public Property quantity As String
    Public Property tax_group As String
    Public Property tax_percentage As String
    Public Property category_1 As String
    Public Property category_2 As String
    Public Property category_3 As String
    Public Property supplier_account As String
    Public Property supplier_item_code As String
    Public Property discount_value As String
    Public Property discount_reason As String
    Public Property size_grid As String
    Public Property size As String
    Public Property colour_grid As String
    Public Property colour As String
    Public Property updated_cost As String
    Public Property affect_quantities As String
    Public Property branch_code As String

End Class

<Serializable()> _
Public Class IBTTransactionData

    Public Property Password As String
    Public Property DataBase As String

    Public Property Guid As String
    Public Property Link_GuID As String

    Public Property received_qty As String
    Public Property receiving_date As String
    Public Property transaction_number As String
    Public Property receiving_branch_code As String
    Public Property generated_code As String
    Public Property sending_branch_code As String
    Public Property sent_qty As String
    Public Property sending_date As String
    Public Property selling_tax_group As String
    Public Property notes As String


End Class

<Serializable()>
Public Class CashUpData

    Public Property Password As String
    Public Property DataBase As String
    Public Property Guid As String
    Public Property branch_code As String
    Public Property username As String
    Public Property total_cash As String
    Public Property total_cheque As String
    Public Property total_credit_card As String
    Public Property total_voucher As String
    Public Property total_credit_note As String
    Public Property total_system_cash As String
    Public Property total_system_cheque As String
    Public Property total_system_credit_card As String
    Public Property total_system_voucher As String
    Public Property total_system_credit_note As String
    Public Property date_of_cashup As String
    Public Property time_of_cashup As String
    Public Property till_number As String

End Class

<Serializable()>
Public Class StationaryTransactionData

    Public Property Password As String
    Public Property DataBase As String
    Public Property Guid As String
    Public Property BranchCode As String
    Public Property item_code As String
    Public Property received_qty As String

End Class

<Serializable()>
Public Class ReprintTransaction

    Public Property Password As String
    Public Property DataBase As String
    Public Property Guid As String
    Public Property ReprintTimestamp As String
    Public Property BranchCode As String
    Public Property Username As String
    Public Property TransactionType As String
    Public Property TransactionNumber As String

End Class

<Serializable()>
Public Class IBTOut

    Public Property strMessage As String
    Public Property LineItems As List(Of IBTOutLineItems)

End Class

<Serializable()>
Public Class Void

    Public Property guid As String
    Public Property void_date As String
    Public Property void_time As String
    Public Property branch_code As String
    Public Property user_name As String
    Public Property generated_code As String
    Public Property quantity As String


End Class


<Serializable()>
Public Class IBTOutLineItems

    Public Property generated_code As String
    Public Property description As String
    Public Property selling_price As String
    Public Property master_code As String
    Public Property is_service_item As String
    Public Property item_size As String
    Public Property item_colour As String
    Public Property size_matrix As String
    Public Property colour_matrix As String
    Public Property supplier As String
    Public Property suppliers_code As String
    Public Property cost_price As String
    Public Property average_cost_price As String
    Public Property category_1 As String
    Public Property category_2 As String
    Public Property category_3 As String
    Public Property sent_qty As String
    Public Property sending_branch_code As String
    Public Property selling_tax_group As String

End Class

<Serializable()>
Public Class Employee

    Public Property upload_guid As String
    Public Property database As String
    Public Property password As String
    Public Property guid As String
    Public Property employee_number As String
    Public Property is_logged_in As Boolean
    Public Property branch_code As String
    Public Property clocking_time_in As String
    Public Property clocking_date_in As String
    Public Property clocking_hour_in As String
    Public Property clocking_time_out As String
    Public Property clocking_date_out As String
    Public Property clocking_hour_out As String
    Public Property is_sunday As String
    Public Property is_public_holiday As String
    Public Property reason As String
    Public Property user_name As String
    Public Property time_worked As String
    Public Property last_guid As String
    Public Property last_login As String
    Public Property last_logout As String

End Class

<Serializable()>
Public Class CreditCardAuth
    Public Property Password As String
    Public Property transaction_guid As String
    Public Property sale_date As String
    Public Property sale_time As String
    Public Property branch_code As String
    Public Property tran As String
    Public Property transaction_type As String
    Public Property transaction_number As String
    Public Property amount As String
    Public Property response_code As String
    Public Property response_text As String
    Public Property reference As String
    Public Property seq As String
    Public Property rrn As String
    Public Property pos As String
    Public Property store As String
    Public Property card As String
    Public Property card_name As String
    Public Property card_entry As String
    Public Property sign As String
    Public Property cryptogram_type As String
    Public Property cryptogram As String
    Public Property apl As String
    Public Property tvr As String
    Public Property tsi As String
    Public Property cvm_results As String
    Public Property iad As String
    Public Property pin_statement As String
    Public Property data As String
    Public Property receipt As String
    Public Property merchant_id As String
    Public Property aid As String
End Class