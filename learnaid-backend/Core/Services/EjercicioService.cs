using iText.Layout;
using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.Exceptions;
using learnaid_backend.Core.Integrations.iText.Interfaces;
using learnaid_backend.Core.Models;
using learnaid_backend.Core.Repository.Interfaces;
using learnaid_backend.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace learnaid_backend.Core.Services
{
    public class EjercicioService : IEjercicioService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IAdaptadoRepository _adaptadoRepository;
        private readonly IEjercitacionAdaptadaRepository _ejercitacionAdaptadaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IChatGPTService _chatgptService;
        private readonly IiText _iTextIntegration;

        public EjercicioService(IGenericRepository genericRepository, IAdaptadoRepository adaptadoRepository, IUsuarioRepository usuarioRepository, IChatGPTService chatGPTService, IiText itextIntegration, IEjercitacionAdaptadaRepository ejercitacionAdaptadaRepository)
        {
            _genericRepository = genericRepository;
            _adaptadoRepository = adaptadoRepository;
            _usuarioRepository = usuarioRepository;
            _chatgptService = chatGPTService;
            _iTextIntegration = itextIntegration;
            _ejercitacionAdaptadaRepository = ejercitacionAdaptadaRepository;
        }

        public async Task<EjercitacionAdaptadaDTO> AdaptarEjercitacion (EjercitacionNoAdaptadaDTO ejercitacion, int userid)
        {
            Console.WriteLine("Entre");
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            var adaptada = new EjercitacionAdaptada(ejercitacion);
            adaptada.Fecha = DateTime.Today;
            foreach(var ejercicio in ejercitacion.Ejercicios)
            {
                var adaptado = await _chatgptService.AdaptarEjercicioPorPartes(ejercicio, usuario.Profesion, ejercitacion.Edad, ejercitacion.Idioma);
                adaptada.AgregarEjercicio(adaptado);
            }
            usuario.CrearEjercitacion(adaptada);
            await _genericRepository.GuardarCambiosAsync();
            var dto = new EjercitacionAdaptadaDTO(adaptada);
            return dto;
        }
        /*
        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicio(EjercicioPorAdaptarDTO ejercicio, int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            //var adaptado = await _chatgptService.AdaptarEjercicio(ejercicio, usuario.Profesion);
            //adaptado.Fecha = DateTime.Today;
            //usuario.CrearEjercicio(adaptado);
            await _genericRepository.GuardarCambiosAsync();
            return adaptado;
        }*/

        public async Task<EjercicioAdaptadoDTO> AdaptarEjercicioPorPartes(EjercicioPorAdaptarDTO ejercicio, int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            //var adaptado = await _chatgptService.AdaptarEjercicioPorPartes(ejercicio, usuario.Profesion);
            //adaptado.Fecha = DateTime.Today;
            //usuario.CrearEjercicio(adaptado);
            await _genericRepository.GuardarCambiosAsync();
            var ejercicios = await _adaptadoRepository.GetAdaptadosByUser(userid);
            var respuesta = new EjercicioAdaptadoDTO(ejercicios.Last<EjercicioAdaptado>());
            return respuesta;
        }

        public async Task EliminarEjercicio(int id, int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }

            //var ejercicio = await _adaptadoRepository.GetAdaptadoById(id);
            var ejercicio = await _ejercitacionAdaptadaRepository.GetAdaptadoById(userid);
            if (ejercicio == null)
            {
                throw new AppException("Ejercicio invalido", HttpStatusCode.NotFound);
            }

            if (usuario.Ejercicios.Contains(ejercicio))
            {
                throw new AppException("No tiene permiso a eliminar ejercicio", HttpStatusCode.Forbidden);
            }
            await _genericRepository.Eliminar(ejercicio);
            await _genericRepository.GuardarCambiosAsync();
        }

        public async Task<EjercitacionAdaptadaDTO> GetEjercicioById(int id)
        {

            //var ejercicio = await _adaptadoRepository.GetAdaptadoById(id);
            var ejercicio = await _ejercitacionAdaptadaRepository.GetAdaptadoById(id);
            if (ejercicio == null)
            {
                throw new AppException("Ejercicio invalido", HttpStatusCode.NotFound);
            }
            var respuesta = new EjercitacionAdaptadaDTO(ejercicio);
            return respuesta;
    }

        public async Task<List<EjercitacionAdaptadaDTO>> GetEjercicios()
        {
            //var ejercicios = await _adaptadoRepository.GetAdaptados();
            var ejercicios = await _ejercitacionAdaptadaRepository.GetAdaptados();
            var respuesta = new List<EjercitacionAdaptadaDTO>();
            ejercicios.ForEach(ejercicio => respuesta.Add(new EjercitacionAdaptadaDTO(ejercicio)));
            return respuesta;
        }

        public async Task<List<EjercitacionAdaptadaDTO>> GetEjerciciosByUser(int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            var ejercicios = new List<EjercitacionAdaptada>();
            foreach(var ejercicio in usuario.Ejercicios)
            {
                var ej = await _ejercitacionAdaptadaRepository.GetAdaptadoById(ejercicio.Id);
                ejercicios.Add(ej);
            }
            var respuesta = new List<EjercitacionAdaptadaDTO>();
            ejercicios.ForEach(ejercicio => respuesta.Add(new EjercitacionAdaptadaDTO(ejercicio)));
            return respuesta;
        }

        public async Task<EjercitacionAdaptadaDTO> EditarEjercicio(EjercitacionAdaptadaDTO ejercicio, int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            var ejercitacion = await _ejercitacionAdaptadaRepository.GetAdaptadoById(ejercicio.Id);
            if (ejercitacion == null)
            {
                throw new AppException("Ejercitacion invalida", HttpStatusCode.NotFound);
            }
            ejercitacion.Titulo = ejercicio.Titulo;
            ejercitacion.Ejercicios = new List<EjercicioAdaptado>();
            foreach (var ej in ejercicio.Ejercicios)
            {
                ejercitacion.Ejercicios.Add(new EjercicioAdaptado(ej,true));
            }
            await _genericRepository.Actualizar(ejercitacion);
            await _genericRepository.GuardarCambiosAsync();
            var respuesta = new EjercitacionAdaptadaDTO(ejercitacion);
            return respuesta;
        }

        public async Task<Stream> GetPdfEjercicio(int ejercicioid, int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            //var ej = await _adaptadoRepository.GetAdaptadoById(ejercicioid);
            var ej = await _ejercitacionAdaptadaRepository.GetAdaptadoById(ejercicioid);
            if(ej == null)
            {
                throw new AppException("Ejercicio invalido", HttpStatusCode.NotFound);
            }
            var res = _iTextIntegration.GenerarPdF(ej);
            return res;
        }

        public string GetNombrePdf(string titulo)
        {
            var respuesta = titulo.Replace(" ", "");
            respuesta = respuesta.Replace("/", "-");
            respuesta = respuesta + ".pdf";
            return respuesta;
        }
        public async Task<Stream> GetPdfEjercicioOriginal(int ejercicioid, int userid)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(userid);
            if (usuario == null)
            {
                throw new AppException("Usuario invalido", HttpStatusCode.NotFound);
            }
            //var ej = await _adaptadoRepository.GetAdaptadoById(ejercicioid);
            var ej = await _ejercitacionAdaptadaRepository.GetAdaptadoById(ejercicioid);
            if (ej == null)
            {
                throw new AppException("Ejercicio invalido", HttpStatusCode.NotFound);
            }
            var res = _iTextIntegration.GenerarPdFOriginal(ej.EjercicioOriginal);
            return res;
        }

        public string GetNombrePdfOriginal(string titulo)
        {
            var respuesta = titulo.Replace(" ", "");
            respuesta = respuesta.Replace("/", "-");
            respuesta = respuesta + "Orignal.pdf";
            return respuesta;
        }
    }
}
