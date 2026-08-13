using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class addActivoIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "activo",
                table: "income_transfers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activo",
                table: "income_transfers");
        }
    }
}
