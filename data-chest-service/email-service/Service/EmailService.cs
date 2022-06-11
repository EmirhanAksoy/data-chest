
using email_service.Configuration;
using email_service.Model;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace email_service.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        public EmailService(IOptionsSnapshot<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }
        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            using Pop3Client emailClient = new();

            emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

            emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

            List<EmailMessage> emails = new();
            for (int i = 0; i < emailClient.Count && i < maxCount; i++)
            {
                var message = emailClient.GetMessage(i);
                var emailMessage = new EmailMessage
                {
                    Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                    Subject = message.Subject
                };
                emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                emails.Add(emailMessage);
            }

            return emails;
        }
        public void Send(EmailMessage emailMessage)
        {
            MimeMessage message = new();

            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.From.Add(new MailboxAddress(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpEmailAddress));

            // message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            if (emailMessage != null && emailMessage.CC.Count > 0)
                message.Cc.AddRange(emailMessage.CC.Select(x => new MailboxAddress(x.Name, x.Address)));
            if (emailMessage != null && emailMessage.BCC.Count > 0)
                message.Bcc.AddRange(emailMessage.BCC.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage?.Subject;


            //We will say we are sending HTML. But there are options for plaintext etc. 

            BodyBuilder builder = new();
            builder.TextBody = emailMessage?.Content;

            foreach (var attachment in emailMessage.Attachments.ToList())
            {
                string[] contentTypePars = attachment.ContentType.Split("/");

                if (contentTypePars.Length == 2)
                    builder.Attachments.Add(attachment.FileName, attachment.OpenReadStream(), new ContentType(contentTypePars[0], contentTypePars[1]));
                else
                    builder.Attachments.Add(attachment.FileName, attachment.OpenReadStream());
            }

            message.Body = builder.ToMessageBody();

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using SmtpClient emailClient = new();
            //The last parameter here is to use SSL (Which you should!)
            emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, useSsl: true);

            //Remove any OAuth functionality as we won't be using it. 
            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

            emailClient.Authenticate(_emailConfiguration.SmtpEmailAddress, _emailConfiguration.SmtpEmailPassword);

            try
            {
                emailClient.Send(message);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

            emailClient.Disconnect(true);

        }
    }
}
