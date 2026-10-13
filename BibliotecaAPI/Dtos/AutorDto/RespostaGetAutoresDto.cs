namespace BibliotecaAPI.Dtos.AutorDto
{
    public class RespostaGetAutoresDto
    {
        public int TotalAutores { get; set; }

        public int PaginaAtual { get; set; }

        public int TamanhoPagina { get; set; }

        public int TotalPaginas { get; set; }

        public List<AutorReadDto> Autores { get; set; }
    }
}
