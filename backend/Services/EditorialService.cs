using Biblio.models;
using Biblio.Repositories;
using Microsoft.AspNetCore.Components.Forms;

namespace Biblio.Services
{
    public class EditorialService: IEditorialService
    {
        private IEditorialRepository _editorialRepository;
        public EditorialService(IEditorialRepository p_editorialRepository)
        {
            _editorialRepository = p_editorialRepository;
        }

        

        public async Task<List<Editorial>> GetEditorialesAsync(int IdEditorial,string nombre, bool OrderAsc)
        {
            List<Editorial> editorials = await _editorialRepository.GetEditorialesAsync(OrderAsc);
            if(IdEditorial > 0)
            {
                editorials = editorials.FindAll(editorial =>editorial.Id == IdEditorial);
            }
            if(!string.IsNullOrEmpty(nombre))
            {
                editorials = editorials.FindAll(editorial =>editorial.Nombre.Contains(nombre));
            }
            return editorials;
        }

        public async Task<bool> PostEditorialAsync(Editorial editorial)
        {
            bool bRet = await _editorialRepository.PostEditorialAsync(editorial);
            return bRet;
        }

        public async Task<bool> PutEditorialAsync(Editorial editorial)
        {
            bool bRet = await _editorialRepository.PutEditorialAsync(editorial);
            return bRet;
        }

        public async Task<bool> DeleteEditorialAsync(int id)
        {
            bool bRet = await _editorialRepository.DeleteEditorialAsync(id);
            return bRet;
        }
    }
}