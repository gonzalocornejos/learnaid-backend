using learnaid_backend.Core.Models;

namespace learnaid_backend.Core.Repository.Interfaces
{
    public interface IEjercitacionAdaptadaRepository
    {
        /// <summary>
        ///     Busca todos los ejercicios
        /// </summary>
        /// <returns>ejercicios encontrados</returns>
        Task<List<EjercitacionAdaptada>> GetAdaptados();

        /// <summary>
        ///     Busca ejercicios por el usuario
        /// </summary>
        /// <param name="userid">id del usuario a buscar</param>
        /// <returns>ejercicios encontrados</returns>
        Task<List<EjercitacionAdaptada>> GetAdaptadosByUser(int userid);

        /// <summary>
        ///     Busca el ejercicio por el id
        /// </summary>
        /// <param name="id">id del ejercicio a buscar</param>
        /// <returns>ejercicio encontrado</returns>
        Task<EjercitacionAdaptada> GetAdaptadoById(int id);
    }
}
