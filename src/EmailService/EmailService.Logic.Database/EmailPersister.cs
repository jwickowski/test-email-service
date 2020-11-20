using System;

namespace EmailService.Logic.Database
{
    public class EmailPersister : IEmailPersister
    {
        public  Guid PersistEmail(EmailMessage emailMessage, EmailSendingStatus emailSendingStatus)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(Guid emailId, EmailSendingStatus sent)
        {
            throw new NotImplementedException();
        }
    }
}