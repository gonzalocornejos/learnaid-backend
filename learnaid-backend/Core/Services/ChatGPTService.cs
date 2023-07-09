using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.Services.Interfaces;

namespace learnaid_backend.Core.Services
{
    public class ChatGPTService : IChatGPTService
    {
        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio)
        {
            var respuesta =new EjercicioAdaptadoDTO(ejercicio);
            return respuesta;
        }
    }
}
