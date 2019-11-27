Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities

Public Class CashCards1
    Inherits System.Web.UI.Page

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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.CashCardSummary) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("current_company") = "010"
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
        End If
    End Sub

    Private Sub CashCards_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        dxGrid.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        'Dim _BLayer As New ReportsPositiveCashCardsBL(Session("current_company"))

        Dim data As DataTable = GetData()

        gridView.DataSource = data

        dxGrid.EndUpdate()

    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click

        Session.Remove("data")

        dxGrid.DataBind()

    End Sub

    Private Function GetData() As DataTable

        If Not IsNothing(Session("data")) Then
            Return Session("data")
        End If

        Dim _BLayer As New ReportsPositiveCashCardsBL(Session("current_company"))

        Session("data") = _BLayer.ReturnCashCard(txtFromDate.Text, txtToDate.Text, chkAllAccounts.Checked)

        Return Session("data")

    End Function

    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click

        Dim pageKeys As List(Of Object) = dxGrid.GetCurrentPageRowValues(dxGrid.KeyFieldName)

        If pageKeys IsNot Nothing Then
            For Each key As Object In pageKeys
                Exporter.GridView.Selection.SetSelectionByKey(key, True)
            Next
        End If

        Exporter.WritePdfToResponse()

    End Sub

End Class