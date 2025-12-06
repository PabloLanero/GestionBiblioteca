using System.Runtime.CompilerServices;
using Biblio.models;
using Biblio.Repositories;

namespace Biblio.Services
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly ILibroService _libroService;
        public PrestamoService(IPrestamoRepository p_prestamoRepository, IUsuarioService p_usuarioService, ILibroService p_libroService)
        {
            _prestamoRepository = p_prestamoRepository;
            _usuarioService = p_usuarioService;
            _libroService = p_libroService;
        }

        


        public async Task<List<Prestamo>> GetPrestamosAsync()
        {
            List<GetPrestamoDTO> prestamoDTOs = await _prestamoRepository.GetPrestamosAsync();
            List<Prestamo> prestamos = new List<Prestamo>();
            foreach( GetPrestamoDTO prestamoDTO in prestamoDTOs)
            {
                Prestamo prestamo = new Prestamo
                {
                    Id = prestamoDTO.Id,
                    Libro = await _libroService.GetOneLibroAsync(prestamoDTO.IdLibro),
                    Usuario = await _usuarioService.GetOneUsuarioAsync(prestamoDTO.IdUsuario),
                    FechaPrestamo = prestamoDTO.FechaPrestamo,
                    EstadoPrestamo = prestamoDTO.EstadoPrestamo,
                    FechaDevolucionPrevista = prestamoDTO.FechaDevolucionPrevista,
                    FechaDevolucionReal = prestamoDTO.FechaDevolucionReal,
                    Multa = prestamoDTO.Multa,
                };
                prestamos.Add(prestamo);
            }
            return prestamos;
        }
        public Task<List<Prestamo>> GetPrestamoByLibroAsync(string ISBN)
        {
            throw new NotImplementedException();
        }

        public Task<List<Prestamo>> GetPrestamoByUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PostPrestamoAsync(PostPrestamoDTO prestamoDTO)
        {
            bool bRet = await _prestamoRepository.PostPrestamosAsync(prestamoDTO);
            return bRet;
        }

        public async Task<bool> PutPrestamoAsync(PutPrestamoDTO prestamoDTO)
        {
            bool bRet = await _prestamoRepository.PutPrestamosAsync(prestamoDTO);
            return bRet;
        }
        public async Task<bool> DeletePrestamoAsync(int prestamoId)
        {
            bool bRet = await _prestamoRepository.DeletePrestamosAsync(prestamoId);
            return bRet;
        }
    }
}