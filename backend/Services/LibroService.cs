using Biblio.models;
using Biblio.Repositories;

namespace Biblio.Services
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;
        public LibroService(ILibroRepository p_libroRepository)
        {
            _libroRepository = p_libroRepository;
        }

        public async Task<List<Libro>> GetLibrosAsync()
        {
            List<Libro> libros = await _libroRepository.GetLibrosAsync();
            return libros;
        }
    }
}