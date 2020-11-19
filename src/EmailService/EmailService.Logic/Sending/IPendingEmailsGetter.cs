using System.Collections.Generic;

namespace EmailService.Logic.UnitTests
{
    public interface IPendingEmailsGetter
    {
        public IEnumerable<ConcreteEmailMessage> GetPendingMails();
    }
}