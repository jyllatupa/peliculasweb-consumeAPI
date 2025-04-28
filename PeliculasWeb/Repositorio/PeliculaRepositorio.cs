using Newtonsoft.Json;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;

namespace PeliculasWeb.Repositorio
{
    public class PeliculaRepositorio : Repositorio<Pelicula>, IPeliculaRepositorio
    {
        //Injeccion de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clientFactory;

        public PeliculaRepositorio(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Pelicula>> GetPeliculasTodoAsyncEnt(string url)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                //Deserializar a PeliculaResponse
                var peliculaResponse = JsonConvert.DeserializeObject<PeliculaResponse>(jsonString);
                //Devolver la lista de peliculas
                //Si peliculaResponse no es null, devuelve Items
                //Si es null, devuelve una lista vacía para evitar errores.
                return peliculaResponse?.Items ?? new List<Pelicula>();
            }
            else
            {
                return new List<Pelicula>();
            }
        }

        public async Task<PeliculaResponse> GetPeliculasTodoAsync(string url)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                //Deserializar a PeliculaResponse
                var peliculaResponse = JsonConvert.DeserializeObject<PeliculaResponse>(jsonString);
                //Devolver la lista de peliculas
                //Si peliculaResponse no es null, devuelve Items
                //Si es null, devuelve una lista vacía para evitar errores.
                return peliculaResponse ?? new PeliculaResponse();
            }
            else
            {
                return new PeliculaResponse();
            }
        }
    }
}
