namespace learnaid_backend.Core.Integrations.CloudinaryAPI.Interfaces
{
    public interface ICloudinaryAPI
    {
        public Task<string> UploadImage(Stream stream, string nombre);
    }
}
