Public Class statement

    Public Sub New()
        MyBase.New()

        'This call is required by the Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Sub SetSource(ByVal Source As String)
        Dim xrep As statement_details = DirectCast(Me.xrSubreport1.ReportSource, statement_details)

        xrep.SetSource(Source)

    End Sub

End Class