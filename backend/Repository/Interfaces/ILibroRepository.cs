using Biblio.models;

namespace Biblio.Repositories
{
    public interface ILibroRepository
    {
        public Task<List<Libro>> GetLibrosAsync();
        public Task PostLibroAsync(string ISBNLibro, Libro libro);
        public Task DeleteLibroAsync(string ISBNLibro);
    }
}