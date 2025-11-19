using Microsoft.AspNetCore.Mvc;
using Biblio.models;
using Biblio.Services;

namespace Biblio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibroController : ControllerBase
    {
        private readonly ILogger<Libro> _logger;
        private readonly ILibroService _libroService;
        public LibroController(ILogger<Libro> logger, ILibroService libroService)
        {
            _logger = logger;
            _libroService =libroService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> GetAllLibros()
        {
            List<Libro> lstLibros = await _libroService.GetLibrosAsync();
            
            return Ok(lstLibros);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteLibroAsync([FromQuery(Name = "ISBN")] string ISBN)
        {
            await _libroService.DeleteLibroAsync(ISBN);
            return Ok();
        }
    }
}