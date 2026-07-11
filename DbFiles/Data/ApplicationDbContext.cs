using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.Models;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.DbFiles.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // ==========================================================
    // SEGURIDAD Y AUDITORÍA
    // ==========================================================

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();
    public DbSet<RolPermiso> RolPermisos => Set<RolPermiso>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();

    // ==========================================================
    // GESTIÓN CLÍNICA BASE
    // ==========================================================

    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Doctor> Doctores => Set<Doctor>();
    public DbSet<HorarioDoctor> HorariosDoctor => Set<HorarioDoctor>();
    public DbSet<Cita> Citas => Set<Cita>();
    
    // esto es exclusivo de evolution api, no incluir al sistema
    public DbSet<NotificacionCita> NotificacionesCitas => Set<NotificacionCita>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<MensajeChat> MensajesChat => Set<MensajeChat>();
    
    // ==========================================================
    // SERVICIOS, HISTORIAL, ATENCIONES Y FINANZAS
    // ==========================================================

    public DbSet<ServicioClinico> ServiciosClinicos => Set<ServicioClinico>();
    public DbSet<HistorialClinico> HistorialesClinicos => Set<HistorialClinico>();
    public DbSet<HistorialDetalle> HistorialDetalles => Set<HistorialDetalle>();
    public DbSet<Atencion> Atenciones => Set<Atencion>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<AjusteFinanciero> AjustesFinancieros => Set<AjusteFinanciero>();

    // ==========================================================
    // COMPROBANTES
    // ==========================================================

    public DbSet<Comprobante> Comprobantes => Set<Comprobante>();
    public DbSet<ComprobanteDetalle> ComprobanteDetalles => Set<ComprobanteDetalle>();

    // ==========================================================
    // MÓDULOS OBSTÉTRICOS (NUEVO)
    // ==========================================================
    
    public DbSet<Anamnesis> Anamnesis => Set<Anamnesis>();
    public DbSet<ExamenFisico> ExamenesFisicos => Set<ExamenFisico>();
    public DbSet<TactoVaginal> TactosVaginales => Set<TactoVaginal>();
    public DbSet<EcografiaObstetrica> EcografiasObstetricas => Set<EcografiaObstetrica>();
    public DbSet<ImpresionDiagnostica> ImpresionesDiagnosticas => Set<ImpresionDiagnostica>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}