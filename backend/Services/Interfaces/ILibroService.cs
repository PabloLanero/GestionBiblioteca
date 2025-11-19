using Biblio.models;

namespace Biblio.Services
{
    public interface ILibroService
    {
        public Task<List<Libro>> GetLibrosAsync();
        public Task DeleteLibroAsync(string ISBNLibro);
    }
}