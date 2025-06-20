using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Models;

namespace SistemaPrestamos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PagosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("registrar")]
        public IActionResult RegistrarPago([FromBody] Pago pago)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_RegistrarPago", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPagoProgramado", pago.IdPagoProgramado);
                cmd.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                cmd.Parameters.AddWithValue("@MontoPagado", pago.MontoPagado);
                cmd.Parameters.AddWithValue("@MedioPago", pago.MedioPago);
                cmd.Parameters.AddWithValue("@Observaciones", pago.Observaciones ?? "");

                try
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return Ok(new { mensaje = reader["Mensaje"].ToString() });
                    }

                    return BadRequest("No se recibió respuesta del procedimiento.");
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpGet("consultar")]
        public IActionResult ConsultarPagos([FromQuery] int? idCliente, [FromQuery] int? idPrestamo,
                                    [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<object> pagos = new List<object>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ConsultarPagos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", (object)idCliente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdPrestamo", (object)idPrestamo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Desde", (object)desde ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Hasta", (object)hasta ?? DBNull.Value);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pagos.Add(new
                            {
                                fechaPago = Convert.ToDateTime(reader["FechaPago"]),
                                montoPagado = Convert.ToDecimal(reader["MontoPagado"]),
                                medioPago = reader["MedioPago"].ToString(),
                                observaciones = reader["Observaciones"].ToString(),
                                estado = reader["Estado"].ToString()
                            });
                        }
                    }

                    return Ok(pagos);
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpGet("detalle")]
        public IActionResult ObtenerPago([FromQuery] int idPago)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ConsultarPagoPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPago", idPago);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Ok(new
                            {
                                fecha = reader["FechaPago"],
                                monto = reader["MontoPagado"],
                                medio = reader["MedioPago"],
                                estado = reader["Estado"]
                            });
                        }

                        return NotFound("Pago no encontrado.");
                    }
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpDelete("eliminar")]
        public IActionResult EliminarPago([FromQuery] int idPago)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_EliminarPago", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPago", idPago);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Pago eliminado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpPut("modificar")]
        public IActionResult ModificarPago([FromBody] Pago pago)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ModificarPago", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPago", pago.IdPago);
                cmd.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                cmd.Parameters.AddWithValue("@MontoPagado", pago.MontoPagado);
                cmd.Parameters.AddWithValue("@MedioPago", pago.MedioPago);
                cmd.Parameters.AddWithValue("@Observaciones", pago.Observaciones ?? "");

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Pago modificado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    }
}
