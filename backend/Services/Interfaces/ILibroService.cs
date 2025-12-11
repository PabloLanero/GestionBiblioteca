using Biblio.models;

namespace Biblio.Services
{
    public interface ILibroService
    {
        public Task<List<Libro>> GetLibrosAsync(string ISBN, string Title, bool OrderAsc);
        public Task<Libro> GetOneLibroAsync(string Id);
        public Task<bool> PostLibroAsync(Libro libro);
        public Task<bool> PutLibroAsync(Libro libro);
        public Task<bool> DeleteLibroAsync(string ISBNLibro);
    }
}