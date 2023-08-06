namespace learnaid_backend.Core.DataTransferObjects.Usuario
{
    public class CrearUsuarioDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public string Profesion { get; set; }
        public IFormFile Foto { get; set; }

    }
}
