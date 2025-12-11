using Biblio.models;

namespace Biblio.Repositories
{
    public interface IPrestamoRepository
    {
        /// <summary>
        /// Te recogera todos los datos de los prestamos, los datos del libro y del usuario se recogeran 
        /// </summary>
        /// <returns>
        /// Devolvera una lista con todos los datos de los usuarios 
        /// </returns>
        public Task<List<GetPrestamoDTO>> GetPrestamosAsync(bool OrderAsc);
        /// <summary>
        /// Este metodo sera para añadir datos en la base de datos de un prestamo
        /// </summary>
        /// <param name="postPrestamoDTO">Este DTO solo tiene el Id, IdLibro, IdUsuario, 
        /// FechaPrestamo, FechaDevolucionPrevista</param>
        /// <returns>
        /// Devolvera true si todo ha ido bien, en caso de que algo raro haya pasado, devolvera false
        /// </returns>
        public Task<bool> PostPrestamosAsync(PostPrestamoDTO postPrestamoDTO);
        /// <summary>
        /// Este metodo sera para actualizar datos en la base de datos de un prestamo
        /// </summary>
        /// <param name="putPrestamoDTO">Este DTO solo tiene el Id, FechaDevolucionReal, EstadoPrestamo, Multa</param>
        /// <returns>
        /// Devolvera true si todo ha ido bien, en caso de que algo raro haya pasado, devolvera false
        /// </returns>
        public Task<bool> PutPrestamosAsync(PutPrestamoDTO putPrestamoDTO);
        /// <summary>
        /// Este metodo sera para borrar datos en la base de datos de un prestamo
        /// </summary>
        /// <param name="id">Este id debe de ser el id del prestamo que se quiera borrar</param>
        /// <returns>
        /// Devolvera true si todo ha ido bien, en caso de que algo raro haya pasado, devolvera false
        /// </returns>
        public Task<bool> DeletePrestamosAsync(int id);
    }
}