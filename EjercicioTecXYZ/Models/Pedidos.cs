using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EjercicioTecXYZ.Models
{
    public class Pedidos
    {
        [Key]
        public int Nro_Pedido { get; set; }
        public string Productos { get; set; } // JSON se representa como string
        public DateTime Fecha_pedido { get; set; }

        public DateTime? Fecha_recepcion { get; set; }
        public DateTime? Fecha_despacho { get; set; }
        public DateTime? Fecha_entrega { get; set; }

        public int? Vendedor { get; set; }
        public int? Repartidor { get; set; }
        public int? Estado { get; set; }

        [ForeignKey("Vendedor")]
        public virtual Usuario VendedorNavigation { get; set; }

        [ForeignKey("Repartidor")]
        public virtual Usuario RepartidorNavigation { get; set; }

        [ForeignKey("Estado")]
        public virtual EstadoPedido EstadoNavigation { get; set; }
    }
}
