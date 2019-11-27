Imports System
Imports System.Data
Imports DevExpress.XtraReports.UI
Imports System.Drawing.Printing
Imports pcm.Website.UTIL
Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class ViewStatement
    Inherits System.Web.UI.Page

    'Dim _npg As clsNPDatabase
    Dim _ds As DataSet
    Dim _dsInner As DataSet
    Dim _tmpSql As String
    Dim _util As New clsUtil
    Dim _hasData As Boolean = False
    Dim _tmpGuId As String
    Dim _r As New statement

    Shared Sub New()
        DevExpress.XtraReports.UI.ExternalFileAccessSecurityLevelSettings.SecurityLevel = ExternalFileAccessSecurityLevel.Unrestricted
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        '' Create a DataSet and fill it with data from an XML file.
        'Dim xmlDataSet As New DataSet()
        'xmlDataSet.ReadXml("F:\Website\pcm\Data\data.xml")

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("account_number") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Default.aspx")
                End If
            End If
            'Else
            '   Session("account_number") = "100000068"
            '   Session("account_number") = "100000068"
        End If

        Dim _BLayer As New StatementBL

        Dim tmpGuId As String = _BLayer.PrintStatement(Session("account_number"))
        Dim xmlFilePath As String = HttpContext.Current.Server.MapPath("data\" & tmpGuId & ".XML")
        _r.SetSource(xmlFilePath)
        _r.XmlDataPath = xmlFilePath
        repStatementsViewerNew.OpenReport(_r)

    End Sub


End Class

