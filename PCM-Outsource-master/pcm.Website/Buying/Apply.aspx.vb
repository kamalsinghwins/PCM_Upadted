Imports pcm.BusinessLayer

Public Class Apply
    Inherits System.Web.UI.Page

    Dim _BLayer As BuyingBL = New BuyingBL()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            cboProvince.Items.Add("")
            cboProvince.Items.Add("Eastern Cape")
            cboProvince.Items.Add("Free State")
            cboProvince.Items.Add("Gauteng")
            cboProvince.Items.Add("KZN")
            cboProvince.Items.Add("Limpopo")
            cboProvince.Items.Add("Mpumalanga")
            cboProvince.Items.Add("North West")
            cboProvince.Items.Add("Northern Cape")
            cboProvince.Items.Add("Western Cape")
        End If

    End Sub

    Protected Sub SaveItmButton_Click(sender As Object, e As EventArgs) Handles SaveItmButton.Click

        'Dim ReturnString As String

        'ReturnString = _BLayer.InsertApplication(txtFirstName.Text, txtLastName.Text, txtIDNumber.Text, txtEmail.Text, txtContactNumber.Text,
        '                                         cboProvince.Text, txtKids.Text)

        'If ReturnString = "Success" Then
        '    hdCompleted.Value = "true"
        'Else
        '    hdError.Value = "true"
        'End If

    End Sub
End Class