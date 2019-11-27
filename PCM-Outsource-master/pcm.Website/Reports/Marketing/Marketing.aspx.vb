Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports OfficeOpenXml.Style
Imports OfficeOpenXml

Public Class Marketing
    Inherits System.Web.UI.Page

    Dim _ReportBL As PCMReportingBusinessLayer = New PCMReportingBusinessLayer
    Dim _Logging As UsersBL = New UsersBL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Increase page timeout to 20 minutes
        Page.Server.ScriptTimeout = 600

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.MarketingExport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("username") = "DANIEL"
        End If

        If Not IsPostBack Then
            Dim _BLayer As New GeneralHOBL()
            Dim _dt As DataTable

            _dt = _BLayer.GetBranches

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboBranch.Items.Add(_dt(i)("branch_code") & " - " & _dt(i)("branch_name"))
            Next
        End If


    End Sub

    Protected Sub cmdRunEMailAddresses_Click(sender As Object, e As EventArgs) Handles cmdRunEMailAddresses.Click

        Dim _csv As String

        _csv = _ReportBL.GetEMailAddresses

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run EMail Addresses"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = ""
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Marketing Exports"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Try
            'Dim pck As ExcelPackage = New ExcelPackage
            ''Load the datatable into the sheet, starting from cell A1. 
            ''Print the column names on row 1
            'Dim ws As ExcelWorksheet = pck.Workbook.Worksheets.Add("Demo")
            'ws.Cells("A1").LoadFromDataTable(_csv, True)
            ''Write it back to the client
            'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.AddHeader("content-disposition", "attachment;  filename=EMailAddresses.xlsx")
            'Response.BinaryWrite(pck.GetAsByteArray)
            Dim tFile As String

            Using reader As IO.StreamReader = New IO.StreamReader(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
                ' Read one line from file
                tFile = reader.ReadToEnd
            End Using

            If IO.File.Exists(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv")) Then
                IO.File.Delete(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
            End If

            If _csv.Length > 0 Then
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=email_addresses.csv")
                Response.Charset = ""
                Response.ContentType = "application/text"
                Response.Output.Write(tFile)
                Response.Flush()
                Response.End()
            End If
        Catch ex As Exception
            lblError.Text = "Export Failed"
            dxPopUpError.ShowOnPageLoad = True
        End Try

    End Sub

    Private Sub Marketing_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub cmdCellphoneNumbers_Click(sender As Object, e As EventArgs) Handles cmdCellphoneNumbers.Click

        Dim _csv As String

        _csv = _ReportBL.GetCellphoneNumbers

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Cellphone Numbers"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = ""
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Marketing Exports"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Try
            'Dim pck As ExcelPackage = New ExcelPackage
            ''Load the datatable into the sheet, starting from cell A1. 
            ''Print the column names on row 1
            'Dim ws As ExcelWorksheet = pck.Workbook.Worksheets.Add("Demo")
            'ws.Cells("A1").LoadFromDataTable(_csv, True)
            ''Write it back to the client
            'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.AddHeader("content-disposition", "attachment;  filename=CellphoneNumbers.xlsx")
            'Response.BinaryWrite(pck.GetAsByteArray)
            Dim tFile As String

            Using reader As IO.StreamReader = New IO.StreamReader(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
                ' Read one line from file
                tFile = reader.ReadToEnd
            End Using

            If IO.File.Exists(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv")) Then
                IO.File.Delete(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
            End If

            If _csv.Length > 0 Then
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=CellphoneNumbers.csv")
                Response.Charset = ""
                Response.ContentType = "application/text"
                Response.Output.Write(tFile)
                Response.Flush()
                Response.End()
            End If

        Catch ex As Exception
            lblError.Text = "Export Failed"
            dxPopUpError.ShowOnPageLoad = True
        End Try
    End Sub

    Protected Sub cmdCellphoneNumbersAll_Click(sender As Object, e As EventArgs) Handles cmdCellphoneNumbersAll.Click

        Dim _csv As String

        _csv = _ReportBL.GetAllCellphoneNumbers

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run All Cellphone Numbers"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = ""
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Marketing Exports"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Try
            'Dim pck As ExcelPackage = New ExcelPackage

            ''Load the datatable into the sheet, starting from cell A1. 
            ''Print the column names on row 1
            'Dim ws As ExcelWorksheet = pck.Workbook.Worksheets.Add("Demo")
            'ws.Cells("A1").LoadFromDataTable(_csv, True)
            ''Write it back to the client
            'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.AddHeader("content-disposition", "attachment;  filename=AllCellphoneNumbers.xlsx")
            'Response.BinaryWrite(pck.GetAsByteArray)
            Dim tFile As String

            Using reader As IO.StreamReader = New IO.StreamReader(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
                ' Read one line from file
                tFile = reader.ReadToEnd
            End Using

            If IO.File.Exists(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv")) Then
                IO.File.Delete(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
            End If

            If _csv.Length > 0 Then
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=CellphoneNumbersAll.csv")
                Response.Charset = ""
                Response.ContentType = "application/text"
                Response.Output.Write(tFile)
                Response.Flush()
                Response.End()
            End If

        Catch ex As Exception
            lblError.Text = "Export Failed"
            dxPopUpError.ShowOnPageLoad = True
        End Try
    End Sub

    Protected Sub cmdCellphoneByBranch_Click(sender As Object, e As EventArgs) Handles cmdCellphoneByBranch.Click

        Dim _csv As String

        _csv = _ReportBL.GetCellphonesByBranch(Mid(cboBranch.Text, 1, 3))

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Cellphone Numbers By Branch"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = ""
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Marketing Exports"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================


        Try
            'Dim pck As ExcelPackage = New ExcelPackage
            ''Load the datatable into the sheet, starting from cell A1. 
            ''Print the column names on row 1
            'Dim ws As ExcelWorksheet = pck.Workbook.Worksheets.Add("Demo")
            'ws.Cells("A1").LoadFromDataTable(_csv, True)
            ''Write it back to the client
            'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            'Response.AddHeader("content-disposition", "attachment;  filename=AllCellphoneNumbers.xlsx")
            'Response.BinaryWrite(pck.GetAsByteArray)
            Dim tFile As String

            Using reader As IO.StreamReader = New IO.StreamReader(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
                ' Read one line from file
                tFile = reader.ReadToEnd
            End Using

            If IO.File.Exists(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv")) Then
                IO.File.Delete(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
            End If

            If _csv.Length > 0 Then
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=GetCellphonesByBranch.csv")
                Response.Charset = ""
                Response.ContentType = "application/text"
                Response.Output.Write(tFile)
                Response.Flush()
                Response.End()
            End If



        Catch ex As Exception
            lblError.Text = "Export Failed"
            dxPopUpError.ShowOnPageLoad = True
        End Try
    End Sub

    Protected Sub cmdCellphoneNumbersLoyalty_Click(sender As Object, e As EventArgs) Handles cmdCellphoneNumbersLoyalty.Click
        Dim _csv As String

        _csv = _ReportBL.GetCellphoneNumbersByLoayaltyAccounts

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Cellphone Numbers By Loyalty Accounts"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = ""
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Marketing Exports"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        Try

            Dim tFile As String

            Using reader As IO.StreamReader = New IO.StreamReader(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
                ' Read one line from file
                tFile = reader.ReadToEnd
            End Using

            If IO.File.Exists(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv")) Then
                IO.File.Delete(HttpContext.Current.Server.MapPath("~\Docs\" & _csv & ".csv"))
            End If

            If _csv.Length > 0 Then
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=CellphoneNumbersByLoyaltyAccounts.csv")
                Response.Charset = ""
                Response.ContentType = "application/text"
                Response.Output.Write(tFile)
                Response.Flush()
                Response.End()
            End If

        Catch ex As Exception
            lblError.Text = "Export Failed"
            dxPopUpError.ShowOnPageLoad = True
        End Try
    End Sub
End Class