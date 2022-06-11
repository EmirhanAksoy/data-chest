using email_service.Configuration;
using Serilog;

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
