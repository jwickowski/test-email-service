using System;
using System.Collections.Generic;
using EmailService.Logic.Sending;

namespace EmailService.Logic
{
    public interface IEmailDataReader
    {
        EmailSendingStatus GetEmailSendingStatus(Guid id);
        EmailMessage GetEmailMessage(Guid id);

        IEnumerable<ConcreteEmailMessage> GetAll();
    }
}