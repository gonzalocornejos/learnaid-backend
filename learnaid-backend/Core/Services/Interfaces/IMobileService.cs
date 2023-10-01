using learnaid_backend.Core.DataTransferObjects.Mobile;
using Microsoft.AspNetCore.Mvc;

namespace learnaid_backend.Core.Services.Interfaces
{
    public interface IMobileService
    {
        Task<TextoYAudioDTO> GenerarTextoYAudio(IFormFile foto);

        Task<string> GenerarTexto(IFormFile foto);

        Task<byte[]> GenerarAudio(string texto);
    }
}
