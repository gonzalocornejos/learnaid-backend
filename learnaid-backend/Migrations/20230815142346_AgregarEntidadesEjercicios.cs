using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace learnaid_backend.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEntidadesEjercicios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EjercitacionNoAdaptada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Idioma = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjercitacionNoAdaptada", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profesion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EjercicioNoAdaptado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Consigna = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ejercicio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EjercitacionNoAdaptadaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjercicioNoAdaptado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EjercicioNoAdaptado_EjercitacionNoAdaptada_EjercitacionNoAdaptadaId",
                        column: x => x.EjercitacionNoAdaptadaId,
                        principalTable: "EjercitacionNoAdaptada",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EjercitacionAdaptada",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EjercicioOriginalId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjercitacionAdaptada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EjercitacionAdaptada_EjercitacionNoAdaptada_EjercicioOriginalId",
                        column: x => x.EjercicioOriginalId,
                        principalTable: "EjercitacionNoAdaptada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EjercitacionAdaptada_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Adaptaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adaptacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EjercicioNoAdaptadoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adaptaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adaptaciones_EjercicioNoAdaptado_EjercicioNoAdaptadoId",
                        column: x => x.EjercicioNoAdaptadoId,
                        principalTable: "EjercicioNoAdaptado",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EjercicioAdaptado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Consigna = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ejercicio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EjercitacionAdaptadaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjercicioAdaptado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EjercicioAdaptado_EjercitacionAdaptada_EjercitacionAdaptadaId",
                        column: x => x.EjercitacionAdaptadaId,
                        principalTable: "EjercitacionAdaptada",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adaptaciones_EjercicioNoAdaptadoId",
                table: "Adaptaciones",
                column: "EjercicioNoAdaptadoId");

            migrationBuilder.CreateIndex(
                name: "IX_EjercicioAdaptado_EjercitacionAdaptadaId",
                table: "EjercicioAdaptado",
                column: "EjercitacionAdaptadaId");

            migrationBuilder.CreateIndex(
                name: "IX_EjercicioNoAdaptado_EjercitacionNoAdaptadaId",
                table: "EjercicioNoAdaptado",
                column: "EjercitacionNoAdaptadaId");

            migrationBuilder.CreateIndex(
                name: "IX_EjercitacionAdaptada_EjercicioOriginalId",
                table: "EjercitacionAdaptada",
                column: "EjercicioOriginalId");

            migrationBuilder.CreateIndex(
                name: "IX_EjercitacionAdaptada_UsuarioId",
                table: "EjercitacionAdaptada",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adaptaciones");

            migrationBuilder.DropTable(
                name: "EjercicioAdaptado");

            migrationBuilder.DropTable(
                name: "EjercicioNoAdaptado");

            migrationBuilder.DropTable(
                name: "EjercitacionAdaptada");

            migrationBuilder.DropTable(
                name: "EjercitacionNoAdaptada");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
