using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Biblio.models;
using Biblio.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;
        public PrestamoController(IPrestamoService p_prestamoService)
        {
            _prestamoService = p_prestamoService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Prestamo>>> GetPrestamosController()
        {
            List<Prestamo> prestamos = await _prestamoService.GetPrestamosAsync();
            return Ok(prestamos);
        }
        [HttpPost]
        public async Task<ActionResult<bool>> PostPrestamoController([FromBody]PostPrestamoDTO postPrestamoDTO)
        {
            bool bRet = await _prestamoService.PostPrestamoAsync(postPrestamoDTO);
            return Ok(bRet);
        }
        [HttpPut]
        public async Task<ActionResult<bool>> PutPrestamoController([FromBody]PutPrestamoDTO putPrestamoDTO)
        {
            bool bRet = await _prestamoService.PutPrestamoAsync(putPrestamoDTO);
            return Ok(bRet);
        }
        
        [HttpDelete]
        public async Task<ActionResult<bool>> DeletePrestamoController([FromHeader( Name ="IdPrestamo")][Required] int id)
        {
            bool bRet = await _prestamoService.DeletePrestamoAsync(id);
            return Ok(bRet);
        }
    }
}