using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanLedger.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Loans");
        }
    }
}
