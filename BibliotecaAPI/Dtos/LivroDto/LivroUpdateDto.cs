using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Dtos.LivroDto
{
    public class LivroUpdateDto
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(150, MinimumLength = 2)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ISBN é obrigatório.")]
        [StringLength(13, MinimumLength = 10)]
        public string ISBN { get; set; } = string.Empty;

        [Range(1, 9999, ErrorMessage = "Informe um ano válido.")]
        public int AnoPublicacao { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Informe um autor válido.")]
        public int AutorId { get; set; }
    }
}
