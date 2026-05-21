using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PresupuestoMVC.Migrations
{
    /// <inheritdoc />
    public partial class CreateLocalidadYProvincia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "provincias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),

                    date_inserted = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provincias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "localidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),

                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),

                    codigo_postal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),

                    provincia_id = table.Column<int>(type: "integer", nullable: false),

                    date_inserted = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localidades", x => x.id);

                    table.ForeignKey(
                        name: "FK_localidades_provincias_provincia_id",
                        column: x => x.provincia_id,
                        principalTable: "provincias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_localidades_provincia_id",
                table: "localidades",
                column: "provincia_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "localidades");

            migrationBuilder.DropTable(
                name: "provincias");
        }
    }
}
