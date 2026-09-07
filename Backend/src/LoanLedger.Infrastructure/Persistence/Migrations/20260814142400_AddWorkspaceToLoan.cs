using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanLedger.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkspaceToLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Loans",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_WorkspaceId",
                table: "Loans",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Workspaces_WorkspaceId",
                table: "Loans",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Workspaces_WorkspaceId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_WorkspaceId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Loans");
        }
    }
}
