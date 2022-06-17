using data_chest_service.Configurations;
using Serilog;

LogConfig.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.AddLogConfiguartion();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddEmailConfiguration(builder.Configuration);

builder.Services.AddCacheConfiguration(builder.Configuration);

var app = builder.Build();

app.MapHealthChecks("/healthz");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();

app.Run();
