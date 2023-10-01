namespace learnaid_backend.Core.Integrations.AmazonRekognition.Interfaces
{
    public interface IAmazonRekognitionAPI
    {
        Task<string> GenerarTexto(IFormFile image);
    }
}
