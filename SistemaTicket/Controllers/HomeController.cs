using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaTicket.Models;

namespace SistemaTicket.Controllers
{
    public class HomeController : Controller
    {
        // Página pública de presentación
        public IActionResult Index()
        {
            return View();
        }

        // Login (accede desde "Abrir ticket" o "Ver estado")
        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string? returnUrl)
        {
            if (username == "demo" && password == "demo")
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Admin");
            }

            ViewData["LoginError"] = "Usuario o contraseña incorrectos.";
            ViewData["Username"] = username;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
