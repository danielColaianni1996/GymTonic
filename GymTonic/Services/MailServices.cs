using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GymTonic.Services
{
    public class MailServices
    {
        public StringBuilder Template;
        
        public bool SendSchedaMail(string mailTo)
        {
            MailMessage message = new MailMessage("daniel.colaianni@gmail.com", mailTo, "scheda Gym-tonic","Ecco in allegato la tua scheda personalizzata!!");
            var render = new IronPdf.HtmlToPdf();
            var pdf = render.RenderHtmlAsPdf(Template.ToString(), @"C:\Users\dani1\source\repos\GymTonic\GymTonic\");
            
            message.Attachments.Add(new Attachment(pdf.Stream, "scheda.pdf"));
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("daniel.colaianni96@gmail.com", "tecktonik")
            };
            try
            {
                smtp.Send(message);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
        public void LoadTemplate(string templaetPath)
        {
            try
            {
                var path = Path.GetFullPath("SchedaBase.html");
                var file = File.ReadAllText(path);
                Template = new StringBuilder(file);
            }catch(Exception ex)
            {
                //
            }
        }
        public void CompilaTemplate(Dictionary<string,string> data)
        {
            foreach(var dato in data)
            {
                Template.Replace(dato.Key, dato.Value);
            }
        }
    }
}
