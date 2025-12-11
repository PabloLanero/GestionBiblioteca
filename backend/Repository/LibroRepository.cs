using System.Data.Common;
using Biblio.Exceptions;
using Biblio.models;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using Serilog;

namespace Biblio.Repositories
{
    public class LibroRepository : ILibroRepository
    {
        private readonly string _connectionString;
        public LibroRepository(IConfiguration p_configuration)
        {
            _connectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? "";
            //Esto viene de la libreria SeriLog, se encargara de escribirlo en un txt
            //Habra que mirar a ver si se puede configurar de alguna manera mas optima
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File("/logs/Libros/logsRepository.txt",rollingInterval: RollingInterval.Day).CreateLogger(); 
        }

        public async Task<List<Libro>> GetLibrosAsync(bool OrderAsc)
        {
            List<Libro> libros = new List<Libro>();
            try{
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT ISBN, Titulo, Genero, Precio, Disponible, NumeroPaginas,FechaPublicacion FROM Libro ";
                    query += OrderAsc ? " ORDER BY ISBN ; " : " ORDER BY ISBN DESC ; ";
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
                    Log.Information("Se ha seleccionado todos los libros");
                    await conn.CloseAsync();
                }
            }catch(MySqlException ex)
            {
                Log.Error("Ha habido un error al seleccionar todos los libros: \r\n"+ex.ToString());
            }catch(Exception ex)
            {
                Log.Error("Ha habido un error inesperado: \r\n"+ ex.ToString());
            }

            return libros;
        }
        public async Task<Libro> GetOneLibroAsync(string ISBN)
        {
            Libro librito = new Libro
            {
                ISBN = ISBN
            };
            try
            {
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT ISBN, Titulo, Genero, Precio, Disponible, NumeroPaginas,FechaPublicacion FROM Libro WHERE ISBN = @ISBN";
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@ISBN",ISBN);
                        using (DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                librito = new Libro
                                {
                                    ISBN = reader.GetString(0),
                                    Titulo = reader.GetString(1),
                                    Genero = reader.GetString(2),
                                    Precio = (double)reader.GetDecimal(3),
                                    Disponible = reader.GetBoolean(4),
                                    FechaPublicacion = reader.GetDateTime(6),
                                    NumeroPaginas = reader.GetInt32(5)  
                                };
                                
                            }
                        }
                    }
                    await conn.CloseAsync();
                }
                Log.Information("Se ha seleccionado con exito todos los libros");
            }catch(MySqlException ex)
            {
                Log.Error("Ha habido un error al seleccionar todos los libros: \r\n"+ex.ToString());
            }catch(Exception ex)
            {
                Log.Error("Ha habido un error inesperado: \r\n"+ ex.ToString());
            }
            return librito;
        }

        public async Task<bool> PutLibroAsync( Libro libro)
        {
            bool bRet = true;
            try{
                
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
                            if(rowsAfected >1)throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information($"Se ha actualizado con exito esta libro: {libro.ISBN}");
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha actualizado mas de un Libro, deberias de revisar la base de datos: \r\n"+ex.ToString());
                bRet = false;
            }catch(MySqlException ex)
            {
                Log.Error("Algo inesperado ha ocurrido, deberias de revisar la sintaxis de la sentencia: \r\n"+ex.ToString());
                bRet = false;
            }catch(Exception ex)
            {
                Log.Fatal("Ha ocurrido un error inesperado, deberias de revisar la base de datos: \r\n"+ex.ToString());
                bRet = false;
            }
            return bRet;
        }
        public async Task<bool> PostLibroAsync(Libro libro)
        {
            bool bRet = true;
            try
            {
                
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
                            if(rowsAfected>1)throw new MoreThanOneRowException();
                        }
                    }

                }
                Log.Information("Se ha añadido con exito este libro: "+libro.ISBN);
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha añadido mas de un Libro, deberias de revisar la base de datos: \r\n"+ex.ToString());
                bRet = false;
            }catch(MySqlException ex)
            {
                Log.Error("Algo inesperado ha ocurrido, deberias de revisar la sintaxis de la sentencia: \r\n"+ex.ToString());
                bRet = false;
            }catch(Exception ex)
            {
                Log.Fatal("Ha ocurrido un error inesperado, deberias de revisar la base de datos: \r\n"+ex.ToString());
                bRet = false;
            }
            return bRet;
        }

        
        public async Task<bool> DeleteLibroAsync(string ISBNLibro)
        {
            bool bRet = true;
            try
            {
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
                            if(rowsAffected>1)throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information($"Se ha eliminado con exito este libro: {ISBNLibro}");
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha añadido mas de un Libro, deberias de revisar la base de datos: \r\n"+ex.ToString());
                bRet = false;
            }catch(MySqlException ex)
            {
                Log.Error("Algo inesperado ha ocurrido, deberias de revisar la sintaxis de la sentencia: \r\n"+ex.ToString());
                bRet = false;
            }catch(Exception ex)
            {
                Log.Fatal("Ha ocurrido un error inesperado, deberias de revisar la base de datos: \r\n"+ex.ToString());
                bRet = false;
            }
            return bRet;
        }

        
    }
}