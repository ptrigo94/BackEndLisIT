using Autorizacion.Models;

namespace AyudasSociales.Models
{
    public class AyudaComuna
    {
        public int Id { get; set; }
        public int CiudadId { get; set; }
        public int AyudaSocialId { get; set; }

        public Ciudad Ciudad { get; set; }
        public AyudaSocial Ayuda { get; set; }

    }
}
