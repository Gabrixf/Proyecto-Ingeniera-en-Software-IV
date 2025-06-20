using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Models;

namespace SistemaPrestamos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasaInteresController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TasaInteresController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("listar")]
        public IActionResult ObtenerTasas()
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<object> tasas = new List<object>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ConsultarTasaInteres", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasas.Add(new
                            {
                                idTasa = reader["IdTasa"],
                                tasaAnual = reader["TasaAnual"],
                                tasaMoratoria = reader["TasaMoratoria"],
                                vigente = reader["Vigente"]
                            });
                        }
                    }
                    return Ok(tasas);
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }

        [HttpPost("agregar")]
        public IActionResult AgregarTasa([FromBody] TasaInteres tasa)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AgregarTasaInteres", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TasaAnual", tasa.TasaAnual);
                cmd.Parameters.AddWithValue("@TasaMoratoria", tasa.TasaMoratoria);
                cmd.Parameters.AddWithValue("@Vigente", tasa.Vigente);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Tasa agregada correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }

        [HttpPut("modificar")]
        public IActionResult ModificarTasa([FromBody] TasaInteres tasa)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ModificarTasaInteres", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdTasa", tasa.IdTasa);
                cmd.Parameters.AddWithValue("@TasaAnual", tasa.TasaAnual);
                cmd.Parameters.AddWithValue("@TasaMoratoria", tasa.TasaMoratoria);
                cmd.Parameters.AddWithValue("@Vigente", tasa.Vigente);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Tasa modificada correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }

        [HttpDelete("eliminar")]
        public IActionResult EliminarTasa([FromQuery] int idTasa)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_EliminarTasaInteres", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdTasa", idTasa);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Tasa eliminada correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    }
}
