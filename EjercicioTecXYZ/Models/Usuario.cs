using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EjercicioTecXYZ.Models
{
    public class Usuario
    {
        [Key]
        public int ID_Trabajador { get; set; }
        public string? Nombre { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Telefono { get; set; }
        public string? Puesto { get; set; }
        public string? Rol { get; set; }

        [ForeignKey("Puesto")]
        public virtual Puesto? PuestoNavigation { get; set; }

        [ForeignKey("Roles")]
        public virtual Roles? RolNavigation { get; set; }
    }
}
