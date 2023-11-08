using Autorizacion.Models;

namespace AyudasSociales.Models
{
    public class AyudaUsuario
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AyudaSocialId { get; set; }

        public User User { get; set; }
        public AyudaSocial Ayuda { get; set; }

        

    }
}
