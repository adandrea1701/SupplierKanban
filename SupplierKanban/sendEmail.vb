Imports System.Net.Mail

Public Class sendEmail
    Public sent As String

    Public Sub New(ByVal strQueryString As String)
        Dim email_params As New ScanInfo(strQueryString, True)

        If email_params.buyer_name <> "" And email_params.email_buyer <> "" Then
            Try
                Dim subject As String = "Airmar order for " & email_params.ord_qty & " for Item: " & email_params.part_no
                Dim smtp_host As New SmtpClient("relay.airmar.com")
                Dim smtp_mess As New MailMessage(email_params.email_buyer, email_params.email_buyer)
                Dim message_body As String = getEmailBody()

                message_body = message_body.Replace("{qty}", email_params.ord_qty).Replace("{Ship_Via}", email_params.ship_via).Replace("{Notes}", email_params.notes)
                message_body = message_body.Replace("{item}", email_params.part_no).Replace("{buyer_name}", email_params.buyer_name).Replace("{buyer_email}", email_params.email_buyer)

                If email_params.email_to <> "" Then smtp_mess.To.Add(email_params.email_to)
                If email_params.email1 <> "" Then smtp_mess.CC.Add(email_params.email1)
                If email_params.email2 <> "" Then smtp_mess.CC.Add(email_params.email2)
                If email_params.email3 <> "" Then smtp_mess.CC.Add(email_params.email3)

                smtp_mess.IsBodyHtml = True
                smtp_mess.Subject = subject
                smtp_mess.Body = message_body

                smtp_host.Send(smtp_mess)
                sent = "success"
            Catch ex As Exception
                Dim x As New CustomError
                x.ThrowError(Net.HttpStatusCode.ExpectationFailed, ex.Message)
            End Try

        End If



    End Sub

    Public Function getEmailBody()
        getEmailBody = "
<p>Dear Sir or Madame,</p>
<p><span> Airmar Technology Corporation requires a quantity of {qty} of Item Number {item}.</span></p>
<p>{Ship_Via}</p>
<p>Special Notes: </p>
<p>{Notes}</p>
<p><o:p>&nbsp;</o:p></p>
<p>Please send any inquiries to: {buyer_name} at <a href='mailto:{buyer_email}'>{buyer_email}</a></p>
<p style='margin-bottom:0in;margin-bottom:.0001pt;line-height:107%'>Airmar Technology Corporation</p>
<p style='margin-bottom:0in;margin-bottom:.0001pt;line-height:107%'>35 Meadowbrook Dr.</p>
<p style='margin-bottom:0in;margin-bottom:.0001pt;line-height:107%'>Milford, NH 03055</p>
<p style='margin-bottom:0in;margin-bottom:.0001pt;line-height:107%'>Phone: 603-673-9570</p>
<p style='margin-bottom:0in;margin-bottom:.0001pt;line-height:107%'>Fax: 603-673-4624</p>
<p style='margin-bottom:0in;margin-bottom:.0001pt;line-height:107%'><a href='http://www.airmar.com'>www.airmar.com</a></p>
"
    End Function
End Class
