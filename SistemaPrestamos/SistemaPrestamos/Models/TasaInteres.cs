namespace SistemaPrestamos.Models
{
    public class TasaInteres
    {
        public int IdTasa { get; set; }
        public decimal TasaAnual { get; set; }
        public decimal TasaMoratoria { get; set; }
        public bool Vigente { get; set; }
    }
}
