namespace learnaid_backend.Core.DataTransferObjects.Ejercicio
{
    using learnaid_backend.Core.Models;

    public class EjercicioAdaptadoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Consigna { get; set; }
        public string Ejercicio { get; set; }
        public DateTime Fecha { get; set; }
        public EjercicioAdaptadoDTO() { }
        public EjercicioAdaptadoDTO(EjercicioAdaptado ejercicioEntity)
        {
            Id = ejercicioEntity.Id;
            Titulo = ejercicioEntity.Titulo;
            Consigna = ejercicioEntity.Consigna;
            Ejercicio = ejercicioEntity.Ejercicio;
            Fecha = ejercicioEntity.Fecha;
        }
        public EjercicioAdaptadoDTO(EjercicioPorAdaptarDTO ejercicioEntity)
        {
            Titulo = ejercicioEntity.Titulo;
            Consigna = ejercicioEntity.Consigna;
            Ejercicio = ejercicioEntity.Ejercicio;
            Fecha = new DateTime();
        }
        public EjercicioAdaptado toEntity()
        {
            return new EjercicioAdaptado(this);
        }
    }
}
