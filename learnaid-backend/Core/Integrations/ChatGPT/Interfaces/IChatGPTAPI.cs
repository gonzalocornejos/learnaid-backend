using learnaid_backend.Core.DataTransferObjects.Ejercicio;

namespace learnaid_backend.Core.Integrations.ChatGPT.Interfaces
{
    public interface IChatGPTAPI
    {
        public Task<EjercicioAdaptadoDTO> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio);
    }
}
