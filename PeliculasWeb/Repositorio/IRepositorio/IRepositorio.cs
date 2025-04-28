using System.Collections;

namespace PeliculasWeb.Repositorio.IRepositorio
{
    //IRepositorio<T>: es una interfaz genérica, donde T es un tipo genérico. Eso significa que no está atada a una entidad específica
    //where T : class: Le dice al compilador: “El tipo T tiene que ser una clase (no un struct ni un tipo primitivo)
    public interface IRepositorio<T> where T : class
    {
        Task<IEnumerable> GetTodoAsync(string url);
        Task<IEnumerable> GetTodoAsyncUser(string url, string token);
        Task<IEnumerable> GetPeliculasEnCategoriaAsync(string url, int categoriaid);
        Task<IEnumerable> Buscar(string url, string nombre);
        Task<T> GetAsync(string url, int Id);
        Task<bool> CrearAsync(string url, T itemCrear, string token);
        Task<bool> CrearPeliculaAsync(string url, T peliculaCrear, string token);
        Task<bool> ActualizarAsync(string url, T itemActualizar, string token);
        Task<bool> ActualizarPeliculaAsync(string url, T peliculaActualizar, string token);
        Task<bool> BorrarAsync(string url, int Id, string token);
    }
}
