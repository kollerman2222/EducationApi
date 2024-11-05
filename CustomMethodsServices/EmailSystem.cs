
using FgssrApi.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
//using Microsoft.CodeAnalysis.Elfie.Diagnostics;
//using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;
//using NuGet.Protocol.Plugins;

namespace FgssrApi.CustomMethodsServices
{
    public class EmailSystem : IEmailSystem
    {
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmailSystem(IOptions<EmailSettings> emailSettings, IWebHostEnvironment hostingEnvironment)
        {
            _emailSettings = emailSettings.Value;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<bool> SendEmail(string receiverMail, string subject, string? code, string fullnameenglish)
        {
            string projectPath = _hostingEnvironment.ContentRootPath;
            var filepath = Path.Combine(projectPath, "CustomMethodsServices", "EmailTemplate.html");
            var stream = new StreamReader(filepath);
            var reader = stream.ReadToEnd();
            stream.Close();
            reader = reader.Replace("fullnameenglish", fullnameenglish).Replace("verifycode", code);



            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.MyEmail));
            message.To.Add(new MailboxAddress("Hello", receiverMail));
            message.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = reader;

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port , SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.MyEmail, _emailSettings.MyPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;

        }



    }
}
