namespace psicomedixMonolito.Utils.Helpers;

public static class FechaHelper
{
    public static DateTime ToUtc(DateTime fecha)
    {
        return fecha.Kind switch
        {
            DateTimeKind.Utc => fecha,
            DateTimeKind.Local => fecha.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(fecha, DateTimeKind.Utc),
            _ => fecha
        };
    }

    public static DateTime? ToUtc(DateTime? fecha)
    {
        return fecha.HasValue ? ToUtc(fecha.Value) : null;
    }
}