using learnaid_backend.Core.DataTransferObjects.Ejercicio;

namespace learnaid_backend.Core.Services.Interfaces
{
    public interface IChatGPTService
    {
        public Task<EjercicioAdaptadoDTO> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio);
    }
}
