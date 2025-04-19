using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ProyectoFinal.Models;

public class CitaMedicaDAL
{
    private readonly string _cadenaConexion;

    public CitaMedicaDAL(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    public List<CitaMedica> ObtenerTodas()
    {
        var lista = new List<CitaMedica>();

        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand(@"
                SELECT C.CitaID, C.Paciente, C.DoctorID, D.Nombre AS NombreDoctor, C.Fecha, C.Motivo
                FROM CitasMedicas C
                JOIN Doctores D ON C.DoctorID = D.DoctorID", con);

            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lista.Add(new CitaMedica
                    {
                        CitaID = (int)dr["CitaID"],
                        Paciente = dr["Paciente"]?.ToString() ?? string.Empty,
                        DoctorID = (int)dr["DoctorID"],
                        NombreDoctor = dr["NombreDoctor"]?.ToString() ?? string.Empty,
                        Fecha = (DateTime)dr["Fecha"],
                        Motivo = dr["Motivo"]?.ToString() ?? string.Empty
                    });
                }
            }
        }

        return lista;
    }

    // ✅ Corrección: método puede retornar null explícitamente con '?'
    public CitaMedica? ObtenerPorId(int id)
    {
        CitaMedica? cita = null;

        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("SELECT * FROM CitasMedicas WHERE CitaID=@id", con);
            cmd.Parameters.AddWithValue("@id", id);

            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    cita = new CitaMedica
                    {
                        CitaID = (int)dr["CitaID"],
                        Paciente = dr["Paciente"]?.ToString() ?? string.Empty,
                        DoctorID = (int)dr["DoctorID"],
                        Fecha = (DateTime)dr["Fecha"],
                        Motivo = dr["Motivo"]?.ToString() ?? string.Empty,
                        NombreDoctor = string.Empty // no viene en este query, lo inicializamos vacío
                    };
                }
            }
        }

        return cita;
    }

    public void Insertar(CitaMedica cita)
    {
        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("INSERT INTO CitasMedicas (Paciente, DoctorID, Fecha, Motivo) VALUES (@paciente, @doctor, @fecha, @motivo)", con);
            cmd.Parameters.AddWithValue("@paciente", cita.Paciente);
            cmd.Parameters.AddWithValue("@doctor", cita.DoctorID);
            cmd.Parameters.AddWithValue("@fecha", cita.Fecha);
            cmd.Parameters.AddWithValue("@motivo", cita.Motivo);
            cmd.ExecuteNonQuery();
        }
    }

    public void Actualizar(CitaMedica cita)
    {
        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("UPDATE CitasMedicas SET Paciente=@paciente, DoctorID=@doctor, Fecha=@fecha, Motivo=@motivo WHERE CitaID=@id", con);
            cmd.Parameters.AddWithValue("@paciente", cita.Paciente);
            cmd.Parameters.AddWithValue("@doctor", cita.DoctorID);
            cmd.Parameters.AddWithValue("@fecha", cita.Fecha);
            cmd.Parameters.AddWithValue("@motivo", cita.Motivo);
            cmd.Parameters.AddWithValue("@id", cita.CitaID);
            cmd.ExecuteNonQuery();
        }
    }

    public void Eliminar(int id)
    {
        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var cmd = new SqlCommand("DELETE FROM CitasMedicas WHERE CitaID=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public bool DoctorDisponible(int doctorID, DateTime fecha, int? citaId = null)
    {
        using (var con = new SqlConnection(_cadenaConexion))
        {
            con.Open();
            var query = "SELECT COUNT(*) FROM CitasMedicas WHERE DoctorID=@id AND Fecha=@fecha";
            if (citaId != null)
            {
                query += " AND CitaID != @citaId";
            }

            var cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", doctorID);
            cmd.Parameters.AddWithValue("@fecha", fecha);
            if (citaId != null)
                cmd.Parameters.AddWithValue("@citaId", citaId);

            int count = (int)cmd.ExecuteScalar();
            return count == 0;
        }
    }
}


