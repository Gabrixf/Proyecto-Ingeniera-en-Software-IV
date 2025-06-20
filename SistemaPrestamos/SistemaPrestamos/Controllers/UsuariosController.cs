using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaPrestamos.Models;


namespace SistemaPrestamos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsuariosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("listar")]
        public IActionResult ListarUsuarios()
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");
            List<UsuarioRespuestaDTO> usuarios = new List<UsuarioRespuestaDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ListarUsuarios", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        usuarios.Add(new UsuarioRespuestaDTO
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Rol = reader["Rol"].ToString(),
                            Estado = reader["Estado"] != DBNull.Value ? (bool?)Convert.ToBoolean(reader["Estado"]) : null,
                            FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"])
                        });
                    }
                    return Ok(usuarios);
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }

        [HttpPost("agregar")]
        public IActionResult AgregarUsuario([FromBody] UsuarioCrearDTO usuario)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AgregarUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                cmd.Parameters.AddWithValue("@Rol", "Usuario");

                try
                {
                    conn.Open();
                    var idUsuario = (int)cmd.ExecuteScalar();
                    return Ok(new { idUsuario });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
        [HttpPut("modificar")]
        public IActionResult ModificarUsuario([FromBody] UsuarioModificarDTO usuario)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ModificarUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                cmd.Parameters.AddWithValue("@Estado", (object)usuario.Estado ?? DBNull.Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Usuario modificado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }

        [HttpDelete("eliminar")]
        public IActionResult EliminarUsuario([FromQuery] int idUsuario)
        {
            string connectionString = _configuration.GetConnectionString("ConexionBD");

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Usuario eliminado correctamente." });
                }
                catch (SqlException ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    }
}
