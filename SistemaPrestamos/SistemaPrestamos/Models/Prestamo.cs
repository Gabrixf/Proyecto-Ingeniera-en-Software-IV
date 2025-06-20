namespace SistemaPrestamos.Models
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }
        public int IdCliente { get; set; }
        public int IdTasa { get; set; }
        public decimal MontoPrestamo { get; set; }
        public int PlazoMeses { get; set; }
        public DateTime FechaInicio { get; set; }
        public int DiaPago { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string Estado { get; set; }
    }
}
