using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableAndColumnsVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articulos_marca_brend",
                table: "articulos");

            migrationBuilder.DropForeignKey(
                name: "FK_Articulos_proveedorId_provider",
                table: "articulos");

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF to_regclass('"articulos_precios"') IS NOT NULL THEN
                        ALTER TABLE "articulos_precios"
                            DROP CONSTRAINT IF EXISTS "articulos_precios_articulo_id_fkey";
                        ALTER TABLE "articulos_precios"
                            DROP CONSTRAINT IF EXISTS "articulos_precios_lista_precio_id_fkey";
                    END IF;
                END $$;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_listas_precios_lista_precio_id",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "fk_income_transfers_to_account_id_account",
                table: "income_transfers");

            migrationBuilder.DropPrimaryKey(
                name: "Clientes_pkey",
                table: "Clientes");

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF to_regclass('"articulos_precios"') IS NOT NULL THEN
                        ALTER TABLE "articulos_precios"
                            DROP CONSTRAINT IF EXISTS "articulos_precios_pkey";
                    END IF;
                END $$;
                """);

            migrationBuilder.DropPrimaryKey(
                name: "articulos_pkey",
                table: "articulos");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "client");

            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF to_regclass('"articulos_precios"') IS NOT NULL
                       AND to_regclass('articles_prices') IS NULL THEN
                        ALTER TABLE "articulos_precios" RENAME TO articles_prices;
                    END IF;
                END $$;
                """);

            migrationBuilder.RenameTable(
                name: "articulos",
                newName: "articles");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "product_category",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "descripcion",
                table: "listas_precios",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "activo",
                table: "income_transfers",
                newName: "active");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "brand",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "client",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "DNI",
                table: "client",
                newName: "dni");

            migrationBuilder.RenameColumn(
                name: "CUIT",
                table: "client",
                newName: "cuit");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "client",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "client",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Provincia",
                table: "client",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "OperacionesContado",
                table: "client",
                newName: "cash_pperations");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "client",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Localidad",
                table: "client",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "InhabilitadoFacturar",
                table: "client",
                newName: "is_billing_disabled");

            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "client",
                newName: "date_registration");

            migrationBuilder.RenameColumn(
                name: "Fantasia",
                table: "client",
                newName: "nick_name");

            migrationBuilder.RenameColumn(
                name: "Domicilio",
                table: "client",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "CondicionDeVenta",
                table: "client",
                newName: "sale_condition");

            migrationBuilder.RenameColumn(
                name: "CodigoPostal",
                table: "client",
                newName: "zip_code");

            migrationBuilder.RenameColumn(
                name: "Celular",
                table: "client",
                newName: "mobile_phone");

            migrationBuilder.RenameColumn(
                name: "Categoria",
                table: "client",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "client",
                newName: "active");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_lista_precio_id",
                table: "client",
                newName: "IX_client_lista_precio_id");

            // migrationBuilder.RenameIndex(
            //     name: "IX_articulos_precios_lista_precio_id",
            //     table: "articles_prices",
            //     newName: "IX_articles_prices_price_list_id");

            // migrationBuilder.RenameIndex(
            //     name: "IX_articulos_precios_articulo_id",
            //     table: "articles_prices",
            //     newName: "IX_articles_prices_article_id");

            migrationBuilder.RenameColumn(
                name: "unidad_medida",
                table: "articles",
                newName: "unit_measure");

            migrationBuilder.RenameColumn(
                name: "rubroId",
                table: "articles",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "proveedorId",
                table: "articles",
                newName: "provider_id");

            migrationBuilder.RenameColumn(
                name: "precio_venta",
                table: "articles",
                newName: "sale_price");

            migrationBuilder.RenameColumn(
                name: "precio_compra",
                table: "articles",
                newName: "purchase_price");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "articles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "margen",
                table: "articles",
                newName: "margin");

            migrationBuilder.RenameColumn(
                name: "marcaId",
                table: "articles",
                newName: "brand_id");

            migrationBuilder.RenameColumn(
                name: "codigo",
                table: "articles",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "activo",
                table: "articles",
                newName: "active");

            //migrationBuilder.RenameIndex(
            //    name: "IX_articulos_rubroId",
            //    table: "articles",
            //    newName: "IX_articles_category_id");

            //migrationBuilder.RenameIndex(
            //    name: "IX_articulos_proveedorId",
            //    table: "articles",
            //    newName: "IX_articles_provider_id");

            //migrationBuilder.RenameIndex(
            //    name: "IX_articulos_marcaId",
            //    table: "articles",
            //    newName: "IX_articles_brand_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_client",
                table: "client",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_articles",
                table: "articles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_articles_brand_brand_id",
                table: "articles",
                column: "brand_id",
                principalTable: "brand",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articles_product_category_category_id",
                table: "articles",
                column: "category_id",
                principalTable: "product_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articles_provider_provider_id",
                table: "articles",
                column: "provider_id",
                principalTable: "provider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_client_listas_precios_lista_precio_id",
                table: "client",
                column: "lista_precio_id",
                principalTable: "listas_precios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_income_transfers_to_account_id_account",
                table: "income_transfers",
                column: "to_account_id",
                principalTable: "account",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_articles_brand_brand_id",
                table: "articles");

            migrationBuilder.DropForeignKey(
                name: "FK_articles_product_category_category_id",
                table: "articles");

            migrationBuilder.DropForeignKey(
                name: "FK_articles_provider_provider_id",
                table: "articles");

            migrationBuilder.DropForeignKey(
                name: "FK_client_listas_precios_lista_precio_id",
                table: "client");

            migrationBuilder.DropForeignKey(
                name: "fk_income_transfers_to_account_id_account",
                table: "income_transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_client",
                table: "client");

            migrationBuilder.DropPrimaryKey(
                name: "PK_articles",
                table: "articles");

            migrationBuilder.RenameTable(
                name: "client",
                newName: "Clientes");

            migrationBuilder.RenameTable(
                name: "articles",
                newName: "articulos");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "product_category",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "listas_precios",
                newName: "descripcion");

            migrationBuilder.RenameColumn(
                name: "active",
                table: "income_transfers",
                newName: "activo");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "brand",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Clientes",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "dni",
                table: "Clientes",
                newName: "DNI");

            migrationBuilder.RenameColumn(
                name: "cuit",
                table: "Clientes",
                newName: "CUIT");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Clientes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "zip_code",
                table: "Clientes",
                newName: "CodigoPostal");

            migrationBuilder.RenameColumn(
                name: "sale_condition",
                table: "Clientes",
                newName: "CondicionDeVenta");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Clientes",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "nick_name",
                table: "Clientes",
                newName: "Fantasia");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Clientes",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "mobile_phone",
                table: "Clientes",
                newName: "Celular");

            migrationBuilder.RenameColumn(
                name: "is_billing_disabled",
                table: "Clientes",
                newName: "InhabilitadoFacturar");

            migrationBuilder.RenameColumn(
                name: "date_registration",
                table: "Clientes",
                newName: "FechaRegistro");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "Clientes",
                newName: "Localidad");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "Clientes",
                newName: "Categoria");

            migrationBuilder.RenameColumn(
                name: "cash_pperations",
                table: "Clientes",
                newName: "OperacionesContado");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Clientes",
                newName: "Domicilio");

            migrationBuilder.RenameColumn(
                name: "active",
                table: "Clientes",
                newName: "Activo");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Clientes",
                newName: "Provincia");

            migrationBuilder.RenameIndex(
                name: "IX_client_lista_precio_id",
                table: "Clientes",
                newName: "IX_Clientes_lista_precio_id");

            migrationBuilder.RenameColumn(
                name: "price_list_id",
                table: "articulos_precios",
                newName: "lista_precio_id");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "articulos_precios",
                newName: "precio");

            migrationBuilder.RenameColumn(
                name: "article_id",
                table: "articulos_precios",
                newName: "articulo_id");

            migrationBuilder.RenameColumn(
                name: "unit_measure",
                table: "articulos",
                newName: "unidad_medida");

            migrationBuilder.RenameColumn(
                name: "sale_price",
                table: "articulos",
                newName: "precio_venta");

            migrationBuilder.RenameColumn(
                name: "purchase_price",
                table: "articulos",
                newName: "precio_compra");

            migrationBuilder.RenameColumn(
                name: "provider_id",
                table: "articulos",
                newName: "proveedorId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "articulos",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "margin",
                table: "articulos",
                newName: "margen");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "articulos",
                newName: "codigo");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "articulos",
                newName: "rubroId");

            migrationBuilder.RenameColumn(
                name: "brand_id",
                table: "articulos",
                newName: "marcaId");

            migrationBuilder.RenameColumn(
                name: "active",
                table: "articulos",
                newName: "activo");

            migrationBuilder.RenameIndex(
                name: "IX_articles_provider_id",
                table: "articulos",
                newName: "IX_articulos_proveedorId");

            migrationBuilder.RenameIndex(
                name: "IX_articles_category_id",
                table: "articulos",
                newName: "IX_articulos_rubroId");

            migrationBuilder.RenameIndex(
                name: "IX_articles_brand_id",
                table: "articulos",
                newName: "IX_articulos_marcaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "articulos_precios_pkey",
                table: "articulos_precios",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "articulos_pkey",
                table: "articulos",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articulos_marca_brend",
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
                name: "FK_Clientes_listas_precios_lista_precio_id",
                table: "Clientes",
                column: "lista_precio_id",
                principalTable: "listas_precios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_income_transfers_account_to_account_id",
                table: "income_transfers",
                column: "to_account_id",
                principalTable: "account",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
