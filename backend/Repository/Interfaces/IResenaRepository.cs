using Biblio.models;

namespace Biblio.Repositories{
    public interface IResenaRepository{
        public Task<List<ResenaDTO>> GetAllResenaAsync();
        public Task<bool> PostResenaAsync(ResenaDTO resenaDTO);
        public Task<bool> PutResenaAsync(ResenaDTO resenaDTO);
        public Task<bool> DeleteResenaAsync(int id);

    }
}