using Microsoft.AspNetCore.Mvc;
using EjercicioTecXYZ.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using MySqlConnector;
using System.Data;

namespace EjercicioTecXYZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly string secretKey;
        private readonly string cadenaMySQL;
        public AuthenticateController(IConfiguration config)
        {
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
            cadenaMySQL = config.GetConnectionString("CadenaMySQL");
        }
        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] Usuario request)
        {
            var usuario = ObtenerUsuarioDesdeBD(request.Email, request.Password);

            if (usuario != null)
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Email));

                // Agregar el rol del usuario como un claim al token
                claims.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokencreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokencreado });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
        }
        private Usuario ObtenerUsuarioDesdeBD(string email, string password)
        {
            Usuario usuario = null;

            try
            {
                using (var conexion = new MySqlConnection(cadenaMySQL))
                {
                    conexion.Open();
                    var cmd = new MySqlCommand("sp_validar_usuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("p_email", email);
                    cmd.Parameters.AddWithValue("p_password", password);

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            usuario = new Usuario
                            {
                                Email = rd["Email"].ToString(),
                                Password = rd["Password"].ToString(),
                                Rol = rd["Rol"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                // Por ejemplo, puedes registrar el error en un archivo de registro o en la consola
                Console.WriteLine("Error al obtener usuario desde la base de datos: " + ex.Message);
            }

            return usuario;
        }
    }
}
