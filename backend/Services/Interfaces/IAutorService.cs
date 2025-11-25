using Biblio.models;

namespace Biblio.Services
{
    public interface IAutorService
    {
        public Task<List<Autor>> GetAutorsAsync();
        public Task<bool> PostAutorAsync();
        public Task<bool> PutAutorAsync();
        public Task<bool> DeleteAutorAsync(int id);
    } 
}