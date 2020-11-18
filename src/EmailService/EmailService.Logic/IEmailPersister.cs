namespace EmailService.Logic
{
    public interface IEmailPersister
    {
        void PersistEmail(EmailMessage emailMessage);
    }
}