Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class PrePacks
    Inherits System.Web.UI.Page

    Private Sub PrePacks_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim url As String = Request.Url.AbsoluteUri

        If HttpContext.Current.IsDebuggingEnabled Then
            Session("current_company") = "010"
        End If

        Me.Form.DefaultButton = cmdAccept.UniqueID

        Page.Server.ScriptTimeout = 300

        If (Not IsPostBack) Then
            'Populate size grid
            Dim _BLayerStock As New StockcodesHOBL()
            Dim _dt As DataTable

            _dt = _BLayerStock.GetSizeGrids

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboSizeGrid.Items.Add(_dt(i)("grid_number") & " - " & _dt(i)("grid_description"))
            Next
        End If

    End Sub

    Protected Sub PrePack_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim tb As ASPxTextBox = TryCast(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(tb.NamingContainer, GridViewDataItemTemplateContainer)

        Dim key As String = container.KeyValue.ToString()

        tb.ClientInstanceName = "prepack" + key
        tb.ClientSideEvents.KeyPress = "NumericOnly"

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.PrePacks) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub PrePacks_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback

        'Search
        If hdWhichButton.Value = "IndexChange" Then

            If cboSizeGrid.Text = "" Then Exit Sub

            Dim _dt As New DataTable
            Dim _BLayer As New StockcodesHOBL()

            Dim size() As String
            If cboSizeGrid.Text <> "" Then
                size = cboSizeGrid.Text.Split(" - ")
            End If

            _dt = _BLayer.GetSizeGridDescription(size(0).ToUpper)

            Dim dt As New DataTable

            dt.Columns.Add("ID")
            dt.Columns.Add("SizeGridNumber")
            dt.Columns.Add("Size")
            dt.Columns.Add("PrePack")

            For i As Integer = 1 To 30
                If _dt.Rows(0)("s" & i) <> "" Then
                    dt.Rows.Add(i, "Size " & i, _dt.Rows(0)("s" & i))
                End If
            Next

            grdSizes.DataSource = dt
            grdSizes.DataBind()

            _dt = _BLayer.GetPrePacks(size(0))

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboPackSize.Items.Add(_dt(i)("pre_pack_code"))
            Next

        End If

        'PrePack
        If hdWhichButton.Value = "PrePackIndexChange" Then

            If cboSizeGrid.Text = "" Then Exit Sub
            If cboPackSize.Text = "" Then Exit Sub

            Dim _dt As New DataTable
            Dim _BLayer As New StockcodesHOBL()

            Dim size() As String
            If cboSizeGrid.Text <> "" Then
                size = cboSizeGrid.Text.Split(" - ")
            End If

            _dt = _BLayer.GetPrePackData(size(0).ToUpper, cboPackSize.Text)

            'New pre pack
            If _dt.Rows.Count = 0 Then Exit Sub

            Dim dt As New DataTable

            dt.Columns.Add("ID")
            dt.Columns.Add("SizeGridNumber")
            dt.Columns.Add("Size")
            dt.Columns.Add("PrePack")

            For i As Integer = 0 To _dt.Rows.Count - 1
                dt.Rows.Add(i, "Size " & i, _dt.Rows(i)("size"), _dt.Rows(i)("pre_pack_quantity"))
            Next

            grdSizes.DataSource = dt
            grdSizes.DataBind()

            '_dt = _BLayer.GetPrePacks(cboSizeGrid.Text)

            'For i As Integer = 0 To _dt.Rows.Count - 1
            '    cboPackSize.Items.Add(_dt(i)("pre_pack_code"))
            'Next


        End If

        'Submit
        If hdWhichButton.Value = "Accept" Then
            If cboSizeGrid.Text = "" Then Exit Sub
            If cboPackSize.Text = "" Then Exit Sub

            Dim _dt As New DataTable
            Dim _BLayer As New StockcodesHOBL()

            Dim size() As String
            If cboSizeGrid.Text <> "" Then
                size = cboSizeGrid.Text.Split(" - ")
            End If

            Dim _pp As New PrePack
            _pp.PrePackCode = cboPackSize.Text
            _pp.SizeGrid = size(0)

            For i As Integer = 0 To grdSizes.VisibleRowCount - 1
                'Dim column1 As GridViewDataColumn = TryCast(grdSizes.Columns("Size"), GridViewDataColumn)
                'Dim txtSize As ASPxTextBox = CType(grdSizes.FindRowCellTemplateControl(i, column1, "txtBox"), ASPxTextBox)

                Dim column2 As GridViewDataColumn = TryCast(grdSizes.Columns("PrePack"), GridViewDataColumn)
                Dim txtPrePack As ASPxTextBox = CType(grdSizes.FindRowCellTemplateControl(i, column2, "txtBox"), ASPxTextBox)

                Dim prepackdata As New PrePackGrid
                prepackdata.Size = grdSizes.GetRowValues(i, "Size")

                prepackdata.Quantity = txtPrePack.Text

                _pp.PrePackGrid.Add(prepackdata)

            Next

            Dim ReturnString As String = ""

            ReturnString = _BLayer.InsertPrePack(_pp)

            If ReturnString <> "Success" Then
                lblError.Text = ReturnString
            Else
                lblError.Text = "The Pre Pack listing has been updated!"
                ClearScreen()
            End If

            dxPopUpError.ShowOnPageLoad = True

        End If

        'Clear
        If hdWhichButton.Value = "Clear" Then
            ClearScreen()
        End If
    End Sub

    Private Sub ClearScreen()

        Dim _dt As New DataTable
        grdSizes.DataSource = _dt
        grdSizes.DataBind()

        cboSizeGrid.Text = ""

        cboPackSize.Items.Clear()
        cboPackSize.Text = ""

    End Sub

    Protected Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click

    End Sub
End Class