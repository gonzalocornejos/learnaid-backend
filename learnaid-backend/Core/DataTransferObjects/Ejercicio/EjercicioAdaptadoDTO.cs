namespace learnaid_backend.Core.DataTransferObjects.Ejercicio
{
    using learnaid_backend.Core.Models;

    public class EjercicioAdaptadoDTO
    {
        public int Id { get; set; }
        public string Consigna { get; set; }
        public string Ejercicio { get; set; } 
        public EjercicioAdaptadoDTO() { }
        public EjercicioAdaptadoDTO(EjercicioAdaptado ejercicioEntity)
        {
            Id = ejercicioEntity.Id;
            Consigna = ejercicioEntity.Consigna;
            Ejercicio = ejercicioEntity.Ejercicio;
        }
        public EjercicioAdaptadoDTO(EjercicioPorAdaptarDTO ejercicioEntity)
        {
            Consigna = ejercicioEntity.Consigna;
            Ejercicio = ejercicioEntity.Ejercicio;
        }
        public EjercicioAdaptadoDTO(string titulo, string consigna, string ejercicio)
        {
            Consigna = consigna;
            Ejercicio = ejercicio;
        }
       
        public EjercicioAdaptado toEntity()
        {
            return new EjercicioAdaptado(this);
        }
    }
}
