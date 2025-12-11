using Biblio.models;
namespace Biblio.Services
{
    public interface IPrestamoService{
        public Task<List<Prestamo>> GetPrestamosAsync(int idPrestamo, int idUsuario, string ISBNLibro, bool OrderAsc);
        public Task<bool> PostPrestamoAsync(PostPrestamoDTO prestamoDTO);
        public Task<bool> PutPrestamoAsync(PutPrestamoDTO prestamoDTO);
        public Task<bool> DeletePrestamoAsync(int prestamoId);
    }
}