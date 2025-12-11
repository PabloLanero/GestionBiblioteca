using Biblio.models;

namespace Biblio.Repositories
{
    public interface IEditorialRepository
    {
        /// <summary>
        /// Recoge todas las editoriales de la base de datos
        /// </summary>
        /// <returns></returns>
        public Task<List<Editorial>> GetEditorialesAsync(bool OrderAsc);
        /// <summary>
        /// Añade una editorial a la base de datos
        /// </summary>
        /// <param name="editorial"></param>
        /// <returns></returns>
        public Task<bool> PostEditorialAsync(Editorial editorial);
        /// <summary>
        /// Actualiza una editorial en la base de datos
        /// </summary>
        /// <param name="editorial"></param>
        /// <returns></returns>
        public Task<bool> PutEditorialAsync(Editorial editorial);
        /// <summary>
        /// Borra una editorial en funcion del id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteEditorialAsync(int id);
    }
}