using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using Biblio.models;
using MySql.Data.MySqlClient;

namespace Biblio.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BiblioDB") ?? "";
        }


        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            List<Usuario> usuarios = new List<Usuario>();
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
            return usuarios;
        }

        public async Task<bool> PostUsuarioAsync(Usuario usuario)
        {
            bool bRet = true;
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
                        if(rowsAffected>1)throw new Exception("Ha afectado a mas de una fila de datos, revisar base de datos");
                    }
                }
            }
            return bRet;
        }

        public async Task<bool> PutUsuarioAsync(Usuario usuario)
        {
            bool bRet = true;
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Editorial SET Id = Id, ";
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
                        if(rowsAffected>1)throw new Exception("Ha afectado a mas de una fila, revisa la base de datos");
                    }
                }
            }
            return bRet;
        }
        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            bool bRet = true;
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
                        if(rowsAffected>1)throw new Exception("Ha afectado a mas de una fila, revisa la base de datos");
                    }
                }
            }
            return bRet;
        }
    }
}