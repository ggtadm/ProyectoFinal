using Microsoft.Data.SqlClient;
using ProyectoFinal.Models;

namespace ProyectoFinal.DataAccess
{
    public class UsuarioDAL
    {
        private readonly string cadenaConexion;

        public UsuarioDAL(string cadena)
        {
            cadenaConexion = cadena;
        }

        //  devolver un objeto Usuario si existe
        public Usuario? ValidarUsuario(string nombreUsuario, string contrasena)
        {
            Usuario? usuario = null;

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                con.Open();

                string query = "SELECT UsuarioID, NombreUsuario, Contrasena FROM Usuarios WHERE NombreUsuario = @nombreUsuario AND Contrasena = @contrasena";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@contrasena", contrasena);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                                NombreUsuario = reader["NombreUsuario"]?.ToString() ?? string.Empty,
                                Contrasena = reader["Contrasena"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }

            return usuario;
        }
    }
}


