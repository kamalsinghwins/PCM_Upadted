Imports Entities
Imports pcm.BusinessLayer

Public Class CallsForToday
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RefreshCalls()
    End Sub

    Public Sub RefreshCalls()
        Dim _blWidget As New WidgetBusinessLayer

        Dim _NewData As New CallWidgets

        _NewData = _blWidget.GetCallsForToday(Session("username"))

        If Not IsNothing(_NewData) Then
            lblCalls.Text = _NewData.CallsForToday
            lblPTP.Text = _NewData.PTPsForToday
            lblPTPAmount.Text = _NewData.AmountOfPTPs
        End If

    End Sub

    Protected Sub ASPxTimer1_Tick(sender As Object, e As EventArgs) Handles ASPxTimer1.Tick
        RefreshCalls()
    End Sub
End Class