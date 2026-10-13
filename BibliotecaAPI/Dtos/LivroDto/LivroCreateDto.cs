namespace BibliotecaAPI.Dtos.LivroDto
{
    public class LivroCreateDto
    {

        public string Titulo { get; set; }

        public string ISBN { get; set; }

        public int AnoPublicacao { get; set; }

        public int AutorId { get; set; }
    }
}
