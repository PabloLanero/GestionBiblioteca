using Biblio.models;

namespace Biblio.Repositories
{
    public interface IEditorialRepository
    {
        public Task<List<Editorial>> GetEditorialesAsync();
        public Task<bool> PostEditorialAsync(Editorial editorial);
        public Task<bool> PutEditorialAsync(Editorial editorial);
        public Task<bool> DeleteEditorialAsync(int id);
    }
}