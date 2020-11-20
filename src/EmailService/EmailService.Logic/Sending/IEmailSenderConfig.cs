namespace EmailService.Logic.Sending
{
    public interface IEmailSenderConfig
    {
        public string DefaultSenderEmail { get; }
    }
}