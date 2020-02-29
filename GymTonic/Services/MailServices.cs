
using GymTonic.DataBase.Table;
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
            MailMessage message = new MailMessage("gymtonic2020@gmail.com", mailTo, "scheda Gym-tonic","Ecco in allegato la tua scheda personalizzata!!");
            //var render = new IronPdf.HtmlToPdf();
            //var pdf = render.RenderHtmlAsPdf(Template.ToString());
            var pdf = new MemoryStream(OpenHtmlToPdf.Pdf.From(Template.ToString()).Content());


            message.Attachments.Add(new Attachment( pdf , "scheda.pdf"));
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("gymtonic2020@gmail.com", "gym123456789")
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
        public void LoadTemplate()
        {
            try
            {
                var path = Path.GetFullPath("SchedaBase.html");
                var file = File.ReadAllText("h:\\root\\home\\daniel1996-001\\www\\schedabase\\SchedaBase.html");
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

        public bool SendPromemoria (string mailTo,string abbonamentoFine)
        {
            string body = string.Format("Ehi, Il tuo abbonamento  è in scadenza il giorno {0} corri in palestra a rinnovarlo!", abbonamentoFine);
            MailMessage message = new MailMessage("gymtonic2020@gmail.com", mailTo,"Abbonamento GymTonic in scadenza",body);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("gymtonic2020@gmail.com", "gym123456789")
            };
            try
            {
                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
