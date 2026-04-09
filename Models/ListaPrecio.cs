using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Actualizador_Precios.Models
{
    [Table("PRECIOS01")]
    public class ListaPrecio
    {
        [Key]
        public int CVE_PRECIO { get; set; }

        [StringLength(25)]
        public string DESCRIPCION { get; set; } = string.Empty;

        public int? CVE_BITA { get; set; }

        [StringLength(1)]
        public string? STATUS { get; set; }

        [StringLength(50)]
        public string? UUID { get; set; }

        public DateTime? VERSION_SINC { get; set; }

        [StringLength(1)]
        public string? CON_IMPU { get; set; }
    }
}
