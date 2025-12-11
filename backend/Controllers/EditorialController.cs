using System.ComponentModel.DataAnnotations;
using Biblio.models;
using Biblio.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditorialController : ControllerBase
    {
        private readonly IEditorialService _editorialService;
        public EditorialController(IEditorialService p_editorialService)
        {
            _editorialService = p_editorialService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Editorial>>> GetEditorialesAsync([FromHeader (Name ="IdEditorial")]int IdEditorial = 0,[FromHeader (Name ="Nombre")]string nombre = "", [FromHeader (Name ="OrderAsc")]bool OrderAsc = true)
        {
            List<Editorial> editorials = await _editorialService.GetEditorialesAsync(IdEditorial,nombre, OrderAsc);
            return Ok(editorials);
        }
        [HttpPost]
        public async Task<ActionResult<bool>> PostEditorial(Editorial editorial)
        {
            bool bRet = await _editorialService.PostEditorialAsync(editorial);
            return Ok(bRet);
        }
        [HttpPut]
        public async Task<ActionResult<bool>> PutEditorial(Editorial editorial)
        {
            bool bRet = await _editorialService.PutEditorialAsync(editorial);
            return Ok(bRet);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteEditorial([FromQuery(Name ="IdEditorial")][Required]int id)
        {
            bool bRet = await _editorialService.DeleteEditorialAsync(id);
            return Ok(bRet);
        }
    }
}