namespace PeliculasWeb.Utilidades
{
    public class Constantes
    {
        public static string UrlBaseApi = "https://localhost:7282/";
        public static string RutaCategoriasApi = UrlBaseApi + "api/v1/Categorias/";
        public static string RutaPeliculasApi = UrlBaseApi + "api/v1/Peliculas/";
        public static string RutaUsuariosApi = UrlBaseApi + "api/v1/usuarios/";

        //Faltan otras rutas para buscar y filtrar peliculas por categorias
        public static string RutaPeliculasEnCategoriaApi = UrlBaseApi + "api/v1/Peliculas/GetPeliculasEnCategoria/";
        public static string RutaPeliculasBusquedaApi = UrlBaseApi + "api/v1/Peliculas/Buscar?nombre=";
    }
}
