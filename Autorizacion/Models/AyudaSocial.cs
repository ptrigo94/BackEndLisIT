using Autorizacion.Models;

namespace AyudasSociales.Models
{
    public class AyudaSocial
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime Anio { get; set; }
        public bool Vigente { get; set; }

        public ICollection<AyudaUsuario> AyudaUsuario { get; set; }

        public ICollection<AyudaComuna> AyudaComunas { get; set; }

        public ICollection<AyudaRegion> AyudaRegiones { get; set; }
    }
}
