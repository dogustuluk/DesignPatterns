using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace WebApp.ChainOfResponsibilityDesignPattern.ChainOfResponsibility
{
    public class SendEmailProccessHandler : ProccessHandler
    {
        private readonly string _fileName;
        private readonly string _toEmail;
        public SendEmailProccessHandler(string fileName, string toEmail)
        {
            _fileName = fileName;
            _toEmail = toEmail;
        }
        public override object handle(object o)
        {
            var zipMemoryStream = o as MemoryStream;
            
            zipMemoryStream.Position = 0;
            
            var mailMessage = new MailMessage();

            var smptClient = new SmtpClient("srvm11.trwww.com");

            mailMessage.From = new MailAddress("deneme@kariyersistem.com");

            mailMessage.To.Add(new MailAddress(_toEmail));

            mailMessage.Subject = "Zip dosyası";

            mailMessage.Body = "<p>Zip dosyası ektedir</p>";

            Attachment attachment = new Attachment(zipMemoryStream, _fileName, MediaTypeNames.Application.Zip);

            mailMessage.Attachments.Add(attachment);
            
            mailMessage.IsBodyHtml = true;

           // smptClient.UseDefaultCredentials = true;

            smptClient.Port = 587;

            smptClient.Credentials = new NetworkCredential("deneme@kariyersistem.com", "Password12*");


            smptClient.Send(mailMessage);

            return base.handle(null); //zincirin bir sonraki halkası olmadığından dolayı, "o" yerine "null" şeklinde set edebiliriz.
        }
    }
}
