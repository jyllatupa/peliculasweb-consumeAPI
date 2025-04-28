using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepositorio _repoUsuario;

        //contructor
        public UsuariosController(IUsuarioRepositorio repoUsuario)
        {
            _repoUsuario = repoUsuario;
        }

        public IActionResult Index()
        {
            return View(new Usuario());
        }

        [HttpGet]
        public async Task<IActionResult> GetTodosUsuarios()
        {
            var usuarios = await _repoUsuario.GetTodoAsyncUser(Constantes.RutaUsuariosApi, HttpContext.Session.GetString("JWToken"));

            return Json(new { data = usuarios });
        }
    }
}
