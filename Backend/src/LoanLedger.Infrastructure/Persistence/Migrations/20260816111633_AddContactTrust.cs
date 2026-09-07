using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanLedger.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddContactTrust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactTrusts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTrusts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactTrusts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactTrusts_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactTrusts_ContactId_WorkspaceId",
                table: "ContactTrusts",
                columns: new[] { "ContactId", "WorkspaceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactTrusts_WorkspaceId",
                table: "ContactTrusts",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactTrusts");
        }
    }
}
