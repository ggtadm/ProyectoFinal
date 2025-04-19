namespace ProyectoFinal.Models
{
    public class CitaMedica
    {
        public int CitaID { get; set; }
        public string Paciente { get; set; } = string.Empty;
        public int DoctorID { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string NombreDoctor { get; set; } = string.Empty;
    }
}

