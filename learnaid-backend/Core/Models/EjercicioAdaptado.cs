namespace learnaid_backend.Core.Models
{
    using learnaid_backend.Core.DataTransferObjects.Ejercicio;
    using learnaid_backend.Core.Models.ORM;

    public class EjercicioAdaptado : Entity
    {
        public string Titulo { get; set; }
        public string Consigna { get; set; }
        public string Ejercicio { get; set; }
        public DateTime Fecha { get; set; }
        public Usuario Usuario { get; set; }
        public EjercicioAdaptado() { }
        public EjercicioAdaptado(EjercicioAdaptadoDTO dto)
        {
            Id = dto.Id;
            Titulo = dto.Titulo;
            Consigna = dto.Consigna;
            Ejercicio = dto.Ejercicio;
            Fecha = dto.Fecha;
        }
        public EjercicioAdaptado(EjercicioPorAdaptarDTO dto) : this()
        {
            Titulo = dto.Titulo;
            Consigna = dto.Consigna;
            Ejercicio = dto.Ejercicio;
            Fecha = DateTime.Now;
        }
        public EjercicioAdaptadoDTO toDTO()
        {
            return new EjercicioAdaptadoDTO(this);
        }
    }
}
