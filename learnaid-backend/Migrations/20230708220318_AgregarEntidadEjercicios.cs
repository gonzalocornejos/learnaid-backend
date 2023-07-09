using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace learnaid_backend.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEntidadEjercicios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjercicioAdaptado_Usuario_UsuarioId",
                table: "EjercicioAdaptado");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "EjercicioAdaptado",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EjercicioAdaptado_Usuario_UsuarioId",
                table: "EjercicioAdaptado",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjercicioAdaptado_Usuario_UsuarioId",
                table: "EjercicioAdaptado");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "EjercicioAdaptado",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_EjercicioAdaptado_Usuario_UsuarioId",
                table: "EjercicioAdaptado",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }
    }
}
