using Autorizacion.Models;

namespace AyudasSociales.Models
{
    public class Ciudad
    {
        public int CiudadId { get; set; }
        public string Nombre { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }

        public ICollection<User> Usuarios{ get; set; }

        public ICollection<AyudaComuna> AyudaComunas{ get; set; }
    }
}
