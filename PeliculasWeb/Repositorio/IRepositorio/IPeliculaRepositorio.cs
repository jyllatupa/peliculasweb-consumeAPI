using PeliculasWeb.Models;

namespace PeliculasWeb.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio : IRepositorio<Pelicula>
    {
        Task<IEnumerable<Pelicula>> GetPeliculasTodoAsyncEnt(string url);

        //con paginado
        Task<PeliculaResponse> GetPeliculasTodoAsync(string url);
    }
}
