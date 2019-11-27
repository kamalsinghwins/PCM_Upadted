Imports pcm.DataLayer

Public Class ReportsPositiveWebServiceLogsBL

    Private ReadOnly _dLayer As ReportsPositiveWebServiceLogsDL

    'Public Sub New(ByVal CompanyCode As String)
    '    _dLayer = New ReportsPositiveWebServiceLogsDL(CompanyCode)
    'End Sub

    Public Sub New()
        _dLayer = New ReportsPositiveWebServiceLogsDL
    End Sub

    Public Function GetWebServiceLogs(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _dLayer.GetWebServiceLogs(StartDate, EndDate)

    End Function
End Class
