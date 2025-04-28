using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class UsuarioAuth
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El password es obligatorio")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "El password debe estar entre 4 y 20 caracteres")]
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
