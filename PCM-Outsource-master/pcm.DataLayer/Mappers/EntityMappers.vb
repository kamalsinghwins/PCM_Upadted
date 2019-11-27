Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Collections
Imports System.Reflection
Imports Npgsql
Public Class EntityMappers
    Public Shared Function MapDataToBusinessEntityCollection(Of T As New)(dr As NpgsqlDataReader) As List(Of T)
        Dim businessEntityType As Type = GetType(T)
        Dim entitys As New List(Of T)()
        Dim hashtable As New Hashtable()
        Dim properties As PropertyInfo() = businessEntityType.GetProperties()
        For Each info As PropertyInfo In properties
            hashtable(info.Name.ToUpper()) = info
        Next
        While dr.Read()
            Dim newObject As New T()
            For index As Integer = 0 To dr.FieldCount - 1
                Dim nameDat As String = dr.GetName(index).Replace("_", String.Empty)
                Dim propName As Object = hashtable(nameDat.ToUpper())
                Dim info As PropertyInfo = DirectCast(propName, PropertyInfo)
                If (info IsNot Nothing) AndAlso info.CanWrite Then
                    Dim propVal As Object = dr.GetValue(index)
                    If (propVal IsNot DBNull.Value) Then
                        info.SetValue(newObject, propVal, Nothing)
                    End If
                End If
            Next
            entitys.Add(newObject)
        End While
        dr.Close()
        Return entitys
    End Function
End Class