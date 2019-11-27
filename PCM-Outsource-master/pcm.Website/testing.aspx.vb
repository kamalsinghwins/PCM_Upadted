Imports System.Web.Services
Imports DevExpress.Web.ASPxTreeList

Public Class testing1
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CreateNodes()

    End Sub
    'Private Sub CreateNodes()
    '    Dim localFolders As TreeListNode = CreateNodeCore(1, "Folder", "<b>Local Folders</b>", Nothing)
    '    localFolders.Expanded = False
    '    CreateNodeCore(2, "Features", "Inbox", localFolders)
    '    CreateNodeCore(3, "Features", "Outbox", localFolders)
    '    CreateNodeCore(4, "Features", "Sent Items", localFolders)
    '    CreateNodeCore(5, "Features", "Deleted Items", localFolders)
    '    CreateNodeCore(6, "Features", "Drafts", localFolders)
    '    Dim communityNode As TreeListNode = CreateNodeCore(7, "Folder", "<b>community.devexpress.com</b>", Nothing)

    '    CreateNodeCore(8, "Features", "general.announcements", communityNode)
    '    CreateNodeCore(9, "Features", "general.discussion", communityNode)
    '    CreateNodeCore(10, "Features", "general.ordering", communityNode)
    'End Sub

    'Private Function CreateNodeCore(ByVal key As Object, ByVal iconName As String, ByVal text As String, ByVal parentNode As TreeListNode) As TreeListNode
    '    Dim node As TreeListNode = treeList.AppendNode(key, parentNode)
    '    node("IconName") = iconName
    '    node("Name") = text
    '    Return node
    'End Function

    Protected Function GetIconUrl(ByVal container As TreeListDataCellTemplateContainer) As String
        Return "~/Images/features.png"
    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'TextBox2.Text = Val(TextBox1.Text)
        'TextBox3.Text = CDbl(TextBox1.Text)

        Dim startDate As Date = "2016-01-28"
        Dim xDate As Date = "2016-02-05"


        If DateDiff("d", startDate, Format(Now, "yyyy-MM-dd")) >= 0 Then
            If DateDiff("d", xDate, Format(Now, "yyyy-MM-dd")) < 0 Then
                'If AccountDetails.TransactionType = "PAY" Then
                '    'If dr("show_on_age_analysis") & "" = False Then
                '    is150 = True
                '    AccountDetails.isSpecialPayment = True
                '    'End If
                'End If
            End If
        End If

    End Sub
End Class