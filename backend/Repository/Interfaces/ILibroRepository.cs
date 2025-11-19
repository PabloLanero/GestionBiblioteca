using Biblio.models;

namespace Biblio.Repositories
{
    public interface ILibroRepository
    {
        public Task<List<Libro>> GetLibrosAsync();
        public Task<Libro> GetLibroAsync();
        public Task<Libro> PostLibroAsync();
        public Task<Libro> DeleteLibroAsync();
    }
}