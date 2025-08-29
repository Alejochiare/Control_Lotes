namespace Control_de_Lotes.Models
{
    public class Cultivos
    {
        public int Id { get; set; }
        public string Semilla { get; set; }

        public ICollection<Siembra> siembra { get; set; }
        public ICollection<Cosecha> cosecha { get; set; }
    }
}
