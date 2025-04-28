using Newtonsoft.Json;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Collections;
using System.Text;

namespace PeliculasWeb.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //Injección de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clientFactory;
        public Repositorio(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> ActualizarAsync(string url, T itemActualizar, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);

            if(itemActualizar != null) 
            { 
                peticion.Content = new StringContent(JsonConvert.SerializeObject(itemActualizar),
                    Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            // Asigna el token al encabezado de autorización
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ActualizarPeliculaAsync(string url, T peliculaActualizar, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);
            //Se instancia MultipartFormDataContent, que permite enviar datos en formato multipart/form-data,
            //comúnmente usado para cargar archivos junto con datos de formulario.
            var multipartContent = new MultipartFormDataContent();

            if (peliculaActualizar != null)
            {
                // Serializar cada propiedad de peliculaActualizar y añadirla al contenido
                // multipart/form-data
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(peliculaActualizar);
                    if (value != null)
                    {
                        if (property.PropertyType == typeof(IFormFile))
                        {
                            var file = value as IFormFile;
                            if (file != null)
                            {
                                var streamContent = new StreamContent(file.OpenReadStream());
                                streamContent.Headers.ContentType =
                                    new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                                multipartContent.Add(streamContent, property.Name, file.FileName);
                            }
                        }
                        else
                        {
                            var stringContent = new StringContent(value.ToString());
                            multipartContent.Add(stringContent, property.Name);
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            peticion.Content = multipartContent;
            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            // Asigna el token al encabezado de autorización
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> BorrarAsync(string url, int Id, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Delete, url + Id);

            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            // Asigna el token al encabezado de autorización
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable> Buscar(string url, string nombre)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + nombre);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CrearAsync(string url, T itemCrear, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Post, url);
            if (itemCrear != null)
            {
                peticion.Content = new StringContent(JsonConvert.SerializeObject(itemCrear),
                     Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            // Asigna el token al encabezado de autorización
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CrearPeliculaAsync(string url, T peliculaCrear, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Post, url);
            //Se instancia MultipartFormDataContent, que permite enviar datos en formato multipart/form-data,
            //comúnmente usado para cargar archivos junto con datos de formulario.
            var multipartContent = new MultipartFormDataContent();

            if (peliculaCrear != null)
            {
                // Serializar cada propiedad de peliculaActualizar y añadirla al contenido
                // multipart/form-data
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(peliculaCrear);
                    if (value != null)
                    {
                        if (property.PropertyType == typeof(IFormFile))
                        {
                            var file = value as IFormFile;
                            if (file != null)
                            {
                                var streamContent = new StreamContent(file.OpenReadStream());
                                streamContent.Headers.ContentType =
                                    new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                                multipartContent.Add(streamContent, property.Name, file.FileName);
                            }
                        }
                        else
                        {
                            var stringContent = new StringContent(value.ToString());
                            multipartContent.Add(stringContent, property.Name);
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            peticion.Content = multipartContent;
            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            // Asigna el token al encabezado de autorización
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<T> GetAsync(string url, int Id)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + Id);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable> GetPeliculasEnCategoriaAsync(string url, int categoriaid)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + categoriaid);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable> GetTodoAsync(string url)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable> GetTodoAsyncUser(string url, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url);

            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            // Asigna el token al encabezado de autorización
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }
    }
}
