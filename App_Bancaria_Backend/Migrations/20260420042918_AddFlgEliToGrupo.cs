using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Bancaria_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddFlgEliToGrupo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FlgEli",
                table: "GrupoAhorro",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlgEli",
                table: "GrupoAhorro");
        }
    }
}
