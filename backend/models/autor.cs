namespace Biblio.models
{
    public class Autor
    {
        public required int Id {get;set;}
        public string? Nombre {get;set;}
        public string? Apellido {get;set;}
        public string? Nacionalidad {get;set;}
        public DateTime? FechaNacimiento{get;set;}
        public bool? EstaVivo {get;set;}
        public string? Biografia {get;set;}
        public Autor(){}
    }
}