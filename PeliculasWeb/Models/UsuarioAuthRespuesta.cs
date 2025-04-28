namespace PeliculasWeb.Models
{
    public class UsuarioAuthRespuesta
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public ResultData Result { get; set; }
    }
}
