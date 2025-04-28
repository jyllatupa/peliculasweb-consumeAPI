using Newtonsoft.Json;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Text;

namespace PeliculasWeb.Repositorio
{
    public class AccountRepositorio : Repositorio<UsuarioAuth>, IAccountRepositorio
    {
        //Injeccion de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clientFactory;

        public AccountRepositorio(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<UsuarioAuth> LoginAsync(string url, UsuarioAuth itemCrear)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (itemCrear != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json");
            }
            else
            {
                return new UsuarioAuth();
            }

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(request);


            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                var usuarioAuthRespuesta = JsonConvert.DeserializeObject<UsuarioAuthRespuesta>(jsonString);

                // Mapea los datos de UsuarioAuthRespuesta a UsuarioAuth
                var usuarioAuth = new UsuarioAuth
                {
                    Id = usuarioAuthRespuesta.Result.Usuario.Id,
                    NombreUsuario = usuarioAuthRespuesta.Result.Usuario.Username,
                    Nombre = usuarioAuthRespuesta.Result.Usuario.Nombre,
                    Token = usuarioAuthRespuesta.Result.Token
                };

                //Solo para comprobar si obtiene el token
                Console.WriteLine($"Token recibido: {usuarioAuth.Token}");
                return usuarioAuth;
            }
            else
            {
                var errorContent = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {respuesta.StatusCode} - {errorContent}");
                return new UsuarioAuth();
            }
        }

        public async Task<bool> RegisterAsync(string url, UsuarioAuth itemCrear)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (itemCrear != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(request);


            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
