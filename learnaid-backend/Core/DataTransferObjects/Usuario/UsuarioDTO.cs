﻿namespace learnaid_backend.Core.DataTransferObjects.Usuario
{
    using learnaid_backend.Core.DataTransferObjects.Ejercicio;
    using learnaid_backend.Core.Models;
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public List<EjercitacionAdaptadaDTO> Ejercicios { get; set; } = new List<EjercitacionAdaptadaDTO>();
        public string Profesion { get; set; }
        public string Foto { get; set; }

        public UsuarioDTO() { }

        public UsuarioDTO(Usuario usuarioEntity)
        {
            Id = usuarioEntity.Id;
            Nombre = usuarioEntity.Nombre;
            Apellido = usuarioEntity.Apellido;
            Email = usuarioEntity.Email;
            Contraseña = usuarioEntity.Contraseña;
            if(usuarioEntity.Ejercicios != null)
            {
                usuarioEntity.Ejercicios.ToList().ForEach(e => Ejercicios.Add(new EjercitacionAdaptadaDTO(e)));
            }
            Profesion = usuarioEntity.Profesion;
            Foto = usuarioEntity.Foto;
        }

        public Usuario toEntity()
        {
            return new Usuario(this);
        }
    }
}
