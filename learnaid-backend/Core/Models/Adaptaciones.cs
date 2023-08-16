using learnaid_backend.Core.Models.ORM;

namespace learnaid_backend.Core.Models
{
    public class Adaptaciones : Entity
    {
        public string Adaptacion { get; set; }

        public Adaptaciones (string dto)
        {
            Adaptacion = dto;
        }

        private Adaptaciones() { }
    }
}
