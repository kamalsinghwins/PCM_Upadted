Imports System.Net
Imports Entities
Imports Entities.IBT_In
Imports Entities.IBT_Out
Imports Entities.Manage
Imports Entities.SizeMatrix
Imports Entities.DispatchStock
Imports Entities.Reports
Imports Newtonsoft.Json
Imports pcm.BusinessLayer
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports Entities.Stock

Public Class AjaxServer
    Inherits System.Web.UI.Page

    Private sUserId As String
    Private _SendContent As Boolean
    Private Page As String
    Private Action As String
    Private Message As String
    Private _IBT_Out As IBT_OutHOBL = New IBT_OutHOBL
    Private _IBT_In As IBT_InHOBL = New IBT_InHOBL
    Private _DispatchBL As DispatchBL = New DispatchBL
    Private _Stock As StockHOBL = New StockHOBL
    Private _Report As ReportsBusinessLayer = New ReportsBusinessLayer
    Private _GRV As GRVHOBL = New GRVHOBL

    Dim req As New WebClient
    Private _Manage As ManageHOBL = New ManageHOBL
    Private _JsonResponse As New JsonResponse



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _JsonResponse.Success = False
        'If (Session("UserContext") == null || !HttpContext.Current.User.Identity.IsAuthenticated)

        '    Response.Clear()
        '    Response.Write(Message)
        '    Response.ContentType = "Text/Json"
        '    Response.End()
        '    _SendContent = True
        '    Exit Sub

        'End If


        sUserId = String.Empty
        _SendContent = False
        Page = String.Empty
        Action = String.Empty
        Message = String.Empty


        If String.IsNullOrEmpty(Request.QueryString("Action")) = False Then
            Action = Request.QueryString("Action").ToString()
        End If

        If Request.QueryString("Page") IsNot Nothing Then
            Page = Request.QueryString("Page").ToString()

            Dim RequsestFormData = Request.Form("formData")

            Select Case Page
                Case "IBT"
                    Select Case Action
                        Case "GetBranch"

                            Dim branchRequest = JsonConvert.DeserializeObject(Of GetBranch)(RequsestFormData)
                            Dim branchResult = _IBT_Out.SearchBranch(branchRequest.SearchType, branchRequest.SearchDetail)

                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetItems"
                            Dim itemRequest = JsonConvert.DeserializeObject(Of GetItems)(RequsestFormData)
                            Dim itemResult = _IBT_Out.Searchicibtout(itemRequest.SearchText, itemRequest.SearchType, itemRequest.Master)

                            Message = JsonConvert.SerializeObject(itemResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True
                        Case "GetItemCodeDetails"
                            Dim itemCodeDetails = JsonConvert.DeserializeObject(Of ItemCode)(RequsestFormData)
                            Dim itemResult = _IBT_Out.GetGeneratedCodeInfo(itemCodeDetails.ItemCode, itemCodeDetails.BranchCode)

                            Message = JsonConvert.SerializeObject(itemResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetBranchDetails"
                            Dim branchDetails = JsonConvert.DeserializeObject(Of BranchDetail)(RequsestFormData)
                            Dim branchResult = _IBT_Out.GetBranchDetails(branchDetails.BranchCode)

                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "Save"
                            Dim saveDetails = JsonConvert.DeserializeObject(Of Save)(RequsestFormData)
                            Dim saveResult = _IBT_Out.Save(saveDetails)

                            Message = JsonConvert.SerializeObject(saveResult)

                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True


                        Case "DownloadLabel"

                            Dim path As String = HttpContext.Current.Server.MapPath("~\temp\" & RequsestFormData & "")
                            Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
                            If file.Exists Then
                                Response.Clear()
                                Dim data As Byte() = req.DownloadData(path)
                                Response.BinaryWrite(data)
                                'Response.ContentType = "Text/Json"
                                Response.ContentType = "application/octet-stream"

                                Response.End()

                            Else
                                Response.Write("This file does not exist.")
                            End If



                            _SendContent = True


                        Case "Reload"
                            Dim user As String = Session("username")
                            Dim reload = _IBT_Out.Reload(user)

                            Message = JsonConvert.SerializeObject(reload)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "GetCodes"
                            Dim getCode = JsonConvert.DeserializeObject(Of GetCodes)(RequsestFormData)
                            Dim codes = _IBT_Out.GetCodes(getCode)

                            Message = JsonConvert.SerializeObject(codes)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "DeleteFile"
                            Dim deletefiles = JsonConvert.DeserializeObject(Of DeleteFiles)(RequsestFormData)
                            _IBT_Out.DeleteFile(deletefiles)

                            'Message = JsonConvert.SerializeObject(barcodes)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                    End Select

                Case "IBT_In"
                    Select Case Action
                        Case "FetchDetails"
                            Dim getDetails = JsonConvert.DeserializeObject(Of FetchDetails)(RequsestFormData)
                            Dim details = _IBT_In.FetchDetails(getDetails)

                            Message = JsonConvert.SerializeObject(details)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "Save"
                            Dim saveDetails = JsonConvert.DeserializeObject(Of SaveIBTIN)(RequsestFormData)
                            Dim saveResult = _IBT_In.Save(saveDetails)

                            Message = JsonConvert.SerializeObject(saveResult)

                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "DeleteFile"
                            Dim deletefiles = JsonConvert.DeserializeObject(Of DeletePrintFile)(RequsestFormData)
                            _IBT_In.DeleteFile(deletefiles)

                            'Message = JsonConvert.SerializeObject(barcodes)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                    End Select

                Case "SizeMatrix"
                    Select Case Action
                        Case "GetGrid"
                            Dim getDetails = JsonConvert.DeserializeObject(Of FetchGrids)(RequsestFormData)
                            Dim details = _Manage.GetGrids(getDetails.SearchType, getDetails.SearchDetail)

                            Message = JsonConvert.SerializeObject(details)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "GetGridDetails"
                            Dim getDetails = JsonConvert.DeserializeObject(Of GridDetail)(RequsestFormData)
                            Dim saveResult = _Manage.GetGrids(getDetails.GridNumber)

                            Message = JsonConvert.SerializeObject(saveResult)

                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "Save"
                            Dim saveRequest = JsonConvert.DeserializeObject(Of SaveGrid)(RequsestFormData)
                            Dim saveResponse = _Manage.SaveSizeGrid(saveRequest)
                            Message = JsonConvert.SerializeObject(saveResponse)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                    End Select

                Case "Specials"
                    Select Case Action
                        Case "GetSpecial"
                            Dim getDetails = JsonConvert.DeserializeObject(Of GetSpecialDetails)(RequsestFormData)
                            Dim details = _Manage.SearchSpecial(getDetails.SpecialName)

                            Message = JsonConvert.SerializeObject(details)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "GetSelectedSpecialDetails"
                            Dim getDetails = JsonConvert.DeserializeObject(Of GetSpecialDetails)(RequsestFormData)
                            Dim saveResult = _Manage.Populate(getDetails.SpecialName)

                            Message = JsonConvert.SerializeObject(saveResult)

                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "GetItems"
                            Dim getDetails = JsonConvert.DeserializeObject(Of GetSpecialItems)(RequsestFormData)
                            Dim saveResult = _Manage.SearchCode(getDetails.SearchType, getDetails.SearchText, getDetails.Master)
                            Message = JsonConvert.SerializeObject(saveResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "AddStock"
                            Dim getDetails = JsonConvert.DeserializeObject(Of StockRequest)(RequsestFormData)
                            Dim saveResult = _Manage.AddStock(getDetails.Stockcode)
                            Message = JsonConvert.SerializeObject(saveResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "Save"
                            Dim saveRequest = JsonConvert.DeserializeObject(Of SaveSpecial)(RequsestFormData)
                            Dim saveResult = _Manage.SaveSpecials(saveRequest)
                            Message = JsonConvert.SerializeObject(saveResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                    End Select

                Case "DispatchStock"
                    Select Case Action
                        Case "GetStockDetails"
                            Dim getDetails = JsonConvert.DeserializeObject(Of GetStockDetails)(RequsestFormData)
                            Dim details = _DispatchBL.ReturnBranchDetails("010", getDetails.IBTOutNumber, True)

                            Message = JsonConvert.SerializeObject(details)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True


                        Case "DispatchIBT"
                            Dim saveDetails = JsonConvert.DeserializeObject(Of DispatchStockCode)(RequsestFormData)
                            Dim dt As New DataTable()
                            dt.Columns.Add("IBTOutNumber")
                            dt.Columns.Add("BranchCode")
                            dt.Columns.Add("BranchName")
                            For Each field In saveDetails.ListData
                                dt.Rows.Add(field.IBTOutNumber, field.BranchCode, field.BranchName)
                            Next
                            Dim details = _DispatchBL.DispatchIBT("010", saveDetails.Driver, saveDetails.Registration, saveDetails.KM, dt)

                            Message = JsonConvert.SerializeObject(details)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                    End Select

                Case "StockAdjustments"
                    Select Case Action
                        Case "GetBranch"

                            Dim branchRequest = JsonConvert.DeserializeObject(Of GetBranch)(RequsestFormData)
                            Dim branchResult = _IBT_Out.SearchBranch(branchRequest.SearchType, branchRequest.SearchDetail)

                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "GetBranchDetails"
                            Dim branchDetails = JsonConvert.DeserializeObject(Of BranchDetail)(RequsestFormData)
                            Dim branchResult = _IBT_Out.GetBranchDetails(branchDetails.BranchCode)

                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetItems"
                            Dim getDetails = JsonConvert.DeserializeObject(Of GetSpecialItems)(RequsestFormData)
                            Dim saveResult = _Manage.SearchCode(getDetails.SearchType, getDetails.SearchText, getDetails.Master)
                            Message = JsonConvert.SerializeObject(saveResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "GetItemCodeDetails"
                            Dim itemCodeDetails = JsonConvert.DeserializeObject(Of ItemCode)(RequsestFormData)
                            Dim itemResult = _Stock.GetGeneratedCodeInfo(itemCodeDetails.ItemCode, itemCodeDetails.BranchCode)

                            Message = JsonConvert.SerializeObject(itemResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetCodes"
                            Dim getCode = JsonConvert.DeserializeObject(Of GetCodesRequest)(RequsestFormData)
                            Dim codes = _Stock.GetCodes(getCode)

                            Message = JsonConvert.SerializeObject(codes)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "AddStock"
                            Dim getDetails = JsonConvert.DeserializeObject(Of StockRequest)(RequsestFormData)
                            Dim saveResult = _Manage.AddStock(getDetails.Stockcode)
                            Message = JsonConvert.SerializeObject(saveResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "Save"
                            Dim saveRequest = JsonConvert.DeserializeObject(Of SaveStock)(RequsestFormData)
                            Dim saveResult = _Stock.Save(saveRequest)
                            Message = JsonConvert.SerializeObject(saveResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "DeleteFile"
                            Dim deletefile = JsonConvert.DeserializeObject(Of DeleteStockAdjustmentFile)(RequsestFormData)
                            _Stock.DeleteFile(deletefile)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                    End Select

                Case "ColourGrids"
                    Select Case Action
                        Case "GetBranch"
                            Dim branchResult = _Report.GetALLBranches()
                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "Search"
                            Dim request = JsonConvert.DeserializeObject(Of SearchStockcode)(RequsestFormData)
                            Dim getResult = _Report.GetAllStockCodes(request.SearchText, True)
                            Message = JsonConvert.SerializeObject(getResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "Save"
                            Dim request = JsonConvert.DeserializeObject(Of ColourGridRequest)(RequsestFormData)
                            Dim getResult = _Report.RunColourGridsReport(request)
                            Message = JsonConvert.SerializeObject(getResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True
                    End Select

                Case "GRV"
                    Select Case Action
                        Case "GetSupplier"
                            Dim request = JsonConvert.DeserializeObject(Of SupplierRequest)(RequsestFormData)
                            Dim branchResult = _GRV.SearchSupplier(request.SearchType, request.SearchText)
                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "SupplierDetails"
                            Dim request = JsonConvert.DeserializeObject(Of SupplierDetails)(RequsestFormData)
                            Dim branchResult = _GRV.GetSupplierDetails(request.SupplierCode)
                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetGeneratedCodes"
                            Dim request = JsonConvert.DeserializeObject(Of GetGeneratedcodes)(RequsestFormData)
                            Dim getResult = _GRV.GetGeneratedCode(request.SearchText)
                            Message = JsonConvert.SerializeObject(getResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetCodes"
                            Dim request = JsonConvert.DeserializeObject(Of GetItemcodeDetails)(RequsestFormData)
                            Dim getResult = _GRV.GetGeneratedCodeInfo(request.Itemcode, "HHH")
                            Message = JsonConvert.SerializeObject(getResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetAllCodes"
                            Dim request = JsonConvert.DeserializeObject(Of GetCodesRequest)(RequsestFormData)
                            Dim getResult = _GRV.GetCodes(request)
                            Message = JsonConvert.SerializeObject(getResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "SaveGRV"
                            Dim request = JsonConvert.DeserializeObject(Of SaveGRV)(RequsestFormData)
                            Dim getResult = _GRV.Save(request)
                            Message = JsonConvert.SerializeObject(getResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "DownloadLabel"

                            Dim path As String = HttpContext.Current.Server.MapPath("~\temp\" & RequsestFormData & "")
                            Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
                            If file.Exists Then
                                Response.Clear()
                                Dim data As Byte() = req.DownloadData(path)
                                Response.BinaryWrite(data)
                                'Response.ContentType = "Text/Json"
                                Response.ContentType = "application/octet-stream"

                                Response.End()

                            Else
                                Response.Write("This file does not exist.")
                            End If

                            _SendContent = True

                        Case "DeleteFile"
                            Dim deletefiles = JsonConvert.DeserializeObject(Of DeleteGRVFiles)(RequsestFormData)
                            _GRV.DeleteFile(deletefiles)

                            'Message = JsonConvert.SerializeObject(barcodes)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True
                    End Select



            End Select
        End If


        If _SendContent = True Then
            Response.End()
        End If
    End Sub

End Class