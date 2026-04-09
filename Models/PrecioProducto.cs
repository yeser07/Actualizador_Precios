using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Actualizador_Precios.Models
{
    [Table("PRECIO_X_PROD01")]
    public class PrecioProducto
    {
        [StringLength(16)]
        public string CVE_ART { get; set; } = string.Empty;

        public int CVE_PRECIO { get; set; }

        /// <summary>En SQL Server la columna es tipo float.</summary>
        public double PRECIO { get; set; }

        [StringLength(50)]
        public string? UUID { get; set; }

        public DateTime? VERSION_SINC { get; set; }

        public ListaPrecio? ListaPrecio { get; set; }
    }
}
