using System.ComponentModel.DataAnnotations;

namespace EjercicioTecXYZ.Models
{
    public class Roles
    {
        [Key]
        public int ID { get; set; }

        public string Nombre { get; set; }
    }
}
