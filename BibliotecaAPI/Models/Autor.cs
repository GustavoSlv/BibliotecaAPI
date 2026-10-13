using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models
{
    public class Autor
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        public ICollection<Livro> Livros { get; set; } = new List<Livro>();
    }
}
