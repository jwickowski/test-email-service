using System.Net.Mail;

namespace EmailService.Logic.UnitTests
{
    public interface IProtocolEmailSender
    {
        void SendExternal(MailMessage mail);
    }
}