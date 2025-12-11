namespace Biblio.models
{
    public class ResenaDTO {
        public int Id {get;set;}
        public string resena {get;set;}
        public int IdUsuario {get;set;}
        public string ISBNLibro {get;set;}
        public int valoracion {get;set;}
        public DateTime fechaResena {get;set;}
        public ResenaDTO(){}
    }
}