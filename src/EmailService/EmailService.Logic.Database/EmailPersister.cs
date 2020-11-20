using System;
using System.Collections.Generic;
using System.Linq;
using EmailService.Logic.Sending;

namespace EmailService.Logic.Database
{
    public class EmailPersister : IEmailPersister, IEmailDataReader, IPendingEmailsGetter
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

        public IEnumerable<ConcreteEmailMessage> GetPendingMails()
        {
            var pendingIds = _statuses
                .Where(x => x.Value == EmailSendingStatus.Pending)
                .Select(x=> x.Key);
            foreach (var pendingId in pendingIds)
            {
                var result = new ConcreteEmailMessage()
                {
                    EmailId = pendingId,
                    EmailMessage = _mails[pendingId]
                };

                yield return result;
            }

        }
    }
}