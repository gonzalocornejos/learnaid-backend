using learnaid_backend.Core.DataTransferObjects.Usuario;
using learnaid_backend.Core.Exceptions;
using learnaid_backend.Core.Models;
using learnaid_backend.Core.Repository.Interfaces;
using learnaid_backend.Core.Services.Interfaces;
using System.Net;

namespace learnaid_backend.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IGenericRepository genericRepository, IUsuarioRepository usuarioRepository)
        {
            _genericRepository = genericRepository;
            _usuarioRepository = usuarioRepository;
        }


        public async Task CrearUsuario(CrearUsuarioDTO credenciales)
        {
            var usuario = new Usuario(credenciales);
            await _genericRepository.Agregar(usuario);
            await _genericRepository.GuardarCambiosAsync();
        }

        public async Task EditarUsuario(EditarUsuarioDTO usuarioEditDTO)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(usuarioEditDTO.Id);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            usuario.EditarUsuario(usuarioEditDTO);
            await _genericRepository.GuardarCambiosAsync();
        }

        public async Task EliminarUsuario(int id)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(id);
            if(usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            await _genericRepository.Eliminar(usuario);
            await _genericRepository.GuardarCambiosAsync();
        }

        public async Task<UsuarioDTO> GetUsuarioByEmail(string email)
        {
            var usuario = await _usuarioRepository.GetUsuarioByEmail(email);
            if(usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            return usuario.ToDTO(); 
        }

        public async Task<UsuarioDTO> GetUsuarioById(int id)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(id);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            return usuario.ToDTO();
        }

        public async Task<List<UsuarioDTO>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.GetUsuarios();
            var respuesta = new List<UsuarioDTO>();
            usuarios.ForEach(usuario => respuesta.Add(usuario.ToDTO()));
            return respuesta;
        }

        public async Task<UsuarioDTO> Loguearse(LoguearseDTO credenciales)
        {
            if(!await _usuarioRepository.VerificarCredencialesLogueo(credenciales))
            {
                throw new AppException("Credenciales no validas", HttpStatusCode.Unauthorized);
            }
            var resultado = await GetUsuarioByEmail(credenciales.Email);
            return resultado;
        }
    }
}
