Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports DevExpress
Imports CodeCarvings.Piczard
Imports System.IO

Public Class ProductImageUpload
    Inherits System.Web.UI.Page

    Private Const UploadDirectory As String = "~/Uploaded/"
    Private Const TempDirectory As String = "~/Temp/"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("current_company") = "011"


        Dim url As String = Request.Url.AbsoluteUri

        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.UploadBuyingImages) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("current_company") = "010"
        End If

        If Not IsPostBack Then

            cboCategory.Items.Add("Shoes")
            cboCategory.Items.Add("Clothing")

            ' Add a fixed size crop constraint (500 x 500 pixels)
            picUpload.CropConstraint = New FixedCropConstraint(600, 600)
            ' Automatically open image edit window after upload
            picUpload.AutoOpenImageEditPopupAfterUpload = True
            ' Set a square resize constraint (200 x 200 pixels, bg:black) for the preview
            picUpload.PreviewFilter = New FixedResizeConstraint(200, 200)

            Session("file_name") = ""
        End If

        If (Me.ScriptManager1.IsInAsyncPostBack) Then
            ' After every Ajax postback re-initialize the JQuery UI elements
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "initializeUI", "initializeUI();", True)
        End If
    End Sub

    Private Sub ProductImageUpload_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Private Sub ClearScreen()
        txtDescription.Text = ""
        txtMaterials.Text = ""
        txtQtyOrdered.Text = ""
        txtStockcode.Text = ""
        txtStockcodeSearch.Text = ""
        txtPrice.Text = ""

        Session("file_name") = ""

        txtStockcode.ReadOnly = False

        grdStockcodeSearch.DataBind()

        txtDisplayOrder.Text = ""

        hdDEWhichButton.Set("is_update", "")

        cboCategory.Text = ""

        If (Me.picUpload.HasImage) Then
            Me.picUpload.UnloadImage()
        End If

    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        If hdDEWhichButton.Get("clicked") = "Clear" Then
            ClearScreen()
        End If

        'StockcodeLostFocus
        If hdDEWhichButton.Get("clicked") = "StockcodeLostFocus" Then
            Dim _dt As New DataTable
            Dim _BLayer As New BuyingBL()

            _dt = _BLayer.GetStockcodeDetail(txtStockcode.Text)
            If _dt.Rows.Count = 0 Then
                hdDEWhichButton.Set("is_update", "false")
                Exit Sub
            Else
                hdDEWhichButton.Set("is_update", "true")
            End If
            txtDescription.Text = _dt.Rows(_dt.Rows.Count - 1)("description").ToUpper
            txtMaterials.Text = _dt.Rows(_dt.Rows.Count - 1)("material").ToUpper
            txtQtyOrdered.Text = _dt.Rows(_dt.Rows.Count - 1)("qty_ordered")
            txtPrice.Text = _dt.Rows(_dt.Rows.Count - 1)("price")
            cboCategory.Text = _dt.Rows(_dt.Rows.Count - 1)("category")
            'txtStockcode.ReadOnly = True
        End If

        'Search
        If hdDEWhichButton.Get("clicked") = "Search" Then
            grdStockcodeSearch.DataBind()
        End If

        ''Search
        'If hdDEWhichButton.Get("clicked") = "EditPicture" Then
        '    Dim fileName As String = hdDEWhichButton.Get("filetoupload")
        '    'For i As Integer = 0 To UploadControl.UploadedFiles.Length - 1
        '    '    Dim file As UploadedFile = UploadControl.UploadedFiles(i)
        '    '    fileName = file.FileName
        '    'Next i

        '    If IO.File.Exists(fileName) = False Then
        '        Exit Sub
        '    End If

        '    '   Me.popupPictureTrimmer1.LoadImageFromFileSystem(fileName, New FixedCropConstraint(300, 300))

        '    ' Open the image edit popup
        '    '   Me.popupPictureTrimmer1.OpenPopup(800, 510)
        'End If

        'Select "searched" code
        If hdDEWhichButton.Get("clicked") = "Select" Then

            Dim selectedValues = New List(Of Object)()

            selectedValues = Nothing

            selectedValues = grdStockcodeSearch.GetSelectedFieldValues("stockcode")

            If selectedValues.Count > 0 Then
                Dim _dt As New DataTable
                Dim _BLayer As New BuyingBL()

                _dt = _BLayer.GetStockcodeDetail(selectedValues(selectedValues.Count - 1))
                If _dt.Rows.Count = 0 Then
                    txtStockcode.Text = selectedValues(selectedValues.Count - 1)
                    pcMain.ShowOnPageLoad = False
                    hdDEWhichButton.Set("is_update", "false")
                    Exit Sub
                End If

                txtStockcode.Text = selectedValues(selectedValues.Count - 1)
                txtDescription.Text = _dt.Rows(_dt.Rows.Count - 1)("description")
                txtMaterials.Text = _dt.Rows(_dt.Rows.Count - 1)("material")
                txtQtyOrdered.Text = _dt.Rows(_dt.Rows.Count - 1)("qty_ordered")
                txtPrice.Text = _dt.Rows(_dt.Rows.Count - 1)("price")
                txtDisplayOrder.Text = _dt.Rows(_dt.Rows.Count - 1)("display_order")
                cboCategory.Text = _dt.Rows(_dt.Rows.Count - 1)("category")
                hdDEWhichButton.Set("is_update", "true")
            End If

            pcMain.ShowOnPageLoad = False
        End If

        If hdDEWhichButton.Get("clicked") = "Upload" Then

            If txtStockcode.Text = "" Then
                lblError.Text = "No Stockcode was entered. Files were not uploaded."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If txtDescription.Text = "" Then
                lblError.Text = "No description was entered. Files were not uploaded."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If txtMaterials.Text = "" Then
                lblError.Text = "No material was entered. Files were not uploaded."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If txtQtyOrdered.Text = "" Then
                lblError.Text = "No Qty Ordered was entered. Files were not uploaded."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If txtPrice.Text = "" Then
                lblError.Text = "No Price was entered. Files were not uploaded."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If cboCategory.Text = "" Then
                lblError.Text = "No category was selected. Files were not uploaded."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            If Session("file_name") = "" Then
                lblError.Text = "Please edit and confirm the image is correct by clicking 'Ok' before saving."
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
            End If

            Dim _BLayer As New BuyingBL()
            Dim ReturnString As String = ""

            'Dim fileName As String = Path.Combine(MapPath(TempDirectory), Session("file_name"))

            'IO.File.Copy(fileName, Path.Combine(MapPath(TempDirectory), Session("file_name")))

            Dim ImageOrder As String

            If txtDisplayOrder.Text = "" Then
                ImageOrder = "1"
            Else
                ImageOrder = txtDisplayOrder.Text
            End If

            If hdDEWhichButton.Get("is_update") = "false" Then
                ReturnString = _BLayer.InsertProduct(txtStockcode.Text, txtDescription.Text, txtMaterials.Text, txtQtyOrdered.Text,
                                                     Session("file_name"), txtPrice.Text, txtDisplayOrder.Text, cboCategory.Text)
            ElseIf hdDEWhichButton.Get("is_update") = "true" Then
                ReturnString = _BLayer.UpdateProduct(txtStockcode.Text, txtDescription.Text, txtMaterials.Text, txtQtyOrdered.Text,
                                                     Session("file_name"), txtPrice.Text, txtDisplayOrder.Text, cboCategory.Text)
            End If

            If ReturnString <> "Success" Then
                lblError.Text = ReturnString
            Else
                lblError.Text = "The Product Catalogue has been updated!"
            End If

            hdDEWhichButton.Set("is_update", "true")

            Session("file_name") = ""

            If (Me.picUpload.HasImage) Then
                Me.picUpload.UnloadImage()
            End If

            txtDisplayOrder.Text = ""

            dxPopUpError.ShowOnPageLoad = True
            'IO.File.Delete(fileName)
        End If

    End Sub

    Protected Sub grdStockcodeSearch_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        grdStockcodeSearch.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim _Blayer As New BuyingBL()

        Dim data As DataTable = _Blayer.GetStockcodes(txtStockcodeSearch.Text.ToUpper)

        gridView.KeyFieldName = "stockcode" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        grdStockcodeSearch.EndUpdate()

    End Sub

    Private Sub picUpload_ImageEdit(sender As Object, e As EventArgs) Handles picUpload.ImageEdit

        Dim savedImageFileName As String = ""
        Dim formatEncoderParams As FormatEncoderParams = Nothing

        Dim tmpFileName As String = Guid.NewGuid.ToString

        Dim pngformatEncoderParams As PngFormatEncoderParams = New PngFormatEncoderParams()
        formatEncoderParams = pngformatEncoderParams

        savedImageFileName = "~/TempDirectory/" & tmpFileName & ".jpg"

        picUpload.SaveProcessedImageToFileSystem(savedImageFileName)

        ' Setup an ImageProcessingJob / Output resolution = 72 DPI
        Dim job As ImageProcessingJob = New ImageProcessingJob(72)
        job.Filters.Add(New FixedCropConstraint(600, 600))

        ' Process the image
        job.SaveProcessedImageToFileSystem(savedImageFileName, UploadDirectory & tmpFileName & "x600.jpg")

        ' Setup an ImageProcessingJob / Output resolution = 72 DPI
        Dim job2 As ImageProcessingJob = New ImageProcessingJob(72)
        job2.Filters.Add(New FixedCropConstraint(68, 68))

        ' Process the image
        job2.SaveProcessedImageToFileSystem(savedImageFileName, UploadDirectory & tmpFileName & "x68.jpg")

        ' Setup an ImageProcessingJob / Output resolution = 72 DPI
        Dim job3 As ImageProcessingJob = New ImageProcessingJob(72)
        job3.Filters.Add(New FixedCropConstraint(300, 300))

        ' Process the image
        job3.SaveProcessedImageToFileSystem(savedImageFileName, UploadDirectory & tmpFileName & "x300.jpg")

        Session("file_name") = tmpFileName

        'IO.File.Delete(savedImageFileName)


    End Sub

End Class