namespace AyudasSociales.Models
{
    public class Pais
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string Siglas{ get; set; }
        public ICollection<Region> Regiones { get; set;}

    }
}
