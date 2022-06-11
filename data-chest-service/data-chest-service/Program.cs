using email_service.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Healt check configuration
builder.Services.AddHealthChecks();

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

app.Run();
