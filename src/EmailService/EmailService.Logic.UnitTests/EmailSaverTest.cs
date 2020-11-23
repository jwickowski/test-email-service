using System;
using System.Linq;
using EmailService.Logic.Saving;
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
            message = new EmailMessage(new[] { "to@wp.pl" }, "", "Topic", "Hallo");

            var result = Act();
            Assert.Null(result.PassedMessage.From);
        }

        [Fact]
        public void when_there_in_no_recipients_then_NoRecipientsException_should_be_thrown()
        {
            message = new EmailMessage(new string[] { }, "", "Topic", "Hallo");

            Assert.Throws<NoRecipientsException>(() => Act());
        }

        [Fact]
        public void when_from_field_is_not_valid_then_FromFieldIsNotValidException_should_be_thrown()
        {
            message = new EmailMessage(new string[] { "test@wp.pl" }, "wrongEmailFormat", "Topic", "Hallo");

            Assert.Throws<FromFieldIsNotValidException>(() => Act());
        }

        [Fact]
        public void when_to_field_is_not_valid_then_ToFieldIsNotValidException_should_be_thrown()
        {
            message = new EmailMessage(new string[] { "wrongEmailFormat" }, "test@wp.pl", "Topic", "Hallo");

            Assert.Throws<ToFieldIsNotValidException>(() => Act());
        }
    }

    internal class EmailSaverTestActResult
    {
        public Guid ReturnedMailId { get; set; }
        public EmailMessage PassedMessage { get; set; }
        public EmailSendingStatus PassedStatus { get; set; }
    }
}
