Imports System.Data
Imports pcm.BusinessLayer
Imports DevExpress.Web

Public Class product_preview
    Inherits System.Web.UI.Page

    Dim _buyingCodes As List(Of String) = New List(Of String)
    Dim _BLayer As BuyingBL = New BuyingBL()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim url As String = Request.Url.AbsoluteUri

        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            End If
        Else
            Session("current_company") = "010"
        End If

        If Not IsPostBack Then

            'http://myrage.co.za/Buying/Product_Preview.aspx?code=BVML002PUYE
            Dim stockcode As String = Request.QueryString("code")

            If stockcode = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Buying/ProductImageUpload.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Buying/ProductImageUpload.aspx")
                End If
            End If

            FillData(stockcode)

        End If

    End Sub

    Private Sub FillData(itemCodeData As String)
        Dim item As DataTable = _BLayer.GetStockcodeDetail(itemCodeData)
        If Not IsNothing(item) Then
            itemCode.InnerText = "PRICE: R" + item.Rows(0).Item("price").ToString
            itemMaterial.InnerText = item.Rows(0).Item("material")
            itemDescription.InnerText = item.Rows(0).Item("description")
            itemPrice.InnerText = "Details"
            Session.Item("SelectedCode") = itemCodeData
        End If

        FillImageInfo(itemCodeData)
    End Sub

    Private Sub FillImageInfo(itemCodeData As String)
        Dim itemImages As DataTable = _BLayer.GetAllStockCodeImages(itemCodeData)
        Dim stringData As StringBuilder = New StringBuilder()
        If (Not IsNothing(itemImages) And itemImages.Rows.Count() > 0) Then

            For Each item As DataRow In itemImages.Rows
                stringData.Append("<div>")
                Dim path As String = "../Uploaded/" + item.Item(0) & "x300.jpg"
                stringData.Append("<img u='image' src=" + path + " />")
                Dim mainPath As String = "../Uploaded/" + item.Item(0) & "x68.jpg"
                stringData.Append("<img u='image' src=" + mainPath + " />")
                stringData.Append("</div>")
            Next

            previewPanel.InnerHtml = stringData.ToString

        End If
    End Sub

End Class