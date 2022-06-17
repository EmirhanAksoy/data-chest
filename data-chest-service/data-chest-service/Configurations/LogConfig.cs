using Serilog;

namespace data_chest_service.Configurations
{
    public static class LogConfig
    {
        public static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateBootstrapLogger();

            Log.Information("Starting up");
        }

        public static void AddLogConfiguartion(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((hostBuilderContext, loggingConfiguration) =>
                loggingConfiguration.WriteTo.Console().ReadFrom.Configuration(hostBuilderContext.Configuration)
           );
        }
    }
}
