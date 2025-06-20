namespace SistemaPrestamos.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public bool? Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
    public class UsuarioCrearDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }

    public class UsuarioModificarDTO
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public bool? Estado { get; set; } // Por si deseas permitirlo en modificaciones
    }

    public class UsuarioRespuestaDTO
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public bool? Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

}
