using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SistemaPrestamos.Models;

namespace SistemaPrestamos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ClientesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("agregar")]
        public IActionResult AgregarCliente([FromBody] ClienteCrearDTO cliente)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AgregarCliente", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUsuario", cliente.IdUsuario);
                cmd.Parameters.AddWithValue("@Cedula", cliente.Cedula);
                cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                cmd.Parameters.AddWithValue("@IngresoMensual", cliente.IngresoMensual);

                try
                {
                    conn.Open();
                    var result = cmd.ExecuteReader();
                    if (result.Read())
                    {
                        return Ok(new { mensaje = result["Mensaje"].ToString() });
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
        public IActionResult ConsultarClientes([FromQuery] string? nombre, [FromQuery] string? cedula)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            List<Cliente> clientes = new List<Cliente>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ConsultarClientes", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", (object?)nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cedula", (object?)cedula ?? DBNull.Value);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new Cliente
                            {
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Cedula = reader["Cedula"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                IngresoMensual = Convert.ToDecimal(reader["IngresoMensual"])
                            });
                        }
                    }

                    return Ok(clientes);
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpPut("modificar")]
        public IActionResult ModificarCliente([FromBody] Cliente cliente)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ModificarCliente", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                cmd.Parameters.AddWithValue("@Cedula", cliente.Cedula);
                cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                cmd.Parameters.AddWithValue("@Correo", cliente.Correo);
                cmd.Parameters.AddWithValue("@IngresoMensual", cliente.IngresoMensual);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Cliente modificado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpDelete("eliminar")]
        public IActionResult EliminarCliente([FromQuery] int idCliente)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_EliminarCliente", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Cliente eliminado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpPost("registrar-completo")]
        public IActionResult RegistrarClienteCompleto([FromBody] RegistroClienteDTO data)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insertar en Usuarios
                    SqlCommand cmdUsuario = new SqlCommand("sp_AgregarUsuario", conn, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdUsuario.Parameters.AddWithValue("@Nombre", data.Nombre);
                    cmdUsuario.Parameters.AddWithValue("@Apellido", data.Apellido);
                    cmdUsuario.Parameters.AddWithValue("@Correo", data.Correo);
                    cmdUsuario.Parameters.AddWithValue("@Contrasena", data.Contrasena);
                    cmdUsuario.Parameters.AddWithValue("@Rol", "Cliente");

                    int idUsuario = Convert.ToInt32(cmdUsuario.ExecuteScalar());

                    // Insertar en Clientes
                    SqlCommand cmdCliente = new SqlCommand("sp_AgregarCliente", conn, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdCliente.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmdCliente.Parameters.AddWithValue("@Cedula", data.Cedula);
                    cmdCliente.Parameters.AddWithValue("@Telefono", data.Telefono);
                    cmdCliente.Parameters.AddWithValue("@IngresoMensual", data.IngresoMensual);

                    cmdCliente.ExecuteNonQuery();

                    transaction.Commit();

                    return Ok(new { mensaje = "Usuario y cliente registrados correctamente.", idUsuario });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, new { error = "Error al registrar: " + ex.Message });
                }
            }
        }
    }

}
