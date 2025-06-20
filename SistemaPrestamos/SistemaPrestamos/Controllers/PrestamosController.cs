using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Models;

namespace SistemaPrestamos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PrestamosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("agregar")]
        public IActionResult AgregarPrestamo([FromBody] Prestamo prestamo)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AgregarPrestamo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", prestamo.IdCliente);
                cmd.Parameters.AddWithValue("@IdTasa", prestamo.IdTasa);
                cmd.Parameters.AddWithValue("@MontoPrestamo", prestamo.MontoPrestamo);
                cmd.Parameters.AddWithValue("@PlazoMeses", prestamo.PlazoMeses);
                cmd.Parameters.AddWithValue("@FechaInicio", prestamo.FechaInicio);
                cmd.Parameters.AddWithValue("@DiaPago", prestamo.DiaPago);

                try
                {
                    conn.Open();
                    var result = cmd.ExecuteReader();
                    if (result.Read())
                    {
                        return Ok(new
                        {
                            IdPrestamo = result["IdPrestamo"],
                            mensaje = result["Mensaje"].ToString()
                        });
                    }

                    return BadRequest("No se recibió respuesta del procedimiento.");
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpPost("generar-calendario")]
        public IActionResult GenerarCalendario([FromQuery] int idPrestamo)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GenerarCalendarioPagos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);

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
        [HttpGet("calendario")]
        public IActionResult ConsultarCalendario([FromQuery] int idPrestamo)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<CalendarioPago> calendario = new List<CalendarioPago>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ConsultarCalendarioPagos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            calendario.Add(new CalendarioPago
                            {
                                FechaPago = Convert.ToDateTime(reader["FechaPago"]),
                                MontoCuota = Convert.ToDecimal(reader["MontoCuota"]),
                                Interes = Convert.ToDecimal(reader["Interes"]),
                                Amortizacion = Convert.ToDecimal(reader["Amortizacion"]),
                                Saldo = Convert.ToDecimal(reader["Saldo"]),
                                Estado = reader["Estado"].ToString()
                            });
                        }
                    }

                    return Ok(calendario);
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpPut("modificar")]
        public IActionResult ModificarPrestamo([FromBody] Prestamo prestamo)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ModificarPrestamo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPrestamo", prestamo.IdPrestamo);
                cmd.Parameters.AddWithValue("@MontoPrestamo", prestamo.MontoPrestamo);
                cmd.Parameters.AddWithValue("@PlazoMeses", prestamo.PlazoMeses);
                cmd.Parameters.AddWithValue("@FechaInicio", prestamo.FechaInicio);
                cmd.Parameters.AddWithValue("@DiaPago", prestamo.DiaPago);
                cmd.Parameters.AddWithValue("@SaldoPendiente", prestamo.SaldoPendiente);
                cmd.Parameters.AddWithValue("@Estado", prestamo.Estado);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Préstamo modificado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpDelete("eliminar")]
        public IActionResult EliminarPrestamo([FromQuery] int idPrestamo)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_EliminarPrestamo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Préstamo eliminado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpGet("consultar")]
        public IActionResult ConsultarPrestamos()
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<object> prestamos = new List<object>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ConsultarPrestamos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prestamos.Add(new
                            {
                                idPrestamo = Convert.ToInt32(reader["IdPrestamo"]),
                                cliente = reader["NombreCompleto"].ToString(),
                                monto = Convert.ToDecimal(reader["MontoPrestamo"]),
                                plazoMeses = Convert.ToInt32(reader["PlazoMeses"]),
                                fechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                saldoPendiente = Convert.ToDecimal(reader["SaldoPendiente"]),
                                estado = reader["Estado"].ToString()
                            });
                        }
                    }

                    return Ok(prestamos);
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpGet("detalle")]
        public IActionResult DetallePrestamo([FromQuery] int idPrestamo)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ConsultarPrestamoPorId", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamo);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Ok(new
                            {
                                idPrestamo = reader["IdPrestamo"],
                                cliente = reader["NombreCompleto"].ToString(),
                                monto = reader["MontoPrestamo"],
                                plazo = reader["PlazoMeses"],
                                fecha = reader["FechaInicio"],
                                saldoPendiente = reader["SaldoPendiente"],
                                estado = reader["Estado"]
                            });
                        }

                        return NotFound("Préstamo no encontrado.");
                    }
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    }
}
