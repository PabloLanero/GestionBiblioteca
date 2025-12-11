using System.Data.Common;
using Biblio.Exceptions;
using Biblio.models;
using MySql.Data.MySqlClient;
using Serilog;

namespace Biblio.Repositories
{
    public class AutorRepository : IAutorRepository
    {
        private readonly string _connectionString;

        public AutorRepository(IConfiguration _configuration)
        {
            _connectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? "";//_configuration.GetConnectionString("BiblioDB") ?? "";
            System.Console.WriteLine(_connectionString);
            //Esto viene de la libreria SeriLog, se encargara de escribirlo en un txt
            //Habra que mirar a ver si se puede configurar de alguna manera mas optima
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File("/logs/Autores/logsRepository.txt",rollingInterval: RollingInterval.Day).CreateLogger(); 
        }

        

        public async Task<List<Autor>> GetAutorsAsync(bool OrderAsc)
        {
            List<Autor> autors = new List<Autor>();
            
            try
            {
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT Id, Nombre, Apellido, Nacionalidad, FechaNacimiento, EstaVivo, Biografia FROM Autor WHERE 1=1 ";
                    query += OrderAsc ? " ORDER BY Id ; ":" ORDER BY Id DESC ;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using(DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Autor autor = new Autor
                                {
                                    Id= reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2),
                                    Nacionalidad = reader.GetString(3),
                                    FechaNacimiento = reader.GetDateTime(4),
                                    EstaVivo = reader.GetBoolean(5),
                                    Biografia = reader.GetString(6)
                                };
                                autors.Add(autor);
                            }
                        }
                    }
                    Log.Information("Se ha seleccionado todos los autores");
                }
            }catch(MySqlException ex)
            {
                Log.Error("Revisa la sintaxis de la sentencia"+ex.ToString());
            }catch(Exception ex)
            {
                Log.Fatal("Algo inesperado a ocurrido: "+ex.ToString());
            }
            return autors;
        }

        public async Task<bool> PostAutorAsync(Autor autor)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "INSERT INTO Autor (Id, Nombre, Apellido, Nacionalidad, FechaNacimiento, EstaVivo, Biografia) VALUES "+
                        "(@Id, @Nombre, @Apellido, @Nacionalidad, @FechaNacimiento, @EstaVivo, @Biografia);";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",autor.Id);
                        command.Parameters.AddWithValue("@Nombre",autor.Nombre);
                        command.Parameters.AddWithValue("@Apellido",autor.Apellido);
                        command.Parameters.AddWithValue("@Nacionalidad",autor.Nacionalidad);
                        command.Parameters.AddWithValue("@FechaNacimiento",autor.FechaNacimiento);
                        command.Parameters.AddWithValue("@EstaVivo",autor.EstaVivo);
                        command.Parameters.AddWithValue("@Biografia",autor.Biografia);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet=false;
                            if(rowsAffected >1)throw new MoreThanOneRowException();
                        }
                    }
                    Log.Information("Se ha añadido a este autor: "+autor.Id);
                }
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha añadido mas de un Autor, deberias de revisar la base de datos: \r\n"+ex.ToString());
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

        public async Task<bool> PutAutorAsync(Autor autor)
        {
            bool bRet = true;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "UPDATE Autor SET Id = Id, ";
                    if(!string.IsNullOrEmpty(autor.Nombre)) query += "Nombre = @Nombre , ";
                    if(!string.IsNullOrEmpty(autor.Apellido)) query += "Apellido = @Apellido , ";
                    if(!string.IsNullOrEmpty(autor.Nacionalidad))query += "Nacionalidad = @Nacionalidad , ";
                    if(DateTime.Now >autor.FechaNacimiento)query += "FechaNacimiento = @FechaNacimiento , ";
                    if(autor.EstaVivo != null)query += "EstaVivo = @EstaVivo , ";
                    if(!string.IsNullOrEmpty(autor.Biografia))query += "Biografia = @Biografia , ";

                    query +=" Id = Id WHERE Id = @Id ;";

                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",autor.Id);
                        if(!string.IsNullOrEmpty(autor.Nombre)) command.Parameters.AddWithValue("@Nombre",autor.Nombre);
                        if(!string.IsNullOrEmpty(autor.Apellido)) command.Parameters.AddWithValue("@Apellido",autor.Apellido);
                        if(!string.IsNullOrEmpty(autor.Nacionalidad))command.Parameters.AddWithValue("@Nacionalidad",autor.Nacionalidad);
                        if(DateTime.Now >autor.FechaNacimiento)command.Parameters.AddWithValue("@FechaNacimiento",autor.FechaNacimiento);
                        if(autor.EstaVivo != null)command.Parameters.AddWithValue("@EstaVivo",autor.EstaVivo);
                        if(!string.IsNullOrEmpty(autor.Biografia))command.Parameters.AddWithValue("@Biografia",autor.Biografia);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet = false;
                            if(rowsAffected>1)throw new MoreThanOneRowException();
                        }
                    }
                    Log.Information("Se ha actualizado a este autor: "+autor.Id);
                }
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha añadido mas de un Autor, deberias de revisar la base de datos: \r\n"+ex.ToString());
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
        public async Task<bool> DeleteAutorAsync(int id)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "DELETE FROM Autor WHERE Id = @Id ;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",id);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected != 1)
                        {
                            bRet = false;
                            if(rowsAffected >1) throw new MoreThanOneRowException();
                        }
                    }
                    Log.Information("Se ha eliminado a este autor: "+id);
                }
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha añadido mas de un Autor, deberias de revisar la base de datos: \r\n"+ex.ToString());
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