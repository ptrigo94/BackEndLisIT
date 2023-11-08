namespace AyudasSociales.Models
{
    public class AyudaRegion
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public int AyudaSocialId { get; set; }

        public Region Region{ get; set; }
        public AyudaSocial Ayuda { get; set; }

    }
}
