using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.Models.ORM;

namespace learnaid_backend.Core.Models
{
    public class EjercitacionNoAdaptada : Entity
    {
        public string Titulo { get; set; }
        public int Edad { get; set; }
        public string Idioma { get; set; }
        public List<EjercicioNoAdaptado> Ejercicios { get; set; } = new List<EjercicioNoAdaptado>();

        private EjercitacionNoAdaptada () { }
        public EjercitacionNoAdaptada(EjercitacionNoAdaptadaDTO dto)
        {
            Titulo = dto.Titulo;
            Edad = dto.Edad;
            Idioma = dto.Idioma;  
            dto.Ejercicios.ForEach(e => Ejercicios.Add(new EjercicioNoAdaptado(e)));
        }
    }
}
