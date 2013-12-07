using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class Mailer
    {
        
        public static void SendEmail(string client, string title, string body, string from, string password, string to,string attachmentFile=null, string cc = null)
        {
            try
            {
                MailMessage emailMessage = new MailMessage();
                SmtpClient SmtpClient = new SmtpClient(client);                
                emailMessage.From = new MailAddress(from);
                emailMessage.To.Add(to);
                emailMessage.IsBodyHtml = true;
                emailMessage.Subject = title;
                emailMessage.Body = body;
                if (!string.IsNullOrEmpty(attachmentFile))
                {
                    if(!System.IO.File.Exists(attachmentFile))
                    {
                        throw new Exception("Invalid attachment file name or file not exist !");
                    }
                    Attachment attach = new Attachment(attachmentFile);
                    emailMessage.Attachments.Add(attach);
                }
                SmtpClient.Port = 587;
                SmtpClient.UseDefaultCredentials = false;
                SmtpClient.Credentials = new System.Net.NetworkCredential(from.Split('@')[0], password);
                SmtpClient.EnableSsl = true;
                SmtpClient.Send(emailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}