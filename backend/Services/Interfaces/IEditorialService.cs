using Biblio.models;

namespace Biblio.Services
{
    public interface IEditorialService
    {
        public Task<List<Editorial>> GetEditorialesAsync(int IdEditorial,string nombre, bool orderAsc);
        public Task<bool> PostEditorialAsync(Editorial editorial);
        public Task<bool> PutEditorialAsync(Editorial editorial);
        public Task<bool> DeleteEditorialAsync(int id);
    }
}