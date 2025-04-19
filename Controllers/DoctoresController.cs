using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;

public class DoctoresController : Controller
{
    private readonly DoctorDAL _doctorDAL;

    public DoctoresController(IConfiguration config)
    {
        var cadena = config.GetConnectionString("ConexionProyectoFinal") ?? string.Empty;
        _doctorDAL = new DoctorDAL(cadena);
    }

    // Mostrar todos los doctores
    public IActionResult Index()
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = false;
        return View(_doctorDAL.ObtenerTodos());
    }

    // Mostrar formulario para agregar un nuevo doctor
    public IActionResult Crear()
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = true;
        ViewBag.UrlVolver = Url.Action("Index", "Doctores");

        return View(new Doctor());
    }

    // Guardar nuevo doctor en base de datos
    [HttpPost]
    public IActionResult Crear(Doctor doctor)
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = true;
        ViewBag.UrlVolver = Url.Action("Index", "Doctores");

        if (!ModelState.IsValid)
        {
            ViewBag.Error = "El nombre del doctor es obligatorio.";
            return View(doctor);
        }

        _doctorDAL.Insertar(doctor);
        return RedirectToAction("Index");
    }

    // Mostrar formulario para editar un doctor
    public IActionResult Editar(int id)
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = true;
        ViewBag.UrlVolver = Url.Action("Index", "Doctores");

        return View(_doctorDAL.ObtenerPorId(id));
    }

    // Guardar cambios al editar un doctor
    [HttpPost]
    public IActionResult Editar(Doctor doctor)
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = true;
        ViewBag.UrlVolver = Url.Action("Index", "Doctores");

        if (!ModelState.IsValid)
        {
            ViewBag.Error = "El nombre del doctor es obligatorio.";
            return View(doctor);
        }

        _doctorDAL.Actualizar(doctor);
        return RedirectToAction("Index");
    }
}



