using CloudinaryDotNet;
using learnaid_backend.Configuration;
using Microsoft.Extensions.Options;

namespace learnaid_backend.Core.Integrations.CloudinaryAPI
{
    public class CloudinaryAPI
    {
        private readonly learnaid_backend.Configuration.CloudinaryConfiguration _cloudinaryConfiguration;

        public CloudinaryAPI(IOptionsMonitor<learnaid_backend.Configuration.CloudinaryConfiguration> optionsMonitor)
        {
            _cloudinaryConfiguration = optionsMonitor.CurrentValue;
        }

        public void UploadImage()
        {
            Account account = new Account(_cloudinaryConfiguration.CLOUD_NAME, _cloudinaryConfiguration.API_KEY, _cloudinaryConfiguration.API_SECRET);
            Cloudinary cloudinary = new Cloudinary(account);

            
        }
    }
}
