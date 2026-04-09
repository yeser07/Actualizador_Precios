using System.ComponentModel.DataAnnotations;

namespace Actualizador_Precios.Models.ViewModels
{
    public class EditorPreciosFormModel
    {
        public int IdAlerta { get; set; }

        [Required(ErrorMessage = "La clave de artículo es obligatoria.")]
        [StringLength(50)]
        public string CVE_ART { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        public List<EditorPrecioLineaModel> Lineas { get; set; } = new();
    }

    public class EditorPrecioLineaModel
    {
        [Required]
        public int CvePrecio { get; set; }

        public string DescripcionLista { get; set; } = string.Empty;

        public decimal PrecioActual { get; set; }

        public decimal PrecioNuevo { get; set; }
    }
}
