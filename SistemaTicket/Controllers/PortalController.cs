using Microsoft.AspNetCore.Mvc;

namespace SistemaTicket.Controllers
{
    public class PortalController : Controller
    {
        // Redirige al portal público principal
        public IActionResult Index() => RedirectToAction("Index", "Home");

        // 8. Abrir nuevo ticket (usuario final)
        public IActionResult NewTicket() => View();

        // 8. Ver mis tickets
        public IActionResult MyTickets() => View();

        // 8. Detalle de ticket para usuario final
        public IActionResult TicketDetail(int id = 100249) => View();
    }
}
