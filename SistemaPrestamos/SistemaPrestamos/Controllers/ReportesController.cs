using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static SistemaPrestamos.Models.Reportes;

namespace SistemaPrestamos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReportesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("resumen-pagos")]
        public IActionResult ResumenPagosPorCliente([FromQuery] int idCliente)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<ResumenPagoDTO> resumen = new List<ResumenPagoDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ReporteResumenPagosCliente", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resumen.Add(new ResumenPagoDTO
                            {
                                PrestamoId = Convert.ToInt32(reader["IdPrestamo"]),
                                TotalPagado = Convert.ToDecimal(reader["TotalPagado"]),
                                MontoPrestamo = Convert.ToDecimal(reader["MontoPrestamo"])
                            });
                        }
                    }
                    return Ok(resumen);
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }

        [HttpGet("prestamos-activos")]
        public IActionResult PrestamosActivos()
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<PrestamoActivoDTO> prestamos = new List<PrestamoActivoDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ReportePrestamosActivos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prestamos.Add(new PrestamoActivoDTO
                            {
                                PrestamoId = Convert.ToInt32(reader["IdPrestamo"]),
                                Cliente = reader["NombreCliente"].ToString(),
                                Estado = reader["Estado"].ToString(),
                                Monto = Convert.ToDecimal(reader["Monto"])
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

        [HttpGet("pagos-atrasados")]
        public IActionResult PagosAtrasados()
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<PagoAtrasadoDTO> pagos = new List<PagoAtrasadoDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ReportePagosAtrasados", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pagos.Add(new PagoAtrasadoDTO
                            {
                                IdPago = Convert.ToInt32(reader["IdPago"]),
                                Cliente = reader["NombreCliente"].ToString(),
                                FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"]),
                                MontoPendiente = Convert.ToDecimal(reader["MontoPendiente"])
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
    }
}
