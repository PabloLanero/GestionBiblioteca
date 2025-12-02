using Biblio.models;

namespace Biblio.Services
{
    public interface IUsuarioRepository
    {
        public Task<List<Usuario>> GetUsuariosAsync();
        public Task<bool> PostUsuariosAsync();
        public Task<bool> PutUsuariosAsync();
        public Task<bool> DeleteUsuariosAsync();
    }
}