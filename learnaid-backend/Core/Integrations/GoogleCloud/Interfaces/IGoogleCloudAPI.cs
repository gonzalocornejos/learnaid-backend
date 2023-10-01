namespace learnaid_backend.Core.Integrations.GoogleCloud.Interfaces
{
    public interface IGoogleCloudAPI
    {
        Task<string> GenerarTexto(IFormFile image);

        Task<byte[]> GenerarAudio(string texto);
    }
}
