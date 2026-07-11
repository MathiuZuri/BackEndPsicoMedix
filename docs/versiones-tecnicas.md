# 📌 Versiones y Tecnologías Utilizadas

El sistema **Clínica Santa Mónica** ha sido desarrollado bajo una arquitectura moderna basada en separación de capas (Domain, Infrastructure y API), utilizando tecnologías robustas orientadas a sistemas empresariales.

---

## 🧱 Stack Tecnológico

| Componente | Tecnología | Versión |
|----------|--------|--------|
| Lenguaje principal | C# | .NET 9 |
| Backend | ASP.NET Core Web API | .NET 9 |
| ORM | Entity Framework Core | 9.0.14 |
| Base de datos | PostgreSQL | Última estable |
| Provider PostgreSQL | Npgsql.EntityFrameworkCore.PostgreSQL | 9.0.4 |
| Autenticación | JWT (Bearer) | 9.0.14 |
| Librería JWT | System.IdentityModel.Tokens.Jwt | - |
| Frontend | Blazor WebAssembly | .NET 9 |
| IDE | JetBrains Rider | - |

---

## 🏗️ Arquitectura del Sistema

El sistema sigue una arquitectura por capas con separación de responsabilidades:

```text
Clinica.Domain        → Núcleo del negocio (entidades, reglas, enums)
Clinica.Infrastructure → Acceso a datos (EF Core, repositorios)
Clinica.API           → Exposición de servicios (Controllers, Services, Auth)
Clinica.WASM          → Interfaz de usuario (Frontend)


🧠 Descripción de Capas
🔹 1. Domain (Dominio)

Contiene la lógica central del sistema.

Incluye:

Entities → Modelos del negocio (Paciente, Cita, Atención, Pago, etc.)
Enums → Estados del sistema (EstadoCita, EstadoPago, etc.)
Interfaces → Contratos de repositorios (IPacienteRepository, ICitaRepository, etc.)
DTOs → Objetos de transferencia de datos entre capas

👉 Esta capa NO depende de ninguna otra, es el núcleo del sistema.

🔹 2. Infrastructure (Infraestructura)

Se encarga del acceso a la base de datos.

Incluye:

ApplicationDbContext → Configuración de EF Core
Configurations → Fluent API para mapear entidades
Repositories → Implementación de acceso a datos
Migrations → Versionado de la base de datos
Seeds (DataSeeder) → Datos iniciales del sistema

👉 Esta capa implementa las interfaces definidas en Domain.

🔹 3. API (Aplicación)

Expone la lógica del sistema mediante endpoints REST.

Incluye:

Controllers → Endpoints HTTP (REST)
Services → Lógica de negocio (casos de uso)
AuthService → Autenticación de usuarios
JwtHelper → Generación de tokens JWT
Helpers → Utilidades (fechas, seguridad, etc.)

👉 Aquí ocurre el flujo principal del sistema.

🔄 Flujo Principal del Backend

El sistema está diseñado para gestionar el ciclo completo de atención clínica:

Paciente → Cita → Atención → Pago → Historial Clínico
📌 Flujo detallado:
Registro de Paciente
Se crea un paciente
Se genera automáticamente su historial clínico
Programación de Cita
Se agenda una cita con doctor y servicio
Se registra en historial clínico
Registro de Atención
Se ejecuta la consulta médica
Se genera diagnóstico, tratamiento e indicaciones
Se actualiza historial
Registro de Pago
Se registra pago total o parcial
Se actualiza saldo
Se registra en historial
Historial Clínico
Guarda TODO:
Apertura
Citas
Atenciones
Pagos
Permite trazabilidad completa
🔐 Seguridad

El sistema implementa autenticación mediante:

JWT (JSON Web Token)
Claims incluidos:
Usuario
Roles
Permisos

Ejemplo de claims:

NameIdentifier → ID del usuario
Role → Rol del usuario
permiso → Permisos asignados
🗄️ Repositorios (Responsabilidad)

Cada repositorio cumple una función específica:

Repositorio	Responsabilidad
UsuarioRepository	Gestión de usuarios y autenticación
RolRepository	Gestión de roles
PermisoRepository	Gestión de permisos
PacienteRepository	Gestión de pacientes
DoctorRepository	Gestión de doctores
CitaRepository	Gestión de citas
AtencionRepository	Gestión de atenciones médicas
PagoRepository	Gestión de pagos
HistorialClinicoRepository	Gestión del historial clínico
HistorialDetalleRepository	Registro de eventos clínicos
ServicioClinicoRepository	Catálogo de servicios
AuditoriaRepository	Registro de acciones del sistema
📊 Auditoría del Sistema

El sistema registra:

Creación de registros
Modificación de datos
Eliminaciones
Acciones por usuario

Ejemplo:

Quién registró una cita
Quién registró una atención
Quién modificó un historial
Quién registró un pago
🧪 Estado Actual del Sistema
✔ Backend completamente funcional
✔ Base de datos operativa
✔ Flujo clínico completo probado
✔ Autenticación JWT implementada
✔ Historial clínico dinámico funcionando
🚀 Próximos pasos
Integración con frontend (Blazor)
Autorización por permisos
Mejora de seguridad (BCrypt)
Dashboard clínico
Reportes
📌 Conclusión

El sistema desarrollado cumple con una arquitectura escalable, segura y orientada a sistemas clínicos reales, permitiendo gestionar de forma eficiente pacientes, citas, atenciones y pagos, manteniendo trazabilidad completa mediante el historial clínico.


---

🔥 Esto ya es **documentación nivel tesis / proyecto profesional real**.

---

# 👉 Siguiente

Ahora sí:

```text
👉 Probar login JWT (debería darte token largo)

y si sale bien:

👉 Paso final: proteger endpoints con [Authorize]