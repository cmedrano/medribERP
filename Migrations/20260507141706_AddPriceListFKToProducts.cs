using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceListFKToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "lista_precio_id",
                table: "Clientes",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_lista_precio_id",
                table: "Clientes",
                column: "lista_precio_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_listas_precios_lista_precio_id",
                table: "Clientes",
                column: "lista_precio_id",
                principalTable: "listas_precios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_listas_precios_lista_precio_id",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_lista_precio_id",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "lista_precio_id",
                table: "Clientes");
        }
    }
}
