namespace PeliculasWeb.Models
{
    public class PeliculaResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<Pelicula> Items { get; set; }
    }
}
