using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models
{
    public class Livro
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }

        [MaxLength(20)]
        public string ISBN { get; set; }

        public int AnoPublicacao { get; set; }

        public int AutorId { get; set; }

        public Autor Autor { get; set; }
    }
}
