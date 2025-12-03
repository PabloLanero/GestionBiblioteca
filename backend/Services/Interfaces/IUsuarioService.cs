using Biblio.models;

namespace Biblio.Services
{
    public interface IUsuarioService
    {
        public Task<List<Usuario>> GetUsuariosAsync();
        public Task<bool> PostUsuariosAsync(Usuario usuario);
        public Task<bool> PutUsuariosAsync(Usuario usuario);
        public Task<bool> DeleteUsuariosAsync(int id);
    }
}