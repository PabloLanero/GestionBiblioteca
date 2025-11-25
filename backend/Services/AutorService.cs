using Biblio.models;
using Biblio.Repositories;

namespace Biblio.Services
{
    public class AutorService : IAutorService
    {
        private readonly IAutorRepository _autorRepository;

        public AutorService(IAutorRepository p_autorRepository)
        {
            _autorRepository = p_autorRepository;
        }

        
        public async Task<List<Autor>> GetAutorsAsync()
        {
            List<Autor> autors = await _autorRepository.GetAutorsAsync();
            return autors;
        }

        public Task<bool> PostAutorAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutAutorAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAutorAsync(int id)
        {
            bool bRet = await _autorRepository.DeleteAutorAsync(id);
            return bRet;
        }

    }
}