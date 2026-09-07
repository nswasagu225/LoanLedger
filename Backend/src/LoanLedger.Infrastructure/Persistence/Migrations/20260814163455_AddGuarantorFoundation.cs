using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanLedger.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGuarantorFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GuarantorId",
                table: "Loans",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepaymentPlan",
                table: "Loans",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Guarantors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Relationship = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IdentificationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IdentificationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guarantors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guarantors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_GuarantorId",
                table: "Loans",
                column: "GuarantorId");

            migrationBuilder.CreateIndex(
                name: "IX_Guarantors_UserId",
                table: "Guarantors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Guarantors_GuarantorId",
                table: "Loans",
                column: "GuarantorId",
                principalTable: "Guarantors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Guarantors_GuarantorId",
                table: "Loans");

            migrationBuilder.DropTable(
                name: "Guarantors");

            migrationBuilder.DropIndex(
                name: "IX_Loans_GuarantorId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "GuarantorId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "RepaymentPlan",
                table: "Loans");
        }
    }
}
