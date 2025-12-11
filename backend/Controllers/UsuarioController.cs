using System.ComponentModel.DataAnnotations;
using Biblio.models;
using Biblio.Services;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;

namespace Biblio.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService p_usuarioService)
        {
            _usuarioService = p_usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetAllUsuarios([FromHeader (Name ="IdUsuario")]int IdUsuario = 0, [FromHeader (Name ="Nombre")]string Nombre="", [FromHeader (Name ="OrderAsc")]bool OrderAsc = true)
        {
            List<Usuario> usuarios = await _usuarioService.GetUsuariosAsync(IdUsuario,Nombre,OrderAsc);
            return Ok(usuarios);
        }
        [HttpPost]
        public async Task<ActionResult<bool>> PostAllUsuarios([FromBody]Usuario usuario)
        {
            bool bRet = await _usuarioService.PostUsuariosAsync(usuario);
            return Ok(bRet);
        }
        [HttpPut]
        public async Task<ActionResult<bool>> PutAllUsuarios([FromBody]Usuario usuario)
        {
            bool bRet = await _usuarioService.PutUsuariosAsync(usuario);
            return Ok(bRet);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteAllUsuarios([FromQuery(Name ="idUsuario")][Required]int idUsuario)
        {
            bool bRet = await _usuarioService.DeleteUsuariosAsync(idUsuario);
            return Ok(bRet);
        }

    }
}