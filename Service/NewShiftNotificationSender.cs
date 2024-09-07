using SendGrid.Helpers.Mail;
using SendGrid;

namespace Service
{
    public class NewShiftNotificationSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string fullName, string link)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("testsender302@gmail.com", "TestSender");
            var subject = "New shift available";
            var to = new EmailAddress(toEmail);
            var plainTextContent = $"Hi {fullName}, Use the link below to view your new shifts. {link}";
            var htmlContent = $"<p><strong>{subject}</strong></p><p>Hi {fullName},</p><p>Use the link below to view your new shifts.</p><p><a href=\"{link}\">View your shifts</a></p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
            }
        }
    }
}