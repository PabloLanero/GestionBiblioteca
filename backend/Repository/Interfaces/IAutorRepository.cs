using Biblio.models;

namespace Biblio.Repositories
{
    public interface IAutorRepository
    {
        public Task<List<Autor>> GetAutorsAsync();
        public Task<bool> PostAutorAsync(Autor autor);
        public Task<bool> PutAutorAsync(Autor autor);
        public Task<bool> DeleteAutorAsync(int id);
    }
}