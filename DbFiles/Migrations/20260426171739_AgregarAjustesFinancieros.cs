using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAjustesFinancieros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AjustesFinancieros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PagoId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: true),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoAjuste = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MontoAjuste = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UsuarioRegistroId = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AjustesFinancieros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AjustesFinancieros_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AjustesFinancieros_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AjustesFinancieros_Pagos_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pagos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AjustesFinancieros_Usuarios_UsuarioRegistroId",
                        column: x => x.UsuarioRegistroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AjustesFinancieros_AtencionId",
                table: "AjustesFinancieros",
                column: "AtencionId");

            migrationBuilder.CreateIndex(
                name: "IX_AjustesFinancieros_FechaRegistro",
                table: "AjustesFinancieros",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_AjustesFinancieros_PacienteId",
                table: "AjustesFinancieros",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_AjustesFinancieros_PagoId",
                table: "AjustesFinancieros",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_AjustesFinancieros_TipoAjuste",
                table: "AjustesFinancieros",
                column: "TipoAjuste");

            migrationBuilder.CreateIndex(
                name: "IX_AjustesFinancieros_UsuarioRegistroId",
                table: "AjustesFinancieros",
                column: "UsuarioRegistroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AjustesFinancieros");
        }
    }
}
