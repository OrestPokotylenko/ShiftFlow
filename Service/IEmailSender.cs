namespace Service
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string fullName, string link);
    }
}
