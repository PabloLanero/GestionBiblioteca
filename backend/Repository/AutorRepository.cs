using System.Data.Common;
using Biblio.models;
using MySql.Data.MySqlClient;

namespace Biblio.Repositories
{
    public class AutorRepository : IAutorRepository
    {
        private readonly string _connectionString;

        public AutorRepository(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("BiblioDB") ?? "";
        }

        

        public async Task<List<Autor>> GetAutorsAsync()
        {
            List<Autor> autors = new List<Autor>();
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT Id, Nombre, Apellido, Nacionalidad, FechaNacimiento, EstaVivo, Biografia FROM Autor WHERE 1=1 ;";
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
            }
            return autors;
        }

        public Task<bool> PostAutorAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutAutorAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAutorAsync(int id)
        {
            bool bRet = true;
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
                        if(rowsAffected >1) throw new Exception("Se ha eliminado a mas de uno, a saber que has hecho");
                    }
                }
            }
            return bRet;
        }
    }
}