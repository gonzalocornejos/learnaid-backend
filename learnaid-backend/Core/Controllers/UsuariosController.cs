using learnaid_backend.Core.DataTransferObjects.Usuario;
using learnaid_backend.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace learnaid_backend.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        /// <summary>
        ///     Crea un nuevo usuario.
        /// </summary>
        /// <returns>
        ///     Validacion correcta o incorrecta del proceso.
        /// </returns>
        /// <param name="credenciales">datos del usuario a crear</param>
        /// <response code="204">Si se creo el usuario correctamente</response>
        /// <response code="400">Si las credenciales son incorrectas</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("crear-usuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearUsuario([FromBody]CrearUsuarioDTO credenciales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            await _usuarioService.CrearUsuario(credenciales);
            return Ok();
        }
        /// <summary>
        ///     Edita un usuario.
        /// </summary>
        /// <returns>
        ///     Validacion correcta o incorrecta del proceso.
        /// </returns>
        /// <param name="editarUsuarioDTO">datos del usuario a editar</param>
        /// <response code="204">Si se edito correctamente el usuario</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="404">Si no se encontro el usuario</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("editar-usuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarUsuario([FromBody]EditarUsuarioDTO editarUsuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            await _usuarioService.EditarUsuario(editarUsuarioDTO);
            return Ok();
        }
        /// <summary>
        ///     Elimina un usuario.
        /// </summary>
        /// <returns>
        ///     Validacion correcta o incorrecta del proceso.
        /// </returns>
        /// <param name="id">id del usuario a eliminar</param>
        /// <response code="204">Si se elimino correctamente al usuario</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="404">Si no se encontro el usuario</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("eliminar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarUsuario([FromRoute,Required]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            await _usuarioService.EliminarUsuario(id);
            return Ok();
        }
        /// <summary>
        ///     Busca un usuario por su id.
        /// </summary>
        /// <returns>
        ///     Usuario encontrado
        /// </returns>
        /// <param name="id">id del usuario a buscar</param>
        /// <response code="204">Si se encontro al usuario</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="404">Si no se encontro el usuario</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsuarioById([FromRoute, Required] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            var usuario = await _usuarioService.GetUsuarioById(id);
            return Ok(usuario);
        }
        /// <summary>
        ///     Busca un usuario por su email.
        /// </summary>
        /// <returns>
        ///     Usuario encontrado
        /// </returns>
        /// <param name="email">email del usuario a buscar</param>
        /// <response code="204">Si se encontro al usuario</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="404">Si no se encontro el usuario</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpGet]
        [Route("buscar-usuario/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsuarioByEmail([FromRoute, Required] string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            var usuario = await _usuarioService.GetUsuarioByEmail(email);
            return Ok(usuario);
        }
        /// <summary>
        ///     Busca todos los usuarios
        /// </summary>
        /// <returns>
        ///     Usuarios encontrados
        /// </returns>
        /// <response code="204">Si se encontroron los usuarios</response>
        /// <response code="400">Si el los paramtros son incorrectos</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpGet]
        [Route("usuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsuarios()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            var usuarios = await _usuarioService.GetUsuarios();
            return Ok(usuarios);
        }
        /// <summary>
        ///     Inicia sesion.
        /// </summary>
        /// <returns>
        ///     Autorizacion del usuario.
        /// </returns>
        /// <param name="credenciales">Credenciales para el logueo</param>
        /// <response code="204">Si las credenciales son validas</response>
        /// <response code="400">Si no se enviaron correctamente los parametros requeridos</response>
        /// <response code="401">Si las credenciales son invalidas para autenticar</response>
        /// <response code="500">En el caso de haber un problema interno en el codigo</response>
        [HttpPost]
        [Route("iniciar-sesion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Loguearse([FromBody] LoguearseDTO credenciales)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Parametros enviados incorrectamente");
            }
            await _usuarioService.Loguearse(credenciales);
            return Ok();
        }
    }
}
