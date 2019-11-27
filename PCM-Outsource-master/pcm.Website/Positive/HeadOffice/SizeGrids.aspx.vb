Imports DevExpress.Web

Public Class SizeGrids
    Inherits System.Web.UI.Page

    'Protected Sub ASPxGridView1_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)
    '    Dim ds As DataSet
    '    Dim gridView As ASPxGridView = CType(sender, ASPxGridView)
    '    Dim dataTable As DataTable
    '    If gridView.SettingsDetail.IsDetailGrid Then
    '        dataTable = ds.Tables(1)
    '    Else
    '        dataTable = ds.Tables(0)
    '    End If
    '    Dim row As DataRow = dataTable.NewRow()
    '    e.NewValues("ID") = GetNewId()
    '    Dim enumerator As IDictionaryEnumerator = e.NewValues.GetEnumerator()
    '    enumerator.Reset()
    '    Do While enumerator.MoveNext()
    '        If enumerator.Key.ToString() <> "Count" Then
    '            row(enumerator.Key.ToString()) = enumerator.Value
    '        End If
    '    Loop
    '    gridView.CancelEdit()
    '    e.Cancel = True
    '    dataTable.Rows.Add(row)
    'End Sub

    'Private Function GetNewId() As Integer
    '    Dim ds As DataSet
    '    Dim table As DataTable = ds.Tables(0)
    '    If table.Rows.Count = 0 Then
    '        Return 0
    '    End If
    '    Dim max As Integer = Convert.ToInt32(table.Rows(0)("ID"))
    '    For i As Integer = 1 To table.Rows.Count - 1
    '        If Convert.ToInt32(table.Rows(i)("ID")) > max Then
    '            max = Convert.ToInt32(table.Rows(i)("ID"))
    '        End If
    '    Next i
    '    Return max + 1
    'End Function

    Public Function GetData() As DataTable
        Dim Table As New DataTable()
        Table.Rows.Add(1)
        Return Table

    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        grid.AddNewRow()


    End Sub
End Class