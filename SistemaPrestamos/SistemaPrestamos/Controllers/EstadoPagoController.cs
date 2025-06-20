using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Models;

namespace SistemaPrestamos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoPagoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EstadoPagoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("listar")]
        public IActionResult ListarEstados()
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<object> estados = new List<object>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ConsultarEstadoPago", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        estados.Add(new
                        {
                            idEstado = Convert.ToInt32(reader["IdEstado"]),
                            estado = reader["NombreEstado"].ToString()
                        });
                    }
                    return Ok(estados);
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }

        [HttpPost("agregar")]
        public IActionResult AgregarEstado([FromBody] EstadoPago estado)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AgregarEstadoPago", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreEstado", estado.NombreEstado);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Estado de pago agregado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    }
}

   