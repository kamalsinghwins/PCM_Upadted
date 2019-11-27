Imports Npgsql
Imports Microsoft.VisualBasic
Imports System.Data
Imports Entities
Imports System.IO

Public Class ScreensaverHODL
    Inherits DataAccessLayerBase

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing

    Public Sub New(ByVal CompanyCode As String)

        'objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite", "pos_" & CompanyCode)
        'objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead", "pos_" & CompanyCode)
        'connection = Me.DataBase("PostgreConnectionStringPositiveRead", "pos_" & CompanyCode)

        'Only pos_010
        'If HttpContext.Current.IsDebuggingEnabled Then
        '    CompanyCode = "010"
        'End If

        'If Not HttpContext.Current.IsDebuggingEnabled Then
        'Not debugging
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = Me.DataBase("PostgreConnectionStringPositiveRead")
        'Else
        'objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWriteTesting", "pos_demo")
        'objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting", "pos_demo")
        'connection = Me.DataBase("PostgreConnectionStringPositiveReadTesting", "pos_demo")
        'End If

    End Sub

    Public Sub New()

        'If Not HttpContext.Current.IsDebuggingEnabled Then
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = Me.DataBase("PostgreConnectionStringPositiveRead")
        'Else
        'objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWriteTesting", "pos_demo")
        'objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting", "pos_demo")
        'connection = Me.DataBase("PostgreConnectionStringPositiveReadTesting", "pos_demo")
        'End If
    End Sub

    Public Function UploadScreensaverImage(ByVal ImageName As String, ByVal ImageDescription As String, ByVal FileLocation As String) As String

        tmpSQL = "INSERT INTO screensaver_images " & _
           "(image_name,image_description,is_active,date_of_insert,file_location) " & _
           "VALUES ('" & Mid$(RG.Apos(ImageName), 1, 100) & "','" & Mid$(RG.Apos(ImageDescription), 1, 50) & "','True',now(),'" & Mid$(RG.Apos(FileLocation), 1, 250) & "')"
        Try
            'objDBWrite.ExecuteQuery(tmpSQL)
            usingObjDB.ExecuteQuery(_POSWriteConnectionString, tmpSQL)
        Catch ex As Exception

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return ex.Message
        End Try

        'If (objDBWrite IsNot Nothing) Then
        '    objDBWrite.CloseConnection()
        'End If

        Return "Success"

    End Function

    Public Function UpdateImages(ByVal lstImages As entScreensaverImages) As String

        For i As Integer = 0 To lstImages.lstImages.Count - 1

            tmpSQL = "SELECT is_active FROM screensaver_images " & _
                     "WHERE image_name = '" & RG.Apos(lstImages.lstImages(i).image_name) & "'"
            Try
                'Ds = objDBRead.GetDataSet(tmpSQL)
                Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
                If usingObjDB.isR(Ds) Then
                    For Each dr As DataRow In Ds.Tables(0).Rows
                        If dr("is_active").ToString.ToLower <> lstImages.lstImages(i).is_active.ToLower Then
                            'Need to update
                            tmpSQL = "UPDATE screensaver_images " &
                                     "SET is_active = '" & lstImages.lstImages(i).is_active & "',updated = '" & Format(Now, "yyyy-MM-dd") & "' " &
                                     "WHERE image_name = '" & RG.Apos(lstImages.lstImages(i).image_name) & "'"
                            Try
                                'objDBWrite.ExecuteQuery(tmpSQL)
                                usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)
                            Catch ex As Exception
                                'objDBWrite.CloseConnection()
                                Return ex.Message
                            Finally
                                'objDBWrite.CloseConnection()
                            End Try
                        End If
                    Next
                End If
            Catch ex As Exception
                'If (objDBRead IsNot Nothing) Then
                '    objDBRead.CloseConnection()
                'End If
                Return Nothing
            Finally
                'If (objDBWrite IsNot Nothing) Then
                '    objDBWrite.CloseConnection()
                'End If
            End Try

        Next

        'If (objDBRead IsNot Nothing) Then
        '    objDBRead.CloseConnection()
        'End If

        Return "Success"

    End Function

    Private Function ReturnImageAsByte(ByVal FileLocation As String) As Byte()

        If System.IO.File.Exists(FileLocation) Then
            Return System.IO.File.ReadAllBytes(FileLocation)
        Else
            Return New Byte() {0}
        End If


    End Function

    Public Function GetImages() As entScreensaverImages

        Dim SixMonthAgo As String
        SixMonthAgo = Format(Now.Date.AddMonths(-6), "yyyy-MM-dd")

        Dim _return As New entScreensaverImages

        tmpSQL = "SELECT * FROM screensaver_images " & _
                 "WHERE (is_active = True OR updated >= '" & SixMonthAgo & "')"
        Try
            'Ds = objDBWrite.GetDataSet(tmpSQL)
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each dr As DataRow In Ds.Tables(0).Rows
                    Dim _si As New ssImages
                    _si.image_name = dr("image_name") & ""
                    _si.is_active = dr("is_active") & ""
                    '2015-05-04
                    'Updated to not send inactive
                    If dr("is_active") & "" = "True" Then
                        _si.image_byte = ReturnImageAsByte(dr("file_location") & "")
                    End If

                    _return.lstImages.Add(_si)

                Next
            End If
        Catch ex As Exception
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
            Return Nothing
        Finally
            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If
        End Try

        'If (objDBWrite IsNot Nothing) Then
        '    objDBWrite.CloseConnection()
        'End If

        Return _return

    End Function

    Public Function GetImageList() As DataTable

        Dim xData As New DataTable

        Try

            'Dim command As New NpgsqlCommand()
            'connection.Open()
            'command.Connection = connection
            'command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT running_id as ID,image_name,image_description,is_active,file_location FROM screensaver_images ORDER BY date_of_insert DESC"

            Dim reader As New NpgsqlDataAdapter(strQuery, _POSReadConnectionString)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        End Try

        'If (connection IsNot Nothing) Then
        '    connection.Close()
        'End If

        Return xData

    End Function


End Class
