namespace psicomedixMonolito.Utils.Authorization;

public static class PermisosPolicies
{
    // ==============================
    // PACIENTES
    // ==============================
    public const string PacienteVer = "PACIENTE_VER";
    public const string PacienteCrear = "PACIENTE_CREAR";
    public const string PacienteEditar = "PACIENTE_EDITAR";

    // ==============================
    // CITAS
    // ==============================
    public const string CitaVer = "CITA_VER";
    public const string CitaProgramar = "CITA_PROGRAMAR";
    public const string CitaReprogramar = "CITA_REPROGRAMAR";
    public const string CitaCancelar = "CITA_CANCELAR";

    // ==============================
    // ATENCIONES (Core)
    // ==============================
    public const string AtencionVer = "ATENCION_VER";
    public const string AtencionRegistrar = "ATENCION_REGISTRAR";
    public const string AtencionCerrar = "ATENCION_CERRAR";

    // ==============================
    // PAGOS
    // ==============================
    public const string PagoVer = "PAGO_VER";
    public const string PagoRegistrar = "PAGO_REGISTRAR";

    // ==============================
    // FINANZAS
    // ==============================
    public const string FinanzasVer = "FINANZAS_VER";
    public const string FinanzasExportar = "FINANZAS_EXPORTAR";   // Reservado para exportaciones futuras
    public const string FinanzasAjustar = "FINANZAS_AJUSTAR";      // Reservado para ajustes financieros

    // ==============================
    // DOCTORES
    // ==============================
    public const string DoctorVer = "DOCTOR_VER";
    public const string DoctorCrear = "DOCTOR_CREAR";
    public const string DoctorEditar = "DOCTOR_EDITAR";

    // ==============================
    // HORARIOS
    // ==============================
    public const string HorarioVer = "HORARIO_VER";
    public const string HorarioCrear = "HORARIO_CREAR";
    public const string HorarioEditar = "HORARIO_EDITAR";

    // ==============================
    // SERVICIOS CLÍNICOS
    // ==============================
    public const string ServicioVer = "SERVICIO_VER";

    // ==============================
    // HISTORIAL CLÍNICO
    // ==============================
    public const string HistorialVer = "HISTORIAL_VER";
    public const string HistorialImprimir = "HISTORIAL_IMPRIMIR";

    // ==============================
    // USUARIOS
    // ==============================
    public const string UsuarioVer = "USUARIO_VER";
    public const string UsuarioCrear = "USUARIO_CREAR";
    public const string UsuarioEditar = "USUARIO_EDITAR";
    public const string UsuarioAsignarRol = "USUARIO_ASIGNAR_ROL";

    // ==============================
    // ROLES
    // ==============================
    public const string RolVer = "ROL_VER";
    public const string RolCrear = "ROL_CREAR";
    public const string RolEditar = "ROL_EDITAR";
    public const string RolAsignarPermisos = "ROL_ASIGNAR_PERMISOS";

    // ==============================
    // PERMISOS (catálogo)
    // ==============================
    public const string PermisoVer = "PERMISO_VER";

    // ==============================
    // AUDITORÍA
    // ==============================
    public const string AuditoriaVer = "AUDITORIA_VER";

    // ==============================
    // COMPROBANTES (documentos)
    // ==============================
    public const string ComprobanteVer = "COMPROBANTE_VER";
    public const string ComprobanteEmitir = "COMPROBANTE_EMITIR";
    public const string ComprobanteAnular = "COMPROBANTE_ANULAR";
    public const string ComprobanteImprimir = "COMPROBANTE_IMPRIMIR";

    // ==============================
    // CERTIFICADOS
    // ==============================
    public const string CertificadoGenerar = "CERTIFICADO_GENERAR";
    public const string CertificadoBlock = "CERTIFICADO_BLOCK";

    // ==============================
    // LISTA COMPLETA DE TODOS LOS PERMISOS
    // ==============================
    public static readonly string[] Todos =
    {
        PacienteVer, PacienteCrear, PacienteEditar,
        CitaVer, CitaProgramar, CitaReprogramar, CitaCancelar,
        AtencionVer, AtencionRegistrar, AtencionCerrar,
        PagoVer, PagoRegistrar,
        FinanzasVer, FinanzasExportar, FinanzasAjustar,
        ComprobanteVer, ComprobanteEmitir, ComprobanteAnular, ComprobanteImprimir,
        HistorialVer, HistorialImprimir,
        DoctorVer, DoctorCrear, DoctorEditar,
        HorarioVer, HorarioCrear, HorarioEditar,
        ServicioVer,
        UsuarioVer, UsuarioCrear, UsuarioEditar, UsuarioAsignarRol,
        RolVer, RolCrear, RolEditar, RolAsignarPermisos,
        PermisoVer,
        AuditoriaVer,
        CertificadoGenerar, CertificadoBlock
    };
}