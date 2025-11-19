using System.Data.Common;
using Biblio.models;
using MySql.Data.MySqlClient;

namespace Biblio.Repositories
{
    public class LibroRepository : ILibroRepository
    {
        private readonly string _connectionString;
        public LibroRepository(IConfiguration p_configuration)
        {
            _connectionString = p_configuration.GetConnectionString("BiblioDB") ?? "";
        }

        public async Task DeleteLibroAsync(string ISBNLibro)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Libro WHERE ISBN = @ISBN ;";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ISBN", ISBNLibro);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        

        public async Task<List<Libro>> GetLibrosAsync()
        {
            List<Libro> libros = new List<Libro>();
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT ISBN, Titulo, Genero, Precio, Disponible, NumeroPaginas,FechaPublicacion FROM Libro";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            Libro libro = new Libro
                            {
                                ISBN = reader.GetString(0),
                                Titulo = reader.GetString(1),
                                Genero = reader.GetString(2),
                                Precio = (double)reader.GetDecimal(3),
                                Disponible = reader.GetBoolean(4),
                                FechaPublicacion = reader.GetDateTime(6),
                                NumeroPaginas = reader.GetInt32(5)  
                            };
                            libros.Add(libro);
                        }
                    }
                }
                await conn.CloseAsync();
            }

            return libros;
        }

        public Task<Libro> PostLibroAsync()
        {
            throw new NotImplementedException();
        }

    }
}