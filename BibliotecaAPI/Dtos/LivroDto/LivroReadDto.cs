using BibliotecaAPI.Dtos.AutorDto;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Dtos.LivroDto
{
    public class LivroReadDto
    {

        public int Id { get; set; }

        public string Titulo { get; set; }

        public string ISBN { get; set; }

        public int AnoPublicacao { get; set; }

        public int AutorId { get; set; }

        public AutorReadDto Autor { get; set; }
    }
}
