using EmployeeManagement.Domain.Models;
using EmployeeManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration
    .GetSection(AppSettings.SectionName)
    .Get<AppSettings>()
    ?? throw new InvalidOperationException(
        $"Configuration section '{AppSettings.SectionName}' was not found.");

appSettings.CheckSettings();
builder.Services.AddSingleton(appSettings);
builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

await app.ApplyAppMigrationsAsync(app.Configuration);

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
