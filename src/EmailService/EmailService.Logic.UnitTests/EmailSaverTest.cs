using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EmailService.Logic.UnitTests
{
    public class EmailSaverTest
    {
        [Fact]
        public void pass_an_email_to_persist()
        {
            
            var emailSaver = new EmailSaver();
            var message = new EmailMessage(new[] { "to@wp.pl" }, "from@wp.pl", "Hallo");

            emailSaver.SaveEmail(message);
            Assert.NotNull(emailSaver);
        }
    }
}
