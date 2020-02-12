
Public Class ifsInfo
    Private Const strIfsTest_ As String = "https://ifstdm-apps.airmar.com:60080"
    Private Const strIfsMigr_ As String = "https://ifstdm-apps.airmar.com:59080"
    Private Const strIfsDev_ As String = "https://ifstdm-apps.airmar.com:58080"
    Private Const strIfsProduction_ As String = "https://ifsapps.airmar.com:58080"

    Private Const userName_ As String = "airm1app"

    Private Const strIfsProductionPassWord_ As String = "ifsOwner2017"
    Private Const strIfsTestPassWord_ As String = "ifsOwner2017"
    Private Const strIfsDevPassWord_ As String = "ifsOwner2017"
    Private Const strIfsMigrPassWord_ As String = "ifsSupport2018"

    Private currentEnvironment_ As String
    Private currentPassword_ As String

    Public Sub New()
        Dim buildEnvironment As String = My.Settings.BuildEnvironment.ToUpper
        'HttpContext.Current.Request.Url.Host

        Select Case buildEnvironment
            Case "PROD"
                currentEnvironment_ = strIfsProduction_
                currentPassword_ = strIfsProductionPassWord_
            Case "DEV"
                currentEnvironment_ = strIfsDev_
                currentPassword_ = strIfsDevPassWord_
            Case "TEST"
                currentEnvironment_ = strIfsTest_
                currentPassword_ = strIfsTestPassWord_
            Case "MIGR"
                currentEnvironment_ = strIfsMigr_
                currentPassword_ = strIfsMigrPassWord_
        End Select
    End Sub

    Public Function getConnection() As String
        Return currentEnvironment_
    End Function


    Public Function getPassword() As String
        Return currentPassword_
    End Function

    Public Function getUser() As String
        Return userName_
    End Function
End Class
