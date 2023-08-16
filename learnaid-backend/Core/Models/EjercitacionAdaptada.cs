using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.Models.ORM;

namespace learnaid_backend.Core.Models
{
    public class EjercitacionAdaptada : Entity
    {
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public List<EjercicioAdaptado> Ejercicios { get; set; } = new List<EjercicioAdaptado>();
        public EjercitacionNoAdaptada EjercicioOriginal { get; set; }

        private EjercitacionAdaptada() { }

        public EjercitacionAdaptada(EjercitacionAdaptadaDTO dto)
        {
            Titulo = dto.Titulo;
            Fecha = dto.Fecha;
            dto.Ejercicios.ToList().ForEach(e => Ejercicios.Add(new EjercicioAdaptado(e)));
            EjercicioOriginal = new EjercitacionNoAdaptada(dto.Original);
        }

        public EjercitacionAdaptada(EjercitacionNoAdaptadaDTO original)
        {
            Titulo = original.Titulo;
            EjercicioOriginal = new EjercitacionNoAdaptada(original);
        }

        public void AgregarEjercicio(EjercicioAdaptadoDTO dto)
        {
            var ejercicio = new EjercicioAdaptado(dto);
            Ejercicios.Add(ejercicio);
        }
    }
}
