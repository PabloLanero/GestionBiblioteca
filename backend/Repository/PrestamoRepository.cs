using System.Data;
using System.Data.Common;
using Biblio.Exceptions;
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


        /// <inheritdoc/>
        public async Task<List<GetPrestamoDTO>> GetPrestamosAsync()
        {
            List<GetPrestamoDTO> prestamos = new List<GetPrestamoDTO>();
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT Id, LibroISBN, UsuarioId, FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal, EstadoPrestamo, Multa FROM Prestamo;";
                using(MySqlCommand command = new MySqlCommand(query, conn))
                {
                    using(DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            GetPrestamoDTO prestamo = new GetPrestamoDTO
                            {
                                Id = reader.GetInt32(0),
                                IdLibro = reader.GetString(1),
                                IdUsuario = reader.GetInt32(2),
                                FechaPrestamo = reader.IsDBNull(3)?  null :reader.GetDateTime(3),
                                FechaDevolucionPrevista = reader.IsDBNull(4)?  null :reader.GetDateTime(4),
                                FechaDevolucionReal = reader.IsDBNull(5)?  null : reader.GetDateTime(5),
                                EstadoPrestamo = reader.IsDBNull(6)?  null :reader.GetString(6),
                                Multa= reader.IsDBNull(7)?  null: reader.GetDouble(7)
                            };
                            prestamos.Add(prestamo);
                        }
                    }
                }
            }
            return prestamos;
        }

        /// <inheritdoc/>
        public async Task<bool> PostPrestamosAsync(PostPrestamoDTO postPrestamoDTO)
        {
            bool bRet = true;
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Prestamo (Id, LibroISBN, UsuarioId, FechaPrestamo, FechaDevolucionPrevista) VALUES (@Id, @LibroISBN, @UsuarioId, @FechaPrestamo, @FechaDevolucionPrevista);";
                using (MySqlCommand command = new MySqlCommand(query,conn))
                {
                    command.Parameters.AddWithValue("@Id",postPrestamoDTO.Id);
                    command.Parameters.AddWithValue("@LibroISBN",postPrestamoDTO.IdLibro);
                    command.Parameters.AddWithValue("@UsuarioId",postPrestamoDTO.IdUsuario);
                    command.Parameters.AddWithValue("@FechaPrestamo",postPrestamoDTO.FechaPrestamo);
                    command.Parameters.AddWithValue("@FechaDevolucionPrevista",postPrestamoDTO.FechaDevolucionPrevista);
                    int rowsAfected = await command.ExecuteNonQueryAsync();
                    if(rowsAfected != 1)
                    {
                        bRet = false;
                        if(rowsAfected >1) throw new MoreThanOneRowException();
                    }
                }
            }
            return bRet;
        }
        /// <inheritdoc/>
        public async Task<bool> PutPrestamosAsync(PutPrestamoDTO putPrestamoDTO)
        {
            bool bRet = true;
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Prestamo SET Id = Id ";
                if(putPrestamoDTO.FechaDevolucionReal !=null && putPrestamoDTO.FechaDevolucionReal <=DateTime.Now) query += " , FechaDevolucionReal = @FechaDevolucionReal ";
                if(!string.IsNullOrEmpty(putPrestamoDTO.EstadoPrestamo)) query += " , EstadoPrestamo = @EstadoPrestamo ";
                if(putPrestamoDTO.Multa != null && putPrestamoDTO.Multa >0 ) query += " , Multa = @Multa ";
                query += " WHERE Id = @Id ;";
                using(MySqlCommand command = new MySqlCommand(query, conn))
                {
                    if(putPrestamoDTO.FechaDevolucionReal !=null && putPrestamoDTO.FechaDevolucionReal <=DateTime.Now) command.Parameters.AddWithValue("@FechaDevolucionReal",putPrestamoDTO.FechaDevolucionReal);
                    if(!string.IsNullOrEmpty(putPrestamoDTO.EstadoPrestamo))command.Parameters.AddWithValue("@EstadoPrestamo",putPrestamoDTO.EstadoPrestamo);
                    if(putPrestamoDTO.Multa != null && putPrestamoDTO.Multa >0 )command.Parameters.AddWithValue("@Multa",putPrestamoDTO.Multa);
                    command.Parameters.AddWithValue("@Id",putPrestamoDTO.Id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if(rowsAffected != 1)
                    {
                        bRet = false;
                        if(rowsAffected >1 ) throw new MoreThanOneRowException();
                    }
                }
            }
            return bRet;
        }
        /// <inheritdoc/>
        public async Task<bool> DeletePrestamosAsync(int id)
        {
            bool bRet = true;
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = " DELETE FROM Prestamo WHERE Id = @Id ;";
                using(MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Id",id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if(rowsAffected != 1)
                    {
                        bRet = false;
                        if(rowsAffected > 1)throw new MoreThanOneRowException();
                    }
                }
            }
            return bRet;
        }
    }
}