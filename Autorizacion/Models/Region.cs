namespace AyudasSociales.Models
{
    public class Region 
    {
        public int RegionId { get; set; }
        public string Name { get; set; }
        public string Siglas { get; set; }
        public int PaisId { get; set; }
        public Pais Pais { get; set; }
        public ICollection<Ciudad> Ciudades { get; set; }

        public ICollection<AyudaRegion> AyudaRegiones { get; set; }

    }
}
