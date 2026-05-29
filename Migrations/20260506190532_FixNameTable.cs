using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class FixNameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sale_detalles_sales_sale_id",
                table: "sale_detalles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sale_detalles",
                table: "sale_detalles");

            migrationBuilder.RenameTable(
                name: "sale_detalles",
                newName: "sales_details");

            migrationBuilder.RenameIndex(
                name: "IX_sale_detalles_sale_id",
                table: "sales_details",
                newName: "IX_sales_details_sale_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sales_details",
                table: "sales_details",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_details_sales_sale_id",
                table: "sales_details",
                column: "sale_id",
                principalTable: "sales",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sales_details_sales_sale_id",
                table: "sales_details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sales_details",
                table: "sales_details");

            migrationBuilder.RenameTable(
                name: "sales_details",
                newName: "sale_detalles");

            migrationBuilder.RenameIndex(
                name: "IX_sales_details_sale_id",
                table: "sale_detalles",
                newName: "IX_sale_detalles_sale_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sale_detalles",
                table: "sale_detalles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_sale_detalles_sales_sale_id",
                table: "sale_detalles",
                column: "sale_id",
                principalTable: "sales",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
