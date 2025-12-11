using Biblio.models;

namespace Biblio.Services{
    public interface IResenaService{
        public Task<List<Resena>> GetAllResenaAsync(string ISBNLibro, int Idusuario);
        public Task<bool> PostResenaAsync(ResenaDTO resenaDTO);
        public Task<bool> PutResenaAsync(ResenaDTO resenaDTO);
        public Task<bool> DeleteResenaAsync(int id);

    }
}