namespace SistemaPrestamos.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public decimal IngresoMensual { get; set; }
    }
    public class ClienteCrearDTO
    {
        public int IdUsuario { get; set; } // ← viene del resultado de sp_AgregarUsuario
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public decimal IngresoMensual { get; set; }
    }
    public class RegistroClienteDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public decimal IngresoMensual { get; set; }
    }
}
