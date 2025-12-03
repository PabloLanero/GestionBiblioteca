using Biblio.models;

namespace Biblio.Services
{
    public interface ILibroService
    {
        public Task<List<Libro>> GetLibrosAsync();
        public Task<bool> PostLibroAsync(Libro libro);
        public Task<bool> PutLibroAsync(Libro libro);
        public Task<bool> DeleteLibroAsync(string ISBNLibro);
    }
}