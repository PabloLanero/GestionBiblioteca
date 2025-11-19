namespace Biblio.models
{
    public class Libro
    {
        public required string ISBN {get;set;}
        public string? Titulo {get;set;}
        public string? Genero {get;set;}
        public int? NumeroPaginas {get;set;}
        public double? Precio {get;set;}
        public bool? Disponible {get;set;}
        public DateTime? FechaPublicacion {get;set;}
        public Libro(){}
    }
}