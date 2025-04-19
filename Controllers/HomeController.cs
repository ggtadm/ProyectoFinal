using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.MostrarCerrarSesion = true;
        ViewBag.MostrarVolver = false;
        return View();
    }
}

