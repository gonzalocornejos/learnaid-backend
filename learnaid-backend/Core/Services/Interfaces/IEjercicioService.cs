﻿using learnaid_backend.Core.DataTransferObjects.Ejercicio;

namespace learnaid_backend.Core.Services.Interfaces
{
    public interface IEjercicioService
    {
        /// <summary>
        ///     Adapta un ejerciio
        /// </summary>
        /// <param name="ejercicio">ejercicio a adpatar</param>
        /// <param name="userid">id del usuario que va a adaptar ejercicio</param>
        Task<EjercicioAdaptadoDTO> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio, int userid);

        /// <summary>
        ///     Busca todos los ejercicios
        /// </summary>
        /// <returns>ejercicios encontrados</returns>
        Task<List<EjercicioAdaptadoDTO>> GetEjercicios();

        /// <summary>
        ///     Busca ejercicios por el usuario
        /// </summary>
        /// <param name="userid">id del usuario a buscar</param>
        /// <returns>ejercicios encontrados</returns>
        Task<List<EjercicioAdaptadoDTO>> GetEjerciciosByUser(int userid);

        /// <summary>
        ///     Busca el ejercicio por el id
        /// </summary>
        /// <param name="id">id del ejercicio a buscar</param>
        /// <returns>ejercicio encontrado</returns>
        Task<EjercicioAdaptadoDTO> GetEjercicioById(int id);

        /// <summary>
        ///     Elimina un ejercicio
        /// </summary>
        /// <param name="id">id del ejercicio a eliminar</param>
        /// <param name="userid">id del usuario</param>
        Task EliminarEjercicio(int id, int userid);
    }
}