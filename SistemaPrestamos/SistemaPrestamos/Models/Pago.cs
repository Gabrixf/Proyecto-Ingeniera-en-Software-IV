namespace SistemaPrestamos.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdPagoProgramado { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoPagado { get; set; }
        public string MedioPago { get; set; }
        public string Observaciones { get; set; }
    }
}
