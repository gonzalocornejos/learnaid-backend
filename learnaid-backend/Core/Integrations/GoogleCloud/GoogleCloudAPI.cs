using Google.Api;
using Google.Cloud.Vision.V1;
using Google.Cloud.TextToSpeech.V1;
using learnaid_backend.Core.Integrations.GoogleCloud.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace learnaid_backend.Core.Integrations.GoogleCloud
{
    public class GoogleCloudAPI : IGoogleCloudAPI
    {
        public async Task<byte[]> GenerarAudio(string texto)
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:/Users/gonza/source/repos/learnaid-backend/learnaid-backend/learnaid-399117-c8a23f7b1b2a.json");
            var client = TextToSpeechClient.Create();
            var input = new SynthesisInput
            {
                Text = texto,
            };

            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = "es-US",
                Name = "es-US-Studio-B"
            };

            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            var response = await client.SynthesizeSpeechAsync(input, voiceSelection, audioConfig);

            byte[] output = response.AudioContent.ToByteArray();

            return output;
        }

        public async Task<string> GenerarTexto(IFormFile image)
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:/Users/gonza/source/repos/learnaid-backend/learnaid-backend/learnaid-399117-c8a23f7b1b2a.json");
            var builder = new ImageAnnotatorClientBuilder();
            var client = builder.Build();
            var imagen = Image.FromStream(image.OpenReadStream());
            var response = await client.DetectTextAsync(imagen);
            var text = "";
            foreach(var annotation in response)
            {
                if(annotation != null)
                {
                    text = text + annotation.Description;
                }
            }
            return text;
;        }


    }
}
