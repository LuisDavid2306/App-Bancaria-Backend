using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Bancaria_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioSolicitanteManual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioSolicitante",
                table: "Retiro",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Retiro_IdUsuarioSolicitante",
                table: "Retiro",
                column: "IdUsuarioSolicitante");

            migrationBuilder.AddForeignKey(
                name: "FK_Retiro_Usuario_IdUsuarioSolicitante",
                table: "Retiro",
                column: "IdUsuarioSolicitante",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Retiro_Usuario_IdUsuarioSolicitante",
                table: "Retiro");

            migrationBuilder.DropIndex(
                name: "IX_Retiro_IdUsuarioSolicitante",
                table: "Retiro");

            migrationBuilder.DropColumn(
                name: "IdUsuarioSolicitante",
                table: "Retiro");
        }
    }
}
