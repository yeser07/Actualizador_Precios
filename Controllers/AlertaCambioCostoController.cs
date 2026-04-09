using Actualizador_Precios.Models;
using Actualizador_Precios.Models.ViewModels;
using Actualizador_Precios.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Actualizador_Precios.Controllers
{
    public class AlertaCambioCostoController : Controller
    {
        private readonly AlertaCambioCostoRepository _repository;

        public AlertaCambioCostoController(AlertaCambioCostoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            List<AlertaCambioCosto> alertas = new();

            try
            {
                _ = await _repository.TestConnectionAsync();
                alertas = await _repository.GetAlertasAsync();
                return View("~/Views/AlertaCambioCosto/Index.cshtml", alertas);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar las alertas. Intente nuevamente. " + ex.Message;
                return View("~/Views/AlertaCambioCosto/Index.cshtml", alertas);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditorPrecios(int idAlerta)
        {
            var model = await _repository.GetEditorPreciosAsync(idAlerta);
            if (model == null)
                return Content("<div class=\"alert alert-warning m-0\">La alerta no existe o ya fue revisada.</div>", "text/html; charset=utf-8");

            return PartialView("~/Views/AlertaCambioCosto/_EditorPreciosForm.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarPrecios(EditorPreciosFormModel model)
        {
            model.Lineas ??= new List<EditorPrecioLineaModel>();

            if (model.IdAlerta <= 0 || string.IsNullOrWhiteSpace(model.CVE_ART))
            {
                TempData["ErrorMessage"] = "Datos de alerta no válidos.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var linea in model.Lineas)
            {
                if (linea.PrecioNuevo < 0)
                {
                    TempData["ErrorMessage"] = "Hay precios nuevos negativos. Revise los importes.";
                    return RedirectToAction(nameof(Index));
                }
            }

            try
            {
                await _repository.GuardarPreciosYMarcarRevisadaAsync(model.IdAlerta, model.CVE_ART, model.Lineas);
                TempData["Message"] = "Precios actualizados y alerta marcada como revisada.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "No se pudieron guardar los cambios: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
