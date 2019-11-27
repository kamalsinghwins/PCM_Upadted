Imports DevExpress.Web
Imports System.ComponentModel

Public Class InventoryAndTax
    Inherits System.Web.UI.UserControl

    Public Property txtStockcodeText As String
        Get
            Return txtStockcode.Text
        End Get
        Set(value As String)
            txtStockcode.Text = value
        End Set
    End Property

    Public Property txtBarcodeText As String
        Get
            Return txtBarcode.Text
        End Get
        Set(value As String)
            txtBarcode.Text = value
        End Set
    End Property

    Public Property txtDescriptionText As String
        Get
            Return txtDescription.Text
        End Get
        Set(value As String)
            txtDescription.Text = value
        End Set
    End Property

    <Category("Data"), Bindable(True), _
    Browsable(True), EditorBrowsable(EditorBrowsableState.Always), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), _
    AttributeProvider(GetType(IListSource))> _
    Public Property cboPurchaseTaxDataSource As Object
        Get
            Return cboPurchaseTax.DataSource
        End Get
        Set(value As Object)
            cboPurchaseTax.DataSource = value
        End Set
    End Property

    <Category("Data"), Bindable(True), _
    Browsable(True), EditorBrowsable(EditorBrowsableState.Always), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), _
    AttributeProvider(GetType(IListSource))> _
    Public Property cboSalesTaxDataSource As Object
        Get
            Return cboSalesTax.DataSource
        End Get
        Set(value As Object)
            cboSalesTax.DataSource = value
        End Set
    End Property

    Public Sub DataBindPurchaseTax(ByVal DataSource As Object)

        cboPurchaseTax.DataSource = DataSource
        cboPurchaseTax.DataBind()

    End Sub

    Public Sub DataBindSalesTax(ByVal DataSource As Object)

        cboSalesTax.DataSource = DataSource
        cboSalesTax.DataBind()
        'cboSalesTaxDataSource = DataSource

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
End Class
