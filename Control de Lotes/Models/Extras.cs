namespace Control_de_Lotes.Models
{
    public class Extras
    {
        public int Id { get; set; }
        public string Nota { get; set; }
        public DateTime Fecha { get; set; }
        public int LoteId { get; set; }
        public Lote Lote { get; set; }
    }
}
