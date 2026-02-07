namespace Actualizador_Precios.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRECIO_X_PROD01")]
    public class PrecioProducto
    {
        [StringLength(50)]
        public string CVE_ART { get; set; }

        [StringLength(20)]
        public string CVE_PRECIO { get; set; }

        public decimal PRECIO { get; set; }

        [StringLength(50)]
        public string UUID { get; set; }

        public int VERSION_SINC { get; set; }

        // Navegación
        public ListaPrecio ListaPrecio { get; set; }
    }

}
