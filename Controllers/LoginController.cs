using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.DataAccess;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _cadenaConexion;

        public LoginController(IConfiguration config)
        {
            _cadenaConexion = config.GetConnectionString("ConexionProyectoFinal") ?? string.Empty;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Si ya hay sesión, redirige al Home
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Index(string NombreUsuario, string Contrasena)
        {
            var usuarioDAL = new UsuarioDAL(_cadenaConexion);
            var usuario = usuarioDAL.ValidarUsuario(NombreUsuario, Contrasena);

            if (usuario != null)
            {
                // Guardar el nombre en la sesión
                HttpContext.Session.SetString("Usuario", usuario.NombreUsuario ?? "usuario");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrecta";
            return View();
        }

        public IActionResult Logout()
        {
            // Limpiar sesión y redirigir al login
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}


