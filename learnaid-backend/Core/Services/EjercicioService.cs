using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.Exceptions;
using learnaid_backend.Core.Models;
using learnaid_backend.Core.Repository.Interfaces;
using learnaid_backend.Core.Services.Interfaces;
using System.Net;

namespace learnaid_backend.Core.Services
{
    public class EjercicioService : IEjercicioService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IAdaptadoRepository _adaptadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IChatGPTService _chatgptService; 

        public EjercicioService(IGenericRepository genericRepository, IAdaptadoRepository adaptadoRepository, IUsuarioRepository usuarioRepository)
        {
            _genericRepository = genericRepository;
            _adaptadoRepository = adaptadoRepository;
            _usuarioRepository = usuarioRepository;
        }


        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio, int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            var adaptado = await _chatgptService.AdaptarEjercicio(ejercicio);
            usuario.CrearEjercicio(adaptado);
            await _genericRepository.GuardarCambiosAsync();
            return adaptado;
        }

        public async Task EliminarEjercicio(int id, int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }

            var ejercicio = await _adaptadoRepository.GetAdaptadoById(id);
            if (ejercicio == null)
            {
                throw new AppException("Ejercicio invalido", HttpStatusCode.NotFound);
            }

            if (ejercicio.Usuario != usuario)
            {
                throw new AppException("No tiene permiso a eliminar ejercicio", HttpStatusCode.Forbidden);
            }
            await _genericRepository.Eliminar(ejercicio);
            await _genericRepository.GuardarCambiosAsync();
        }

        public async Task<EjercicioAdaptadoDTO> GetEjercicioById(int id)
        {
            var ejercicio = await _adaptadoRepository.GetAdaptadoById(id);
            if (ejercicio == null)
            {
                throw new AppException("Ejercicio invalido", HttpStatusCode.NotFound);
            }
            return ejercicio.toDTO();
    }

        public async Task<List<EjercicioAdaptadoDTO>> GetEjercicios()
        {
            var ejercicios = await _adaptadoRepository.GetAdaptados();
            var respuesta = new List<EjercicioAdaptadoDTO>();
            ejercicios.ForEach(ejercicio => respuesta.Add(ejercicio.toDTO()));
            return respuesta;
        }

        public async Task<List<EjercicioAdaptadoDTO>> GetEjerciciosByUser(int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            var ejercicios = await _adaptadoRepository.GetAdaptadosByUser(userid);
            var respuesta = new List<EjercicioAdaptadoDTO>();
            ejercicios.ForEach(ejercicio => respuesta.Add(ejercicio.toDTO()));
            return respuesta;
        }
    }
}
