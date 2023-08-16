using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.Integrations.OpenAI.Interfaces;
using learnaid_backend.Core.Services.Interfaces;

namespace learnaid_backend.Core.Services
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly IOpenAIAPI _openAIAPI;

        public ChatGPTService(IOpenAIAPI openAIAPI)
        {
            _openAIAPI = openAIAPI;
        }

        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicioPorPartes(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma)
        {
            var respuesta = await _openAIAPI.AdaptarEjercicioPorPartes(ejercicio, profesion, edad, idioma);
            return respuesta;
        }

            //public EjercicioAdaptadoDTO EliminarPalabrasExtra(EjercicioAdaptadoDTO ejercicio)
            //{  
            //}
        }
    }
