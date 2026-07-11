using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialFinalBackend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_PersonalMedico_PersonalMedicoId",
                table: "Citas");

            migrationBuilder.DropTable(
                name: "PersonalMedico");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_NumeroHC",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Citas_PacienteId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CorreoSecundario",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "LugarNacimiento",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "NumeroHC",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "DuracionMinutos",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "FechaHoraProgramada",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "MotivoConsulta",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Notas",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Servicio",
                table: "Citas");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Pacientes",
                newName: "FechaRegistro");

            migrationBuilder.RenameColumn(
                name: "PersonalMedicoId",
                table: "Citas",
                newName: "UsuarioRegistroId");

            migrationBuilder.RenameIndex(
                name: "IX_Citas_PersonalMedicoId",
                table: "Citas",
                newName: "IX_Citas_UsuarioRegistroId");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoUsuario",
                table: "Usuarios",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Usuarios",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimoAcceso",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Usuarios",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Pacientes",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPaciente",
                table: "Pacientes",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Pacientes",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Pacientes",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Citas",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "CodigoCita",
                table: "Citas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                table: "Citas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateOnly>(
                name: "Fecha",
                table: "Citas",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraFin",
                table: "Citas",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicio",
                table: "Citas",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<Guid>(
                name: "HorarioDoctorId",
                table: "Citas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "Citas",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Citas",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServicioClinicoId",
                table: "Citas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    TipoAccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Modulo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    EntidadAfectada = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntidadId = table.Column<Guid>(type: "uuid", nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ValorAnterior = table.Column<string>(type: "text", nullable: true),
                    ValorNuevo = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    FueExitoso = table.Column<bool>(type: "boolean", nullable: false),
                    DetalleError = table.Column<string>(type: "text", nullable: true),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditorias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Doctores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoDoctor = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CMP = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Especialidad = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Celular = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    FechaInicioContrato = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFinContrato = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctores_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialesClinicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoHistorial = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialesClinicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialesClinicos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Modulo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    EsSistema = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiciosClinicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoServicio = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CostoBase = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    DuracionMinutos = table.Column<int>(type: "integer", nullable: false),
                    RequiereCita = table.Column<bool>(type: "boolean", nullable: false),
                    GeneraHistorial = table.Column<bool>(type: "boolean", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiciosClinicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosDoctor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiaSemana = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    FechaInicioVigencia = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFinVigencia = table.Column<DateOnly>(type: "date", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosDoctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorariosDoctor_Doctores_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolPermisos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RolId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermisoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermisos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolPermisos_Permisos_PermisoId",
                        column: x => x.PermisoId,
                        principalTable: "Permisos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolPermisos_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Atenciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoAtencion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicioClinicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    CitaId = table.Column<Guid>(type: "uuid", nullable: true),
                    HistorialClinicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MotivoConsulta = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    DiagnosticoResumen = table.Column<string>(type: "text", nullable: true),
                    Indicaciones = table.Column<string>(type: "text", nullable: true),
                    Tratamiento = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CostoFinal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MontoPagado = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    SaldoPendiente = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    UsuarioRegistroId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atenciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atenciones_Citas_CitaId",
                        column: x => x.CitaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Atenciones_Doctores_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_HistorialesClinicos_HistorialClinicoId",
                        column: x => x.HistorialClinicoId,
                        principalTable: "HistorialesClinicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_ServiciosClinicos_ServicioClinicoId",
                        column: x => x.ServicioClinicoId,
                        principalTable: "ServiciosClinicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_Usuarios_UsuarioRegistroId",
                        column: x => x.UsuarioRegistroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoPago = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicioClinicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    CitaId = table.Column<Guid>(type: "uuid", nullable: true),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: true),
                    MontoTotal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MontoPagado = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    SaldoPendiente = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MontoAdelanto = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MetodoPago = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaPago = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioRegistroId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pagos_Citas_CitaId",
                        column: x => x.CitaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pagos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_ServiciosClinicos_ServicioClinicoId",
                        column: x => x.ServicioClinicoId,
                        principalTable: "ServiciosClinicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_UsuarioRegistroId",
                        column: x => x.UsuarioRegistroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HistorialDetalles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoDetalle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HistorialClinicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CitaId = table.Column<Guid>(type: "uuid", nullable: true),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: true),
                    PagoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialDetalles_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HistorialDetalles_Citas_CitaId",
                        column: x => x.CitaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HistorialDetalles_HistorialesClinicos_HistorialClinicoId",
                        column: x => x.HistorialClinicoId,
                        principalTable: "HistorialesClinicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialDetalles_Pagos_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pagos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HistorialDetalles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CodigoUsuario",
                table: "Usuarios",
                column: "CodigoUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UserName",
                table: "Usuarios",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_CodigoPaciente",
                table: "Pacientes",
                column: "CodigoPaciente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_CodigoCita",
                table: "Citas",
                column: "CodigoCita",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_DoctorId_Fecha_HoraInicio_HoraFin",
                table: "Citas",
                columns: new[] { "DoctorId", "Fecha", "HoraInicio", "HoraFin" });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_HorarioDoctorId",
                table: "Citas",
                column: "HorarioDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PacienteId_Fecha",
                table: "Citas",
                columns: new[] { "PacienteId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_ServicioClinicoId",
                table: "Citas",
                column: "ServicioClinicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_CitaId",
                table: "Atenciones",
                column: "CitaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_CodigoAtencion",
                table: "Atenciones",
                column: "CodigoAtencion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_DoctorId",
                table: "Atenciones",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_HistorialClinicoId",
                table: "Atenciones",
                column: "HistorialClinicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_PacienteId",
                table: "Atenciones",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_ServicioClinicoId",
                table: "Atenciones",
                column: "ServicioClinicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_UsuarioRegistroId",
                table: "Atenciones",
                column: "UsuarioRegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_FechaHora",
                table: "Auditorias",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_TipoAccion",
                table: "Auditorias",
                column: "TipoAccion");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_UsuarioId",
                table: "Auditorias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctores_CMP",
                table: "Doctores",
                column: "CMP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctores_CodigoDoctor",
                table: "Doctores",
                column: "CodigoDoctor",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctores_UsuarioId",
                table: "Doctores",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDetalles_AtencionId",
                table: "HistorialDetalles",
                column: "AtencionId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDetalles_CitaId",
                table: "HistorialDetalles",
                column: "CitaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDetalles_CodigoDetalle",
                table: "HistorialDetalles",
                column: "CodigoDetalle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDetalles_FechaRegistro",
                table: "HistorialDetalles",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDetalles_HistorialClinicoId",
                table: "HistorialDetalles",
                column: "HistorialClinicoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDetalles_PagoId",
                table: "HistorialDetalles",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDetalles_TipoMovimiento",
                table: "HistorialDetalles",
                column: "TipoMovimiento");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialDetalles_UsuarioId",
                table: "HistorialDetalles",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesClinicos_CodigoHistorial",
                table: "HistorialesClinicos",
                column: "CodigoHistorial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesClinicos_PacienteId",
                table: "HistorialesClinicos",
                column: "PacienteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorariosDoctor_DoctorId_DiaSemana_HoraInicio_HoraFin_FechaI~",
                table: "HorariosDoctor",
                columns: new[] { "DoctorId", "DiaSemana", "HoraInicio", "HoraFin", "FechaInicioVigencia" });

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_AtencionId",
                table: "Pagos",
                column: "AtencionId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CitaId",
                table: "Pagos",
                column: "CitaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CodigoPago",
                table: "Pagos",
                column: "CodigoPago",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_FechaPago",
                table: "Pagos",
                column: "FechaPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_PacienteId",
                table: "Pagos",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ServicioClinicoId",
                table: "Pagos",
                column: "ServicioClinicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_UsuarioRegistroId",
                table: "Pagos",
                column: "UsuarioRegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Codigo",
                table: "Permisos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Nombre",
                table: "Roles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolPermisos_PermisoId",
                table: "RolPermisos",
                column: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermisos_RolId_PermisoId",
                table: "RolPermisos",
                columns: new[] { "RolId", "PermisoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosClinicos_CodigoServicio",
                table: "ServiciosClinicos",
                column: "CodigoServicio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRoles_RolId",
                table: "UsuarioRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRoles_UsuarioId_RolId",
                table: "UsuarioRoles",
                columns: new[] { "UsuarioId", "RolId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Doctores_DoctorId",
                table: "Citas",
                column: "DoctorId",
                principalTable: "Doctores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_HorariosDoctor_HorarioDoctorId",
                table: "Citas",
                column: "HorarioDoctorId",
                principalTable: "HorariosDoctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_ServiciosClinicos_ServicioClinicoId",
                table: "Citas",
                column: "ServicioClinicoId",
                principalTable: "ServiciosClinicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Usuarios_UsuarioRegistroId",
                table: "Citas",
                column: "UsuarioRegistroId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Doctores_DoctorId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Citas_HorariosDoctor_HorarioDoctorId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Citas_ServiciosClinicos_ServicioClinicoId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Usuarios_UsuarioRegistroId",
                table: "Citas");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "HistorialDetalles");

            migrationBuilder.DropTable(
                name: "HorariosDoctor");

            migrationBuilder.DropTable(
                name: "RolPermisos");

            migrationBuilder.DropTable(
                name: "UsuarioRoles");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Atenciones");

            migrationBuilder.DropTable(
                name: "Doctores");

            migrationBuilder.DropTable(
                name: "HistorialesClinicos");

            migrationBuilder.DropTable(
                name: "ServiciosClinicos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_CodigoUsuario",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_UserName",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_CodigoPaciente",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Citas_CodigoCita",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_DoctorId_Fecha_HoraInicio_HoraFin",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_HorarioDoctorId",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_PacienteId_Fecha",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_ServicioClinicoId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoUsuario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UltimoAcceso",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoPaciente",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "CodigoCita",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "HorarioDoctorId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "ServicioClinicoId",
                table: "Citas");

            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "Pacientes",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "UsuarioRegistroId",
                table: "Citas",
                newName: "PersonalMedicoId");

            migrationBuilder.RenameIndex(
                name: "IX_Citas_UsuarioRegistroId",
                table: "Citas",
                newName: "IX_Citas_PersonalMedicoId");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Pacientes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorreoSecundario",
                table: "Pacientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LugarNacimiento",
                table: "Pacientes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroHC",
                table: "Pacientes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Citas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "DuracionMinutos",
                table: "Citas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraProgramada",
                table: "Citas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoConsulta",
                table: "Citas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notas",
                table: "Citas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Servicio",
                table: "Citas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PersonalMedico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Apellidos = table.Column<string>(type: "text", nullable: false),
                    CMP = table.Column<string>(type: "text", nullable: false),
                    Especialidad = table.Column<string>(type: "text", nullable: false),
                    Nombres = table.Column<string>(type: "text", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalMedico_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_NumeroHC",
                table: "Pacientes",
                column: "NumeroHC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PacienteId",
                table: "Citas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalMedico_UsuarioId",
                table: "PersonalMedico",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_PersonalMedico_PersonalMedicoId",
                table: "Citas",
                column: "PersonalMedicoId",
                principalTable: "PersonalMedico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
