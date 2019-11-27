Imports Microsoft.VisualBasic

Public Class clsLogin

    Private _IDNumber As String
    Private _LastName As String
    Private _CaptchaCode As String
    Private _IPAddress As String
    Private _Successful As Boolean

    Public Property IDNumber() As String
        Get
            Return _IDNumber
        End Get
        Set(value As String)
            _IDNumber = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(value As String)
            _LastName = value
        End Set
    End Property

    Public Property CaptchaCode() As String
        Get
            Return _CaptchaCode
        End Get
        Set(value As String)
            _CaptchaCode = value
        End Set
    End Property

    Public Property IPAddress() As String
        Get
            Return _IPAddress
        End Get
        Set(value As String)
            _IPAddress = value
        End Set
    End Property

    Public Property Successful() As Boolean
        Get
            Return _Successful
        End Get
        Set(value As Boolean)
            _Successful = value
        End Set
    End Property


End Class
