using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CodexRoyaleClassesCore3.Models.Email
{
    public class EmailSender
    {
        private AuthMessageSenderOptions options;
        private string FrontEndAddress = "https://codexroyale.com/";

                    
        public EmailSender(AuthMessageSenderOptions _options)
        {
            options = _options;
        }


        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(options.SendGridKey, subject, message, email);
        }


        public Task SendEmailVerificationAsync(string recipientEmail, string emailVerificationCode)
        {
            string welcomeSubject = "Authenticate account";
            string emailBody = "Follow the link to verify your Clash Codex account " + FrontEndAddress + "register/authenticate/" + emailVerificationCode;

            return Execute(options.SendGridKey, welcomeSubject, emailBody, recipientEmail);
        }

        public Task SendPasswordResetAsync(string recipientEmail, string username, string passwordResetCode)
        {
            string welcomeSubject = "Reset Clash Codex Password";
            string emailBody = "Hi " + username + ", Follow the link to reset your Clash Codex account " + FrontEndAddress + "/forgotpassword/" + passwordResetCode;

            return Execute(options.SendGridKey, welcomeSubject, emailBody, recipientEmail);
        }



        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(options.SendGridUser, "Clash Codex"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}