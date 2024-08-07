using Microsoft.AspNetCore.Mvc;
using EjercicioTecXYZ.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using MySqlConnector;

namespace EjercicioTecXYZ.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "4")]
    public class PedidoController : ControllerBase
    {
        private readonly string cadenaMySQL;
        public PedidoController(IConfiguration config)
        {
            cadenaMySQL = config.GetConnectionString("CadenaMySQL");
        }
        [HttpPut]
        [Route("UpdatePedido/{nro_pedido}")]
        public IActionResult UpdatePedido(int nro_pedido, [FromBody] Pedidos pedido)
        {
            if (nro_pedido <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El número de pedido es inválido." });
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(cadenaMySQL))
                {
                    connection.Open();
                    var cmd = new MySqlCommand("sp_actualizar_estado_pedido", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("p_Nro_Pedido", nro_pedido);
                    cmd.Parameters.AddWithValue("p_Nuevo_Estado", pedido.Estado);

                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Estado del pedido actualizado correctamente" });
            }
            catch (MySqlException sqlError)
            {
                if (sqlError.Message.Contains("El nuevo estado debe ser mayor que el estado actual."))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "El nuevo estado debe ser mayor que el estado actual." });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = $"Error SQL: {sqlError.Message}" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
