namespace Autorizacion.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public ICollection<User> Users { get; set; }

    }

}
