Public Class statement_details
    Inherits DevExpress.XtraReports.UI.XtraReport

    Private _Source As String

    Public Sub New()
        MyBase.New()

        'This call is required by the Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Private Sub Detail_BeforePrint(sender As Object, e As System.Drawing.Printing.PrintEventArgs) Handles Me.BeforePrint
        'Delete the file
        System.IO.File.Delete(_Source)
    End Sub

    Public Sub SetSource(ByVal tmpSource As String)
        _Source = tmpSource
        XmlDataPath = _Source
    End Sub

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub


End Class