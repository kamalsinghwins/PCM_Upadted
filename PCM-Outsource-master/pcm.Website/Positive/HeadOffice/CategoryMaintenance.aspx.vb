Imports DevExpress.Web
Imports Entities
Imports Entities.Manage
Imports pcm.BusinessLayer
Public Class CategoryMaintenance
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Shared _Blayer As New ManageHOBL
    Dim dt As New DataTable
    Dim baseResponse As New BaseResponse

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri
        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("maintenance_permission_sequence"), Screens.Maintenance.ManageCategory) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If
        If Not IsPostBack Then
            BindCategories()
        End If
    End Sub
    Private Sub CategoryMaintenance_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

        Try
            If hdWhichButton.Value = "SelectCategory" Then
                SelectCategory()
            End If

            If hdWhichButton.Value = "Save" Then
                SaveCategory()
            End If

            If hdWhichButton.Value = "Delete" Then
                DeleteCategory()
            End If
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Protected Sub SelectCategory()
        Dim arrArray() As String

        If hdWhichButton1.Value = "Category1" Then
            arrArray = Split(lstCategory1.Value, " - ")
            cboCategory.SelectedIndex = 0

        ElseIf hdWhichButton1.Value = "Category2" Then
            arrArray = Split(lstCategory2.Value, " - ")
            cboCategory.SelectedIndex = 1

        ElseIf hdWhichButton1.Value = "Category3" Then
            arrArray = Split(lstCategory3.Value, " - ")
            cboCategory.SelectedIndex = 2
        End If

        CategoryTab.ActiveTabIndex = 0
        txtCategoryCode.Text = arrArray(0)
        txtCategoryDescription.Text = arrArray(1)
    End Sub
    Private Sub SaveCategory()
        If txtCategoryCode.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please input a Category Codes."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If txtCategoryDescription.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "A Category Description is required"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        If cboCategory.Text = "" Then
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Please select a Category Number"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Dim category As New Categories
        category.CategoryCode = txtCategoryCode.Text
        category.Description = txtCategoryDescription.Text
        category.CategoryNumber = cboCategory.Text
        category.CategoryType = "STOCK"

        baseResponse = _Blayer.SaveCategory(category)
        If baseResponse.Success = True Then
            dxPopUpError.HeaderText = "Success"
            lblError.Text = baseResponse.Message
            ClearForm()
            BindCategories()
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Something Went Wrong"
        End If
        dxPopUpError.ShowOnPageLoad = True
    End Sub
    Private Sub ClearForm()
        txtCategoryCode.Text = String.Empty
        txtCategoryDescription.Text = String.Empty
        cboCategory.SelectedIndex = -1
    End Sub
    Private Sub DeleteCategory()
        Dim arrArray() As String
        Dim categoryCode As String = String.Empty

        If CategoryTab.ActiveTabIndex = 0 Then
            dxPopUpError.HeaderText = "Success"
            lblError.Text = "Please select the category code"
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Dim category As New Categories
        category.CategoryType = "STOCK"
        category.CategoryNumber = CategoryTab.ActiveTabIndex


        If CategoryTab.ActiveTabIndex = 1 Then
            arrArray = Split(lstCategory1.Value, " - ")

            If arrArray(0).Length > 0 Then
                category.CategoryCode = arrArray(0)
            Else
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "Please select the category code"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
                Exit Sub
            End If
        End If


        If CategoryTab.ActiveTabIndex = 2 Then
            arrArray = Split(lstCategory2.Value, " - ")
            If arrArray(0).Length > 0 Then
                category.CategoryCode = arrArray(0)
            Else
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "Please select the category code"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
                Exit Sub
            End If
        End If

        If CategoryTab.ActiveTabIndex = 3 Then
            arrArray = Split(lstCategory3.Value, " - ")
            If arrArray(0).Length > 0 Then
                category.CategoryCode = arrArray(0)
            Else
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "Please select the category code"
                dxPopUpError.ShowOnPageLoad = True
                Exit Sub
                Exit Sub
            End If
        End If

        baseResponse = _Blayer.DeleteCategory(category)
        If baseResponse.Success = True Then
            dxPopUpError.HeaderText = "Success"
            lblError.Text = baseResponse.Message
            ClearForm()
            BindCategories()
        Else
            dxPopUpError.HeaderText = "Error"
            lblError.Text = "Something Went Wrong"
        End If
        dxPopUpError.ShowOnPageLoad = True

    End Sub
    Private Sub BindCategories()
        Try
            dt = _Blayer.GetCategories("STOCK", 1)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each drCategories As DataRow In dt.Rows
                    lstCategory1.Items.Add(drCategories.Item("category_code") & " - " & drCategories.Item("category_description") & "")
                Next
            End If
            dt.Clear()

            dt = _Blayer.GetCategories("STOCK", 2)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each drCategories As DataRow In dt.Rows
                    lstCategory2.Items.Add(drCategories.Item("category_code") & " - " & drCategories.Item("category_description") & "")
                Next
            End If
            dt.Clear()

            dt = _Blayer.GetCategories("STOCK", 3)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each drCategories As DataRow In dt.Rows
                    lstCategory3.Items.Add(drCategories.Item("category_code") & " - " & drCategories.Item("category_description") & "")
                Next
            End If
            dt.Clear()

            'Add to the dropdown
            cboCategory.Items.Clear()
            cboCategory.Items.Add("Category 1")
            cboCategory.Items.Add("Category 2")
            cboCategory.Items.Add("Category 3")
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try


    End Sub
End Class