using SendGrid;
using SendGrid.Helpers.Mail;

namespace Service
{
    public class ResetPasswordEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string fullName)
        {
            var link = "https://www.example.com/reset-password";
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("testsender302@gmail.com", "TestSender");
            var subject = "Reset password";
            var to = new EmailAddress(toEmail);
            var plainTextContent = $"Hi {fullName}, Use this link below to set up a new password for your account. If you did not request to reset your password, ignore this email and the link will expire on its own. {link}";
            var html = $"<p><strong>{subject}</strong></p><p>Hi {fullName},</p><p>Use this link below to set up a new password for your account. If you did not request to reset your password, ignore this email and the link will expire on its own.</p><p>{link}</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, html);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
            }
        }
    }
}
