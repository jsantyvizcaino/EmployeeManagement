using System.Text.Json.Serialization;
using Asp.Versioning;
using EmployeeManagement.Application;
using EmployeeManagement.API.Middleware;
using EmployeeManagement.API.OpenApi;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using AppEmptyResult = EmployeeManagement.Domain.Dtos.EmptyResult;

var builder = WebApplication.CreateBuilder(args);

const string corsPolicy = "ApiCorsPolicy";

var appSettings = builder.Configuration
    .GetSection(AppSettings.SectionName)
    .Get<AppSettings>()
    ?? throw new InvalidOperationException(
        $"Configuration section '{AppSettings.SectionName}' was not found.");

appSettings.CheckSettings();
builder.Services.AddSingleton(appSettings);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy
            .WithOrigins(appSettings.BaseDomain)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values
                .SelectMany(value => value.Errors)
                .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                    ? "El cuerpo de la solicitud no es válido."
                    : error.ErrorMessage)
                .Distinct();
            var response = AppEmptyResult.InvalidRequest(
                string.Join(Environment.NewLine, errors));

            return new BadRequestObjectResult(response);
        };
    });

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.AddHealthChecks();

var app = builder.Build();

await app.ApplyAppMigrationsAsync(app.Configuration);

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}.json");
    app.MapScalarApiReference("/scalar", options =>
    {
        options.Title = "Employee Management API";
        options.OpenApiRoutePattern = "/openapi/v1.json";
        options.AddPreferredSecuritySchemes("Bearer");
    });
}

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
