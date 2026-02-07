using Microsoft.AspNetCore.Mvc;
using Actualizador_Precios.Repository;
using Actualizador_Precios.Models;
using System.Threading.Tasks;

namespace Actualizador_Precios.Controllers
{
    public class AlertaCambioCostoController : Controller
    {
        private readonly AlertaCambioCostoRepository AlertaCambioCostoRepository;

        public AlertaCambioCostoController(AlertaCambioCostoRepository alertaCambioCostoRepository)
        {
            AlertaCambioCostoRepository = alertaCambioCostoRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<AlertaCambioCosto> alertas = new List<AlertaCambioCosto>();

            try
                {
                var canConnect = await AlertaCambioCostoRepository.TestConnectionAsync();
                Console.WriteLine($"DB conectada: {canConnect}");

                alertas = await AlertaCambioCostoRepository.GetAlertasAsync();

                return View("~/Views/AlertaCambioCosto/Index.cshtml", alertas);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar las alertas. Intente nuevamente. " + ex.Message;

                return View("~/Views/AlertaCambioCosto/Index.cshtml", alertas);
            }

        }
    }
}
