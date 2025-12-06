using Biblio.models;
namespace Biblio.Services
{
    public interface IPrestamoService{
        public Task<List<Prestamo>> GetPrestamosAsync();
        public Task<List<Prestamo>> GetPrestamoByLibroAsync(string ISBN);
        public Task<List<Prestamo>> GetPrestamoByUserAsync(int id);
        public Task<bool> PostPrestamoAsync(PostPrestamoDTO prestamoDTO);
        public Task<bool> PutPrestamoAsync(PutPrestamoDTO prestamoDTO);
        public Task<bool> DeletePrestamoAsync(int prestamoId);
    }
}