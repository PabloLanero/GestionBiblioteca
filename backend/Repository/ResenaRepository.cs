using System.Data.Common;
using Biblio.Exceptions;
using Biblio.models;
using MySql.Data.MySqlClient;
using Serilog;

namespace Biblio.Repositories
{
    public class ResenaRepository : IResenaRepository
    {
        private readonly string _connectionString;
        public ResenaRepository(IConfiguration p_configuration)
        {
            _connectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? "";
            //Esto viene de la libreria SeriLog, se encargara de escribirlo en un txt
            //Habra que mirar a ver si se puede configurar de alguna manera mas optima
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File("/logs/Resenas/logsRepository.txt",rollingInterval: RollingInterval.Day).CreateLogger(); 
        }


        public async Task<List<ResenaDTO>> GetAllResenaAsync()
        {
            List<ResenaDTO> resenas = new List<ResenaDTO>();
            try{

                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT Id, Resena, ISBNLibro,  IdUsuario, Valoracion, FechaResena FROM Resenas;";
                    using (MySqlCommand command = new MySqlCommand(query,conn))
                    {
                        using(DbDataReader reader = await command.ExecuteReaderAsync()){
                            while(reader.Read()){
                                ResenaDTO resenaDTO = new ResenaDTO{
                                    Id = reader.GetInt32(0),
                                    resena = reader.GetString(1),
                                    ISBNLibro = reader.GetString(2),
                                    IdUsuario = reader.GetInt32(3),
                                    valoracion = reader.GetInt32(4),
                                    fechaResena = reader.GetDateTime(5), 
                                };
                                resenas.Add(resenaDTO);
                            }
                        }
                    }
                };
                Log.Information("Ha recogido todos las las resenas");
            }catch(MySqlException ex){
                Log.Error("Ha habido un problema al recoger todas las resenas, revisa la cadena de conexion \r\n" + ex.Message);
            }catch(Exception ex){
                Log.Error("Ha habido un problema inesperado al recoger todas las resenas\r\n" + ex.Message);
            }
            
            return resenas;
        }

        public async Task<bool> PostResenaAsync(ResenaDTO resenaDTO)
        {
            bool bRet = true;
            try{

            
            using (MySqlConnection conn = new MySqlConnection(_connectionString)){
                await conn.OpenAsync();
                string query = "INSERT INTO Resenas (Id, Resena, ISBNLibro, IdUsuario, Valoracion, FechaResena) VALUES (@Id, @Resena, @ISBNLibro, @IdUsuario, @Valoracion, @FechaResena);";
                using(MySqlCommand command = new MySqlCommand(query, conn)){
                    command.Parameters.AddWithValue("@Id",resenaDTO.Id);
                    command.Parameters.AddWithValue("@Resena",resenaDTO.resena);
                    command.Parameters.AddWithValue("@ISBNLibro",resenaDTO.ISBNLibro);
                    command.Parameters.AddWithValue("@IdUsuario",resenaDTO.IdUsuario);
                    command.Parameters.AddWithValue("@Valoracion",resenaDTO.valoracion);
                    command.Parameters.AddWithValue("@FechaResena",resenaDTO.fechaResena);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if(rowsAffected != 1){
                        bRet=false;
                        if(rowsAffected > 1) throw new MoreThanOneRowException();
                    }
                }
            }
            Log.Information("Se ha añadido la reseña con exito");
            }catch(Exception ex ){
                bRet=false;
                Log.Fatal("Ha habido un problema inesperado: \r\n"+ex.Message+"\r\n-----------------------");
            }
            return bRet;
        }

        public async Task<bool> PutResenaAsync(ResenaDTO resenaDTO)
        {
            //Si sobra tiempo lo hago bien (Y si me acuerdo)
            bool bRet = true;
            try{

            
                using (MySqlConnection conn = new MySqlConnection(_connectionString)){
                    await conn.OpenAsync();
                    //(@Id, @Resena, @ISBNLibro, @IdUsuario, @Valoracion, @FechaResena)
                    string query = @"UPDATE Resenas 
                    SET Resena= @Resena , ISBNLibro = @ISBNLibro , IdUsuario = @IdUsuario ,
                    Valoracion = @Valoracion , FechaResena = @FechaResena WHERE  Id=@Id";
                    using(MySqlCommand command = new MySqlCommand(query, conn)){
                        command.Parameters.AddWithValue("@Id",resenaDTO.Id);
                        command.Parameters.AddWithValue("@Resena",resenaDTO.resena);
                        command.Parameters.AddWithValue("@ISBNLibro",resenaDTO.ISBNLibro);
                        command.Parameters.AddWithValue("@IdUsuario",resenaDTO.IdUsuario);
                        command.Parameters.AddWithValue("@Valoracion",resenaDTO.valoracion);
                        command.Parameters.AddWithValue("@FechaResena",resenaDTO.fechaResena);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1){
                            bRet=false;
                            if(rowsAffected > 1) throw new MoreThanOneRowException();
                    }
                }
            }
            Log.Information("Se ha actualizado la reseña con exito");
            }catch(Exception ex ){
                bRet=false;
                Log.Fatal("Ha habido un problema inesperado: \r\n"+ex.Message+"\r\n-----------------------");
            }
            return bRet;
        }
        public async Task<bool> DeleteResenaAsync(int id)
        {
            bool bRet = true;
            try{
                using (MySqlConnection conn = new MySqlConnection(_connectionString)){
                    await conn.OpenAsync();
                    string query = "DELETE FROM Resenas WHERE Id = @Id";
                    using(MySqlCommand command = new MySqlCommand(query,conn)){
                        command.Parameters.AddWithValue("@Id",id);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1){
                            bRet=false;
                            if(rowsAffected > 1) throw new MoreThanOneRowException();
                        }
                    }
                }
                Log.Information("Se ha eliminado con exito esta reseña "+id);
            }catch(Exception ex ){
                bRet=false;
                Log.Fatal("Ha habido un problema inesperado: \r\n"+ex.Message+"\r\n-----------------------");
            }
            return bRet;
        }
    }
}