﻿namespace learnaid_backend.Core.Models
{
    using learnaid_backend.Core.DataTransferObjects.Ejercicio;
    using learnaid_backend.Core.DataTransferObjects.Usuario;
    using learnaid_backend.Core.Models.ORM;

    public class Usuario : Entity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public List<EjercitacionAdaptada> Ejercicios { get; set; } = new List<EjercitacionAdaptada>();
        public string Profesion { get; set; }
        public string Foto { get; set; }
        private Usuario() { }

        public Usuario(UsuarioDTO dto)
        {
            Id = dto.Id;
            Nombre = dto.Nombre;
            Apellido = dto.Apellido;
            Email = dto.Email;
            Contraseña = dto.Contraseña;
            dto.Ejercicios.ToList().ForEach(e => Ejercicios.Add(new EjercitacionAdaptada(e)));
            Profesion = dto.Profesion;
            Foto = dto.Foto;
        }
        public Usuario(CrearUsuarioDTO dto, string foto = "foto") : this()
        {
            Nombre = dto.Nombre;
            Apellido = dto.Apellido;
            Email = dto.Email;
            Contraseña = dto.Contraseña;
            Profesion = dto.Profesion;
            Foto = foto;
        }
        public UsuarioDTO ToDTO()
        {
            return new UsuarioDTO(this);
        }

        public void EditarUsuario(EditarUsuarioDTO dto)
        {
            Nombre = dto.Nombre;
            Apellido = dto.Apellido;
            Contraseña = dto.Contraseña;
            Profesion = dto.Descripcion;
            Foto = dto.Foto;
        }

        public void CrearEjercitacion(EjercitacionAdaptada ejercicio)
        {
            Ejercicios.Add(ejercicio);
        }
    }
}
