using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using learnaid_backend.Configuration;
using learnaid_backend.Core.Integrations.CloudinaryAPI.Interfaces;
using Microsoft.Extensions.Options;

namespace learnaid_backend.Core.Integrations.CloudinaryAPI
{
    public class CloudinaryAPI : ICloudinaryAPI
    {
        private readonly learnaid_backend.Configuration.CloudinaryConfiguration _cloudinaryConfiguration;

        public CloudinaryAPI(IOptionsMonitor<learnaid_backend.Configuration.CloudinaryConfiguration> optionsMonitor)
        {
            _cloudinaryConfiguration = optionsMonitor.CurrentValue;
        }

        public async Task<string> UploadImage(Stream stream, string nombre)
        {
            Account account = new Account(_cloudinaryConfiguration.CLOUD_NAME, _cloudinaryConfiguration.API_KEY, _cloudinaryConfiguration.API_SECRET);
            Cloudinary cloudinary = new Cloudinary(account);

            var imageParams = new ImageUploadParams()
            {
                File = new FileDescription(nombre, stream)
        };

            var result = await cloudinary.UploadAsync(imageParams);
            return result.Url.ToString();
        }
    }
}
