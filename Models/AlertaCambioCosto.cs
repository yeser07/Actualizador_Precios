using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Actualizador_Precios.Models
{
    [Table("Alerta_Cambio_Costo")]
    public class AlertaCambioCosto
    {
        [Key]
        public int IdAlerta { get; set; }

        [StringLength(50)]
        public string? CVE_ART { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }

        public decimal? CostoProm_Ant { get; set; }
        public decimal? CostoProm_Nvo { get; set; }
        public decimal? UltCosto_Ant { get; set; }
        public decimal? UltCosto_Nvo { get; set; }

        public DateTime? FechaCambio { get; set; }

        public bool? Revisado { get; set; }

        public DateTime? FechaRevision { get; set; }
    }
}
