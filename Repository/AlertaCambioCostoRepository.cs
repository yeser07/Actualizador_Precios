using Actualizador_Precios.Data;
using Actualizador_Precios.Models;
using Actualizador_Precios.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Actualizador_Precios.Repository
{
    public class AlertaCambioCostoRepository
    {
        private readonly AppDBContext _context;

        public AlertaCambioCostoRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<AlertaCambioCosto>> GetAlertasAsync()
        {
            return await _context.AlertaCambioCosto
                .Where(a => a.Revisado != true)
                .OrderByDescending(a => a.FechaCambio)
                .ToListAsync();
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        public async Task<EditorPreciosFormModel?> GetEditorPreciosAsync(int idAlerta)
        {
            var alerta = await _context.AlertaCambioCosto
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IdAlerta == idAlerta);

            if (alerta == null || alerta.Revisado == true)
                return null;
            if (string.IsNullOrWhiteSpace(alerta.CVE_ART))
                return null;

            var cveArt = alerta.CVE_ART.Trim();

            var precios = await _context.PrecioProductos
                .AsNoTracking()
                .Where(p => p.CVE_ART == cveArt)
                .Include(p => p.ListaPrecio)
                .OrderBy(p => p.CVE_PRECIO)
                .ToListAsync();

            var lineas = precios.Select(p => new EditorPrecioLineaModel
            {
                CvePrecio = p.CVE_PRECIO,
                DescripcionLista = p.ListaPrecio != null && !string.IsNullOrEmpty(p.ListaPrecio.DESCRIPCION)
                    ? p.ListaPrecio.DESCRIPCION
                    : p.CVE_PRECIO.ToString(),
                PrecioActual = Convert.ToDecimal(p.PRECIO),
                PrecioNuevo = Convert.ToDecimal(p.PRECIO)
            }).ToList();

            return new EditorPreciosFormModel
            {
                IdAlerta = alerta.IdAlerta,
                CVE_ART = cveArt,
                Descripcion = alerta.Descripcion ?? string.Empty,
                Lineas = lineas
            };
        }

        public async Task GuardarPreciosYMarcarRevisadaAsync(
            int idAlerta,
            string? cveArtSolicitado,
            IReadOnlyList<EditorPrecioLineaModel> lineas)
        {
            var alerta = await _context.AlertaCambioCosto
                .FirstOrDefaultAsync(a => a.IdAlerta == idAlerta);

            if (alerta == null)
                throw new InvalidOperationException("La alerta no existe.");
            if (alerta.Revisado == true)
                throw new InvalidOperationException("La alerta ya fue revisada.");
            if (string.IsNullOrWhiteSpace(alerta.CVE_ART))
                throw new InvalidOperationException("La alerta no tiene clave de artículo.");
            if (string.IsNullOrWhiteSpace(cveArtSolicitado) ||
                !string.Equals(alerta.CVE_ART.Trim(), cveArtSolicitado.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("El artículo no coincide con la alerta.");

            var cveArt = alerta.CVE_ART.Trim();

            foreach (var linea in lineas)
            {
                if (linea.PrecioNuevo < 0)
                    throw new InvalidOperationException("El precio no puede ser negativo.");
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var linea in lineas)
                {
                    var tracked = await _context.PrecioProductos.FirstOrDefaultAsync(p =>
                        p.CVE_ART == cveArt && p.CVE_PRECIO == linea.CvePrecio);
                    if (tracked != null)
                        tracked.PRECIO = decimal.ToDouble(linea.PrecioNuevo);
                }

                alerta.Revisado = true;
                alerta.FechaRevision = DateTime.Now;

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
