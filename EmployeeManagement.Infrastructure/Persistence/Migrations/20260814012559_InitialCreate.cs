using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pro");

            migrationBuilder.EnsureSchema(
                name: "asp");

            migrationBuilder.CreateTable(
                name: "Areas",
                schema: "pro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "pro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "asp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "pro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AreaId = table.Column<long>(type: "bigint", nullable: false),
                    PositionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Areas_AreaId",
                        column: x => x.AreaId,
                        principalSchema: "pro",
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "pro",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "asp",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaries",
                schema: "pro",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    MonthlyAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaries", x => x.Id);
                    table.CheckConstraint("CK_EmployeeSalaries_MonthlyAmount_Positive", "[MonthlyAmount] > 0");
                    table.ForeignKey(
                        name: "FK_EmployeeSalaries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "pro",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AreaId",
                schema: "pro",
                table: "Employees",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DocumentNumber",
                schema: "pro",
                table: "Employees",
                column: "DocumentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PositionId",
                schema: "pro",
                table: "Employees",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                schema: "pro",
                table: "Employees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_EmployeeId",
                schema: "pro",
                table: "EmployeeSalaries",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "asp",
                table: "Users",
                column: "UserName",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DROP PROCEDURE IF EXISTS [pro].[GetEmployees];");

            migrationBuilder.DropTable(
                name: "EmployeeSalaries",
                schema: "pro");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "pro");

            migrationBuilder.DropTable(
                name: "Areas",
                schema: "pro");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "pro");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "asp");
        }
    }
}
