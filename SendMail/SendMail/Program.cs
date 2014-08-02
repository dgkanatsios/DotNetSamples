using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SendMail
{
    class Program
    {
        static void Main(string[] args)
        {
            MailMessage m = new MailMessage();
            m.From = new MailAddress("dt008@otenet.gr", "Dimitris - Ilias Gkanatsios");
            m.To.Add(new MailAddress("d.gkanatsios@gmail.com"));
            m.Subject = "Hello there";
            m.Body = "How are you?";

            SmtpClient sc = new SmtpClient();
            sc.Host = "mail.otenet.gr";
            sc.Send(m);
        }
    }
}
