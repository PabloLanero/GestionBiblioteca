using Biblio.models;

namespace Biblio.Services
{
    public interface IAutorService
    {
        public Task<List<Autor>> GetAutorsAsync();
        public Task<bool> PostAutorAsync(Autor autor);
        public Task<bool> PutAutorAsync(Autor autor);
        public Task<bool> DeleteAutorAsync(int id);
    } 
}