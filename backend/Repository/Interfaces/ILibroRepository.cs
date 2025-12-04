using Biblio.models;

namespace Biblio.Repositories
{
    public interface ILibroRepository
    {
        public Task<List<Libro>> GetLibrosAsync();
        public Task<Libro> GetOneLibroAsync(string ISBN);
        public Task<bool> PostLibroAsync( Libro libro);
        public Task<bool> PutLibroAsync(Libro libro);
        public Task<bool> DeleteLibroAsync(string ISBNLibro);
    }
}