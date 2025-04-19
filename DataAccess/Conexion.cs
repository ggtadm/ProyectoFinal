using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class Conexion
{
    private readonly string cadena;

    public Conexion(IConfiguration configuration)
    {
        cadena = configuration.GetConnectionString("ConexionProyectoFinal") ?? string.Empty;
    }

    public SqlConnection ObtenerConexion()
    {
        return new SqlConnection(cadena);
    }
}

