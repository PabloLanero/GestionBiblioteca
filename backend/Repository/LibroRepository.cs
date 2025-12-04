using System.Data.Common;
using Biblio.models;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Biblio.Repositories
{
    public class LibroRepository : ILibroRepository
    {
        private readonly string _connectionString;
        public LibroRepository(IConfiguration p_configuration)
        {
            _connectionString = p_configuration.GetConnectionString("BiblioDB") ?? "";
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

        public async Task<bool> PutLibroAsync( Libro libro)
        {
            bool bRet = true;
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Libro SET ISBN = ISBN ";  
                //Recordar preguntar a alejandro si hay alguna forma optima de hacerlo de verdad, porque acaban siendo demasiados ifs
                
                if(!string.IsNullOrEmpty(libro.Titulo)) query += ", Titulo = @Titulo "; 
                if(!string.IsNullOrEmpty(libro.Genero)) query += ", Genero = @Genero ";
                if(libro.NumeroPaginas >0) query += ", NumeroPaginas= @NumeroPaginas ";
                if(libro.Precio >0) query += ", Precio = @Precio ";
                if(libro.Disponible != null) query += ", Disponible = @Disponible ";
                if(libro.FechaPublicacion != null) query += ", FechaPublicacion = @FechaPublicacion ";
                query += " WHERE ISBN= @ISBN ;";
                

                //Una vez validado los datos para poder meterlos, empezamos a meter datos
                using(MySqlCommand command = new MySqlCommand(query, conn))
                {
                    if(!string.IsNullOrEmpty(libro.Titulo)) command.Parameters.AddWithValue("@Titulo",libro.Titulo);
                    if(!string.IsNullOrEmpty(libro.Genero)) command.Parameters.AddWithValue("@Genero",libro.Genero);
                    if(libro.NumeroPaginas >0) command.Parameters.AddWithValue("@NumeroPaginas",libro.NumeroPaginas);
                    if(libro.Precio >0) command.Parameters.AddWithValue("@Precio",libro.Precio);
                    if(libro.Disponible != null) command.Parameters.AddWithValue("@Disponible",libro.Disponible);
                    if(libro.FechaPublicacion != null) command.Parameters.AddWithValue("@FechaPublicacion", libro.FechaPublicacion);
                    command.Parameters.AddWithValue("@ISBN", libro.ISBN);
                    int rowsAfected = await command.ExecuteNonQueryAsync();
                    if (rowsAfected != 1)
                    {
                        bRet = false;
                        if(rowsAfected >1)throw new Exception("Ha afectado a mas de una fila, esto no deberia de pasar");
                    }
                }
            }
            return bRet;
        }
        public async Task<bool> PostLibroAsync(Libro libro)
        {
            bool bRet = true;
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Libro (ISBN, Titulo, Genero, NumeroPaginas, Precio, Disponible, FechaPublicacion) VALUES (@ISBN, @Titulo, @Genero, @NumeroPaginas, @Precio, @Disponible, @FechaPublicacion);";
                using(MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ISBN",libro.ISBN);
                    command.Parameters.AddWithValue("@Titulo",libro.Titulo);
                    command.Parameters.AddWithValue("@Genero",libro.Genero);
                    command.Parameters.AddWithValue("@NumeroPaginas",libro.NumeroPaginas);
                    command.Parameters.AddWithValue("@Precio",libro.Precio);
                    command.Parameters.AddWithValue("@Disponible",libro.Disponible);
                    command.Parameters.AddWithValue("@FechaPublicacion",libro.FechaPublicacion);
                    int rowsAfected = await command.ExecuteNonQueryAsync();
                    if(rowsAfected != 1)
                    {
                        bRet=false;
                        if(rowsAfected>1)throw new Exception("Ha afectado a mas de una columna, revisa la base de datos");
                    }
                }

            }
            return bRet;
        }

        //No funcionara de forma normal, ya que tiene los indices puestos
        public async Task<bool> DeleteLibroAsync(string ISBNLibro)
        {
            bool bRet = true;
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Libro WHERE ISBN = @ISBN ;";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ISBN", ISBNLibro);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if(rowsAffected != 1)
                    {
                        bRet = false;
                        if(rowsAffected>1)throw new Exception("Ha afectado a mas de una fila, revisa que ha pasado");
                    }
                }
            }
            return bRet;
        }

    }
}