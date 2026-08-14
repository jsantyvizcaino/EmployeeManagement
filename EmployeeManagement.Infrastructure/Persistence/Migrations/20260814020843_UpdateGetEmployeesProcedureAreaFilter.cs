using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGetEmployeesProcedureAreaFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE OR ALTER PROCEDURE [pro].[GetEmployees]
                    @AreaId BIGINT = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        e.[Id],
                        e.[DocumentNumber],
                        e.[FirstName],
                        e.[LastName],
                        e.[BirthDate],
                        DATEDIFF(YEAR, e.[BirthDate], CAST(GETUTCDATE() AS date))
                            - CASE
                                WHEN DATEADD(
                                    YEAR,
                                    DATEDIFF(YEAR, e.[BirthDate], CAST(GETUTCDATE() AS date)),
                                    e.[BirthDate]) > CAST(GETUTCDATE() AS date)
                                THEN 1
                                ELSE 0
                              END AS [Age],
                        a.[Id] AS [AreaId],
                        a.[Name] AS [AreaName],
                        p.[Id] AS [PositionId],
                        p.[Name] AS [PositionName],
                        s.[MonthlyAmount]
                    FROM [pro].[Employees] e
                    INNER JOIN [pro].[Areas] a ON a.[Id] = e.[AreaId]
                    INNER JOIN [pro].[Positions] p ON p.[Id] = e.[PositionId]
                    INNER JOIN [pro].[EmployeeSalaries] s ON s.[EmployeeId] = e.[Id]
                    WHERE @AreaId IS NULL
                       OR e.[AreaId] = @AreaId
                    ORDER BY e.[LastName], e.[FirstName];
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE OR ALTER PROCEDURE [pro].[GetEmployees]
                    @AreaName NVARCHAR(100) = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        e.[Id],
                        e.[DocumentNumber],
                        e.[FirstName],
                        e.[LastName],
                        e.[BirthDate],
                        DATEDIFF(YEAR, e.[BirthDate], CAST(GETUTCDATE() AS date))
                            - CASE
                                WHEN DATEADD(
                                    YEAR,
                                    DATEDIFF(YEAR, e.[BirthDate], CAST(GETUTCDATE() AS date)),
                                    e.[BirthDate]) > CAST(GETUTCDATE() AS date)
                                THEN 1
                                ELSE 0
                              END AS [Age],
                        a.[Id] AS [AreaId],
                        a.[Name] AS [AreaName],
                        p.[Id] AS [PositionId],
                        p.[Name] AS [PositionName],
                        s.[MonthlyAmount]
                    FROM [pro].[Employees] e
                    INNER JOIN [pro].[Areas] a ON a.[Id] = e.[AreaId]
                    INNER JOIN [pro].[Positions] p ON p.[Id] = e.[PositionId]
                    INNER JOIN [pro].[EmployeeSalaries] s ON s.[EmployeeId] = e.[Id]
                    WHERE @AreaName IS NULL
                       OR a.[Name] LIKE N'%' + @AreaName + N'%'
                    ORDER BY e.[LastName], e.[FirstName];
                END
                """);
        }
    }
}
