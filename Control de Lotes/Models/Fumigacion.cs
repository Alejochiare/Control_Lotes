namespace Control_de_Lotes.Models
{
    public class Fumigacion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Producto { get; set; }
        public string Dosis { get; set; }
        public string Aplicador { get; set; }

        // Relación con Lote
        public int LoteId { get; set; }
        public Lote Lote { get; set; }

        public int MotivoId { get; set; }
        public Motivos Motivo { get; set; }
    }
}
