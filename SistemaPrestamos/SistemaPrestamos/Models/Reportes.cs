namespace SistemaPrestamos.Models
{
    public class Reportes
    {
        public class ResumenPagoDTO
        {
            public int PrestamoId { get; set; }
            public decimal TotalPagado { get; set; }
            public decimal MontoPrestamo { get; set; }
        }

        public class PrestamoActivoDTO
        {
            public int PrestamoId { get; set; }
            public string Cliente { get; set; }
            public string Estado { get; set; }
            public decimal Monto { get; set; }
        }

        public class PagoAtrasadoDTO
        {
            public int IdPago { get; set; }
            public string Cliente { get; set; }
            public DateTime FechaVencimiento { get; set; }
            public decimal MontoPendiente { get; set; }
        }
    }
}
