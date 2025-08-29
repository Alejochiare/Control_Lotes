namespace Control_de_Lotes.Models
{
    public class Siembra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public double ProduccionEstimativa { get; set; }
        public double CantidadSemillas { get; set; }
        public double Superficiesembrada { get; set; }

        public string Destino { get; set; }
        // Relación con Lote
        public int LoteId { get; set; }
        public Lote Lote { get; set; }

        public int CultivoId { get; set; }
        public Cultivos cultivo { get; set; }

    }
}
