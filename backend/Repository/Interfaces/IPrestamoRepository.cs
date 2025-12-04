using Biblio.models;

namespace Biblio.Repositories
{
    public interface IPrestamoRepository
    {
        public Task<List<Prestamo>> GetPrestamosAsync();
        public Task<bool> PostPrestamosAsync();
        public Task<bool> PutPrestamosAsync();
        public Task<bool> DeletePrestamosAsync();
    }
}