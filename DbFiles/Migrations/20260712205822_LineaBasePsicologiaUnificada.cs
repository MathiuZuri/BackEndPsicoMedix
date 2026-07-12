using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace psicomedixMonolito.Migrations
{
    /// <inheritdoc />
    public partial class LineaBasePsicologiaUnificada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoUsuario = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DebeCambiarContrasena = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
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
                    Nivel = table.Column<int>(type: "integer", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EsConsulta = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoPaciente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DNI = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FechaAtencion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TieneInformante = table.Column<bool>(type: "boolean", nullable: true),
                    InformanteNombre = table.Column<string>(type: "text", nullable: true),
                    InformanteParentesco = table.Column<string>(type: "text", nullable: true),
                    InformanteOcupacion = table.Column<string>(type: "text", nullable: true),
                    InformanteEstadoCivil = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    InformanteEdad = table.Column<int>(type: "integer", nullable: true),
                    InformanteCelular = table.Column<string>(type: "text", nullable: true),
                    PrimeraAtencionPsicologia = table.Column<bool>(type: "boolean", nullable: true),
                    Nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Apellidos = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Genero = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    EdadAnos = table.Column<int>(type: "integer", nullable: true),
                    EdadMeses = table.Column<int>(type: "integer", nullable: true),
                    Celular = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstadoCivil = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    LugarNacimiento = table.Column<string>(type: "text", nullable: true),
                    Ocupacion = table.Column<string>(type: "text", nullable: true),
                    GradoInstruccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    GradoSemestreCiclo = table.Column<string>(type: "text", nullable: true),
                    Carrera = table.Column<string>(type: "text", nullable: true),
                    TieneMasHermanosHijos = table.Column<bool>(type: "boolean", nullable: true),
                    TieneMediosHermanosPadrastros = table.Column<bool>(type: "boolean", nullable: true),
                    IntegrantesFamilia = table.Column<int>(type: "integer", nullable: true),
                    Religion = table.Column<string>(type: "text", nullable: true),
                    PesoKg = table.Column<decimal>(type: "numeric", nullable: true),
                    TallaMetros = table.Column<decimal>(type: "numeric", nullable: true),
                    IMC = table.Column<decimal>(type: "numeric", nullable: true),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: true),
                    OtrasEspecialidadesRaw = table.Column<string>(type: "text", nullable: true),
                    ConsultaAsuntosLegales = table.Column<bool>(type: "boolean", nullable: true),
                    MotivoConsulta = table.Column<string>(type: "text", nullable: true),
                    RecepcionadoPorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pacientes_Doctores_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pacientes_Usuarios_RecepcionadoPorId",
                        column: x => x.RecepcionadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pacientes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TelefonoWhatsApp = table.Column<string>(type: "text", nullable: false),
                    NombreContacto = table.Column<string>(type: "text", nullable: false),
                    UltimoMensaje = table.Column<string>(type: "text", nullable: false),
                    FechaUltimaInteraccion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MensajesNoLeidos = table.Column<int>(type: "integer", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoCita = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicioClinicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    HorarioDoctorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioRegistroId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Citas_Doctores_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_HorariosDoctor_HorarioDoctorId",
                        column: x => x.HorarioDoctorId,
                        principalTable: "HorariosDoctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Citas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_ServiciosClinicos_ServicioClinicoId",
                        column: x => x.ServicioClinicoId,
                        principalTable: "ServiciosClinicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citas_Usuarios_UsuarioRegistroId",
                        column: x => x.UsuarioRegistroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                name: "PacienteFamiliares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Parentesco = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Edad = table.Column<int>(type: "integer", nullable: true),
                    Ocupacion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    VivenJuntos = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacienteFamiliares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PacienteFamiliares_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MensajesChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Texto = table.Column<string>(type: "text", nullable: false),
                    EsMio = table.Column<bool>(type: "boolean", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MessageIdWhatsApp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensajesChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MensajesChat_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificacionesCitas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CitaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TelefonoDestino = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Canal = table.Column<int>(type: "integer", nullable: false),
                    Mensaje = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FechaProgramadaEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    Intentos = table.Column<int>(type: "integer", nullable: false),
                    Error = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacionesCitas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificacionesCitas_Citas_CitaId",
                        column: x => x.CitaId,
                        principalTable: "Citas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificacionesCitas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Atenciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoAtencion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uuid", nullable: false),
                    HistorialClinicoId = table.Column<Guid>(type: "uuid", nullable: true),
                    ServicioClinicoId = table.Column<Guid>(type: "uuid", nullable: false),
                    CitaId = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ObservacionesIniciales = table.Column<string>(type: "text", nullable: true)
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
                        principalColumn: "Id");
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
                name: "PsicoAnamnesisHistoriales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SustanciasNotasGenerales = table.Column<string>(type: "text", nullable: true),
                    SustanciasLegales = table.Column<string>(type: "text", nullable: true),
                    ConsumoOH = table.Column<string>(type: "text", nullable: true),
                    CigarrillosVape = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SustanciasNoLegales = table.Column<string>(type: "text", nullable: true),
                    Medicamentos = table.Column<string>(type: "text", nullable: true),
                    Suplementos = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EnfermedadesAccidentesNotasGenerales = table.Column<string>(type: "text", nullable: true),
                    Enfermedades = table.Column<string>(type: "text", nullable: true),
                    Accidentes = table.Column<string>(type: "text", nullable: true),
                    Cirugias = table.Column<string>(type: "text", nullable: true),
                    Hospitalizacion = table.Column<string>(type: "text", nullable: true),
                    FamiliaresAntecedentesRelacionados = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsicoAnamnesisHistoriales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsicoAnamnesisHistoriales_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsicoDesarrollosPsicosociales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AutoestimaAutocuidado = table.Column<string>(type: "text", nullable: true),
                    AcademicoLaboral = table.Column<string>(type: "text", nullable: true),
                    SocializacionFamilia = table.Column<string>(type: "text", nullable: true),
                    PersonalidadAutoexpresion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsicoDesarrollosPsicosociales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsicoDesarrollosPsicosociales_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsicoDiagnosticosCierres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiagnosticoDiferencial1 = table.Column<string>(type: "text", nullable: true),
                    DiagnosticoDiferencial2 = table.Column<string>(type: "text", nullable: true),
                    DiagnosticoDiferencial3 = table.Column<string>(type: "text", nullable: true),
                    ImpresionDiagnostica = table.Column<string>(type: "text", nullable: true),
                    Recomendaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsicoDiagnosticosCierres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsicoDiagnosticosCierres_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsicoEscalasAnimos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    EscalaIrritabilidad = table.Column<int>(type: "integer", nullable: true),
                    EscalaTristeza = table.Column<int>(type: "integer", nullable: true),
                    EscalaAnsiedad = table.Column<int>(type: "integer", nullable: true),
                    EscalaPreocupacion = table.Column<int>(type: "integer", nullable: true),
                    EscalaImpulsividad = table.Column<int>(type: "integer", nullable: true),
                    EscalaEstres = table.Column<int>(type: "integer", nullable: true),
                    EscalaFatiga = table.Column<int>(type: "integer", nullable: true),
                    EscalaDesmotivacion = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsicoEscalasAnimos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsicoEscalasAnimos_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsicoEvaluacionesCognitivas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    BeckPersonal = table.Column<string>(type: "text", nullable: true),
                    BeckMundoExterior = table.Column<string>(type: "text", nullable: true),
                    BeckFuturo = table.Column<string>(type: "text", nullable: true),
                    BeckAutolesiones = table.Column<string>(type: "text", nullable: true),
                    BeckAutolisis = table.Column<string>(type: "text", nullable: true),
                    BeckOtros = table.Column<string>(type: "text", nullable: true),
                    FcPensamiento = table.Column<string>(type: "text", nullable: true),
                    FcAtencion = table.Column<string>(type: "text", nullable: true),
                    FcConcentracion = table.Column<string>(type: "text", nullable: true),
                    FcLenguaje = table.Column<string>(type: "text", nullable: true),
                    FcPercepcion = table.Column<string>(type: "text", nullable: true),
                    FcJuicio = table.Column<string>(type: "text", nullable: true),
                    FcAbstraccion = table.Column<string>(type: "text", nullable: true),
                    FcAprendizaje = table.Column<string>(type: "text", nullable: true),
                    FcMemoria = table.Column<string>(type: "text", nullable: true),
                    FcMotivacion = table.Column<string>(type: "text", nullable: true),
                    FcEmocion = table.Column<string>(type: "text", nullable: true),
                    FcCalculo = table.Column<string>(type: "text", nullable: true),
                    FcCoordinacionMotoraFina = table.Column<string>(type: "text", nullable: true),
                    FcCoordinacionMotoraGruesa = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsicoEvaluacionesCognitivas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsicoEvaluacionesCognitivas_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PsicoSomaticoVegetativos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtencionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SuenoNotasGenerales = table.Column<string>(type: "text", nullable: true),
                    SuenoDuracionInicio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SuenoDuracionFin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Ensonaciones = table.Column<string>(type: "text", nullable: true),
                    Pesadillas = table.Column<string>(type: "text", nullable: true),
                    ApneaSueno = table.Column<string>(type: "text", nullable: true),
                    Sobresaltos = table.Column<string>(type: "text", nullable: true),
                    ParalisisSueno = table.Column<string>(type: "text", nullable: true),
                    SuenoOtros = table.Column<string>(type: "text", nullable: true),
                    AlimentacionNotasGenerales = table.Column<string>(type: "text", nullable: true),
                    Peso = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AspectoFisicoActividadFisica = table.Column<string>(type: "text", nullable: true),
                    Apetito = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AntecedentesAlteracionesClinicas = table.Column<string>(type: "text", nullable: true),
                    SomatizacionesNotasGenerales = table.Column<string>(type: "text", nullable: true),
                    Cefalea = table.Column<string>(type: "text", nullable: true),
                    Adormecimientos = table.Column<string>(type: "text", nullable: true),
                    Sudoracion = table.Column<string>(type: "text", nullable: true),
                    Rubefaccion = table.Column<string>(type: "text", nullable: true),
                    SomatizacionesOtros = table.Column<string>(type: "text", nullable: true),
                    SignosVitalesNotasGenerales = table.Column<string>(type: "text", nullable: true),
                    SaturacionOxigeno = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    ReflejoPupilar = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FrecuenciaCardiaca = table.Column<int>(type: "integer", nullable: true),
                    SignosVitalesOtros = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsicoSomaticoVegetativos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsicoSomaticoVegetativos_Atenciones_AtencionId",
                        column: x => x.AtencionId,
                        principalTable: "Atenciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    DatosSnapshotJson = table.Column<string>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb")
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
                name: "IX_Chats_PacienteId",
                table: "Chats",
                column: "PacienteId");

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
                name: "IX_Citas_UsuarioRegistroId",
                table: "Citas",
                column: "UsuarioRegistroId");

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
                name: "IX_MensajesChat_ChatId",
                table: "MensajesChat",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionesCitas_CitaId",
                table: "NotificacionesCitas",
                column: "CitaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionesCitas_Estado_FechaProgramadaEnvio",
                table: "NotificacionesCitas",
                columns: new[] { "Estado", "FechaProgramadaEnvio" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionesCitas_PacienteId",
                table: "NotificacionesCitas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PacienteFamiliares_PacienteId",
                table: "PacienteFamiliares",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_CodigoPaciente",
                table: "Pacientes",
                column: "CodigoPaciente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_DoctorId",
                table: "Pacientes",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_RecepcionadoPorId",
                table: "Pacientes",
                column: "RecepcionadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_UsuarioId",
                table: "Pacientes",
                column: "UsuarioId");

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
                name: "IX_PsicoAnamnesisHistoriales_AtencionId",
                table: "PsicoAnamnesisHistoriales",
                column: "AtencionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PsicoDesarrollosPsicosociales_AtencionId",
                table: "PsicoDesarrollosPsicosociales",
                column: "AtencionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PsicoDiagnosticosCierres_AtencionId",
                table: "PsicoDiagnosticosCierres",
                column: "AtencionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PsicoEscalasAnimos_AtencionId",
                table: "PsicoEscalasAnimos",
                column: "AtencionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PsicoEvaluacionesCognitivas_AtencionId",
                table: "PsicoEvaluacionesCognitivas",
                column: "AtencionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PsicoSomaticoVegetativos_AtencionId",
                table: "PsicoSomaticoVegetativos",
                column: "AtencionId",
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

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CodigoUsuario",
                table: "Usuarios",
                column: "CodigoUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UserName",
                table: "Usuarios",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AjustesFinancieros");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "ComprobanteDetalles");

            migrationBuilder.DropTable(
                name: "HistorialDetalles");

            migrationBuilder.DropTable(
                name: "MensajesChat");

            migrationBuilder.DropTable(
                name: "NotificacionesCitas");

            migrationBuilder.DropTable(
                name: "PacienteFamiliares");

            migrationBuilder.DropTable(
                name: "PsicoAnamnesisHistoriales");

            migrationBuilder.DropTable(
                name: "PsicoDesarrollosPsicosociales");

            migrationBuilder.DropTable(
                name: "PsicoDiagnosticosCierres");

            migrationBuilder.DropTable(
                name: "PsicoEscalasAnimos");

            migrationBuilder.DropTable(
                name: "PsicoEvaluacionesCognitivas");

            migrationBuilder.DropTable(
                name: "PsicoSomaticoVegetativos");

            migrationBuilder.DropTable(
                name: "RolPermisos");

            migrationBuilder.DropTable(
                name: "UsuarioRoles");

            migrationBuilder.DropTable(
                name: "Comprobantes");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Atenciones");

            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "HistorialesClinicos");

            migrationBuilder.DropTable(
                name: "HorariosDoctor");

            migrationBuilder.DropTable(
                name: "ServiciosClinicos");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Doctores");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
