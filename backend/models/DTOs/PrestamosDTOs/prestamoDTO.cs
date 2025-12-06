namespace Biblio.models
{
    /// <summary>
    /// Este DTO es especializado para RECOGER los datos de la base de datos
    /// </summary>
    public class GetPrestamoDTO
    {
        public required int Id {get;set;}
        public required string IdLibro {get;set;} //Para el id
        public required int IdUsuario {get;set;} //Para el id
        public DateTime? FechaPrestamo {get;set;}
        public DateTime? FechaDevolucionPrevista {get;set;}
        public DateTime? FechaDevolucionReal {get;set;}
        public string? EstadoPrestamo {get;set;}
        public double? Multa {get;set;}
        public GetPrestamoDTO(){}
    }
    /// <summary>
    /// Este DTO esta especializado para AÑADIR datos 
    /// </summary>
    public class PostPrestamoDTO
    {
        public required int Id {get;set;}
        public required string IdLibro {get;set;} //Para el id
        public required int IdUsuario {get;set;} //Para el id
        public DateTime? FechaPrestamo {get;set;}
        public DateTime? FechaDevolucionPrevista {get;set;}
        public PostPrestamoDTO(){}
    }
    /// <summary>
    /// Este DTO esta especializado en ACTUALIZAR la base de datos
    /// </summary>
    public class PutPrestamoDTO
    {
        public required int Id {get;set;}
        public DateTime? FechaDevolucionReal {get;set;}
        public string? EstadoPrestamo {get;set;}
        public double? Multa {get;set;}
    }
}