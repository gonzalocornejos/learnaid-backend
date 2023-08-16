namespace learnaid_backend.Core.Models
{
    using learnaid_backend.Core.DataTransferObjects.Ejercicio;
    using learnaid_backend.Core.Models.ORM;

    public class EjercicioAdaptado : Entity
    {
        public string Consigna { get; set; }
        public string Ejercicio { get; set; }

        public EjercicioAdaptado() { }
        public EjercicioAdaptado(EjercicioAdaptadoDTO dto)
        {
            Id = dto.Id;
            Consigna = dto.Consigna;
            Ejercicio = dto.Ejercicio;
        }

        public EjercicioAdaptado(EjercicioAdaptadoDTO dto, bool esEdit)
        {
            if (esEdit)
            {
                Consigna = dto.Consigna;
                Ejercicio = dto.Ejercicio;
            }
        }
        public EjercicioAdaptado(EjercicioPorAdaptarDTO dto) : this()
        {
            Consigna = dto.Consigna;
            Ejercicio = dto.Ejercicio;
        }
        public EjercicioAdaptadoDTO toDTO()
        {
            return new EjercicioAdaptadoDTO(this);
        }
    }
}
