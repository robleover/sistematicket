using Microsoft.AspNetCore.Mvc;

namespace SistemaTicket.Controllers
{
    public class AdminController : Controller
    {
        // Dashboard principal
        public IActionResult Index() => View();

        // 6.1 Ajustes del sistema
        public IActionResult SystemSettings() => View();

        // 6.2 Correo electrónico
        public IActionResult Email() => View();

        // 6.3 Formularios y campos personalizados
        public IActionResult Forms() => View();

        // 6.4 Departamentos
        public IActionResult Departments() => View();

        // 6.5 Equipos (Teams)
        public IActionResult Teams() => View();

        // 6.6 Agentes
        public IActionResult Agents() => View();

        // 6.7 Roles y permisos
        public IActionResult Roles() => View();

        // 6.8 Temas de ayuda (Help Topics)
        public IActionResult HelpTopics() => View();

        // 6.9 SLA — Acuerdos de nivel de servicio
        public IActionResult SLAPlans() => View();

        // 6.10 Horarios de atención
        public IActionResult Schedules() => View();

        // 6.11 Plugins
        public IActionResult Plugins() => View();
    }
}
