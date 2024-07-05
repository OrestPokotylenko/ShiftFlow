using SendGrid;
using SendGrid.Helpers.Mail;

namespace Service
{
    public class ResetPasswordEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string fullName, string link)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("testsender302@gmail.com", "TestSender");
            var subject = "Reset password";
            var to = new EmailAddress(toEmail);
            var plainTextContent = $"Hi {fullName}, Use the link below to set up a new password for your account. If you did not request to reset your password, ignore this email and the link will expire on its own. {link}";
            var htmlContent = $"<p><strong>{subject}</strong></p><p>Hi {fullName},</p><p>Use the link below to set up a new password for your account. If you did not request to reset your password, ignore this email and the link will expire on its own.</p><p><a href=\"{link}\">Reset your ShiftFlow password</a></p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
            }
        }
    }
}
