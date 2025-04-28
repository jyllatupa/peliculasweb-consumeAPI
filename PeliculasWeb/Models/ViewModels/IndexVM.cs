namespace PeliculasWeb.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<Categoria> ListaCategorias { get; set; }
        public IEnumerable<Pelicula> ListaPeliculas { get; set; }
        public int TotalPages { get; set; } //Total de páginas disponibles
        public int CurrentPage { get; set; } //Página actual
    }
}
