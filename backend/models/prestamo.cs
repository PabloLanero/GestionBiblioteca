namespace Biblio.models
{
    public class Prestamo
    {
        public required int Id {get;set;}
        public Libro Libro {get;set;} //Para el id
        public Usuario Usuario {get;set;} //Para el id
        public DateTime? FechaPrestamo {get;set;}
        public DateTime? FechaDevolucionPrevista {get;set;}
        public DateTime? FechaDevolucionReal {get;set;}
        public string? EstadoPrestamo {get;set;}
        public double? Multa {get;set;}
        public Prestamo(){}
    }
}