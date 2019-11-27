Imports pcm.BusinessLayer

Public Class store_videos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim _blayer As New VideosBusinessLayer

        Dim video_location As String

        video_location = _blayer.GetVideoLocation

        'video.InnerHtml = "<table align=""center""><tr><td><iframe width=""560"" height=""315"" src=""//www.youtube.com/embed/oS6470AO3FQ"" frameborder=""0"" allowfullscreen></iframe></td></tr></table>"
        video.InnerHtml = "<table align=""center""><tr><td><iframe width=""640"" height=""480"" src=""//" & video_location & "?rel=0&amp;showinfo=0"" frameborder=""0"" allowfullscreen></iframe></td></tr></table>"
    End Sub


End Class