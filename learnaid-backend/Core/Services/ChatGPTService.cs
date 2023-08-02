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
        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio,string profesion)
        {
            var respuestaAPI = await _openAIAPI.AdaptarEjercicio(ejercicio, profesion);
            var respuesta = ProcesarRespuesta(respuestaAPI, ejercicio.Titulo);
            return respuesta;
        }

        public EjercicioAdaptadoDTO ProcesarRespuesta(string respuesta, string titulo)
        {
            string[] words = respuesta.Split(' ');
            var consigna = "";
            var ejercicio = "";
            var esConsigna = false;
            var esEjercicio= false;
            foreach(string word in words)
            {
                if (word.ToLower().Contains("consigna"))
                {
                    esConsigna = true;
                }
                else if (word.ToLower().Contains("ejercicio") || word.ToLower().Contains("texto"))
                {
                    esEjercicio = true;
                }

                if (esConsigna && esEjercicio)
                {
                    ejercicio += word + " ";
                }
                else if (esConsigna && !esEjercicio)
                {
                    consigna += word + " ";
                }
               
            }
            var ejercicioAdaptado = new EjercicioAdaptadoDTO();
            if (consigna == "")
            {
                ejercicioAdaptado = new EjercicioAdaptadoDTO(titulo, consigna, respuesta);
            } else
            {
                ejercicioAdaptado = new EjercicioAdaptadoDTO(titulo, consigna, ejercicio);
            }
            return ejercicioAdaptado;
        }

        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicioPorPartes(EjercicioPorAdaptarDTO ejercicio, string profesion)
        {
            var respuesta = await _openAIAPI.AdaptarEjercicioPorPartes(ejercicio, profesion);
            return respuesta;
        }

            //public EjercicioAdaptadoDTO EliminarPalabrasExtra(EjercicioAdaptadoDTO ejercicio)
            //{  
            //}
        }
    }
