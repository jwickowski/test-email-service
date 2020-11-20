using System;
using System.Collections.Generic;

namespace EmailService.Logic.Database
{
    public class EmailPersister : IEmailPersister, IEmailDataReader
    {
        private Dictionary<Guid, EmailMessage> _mails;
        private Dictionary<Guid, EmailSendingStatus> _statuses;

        public EmailPersister()
        {
            _mails = new Dictionary<Guid, EmailMessage>();
            _statuses = new Dictionary<Guid, EmailSendingStatus>();
        }

        public Guid PersistEmail(EmailMessage emailMessage, EmailSendingStatus emailSendingStatus)
        {
            var newKey = Guid.NewGuid();
            _mails.Add(newKey, emailMessage);
            _statuses.Add(newKey, emailSendingStatus);
            return newKey;
        }

        public void UpdateStatus(Guid emailId, EmailSendingStatus emailSendingStatus)
        {
            _statuses[emailId] = emailSendingStatus;
        }

        public EmailSendingStatus GetEmailSendingStatus(Guid id)
        {
            return _statuses[id];
        }

        public EmailMessage GetEmailMessage(Guid id)
        {
            return _mails[id];
        }
    }
}