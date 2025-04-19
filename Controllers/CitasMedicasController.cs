using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;

public class CitasMedicasController : Controller
{
    private readonly CitaMedicaDAL _citaDAL;
    private readonly DoctorDAL _doctorDAL;

    public CitasMedicasController(IConfiguration config)
    {
        var cadena = config.GetConnectionString("ConexionProyectoFinal") ?? string.Empty;
        _citaDAL = new CitaMedicaDAL(cadena);
        _doctorDAL = new DoctorDAL(cadena);
    }

    public IActionResult Index()
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = false;
        return View(_citaDAL.ObtenerTodas());
    }

    public IActionResult AgregarEditar(int? id)
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = true;
        ViewBag.UrlVolver = Url.Action("Index", "CitasMedicas");

        ViewBag.Doctores = _doctorDAL.ObtenerTodos();
        return View(id == null ? new CitaMedica() : _citaDAL.ObtenerPorId(id.Value));
    }

    [HttpPost]
    public IActionResult AgregarEditar(CitaMedica cita)
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = true;
        ViewBag.UrlVolver = Url.Action("Index", "CitasMedicas");

        ViewBag.Doctores = _doctorDAL.ObtenerTodos();

        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Todos los campos son obligatorios.";
            return View(cita);
        }

        if (!_citaDAL.DoctorDisponible(cita.DoctorID, cita.Fecha, cita.CitaID > 0 ? (int?)cita.CitaID : null))
        {
            ViewBag.Error = "El doctor ya tiene una cita en esa fecha.";
            return View(cita);
        }

        if (cita.CitaID == 0)
            _citaDAL.Insertar(cita);
        else
            _citaDAL.Actualizar(cita);

        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int id)
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = true;
        ViewBag.UrlVolver = Url.Action("Index", "CitasMedicas");

        _citaDAL.Eliminar(id);
        return RedirectToAction("Index");
    }
}
