Imports Newtonsoft.Json
Imports Ifs.Fnd.AccessProvider
Imports Ifs.Fnd.Data
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http

Public Class ScanInfo
    Public part_no As String
    Public part_desc As String
    Public supplier As String
    Public supplier_name As String
    Public buyer As String
    Public buyer_name As String
    Public ord_qty As String
    Public last_date As String
    Public email1 As String
    Public email2 As String
    Public email3 As String
    Public email_buyer As String
    Public email_to As String
    Public ship_via As String
    Public notes As String

    Public Sub New(strQueryString As String, Optional ByVal isPost As Boolean = False)
        Dim incoming As New Incoming_Data
        Dim ifsConnection As New ifsInfo()
        Dim fndConn As New FndConnection(ifsConnection.getConnection, ifsConnection.getUser, ifsConnection.getPassword)

        fndConn.InteractiveMode = False
        fndConn.CatchExceptions = True

        If isPost Then
            incoming = JsonConvert.DeserializeObject(Of Incoming_Data)(strQueryString)
        Else
            incoming.part_num = strQueryString
        End If
        part_no = incoming.part_num

        Dim retry As Integer = 0
        While retry < 4
            Try
                Dim cmdFndSql As New PLSQL.FndPLSQLSelectCommand(fndConn, getSQL())

                cmdFndSql.BindVariables.AddFndTextVariable("p0", incoming.part_num, PLSQL.FndBindVariableDirection.In)
                cmdFndSql.BindVariables.AddFndTextVariable("desc_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("sup_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("sup_name_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("buyer_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("buyer_name_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("ord_qty_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("last_ord_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("ship_via_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("email_buyer_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("email_to_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("email1_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("email2_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("email3_", "", PLSQL.FndBindVariableDirection.InOut)
                cmdFndSql.BindVariables.AddFndTextVariable("notes_", "", PLSQL.FndBindVariableDirection.InOut)

                cmdFndSql.ExecuteNonQuery()

                part_desc = cmdFndSql.BindVariables("desc_").Value.ToString
                supplier = cmdFndSql.BindVariables("sup_").Value.ToString
                supplier_name = cmdFndSql.BindVariables("sup_name_").Value.ToString
                buyer = cmdFndSql.BindVariables("buyer_").Value.ToString
                buyer_name = cmdFndSql.BindVariables("buyer_name_").Value.ToString
                ord_qty = cmdFndSql.BindVariables("ord_qty_").Value.ToString
                last_date = cmdFndSql.BindVariables("last_ord_").Value.ToString
                ship_via = cmdFndSql.BindVariables("ship_via_").Value.ToString
                email_buyer = cmdFndSql.BindVariables("email_buyer_").Value.ToString
                email_to = cmdFndSql.BindVariables("email_to_").Value.ToString
                email1 = cmdFndSql.BindVariables("email1_").Value.ToString
                email2 = cmdFndSql.BindVariables("email2_").Value.ToString
                email3 = cmdFndSql.BindVariables("email3_").Value.ToString
                notes = cmdFndSql.BindVariables("notes_").Value.ToString

                retry = 5
            Catch ex As Exception
                If Not IsNothing(ex.InnerException) Then
                    If ex.InnerException.ToString.IndexOf("SSL/TLS") > 0 Then
                        retry += 1
                        Continue While
                    End If
                End If

                Dim x As New CustomError
                x.ThrowError(Net.HttpStatusCode.ExpectationFailed, ex.Message)

            End Try
        End While

    End Sub

    Public Function getSQL() As String
        getSQL = "BEGIN Amr_Purchase_Part_Supplier_Api.Get_Supplier_Kanban_Details(:p0, :desc_, :sup_, :sup_name_, :buyer_, :buyer_name_, :ord_qty_, :last_ord_, :ship_via_, :email_buyer_, :email_to_, :email1_, :email2_, :email3_, :notes_); END;"
    End Function

End Class

Public Class Incoming_Data
    Public part_num As String
End Class

Public Class CustomError
    Private errCode As List(Of KeyValuePair(Of Integer, String))

    Public Sub ThrowError(ByVal iCode As httpStatusCode, ByVal strMessage As String)
        Dim response As New HttpResponseMessage(iCode)
        response.Content = New StringContent(strMessage)
        Throw New HttpResponseException(response)
    End Sub
End Class
