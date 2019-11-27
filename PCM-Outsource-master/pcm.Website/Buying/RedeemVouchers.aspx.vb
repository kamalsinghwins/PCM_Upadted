Imports System.Data
Imports pcm.BusinessLayer
Imports DevExpress.Web

Public Class RedeemVouchers
    Inherits System.Web.UI.Page

    Dim _BLayer As BuyingBL = New BuyingBL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("user_id") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Buying/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Buying/Default.aspx")
                End If
            End If
        Else
            If Session("user_id") = "" Then
                Session("user_id") = "12846"
                Session("tier") = "1"
            End If
        End If

        If Session("tier") = "1" Then
            redeem.InnerText = "Each point is worth 50 cents. So for example, if you have 25 points a voucher will be created for R12.50"
        ElseIf Session("tier") = "2" Then
            redeem.InnerText = "Each point is worth R1. So for example, if you have 25 points a voucher will be created for R25"
        End If

        If Not IsPostBack Then
            SetUserData()
        End If

        dxGrid.DataBind()

    End Sub

    Protected Sub txtCardNumber_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim tb As ASPxTextBox = TryCast(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(tb.NamingContainer, GridViewDataItemTemplateContainer)

        Dim key As String = container.KeyValue.ToString()

        tb.ClientInstanceName = "card_number" + key
        'tb.ClientSideEvents.KeyUp = String.Format("function(s, e) {{ OnKeyUpMarkup(s, e, {0}); }}", key)
        tb.Attributes.Add("onclick", "openwindow(" & "card_number" + key & ");")
        ' "openwindow(s,e," & key & ");")
        '("onclick", String.Format("OnCellClick(this, \"{0}\");", e.CellValue));
        tb.ReadOnly = True
        

    End Sub

    Protected Sub txtBalance_Init(ByVal sender As Object, ByVal e As EventArgs)
        'Dim tb As ASPxTextBox = TryCast(sender, ASPxTextBox)
        'Dim container As GridViewDataItemTemplateContainer = TryCast(tb.NamingContainer, GridViewDataItemTemplateContainer)

        'Dim key As String = container.KeyValue.ToString()

        'tb.ClientInstanceName = "markup" + key
        'tb.ClientSideEvents.KeyUp = String.Format("function(s, e) {{ OnKeyUpMarkup(s, e, {0}); }}", key)
        'tb.ClientSideEvents.KeyPress = "NumericOnly"

    End Sub

   Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        dxGrid.DataSource = _BLayer.GetVouchers(Session("user_id"))

    End Sub

    Private Sub SetUserData()
        Dim userData As DataTable = _BLayer.GetUserDataValues(Session("user_id"))
        If (Not IsNothing(userData) And userData.Rows.Count() > 0) Then
            stylesRated.InnerText = Val(userData.Rows.Item(0).Item(0))
            pointsAwarded.InnerText = Val(userData.Rows.Item(0).Item(1))
        End If
    End Sub



    Protected Sub cmdGenerateNewVoucher_Click(sender As Object, e As EventArgs) Handles cmdGenerateNewVoucher.Click
        cmdGenerateNewVoucher.Enabled = False

        Dim ReturnString As String = _BLayer.RedeemVoucher(Session("user_id"), Session("tier"))

        If ReturnString = "NotEnoughPoints" Then
            NotEnoughPoints.Value = "true"
        End If

        dxGrid.DataBind()

        SetUserData()

    End Sub
End Class