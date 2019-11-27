Public Class test
    Inherits System.Web.UI.UserControl

    Protected Function GetSubElementId(ByVal subId As String) As String
        Return Me.ClientID + "_" + subId
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub txtZubi_TextChanged(sender As Object, e As EventArgs) Handles txtZubi.TextChanged

    End Sub
End Class