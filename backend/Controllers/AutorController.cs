using Biblio.models;
using Biblio.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> GetAutores()
        {
            List<Autor> autors = await _autorService.GetAutorsAsync();
            return Ok(autors);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteAutor([FromQuery] int id)
        {
            bool borrado = await _autorService.DeleteAutorAsync(id);
            return Ok(borrado);
        }
    }
}