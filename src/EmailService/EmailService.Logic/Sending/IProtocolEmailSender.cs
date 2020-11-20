﻿using System.Net.Mail;

namespace EmailService.Logic.Sending
{
    public interface IProtocolEmailSender
    {
        void SendExternal(EmailMessage mail);
    }
}