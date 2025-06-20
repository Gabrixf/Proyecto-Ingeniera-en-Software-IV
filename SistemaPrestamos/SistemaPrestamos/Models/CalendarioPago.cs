namespace SistemaPrestamos.Models
{
    public class CalendarioPago
    {
        public DateTime FechaPago { get; set; }
        public decimal MontoCuota { get; set; }
        public decimal Interes { get; set; }
        public decimal Amortizacion { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; }
    }
}
