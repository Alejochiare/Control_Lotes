namespace Control_de_Lotes.Models
{
    public class Cosecha
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public double ProduccionReal { get; set; }

        public int LoteId { get; set; }
        public Lote Lote { get; set; }

        public int CultivoId { get; set; }
        public Cultivos cultivo { get; set; }
    }
}
