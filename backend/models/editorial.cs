namespace Biblio.models
{
    public class Editorial
    {
        public required int Id {get;set;}
        public string? Nombre {get;set;}
        public string? Direccion {get;set;}
        public string? Telefono {get;set;}
        public string? Email {get;set;}
        public DateTime? FechaFundacion {get;set;}
        public string? SitioWeb {get;set;}
        public Editorial(){}
    }
}