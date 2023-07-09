using learnaid_backend.Core.DataTransferObjects.Usuario;
using learnaid_backend.Core.Models;

namespace learnaid_backend.Core.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        /// <summary>
        ///     Busca el usuario por el id
        /// </summary>
        /// <param name="id">id del usuario a buscar</param>
        /// <returns>usuario encontrado</returns>
        Task<Usuario> GetUsuarioById(int id);
        /// <summary>
        ///     Busca el usuario por el email
        /// </summary>
        /// <param name="email">email del usuario a buscar</param>
        /// <returns>usuario encontrado</returns>
        Task<Usuario> GetUsuarioByEmail(string email);
        /// <summary>
        ///     Busca todos los usuarios
        /// </summary>
        /// <returns>listado de usuarios</returns>
        Task<List<Usuario>> GetUsuarios();
        /// <summary>
        ///     Valida las credenciales pasadas para el inicio de sesion
        /// </summary>
        /// <param name="credenciales">credenciales a validar</param>
        /// <returns>Verdadero o falso dependiendo si las credenciales fueron validadas</returns>
        Task<bool> VerificarCredencialesLogueo(LoguearseDTO credenciales);
    }
}
