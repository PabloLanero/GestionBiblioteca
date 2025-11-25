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

        public async Task<bool> PostAutorAsync(Autor autor)
        {
            bool bRet = await _autorRepository.PostAutorAsync(autor);
            return bRet;
        }

        public async Task<bool> PutAutorAsync(Autor autor)
        {
            bool bRet = await _autorRepository.PutAutorAsync(autor);
            return bRet;
        }
        public async Task<bool> DeleteAutorAsync(int id)
        {
            bool bRet = await _autorRepository.DeleteAutorAsync(id);
            return bRet;
        }

    }
}