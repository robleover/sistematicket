using Microsoft.AspNetCore.Mvc;

namespace SistemaTicket.Controllers
{
    public class TicketsController : Controller
    {
        // 7.1 Lista de tickets (ciclo de vida, estados, prioridades)
        public IActionResult Index() => View();

        // 7.2 / 7.4 Crear ticket (portal web / interfaz agente)
        public IActionResult Create() => View();

        // 7.5–7.11 Vista detalle completa del ticket
        public IActionResult Detail(int id = 100249) => View();
    }
}
