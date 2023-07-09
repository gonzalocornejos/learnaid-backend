using learnaid_backend.Core.Models;

namespace learnaid_backend.Core.Repository.Interfaces
{
    public interface IAdaptadoRepository
    {
        /// <summary>
        ///     Busca todos los ejercicios
        /// </summary>
        /// <returns>ejercicios encontrados</returns>
        Task<List<EjercicioAdaptado>> GetAdaptados();

        /// <summary>
        ///     Busca ejercicios por el usuario
        /// </summary>
        /// <param name="userid">id del usuario a buscar</param>
        /// <returns>ejercicios encontrados</returns>
        Task<List<EjercicioAdaptado>> GetAdaptadosByUser(int userid);

        /// <summary>
        ///     Busca el ejercicio por el id
        /// </summary>
        /// <param name="id">id del ejercicio a buscar</param>
        /// <returns>ejercicio encontrado</returns>
        Task<EjercicioAdaptado> GetAdaptadoById(int id);
    }
}
