using EmployeeManagement.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration
    .GetSection(AppSettings.SectionName)
    .Get<AppSettings>()
    ?? throw new InvalidOperationException(
        $"Configuration section '{AppSettings.SectionName}' was not found.");

appSettings.CheckSettings();
builder.Services.AddSingleton(appSettings);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
