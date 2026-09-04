using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBalanceTableRemovePresupuestoAddPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_balance_budget_presupuesto_id",
                table: "balance");

            migrationBuilder.DropIndex(
                name: "IX_balance_presupuesto_id",
                table: "balance");

            migrationBuilder.RenameColumn(
                name: "presupuesto_id",
                table: "balance",
                newName: "year");

            migrationBuilder.AddColumn<int>(
                name: "month",
                table: "balance",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "month",
                table: "balance");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "balance",
                newName: "presupuesto_id");

            migrationBuilder.CreateIndex(
                name: "IX_balance_presupuesto_id",
                table: "balance",
                column: "presupuesto_id");

            migrationBuilder.AddForeignKey(
                name: "FK_balance_budget_presupuesto_id",
                table: "balance",
                column: "presupuesto_id",
                principalTable: "budget",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
