using learnaid_backend.Core.Models;

namespace learnaid_backend.Core.DataTransferObjects.Ejercicio
{
    public class EjercitacionAdaptadaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public List<EjercicioAdaptadoDTO> Ejercicios { get; set; } = new List<EjercicioAdaptadoDTO>();
        public EjercitacionNoAdaptadaDTO Original { get; set; }

        public EjercitacionAdaptadaDTO() { }

        public EjercitacionAdaptadaDTO(EjercitacionAdaptada entity)
        {
            Id = entity.Id;
            Titulo = entity.Titulo;
            Fecha = entity.Fecha;
            entity.Ejercicios.ForEach(e => Ejercicios.Add(new EjercicioAdaptadoDTO(e)));
            Original = new EjercitacionNoAdaptadaDTO(entity.EjercicioOriginal);
        }

        public EjercitacionAdaptadaDTO (EjercitacionNoAdaptadaDTO dto)
        {
            Titulo = dto.Titulo;
            Original = dto;
        }
    }
}
