namespace Actualizador_Precios.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRECIOS01")]
    public class ListaPrecio
    {
        [Key]
        [StringLength(20)]
        public string CVE_PRECIO { get; set; }

        [StringLength(100)]
        public string DESCRIPCION { get; set; }

        [StringLength(10)]
        public string STATUS { get; set; }

        [StringLength(50)]
        public string UUID { get; set; }

        public int VERSION_SINC { get; set; }

        public bool CON_IMPU { get; set; }
    }

}
