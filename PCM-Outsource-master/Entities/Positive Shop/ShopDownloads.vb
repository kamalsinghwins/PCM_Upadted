<Serializable()>
Public Class Stockcodes


    Public Property generated_code As String
    Public Property master_code As String
    Public Property category_1 As String
    Public Property category_2 As String
    Public Property category_3 As String
    Public Property sku_number As String
    Public Property is_service_item As Boolean
    Public Property description As String
    Public Property purchase_tax_group As String
    Public Property sales_tax_group As String
    Public Property supplier As String
    Public Property suppliers_code As String
    Public Property is_not_discountable As Boolean
    Public Property is_blocked As Boolean
    Public Property minimum_stock_level As String
    Public Property item_size As String
    Public Property item_colour As String
    Public Property size_matrix As String
    Public Property colour_matrix As String
    Public Property cost_price As String
    Public Property estimated_cost As String
    Public Property selling_price_1 As String
    Public Property selling_price_2 As String
    Public Property selling_price_3 As String
    Public Property updated As String
    Public Property created As String
    Public Property qty_on_hand As String

End Class

Public Class TillNumbers
    Public Property branch_code As String
    Public Property till_number As String

End Class

Public Class UserPermissions
    Public Property user_name As String
    Public Property user_password As String
    Public Property branch_code As String
    Public Property is_head_office_user As String
    Public Property is_locked_to_branch As String
    Public Property pos_sequence As String
    Public Property maintenance_sequence As String
    Public Property transaction_sequence As String

End Class

Public Class TaxGroup
    Public Property tax_group As String
    Public Property tax_value As String
    Public Property tax_description As String

End Class

Public Class SizeGrid
    Public Property grid_number As String
    Public Property grid_description As String
    Public Property s1 As String
    Public Property s2 As String
    Public Property s3 As String
    Public Property s4 As String
    Public Property s5 As String
    Public Property s6 As String
    Public Property s7 As String
    Public Property s8 As String
    Public Property s9 As String
    Public Property s10 As String
    Public Property s11 As String
    Public Property s12 As String
    Public Property s13 As String
    Public Property s14 As String
    Public Property s15 As String
    Public Property s16 As String
    Public Property s17 As String
    Public Property s18 As String
    Public Property s19 As String
    Public Property s20 As String
    Public Property s21 As String
    Public Property s22 As String
    Public Property s23 As String
    Public Property s24 As String
    Public Property s25 As String
    Public Property s26 As String
    Public Property s27 As String
    Public Property s28 As String
    Public Property s29 As String
    Public Property s30 As String


End Class

Public Class ColourGrid
    Public Property colour_code As String
    Public Property colour_description As String

End Class

Public Class StockOnHand
    Public Property generated_code As String
    Public Property qty_on_hand As String
    Public Property branch_code As String
    Public Property branch_name As String
End Class

Public Class EmployeeDetail
    Public Property employee_number As String
    Public Property first_name As String
    Public Property last_name As String
    Public Property is_active As String

End Class

Public Class CompanyDetails
    Public Property company_code As String
    Public Property company_name As String
    Public Property allow_negative_stock As String
    Public Property currency_symbol As String
    Public Property round_down_to_closest As String
    Public Property maximum_discount_per_row As String
    Public Property default_purchase_tax As String
    Public Property default_sales_tax As String
    Public Property points_to_cash As String
    Public Property cash_to_points As String
    Public Property image_server As String

End Class


Public Class Special
    Public Property SpecialID() As String
    Public Property SpecialName() As String
    Public Property StartDate() As DateTime
    Public Property EndDate() As DateTime
    Public Property is_Active() As Boolean
    Public Property Price() As String
    Public Property UpdateDate() As DateTime
    Public Property ListOfItems As List(Of SpecialLineItems)

End Class

Public Class SpecialLineItems
    Public Property Description() As String
    Public Property specials_link_id() As String
    Public Property Mastercode() As String
    Public Property Qty() As String

End Class

Public Class LoyaltyDiscounts
    Public Property discount_name As String
    Public Property discount_date As String
    Public Property discount_percentage As String
    Public Property is_enabled As String

End Class

Public Class BranchStock
    Public Property branch_code As String
    Public Property qty_on_hand As String
    Public Property branch_name_web As String
    Public Property telephone_number As String
    Public Property longitude As String
    Public Property latitude As String
    Public Property url As String
    Public Property address_line_1 As String
    Public Property address_line_2 As String
    Public Property address_line_3 As String
    Public Property address_line_4 As String
    Public Property address_line_5 As String
End Class


Public Class StockResponse
    Inherits BaseResponse

    Public Property Stocks As List(Of BranchStock)

End Class