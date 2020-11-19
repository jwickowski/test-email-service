using System;
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

        [Fact]
        public void when_email_is_saving_then_new_guid_id_should_be_returned()
        {
            var emailPersister = Substitute.For<IEmailPersister>();
            var emailSaver = new EmailSaver(emailPersister);
            var message = new EmailMessage(new[] { "to@wp.pl" }, "from@wp.pl", "Hallo");

            Guid emailId = emailSaver.SaveEmail(message);
            Assert.NotEqual(Guid.Empty, emailId);
        }
    }
}
        