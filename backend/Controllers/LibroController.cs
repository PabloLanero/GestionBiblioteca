using Microsoft.AspNetCore.Mvc;
using Biblio.models;
using Biblio.Services;

namespace Biblio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibroController : ControllerBase
    {
        private readonly ILogger<LibroController> _logger;
        private readonly ILibroService _libroService;
        public LibroController(ILogger<LibroController> logger, ILibroService libroService)
        {
            _logger = logger;
            _libroService =libroService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetAllLibros()
        {
            List<Libro> lstLibros = await _libroService.GetLibrosAsync();
            var info =  string.Join( ",", lstLibros );
            _logger.LogDebug("lstlibros: " + info);
            return Ok(lstLibros);
        }
        [HttpPost]
        public async Task<ActionResult<bool>> PostLibroAsync([FromBody]Libro libro)
        {
            bool bRet = await _libroService.PostLibroAsync(libro);
            return Ok(bRet);
        }
        [HttpPut]
        public async Task<ActionResult<bool>> PutLibroAsync([FromBody]Libro libro)
        {
            bool bRet = await _libroService.PutLibroAsync(libro);
            return Ok(bRet);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteLibroAsync([FromQuery(Name = "ISBN")] string ISBN)
        {
            await _libroService.DeleteLibroAsync(ISBN);
            return Ok();
        }
    }
}