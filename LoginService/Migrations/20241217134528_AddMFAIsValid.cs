using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoginService.Migrations
{
    /// <inheritdoc />
    public partial class AddMFAIsValid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnboardingStatus");

            migrationBuilder.AddColumn<int>(
                name: "IsValid",
                table: "MFA",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OnboardingStatusRepository",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingStatusRepository", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OnboardingStatusRepository",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "Success" },
                    { 2, "Fail" },
                    { 3, "Account Existed" },
                    { 4, "Error" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnboardingStatusRepository");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "MFA");

            migrationBuilder.CreateTable(
                name: "OnboardingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OnboardingStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "Success" },
                    { 2, "Fail" },
                    { 3, "Account Existed" },
                    { 4, "Error" }
                });
        }
    }
}
