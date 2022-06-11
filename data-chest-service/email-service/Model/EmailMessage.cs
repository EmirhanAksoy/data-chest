
using Microsoft.AspNetCore.Http;

namespace email_service.Model
{
	public class EmailMessage
    {
		public EmailMessage()
		{
			ToAddresses = new List<EmailAddress>();
			FromAddresses = new List<EmailAddress>();
			CC = new List<EmailAddress>();
			BCC = new List<EmailAddress>();
			Attachments = new List<IFormFile>();
		}

		public List<EmailAddress> ToAddresses { get; set; }
		public List<EmailAddress> FromAddresses { get; set; }
		public List<EmailAddress> CC { get; set; }
		public List<EmailAddress> BCC { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
		public List<IFormFile> Attachments { get; set; }
	}
}
