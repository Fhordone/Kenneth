using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EjercicioTecXYZ.Models
{
    public class Productos
    {
        [Key]
        public int SKU { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; }

        [Required]
        [StringLength(50)]
        public string Etiquetas { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Precio { get; set; }

        [Required]
        [StringLength(50)]
        public string Unidad_medida { get; set; }
    }
}
