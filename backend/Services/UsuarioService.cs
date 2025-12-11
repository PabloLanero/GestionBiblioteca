using Biblio.models;
using Biblio.Repositories;

namespace Biblio.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository p_usuarioRepository)
        {
            _usuarioRepository = p_usuarioRepository;
        }
        public async Task<List<Usuario>> GetUsuariosAsync(int IdUsuario, string Nombre, bool OrderAsc)
        {
            List<Usuario> usuarios = await _usuarioRepository.GetUsuariosAsync(OrderAsc);
            if(IdUsuario > 0)
            {
                usuarios = usuarios.FindAll(usuario => usuario.Id == IdUsuario);
            }
            if(!string.IsNullOrEmpty(Nombre))
            {
                usuarios = usuarios.FindAll(usuario => usuario.Nombre.Contains(Nombre));
            }
            return usuarios;
        }
        public async Task<Usuario> GetOneUsuarioAsync(int id)
        {
            Usuario usuario = await _usuarioRepository.GetOneUsuarioAsync(id);
            return usuario;
        }

        public async Task<bool> PostUsuariosAsync(Usuario usuario)
        {
            bool bRet = await _usuarioRepository.PostUsuarioAsync(usuario);
            return bRet;
        }

        public async Task<bool> PutUsuariosAsync(Usuario usuario)
        {
            bool bRet = await _usuarioRepository.PutUsuarioAsync(usuario);
            return bRet;
        }
        public async Task<bool> DeleteUsuariosAsync(int id)
        {
            bool bRet = await _usuarioRepository.DeleteUsuarioAsync(id);
            return bRet;
        }

        
    }
}