using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingBudgetFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeleteByUserId",
                table: "Budget",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Budget",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdateByUserId",
                table: "Budget",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budget_DeleteByUserId",
                table: "Budget",
                column: "DeleteByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Budget_UpdateByUserId",
                table: "Budget",
                column: "UpdateByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_Users_DeleteByUserId",
                table: "Budget",
                column: "DeleteByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_Users_UpdateByUserId",
                table: "Budget",
                column: "UpdateByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_Users_DeleteByUserId",
                table: "Budget");

            migrationBuilder.DropForeignKey(
                name: "FK_Budget_Users_UpdateByUserId",
                table: "Budget");

            migrationBuilder.DropIndex(
                name: "IX_Budget_DeleteByUserId",
                table: "Budget");

            migrationBuilder.DropIndex(
                name: "IX_Budget_UpdateByUserId",
                table: "Budget");

            migrationBuilder.DropColumn(
                name: "DeleteByUserId",
                table: "Budget");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Budget");

            migrationBuilder.DropColumn(
                name: "UpdateByUserId",
                table: "Budget");
        }
    }
}
