using MailKit.Net.Smtp;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using System.Net;
using System.Text;
using System.Web;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }


        public async Task<StandardResponse<string>> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(to) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
                    return StandardResponse<string>.Failed("Recipient, subject, and body are required.");

                var smtpSettings = _config.GetSection("SmtpSettings");
                var fromEmail = smtpSettings["FromEmail"];
                var host = smtpSettings["Host"];
                var port = int.Parse(smtpSettings["Port"]);
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];

                // build message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("PQG App", fromEmail));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                //message.Body = new TextPart("html") { Text = body };
                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                message.Body = bodyBuilder.ToMessageBody();

                //using var client = new SmtpClient();
                using var client = new SmtpClient();

                // connect based on port
                if (port == 465)
                {
                    // implicit SSL
                    await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                }
                else
                {
                    // STARTTLS (e.g., port 587)
                    await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                }

                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return StandardResponse<string>.Success("Email sent successfully.", to);
            }
            catch (SmtpCommandException ex)
            {
                return StandardResponse<string>.Failed($"SMTP Command error: {ex.Message} | Code: {ex.StatusCode}");
            }
            catch (SmtpProtocolException ex)
            {
                return StandardResponse<string>.Failed($"SMTP Protocol error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StandardResponse<string>.Failed($"Failure sending mail: {ex.Message}");
            }
        }

        public Task<string> GenerateEmailConfirmationLinkAsync(string email, string token, string scheme)
        {
            var baseUrl = "https://localhost:7166/api/auth/confirmemail";
            //var encodedToken = WebUtility.UrlEncode(token);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return Task.FromResult($"{baseUrl}?email={email}&token={encodedToken}");
        }

        public Task<string> GeneratePasswordResetLinkAsync(string email, string token, string scheme)
        {
            var baseUrl = "https://localhost:7166/api/auth/resetpassword";
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return Task.FromResult($"{scheme}://{baseUrl}?email={email}&token={encodedToken}");
        }
         


    }
}
