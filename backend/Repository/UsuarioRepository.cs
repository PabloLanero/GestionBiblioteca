using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using Biblio.Exceptions;
using Biblio.models;
using MySql.Data.MySqlClient;
using Serilog;
namespace Biblio.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BiblioDB") ?? "";
            //Esto viene de la libreria SeriLog, se encargara de escribirlo en un txt
            //Habra que mirar a ver si se puede configurar de alguna manera mas optima
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File("/logs/Usuarios/logsRepository.txt",rollingInterval: RollingInterval.Day).CreateLogger(); 
        }


        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT Id, Nombre, Apellido, Email, FechaRegistro, EstaActivo FROM Usuario;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using(DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Usuario usuario = new Usuario
                                {
                                    Id= reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    FechaRegistro = reader.GetDateTime(4),
                                    EstaActivo = reader.GetBoolean(5)
                                };
                                usuarios.Add(usuario);
                            }
                        }
                    }
                }
                Log.Information("Se ha seleccionado todos los Usuarios");
            }catch(MySqlException ex)
            {
                Log.Error("Ha habido un error al seleccionar todos los usuarios: \r\n"+ex.ToString());
            }catch(Exception ex)
            {
                Log.Error("Ha habido un error inesperado: \r\n"+ ex.ToString());
            }
            return usuarios;
        }
        public async Task<Usuario> GetOneUsuarioAsync(int id)
        {
            Usuario usuario = new Usuario
            {
                Id = 0
            };
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT Id, Nombre, Apellido, Email, FechaRegistro, EstaActivo FROM Usuario WHERE Id = @Id;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",id);
                        using(DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    Id= reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    FechaRegistro = reader.GetDateTime(4),
                                    EstaActivo = reader.GetBoolean(5)
                                };
                                
                            }
                        }
                    }
                }

            }catch(MySqlException ex)
            {
                Log.Error($"Ha habido un error al seleccionar un usuario con el id {id}: \r\n"+ex.ToString());
            }catch(Exception ex)
            {
                Log.Error("Ha habido un error inesperado: \r\n"+ ex.ToString());
            }
            return usuario;
        }

        public async Task<bool> PostUsuarioAsync(Usuario usuario)
        {
            bool bRet = true;
            try
            {
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "INSERT INTO Usuario (Id, Nombre, Apellido, Email, FechaRegistro, EstaActivo) VALUES (@Id, @Nombre, @Apellido, @Email, @FechaRegistro, @EstaActivo);";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",usuario.Id);
                        command.Parameters.AddWithValue("@Nombre",usuario.Nombre);
                        command.Parameters.AddWithValue("@Apellido",usuario.Apellido);
                        command.Parameters.AddWithValue("@Email",usuario.Email);
                        command.Parameters.AddWithValue("@FechaRegistro",usuario.FechaRegistro);
                        command.Parameters.AddWithValue("@EstaActivo",usuario.EstaActivo);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet= false;
                            if(rowsAffected>1)throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information("Se ha añadido con exito este usuario: "+usuario.Id);
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha añadido mas de un usuario, deberias de revisar la base de datos: \r\n"+ex.ToString());
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

        public async Task<bool> PutUsuarioAsync(Usuario usuario)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "UPDATE Usuario SET Id = Id, ";
                    if(!string.IsNullOrEmpty(usuario.Nombre)) query += "Nombre = @Nombre , ";
                    if(!string.IsNullOrEmpty(usuario.Apellido)) query += "Apellido = @Apellido , ";
                    if(!string.IsNullOrEmpty(usuario.Email))query += "Email = @Email , ";
                    if(DateTime.Now >usuario.FechaRegistro)query += "FechaRegistro = @FechaRegistro , ";
                    if(usuario.EstaActivo != null)query += "EstaActivo = @EstaActivo , ";
                    query +=" Id = Id WHERE Id = @Id ;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        if(!string.IsNullOrEmpty(usuario.Nombre))command.Parameters.AddWithValue("@Nombre",usuario.Nombre);
                        if(!string.IsNullOrEmpty(usuario.Apellido)) command.Parameters.AddWithValue("@Apellido",usuario.Apellido);
                        if(!string.IsNullOrEmpty(usuario.Email))command.Parameters.AddWithValue("@Email",usuario.Email);
                        if(DateTime.Now >usuario.FechaRegistro)command.Parameters.AddWithValue("@FechaRegistro",usuario.FechaRegistro);
                        if(usuario.EstaActivo != null)command.Parameters.AddWithValue("@EstaActivo",usuario.EstaActivo);
                        command.Parameters.AddWithValue("@Id",usuario.Id);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet= false;
                            if(rowsAffected>1)throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information($"Se ha actualizado con exito este usuario: {usuario.Id}");
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha actualizado mas de un usuario, deberias de revisar la base de datos: \r\n"+ex.ToString());
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
        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "DELETE FROM Usuario WHERE Id = @Id ;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",id);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet= false;
                            if(rowsAffected>1)throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information($"Se ha eliminado con exito este usuario: {id}");
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se haeliminado mas de un usuario, deberias de revisar la base de datos: \r\n\t"+ex.ToString());
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