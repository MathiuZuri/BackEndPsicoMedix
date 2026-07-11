using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarComprobantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comprobantes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoComprobante = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Serie = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    TipoComprobante = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Estado = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    FormatoImpresion = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    PagoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CitaId = table.Column<Guid>(type: "uuid", nullable: true),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: true),
                    HistorialClinicoId = table.Column<Guid>(type: "uuid", nullable: true),
                    TipoDocumentoPaciente = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    NumeroDocumentoPaciente = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NombrePaciente = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DireccionPaciente = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Subtotal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TasaImpuesto = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    MontoImpuesto = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioEmisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaAnulacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioAnulacionId = table.Column<Guid>(type: "uuid", nullable: true),
                    MotivoAnulacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DatosSnapshotJson = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comprobantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Citas_CitaId",
                        column: x => x.CitaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comprobantes_HistorialesClinicos_HistorialClinicoId",
                        column: x => x.HistorialClinicoId,
                        principalTable: "HistorialesClinicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Pagos_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pagos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Usuarios_UsuarioAnulacionId",
                        column: x => x.UsuarioAnulacionId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Usuarios_UsuarioEmisionId",
                        column: x => x.UsuarioEmisionId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteDetalles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ComprobanteId = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoServicio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    PrecioUnitarioFinal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TasaImpuesto = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    MontoImpuesto = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteDetalles_Comprobantes_ComprobanteId",
                        column: x => x.ComprobanteId,
                        principalTable: "Comprobantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteDetalles_ComprobanteId",
                table: "ComprobanteDetalles",
                column: "ComprobanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_AtencionId",
                table: "Comprobantes",
                column: "AtencionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_CitaId",
                table: "Comprobantes",
                column: "CitaId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_CodigoComprobante",
                table: "Comprobantes",
                column: "CodigoComprobante",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_Estado",
                table: "Comprobantes",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_FechaEmision",
                table: "Comprobantes",
                column: "FechaEmision");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_HistorialClinicoId",
                table: "Comprobantes",
                column: "HistorialClinicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_PacienteId",
                table: "Comprobantes",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_PagoId",
                table: "Comprobantes",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_Serie_Numero_TipoComprobante",
                table: "Comprobantes",
                columns: new[] { "Serie", "Numero", "TipoComprobante" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_UsuarioAnulacionId",
                table: "Comprobantes",
                column: "UsuarioAnulacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_UsuarioEmisionId",
                table: "Comprobantes",
                column: "UsuarioEmisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_UsuarioId",
                table: "Comprobantes",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComprobanteDetalles");

            migrationBuilder.DropTable(
                name: "Comprobantes");
        }
    }
}
