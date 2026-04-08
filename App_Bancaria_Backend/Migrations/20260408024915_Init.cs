using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Bancaria_Backend.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrupoAhorro",
                columns: table => new
                {
                    IdGrupo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodGrupo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoObjetivo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MontoActual = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoAhorro", x => x.IdGrupo);
                });

            migrationBuilder.CreateTable(
                name: "TipoTransaccion",
                columns: table => new
                {
                    IdTipoTransaccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodTipoTransaccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTransaccion", x => x.IdTipoTransaccion);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodUsuario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApePat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApeMate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NroTelefono = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    FlgEstado = table.Column<bool>(type: "bit", nullable: false),
                    FlgEli = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Retiro",
                columns: table => new
                {
                    IdRetiro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodRetiro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdGrupo = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retiro", x => x.IdRetiro);
                    table.ForeignKey(
                        name: "FK_Retiro_GrupoAhorro_IdGrupo",
                        column: x => x.IdGrupo,
                        principalTable: "GrupoAhorro",
                        principalColumn: "IdGrupo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta",
                columns: table => new
                {
                    IdCuenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodCuenta = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CodQR = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta", x => x.IdCuenta);
                    table.ForeignKey(
                        name: "FK_Cuenta_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrupoAdministrador",
                columns: table => new
                {
                    IdGrupoAdmin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodGrupoAdmin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdGrupo = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoAdministrador", x => x.IdGrupoAdmin);
                    table.ForeignKey(
                        name: "FK_GrupoAdministrador_GrupoAhorro_IdGrupo",
                        column: x => x.IdGrupo,
                        principalTable: "GrupoAhorro",
                        principalColumn: "IdGrupo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupoAdministrador_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrupoMiembro",
                columns: table => new
                {
                    IdGrupoMiembro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodGrupoMiembro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdGrupo = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    FechaUnion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoMiembro", x => x.IdGrupoMiembro);
                    table.ForeignKey(
                        name: "FK_GrupoMiembro_GrupoAhorro_IdGrupo",
                        column: x => x.IdGrupo,
                        principalTable: "GrupoAhorro",
                        principalColumn: "IdGrupo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupoMiembro_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RetiroAprobacion",
                columns: table => new
                {
                    IdAprobacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodAprobacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdRetiro = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Aprobado = table.Column<bool>(type: "bit", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetiroAprobacion", x => x.IdAprobacion);
                    table.ForeignKey(
                        name: "FK_RetiroAprobacion_Retiro_IdRetiro",
                        column: x => x.IdRetiro,
                        principalTable: "Retiro",
                        principalColumn: "IdRetiro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RetiroAprobacion_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaccion",
                columns: table => new
                {
                    IdTransaccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodTransaccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCuentaOrigen = table.Column<int>(type: "int", nullable: false),
                    IdCuentaDestino = table.Column<int>(type: "int", nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IdTipoTransaccion = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion", x => x.IdTransaccion);
                    table.ForeignKey(
                        name: "FK_Transaccion_Cuenta_IdCuentaDestino",
                        column: x => x.IdCuentaDestino,
                        principalTable: "Cuenta",
                        principalColumn: "IdCuenta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaccion_Cuenta_IdCuentaOrigen",
                        column: x => x.IdCuentaOrigen,
                        principalTable: "Cuenta",
                        principalColumn: "IdCuenta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaccion_TipoTransaccion_IdTipoTransaccion",
                        column: x => x.IdTipoTransaccion,
                        principalTable: "TipoTransaccion",
                        principalColumn: "IdTipoTransaccion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aporte",
                columns: table => new
                {
                    IdAporte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodAporte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdTransaccion = table.Column<int>(type: "int", nullable: false),
                    IdGrupo = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aporte", x => x.IdAporte);
                    table.ForeignKey(
                        name: "FK_Aporte_GrupoAhorro_IdGrupo",
                        column: x => x.IdGrupo,
                        principalTable: "GrupoAhorro",
                        principalColumn: "IdGrupo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aporte_Transaccion_IdTransaccion",
                        column: x => x.IdTransaccion,
                        principalTable: "Transaccion",
                        principalColumn: "IdTransaccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aporte_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aporte_IdGrupo",
                table: "Aporte",
                column: "IdGrupo");

            migrationBuilder.CreateIndex(
                name: "IX_Aporte_IdTransaccion",
                table: "Aporte",
                column: "IdTransaccion");

            migrationBuilder.CreateIndex(
                name: "IX_Aporte_IdUsuario",
                table: "Aporte",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_CodCuenta",
                table: "Cuenta",
                column: "CodCuenta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_CodQR",
                table: "Cuenta",
                column: "CodQR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_IdUsuario",
                table: "Cuenta",
                column: "IdUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAdministrador_IdGrupo",
                table: "GrupoAdministrador",
                column: "IdGrupo");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAdministrador_IdUsuario",
                table: "GrupoAdministrador",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMiembro_IdGrupo",
                table: "GrupoMiembro",
                column: "IdGrupo");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoMiembro_IdUsuario",
                table: "GrupoMiembro",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Retiro_IdGrupo",
                table: "Retiro",
                column: "IdGrupo");

            migrationBuilder.CreateIndex(
                name: "IX_RetiroAprobacion_IdRetiro",
                table: "RetiroAprobacion",
                column: "IdRetiro");

            migrationBuilder.CreateIndex(
                name: "IX_RetiroAprobacion_IdUsuario",
                table: "RetiroAprobacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_IdCuentaDestino",
                table: "Transaccion",
                column: "IdCuentaDestino");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_IdCuentaOrigen",
                table: "Transaccion",
                column: "IdCuentaOrigen");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_IdTipoTransaccion",
                table: "Transaccion",
                column: "IdTipoTransaccion");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_CodUsuario",
                table: "Usuario",
                column: "CodUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NroTelefono",
                table: "Usuario",
                column: "NroTelefono",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aporte");

            migrationBuilder.DropTable(
                name: "GrupoAdministrador");

            migrationBuilder.DropTable(
                name: "GrupoMiembro");

            migrationBuilder.DropTable(
                name: "RetiroAprobacion");

            migrationBuilder.DropTable(
                name: "Transaccion");

            migrationBuilder.DropTable(
                name: "Retiro");

            migrationBuilder.DropTable(
                name: "Cuenta");

            migrationBuilder.DropTable(
                name: "TipoTransaccion");

            migrationBuilder.DropTable(
                name: "GrupoAhorro");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
