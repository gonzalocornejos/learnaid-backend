using learnaid_backend.Core.Models;

namespace learnaid_backend.Core.DataTransferObjects.Ejercicio
{
    public class EjercitacionNoAdaptadaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Edad { get; set; }
        public string Idioma { get; set; }
        public List<EjercicioPorAdaptarDTO> Ejercicios { get; set; } = new List<EjercicioPorAdaptarDTO>();

        public EjercitacionNoAdaptadaDTO() { }

        public EjercitacionNoAdaptadaDTO(EjercitacionNoAdaptada entity)
        {
            Id = entity.Id;
            Titulo = entity.Titulo;
            Edad = entity.Edad;
            Idioma = entity.Idioma;
            entity.Ejercicios.ForEach(e => Ejercicios.Add(new EjercicioPorAdaptarDTO(e)));
        }

        public EjercitacionNoAdaptadaDTO(int id, string titulo, int edad, string idioma, List<EjercicioPorAdaptarDTO> ejercicios)
        {
            Id = id;
            Titulo = titulo;
            Edad = edad;
            Idioma = idioma;
            Ejercicios = ejercicios;
        }
    }
}
