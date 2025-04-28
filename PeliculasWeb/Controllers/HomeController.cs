using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Diagnostics;
using System.Security.Claims;
using PeliculasWeb.Utilidades;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using PeliculasWeb.Models.ViewModels;

namespace PeliculasWeb.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepositorio _accRepo;
        private readonly ICategoriaRepositorio _repoCategoria;
        private readonly IPeliculaRepositorio _repoPelicula;

        public HomeController(IAccountRepositorio accRepo, ICategoriaRepositorio repoCategoria, IPeliculaRepositorio repoPelicula/*ILogger<HomeController> logger*/)
        {
            //_logger = logger;
            _accRepo = accRepo;
            _repoCategoria = repoCategoria;
            _repoPelicula = repoPelicula;
        }

        //public async Task<IActionResult> Index()
        //{
        //    IndexVM listaPeliculasCategorias = new IndexVM()
        //    {
        //        ListaCategorias = (IEnumerable<Categoria>) await _repoCategoria.GetTodoAsync(Constantes.RutaCategoriasApi),
        //        ListaPeliculas = (IEnumerable<Pelicula>) await _repoPelicula.GetPeliculasTodoAsync(Constantes.RutaPeliculasApi)
        //    };

        //    return View(listaPeliculasCategorias);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 5; // O el tama�o de p�gina que prefieras
            var url = $"{Constantes.RutaPeliculasApi}?pageNumber={page}&pageSize={pageSize}";

            var peliculaResponse = await _repoPelicula.GetPeliculasTodoAsync(url);

            IndexVM listaPeliculasCategorias = new IndexVM()
            {
                ListaCategorias = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(Constantes.RutaCategoriasApi),
                ListaPeliculas = peliculaResponse.Items,
                TotalPages = peliculaResponse.TotalPages,
                CurrentPage = page,
            };

            return View(listaPeliculasCategorias);
        }

        [HttpGet]
        public async Task<IActionResult> IndexCategoria(int id)
        {
            var pelisEnCategoria = await _repoPelicula.GetPeliculasEnCategoriaAsync(Constantes.RutaPeliculasEnCategoriaApi, id);

            return View(pelisEnCategoria);
        }

        [HttpPost]
        public async Task<IActionResult> IndexBusqueda(string nombre)
        {
            var pelisEncontradas = await _repoPelicula.Buscar(Constantes.RutaPeliculasBusquedaApi, nombre);

            return View(pelisEncontradas);
        }

        [HttpGet]
        public IActionResult Login()
        {
            UsuarioAuth usuario = new UsuarioAuth();
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioAuth obj)
        {
            if (ModelState.IsValid)
            {
                UsuarioAuth objUser = await _accRepo.LoginAsync(Constantes.RutaUsuariosApi + "Login", obj);
                if (objUser.Token == null)
                {
                    TempData["alert"] = "Los datos son incorrectos";
                    return View();
                }

                //Creaci�n de la identidad y principal
                //Si el login fue exitoso, se crea una identidad de usuario(ClaimsIdentity), la cual es responsable de contener las "reclamaciones" del usuario(informaci�n relevante sobre el mismo).
                //Aqu� se agrega una reclamaci�n de tipo Email con el valor del nombre de usuario del objeto objUser. Esto se utiliza para almacenar informaci�n sobre el usuario en la autenticaci�n.
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Email, objUser.NombreUsuario));

                //Con la identidad creada, se genera un principal (un objeto que contiene la identidad del usuario y sus reclamaciones) que se utilizar� para realizar la autenticaci�n en la aplicaci�n.
                var principal = new ClaimsPrincipal(identity);
                //Se realiza el inicio de sesi�n del usuario utilizando el esquema de autenticaci�n de cookies (CookieAuthenticationDefaults.AuthenticationScheme),
                //lo cual almacena la informaci�n de la sesi�n del usuario en una cookie. Esta cookie se enviar� al navegador para mantener al
                //usuario autenticado durante su sesi�n.
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                //El token JWT (JWToken), que generalmente se usa para autenticaci�n en futuras peticiones.
                //El nombre de usuario(Usuario), que se puede necesitar en la sesi�n para mostrar informaci�n personalizada en la UI
                HttpContext.Session.SetString("JWToken", objUser.Token);
                HttpContext.Session.SetString("Usuario", objUser.NombreUsuario);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UsuarioAuth obj)
        {
            bool result = await _accRepo.RegisterAsync(Constantes.RutaUsuariosApi + "Registro", obj);
            if (result == false)
            {
                return View();
            }

            TempData["alert"] = "Registro Correcto";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Cierra la sesi�n de autenticaci�n
            await HttpContext.SignOutAsync();

            // Limpia la sesi�n de usuario, incluyendo cualquier token
            HttpContext.Session.Clear();

            // Elimina la cookie de sesi�n manualmente
            if (Request.Cookies.ContainsKey(".AspNetCore.Session"))
            {
                Response.Cookies.Delete(".AspNetCore.Session");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
