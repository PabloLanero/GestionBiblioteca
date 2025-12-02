using Biblio.models;

namespace Biblio.Repositories
{
    public interface IUsuarioRepository
    {
        public Task<List<Usuario>> GetUsuariosAsync();
        public Task<bool> PostUsuarioAsync(Usuario usuario);
        public Task<bool> PutUsuarioAsync(Usuario usuario);
        public Task<bool> DeleteUsuarioAsync(int id);
    }
}