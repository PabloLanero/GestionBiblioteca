using System.Data.Common;
using Biblio.models;
using Biblio.Services;
using MySql.Data.MySqlClient;

namespace Biblio.Repositories
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly string _connectionString;
        private readonly IUsuarioService _usuarioService;
        private readonly ILibroService _libroService;
        public PrestamoRepository(IConfiguration p_configuration, IUsuarioService p_usuarioService, ILibroService p_libroService)
        {
            _connectionString = p_configuration.GetConnectionString("BiblioDB") ?? "";
            _usuarioService = p_usuarioService;
            _libroService = p_libroService;
        }


        public async Task<List<Prestamo>> GetPrestamosAsync()
        {
            List<Prestamo> prestamos = new List<Prestamo>();
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT Id, LibroISBN, UsuarioId, FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal, EstadoPrestamo, Multa FROM Prestamos;";
                using(MySqlCommand command = new MySqlCommand(query, conn))
                {
                    using(DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            Prestamo prestamo = new Prestamo
                            {
                                Id = reader.GetInt32(0),
                                Libro = await _libroService.GetOneLibroAsync(reader.GetString(1)),
                                Usuario = new Usuario
                                {
                                    Id = reader.GetInt32(2)
                                },
                                FechaPrestamo = reader.GetDateTime(3),
                                FechaDevolucionPrevista = reader.GetDateTime(4),
                                FechaDevolucionReal = reader.GetDateTime(5),
                                EstadoPrestamo = reader.GetString(6),
                                Multa= reader.GetDouble(7)
                            };

                        }
                    }
                }
            }
            return prestamos;
        }

        public Task<bool> PostPrestamosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutPrestamosAsync()
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeletePrestamosAsync()
        {
            throw new NotImplementedException();
        }
    }
}