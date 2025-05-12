namespace Control_de_Lotes.Models
{
    public class Motivos
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Fumigacion> fumigacione { get; set; }


    }
}
