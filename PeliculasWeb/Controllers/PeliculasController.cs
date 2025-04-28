using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWeb.Models;
using PeliculasWeb.Models.ViewModels;
using PeliculasWeb.Repositorio.IRepositorio;
using PeliculasWeb.Utilidades;

namespace PeliculasWeb.Controllers
{
    [Authorize]
    public class PeliculasController : Controller
    {
        private readonly IPeliculaRepositorio _repoPelicula;
        private readonly ICategoriaRepositorio _repoCategoria;

        public PeliculasController(IPeliculaRepositorio repoPelicula, ICategoriaRepositorio repoCategoria)
        {
            _repoPelicula = repoPelicula;
            _repoCategoria = repoCategoria;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Pelicula() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasPeliculas()
        {
            var peliculas = await _repoPelicula.GetPeliculasTodoAsyncEnt(Constantes.RutaPeliculasApi);

            // Ahora aquí puedes poner un breakpoint para ver qué contiene 'peliculas'

            return Json(new { data = peliculas });

            //return Json(new { data = await _repoPelicula.GetPeliculasTodoAsync(Constantes.RutaPeliculasApi) });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(Constantes.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula)
        {
            //esto es por si se requiere nuevamente la data -------------------------
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(Constantes.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };
            //-------------------------------------------------------------

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    pelicula.Imagen = files[0];// Asignar el IFormFile directamente a la propiedad Imagen
                }
                else
                {
                    return View(objVM);
                }

                await _repoPelicula.CrearPeliculaAsync(Constantes.RutaPeliculasApi, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View(objVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(Constantes.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            if (id == null)
            {
                return NotFound();
            }

            //Para mostrar los datos en el formulario Edit
            objVM.Pelicula = await _repoPelicula.GetAsync(Constantes.RutaPeliculasApi, id.GetValueOrDefault());
            if (objVM.Pelicula == null)
            {
                return NotFound();
            }
            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Pelicula pelicula)
        {
            IEnumerable<Categoria> ctList = (IEnumerable<Categoria>)await
                _repoCategoria.GetTodoAsync(Constantes.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = ctList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    pelicula.Imagen = files[0];// Asignar el IFormFile directamente a la propiedad Imagen
                }
                else
                {
                    return View(objVM);
                }

                await _repoPelicula.ActualizarPeliculaAsync(Constantes.RutaPeliculasApi + pelicula.Id, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View(objVM);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _repoPelicula.BorrarAsync(Constantes.RutaPeliculasApi, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {
                return Json(new { success = true, message = "Borrado correctamente" });
            }

            return Json(new { success = false, message = "No se pudo borrar" });
        }

        [HttpGet]
        public async Task<IActionResult> GetPeliculasEnCategoria(int id)
        {
            return Json(new { data = await _repoPelicula.GetPeliculasEnCategoriaAsync(Constantes.RutaPeliculasEnCategoriaApi, id) });
        }
    }
}
