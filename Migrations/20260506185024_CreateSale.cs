using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class CreateSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: true),
                    name_client = table.Column<string>(type: "text", nullable: false),
                    dni = table.Column<string>(type: "text", nullable: false),
                    price_list_id = table.Column<int>(type: "integer", nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    descuento = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    date_inserted = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sale_detalles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sale_id = table.Column<int>(type: "integer", nullable: false),
                    item_id = table.Column<int>(type: "integer", nullable: false),
                    code_item = table.Column<string>(type: "text", nullable: false),
                    name_item = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_detalles", x => x.id);
                    table.ForeignKey(
                        name: "FK_sale_detalles_sales_sale_id",
                        column: x => x.sale_id,
                        principalTable: "sales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sale_detalles_sale_id",
                table: "sale_detalles",
                column: "sale_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sale_detalles");

            migrationBuilder.DropTable(
                name: "sales");
        }
    }
}
