using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.Models.ORM;

namespace learnaid_backend.Core.Models
{
    public class EjercicioNoAdaptado : Entity
    {
        public string Consigna { get; set; }
        public string Texto { get; set; }
        public string Ejercicio { get; set; }
        public List<Adaptaciones> Adaptaciones { get; set; } = new List<Adaptaciones>();

        private EjercicioNoAdaptado() { }
        public EjercicioNoAdaptado(EjercicioPorAdaptarDTO dto)
        {
            Consigna = dto.Consigna;
            Texto = dto.Texto;
            Ejercicio = dto.Ejercicio;
            dto.Adaptaciones.ForEach(a => Adaptaciones.Add(new Models.Adaptaciones(a)));
        }
    }
}
