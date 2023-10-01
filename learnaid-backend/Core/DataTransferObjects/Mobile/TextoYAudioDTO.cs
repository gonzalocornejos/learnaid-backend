using Microsoft.AspNetCore.Mvc;

namespace learnaid_backend.Core.DataTransferObjects.Mobile
{
    public class TextoYAudioDTO
    {
        public string Texto {  get; set; }
        public FileStreamResult Stream { get; set; }
    }
}
