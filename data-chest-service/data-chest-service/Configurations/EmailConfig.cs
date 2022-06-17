using email_service.Configuration;

namespace data_chest_service.Configurations
{
    public static class EmailConfig
    {
        public static void AddEmailConfiguration(this IServiceCollection serviceCollection,ConfigurationManager? configurationManager)
        {
            serviceCollection.Configure<EmailConfiguration>(configurationManager?.GetSection(nameof(EmailConfiguration)));
            serviceCollection.AddSingleton<EmailConfiguration>();
        }
    }
}
