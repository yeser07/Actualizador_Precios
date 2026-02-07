using Actualizador_Precios.Data;
using Actualizador_Precios.Models;
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
                .Where(a => !a.Revisado)
                .OrderByDescending(a => a.FechaCambio)
                .ToListAsync();
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                bool canConnect = await _context.Database.CanConnectAsync();
                Console.WriteLine($"Conexión a DB: {canConnect}");
                return canConnect;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }


    }
}
