using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.DbFiles.Data.Seeds;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        await SeedPermisosAsync(context);
        await SeedRolesAsync(context);
        await SeedUsuariosAsync(context);
        await SeedDoctoresAsync(context); 
        await SeedServiciosClinicosAsync(context);
        await SeedPacientesAsync(context);
        await SeedCitasYatencionesAsync(context);

        await context.SaveChangesAsync();
    }

    // ==========================================
    // MATRIZ DE PERMISOS
    // ==========================================
    private static async Task SeedPermisosAsync(ApplicationDbContext context)
    {
        var permisosBase = new List<Permiso>
        {
            new() { Codigo = "PACIENTE_VER", Nombre = "Ver pacientes", Modulo = "Pacientes", Activo = true },
            new() { Codigo = "PACIENTE_CREAR", Nombre = "Crear pacientes", Modulo = "Pacientes", Activo = true },
            new() { Codigo = "PACIENTE_EDITAR", Nombre = "Editar pacientes", Modulo = "Pacientes", Activo = true },
            new() { Codigo = "CITA_VER", Nombre = "Ver citas", Modulo = "Citas", Activo = true },
            new() { Codigo = "CITA_PROGRAMAR", Nombre = "Programar citas", Modulo = "Citas", Activo = true },
            new() { Codigo = "CITA_REPROGRAMAR", Nombre = "Reprogramar citas", Modulo = "Citas", Activo = true },
            new() { Codigo = "CITA_CANCELAR", Nombre = "Cancelar citas", Modulo = "Citas", Activo = true },
            new() { Codigo = "ATENCION_VER", Nombre = "Ver atenciones", Modulo = "Atenciones", Activo = true },
            new() { Codigo = "ATENCION_REGISTRAR", Nombre = "Registrar atención", Modulo = "Atenciones", Activo = true },
            new() { Codigo = "ATENCION_CERRAR", Nombre = "Cerrar atención", Modulo = "Atenciones", Activo = true },
            new() { Codigo = "PAGO_VER", Nombre = "Ver pagos", Modulo = "Pagos", Activo = true },
            new() { Codigo = "PAGO_REGISTRAR", Nombre = "Registrar pago", Modulo = "Pagos", Activo = true },
            new() { Codigo = "FINANZAS_VER", Nombre = "Ver finanzas", Modulo = "Finanzas", Activo = true },
            new() { Codigo = "FINANZAS_EXPORTAR", Nombre = "Exportar reportes financieros", Modulo = "Finanzas", Activo = true },
            new() { Codigo = "FINANZAS_AJUSTAR", Nombre = "Registrar ajustes financieros", Modulo = "Finanzas", Activo = true },
            new() { Codigo = "DOCTOR_VER", Nombre = "Ver doctores", Modulo = "Doctores", Activo = true },
            new() { Codigo = "DOCTOR_CREAR", Nombre = "Crear doctores", Modulo = "Doctores", Activo = true },
            new() { Codigo = "DOCTOR_EDITAR", Nombre = "Editar doctores", Modulo = "Doctores", Activo = true },
            new() { Codigo = "HORARIO_VER", Nombre = "Ver horarios", Modulo = "Horarios", Activo = true },
            new() { Codigo = "HORARIO_CREAR", Nombre = "Crear horarios", Modulo = "Horarios", Activo = true },
            new() { Codigo = "HORARIO_EDITAR", Nombre = "Editar horarios", Modulo = "Horarios", Activo = true },
            new() { Codigo = "SERVICIO_VER", Nombre = "Ver servicios clínicos", Modulo = "Servicios Clínicos", Activo = true },
            new() { Codigo = "HISTORIAL_VER", Nombre = "Ver historial clínico", Modulo = "Historial Clínico", Activo = true },
            new() { Codigo = "HISTORIAL_IMPRIMIR", Nombre = "Imprimir historial clínico", Modulo = "Historial Clínico", Activo = true },
            new() { Codigo = "USUARIO_VER", Nombre = "Ver usuarios", Modulo = "Usuarios", Activo = true },
            new() { Codigo = "USUARIO_CREAR", Nombre = "Crear usuarios", Modulo = "Usuarios", Activo = true },
            new() { Codigo = "USUARIO_EDITAR", Nombre = "Editar usuarios", Modulo = "Usuarios", Activo = true },
            new() { Codigo = "USUARIO_ASIGNAR_ROL", Nombre = "Asignar rol a usuario", Modulo = "Usuarios", Activo = true },
            new() { Codigo = "ROL_VER", Nombre = "Ver roles", Modulo = "Roles", Activo = true },
            new() { Codigo = "ROL_CREAR", Nombre = "Crear roles", Modulo = "Roles", Activo = true },
            new() { Codigo = "ROL_EDITAR", Nombre = "Editar roles", Modulo = "Roles", Activo = true },
            new() { Codigo = "ROL_ASIGNAR_PERMISOS", Nombre = "Asignar permisos a rol", Modulo = "Roles", Activo = true },
            new() { Codigo = "PERMISO_VER", Nombre = "Ver permisos", Modulo = "Permisos", Activo = true },
            new() { Codigo = "AUDITORIA_VER", Nombre = "Ver auditoría", Modulo = "Auditoría", Activo = true },
            new() { Codigo = "COMPROBANTE_VER", Nombre = "Ver comprobantes", Modulo = "Comprobantes", Activo = true },
            new() { Codigo = "COMPROBANTE_EMITIR", Nombre = "Emitir comprobantes", Modulo = "Comprobantes", Activo = true },
            new() { Codigo = "COMPROBANTE_ANULAR", Nombre = "Anular comprobantes", Modulo = "Comprobantes", Activo = true },
            new() { Codigo = "COMPROBANTE_IMPRIMIR", Nombre = "Imprimir comprobantes", Modulo = "Comprobantes", Activo = true },
            new() { Codigo = "CERTIFICADO_GENERAR", Nombre = "Generar certificados", Modulo = "Certificados", Activo = true },
            new() { Codigo = "CERTIFICADO_BLOCK", Nombre = "Generar certificados en bloque", Modulo = "Certificados", Activo = true },
        };

        var codigosExistentes = await context.Permisos.Select(x => x.Codigo).ToListAsync();
        var nuevosPermisos = permisosBase.Where(x => !codigosExistentes.Contains(x.Codigo)).ToList();

        if (nuevosPermisos.Count > 0)
            await context.Permisos.AddRangeAsync(nuevosPermisos);
    }

    // ==========================================
    // ROLES DEL SISTEMA
    // ==========================================
    private static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        var rolesBase = new List<Rol>
        {
            new() { Nombre = "Administrador", Descripcion = "Rol principal con acceso total.", EsSistema = true, Activo = true, FechaCreacion = DateTime.UtcNow },
            new() { Nombre = "Recepcionista", Descripcion = "Gestiona pacientes y citas.", EsSistema = true, Activo = true, FechaCreacion = DateTime.UtcNow },
            new() { Nombre = "Doctor", Descripcion = "Gestiona atenciones médicas clínicos.", EsSistema = true, Activo = true, FechaCreacion = DateTime.UtcNow },
            new() { Nombre = "Caja", Descripcion = "Gestiona pagos y cobros financieros.", EsSistema = true, Activo = true, FechaCreacion = DateTime.UtcNow }
        };

        foreach (var rol in rolesBase)
        {
            var existente = await context.Roles.FirstOrDefaultAsync(x => x.Nombre == rol.Nombre);
            if (existente == null) await context.Roles.AddAsync(rol);
        }
        await context.SaveChangesAsync();

        var adminRol = await context.Roles.FirstOrDefaultAsync(x => x.Nombre == "Administrador");
        if (adminRol != null)
        {
            var permisos = await context.Permisos.ToListAsync();
            var permisosAdminExistentes = await context.RolPermisos.Where(x => x.RolId == adminRol.Id).Select(x => x.PermisoId).ToListAsync();

            foreach (var permiso in permisos)
            {
                if (!permisosAdminExistentes.Contains(permiso.Id))
                {
                    context.RolPermisos.Add(new RolPermiso { RolId = adminRol.Id, PermisoId = permiso.Id, FechaAsignacion = DateTime.UtcNow });
                }
            }
        }
    }

    // ==========================================
    // CREDENCIALES DE ACCESO
    // ==========================================
    private static async Task SeedUsuariosAsync(ApplicationDbContext context)
    {
        var usuarios = new List<(string UserName, string Password, string Nombres, string Apellidos, string Correo, string RolNombre)>
        {
            ("admin", "admin123", "Administrador", "Sistema", "admin@psicomedix.com", "Administrador"),
            ("psicologo", "psicologo123", "María", "González", "maria.gonzalez@psicomedix.com", "Doctor"),
            ("recepcion", "recepcion123", "Lucía", "Pérez", "lucia.perez@psicomedix.com", "Recepcionista")
        };

        foreach (var item in usuarios)
        {
            var usuarioExistente = await context.Usuarios.FirstOrDefaultAsync(x => x.UserName == item.UserName);
            if (usuarioExistente != null) continue;

            var rol = await context.Roles.FirstOrDefaultAsync(x => x.Nombre == item.RolNombre);
            if (rol == null) continue;

            var usuario = new Usuario
            {
                CodigoUsuario = $"USR-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
                Nombres = item.Nombres,
                Apellidos = item.Apellidos,
                UserName = item.UserName,
                Correo = item.Correo,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(item.Password),
                Estado = EstadoUsuario.Activo,
                FechaRegistro = DateTime.UtcNow,
                DebeCambiarContrasena = false
            };

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync();

            context.UsuarioRoles.Add(new UsuarioRol { UsuarioId = usuario.Id, RolId = rol.Id, FechaAsignacion = DateTime.UtcNow, Activo = true });
        }
        await context.SaveChangesAsync();
    }
    
    // ==========================================
    // PERSONAL MÉDICO ASIGNADO
    // ==========================================
    private static async Task SeedDoctoresAsync(ApplicationDbContext context)
    {
        if (await context.Doctores.AnyAsync()) return;

        var usuarioPsicologo = await context.Usuarios.FirstOrDefaultAsync(u => u.UserName == "psicologo");
        if (usuarioPsicologo == null) return;

        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            CodigoDoctor = $"DOC-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-102030",
            CMP = "102030",
            Nombres = "María",
            Apellidos = "González",
            Especialidad = "Psicología Clínica",
            Celular = "987654329",
            Correo = "maria.gonzalez@psicomedix.com",
            FechaInicioContrato = DateTime.UtcNow,
            Estado = EstadoDoctor.Activo,
            UsuarioId = usuarioPsicologo.Id
        };

        await context.Doctores.AddAsync(doctor);
        await context.SaveChangesAsync();
    }

    // ==========================================
    // CATÁLOGO DE SERVICIOS MENTALES
    // ==========================================
    private static async Task SeedServiciosClinicosAsync(ApplicationDbContext context)
    {
        if (await context.ServiciosClinicos.AnyAsync()) return;

        var servicios = new List<ServicioClinico>
        {
            new()
            {
                CodigoServicio = "PSIGEN",
                Nombre = "Consulta Psicológica General",
                Descripcion = "Evaluación psicoterapéutica base para adolescentes y adultos.",
                CostoBase = 70,
                DuracionMinutos = 45,
                RequiereCita = true,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            },
            new()
            {
                CodigoServicio = "PSIEVA",
                Nombre = "Evaluación Psicológica Completa",
                Descripcion = "Aplicación de reactivos, baterías psicológicas e informe de evolución mental.",
                CostoBase = 120,
                DuracionMinutos = 60,
                RequiereCita = true,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            },
            new()
            {
                CodigoServicio = "TERPAR",
                Nombre = "Psicoterapia de Pareja",
                Descripcion = "Abordaje clínico de dinámicas vinculares y resolución de conflictos.",
                CostoBase = 100,
                DuracionMinutos = 60,
                RequiereCita = true,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            },
            new()
            {
                CodigoServicio = "TERINF",
                Nombre = "Abordaje Infantil y del Adolescente",
                Descripcion = "Terapia orientada al desarrollo cognitivo-emocional de menores de edad.",
                CostoBase = 80,
                DuracionMinutos = 45,
                RequiereCita = true,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            }
        };

        await context.ServiciosClinicos.AddRangeAsync(servicios);
    }

    // ==========================================
    // PACIENTES SEMILLA
    // ==========================================
    private static async Task SeedPacientesAsync(ApplicationDbContext context)
    {
        if (await context.Pacientes.AnyAsync()) return;

        var adminUsuario = await context.Usuarios.FirstOrDefaultAsync(x => x.UserName == "admin");
        if (adminUsuario == null) return;

        var pacientes = new List<Paciente>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CodigoPaciente = $"PCT-{DateTime.UtcNow:yyyy}-A1",
                DNI = "12345678",
                FechaAtencion = DateTime.UtcNow,
                TieneInformante = false,
                PrimeraAtencionPsicologia = true,
                Nombres = "Ana",
                Apellidos = "Torres",
                Genero = GeneroEvaluado.Femenino,
                EdadAnos = 41,
                Celular = "987654321",
                FechaNacimiento = new DateTime(1985, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                EstadoCivil = EstadoCivil.Casado,
                LugarNacimiento = "Puno",
                Ocupacion = "Ingeniera",
                GradoInstruccion = GradoInstruccion.SuperiorUniversitariaCompleta,
                Religion = "Católica",
                PesoKg = 60m,
                TallaMetros = 1.65m,
                IMC = 22.04m,
                UsuarioId = adminUsuario.Id,
                Estado = EstadoPaciente.Activo,
                FechaRegistro = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                CodigoPaciente = $"PCT-{DateTime.UtcNow:yyyy}-B2",
                DNI = "23456789",
                FechaAtencion = DateTime.UtcNow,
                TieneInformante = true,
                InformanteNombre = "Carlos Gómez",
                InformanteParentesco = "Esposo",
                PrimeraAtencionPsicologia = false,
                Nombres = "María",
                Apellidos = "Fernández",
                Genero = GeneroEvaluado.Femenino, 
                EdadAnos = 36,
                Celular = "987654323",
                FechaNacimiento = new DateTime(1990, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                EstadoCivil = EstadoCivil.Casado,
                LugarNacimiento = "Cusco",
                Ocupacion = "Contadora",
                GradoInstruccion = GradoInstruccion.SuperiorUniversitariaCompleta,
                Religion = "Evangélica",
                PesoKg = 68m,
                TallaMetros = 1.60m,
                IMC = 26.56m,
                UsuarioId = adminUsuario.Id,
                Estado = EstadoPaciente.Activo,
                FechaRegistro = DateTime.UtcNow
            }
        };

        await context.Pacientes.AddRangeAsync(pacientes);
        await context.SaveChangesAsync();

        var historiales = pacientes.Select(p => new HistorialClinico
        {
            CodigoHistorial = $"FIL-{DateTime.UtcNow:yyyy}-{p.DNI ?? "SIN-DNI"}",
            PacienteId = p.Id,
            FechaApertura = DateTime.UtcNow,
            Estado = EstadoHistorialClinico.Activo
        }).ToList();

        await context.HistorialesClinicos.AddRangeAsync(historiales);
        await context.SaveChangesAsync();

        var detallesApertura = historiales.Select(h => new HistorialDetalle
        {
            CodigoDetalle = $"DET-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
            HistorialClinicoId = h.Id,
            TipoMovimiento = TipoMovimientoHistorial.AperturaHistorial,
            Titulo = "Apertura de historial",
            Descripcion = "Historial clínico psicológico aperturado automáticamente.",
            FechaRegistro = DateTime.UtcNow,
            UsuarioId = adminUsuario.Id
        }).ToList();

        await context.HistorialDetalles.AddRangeAsync(detallesApertura);
        await context.SaveChangesAsync();
    }

    // ==========================================
    // AGENDAMIENTO INICIAL
    // ==========================================
    private static async Task SeedCitasYatencionesAsync(ApplicationDbContext context)
    {
        if (await context.Citas.AnyAsync()) return;

        var doctor = await context.Doctores.FirstOrDefaultAsync();
        if (doctor == null) return; 

        var pacientes = await context.Pacientes.Take(2).ToListAsync();
        var servicios = await context.ServiciosClinicos.ToListAsync();
        var adminUsuario = await context.Usuarios.FirstOrDefaultAsync(x => x.UserName == "admin");

        if (pacientes.Count == 0 || servicios.Count == 0 || adminUsuario == null) return;

        var hoy = DateOnly.FromDateTime(DateTime.Today);

        var citas = new List<Cita>
        {
            new()
            {
                CodigoCita = $"CIT-{DateTime.UtcNow:yyyy}-01",
                PacienteId = pacientes[0].Id,
                DoctorId = doctor.Id,
                ServicioClinicoId = servicios.First(s => s.CodigoServicio == "PSIGEN").Id, 
                Fecha = hoy,
                HoraInicio = new TimeOnly(9, 0),
                HoraFin = new TimeOnly(9, 45),
                Motivo = "Consulta inicial de psicoterapia por cuadro de ansiedad reactiva",
                Estado = EstadoCita.Pendiente,
                FechaRegistro = DateTime.UtcNow,
                UsuarioRegistroId = adminUsuario.Id
            }
        };

        await context.Citas.AddRangeAsync(citas);
        await context.SaveChangesAsync();

        foreach (var cita in citas)
        {
            var historial = await context.HistorialesClinicos.FirstOrDefaultAsync(h => h.PacienteId == cita.PacienteId);
            if (historial != null)
            {
                context.HistorialDetalles.Add(new HistorialDetalle
                {
                    CodigoDetalle = $"DET-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
                    HistorialClinicoId = historial.Id,
                    CitaId = cita.Id,
                    TipoMovimiento = TipoMovimientoHistorial.CitaProgramada,
                    Titulo = "Cita programada",
                    Descripcion = $"Cita psicológica agendada para el {cita.Fecha} a las {cita.HoraInicio}.",
                    FechaRegistro = DateTime.UtcNow,
                    UsuarioId = adminUsuario.Id
                });
            }
        }

        await context.SaveChangesAsync();
    }
}