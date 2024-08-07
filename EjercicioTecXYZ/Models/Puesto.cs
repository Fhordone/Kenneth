using System.ComponentModel.DataAnnotations;

namespace EjercicioTecXYZ.Models
{
    public class Puesto
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }
}
