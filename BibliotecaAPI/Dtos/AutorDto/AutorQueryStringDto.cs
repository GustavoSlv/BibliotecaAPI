using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Dtos.AutorDto
{
    public class AutorQueryStringDto
    {
        public string? nome { get; set; }


        [RegularExpression("^(asc|desc)$", ErrorMessage = "Insira 'asc' para crescente ou 'desc' para decrescente.")]
        public string? Direcao { get; set; } = "asc";

        [Range(1, int.MaxValue, ErrorMessage = "O pageNumber deve ser um valor positivo.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 20, ErrorMessage = "O pageSize deve ser um valor positivo.")]
        public int PageSize { get; set; } = 10;
    }
}
