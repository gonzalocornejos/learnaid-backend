using learnaid_backend.Core.DataTransferObjects.Mobile;
using learnaid_backend.Core.Integrations.AmazonRekognition.Interfaces;
using learnaid_backend.Core.Integrations.GoogleCloud.Interfaces;
using learnaid_backend.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace learnaid_backend.Core.Services
{
    public class MobileService : IMobileService
    {
        private readonly IGoogleCloudAPI _googleCloudAPI;

        public MobileService(IGoogleCloudAPI googleCloudAPI)
        {
            _googleCloudAPI = googleCloudAPI;
        }

        public async Task<byte[]> GenerarAudio(string texto)
        {
            var ms = await _googleCloudAPI.GenerarAudio(texto);
            return ms;
        }

        public async Task<string> GenerarTexto(IFormFile foto)
        {
            var respuesta = await _googleCloudAPI.GenerarTexto(foto);
            return respuesta;
        }

        public async Task<TextoYAudioDTO> GenerarTextoYAudio(IFormFile foto)
        {
            var respuesta = new TextoYAudioDTO();
            respuesta.Texto = await _googleCloudAPI.GenerarTexto(foto);
            var ms = await _googleCloudAPI.GenerarAudio(respuesta.Texto);
            return respuesta;
        }
    }
}
