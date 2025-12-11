using Biblio.models;
using Biblio.Repositories;
using Serilog;

namespace Biblio.Services
{
    public class ResenaService : IResenaService{
        private readonly IResenaRepository _ResenaRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly ILibroService _LibroService;
        
        public ResenaService(IResenaRepository p_ResenaRepository, IUsuarioService p_usuarioService, 
        ILibroService p_LibroService)
        {
            _ResenaRepository = p_ResenaRepository;
            _usuarioService = p_usuarioService;
            _LibroService = p_LibroService;
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.File("/logs/Resenas/logsService.txt",rollingInterval: RollingInterval.Day).CreateLogger();
        }

        public async Task<List<Resena>> GetAllResenaAsync(string ISBNLibro, int Idusuario)
        {
            List<ResenaDTO> resenaDTOs = await _ResenaRepository.GetAllResenaAsync();
            List<Resena> resenas = new List<Resena>();
            foreach (ResenaDTO item in resenaDTOs)
            {
                Resena resena = new Resena{
                    Id = item.Id,
                    usuario = await _usuarioService.GetOneUsuarioAsync(item.IdUsuario),
                    libro = await _LibroService.GetOneLibroAsync(item.ISBNLibro),
                    fechaResena = item.fechaResena,
                    valoracion = item.valoracion,
                    resena = item.resena
                };
                resenas.Add(resena);
            }
            
            if (!string.IsNullOrEmpty(ISBNLibro)) resenas = resenas.FindAll(element => element.libro.ISBN.Contains(ISBNLibro));
            if (Idusuario >0) resenas = resenas.FindAll(element => element.usuario.Id == Idusuario);

            return resenas;
        }

        public async Task<bool> PostResenaAsync(ResenaDTO resenaDTO)
        {
            bool bRet = true;
            try{
                if(!(resenaDTO.valoracion >0 && resenaDTO.valoracion <6))throw new Exception("La reseña tiene una valoracion no valida");
                bRet = await _ResenaRepository.PostResenaAsync(resenaDTO);
            }catch(Exception ex){
                Log.Error("Algo ha salido mal:\r\n"+ ex.Message);
                bRet= false;
            }
            return bRet;
        }

        public async Task<bool> PutResenaAsync(ResenaDTO resenaDTO)
        {
            bool bRet = true;
            try{
                if(!(resenaDTO.valoracion >0 && resenaDTO.valoracion <6))throw new Exception("La reseña tiene una valoracion no valida");
                bRet = await _ResenaRepository.PutResenaAsync(resenaDTO);
            }catch(Exception ex){
                Log.Error("Algo ha salido mal:\r\n"+ ex.Message);
                bRet= false;
            }
            return bRet;
        }
        public async Task<bool> DeleteResenaAsync(int id)
        {
            bool bRet = await _ResenaRepository.DeleteResenaAsync(id);
            return bRet;
        }
    }
}