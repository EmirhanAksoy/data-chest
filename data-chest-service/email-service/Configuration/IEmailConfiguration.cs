namespace email_service.Configuration
{
	public interface IEmailConfiguration
    {
		public string SmtpServer { get; }
		public int SmtpPort { get; }
		public string SmtpUsername { get; set; }
		public string SmtpEmailAddress { get; set; }
		public string SmtpEmailPassword { get; set; }

		public string PopServer { get; }
		public int PopPort { get; }
		public string PopUsername { get; }
		public string PopPassword { get; }
	}
}
