using PeliculasWeb.Models;

namespace PeliculasWeb.Repositorio.IRepositorio
{
    public interface IAccountRepositorio : IRepositorio<UsuarioAuth>
    {
        Task<UsuarioAuth> LoginAsync(string url, UsuarioAuth itemCrear);
        Task<bool> RegisterAsync(string url, UsuarioAuth itemCrear);
    }
}
