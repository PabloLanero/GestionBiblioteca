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

        public async Task<bool> PostAutorAsync(Autor autor)
        {
            bool bRet = true;
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
                        if(rowsAffected >1)throw new Exception("Algo raro ha pasado, mas de una fila modificada");
                    }
                }
            }
            return bRet;
        }

        public async Task<bool> PutAutorAsync(Autor autor)
        {
            bool bRet = true;
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
                        if(rowsAffected>1)throw new Exception("Algo ha pasado y ha afectado a varias filas");
                    }
                }
            }
            return bRet;
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