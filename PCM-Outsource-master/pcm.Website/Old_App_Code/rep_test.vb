Public Class rep_test
    Inherits DevExpress.XtraReports.UI.XtraReport

#Region " Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Sub SetSource(ByVal Source As String)
        Dim xrep As repDetails = DirectCast(Me.xrSubreport1.ReportSource, repDetails)

        xrep.SetSource(Source)

    End Sub

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
    Private WithEvents xrSubreport1 As DevExpress.XtraReports.UI.XRSubreport
    Private WithEvents xrPictureBox2 As DevExpress.XtraReports.UI.XRPictureBox
    Private WithEvents PageFooter As DevExpress.XtraReports.UI.PageFooterBand
    Private WithEvents xrPanel1 As DevExpress.XtraReports.UI.XRPanel
    Private WithEvents xrLabel8 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel7 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel6 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel5 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel4 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel3 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel2 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel9 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel10 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel11 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel14 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel13 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel12 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel17 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel16 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel15 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrPanel2 As DevExpress.XtraReports.UI.XRPanel
    Private WithEvents xrLabel18 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel1 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel22 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel21 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel20 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel19 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel23 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents xrLabel24 As DevExpress.XtraReports.UI.XRLabel
    Private WithEvents XrLabel25 As DevExpress.XtraReports.UI.XRLabel

    'Required by the Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Designer
    'It can be modified using the Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        'Dim resourceFileName As String = "XtraReport1.resx"
        'Dim resources As System.Resources.ResourceManager = Global.Resources.XtraReport1.ResourceManager
        Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
        Me.xrLabel22 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel21 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel20 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel19 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrPanel1 = New DevExpress.XtraReports.UI.XRPanel()
        Me.xrLabel17 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel16 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel15 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel14 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel13 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel12 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel11 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel10 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel8 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel7 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel6 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel5 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel4 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel3 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel9 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel23 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel24 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrSubreport1 = New DevExpress.XtraReports.UI.XRSubreport()
        Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.xrPictureBox2 = New DevExpress.XtraReports.UI.XRPictureBox()
        Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
        Me.PageFooter = New DevExpress.XtraReports.UI.PageFooterBand()
        Me.xrPanel2 = New DevExpress.XtraReports.UI.XRPanel()
        Me.xrLabel18 = New DevExpress.XtraReports.UI.XRLabel()
        Me.xrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.XrLabel25 = New DevExpress.XtraReports.UI.XRLabel()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'Detail
        '
        Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrLabel22, Me.xrLabel21, Me.xrLabel20, Me.xrLabel19, Me.xrPanel1, Me.xrSubreport1})
        Me.Detail.HeightF = 594.7917!
        Me.Detail.Name = "Detail"
        Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
        Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'xrLabel22
        '
        Me.xrLabel22.Font = New System.Drawing.Font("Calibri", 12.25!)
        Me.xrLabel22.LocationFloat = New DevExpress.Utils.PointFloat(485.0!, 11.24998!)
        Me.xrLabel22.Name = "xrLabel22"
        Me.xrLabel22.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel22.SizeF = New System.Drawing.SizeF(87.5!, 23.95833!)
        Me.xrLabel22.StylePriority.UseFont = False
        Me.xrLabel22.Text = "Amount"
        '
        'xrLabel21
        '
        Me.xrLabel21.Font = New System.Drawing.Font("Calibri", 12.25!)
        Me.xrLabel21.LocationFloat = New DevExpress.Utils.PointFloat(229.6666!, 11.24998!)
        Me.xrLabel21.Name = "xrLabel21"
        Me.xrLabel21.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel21.SizeF = New System.Drawing.SizeF(205.2083!, 23.95833!)
        Me.xrLabel21.StylePriority.UseFont = False
        Me.xrLabel21.Text = "Transaction Description"
        '
        'xrLabel20
        '
        Me.xrLabel20.Font = New System.Drawing.Font("Calibri", 12.25!)
        Me.xrLabel20.LocationFloat = New DevExpress.Utils.PointFloat(113.75!, 11.24998!)
        Me.xrLabel20.Name = "xrLabel20"
        Me.xrLabel20.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel20.SizeF = New System.Drawing.SizeF(103.9166!, 23.95833!)
        Me.xrLabel20.StylePriority.UseFont = False
        Me.xrLabel20.Text = "Ref Number"
        '
        'xrLabel19
        '
        Me.xrLabel19.Font = New System.Drawing.Font("Calibri", 12.25!)
        Me.xrLabel19.LocationFloat = New DevExpress.Utils.PointFloat(17.5!, 11.24998!)
        Me.xrLabel19.Name = "xrLabel19"
        Me.xrLabel19.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel19.SizeF = New System.Drawing.SizeF(87.5!, 23.95833!)
        Me.xrLabel19.StylePriority.UseFont = False
        Me.xrLabel19.Text = "Date"
        '
        'xrPanel1
        '
        Me.xrPanel1.BackColor = System.Drawing.Color.Black
        Me.xrPanel1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrLabel17, Me.xrLabel16, Me.xrLabel15, Me.xrLabel14, Me.xrLabel13, Me.xrLabel12, Me.xrLabel11, Me.xrLabel10, Me.xrLabel8, Me.xrLabel7, Me.xrLabel6, Me.xrLabel5, Me.xrLabel4, Me.xrLabel3, Me.xrLabel2, Me.xrLabel9, Me.xrLabel23, Me.xrLabel24})
        Me.xrPanel1.LocationFloat = New DevExpress.Utils.PointFloat(626.6667!, 4.999987!)
        Me.xrPanel1.Name = "xrPanel1"
        Me.xrPanel1.SizeF = New System.Drawing.SizeF(189.3333!, 582.0!)
        Me.xrPanel1.StylePriority.UseBackColor = False
        '
        'xrLabel17
        '
        Me.xrLabel17.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.TotalDue")})
        Me.xrLabel17.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel17.ForeColor = System.Drawing.Color.White
        Me.xrLabel17.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 542.9166!)
        Me.xrLabel17.Name = "xrLabel17"
        Me.xrLabel17.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel17.SizeF = New System.Drawing.SizeF(168.3333!, 23.0!)
        Me.xrLabel17.StylePriority.UseFont = False
        Me.xrLabel17.StylePriority.UseForeColor = False
        Me.xrLabel17.StylePriority.UseTextAlignment = False
        Me.xrLabel17.Text = "xrLabel17"
        Me.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel16
        '
        Me.xrLabel16.BackColor = System.Drawing.Color.Black
        Me.xrLabel16.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel16.ForeColor = System.Drawing.Color.White
        Me.xrLabel16.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 511.5833!)
        Me.xrLabel16.Name = "xrLabel16"
        Me.xrLabel16.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel16.SizeF = New System.Drawing.SizeF(168.3333!, 31.33331!)
        Me.xrLabel16.StylePriority.UseBackColor = False
        Me.xrLabel16.StylePriority.UseFont = False
        Me.xrLabel16.StylePriority.UseForeColor = False
        Me.xrLabel16.StylePriority.UseTextAlignment = False
        Me.xrLabel16.Text = "Total Due"
        Me.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel15
        '
        Me.xrLabel15.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.Overdue")})
        Me.xrLabel15.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel15.ForeColor = System.Drawing.Color.White
        Me.xrLabel15.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 483.5833!)
        Me.xrLabel15.Name = "xrLabel15"
        Me.xrLabel15.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel15.SizeF = New System.Drawing.SizeF(168.3333!, 23.0!)
        Me.xrLabel15.StylePriority.UseFont = False
        Me.xrLabel15.StylePriority.UseForeColor = False
        Me.xrLabel15.StylePriority.UseTextAlignment = False
        Me.xrLabel15.Text = "xrLabel15"
        Me.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel14
        '
        Me.xrLabel14.BackColor = System.Drawing.Color.Black
        Me.xrLabel14.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel14.ForeColor = System.Drawing.Color.White
        Me.xrLabel14.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 452.25!)
        Me.xrLabel14.Name = "xrLabel14"
        Me.xrLabel14.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel14.SizeF = New System.Drawing.SizeF(168.3333!, 31.33331!)
        Me.xrLabel14.StylePriority.UseBackColor = False
        Me.xrLabel14.StylePriority.UseFont = False
        Me.xrLabel14.StylePriority.UseForeColor = False
        Me.xrLabel14.StylePriority.UseTextAlignment = False
        Me.xrLabel14.Text = "Amount Overdue"
        Me.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel13
        '
        Me.xrLabel13.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.Current")})
        Me.xrLabel13.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel13.ForeColor = System.Drawing.Color.White
        Me.xrLabel13.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 423.8333!)
        Me.xrLabel13.Name = "xrLabel13"
        Me.xrLabel13.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel13.SizeF = New System.Drawing.SizeF(168.3333!, 23.0!)
        Me.xrLabel13.StylePriority.UseFont = False
        Me.xrLabel13.StylePriority.UseForeColor = False
        Me.xrLabel13.StylePriority.UseTextAlignment = False
        Me.xrLabel13.Text = "xrLabel13"
        Me.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel12
        '
        Me.xrLabel12.BackColor = System.Drawing.Color.Black
        Me.xrLabel12.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel12.ForeColor = System.Drawing.Color.White
        Me.xrLabel12.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 392.5!)
        Me.xrLabel12.Name = "xrLabel12"
        Me.xrLabel12.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel12.SizeF = New System.Drawing.SizeF(168.3333!, 31.33331!)
        Me.xrLabel12.StylePriority.UseBackColor = False
        Me.xrLabel12.StylePriority.UseFont = False
        Me.xrLabel12.StylePriority.UseForeColor = False
        Me.xrLabel12.StylePriority.UseTextAlignment = False
        Me.xrLabel12.Text = "Amount Due"
        Me.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel11
        '
        Me.xrLabel11.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.ClosingBalance")})
        Me.xrLabel11.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel11.ForeColor = System.Drawing.Color.White
        Me.xrLabel11.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 300.4583!)
        Me.xrLabel11.Name = "xrLabel11"
        Me.xrLabel11.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel11.SizeF = New System.Drawing.SizeF(168.3333!, 23.0!)
        Me.xrLabel11.StylePriority.UseFont = False
        Me.xrLabel11.StylePriority.UseForeColor = False
        Me.xrLabel11.StylePriority.UseTextAlignment = False
        Me.xrLabel11.Text = "xrLabel11"
        Me.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel10
        '
        Me.xrLabel10.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.OpeningBalance")})
        Me.xrLabel10.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel10.ForeColor = System.Drawing.Color.White
        Me.xrLabel10.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 240.0!)
        Me.xrLabel10.Name = "xrLabel10"
        Me.xrLabel10.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel10.SizeF = New System.Drawing.SizeF(168.3333!, 23.0!)
        Me.xrLabel10.StylePriority.UseFont = False
        Me.xrLabel10.StylePriority.UseForeColor = False
        Me.xrLabel10.StylePriority.UseTextAlignment = False
        Me.xrLabel10.Text = "xrLabel10"
        Me.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel8
        '
        Me.xrLabel8.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.CurrentDate")})
        Me.xrLabel8.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel8.ForeColor = System.Drawing.Color.White
        Me.xrLabel8.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 48.45834!)
        Me.xrLabel8.Name = "xrLabel8"
        Me.xrLabel8.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel8.SizeF = New System.Drawing.SizeF(168.3333!, 23.0!)
        Me.xrLabel8.StylePriority.UseFont = False
        Me.xrLabel8.StylePriority.UseForeColor = False
        Me.xrLabel8.StylePriority.UseTextAlignment = False
        Me.xrLabel8.Text = "xrLabel8"
        Me.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel7
        '
        Me.xrLabel7.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.AccountNumber")})
        Me.xrLabel7.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel7.ForeColor = System.Drawing.Color.White
        Me.xrLabel7.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 99.33334!)
        Me.xrLabel7.Name = "xrLabel7"
        Me.xrLabel7.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel7.SizeF = New System.Drawing.SizeF(168.3333!, 22.99999!)
        Me.xrLabel7.StylePriority.UseFont = False
        Me.xrLabel7.StylePriority.UseForeColor = False
        Me.xrLabel7.StylePriority.UseTextAlignment = False
        Me.xrLabel7.Text = "xrLabel7"
        Me.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel6
        '
        Me.xrLabel6.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.AccountName")})
        Me.xrLabel6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel6.ForeColor = System.Drawing.Color.White
        Me.xrLabel6.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 155.9584!)
        Me.xrLabel6.Multiline = True
        Me.xrLabel6.Name = "xrLabel6"
        Me.xrLabel6.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel6.SizeF = New System.Drawing.SizeF(168.3333!, 45.70827!)
        Me.xrLabel6.StylePriority.UseFont = False
        Me.xrLabel6.StylePriority.UseForeColor = False
        Me.xrLabel6.StylePriority.UseTextAlignment = False
        Me.xrLabel6.Text = "xrLabel6"
        Me.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel5
        '
        Me.xrLabel5.BackColor = System.Drawing.Color.Black
        Me.xrLabel5.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel5.ForeColor = System.Drawing.Color.White
        Me.xrLabel5.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 268.125!)
        Me.xrLabel5.Name = "xrLabel5"
        Me.xrLabel5.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel5.SizeF = New System.Drawing.SizeF(168.3333!, 31.33331!)
        Me.xrLabel5.StylePriority.UseBackColor = False
        Me.xrLabel5.StylePriority.UseFont = False
        Me.xrLabel5.StylePriority.UseForeColor = False
        Me.xrLabel5.StylePriority.UseTextAlignment = False
        Me.xrLabel5.Text = "Closing Balance"
        Me.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel4
        '
        Me.xrLabel4.BackColor = System.Drawing.Color.Black
        Me.xrLabel4.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel4.ForeColor = System.Drawing.Color.White
        Me.xrLabel4.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 207.6666!)
        Me.xrLabel4.Name = "xrLabel4"
        Me.xrLabel4.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel4.SizeF = New System.Drawing.SizeF(168.3333!, 31.3333!)
        Me.xrLabel4.StylePriority.UseBackColor = False
        Me.xrLabel4.StylePriority.UseFont = False
        Me.xrLabel4.StylePriority.UseForeColor = False
        Me.xrLabel4.StylePriority.UseTextAlignment = False
        Me.xrLabel4.Text = "Opening Balance"
        Me.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel3
        '
        Me.xrLabel3.BackColor = System.Drawing.Color.Black
        Me.xrLabel3.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel3.ForeColor = System.Drawing.Color.White
        Me.xrLabel3.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 128.5833!)
        Me.xrLabel3.Name = "xrLabel3"
        Me.xrLabel3.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel3.SizeF = New System.Drawing.SizeF(168.3333!, 23.0!)
        Me.xrLabel3.StylePriority.UseBackColor = False
        Me.xrLabel3.StylePriority.UseFont = False
        Me.xrLabel3.StylePriority.UseForeColor = False
        Me.xrLabel3.StylePriority.UseTextAlignment = False
        Me.xrLabel3.Text = "Account Name"
        Me.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel2
        '
        Me.xrLabel2.BackColor = System.Drawing.Color.Black
        Me.xrLabel2.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel2.ForeColor = System.Drawing.Color.White
        Me.xrLabel2.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 76.33333!)
        Me.xrLabel2.Name = "xrLabel2"
        Me.xrLabel2.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel2.SizeF = New System.Drawing.SizeF(168.3333!, 23.0!)
        Me.xrLabel2.StylePriority.UseBackColor = False
        Me.xrLabel2.StylePriority.UseFont = False
        Me.xrLabel2.StylePriority.UseForeColor = False
        Me.xrLabel2.StylePriority.UseTextAlignment = False
        Me.xrLabel2.Text = "Account Number"
        Me.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel9
        '
        Me.xrLabel9.BackColor = System.Drawing.Color.Black
        Me.xrLabel9.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel9.ForeColor = System.Drawing.Color.White
        Me.xrLabel9.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 25.45832!)
        Me.xrLabel9.Name = "xrLabel9"
        Me.xrLabel9.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel9.SizeF = New System.Drawing.SizeF(168.3333!, 23.00001!)
        Me.xrLabel9.StylePriority.UseBackColor = False
        Me.xrLabel9.StylePriority.UseFont = False
        Me.xrLabel9.StylePriority.UseForeColor = False
        Me.xrLabel9.StylePriority.UseTextAlignment = False
        Me.xrLabel9.Text = "Today's Date"
        Me.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel23
        '
        Me.xrLabel23.BackColor = System.Drawing.Color.Black
        Me.xrLabel23.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel23.ForeColor = System.Drawing.Color.White
        Me.xrLabel23.LocationFloat = New DevExpress.Utils.PointFloat(10.0!, 328.4583!)
        Me.xrLabel23.Name = "xrLabel23"
        Me.xrLabel23.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel23.SizeF = New System.Drawing.SizeF(168.3333!, 31.3333!)
        Me.xrLabel23.StylePriority.UseBackColor = False
        Me.xrLabel23.StylePriority.UseFont = False
        Me.xrLabel23.StylePriority.UseForeColor = False
        Me.xrLabel23.StylePriority.UseTextAlignment = False
        Me.xrLabel23.Text = "Credit Limit"
        Me.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrLabel24
        '
        Me.xrLabel24.DataBindings.AddRange(New DevExpress.XtraReports.UI.XRBinding() {New DevExpress.XtraReports.UI.XRBinding("Text", Nothing, "Accounts.CreditLimit")})
        Me.xrLabel24.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel24.ForeColor = System.Drawing.Color.White
        Me.xrLabel24.LocationFloat = New DevExpress.Utils.PointFloat(9.999962!, 359.7916!)
        Me.xrLabel24.Name = "xrLabel24"
        Me.xrLabel24.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel24.SizeF = New System.Drawing.SizeF(168.3334!, 23.0!)
        Me.xrLabel24.StylePriority.UseFont = False
        Me.xrLabel24.StylePriority.UseForeColor = False
        Me.xrLabel24.StylePriority.UseTextAlignment = False
        Me.xrLabel24.Text = "xrLabel24"
        Me.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        '
        'xrSubreport1
        '
        Me.xrSubreport1.LocationFloat = New DevExpress.Utils.PointFloat(9.999998!, 39.20832!)
        Me.xrSubreport1.Name = "xrSubreport1"
        Me.xrSubreport1.ReportSource = New repDetails()
        Me.xrSubreport1.SizeF = New System.Drawing.SizeF(602.0833!, 49.49999!)
        '
        'TopMargin
        '
        Me.TopMargin.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrPictureBox2})
        Me.TopMargin.HeightF = 446.875!
        Me.TopMargin.Name = "TopMargin"
        Me.TopMargin.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
        Me.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'xrPictureBox2
        '
        'Me.xrPictureBox2.Image = CType(resources.GetObject("xrPictureBox2.Image"), System.Drawing.Image)
        Me.xrPictureBox2.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter
        Me.xrPictureBox2.LocationFloat = New DevExpress.Utils.PointFloat(4.999979!, 0!)
        Me.xrPictureBox2.Name = "xrPictureBox2"
        Me.xrPictureBox2.SizeF = New System.Drawing.SizeF(816.0!, 442.0833!)
        Me.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
        '
        'BottomMargin
        '
        Me.BottomMargin.HeightF = 0!
        Me.BottomMargin.Name = "BottomMargin"
        Me.BottomMargin.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
        Me.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'PageFooter
        '
        Me.PageFooter.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrPanel2})
        Me.PageFooter.HeightF = 76.45836!
        Me.PageFooter.Name = "PageFooter"
        '
        'xrPanel2
        '
        Me.xrPanel2.BackColor = System.Drawing.Color.Black
        Me.xrPanel2.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrLabel25, Me.xrLabel18, Me.xrLabel1})
        Me.xrPanel2.LocationFloat = New DevExpress.Utils.PointFloat(12.5!, 5.208333!)
        Me.xrPanel2.Name = "xrPanel2"
        Me.xrPanel2.SizeF = New System.Drawing.SizeF(797.9167!, 61.25003!)
        Me.xrPanel2.StylePriority.UseBackColor = False
        '
        'xrLabel18
        '
        Me.xrLabel18.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel18.ForeColor = System.Drawing.Color.White
        Me.xrLabel18.LocationFloat = New DevExpress.Utils.PointFloat(9.999998!, 3.125!)
        Me.xrLabel18.Name = "xrLabel18"
        Me.xrLabel18.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel18.SizeF = New System.Drawing.SizeF(309.375!, 20.20834!)
        Me.xrLabel18.StylePriority.UseFont = False
        Me.xrLabel18.StylePriority.UseForeColor = False
        Me.xrLabel18.Text = "Rage SA | www.ragesa.co.za"
        '
        'xrLabel1
        '
        Me.xrLabel1.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xrLabel1.ForeColor = System.Drawing.Color.White
        Me.xrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(417.25!, 3.125!)
        Me.xrLabel1.Name = "xrLabel1"
        Me.xrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.xrLabel1.SizeF = New System.Drawing.SizeF(365.625!, 20.20834!)
        Me.xrLabel1.StylePriority.UseFont = False
        Me.xrLabel1.StylePriority.UseForeColor = False
        Me.xrLabel1.Text = "For any account queries please call 011 608 6800"
        '
        'XrLabel25
        '
        Me.XrLabel25.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.XrLabel25.ForeColor = System.Drawing.Color.White
        Me.XrLabel25.LocationFloat = New DevExpress.Utils.PointFloat(9.999998!, 31.04172!)
        Me.XrLabel25.Name = "XrLabel25"
        Me.XrLabel25.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
        Me.XrLabel25.SizeF = New System.Drawing.SizeF(747.9166!, 20.20834!)
        Me.XrLabel25.StylePriority.UseFont = False
        Me.XrLabel25.StylePriority.UseForeColor = False
        Me.XrLabel25.Text = "Banking Details: Rage Distribution | ABSA | Account Number: 4081269231 | Branch C" &
    "ode:  632005"
        '
        'rep_test
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail, Me.TopMargin, Me.BottomMargin, Me.PageFooter})
        Me.Margins = New System.Drawing.Printing.Margins(19, 10, 447, 0)
        Me.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic
        Me.Version = "15.1"
        Me.XmlDataPath = "F:\Website\pcm\Data\data.xml"
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents TopMargin As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMargin As DevExpress.XtraReports.UI.BottomMarginBand

#End Region

End Class