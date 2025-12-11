using System.ComponentModel.DataAnnotations;
using Biblio.models;
using Biblio.Services;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace Biblio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutorController : ControllerBase
    {
        private readonly IAutorService _autorService;
        public AutorController(IAutorService p_autorService)
        {
            _autorService = p_autorService;
        }
        /// <summary>
        /// Para recoger todos los autores
        /// </summary>
        /// <remarks>
        /// De momento no requiere de parametros ni puede filtrar
        /// </remarks>
        /// <param name="somepara">Required parameter: Example: </param>
        /// <return>Returns comment</return>
        /// <response code="200">Ok</response>
        [HttpGet]
        public async Task<ActionResult<List<Autor>>> GetAutores([FromHeader (Name = "IdAutor")]int idAutor = 0,[FromHeader (Name = "Nombre")]string Nombre = "", [FromHeader (Name ="OrderAscent")] bool OrderAsc = true)
        {
            List<Autor> autors = await _autorService.GetAutorsAsync(idAutor,Nombre,OrderAsc);
            return Ok(autors);
        }
        /// <summary>
        /// Para añadir un autor 
        /// </summary>
        /// <remarks>
        /// No te voy a decir que el Id no es auto incremental, pero el Id no es auto incremental
        /// </remarks>
        /// <param name="autor">Required parameter: Example: </param>
        /// <return>Returns comment</return>
        /// <response code="200">Ok</response>
        [HttpPost]
        public async Task<ActionResult<bool>> PostAutor([FromBody]Autor autor)
        {
            bool bRet = await _autorService.PostAutorAsync(autor);
            return bRet;
        }
        /// <summary>
        /// Para actualizar un autor 
        /// </summary>
        /// <remarks>
        /// Cambiara el autor en funcion del id que tenga el autor que se envie
        /// </remarks>
        /// <param name="autor">Required parameter: Example: </param>
        /// <return>Returns comment</return>
        /// <response code="200">Ok</response>
        [HttpPut]
        public async Task<ActionResult<bool>> PutAutor([FromBody]Autor autor)
        {
            bool bRet = await _autorService.PutAutorAsync(autor);
            return bRet;
        }

        /// <summary>
        /// Para eliminar un autor
        /// </summary>
        /// <remarks>
        /// Elimina por id
        /// </remarks>
        /// <param name="id">Required parameter: id: </param>
        /// <return>Returns comment</return>
        /// <response code="200">Ok</response>
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteAutor([FromQuery(Name ="IdAutor")][Required] int id)
        {
            bool borrado = await _autorService.DeleteAutorAsync(id);
            return Ok(borrado);
        }
    }
}