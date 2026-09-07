using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanLedger.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionalWitnessInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Witnesses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationNumber",
                table: "Witnesses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationType",
                table: "Witnesses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Witnesses");

            migrationBuilder.DropColumn(
                name: "IdentificationNumber",
                table: "Witnesses");

            migrationBuilder.DropColumn(
                name: "IdentificationType",
                table: "Witnesses");
        }
    }
}
