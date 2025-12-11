using Biblio.models;

namespace Biblio.Repositories
{
    public interface IAutorRepository
    {
        /// <summary>
        /// Devuelve todos los autores en una lista
        /// </summary>
        /// <returns></returns>
        public Task<List<Autor>> GetAutorsAsync(bool OrderAsc);
        /// <summary>
        /// Añade un autor de la base de datos
        /// </summary>
        /// <param name="autor"></param>
        /// <returns></returns>
        public Task<bool> PostAutorAsync(Autor autor);
        /// <summary>
        /// Actualiza un autor en funcion del id que tenga
        /// </summary>
        /// <param name="autor"></param>
        /// <returns></returns>
        public Task<bool> PutAutorAsync(Autor autor);
        /// <summary>
        /// Borra un autor en funcion del id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteAutorAsync(int id);
    }
}