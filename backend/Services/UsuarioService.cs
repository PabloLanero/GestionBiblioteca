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
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            List<Usuario> usuarios = await _usuarioRepository.GetUsuariosAsync();
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