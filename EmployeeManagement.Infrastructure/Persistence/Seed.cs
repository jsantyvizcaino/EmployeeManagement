using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Security;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence;

internal static class Seed
{
    private const string SeedUserName = "admin";
    private const string SeedPassword = "ProCredit2026*";
    private const string SeedDocumentNumber = "0000000001";
    private const string CreatedBy = "seed";

    private static readonly string[] AreaNames =
    [
        "Recursos Humanos",
        "Finanzas",
        "Contabilidad",
        "Marketing",
        "Sistemas",
        "Banca Empresas",
        "Banca Personas"
    ];

    private static readonly string[] PositionNames =
    [
        "Analista de Recursos Humanos",
        "Contador Senior",
        "Supervisor de Créditos",
        "Diseñador UX/UI",
        "Especialista de Sistemas"
    ];

    public static async Task SeedAppAsync(
        AppDbContext context,
        IPasswordHasherService passwordHasher,
        CancellationToken cancellationToken = default)
    {
        await SeedCatalogsAsync(context, cancellationToken);

        var user = await context.Users.FirstOrDefaultAsync(
            entity => entity.UserName == SeedUserName,
            cancellationToken);

        if (user is null)
        {
            user = new User
            {
                UserName = SeedUserName,
                IsActive = true,
                CreatedBy = CreatedBy
            };
            user.PasswordHash = passwordHasher.HashPassword(user, SeedPassword);

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        var employee = await context.Employees.FirstOrDefaultAsync(
            entity => entity.UserId == user.Id,
            cancellationToken);

        if (employee is null)
        {
            var documentIsUsed = await context.Employees.AnyAsync(
                entity => entity.DocumentNumber == SeedDocumentNumber,
                cancellationToken);

            if (documentIsUsed)
                throw new InvalidOperationException(
                    $"The seed document '{SeedDocumentNumber}' is assigned to another employee.");

            var area = await context.Areas.FirstAsync(
                entity => entity.Name == "Sistemas",
                cancellationToken);
            var position = await context.Positions.FirstAsync(
                entity => entity.Name == "Especialista de Sistemas",
                cancellationToken);

            employee = new Employee
            {
                UserId = user.Id,
                DocumentNumber = SeedDocumentNumber,
                FirstName = "Usuario",
                LastName = "Administrador",
                BirthDate = new DateOnly(1990, 1, 1),
                AreaId = area.Id,
                PositionId = position.Id,
                CreatedBy = CreatedBy
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync(cancellationToken);
        }

        var salaryExists = await context.EmployeeSalaries.AnyAsync(
            entity => entity.EmployeeId == employee.Id,
            cancellationToken);

        if (!salaryExists)
        {
            context.EmployeeSalaries.Add(new EmployeeSalary
            {
                EmployeeId = employee.Id,
                MonthlyAmount = 1500m,
                CreatedBy = CreatedBy
            });

            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private static async Task SeedCatalogsAsync(
        AppDbContext context,
        CancellationToken cancellationToken)
    {
        var existingAreas = await context.Areas
            .Select(entity => entity.Name)
            .ToListAsync(cancellationToken);

        context.Areas.AddRange(
            AreaNames
                .Except(existingAreas, StringComparer.OrdinalIgnoreCase)
                .Select(name => new Area
                {
                    Name = name,
                    CreatedBy = CreatedBy
                }));

        var existingPositions = await context.Positions
            .Select(entity => entity.Name)
            .ToListAsync(cancellationToken);

        context.Positions.AddRange(
            PositionNames
                .Except(existingPositions, StringComparer.OrdinalIgnoreCase)
                .Select(name => new Position
                {
                    Name = name,
                    CreatedBy = CreatedBy
                }));

        await context.SaveChangesAsync(cancellationToken);
    }
}
