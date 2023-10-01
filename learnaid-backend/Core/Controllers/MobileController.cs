using learnaid_backend.Core.DataTransferObjects.Ejercicio;
using learnaid_backend.Core.DataTransferObjects.Mobile;
using learnaid_backend.Core.Services;
using learnaid_backend.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace learnaid_backend.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MobileController : ControllerBase
    {
        private readonly IMobileService _mobileService;

        public MobileController(IMobileService mobileService)
        {
            _mobileService = mobileService;
        }

        /// <summary>
        ///     Genera texto y audio a partir de una imagen
        /// </summary>
        /// <returns>
        ///     Texto y audio
        /// </returns>
        /// <param name="foto">datos del ejercicio a adaptar</param>
        /// <response code="204">Si se genero correctamente el texto y el audio a partir de la imagen</response>
        /// <response code="400">Si los parametros se enviaron incorrectamente</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("generar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerarTextoYAudio([FromForm] ImagenDTO imagen)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Parametros enviados incorrectamente");
            //}
            var respuesta = await _mobileService.GenerarTextoYAudio(imagen.Imagen);
            return Ok(respuesta);
        }

        /// <summary>
        ///     Genera texto a partir de una imagen
        /// </summary>
        /// <returns>
        ///     Texto
        /// </returns>
        /// <param name="foto">datos del ejercicio a adaptar</param>
        /// <response code="204">Si se genero correctamente el texto a partir de la imagen</response>
        /// <response code="400">Si los parametros se enviaron incorrectamente</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("generar-texto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerarTexto([FromForm] ImagenDTO imagen)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Parametros enviados incorrectamente");
            //}
            var respuesta = await _mobileService.GenerarTexto(imagen.Imagen);
            return Ok(respuesta);
        }

        /// <summary>
        ///     Genera audio a partir de un texto
        /// </summary>
        /// <returns>
        ///     Texto y audio
        /// </returns>
        /// <param name="foto">datos del ejercicio a adaptar</param>
        /// <response code="204">Si se genero correctamente el audio a partir del texto</response>
        /// <response code="400">Si los parametros se enviaron incorrectamente</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("generar-audio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<FileResult> GenerarAudio([FromForm] string texto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Parametros enviados incorrectamente");
            //}
            var respuesta = await _mobileService.GenerarAudio(texto);
            var res = File(respuesta, "audio/mpeg", "output.mp3");
            return res;
        }
    }
}
