namespace psicomedixMonolito.Models;

public class ApiResponse<T>
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public int Codigo { get; set; }
    public T? Data { get; set; }
    public List<string> Errores { get; set; } = [];
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> Ok(T data, string mensaje = "Operación realizada correctamente.", int codigo = 200)
    {
        return new ApiResponse<T>
        {
            Exitoso = true,
            Mensaje = mensaje,
            Codigo = codigo,
            Data = data
        };
    }

    public static ApiResponse<T> Error(string mensaje, int codigo, List<string>? errores = null)
    {
        return new ApiResponse<T>
        {
            Exitoso = false,
            Mensaje = mensaje,
            Codigo = codigo,
            Errores = errores ?? []
        };
    }
}