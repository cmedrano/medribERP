using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RubroType_CompanyId",
                table: "RubroType",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CompanyId",
                table: "Gastos",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_PeriodoId",
                table: "Gastos",
                column: "PeriodoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_CompanyId",
                table: "Cuentas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_articulos_precios_articulo_id",
                table: "articulos_precios",
                column: "articulo_id");

            migrationBuilder.CreateIndex(
                name: "IX_articulos_precios_lista_precio_id",
                table: "articulos_precios",
                column: "lista_precio_id");

            migrationBuilder.CreateIndex(
                name: "IX_articulos_marcaId",
                table: "articulos",
                column: "marcaId");

            migrationBuilder.CreateIndex(
                name: "IX_articulos_proveedorId",
                table: "articulos",
                column: "proveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_articulos_rubroId",
                table: "articulos",
                column: "rubroId");

            migrationBuilder.CreateIndex(
                name: "IX_areas_per_user_user_id",
                table: "areas_per_user",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_areas_per_user_Users_user_id",
                table: "areas_per_user",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articulos_brand_marcaId",
                table: "articulos",
                column: "marcaId",
                principalTable: "brand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articulos_product_category_rubroId",
                table: "articulos",
                column: "rubroId",
                principalTable: "product_category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articulos_provider_proveedorId",
                table: "articulos",
                column: "proveedorId",
                principalTable: "provider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articulos_precios_articulos_articulo_id",
                table: "articulos_precios",
                column: "articulo_id",
                principalTable: "articulos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articulos_precios_listas_precios_lista_precio_id",
                table: "articulos_precios",
                column: "lista_precio_id",
                principalTable: "listas_precios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Company_CompanyId",
                table: "Cuentas",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Company_CompanyId",
                table: "Gastos",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_periods_PeriodoId",
                table: "Gastos",
                column: "PeriodoId",
                principalTable: "periods",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_RubroType_Company_CompanyId",
                table: "RubroType",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_areas_per_user_Users_user_id",
                table: "areas_per_user");

            migrationBuilder.DropForeignKey(
                name: "FK_articulos_brand_marcaId",
                table: "articulos");

            migrationBuilder.DropForeignKey(
                name: "FK_articulos_product_category_rubroId",
                table: "articulos");

            migrationBuilder.DropForeignKey(
                name: "FK_articulos_provider_proveedorId",
                table: "articulos");

            migrationBuilder.DropForeignKey(
                name: "FK_articulos_precios_articulos_articulo_id",
                table: "articulos_precios");

            migrationBuilder.DropForeignKey(
                name: "FK_articulos_precios_listas_precios_lista_precio_id",
                table: "articulos_precios");

            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Company_CompanyId",
                table: "Cuentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Company_CompanyId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_periods_PeriodoId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_RubroType_Company_CompanyId",
                table: "RubroType");

            migrationBuilder.DropIndex(
                name: "IX_RubroType_CompanyId",
                table: "RubroType");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_CompanyId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_PeriodoId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Cuentas_CompanyId",
                table: "Cuentas");

            migrationBuilder.DropIndex(
                name: "IX_articulos_precios_articulo_id",
                table: "articulos_precios");

            migrationBuilder.DropIndex(
                name: "IX_articulos_precios_lista_precio_id",
                table: "articulos_precios");

            migrationBuilder.DropIndex(
                name: "IX_articulos_marcaId",
                table: "articulos");

            migrationBuilder.DropIndex(
                name: "IX_articulos_proveedorId",
                table: "articulos");

            migrationBuilder.DropIndex(
                name: "IX_articulos_rubroId",
                table: "articulos");

            migrationBuilder.DropIndex(
                name: "IX_areas_per_user_user_id",
                table: "areas_per_user");
        }
    }
}
