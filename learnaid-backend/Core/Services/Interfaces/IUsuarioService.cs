using learnaid_backend.Core.DataTransferObjects.Usuario;

namespace learnaid_backend.Core.Services.Interfaces
{
    public interface IUsuarioService
    {
        /// <summary>
        ///     Crean un usuario nuevo en la base de datos
        /// </summary>
        /// <param name="credenciales">usuario a crear</param>
        Task CrearUsuario(CrearUsuarioDTO credenciales);

        /// <summary>
        ///     Edita un usuario en la base de datos
        /// </summary>
        /// <param name="usuarioEditDTO">usuario a editar</param>
        Task EditarUsuario(EditarUsuarioDTO usuarioEditDTO);

        /// <summary>
        ///     Elimina un usuario
        /// </summary>
        /// <param name="id">id del usuario a eliminar</param>
        Task EliminarUsuario(int id);

        /// <summary>
        ///     Crean un usuario nuevo en la base de datos
        /// </summary>
        /// <param name="credenciales">usuario a crear</param>
        Task<UsuarioDTO> Loguearse(LoguearseDTO credenciales);

        /// <summary>
        ///     Busca un usuario por su id
        /// </summary>
        /// <param name="id">id del usuario a buscar</param>
        /// <returns>usuario encontrado</returns>
        Task<UsuarioDTO> GetUsuarioById(int id);

        /// <summary>
        ///     Busca un usuario por su email
        /// </summary>
        /// <param name="email">email del usuario a buscar</param>
        /// <returns>usuario encontrado</returns>
        Task<UsuarioDTO> GetUsuarioByEmail(string email);

        /// <summary>
        ///     Busca todos los usuarios de la base de datos
        /// </summary>
        /// <returns>usuarios encontrados</returns>
        Task<List<UsuarioDTO>> GetUsuarios();

    }
}
