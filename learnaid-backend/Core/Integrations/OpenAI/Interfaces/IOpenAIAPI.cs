using learnaid_backend.Core.DataTransferObjects.Ejercicio;

namespace learnaid_backend.Core.Integrations.OpenAI.Interfaces
{
    public interface IOpenAIAPI
    {
        //Task<string> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio, string profesion);
        Task<EjercicioAdaptadoDTO> AdaptarEjercicioPorPartes(EjercicioPorAdaptarDTO ejercicio, string profesion, int edad, string idioma);
    }
}
