using System;
using Xunit;

namespace EmailService.Logic.UnitTests
{
    public class EmailSaverTest
    {
        [Fact]
        public void can_create_EmailSaver()
        {
            var emailSaver = new EmailSaver();
            Assert.NotNull(emailSaver);
        }
    }
}
