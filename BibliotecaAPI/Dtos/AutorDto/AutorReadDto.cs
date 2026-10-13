using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Dtos.AutorDto
{
    public class AutorReadDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

    }
}
