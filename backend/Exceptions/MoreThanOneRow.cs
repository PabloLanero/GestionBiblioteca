namespace Biblio.Exceptions
{
    public class MoreThanOneRowException : Exception
    {
        /// <summary>
        /// Esta excepcion se utiliza si en caso de que al hacer algun modificacion en la base de datos 
        /// ha afectado a mas de una linea (Cosa que en los casos de UPDATE, INSERT y DELETE no deberia de pasar)
        /// Si se lanza esta excepcion, es muy problable que haya habido un caso de inyeccion de SQL
        /// </summary>
        public MoreThanOneRowException() :base("Ha afectado a mas de una fila, revision urgente de base de datos") {}
    }
}