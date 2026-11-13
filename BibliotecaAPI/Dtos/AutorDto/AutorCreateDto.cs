using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Dtos.AutorDto
{
    public class AutorCreateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, MinimumLength = 2)]
        public string Nome { get; set; } = string.Empty;
    }
}
