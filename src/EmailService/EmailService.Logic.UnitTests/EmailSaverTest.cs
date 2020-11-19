using System;
using System.Linq;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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

        private EmailSaverTestActResult Act()
        {
            Guid emailId = _emailSaver.SaveEmail(message);

            EmailMessage savedEmailMessage = (EmailMessage)_emailPersister.ReceivedCalls().First().GetArguments()[0];
            EmailSendingStatus passedStatus = (EmailSendingStatus)_emailPersister.ReceivedCalls().First().GetArguments()[1];

            return new EmailSaverTestActResult
            {
                ReturnedMailId = emailId,
                PassedMessage = savedEmailMessage,
                PassedStatus = passedStatus
            };
        }

        [Fact]
        public void pass_an_email_to_persist_with_status_pending()
        {
            Act();
            _emailPersister.Received(1).PersistEmail(Arg.Any<EmailMessage>(), EmailSendingStatus.Pending);
        }

        [Fact]
        public void when_email_is_saving_then_new_guid_id_should_be_returned()
        {
            var newGuid = Guid.NewGuid();
            _emailPersister.PersistEmail(Arg.Any<EmailMessage>(), Arg.Any<EmailSendingStatus>()).Returns(newGuid);

            var result = Act();
            Assert.Equal(newGuid, result.ReturnedMailId);
        }

        [Fact]
        public void when_from_field_is_empty_then_config_data_should_be_used()
        {
            var newGuid = Guid.NewGuid();
            _emailPersister.PersistEmail(Arg.Any<EmailMessage>(), Arg.Any<EmailSendingStatus>()).Returns(newGuid);

            var result = Act();
            Assert.Equal(newGuid, result.ReturnedMailId);
        }

        [Fact]
        public void when_from_field_is_empty_then_it_should_be_saved_as_null()
        {
            message =  new EmailMessage(new[] { "to@wp.pl" }, "", "Topic", "Hallo");

            var result = Act();
             Assert.Null(result.PassedMessage.From);
        }
    }

    internal class EmailSaverTestActResult
    {
        public Guid ReturnedMailId { get; set; }
        public EmailMessage PassedMessage { get; set; }
        public EmailSendingStatus PassedStatus { get; set; }
    }
}
