namespace learnaid_backend.Core.DataTransferObjects.Usuario
{
    public class EditarUsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public string Descripcion { get; set; }
        public string Foto { get; set; }
    }
}
