Imports System.Data
Imports pcm.BusinessLayer
Imports DevExpress.Web

Public Class ProductSelection
    Inherits System.Web.UI.Page

    Dim _buyingCodes As List(Of String) = New List(Of String)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim url As String = Request.Url.AbsoluteUri

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
            End If
        End If

        If Not IsPostBack Then
            '    Dim _BLayer As BuyingBL = New BuyingBL("010")

            '    'Populated from the user login
            '    'If viewClothing = "" then the clothing questions popup will be displayed
            '    'viewClothing.Value = Session("view_clothing")
            '    viewClothing.Value = False

            '    Dim data As New DataTable

            '    'If Session("view_clothing") = "" Then
            '    data = _BLayer.GetAllBuyingStockCodes(Session("user_id"), False)
            '    'Else
            '    'data = _BLayer.GetAllBuyingStockCodes(Session("user_id"), Session("view_clothing"))
            '    'End If

            '    For Each item As DataRow In data.Rows
            '        _buyingCodes.Add(item(0).ToString)
            '    Next

            SetUserData()

            If _buyingCodes.Count = 0 Then
                itemCode.InnerText = ""
                itemMaterial.InnerText = ""
                itemDescription.InnerText = ""
                itemPrice.InnerText = "NO NEW STYLES TO RATE :("

                Dim stringData As StringBuilder = New StringBuilder()
                stringData.Append("<div>")
                Dim path As String = "../Images/no_moreX300.png"
                stringData.Append("<img u='image' src=" + path + " />")
                Dim mainPath As String = "../Images/no_moreX68.png"
                stringData.Append("<img u='image' src=" + mainPath + " />")
                stringData.Append("</div>")

                previewPanel.InnerHtml = stringData.ToString

                SaveItmButton.Visible = False
                noItemRated.Value = "true"
                Exit Sub
            End If

            hdTimeDataServed.Value = Format(Now, "yyyy-MM-dd HH:mm:ss")

            Session("buying_codes") = _buyingCodes

            currentRateIndex.Value = 0
            FillData(_buyingCodes.Item(0))

        End If



    End Sub

    'Protected Sub SaveItemDataClick(sender As Object, e As EventArgs) Handles SaveButton.ServerClick
    '    SaveButton.Disabled = True

    '    If IsNothing(Session("buying_codes")) Then
    '        Exit Sub
    '    End If

    '    _BLayer.InsertStockCodeRating(Session.Item("SelectedCode"), Val(currentRating.Value), currentPriceRange.Value, Session("user_id"))
    '    _BLayer.UpdateUserPoints(1, Session("user_id"))
    '    Dim nextItemToGet As Integer = currentRateIndex.Value
    '    If Session("buying_codes").Count() - 1 > nextItemToGet Then
    '        currentRateIndex.Value = nextItemToGet + 1
    '        Dim itemCode As String = Session("buying_codes").Item(nextItemToGet + 1)
    '        FillData(itemCode)
    '    Else
    '        ratingCompleted.Value = "true"
    '        itemCode.InnerText = ""
    '        itemMaterial.InnerText = ""
    '        itemDescription.InnerText = ""
    '        itemPrice.InnerText = "NO NEW STYLES TO RATE :("

    '        Dim stringData As StringBuilder = New StringBuilder()
    '        stringData.Append("<div>")
    '        Dim path As String = "../Images/no_moreX300.png"
    '        stringData.Append("<img u='image' src=" + path + " />")
    '        Dim mainPath As String = "../Images/no_moreX68.png"
    '        stringData.Append("<img u='image' src=" + mainPath + " />")
    '        stringData.Append("</div>")
    '        SaveButton.Visible = False
    '        previewPanel.InnerHtml = stringData.ToString

    '    End If
    '    SetUserData()
    'End Sub

    'Protected Sub SkipItemDataClick(sender As Object, e As EventArgs) Handles SkipButton.ServerClick
    '    Dim nextItemToGet As Integer = currentRateIndex.Value
    '    If Session("buying_codes").Count() - 1 > nextItemToGet Then
    '        currentRateIndex.Value = nextItemToGet + 1
    '        Dim itemCode As String = Session("buying_codes").Item(nextItemToGet + 1)
    '        FillData(itemCode)
    '    Else
    '        ratingCompleted.Value = "true"
    '    End If
    'End Sub

    Private Sub SetUserData()
        Dim _BLayer As BuyingBL = New BuyingBL()
        Dim userData As DataTable = _BLayer.GetUserDataValues(Session("user_id"))
        If (Not IsNothing(userData) And userData.Rows.Count() > 0) Then
            stylesRated.InnerText = Val(userData.Rows.Item(0).Item(0))
            pointsAwarded.InnerText = Val(userData.Rows.Item(0).Item(1))
        End If
    End Sub

    Private Sub FillData(itemCodeData As String)
        Dim _BLayer As BuyingBL = New BuyingBL()
        Dim item As DataTable = _BLayer.GetStockcodeDetail(itemCodeData)
        If Not IsNothing(item) Then
            itemCode.InnerText = "PRICE: R" + item.Rows(0).Item("price").ToString
            'itemMaterial.InnerText = item.Rows(0).Item("material")
            itemMaterial.InnerText = itemCodeData
            itemDescription.InnerText = item.Rows(0).Item("description")
            itemPrice.InnerText = "Details"
            Session.Item("SelectedCode") = itemCodeData
        End If

        FillImageInfo(itemCodeData)
    End Sub

    Private Sub FillImageInfo(itemCodeData As String)
        Dim _BLayer As BuyingBL = New BuyingBL()
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

    Protected Sub SaveItmButton_Click(sender As Object, e As EventArgs) Handles SaveItmButton.Click

        SaveItmButton.Enabled = False

        If IsNothing(Session("buying_codes")) Then
            Exit Sub
        End If

        Dim ServedTime As Date = CDate(hdTimeDataServed.Value)
        Dim TimeNow As Date = Format(Now, "yyyy-MM-dd HH:mm:ss")

        Dim LengthOfAction As String
        LengthOfAction = DateDiff(DateInterval.Second, ServedTime, TimeNow)

        Dim _BLayer As BuyingBL = New BuyingBL()

        _BLayer.InsertStockCodeRating(Session.Item("SelectedCode"), Val(currentRating.Value),
                              currentPriceRange.Value, Session("user_id"), LengthOfAction, txtComments.Text)
        _BLayer.UpdateUserPoints(1, Session("user_id"))

        'Check if first time for voting clothing
        'If viewClothing = "" then the question box will be displayed
        'currentClothing is only populated when user makes a Yes / No selection for the first time
        'If currentClothing.Value = "Yes" Then
        '    'Get the new listing which includes clothing
        '    viewClothing.Value = "Yes"
        '    currentClothing.Value = ""
        '    Session("view_clothing") = "True"
        '    _BLayer.UpdateUserForClothing(Session("user_id"), True)
        '    GetNewListing()
        '    Exit Sub
        'ElseIf currentClothing.Value = "No" Then
        '    'Default was no clothing to begin with so no need to geta new list
        '    viewClothing.Value = "No"
        '    currentClothing.Value = ""
        '    Session("view_clothing") = "False"
        '    _BLayer.UpdateUserForClothing(Session("user_id"), False)
        'End If

        Dim nextItemToGet As Integer = currentRateIndex.Value
        If Session("buying_codes").Count() - 1 > nextItemToGet Then
            currentRateIndex.Value = nextItemToGet + 1
            Dim itemCode As String = Session("buying_codes").Item(nextItemToGet + 1)
            FillData(itemCode)
            hdTimeDataServed.Value = Format(Now, "yyyy-MM-dd HH:mm:ss")
            txtComments.Text = ""
        Else
            ratingCompleted.Value = "true"
            itemCode.InnerText = ""
            itemMaterial.InnerText = ""
            itemDescription.InnerText = ""
            itemPrice.InnerText = "NO NEW STYLES TO RATE :("
            txtComments.Text = ""

            Dim stringData As StringBuilder = New StringBuilder()
            stringData.Append("<div>")
            Dim path As String = "../Images/no_moreX300.png"
            stringData.Append("<img u='image' src=" + path + " />")
            Dim mainPath As String = "../Images/no_moreX68.png"
            stringData.Append("<img u='image' src=" + mainPath + " />")
            stringData.Append("</div>")
            SaveItmButton.Visible = False
            previewPanel.InnerHtml = stringData.ToString

        End If
        SetUserData()
    End Sub

    Private Sub GetNewListing()

        Dim _BLayer As BuyingBL = New BuyingBL()

        Dim data As New DataTable
        data = _BLayer.GetAllBuyingStockCodes(Session("user_id"), Session("view_clothing"))

        For Each item As DataRow In data.Rows
            _buyingCodes.Add(item(0).ToString)
        Next

        SetUserData()

        If _buyingCodes.Count = 0 Then
            itemCode.InnerText = ""
            itemMaterial.InnerText = ""
            itemDescription.InnerText = ""
            itemPrice.InnerText = "NO NEW STYLES TO RATE :("

            Dim stringData As StringBuilder = New StringBuilder()
            stringData.Append("<div>")
            Dim path As String = "../Images/no_moreX300.png"
            stringData.Append("<img u='image' src=" + path + " />")
            Dim mainPath As String = "../Images/no_moreX68.png"
            stringData.Append("<img u='image' src=" + mainPath + " />")
            stringData.Append("</div>")

            previewPanel.InnerHtml = stringData.ToString

            SaveItmButton.Visible = False
            noItemRated.Value = "true"
            Exit Sub
        End If

        hdTimeDataServed.Value = Format(Now, "yyyy-MM-dd HH:mm:ss")

        Session("buying_codes") = _buyingCodes

        currentRateIndex.Value = 0
        FillData(_buyingCodes.Item(0))
    End Sub


End Class
