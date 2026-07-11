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
        await SeedServiciosClinicosAsync(context);
        await SeedPacientesAsync(context);
        await SeedCitasYatencionesAsync(context);

        await context.SaveChangesAsync();
    }

    // ===================== PERMISOS =====================
    private static async Task SeedPermisosAsync(ApplicationDbContext context)
    {
        var permisosBase = new List<Permiso>
        {
            // Pacientes
            new() { Codigo = "PACIENTE_VER", Nombre = "Ver pacientes", Modulo = "Pacientes", Activo = true },
            new() { Codigo = "PACIENTE_CREAR", Nombre = "Crear pacientes", Modulo = "Pacientes", Activo = true },
            new() { Codigo = "PACIENTE_EDITAR", Nombre = "Editar pacientes", Modulo = "Pacientes", Activo = true },

            // Citas
            new() { Codigo = "CITA_VER", Nombre = "Ver citas", Modulo = "Citas", Activo = true },
            new() { Codigo = "CITA_PROGRAMAR", Nombre = "Programar citas", Modulo = "Citas", Activo = true },
            new() { Codigo = "CITA_REPROGRAMAR", Nombre = "Reprogramar citas", Modulo = "Citas", Activo = true },
            new() { Codigo = "CITA_CANCELAR", Nombre = "Cancelar citas", Modulo = "Citas", Activo = true },

            // Atenciones
            new() { Codigo = "ATENCION_VER", Nombre = "Ver atenciones", Modulo = "Atenciones", Activo = true },
            new() { Codigo = "ATENCION_REGISTRAR", Nombre = "Registrar atención", Modulo = "Atenciones", Activo = true },
            new() { Codigo = "ATENCION_CERRAR", Nombre = "Cerrar atención", Modulo = "Atenciones", Activo = true },

            // Pagos
            new() { Codigo = "PAGO_VER", Nombre = "Ver pagos", Modulo = "Pagos", Activo = true },
            new() { Codigo = "PAGO_REGISTRAR", Nombre = "Registrar pago", Modulo = "Pagos", Activo = true },

            // Finanzas
            new() { Codigo = "FINANZAS_VER", Nombre = "Ver finanzas", Modulo = "Finanzas", Activo = true },
            new() { Codigo = "FINANZAS_EXPORTAR", Nombre = "Exportar reportes financieros", Modulo = "Finanzas", Activo = true },
            new() { Codigo = "FINANZAS_AJUSTAR", Nombre = "Registrar ajustes financieros", Modulo = "Finanzas", Activo = true },

            // Doctores
            new() { Codigo = "DOCTOR_VER", Nombre = "Ver doctores", Modulo = "Doctores", Activo = true },
            new() { Codigo = "DOCTOR_CREAR", Nombre = "Crear doctores", Modulo = "Doctores", Activo = true },
            new() { Codigo = "DOCTOR_EDITAR", Nombre = "Editar doctores", Modulo = "Doctores", Activo = true },

            // Horarios
            new() { Codigo = "HORARIO_VER", Nombre = "Ver horarios", Modulo = "Horarios", Activo = true },
            new() { Codigo = "HORARIO_CREAR", Nombre = "Crear horarios", Modulo = "Horarios", Activo = true },
            new() { Codigo = "HORARIO_EDITAR", Nombre = "Editar horarios", Modulo = "Horarios", Activo = true },

            // Servicios Clínicos
            new() { Codigo = "SERVICIO_VER", Nombre = "Ver servicios clínicos", Modulo = "Servicios Clínicos", Activo = true },

            // Historial Clínico
            new() { Codigo = "HISTORIAL_VER", Nombre = "Ver historial clínico", Modulo = "Historial Clínico", Activo = true },
            new() { Codigo = "HISTORIAL_IMPRIMIR", Nombre = "Imprimir historial clínico", Modulo = "Historial Clínico", Activo = true },

            // Usuarios
            new() { Codigo = "USUARIO_VER", Nombre = "Ver usuarios", Modulo = "Usuarios", Activo = true },
            new() { Codigo = "USUARIO_CREAR", Nombre = "Crear usuarios", Modulo = "Usuarios", Activo = true },
            new() { Codigo = "USUARIO_EDITAR", Nombre = "Editar usuarios", Modulo = "Usuarios", Activo = true },
            new() { Codigo = "USUARIO_ASIGNAR_ROL", Nombre = "Asignar rol a usuario", Modulo = "Usuarios", Activo = true },

            // Roles
            new() { Codigo = "ROL_VER", Nombre = "Ver roles", Modulo = "Roles", Activo = true },
            new() { Codigo = "ROL_CREAR", Nombre = "Crear roles", Modulo = "Roles", Activo = true },
            new() { Codigo = "ROL_EDITAR", Nombre = "Editar roles", Modulo = "Roles", Activo = true },
            new() { Codigo = "ROL_ASIGNAR_PERMISOS", Nombre = "Asignar permisos a rol", Modulo = "Roles", Activo = true },

            // Permisos
            new() { Codigo = "PERMISO_VER", Nombre = "Ver permisos", Modulo = "Permisos", Activo = true },

            // Auditoría
            new() { Codigo = "AUDITORIA_VER", Nombre = "Ver auditoría", Modulo = "Auditoría", Activo = true },

            // Comprobantes
            new() { Codigo = "COMPROBANTE_VER", Nombre = "Ver comprobantes", Modulo = "Comprobantes", Activo = true },
            new() { Codigo = "COMPROBANTE_EMITIR", Nombre = "Emitir comprobantes", Modulo = "Comprobantes", Activo = true },
            new() { Codigo = "COMPROBANTE_ANULAR", Nombre = "Anular comprobantes", Modulo = "Comprobantes", Activo = true },
            new() { Codigo = "COMPROBANTE_IMPRIMIR", Nombre = "Imprimir comprobantes", Modulo = "Comprobantes", Activo = true },

            // Certificados
            new() { Codigo = "CERTIFICADO_GENERAR", Nombre = "Generar certificados", Modulo = "Certificados", Activo = true },
            new() { Codigo = "CERTIFICADO_BLOCK", Nombre = "Generar certificados en bloque", Modulo = "Certificados", Activo = true },
        };

        var codigosExistentes = await context.Permisos
            .Select(x => x.Codigo)
            .ToListAsync();

        var nuevosPermisos = permisosBase
            .Where(x => !codigosExistentes.Contains(x.Codigo))
            .ToList();

        if (nuevosPermisos.Count > 0)
            await context.Permisos.AddRangeAsync(nuevosPermisos);
    }

    // ===================== ROLES =====================
    private static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        var rolesBase = new List<Rol>
        {
            new()
            {
                Nombre = "Administrador",
                Descripcion = "Rol principal del sistema con acceso total.",
                EsSistema = true,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            },
            new()
            {
                Nombre = "Recepcionista",
                Descripcion = "Gestiona pacientes y citas.",
                EsSistema = true,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            },
            new()
            {
                Nombre = "Doctor",
                Descripcion = "Gestiona atenciones médicas.",
                EsSistema = true,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            },
            new()
            {
                Nombre = "Caja",
                Descripcion = "Gestiona pagos y cobros.",
                EsSistema = true,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            }
        };

        foreach (var rol in rolesBase)
        {
            var existente = await context.Roles.FirstOrDefaultAsync(x => x.Nombre == rol.Nombre);
            if (existente == null)
            {
                await context.Roles.AddAsync(rol);
            }
        }

        await context.SaveChangesAsync();

        // Asignar todos los permisos al Administrador
        var adminRol = await context.Roles.FirstOrDefaultAsync(x => x.Nombre == "Administrador");
        if (adminRol != null)
        {
            var permisos = await context.Permisos.ToListAsync();
            var permisosAdminExistentes = await context.RolPermisos
                .Where(x => x.RolId == adminRol.Id)
                .Select(x => x.PermisoId)
                .ToListAsync();

            foreach (var permiso in permisos)
            {
                if (!permisosAdminExistentes.Contains(permiso.Id))
                {
                    context.RolPermisos.Add(new RolPermiso
                    {
                        RolId = adminRol.Id,
                        PermisoId = permiso.Id,
                        FechaAsignacion = DateTime.UtcNow
                    });
                }
            }
        }

        // (Opcional) Asignar permisos específicos a Doctor, Recepcionista, etc. si deseas
        // Por ahora solo se asigna al Administrador.
    }

    // ===================== USUARIOS =====================
    private static async Task SeedUsuariosAsync(ApplicationDbContext context)
    {
        var usuarios = new List<(string UserName, string Password, string Nombres, string Apellidos, string Correo, string RolNombre)>
        {
            ("admin", "admin123", "Administrador", "Sistema", "admin@clinica.com", "Administrador"),
            ("obstetra", "obstetra123", "María", "González", "maria.gonzalez@clinica.com", "Doctor"),
            ("recepcion", "recepcion123", "Lucía", "Pérez", "lucia.perez@clinica.com", "Recepcionista")
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
                DebeCambiarContrasena = true
            };

            await context.Usuarios.AddAsync(usuario);
            await context.SaveChangesAsync(); // Guardar para obtener el Id

            // Asignar rol
            context.UsuarioRoles.Add(new UsuarioRol
            {
                UsuarioId = usuario.Id,
                RolId = rol.Id,
                FechaAsignacion = DateTime.UtcNow,
                Activo = true
            });
        }

        await context.SaveChangesAsync();

        // Si el usuario admin ya existía, asegurarse de que tenga el rol Administrador
        var admin = await context.Usuarios.FirstOrDefaultAsync(x => x.UserName == "admin");
        var adminRol = await context.Roles.FirstOrDefaultAsync(x => x.Nombre == "Administrador");
        if (admin != null && adminRol != null)
        {
            var tieneRol = await context.UsuarioRoles.AnyAsync(x => x.UsuarioId == admin.Id && x.RolId == adminRol.Id);
            if (!tieneRol)
            {
                context.UsuarioRoles.Add(new UsuarioRol
                {
                    UsuarioId = admin.Id,
                    RolId = adminRol.Id,
                    FechaAsignacion = DateTime.UtcNow,
                    Activo = true
                });
            }
        }
    }

    // ===================== SERVICIOS CLÍNICOS =====================
    private static async Task SeedServiciosClinicosAsync(ApplicationDbContext context)
    {
        if (await context.ServiciosClinicos.AnyAsync()) return;

        var servicios = new List<ServicioClinico>
        {
            new()
            {
                CodigoServicio = "ATEGEN",
                Nombre = "Atención genérica",
                Descripcion = "Servicio clínico base para registrar una atención general.",
                CostoBase = 50,
                DuracionMinutos = 30,
                RequiereCita = true,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            },
            new()
            {
                CodigoServicio = "CONOBS",
                Nombre = "Consulta obstétrica",
                Descripcion = "Consulta médica orientada al control obstétrico.",
                CostoBase = 70,
                DuracionMinutos = 30,
                RequiereCita = true,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            },
            new()
            {
                CodigoServicio = "PARTO",
                Nombre = "Atención de parto",
                Descripcion = "Atención integral durante el trabajo de parto y nacimiento.",
                CostoBase = 500,
                DuracionMinutos = 120,
                RequiereCita = false,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            },
            new()
            {
                CodigoServicio = "CESAREA",
                Nombre = "Cesárea",
                Descripcion = "Intervención quirúrgica para el nacimiento por cesárea.",
                CostoBase = 1500,
                DuracionMinutos = 90,
                RequiereCita = false,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            },
            new()
            {
                CodigoServicio = "ECOGRAFIA",
                Nombre = "Ecografía obstétrica",
                Descripcion = "Estudio ecográfico para evaluar el desarrollo fetal.",
                CostoBase = 120,
                DuracionMinutos = 20,
                RequiereCita = true,
                GeneraHistorial = true,
                Estado = EstadoServicioClinico.Activo
            }
        };

        await context.ServiciosClinicos.AddRangeAsync(servicios);
    }

    // ===================== PACIENTES =====================
    private static async Task SeedPacientesAsync(ApplicationDbContext context)
    {
        if (await context.Pacientes.AnyAsync()) return;

        // Obtener el usuario admin (necesario para UsuarioId)
        var adminUsuario = await context.Usuarios.FirstOrDefaultAsync(x => x.UserName == "admin");
        if (adminUsuario == null) return;

        var pacientes = new List<Paciente>
        {
            new()
            {
                CodigoPaciente = $"PCT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-12345678",
                DNI = "12345678",
                Nombres = "Ana",
                Apellidos = "Torres",
                FechaNacimiento = new DateTime(1985, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                Sexo = "F",
                Celular = "987654321",
                Correo = "ana.torres@email.com",
                Direccion = "Av. Los Incas 456, Juliaca",
                LugarNacimiento = "Puno",
                GradoInstruccion = "Superior",
                Ocupacion = "Ingeniera",
                Religion = "Católica",
                EstadoCivil = "Casada",
                NombrePareja = "Carlos Rojas",
                CelularPareja = "987654322",
                UsuarioId = adminUsuario.Id,
                Estado = EstadoPaciente.Activo,
                FechaRegistro = DateTime.UtcNow
            },
            new()
            {
                CodigoPaciente = $"PCT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-23456789",
                DNI = "23456789",
                Nombres = "María",
                Apellidos = "Fernández",
                FechaNacimiento = new DateTime(1990, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                Sexo = "F",
                Celular = "987654323",
                Correo = "maria.fernandez@email.com",
                Direccion = "Jr. San Martín 123, Juliaca",
                LugarNacimiento = "Cusco",
                GradoInstruccion = "Secundaria",
                Ocupacion = "Ama de casa",
                Religion = "Evangélica",
                EstadoCivil = "Casada",
                NombrePareja = "Luis Gómez",
                CelularPareja = "987654324",
                UsuarioId = adminUsuario.Id,
                Estado = EstadoPaciente.Activo,
                FechaRegistro = DateTime.UtcNow
            },
            new()
            {
                CodigoPaciente = $"PCT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-34567890",
                DNI = "34567890",
                Nombres = "Rosa",
                Apellidos = "Mamani",
                FechaNacimiento = new DateTime(1992, 8, 22, 0, 0, 0, DateTimeKind.Utc),
                Sexo = "F",
                Celular = "987654325",
                Correo = "rosa.mamani@email.com",
                Direccion = "Av. Túpac Amaru 789, Juliaca",
                LugarNacimiento = "Arequipa",
                GradoInstruccion = "Técnica",
                Ocupacion = "Enfermera",
                Religion = "Católica",
                EstadoCivil = "Soltera",
                NombrePareja = null,
                CelularPareja = null,
                UsuarioId = adminUsuario.Id,
                Estado = EstadoPaciente.Activo,
                FechaRegistro = DateTime.UtcNow
            },
            new()
            {
                CodigoPaciente = $"PCT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-45678901",
                DNI = "45678901",
                Nombres = "Carmen",
                Apellidos = "Quispe",
                FechaNacimiento = new DateTime(1988, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                Sexo = "F",
                Celular = "987654326",
                Correo = "carmen.quispe@email.com",
                Direccion = "Jr. Lima 321, Juliaca",
                LugarNacimiento = "Puno",
                GradoInstruccion = "Superior",
                Ocupacion = "Contadora",
                Religion = "Católica",
                EstadoCivil = "Viuda",
                NombrePareja = null,
                CelularPareja = null,
                UsuarioId = adminUsuario.Id,
                Estado = EstadoPaciente.Activo,
                FechaRegistro = DateTime.UtcNow
            },
            new()
            {
                CodigoPaciente = $"PCT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}-56789012",
                DNI = "56789012",
                Nombres = "Patricia",
                Apellidos = "Chávez",
                FechaNacimiento = new DateTime(1995, 3, 7, 0, 0, 0, DateTimeKind.Utc),
                Sexo = "F",
                Celular = "987654327",
                Correo = "patricia.chavez@email.com",
                Direccion = "Av. Brasil 654, Juliaca",
                LugarNacimiento = "Moquegua",
                GradoInstruccion = "Secundaria",
                Ocupacion = "Estudiante",
                Religion = "Ninguna",
                EstadoCivil = "Soltera",
                NombrePareja = null,
                CelularPareja = null,
                UsuarioId = adminUsuario.Id,
                Estado = EstadoPaciente.Activo,
                FechaRegistro = DateTime.UtcNow
            }
        };

        await context.Pacientes.AddRangeAsync(pacientes);
        await context.SaveChangesAsync();

        // 1. CORRECCIÓN CRÍTICA: Agregamos .ToList() al final para fijar las entidades en memoria
        var historiales = pacientes.Select(p => new HistorialClinico
        {
            CodigoHistorial = $"FIL-{DateTime.UtcNow:yyyy}-{p.DNI}",
            PacienteId = p.Id,
            FechaApertura = DateTime.UtcNow,
            Estado = EstadoHistorialClinico.Activo
        }).ToList(); // ◄── AHORA SÍ ES UNA LISTA REAL

        await context.HistorialesClinicos.AddRangeAsync(historiales);
        await context.SaveChangesAsync(); // Los IDs reales se escriben sobre la lista 'historiales'

        // 2. CORRECCIÓN: También materializamos los detalles por seguridad
        var detallesApertura = historiales.Select(h => new HistorialDetalle
        {
            CodigoDetalle = $"DET-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
            HistorialClinicoId = h.Id, // Ahorá sí leerá los IDs reales guardados arriba
            TipoMovimiento = TipoMovimientoHistorial.AperturaHistorial,
            Titulo = "Apertura de historial",
            Descripcion = "Historial clínico aperturado automáticamente al registrar al paciente.",
            FechaRegistro = DateTime.UtcNow,
            UsuarioId = adminUsuario.Id
        }).ToList(); // ◄── Materializado para mantener consistencia

        await context.HistorialDetalles.AddRangeAsync(detallesApertura);
        await context.SaveChangesAsync();
    }

    // ===================== CITAS Y ATENCIONES DE PRUEBA (opcional) =====================
    private static async Task SeedCitasYatencionesAsync(ApplicationDbContext context)
    {
        // Si ya hay citas, no hacemos nada
        if (await context.Citas.AnyAsync()) return;

        // Obtener datos necesarios
        var doctor = await context.Doctores.FirstOrDefaultAsync();
        if (doctor == null) return; // Si no hay doctor, no creamos citas

        var pacientes = await context.Pacientes.Take(3).ToListAsync();
        var servicios = await context.ServiciosClinicos.ToListAsync();
        var adminUsuario = await context.Usuarios.FirstOrDefaultAsync(x => x.UserName == "admin");

        if (pacientes.Count == 0 || servicios.Count == 0 || adminUsuario == null) return;

        // Fecha para hoy y mañana
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var manana = hoy.AddDays(1);

        var citas = new List<Cita>
        {
            new()
            {
                CodigoCita = $"CIT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
                PacienteId = pacientes[0].Id,
                DoctorId = doctor.Id,
                ServicioClinicoId = servicios.First(s => s.CodigoServicio == "CONOBS").Id,
                Fecha = hoy,
                HoraInicio = new TimeOnly(9, 0),
                HoraFin = new TimeOnly(9, 30),
                Motivo = "Control prenatal",
                Estado = EstadoCita.Pendiente,
                FechaRegistro = DateTime.UtcNow,
                UsuarioRegistroId = adminUsuario.Id
            },
            new()
            {
                CodigoCita = $"CIT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
                PacienteId = pacientes[1].Id,
                DoctorId = doctor.Id,
                ServicioClinicoId = servicios.First(s => s.CodigoServicio == "ECOGRAFIA").Id,
                Fecha = hoy,
                HoraInicio = new TimeOnly(10, 0),
                HoraFin = new TimeOnly(10, 30),
                Motivo = "Ecografía de tercer trimestre",
                Estado = EstadoCita.Pendiente,
                FechaRegistro = DateTime.UtcNow,
                UsuarioRegistroId = adminUsuario.Id
            },
            new()
            {
                CodigoCita = $"CIT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
                PacienteId = pacientes[2].Id,
                DoctorId = doctor.Id,
                ServicioClinicoId = servicios.First(s => s.CodigoServicio == "CONOBS").Id,
                Fecha = manana,
                HoraInicio = new TimeOnly(11, 0),
                HoraFin = new TimeOnly(11, 30),
                Motivo = "Control de embarazo",
                Estado = EstadoCita.Pendiente,
                FechaRegistro = DateTime.UtcNow,
                UsuarioRegistroId = adminUsuario.Id
            }
        };

        await context.Citas.AddRangeAsync(citas);
        await context.SaveChangesAsync();

        // Registrar en historial los movimientos de cita programada
        foreach (var cita in citas)
        {
            var historial = await context.HistorialesClinicos.FirstOrDefaultAsync(h => h.PacienteId == cita.PacienteId);
            if (historial != null)
            {
                var detalle = new HistorialDetalle
                {
                    CodigoDetalle = $"DET-{Guid.NewGuid().ToString("N")[..5].ToUpper()}",
                    HistorialClinicoId = historial.Id,
                    CitaId = cita.Id,
                    TipoMovimiento = TipoMovimientoHistorial.CitaProgramada,
                    Titulo = "Cita programada",
                    Descripcion = $"Cita para {cita.Motivo} el {cita.Fecha} a las {cita.HoraInicio}",
                    FechaRegistro = DateTime.UtcNow,
                    UsuarioId = adminUsuario.Id
                };
                context.HistorialDetalles.Add(detalle);
            }
        }

        await context.SaveChangesAsync();
    }
}