using System.Data;
using System.Data.Common;
using Biblio.Exceptions;
using Biblio.models;
using Biblio.Services;
using Serilog;
using MySql.Data.MySqlClient;

namespace Biblio.Repositories
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly string _connectionString;
        private readonly IUsuarioService _usuarioService;
        private readonly ILibroService _libroService;
        public PrestamoRepository(IConfiguration p_configuration, IUsuarioService p_usuarioService, ILibroService p_libroService)
        {
            _connectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? "";
            _usuarioService = p_usuarioService;
            _libroService = p_libroService;

            //Esto viene de la libreria SeriLog, se encargara de escribirlo en un txt
            //Habra que mirar a ver si se puede configurar de alguna manera mas optima
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File("/logs/Prestamos/logsRepository.txt",rollingInterval: RollingInterval.Day).CreateLogger(); 
        }


        /// <inheritdoc/>
        public async Task<List<GetPrestamoDTO>> GetPrestamosAsync(bool OrderAsc)
        {
            List<GetPrestamoDTO> prestamos = new List<GetPrestamoDTO>();
            try
            {
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT Id, LibroISBN, UsuarioId, FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal, EstadoPrestamo, Multa FROM Prestamo ";
                    query += OrderAsc ? " ORDER BY Id ; " : " ORDER BY Id DESC ; ";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using(DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                GetPrestamoDTO prestamo = new GetPrestamoDTO
                                {
                                    Id = reader.GetInt32(0),
                                    IdLibro = reader.GetString(1),
                                    IdUsuario = reader.GetInt32(2),
                                    FechaPrestamo = reader.IsDBNull(3)?  null :reader.GetDateTime(3),
                                    FechaDevolucionPrevista = reader.IsDBNull(4)?  null :reader.GetDateTime(4),
                                    FechaDevolucionReal = reader.IsDBNull(5)?  null : reader.GetDateTime(5),
                                    EstadoPrestamo = reader.IsDBNull(6)?  null :reader.GetString(6),
                                    Multa= reader.IsDBNull(7)?  null: reader.GetDouble(7)
                                };
                                prestamos.Add(prestamo);
                            }
                        }
                    }
                }
                Log.Information("Se ha seleccionado todos los prestamos");
            }catch(MySqlException ex)
            {
                Log.Error("Ha habido un error al seleccionar todos los libros: \r\n"+ex.ToString());
            }catch(Exception ex)
            {
                Log.Error("Ha habido un error inesperado: \r\n"+ ex.ToString());
            }
            return prestamos;
        }

        /// <inheritdoc/>
        public async Task<bool> PostPrestamosAsync(PostPrestamoDTO postPrestamoDTO)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "INSERT INTO Prestamo (Id, LibroISBN, UsuarioId, FechaPrestamo, FechaDevolucionPrevista) VALUES (@Id, @LibroISBN, @UsuarioId, @FechaPrestamo, @FechaDevolucionPrevista);";
                    using (MySqlCommand command = new MySqlCommand(query,conn))
                    {
                        command.Parameters.AddWithValue("@Id",postPrestamoDTO.Id);
                        command.Parameters.AddWithValue("@LibroISBN",postPrestamoDTO.IdLibro);
                        command.Parameters.AddWithValue("@UsuarioId",postPrestamoDTO.IdUsuario);
                        command.Parameters.AddWithValue("@FechaPrestamo",postPrestamoDTO.FechaPrestamo);
                        command.Parameters.AddWithValue("@FechaDevolucionPrevista",postPrestamoDTO.FechaDevolucionPrevista);
                        int rowsAfected = await command.ExecuteNonQueryAsync();
                        if(rowsAfected != 1)
                        {
                            bRet = false;
                            if(rowsAfected >1) throw new MoreThanOneRowException();
                        }
                    }
                }
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha añadido mas de un Prestamo, deberias de revisar la base de datos: \r\n"+ex.ToString());
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
        /// <inheritdoc/>
        public async Task<bool> PutPrestamosAsync(PutPrestamoDTO putPrestamoDTO)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "UPDATE Prestamo SET Id = Id ";
                    if(putPrestamoDTO.FechaDevolucionReal !=null && putPrestamoDTO.FechaDevolucionReal <=DateTime.Now) query += " , FechaDevolucionReal = @FechaDevolucionReal ";
                    if(!string.IsNullOrEmpty(putPrestamoDTO.EstadoPrestamo)) query += " , EstadoPrestamo = @EstadoPrestamo ";
                    if(putPrestamoDTO.Multa != null && putPrestamoDTO.Multa >0 ) query += " , Multa = @Multa ";
                    query += " WHERE Id = @Id ;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        if(putPrestamoDTO.FechaDevolucionReal !=null && putPrestamoDTO.FechaDevolucionReal <=DateTime.Now) command.Parameters.AddWithValue("@FechaDevolucionReal",putPrestamoDTO.FechaDevolucionReal);
                        if(!string.IsNullOrEmpty(putPrestamoDTO.EstadoPrestamo))command.Parameters.AddWithValue("@EstadoPrestamo",putPrestamoDTO.EstadoPrestamo);
                        if(putPrestamoDTO.Multa != null && putPrestamoDTO.Multa >0 )command.Parameters.AddWithValue("@Multa",putPrestamoDTO.Multa);
                        command.Parameters.AddWithValue("@Id",putPrestamoDTO.Id);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet = false;
                            if(rowsAffected >1 ) throw new MoreThanOneRowException();
                        }
                    }
                }
            }catch(MoreThanOneRowException ex)
            {
                Log.Error("Se ha actualizado mas de un prestamo, deberias de revisar la base de datos: \r\n"+ex.ToString());
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
        /// <inheritdoc/>
        public async Task<bool> DeletePrestamosAsync(int id)
        {
            bool bRet = true;
            try
            {
                
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = " DELETE FROM Prestamo WHERE Id = @Id ;";
                    using(MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Id",id);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if(rowsAffected != 1)
                        {
                            bRet = false;
                            if(rowsAffected > 1)throw new MoreThanOneRowException();
                        }
                    }
                }
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