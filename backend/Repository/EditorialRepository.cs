using System.Data.Common;
using Biblio.models;
using MySql.Data.MySqlClient;
using Mysqlx.Resultset;

namespace Biblio.Repositories
{
    public class EditorialRepository : IEditorialRepository
    {
        private readonly string _connectionString;
        public EditorialRepository(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("BiblioDB")?? "";
        }

        public async Task<List<Editorial>> GetEditorialesAsync()
        {
            List<Editorial> editorials = new List<Editorial>();
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT Id, Nombre, Direccion, Telefono, Email, FechaFundacion, SitioWeb FROM Editorial ";
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
            }
            return editorials;
        }

        public async Task<bool> PostEditorialAsync(Editorial editorial)
        {
            bool bRet = true;
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
                        if(rowsAffected >1)throw new Exception("Algo raro ha pasado, mas de una fila modificada");
                    }
                }
            }
            return bRet;
        }

        public async Task<bool> PutEditorialAsync(Editorial editorial)
        {
            bool bRet = true;
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
                        if(rowsAffected>1)throw new Exception("Ha afectado a mas de una fila, revisa la base de datos");
                    }
                }
            }
            return bRet;
        }
        public async Task<bool> DeleteEditorialAsync(int id)
        {
            bool bRet = true;
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
                        if(rowsAffected >1)throw new Exception("Se ha modificado mas de una fila, revisar la base de datos ");
                    }
                }
            }
            return bRet;
        }
    }
}