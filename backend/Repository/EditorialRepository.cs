using System.Data.Common;
using Biblio.Exceptions;
using Biblio.models;
using MySql.Data.MySqlClient;
using Mysqlx.Resultset;
using Serilog;

namespace Biblio.Repositories
{
    public class EditorialRepository : IEditorialRepository
    {
        private readonly string _connectionString;
        public EditorialRepository(IConfiguration _configuration)
        {
            _connectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? "";
            //Esto viene de la libreria SeriLog, se encargara de escribirlo en un txt
            //Habra que mirar a ver si se puede configurar de alguna manera mas optima
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File("/logs/Editoriales/logsRepository.txt",rollingInterval:RollingInterval.Day).CreateLogger();
        }

        public async Task<List<Editorial>> GetEditorialesAsync(bool OrderAsc)
        {
            List<Editorial> editorials = new List<Editorial>();
            
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT Id, Nombre, Direccion, Telefono, Email, FechaFundacion, SitioWeb FROM Editorial ";
                    query += OrderAsc ? " ORDER BY Id ; ": "ORDER BY Id DESC ;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using(DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Editorial editorial = new Editorial
                                {
                                    Id = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Direccion = reader.GetString(2),
                                    Telefono = reader.GetString(3),
                                    Email = reader.GetString(4),
                                    FechaFundacion = reader.GetDateTime(5),
                                    SitioWeb = reader.GetString(6)
                                };
                                editorials.Add(editorial);
                            }
                        }
                    }
                    Log.Information("Se ha seleccionado a todas las editoriales");
                }
            
            return editorials;
        }

        public async Task<bool> PostEditorialAsync(Editorial editorial)
        {
            bool bRet = true;
            try
            {
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = @"INSERT INTO BiblioDB.Editorial (Id, Nombre, Direccion, Telefono, Email, FechaFundacion, SitioWeb) 
                    VALUES (@Id, @Nombre, @Direccion, @Telefono, @Email, @FechaFundacion, @SitioWeb);";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",editorial.Id);
                        command.Parameters.AddWithValue("@Nombre",editorial.Nombre);
                        command.Parameters.AddWithValue("@Direccion",editorial.Direccion);
                        command.Parameters.AddWithValue("@Telefono",editorial.Telefono);
                        command.Parameters.AddWithValue("@Email",editorial.Email);
                        command.Parameters.AddWithValue("@FechaFundacion",editorial.FechaFundacion);
                        command.Parameters.AddWithValue("@SitioWeb",editorial.SitioWeb);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet=false;
                            if(rowsAffected >1)throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information("Se ha añadido con exito esta editorial: "+editorial.Id);
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha añadido mas de una editorial, deberias de revisar la base de datos: \r\n"+ex.ToString());
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

        public async Task<bool> PutEditorialAsync(Editorial editorial)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "UPDATE Editorial SET Id = Id, ";
                    if(!string.IsNullOrEmpty(editorial.Nombre)) query += "Nombre = @Nombre , ";
                    if(!string.IsNullOrEmpty(editorial.Direccion)) query += "Direccion = @Direccion , ";
                    if(!string.IsNullOrEmpty(editorial.Telefono))query += "Telefono = @Telefono , ";
                    if(!string.IsNullOrEmpty(editorial.Email))query += "Email = @Email , ";
                    if(DateTime.Now >editorial.FechaFundacion)query += "FechaFundacion = @FechaFundacion , ";
                    if(!string.IsNullOrEmpty(editorial.SitioWeb))query += "SitioWeb = @SitioWeb , ";
                    query +=" Id = Id WHERE Id = @Id ;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        if(!string.IsNullOrEmpty(editorial.Nombre))command.Parameters.AddWithValue("@Nombre",editorial.Nombre);
                        if(!string.IsNullOrEmpty(editorial.Direccion)) command.Parameters.AddWithValue("@Direccion",editorial.Direccion);
                        if(!string.IsNullOrEmpty(editorial.Telefono))command.Parameters.AddWithValue("@Telefono",editorial.Telefono);
                        if(!string.IsNullOrEmpty(editorial.Email))command.Parameters.AddWithValue("@Email",editorial.Email);
                        if(DateTime.Now >editorial.FechaFundacion)command.Parameters.AddWithValue("@FechaFundacion",editorial.FechaFundacion);
                        if(!string.IsNullOrEmpty(editorial.SitioWeb))command.Parameters.AddWithValue("@SitioWeb",editorial.SitioWeb);
                        command.Parameters.AddWithValue("@Id",editorial.Id);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet= false;
                            if(rowsAffected>1)throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information($"Se ha actualizado con exito esta editorial: {editorial.Id}");
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha actualizado mas de una editorial, deberias de revisar la base de datos: \r\n"+ex.ToString());
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
        public async Task<bool> DeleteEditorialAsync(int id)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "DELETE FROM Editorial WHERE Id = @Id";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",id);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet = false;
                            if(rowsAffected >1)throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information($"Se ha eliminado con extio esta editorial: {id}");
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha eliminado mas de una editorial, deberias de revisar la base de datos: \r\n"+ex.ToString());
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