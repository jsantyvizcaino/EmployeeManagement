using System.IdentityModel.Tokens.Jwt;
using System.Text;
using EmployeeManagement.Domain.Interfaces.Persistence;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Domain.Interfaces.Security;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Infrastructure.Persistence.Repositories;
using EmployeeManagement.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var dbSettings = configuration
            .GetSection(DbSettings.SectionName)
            .Get<DbSettings>()
            ?? throw new InvalidOperationException(
                $"Configuration section '{DbSettings.SectionName}' was not found.");

        var jwtSettings = configuration
            .GetSection(JwtSettings.SectionName)
            .Get<JwtSettings>()
            ?? throw new InvalidOperationException(
                $"Configuration section '{JwtSettings.SectionName}' was not found.");

        dbSettings.CheckSettings();
        jwtSettings.CheckSettings();

        services.AddSingleton(dbSettings);
        services.AddSingleton(jwtSettings);

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                dbSettings.ConnectionString,
                sqlOptions =>
                {
                    sqlOptions.CommandTimeout(dbSettings.CommandTimeoutSeconds);
                    sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    sqlOptions.MigrationsHistoryTable(
                        DatabaseConstants.MigrationsHistoryTable,
                        DatabaseConstants.BusinessSchema);
                }));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = JwtRegisteredClaimNames.Sub
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static async Task ApplyAppMigrationsAsync(
        this IApplicationBuilder app,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync(cancellationToken);

        if (!configuration.GetValue<bool>("SeedDb"))
            return;

        var passwordHasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasherService>();

        await Seed.SeedAppAsync(context, passwordHasher, cancellationToken);
    }
}
