Imports System.Net
Imports System.Web.Http
Imports System.Web.Http.Results
Imports Newtonsoft.Json


Public Class SupplierKanbanController
    Inherits ApiController

    ' GET api/<controller>
    <Route("api/SupplierKanban/{part}")>
    <HttpGet>
    Public Function GetValues(ByVal part As String) As JsonResult(Of ScanInfo)
        Dim retData As New ScanInfo(part)
        Dim serializerSettings As New JsonSerializerSettings
        Dim z As New Http.HttpRequestMessage

        Return New JsonResult(Of ScanInfo)(retData, serializerSettings, Encoding.UTF8, z)
    End Function

    <Route("api/SupplierKanban")>
    <HttpPost>
    Public Async Function PostValue() As Threading.Tasks.Task(Of JsonResult(Of ScanInfo))
        Dim body_data As String = Await Request.Content.ReadAsStringAsync

        Dim retData As New ScanInfo(body_data, True)
        Dim serializerSettings As New JsonSerializerSettings
        Dim z As New Http.HttpRequestMessage


        Return New JsonResult(Of ScanInfo)(retData, serializerSettings, Encoding.UTF8, z)
    End Function

    <Route("api/SupplierKanbanEmail")>
    <HttpPost>
    Public Async Function KanbanSendEmail() As Threading.Tasks.Task(Of JsonResult(Of sendEmail))
        Dim body_data As String = Await Request.Content.ReadAsStringAsync

        Dim retData As New sendEmail(body_data)
        Dim serializerSettings As New JsonSerializerSettings
        Dim z As New Http.HttpRequestMessage


        Return New JsonResult(Of sendEmail)(retData, serializerSettings, Encoding.UTF8, z)
    End Function

End Class
