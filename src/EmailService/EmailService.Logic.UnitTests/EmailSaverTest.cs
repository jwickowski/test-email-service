using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Xunit;

namespace EmailService.Logic.UnitTests
{
    public class EmailSaverTest
    {
        [Fact]
        public void pass_an_email_to_persist()
        {
            var emailPersister = Substitute.For<IEmailPersister>();
            var emailSaver = new EmailSaver(emailPersister);
            var message = new EmailMessage(new[] { "to@wp.pl" }, "from@wp.pl", "Hallo");

            emailSaver.SaveEmail(message);
            
            emailPersister.Received(1).PersistEmail(Arg.Any<EmailMessage>());
        }
    }
}
