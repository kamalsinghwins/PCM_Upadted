
Imports Entities
Imports Entities.IBT_Out
Imports Newtonsoft.Json
Imports pcm.BusinessLayer
Public Class IBT_In
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL

    Public Shared Current_Branch_Code As String = "HHH"   'Branch from Login
    Public Branch_Name As String
    Public Current_Company As String
    Public Current_User As String
    Public Branch_Telephone_Number As String
    Public Branch_Fax_Number As String
    Public Current_Company_Code As String

    Public Shared _IBT_Out As IBT_OutHOBL = New IBT_OutHOBL




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then

                Response.Redirect("~/Intranet/Default.aspx")
            Else
                If Not CheckScreenAccess.CheckAccess(Session("processing_permission_sequence"), Screens.Processing.IBTIN) Then
                    Response.Redirect("~/Intranet/Welcome.aspx")
                End If
            End If

        Else
            Session("username") = "DANIEL"
            Session("is_pcm_admin") = True
        End If

        Try
            GetCompanySettings()
            GetBranchSettings(Current_Branch_Code)
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub


    Private Sub IBT_In_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Public Function GetCompanySettings() As Boolean
        Dim getCompanyResponse As New GetCompanyResponse
        Current_User = Session("username")
        getCompanyResponse = _IBT_Out.GetCompanySettings()
        If getCompanyResponse.Success = True Then
            Current_Company = getCompanyResponse.dt.Rows(0)("company_name")
            Current_Company_Code = getCompanyResponse.dt.Rows(0)("company_code")

        End If



    End Function

    Public Function GetBranchSettings(ByVal BranchCode As String) As Boolean
        Dim getBranchSettingsResponse As New GetBranchSettingsResponse
        getBranchSettingsResponse = _IBT_Out.GetBranchSettings(Current_Branch_Code)

        If getBranchSettingsResponse.Success = True Then
            Branch_Name = getBranchSettingsResponse.dt.Rows(0)("branch_name") & ""
            Branch_Fax_Number = getBranchSettingsResponse.dt.Rows(0)("fax_number") & ""
            Branch_Telephone_Number = getBranchSettingsResponse.dt.Rows(0)("telephone_number") & ""

        End If


    End Function
End Class