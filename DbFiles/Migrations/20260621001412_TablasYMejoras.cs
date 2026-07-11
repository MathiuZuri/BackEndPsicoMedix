using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TablasYMejoras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_HistorialesClinicos_HistorialClinicoId",
                table: "Atenciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_Usuarios_UsuarioRegistroId",
                table: "Atenciones");

            migrationBuilder.DropIndex(
                name: "IX_Atenciones_UsuarioRegistroId",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "CostoFinal",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "DiagnosticoResumen",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "Indicaciones",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "MontoPagado",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "MotivoConsulta",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "SaldoPendiente",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "Tratamiento",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "UsuarioRegistroId",
                table: "Atenciones");

            migrationBuilder.AddColumn<bool>(
                name: "DebeCambiarContrasena",
                table: "Usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Pacientes",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<string>(
                name: "CelularPareja",
                table: "Pacientes",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoCivil",
                table: "Pacientes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GradoInstruccion",
                table: "Pacientes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LugarNacimiento",
                table: "Pacientes",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombrePareja",
                table: "Pacientes",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ocupacion",
                table: "Pacientes",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "Pacientes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsConsulta",
                table: "Auditorias",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "HistorialClinicoId",
                table: "Atenciones",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoAtencion",
                table: "Atenciones",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "Anamnesis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MotivoConsulta = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Gestaciones = table.Column<int>(type: "integer", nullable: false),
                    HijosVivos = table.Column<int>(type: "integer", nullable: false),
                    Abortos = table.Column<int>(type: "integer", nullable: false),
                    PartosPretermino = table.Column<int>(type: "integer", nullable: false),
                    PartosATermino = table.Column<int>(type: "integer", nullable: false),
                    FechaUltimaRegla = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaProbableParto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EdadGestacional = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Alergias = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EnfermedadesCronicas = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CirugiasPrevias = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AntecedentesAdicionales = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anamnesis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anamnesis_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcografiasObstetricas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DiametroBiparietal = table.Column<int>(type: "integer", nullable: true),
                    CircunferenciaCefalica = table.Column<int>(type: "integer", nullable: true),
                    CircunferenciaAbdominal = table.Column<int>(type: "integer", nullable: true),
                    LongitudFemur = table.Column<int>(type: "integer", nullable: true),
                    PesoFetalEstimado = table.Column<int>(type: "integer", nullable: true),
                    IndiceLiquidoAmniotico = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    PlacentaLocalizacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PlacentaGranum = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CircularCordon = table.Column<bool>(type: "boolean", nullable: false),
                    Conclusiones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcografiasObstetricas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcografiasObstetricas_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamenesFisicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaHoraExamen = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Lotep = table.Column<bool>(type: "boolean", nullable: false),
                    EstadoGeneral = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EstadoHidratacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EstadoNutricion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EscalaGlasgow = table.Column<int>(type: "integer", nullable: true),
                    UteroGravido = table.Column<bool>(type: "boolean", nullable: false),
                    AlturaUterina = table.Column<int>(type: "integer", nullable: true),
                    SituacionPosicionPresentacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LatidosCardiacosFetales = table.Column<int>(type: "integer", nullable: true),
                    MovimientosFetales = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TonoUterino = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DinamicaUterina = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SangradoTv = table.Column<bool>(type: "boolean", nullable: false),
                    PerdidaLiquidoAmniotico = table.Column<bool>(type: "boolean", nullable: false),
                    ColorLiquidoAmniotico = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaponMucoso = table.Column<bool>(type: "boolean", nullable: false),
                    FlujoVaginal = table.Column<bool>(type: "boolean", nullable: false),
                    PunoPercusionLumbar = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Edemas = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReflejosOsteotendinosos = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamenesFisicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamenesFisicos_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImpresionesDiagnosticas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiagnosticoPrincipal = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DiagnosticosSecundarios = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IndicacionesReceta = table.Column<string>(type: "character varying(2500)", maxLength: 2500, nullable: false),
                    FechaProximaCita = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MotivoProximaCita = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpresionesDiagnosticas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImpresionesDiagnosticas_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TactosVaginales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Dilatacion = table.Column<int>(type: "integer", nullable: true),
                    Borramiento = table.Column<int>(type: "integer", nullable: true),
                    AlturaPresentacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    MembranasOvulares = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ColorLiquido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Pelvis = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VariedadPresentacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TactosVaginales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TactosVaginales_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anamnesis_AtencionId",
                table: "Anamnesis",
                column: "AtencionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EcografiasObstetricas_AtencionId",
                table: "EcografiasObstetricas",
                column: "AtencionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamenesFisicos_AtencionId",
                table: "ExamenesFisicos",
                column: "AtencionId");

            migrationBuilder.CreateIndex(
                name: "IX_ImpresionesDiagnosticas_AtencionId",
                table: "ImpresionesDiagnosticas",
                column: "AtencionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TactosVaginales_AtencionId",
                table: "TactosVaginales",
                column: "AtencionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atenciones_HistorialesClinicos_HistorialClinicoId",
                table: "Atenciones",
                column: "HistorialClinicoId",
                principalTable: "HistorialesClinicos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_HistorialesClinicos_HistorialClinicoId",
                table: "Atenciones");

            migrationBuilder.DropTable(
                name: "Anamnesis");

            migrationBuilder.DropTable(
                name: "EcografiasObstetricas");

            migrationBuilder.DropTable(
                name: "ExamenesFisicos");

            migrationBuilder.DropTable(
                name: "ImpresionesDiagnosticas");

            migrationBuilder.DropTable(
                name: "TactosVaginales");

            migrationBuilder.DropColumn(
                name: "DebeCambiarContrasena",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CelularPareja",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "EstadoCivil",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "GradoInstruccion",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "LugarNacimiento",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "NombrePareja",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Ocupacion",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "EsConsulta",
                table: "Auditorias");

            migrationBuilder.AlterColumn<string>(
                name: "DNI",
                table: "Pacientes",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<Guid>(
                name: "HistorialClinicoId",
                table: "Atenciones",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoAtencion",
                table: "Atenciones",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoFinal",
                table: "Atenciones",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DiagnosticoResumen",
                table: "Atenciones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Indicaciones",
                table: "Atenciones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoPagado",
                table: "Atenciones",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MotivoConsulta",
                table: "Atenciones",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Atenciones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SaldoPendiente",
                table: "Atenciones",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Tratamiento",
                table: "Atenciones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioRegistroId",
                table: "Atenciones",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_UsuarioRegistroId",
                table: "Atenciones",
                column: "UsuarioRegistroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atenciones_HistorialesClinicos_HistorialClinicoId",
                table: "Atenciones",
                column: "HistorialClinicoId",
                principalTable: "HistorialesClinicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Atenciones_Usuarios_UsuarioRegistroId",
                table: "Atenciones",
                column: "UsuarioRegistroId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
