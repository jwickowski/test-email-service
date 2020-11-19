using System;
using System.Linq;
using NSubstitute;
using Xunit;

namespace EmailService.Logic.UnitTests
{
    public class EmailSaverTest
    {
        private readonly IEmailPersister _emailPersister;
        private readonly EmailSaver _emailSaver;

        EmailMessage message = new EmailMessage(new[] { "to@wp.pl" }, "from@wp.pl", "Topic", "Hallo");

        public EmailSaverTest()
        {
            _emailPersister = Substitute.For<IEmailPersister>();
            _emailSaver = new EmailSaver(_emailPersister);
        }

        [Fact]
        public void pass_an_email_to_persist_with_status_pending()
        {
            _emailSaver.SaveEmail(message);
            _emailPersister.Received(1).PersistEmail(Arg.Any<EmailMessage>(), EmailSendingStatus.Pending);
        }

        [Fact]
        public void when_email_is_saving_then_new_guid_id_should_be_returned()
        {
            var newGuid = Guid.NewGuid();
            _emailPersister.PersistEmail(Arg.Any<EmailMessage>(), Arg.Any<EmailSendingStatus>()).Returns(newGuid);

            Guid emailId = _emailSaver.SaveEmail(message);
            Assert.Equal(newGuid, emailId);
        }

        [Fact]
        public void when_from_field_is_empty_then_config_data_should_be_used()
        {
            var newGuid = Guid.NewGuid();
            _emailPersister.PersistEmail(Arg.Any<EmailMessage>(), Arg.Any<EmailSendingStatus>()).Returns(newGuid);

            Guid emailId = _emailSaver.SaveEmail(message);
            Assert.Equal(newGuid, emailId);
        }

        [Fact]
        public void when_from_field_is_empty_then_it_should_be_saved_as_null()
        {
            message=  new EmailMessage(new[] { "to@wp.pl" }, "", "Topic", "Hallo");

             _emailSaver.SaveEmail(message);
             var savedEmailMessage = _emailPersister.ReceivedCalls().First().GetArguments()[0] as EmailMessage;
             Assert.Null(savedEmailMessage.From);
        }
    }
}
