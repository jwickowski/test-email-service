using System;
using EmailService.Logic;
using EmailService.Logic.Sending;

namespace EmailService.SMTP
{
    public class SmtpSender: IProtocolEmailSender
    {
        public void SendExternal(EmailMessage mail)
        {
            //Sending using SMTP
        }
    }
}
