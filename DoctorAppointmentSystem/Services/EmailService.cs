using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace DoctorAppointmentSystem.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                Console.WriteLine($"[EmailService] Sending email to: {toEmail}");

                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = _configuration["EmailSettings:SmtpPort"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];
                var enableSsl = _configuration["EmailSettings:EnableSsl"];

                if (string.IsNullOrWhiteSpace(smtpHost) ||
                    string.IsNullOrWhiteSpace(smtpPort) ||
                    string.IsNullOrWhiteSpace(senderEmail) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("Email settings are not configured properly.");
                }

                using var client = new SmtpClient(smtpHost, int.Parse(smtpPort))
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = bool.Parse(enableSsl ?? "true")
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                message.To.Add(toEmail);

                await client.SendMailAsync(message);

                Console.WriteLine("[EmailService] Email sent successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] Error: {ex.Message}");
                return false;
            }
        }
    }
}