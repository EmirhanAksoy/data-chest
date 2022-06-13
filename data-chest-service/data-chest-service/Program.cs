using Alachisoft.NCache.Web.SessionState;
using email_service.Configuration;
using Serilog;
using StackExchange.Redis;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Healt check configuration
builder.Services.AddHealthChecks();

//Serilog Configuration
builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console()
       .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Email service configuration
builder.Services.Configure<EmailConfiguration>(builder.Configuration?.GetSection(nameof(EmailConfiguration)));
builder.Services.AddSingleton<EmailConfiguration>();

// Cache configurations
string? cacheOption = builder.Configuration?.GetSection("Cache:Value")?.Value;
switch (cacheOption)
{
    case "RedisCache":
        {
            string? endPoint = builder.Configuration?.GetSection("Cache:RedisCache:Endpoint")?.Value;
            string? portValue = builder.Configuration?.GetSection("Cache:RedisCache:Port")?.Value;
            int port = int.TryParse(portValue, out var portVal) ? portVal : 8080;

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new ConfigurationOptions
                {
                    EndPoints = { { endPoint, port } },
                    User = builder.Configuration?.GetSection("Cache:RedisCache:Username")?.Value,
                    Password = builder.Configuration?.GetSection("Cache:RedisCache:Password")?.Value
                };
                options.InstanceName = string.Empty;
            });

            break;
        }
    case "NCache":
        {
            builder.Services.AddNCacheSession(builder.Configuration?.GetSection("Cache:NCache"));
            break;
        }
    case "MemoryCache":
        {
            builder.Services.AddDistributedMemoryCache();
            break;
        }
    case "SqlServerCache":
        {
            builder.Services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = builder.Configuration.GetConnectionString("DBConnectionString");
                options.SchemaName = builder.Configuration?.GetSection("Cache:SqlServerCache:Schema").Value;
                options.TableName = builder.Configuration?.GetSection("Cache:SqlServerCache:Table").Value;
            });
            break;
        }
    default:
        break;
}


var app = builder.Build();

//Healt check endpoint configuration
app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Serilog request loging configuration
app.UseSerilogRequestLogging();

app.Run();
