using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ProyectoFinal.Models;

public class DoctorDAL
{
    private readonly string _cadenaConexion;

    public DoctorDAL(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    public List<Doctor> ObtenerTodos()
    {
        var lista = new List<Doctor>();

        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("SELECT * FROM Doctores", con);
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lista.Add(new Doctor
                    {
                        DoctorID = (int)dr["DoctorID"],
                        Nombre = dr["Nombre"]?.ToString() ?? string.Empty
                    });
                }
            }
        }

        return lista;
    }

    public Doctor? ObtenerPorId(int id)
    {
        Doctor? doctor = null;

        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("SELECT * FROM Doctores WHERE DoctorID=@id", con);
            cmd.Parameters.AddWithValue("@id", id);

            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    doctor = new Doctor
                    {
                        DoctorID = (int)dr["DoctorID"],
                        Nombre = dr["Nombre"]?.ToString() ?? string.Empty
                    };
                }
            }
        }

        return doctor;
    }

    public void Insertar(Doctor doctor)
    {
        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("INSERT INTO Doctores (Nombre) VALUES (@nombre)", con);
            cmd.Parameters.AddWithValue("@nombre", doctor.Nombre);
            cmd.ExecuteNonQuery();
        }
    }

    public void Actualizar(Doctor doctor)
    {
        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("UPDATE Doctores SET Nombre=@nombre WHERE DoctorID=@id", con);
            cmd.Parameters.AddWithValue("@nombre", doctor.Nombre);
            cmd.Parameters.AddWithValue("@id", doctor.DoctorID);
            cmd.ExecuteNonQuery();
        }
    }

    public void Eliminar(int id)
    {
        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("DELETE FROM Doctores WHERE DoctorID=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}

