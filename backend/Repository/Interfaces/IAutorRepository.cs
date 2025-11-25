using Biblio.models;

namespace Biblio.Repositories
{
    public interface IAutorRepository
    {
        public Task<List<Autor>> GetAutorsAsync();
        public Task<bool> PostAutorAsync();
        public Task<bool> PutAutorAsync();
        public Task<bool> DeleteAutorAsync(int id);
    }
}