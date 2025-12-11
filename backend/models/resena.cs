namespace Biblio.models
{
    public class Resena {
        public int Id {get;set;}
        public string resena {get;set;}
        public Usuario usuario {get;set;}
        public Libro libro {get;set;}
        public int valoracion {get;set;}
        public DateTime fechaResena {get;set;}
        public Resena(){}
    }
}