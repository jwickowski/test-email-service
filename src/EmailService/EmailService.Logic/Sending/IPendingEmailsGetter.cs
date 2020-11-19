using System.Collections.Generic;

namespace EmailService.Logic.Sending
{
    public interface IPendingEmailsGetter
    {
        public IEnumerable<ConcreteEmailMessage> GetPendingMails();
    }
}