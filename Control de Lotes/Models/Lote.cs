namespace Control_de_Lotes.Models
{
    public class Lote
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Tamaño { get; set; }
        public string Ubicacion { get; set; }

        public ICollection<Siembra> siembra { get; set; }
        public ICollection<Fumigacion> fumigacione { get; set; }
        public ICollection<Cosecha> cosecha { get; set; }
        public ICollection<Extras> extras { get; set; }

    }
}
