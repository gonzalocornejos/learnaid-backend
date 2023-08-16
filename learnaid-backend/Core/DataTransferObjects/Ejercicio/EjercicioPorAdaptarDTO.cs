using learnaid_backend.Core.Models;

namespace learnaid_backend.Core.DataTransferObjects.Ejercicio
{
    public class EjercicioPorAdaptarDTO
    {
        public int Id { get; set; }
        public string Consigna { get; set; }
        public string Texto { get; set; }
        public string Ejercicio { get; set; }
        public List<string> Adaptaciones { get; set; } = new List<string>();

        public EjercicioPorAdaptarDTO() { }
        public EjercicioPorAdaptarDTO(EjercicioNoAdaptado entity)
        {
            Id = entity.Id;
            Consigna = entity.Consigna;
            Texto = entity.Texto;
            Ejercicio = entity.Ejercicio;
            entity.Adaptaciones.ForEach(a => Adaptaciones.Add(a.Adaptacion));
        }

        public EjercicioPorAdaptarDTO(int id,string consigna, string texto, string ejercicio, List<string> adaptaciones)
        {
            Id = id;
            Consigna = consigna;
            Texto = texto;
            Ejercicio = ejercicio;
            Adaptaciones = adaptaciones;
        }
    }
}
