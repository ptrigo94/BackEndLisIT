using AyudasSociales.Models;

namespace Autorizacion.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int RoleId { get; set; }
        public int CiudadId { get; set; }
        public Ciudad Ciudad { get; set; }

        public Role Role { get; set; }

        public ICollection<AyudaUsuario> AyudaUsuario { get; set; }
    }
}
