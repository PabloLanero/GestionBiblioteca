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

        public async Task<List<Libro>> GetLibrosAsync(string ISBN, string Title, bool OrderAsc)
        {
            List<Libro> libros = await _libroRepository.GetLibrosAsync(OrderAsc);
            //Filtramos por ISBN
            if(!string.IsNullOrEmpty(ISBN))
            {
                libros = libros.FindAll(libro => libro.ISBN.Contains(ISBN));
            }
            //Filtramos por titulo
            if(!string.IsNullOrEmpty(Title))
            {
                libros = libros.FindAll(libro => libro.Titulo.Contains(Title));
            }
            return libros;
        }
        
        public async Task<Libro> GetOneLibroAsync(string ISBN)
        {
            Libro libro = await _libroRepository.GetOneLibroAsync(ISBN);
            return libro;
        }
        public async Task<bool> PostLibroAsync(Libro libro)
        {
            bool bRet = await _libroRepository.PostLibroAsync(libro);
            return bRet;
        }
        public async Task<bool> PutLibroAsync(Libro libro)
        {
            bool bRet = await _libroRepository.PutLibroAsync(libro);
            return bRet;
        }

        public async Task<bool> DeleteLibroAsync(string ISBNLibro)
        {
            bool bRet = await _libroRepository.DeleteLibroAsync(ISBNLibro);
            return bRet;
        }

        
    }
}