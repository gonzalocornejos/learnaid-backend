using learnaid_backend.Core.Models.ORM;

namespace learnaid_backend.Core.Repository.Interfaces
{
    public interface IGenericRepository
    {
        /// <summary>
        ///     Guarda los cambios en la base de datos de forma sincronica
        /// </summary>
        Task GuardarCambiosAsync();

        /// <summary>
        ///     Guarda los cambios en la base de datos de forma asincronica
        /// </summary>
        /// <returns></returns>
        Task GuardarCambios();

        /// <summary>
        ///     Agrega la entity a la base de datos. [Es recomendable utilizar el trackeo del ORM]
        /// </summary>
        /// <param name="entity">entity a agregar</param>
        Task Agregar(Entity entity);

        /// <summary>
        ///     Actualiza toda la entity en la base de datos. [Es recomendable utilizar el trackeo del ORM]
        /// </summary>
        /// <param name="entity">entity a agregar</param>
        Task Actualizar(Entity entity);

        /// <summary>
        ///     Elimina la entity en la base de datos. [Es recomendable utilizar el trackeo del ORM]
        /// </summary>
        /// <param name="entity">entity a agregar</param>
        Task Eliminar(Entity entity);
    }
}
