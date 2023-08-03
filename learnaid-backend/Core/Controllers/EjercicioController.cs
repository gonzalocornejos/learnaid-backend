using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.Exceptions;
using learnaid_backend.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace learnaid_backend.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EjercicioController : ControllerBase
    {
        private readonly IEjercicioService _ejercicioService;

        public EjercicioController(IEjercicioService ejercicioService)
        {
            _ejercicioService = ejercicioService;
        }

        /// <summary>
        ///     Aadapta un Ejercicio
        /// </summary>
        /// <returns>
        ///     Ejercicio adaptado.
        /// </returns>
        /// <param name="ejercicio">datos del ejercicio a adaptar</param>
        /// <param name="userid">id del usuario que va a adaptar el ejercicio</param>
        /// <response code="204">Si se adapto el ejercicio correctamente</response>
        /// <response code="400">Si los parametros se enviaron incorrectamente</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("adaptar-ejercicio/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdaptarEjercicio([FromBody] EjercicioPorAdaptarDTO ejercicio, [FromRoute,Required] int userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            //var respuesta = await _ejercicioService.AdaptarEjercicio(ejercicio,userid);
            var respuesta = await _ejercicioService.AdaptarEjercicioPorPartes(ejercicio, userid);
            return Ok(respuesta);
        }

        /// <summary>
        ///     Elimina un ejercicio.
        /// </summary>
        /// <returns>
        ///     Validacion correcta o incorrecta del proceso.
        /// </returns>
        /// <param name="id">id del ejercicio a eliminar</param>
        /// <param name="userid">id del usuario a eliminar</param>
        /// <response code="204">Si se elimino correctamente al ejercicio</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="404">Si no se encontro el usuario</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("eliminar-ejercicio/{id}/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarEjercicio([FromRoute, Required] int id, [FromRoute,Required] int userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            await _ejercicioService.EliminarEjercicio(id,userid);
            return Ok();
        }
        /// <summary>
        ///     Busca un ejercicio por su id.
        /// </summary>
        /// <returns>
        ///     Ejercicio encontrado
        /// </returns>
        /// <param name="id">id del ejercicio a buscar</param>
        /// <response code="204">Si se encontro al ejercicio</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="404">Si no se encontro el ejercicio</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpGet]
        [Route("buscar-ejercicio/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEjercicioById([FromRoute, Required] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            var respuesta = await _ejercicioService.GetEjercicioById(id);
            return Ok(respuesta);
        }
        /// <summary>
        ///     Busca ejercicios por su usuario.
        /// </summary>
        /// <returns>
        ///     Ejercicios encontrados
        /// </returns>
        /// <param name="id">id del usuario a buscar los ejercicios</param>
        /// <response code="204">Si se encontro los ejercicios</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="404">Si no se encontro el usuario</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpGet]
        [Route("buscar-ejercicios/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEjerciciosByUser([FromRoute, Required] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            var respuesta = await _ejercicioService.GetEjerciciosByUser(id);
            return Ok(respuesta);
        }
        /// <summary>
        ///     Busca todos los ejercicios
        /// </summary>
        /// <returns>
        ///     Ejercicios encontrados
        /// </returns>
        /// <response code="204">Si se encontroron los ejercicios</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpGet]
        [Route("ejercicios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEjercicios()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            var respuesta = await _ejercicioService.GetEjercicios();
            return Ok(respuesta);
        }

        /// <summary>
        ///     Genera un pdf del ejercicio
        /// </summary>
        /// <returns>
        ///     Pdf del ejercicio
        /// </returns>
        /// <response code="204">Si se creo el pdf</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpGet]
        [Route("pdf/{userid}/{ejercicioid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<FileResult> GetPdfEjercicio([FromRoute,Required] int ejercicioid, [FromRoute,Required] int userid)
        {
            var respuesta = await _ejercicioService.GetPdfEjercicio(ejercicioid,userid);
            var ej = _ejercicioService.GetEjercicioById(ejercicioid);
            if (respuesta == null || ej == null)
            {
                throw new AppException("Ejercicio invalido", HttpStatusCode.NotFound);
            }
            string nombre = _ejercicioService.GetNombrePdf(ej.Result.Titulo);
            var res = File(respuesta, "application/pdf", nombre);
            return res;
        }
    }
}
