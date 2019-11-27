Public Class repDetails
    Inherits DevExpress.XtraReports.UI.XtraReport

#Region " Designer generated code "
    Private _Source As String

    Public Sub New()
        MyBase.New()

        'This call is required by the Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Sub SetSource(ByVal tmpSource As String)
        _Source = tmpSource
    End Sub

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
    Private WithEvents DetailReport As DevExpress.XtraReports.UI.DetailReportBand
    Private WithEvents Detail1 As DevExpress.XtraReports.UI.DetailBand
    Private WithEvents xrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel3 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel2 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrTable1 As DevExpress.XtraReports.UI.XRTable
    Private WithEvents xrTableRow1 As DevExpress.XtraReports.UI.XRTableRow
    Private WithEvents xrTableCell4 As DevExpress.XtraReports.UI.XRTableCell
    Private WithEvents xrTableCell1 As DevExpress.XtraReports.UI.XRTableCell
    Private WithEvents xrTableCell2 As DevExpress.XtraReports.UI.XRTableCell
    Private WithEvents xrTableCell3 As DevExpress.XtraReports.UI.XRTableCell

    'Required by the Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Designer
    'It can be modified using the Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.DetailReport = New DevExpress.XtraReports.UI.DetailReportBand()
        Me.Detail1 = New DevExpress.XtraReports.UI.DetailBand()
        Me.xrTable1 = New DevExpress.XtraReports.UI.XRTable()
        Me.xrTableRow1 = New DevExpress.XtraReports.UI.XRTableRow()
        Me.xrTableCell4 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.xrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrTableCell1 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.xrLabel2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrTableCell2 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.xrLabel3 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrTableCell3 = New DevExpress.XtraReports.UI.XRTableCell()
        Me.xrLabel4 = New DevExpress.XtraReports.UI.XRLabel()
        CType(Me.xrTable1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'Detail
        '
        Me.Detail.HeightF = 0!
        Me.Detail.Name = "Detail"
        Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
        Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'TopMargin
        '
        Me.TopMargin.HeightF = 10.0!
        Me.TopMargin.Name = "TopMargin"
        Me.TopMargin.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
        Me.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'BottomMargin
        '
        Me.BottomMargin.HeightF = 0!
        Me.BottomMargin.Name = "BottomMargin"
        Me.BottomMargin.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
        Me.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'DetailReport
        '
        Me.DetailReport.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail1})
        Me.DetailReport.DataMember = "Transactions"
        Me.DetailReport.Level = 0
        Me.DetailReport.Name = "DetailReport"
        '
        'Detail1
        '
        Me.Detail1.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.Detail1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrTable1})
        Me.Detail1.HeightF = 31.0!
        Me.Detail1.Name = "Detail1"
        Me.Detail1.StylePriority.UseBorders = False
        '
        'xrTable1
        '
        Me.xrTable1.Borders = CType((DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom), DevExpress.XtraPrinting.BorderSide)
        Me.xrTable1.LocationFloat = New DevExpress.Utils.PointFloat(0!, 0!)
        Me.xrTable1.Name = "xrTable1"
        Me.xrTable1.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.xrTableRow1})
        Me.xrTable1.SizeF = New System.Drawing.SizeF(569.0!, 31.0!)
        Me.xrTable1.StylePriority.UseBorders = False
        '
        'xrTableRow1
        '
        Me.xrTableRow1.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.xrTableCell4, Me.xrTableCell1, Me.xrTableCell2, Me.xrTableCell3})
        Me.xrTableRow1.Name = "xrTableRow1"
        Me.xrTableRow1.Weight = 1.0R
        '
        'xrTableCell4
        '
        Me.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.xrTableCell4.CanShrink = True
        Me.xrTableCell4.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrLabel1})
        Me.xrTableCell4.Name = "xrTableCell4"
        Me.xrTableCell4.StylePriority.UseBorders = False
        Me.xrTableCell4.Text = "xrTableCell4"
        Me.xrTableCell4.Weight = 0.5R
        '
        'xrLabel1
        '
        Me.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.xrLabel1.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Transactions.TransactionDate")})
        Me.xrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(8.0!, 2.000014!)
        Me.xrLabel1.Name = "xrLabel1"
        Me.xrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel1.SizeF = New System.Drawing.SizeF(84.83334!, 23.0!)
        Me.xrLabel1.StylePriority.UseBorders = False
        Me.xrLabel1.Text = "xrLabel1"
        '
        'xrTableCell1
        '
        Me.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.xrTableCell1.CanShrink = True
        Me.xrTableCell1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrLabel2})
        Me.xrTableCell1.Name = "xrTableCell1"
        Me.xrTableCell1.StylePriority.UseBorders = False
        Me.xrTableCell1.Text = "xrTableCell1"
        Me.xrTableCell1.Weight = 0.59385794864447R
        '
        'xrLabel2
        '
        Me.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.xrLabel2.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Transactions.ID")})
        Me.xrLabel2.LocationFloat = New DevExpress.Utils.PointFloat(4.000013!, 2.000014!)
        Me.xrLabel2.Name = "xrLabel2"
        Me.xrLabel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel2.SizeF = New System.Drawing.SizeF(102.635!, 23.0!)
        Me.xrLabel2.StylePriority.UseBorders = False
        Me.xrLabel2.Text = "xrLabel2"
        '
        'xrTableCell2
        '
        Me.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.xrTableCell2.CanShrink = True
        Me.xrTableCell2.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrLabel3})
        Me.xrTableCell2.Name = "xrTableCell2"
        Me.xrTableCell2.StylePriority.UseBorders = False
        Me.xrTableCell2.Text = "xrTableCell2"
        Me.xrTableCell2.Weight = 1.35430283634529R
        '
        'xrLabel3
        '
        Me.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.xrLabel3.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Transactions.TransactionDescription")})
        Me.xrLabel3.LocationFloat = New DevExpress.Utils.PointFloat(10.00001!, 2.000014!)
        Me.xrLabel3.Name = "xrLabel3"
        Me.xrLabel3.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel3.SizeF = New System.Drawing.SizeF(236.866!, 23.0!)
        Me.xrLabel3.StylePriority.UseBorders = False
        Me.xrLabel3.Text = "xrLabel3"
        '
        'xrTableCell3
        '
        Me.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.xrTableCell3.CanShrink = True
        Me.xrTableCell3.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrLabel4})
        Me.xrTableCell3.Name = "xrTableCell3"
        Me.xrTableCell3.StylePriority.UseBorders = False
        Me.xrTableCell3.Text = "xrTableCell3"
        Me.xrTableCell3.Weight = 0.551839215010241R
        '
        'xrLabel4
        '
        Me.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.xrLabel4.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Transactions.TransactionAmount")})
        Me.xrLabel4.LocationFloat = New DevExpress.Utils.PointFloat(9.999922!, 2.000014!)
        Me.xrLabel4.Name = "xrLabel4"
        Me.xrLabel4.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel4.SizeF = New System.Drawing.SizeF(84.66553!, 23.0!)
        Me.xrLabel4.StylePriority.UseBorders = False
        Me.xrLabel4.Text = "xrLabel4"
        '
        'repDetails
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail, Me.TopMargin, Me.BottomMargin, Me.DetailReport})
        Me.Borders = DevExpress.XtraPrinting.BorderSide.Top
        Me.Margins = New System.Drawing.Printing.Margins(0, 257, 10, 0)
        Me.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic
        Me.Version = "15.1"
        Me.XmlDataPath = "F:\Website\pcm\Data\data.xml"
        CType(Me.xrTable1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents TopMargin As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMargin As DevExpress.XtraReports.UI.BottomMarginBand

#End Region

    Private Sub Detail_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles Me.BeforePrint
        XmlDataPath = HttpContext.Current.Server.MapPath("data\" & _Source & ".XML")
        Me.FillDataSource()

        'Delete the file
        System.IO.File.Delete(HttpContext.Current.Server.MapPath("data\" & _Source & ".XML"))
    End Sub





End Class