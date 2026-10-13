namespace BibliotecaAPI.Dtos.LivroDto
{
    public class RespostaGetLivrosDto
    {
        public int TotalLivros { get; set; }

        public int PaginaAtual { get; set; }

        public int TamanhoPagina { get; set; }

        public int TotalPaginas { get; set; }

        public List<LivroReadDto> Livros { get; set; }

    }
}
