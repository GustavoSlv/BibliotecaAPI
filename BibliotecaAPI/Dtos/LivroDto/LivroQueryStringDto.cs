
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Dtos.LivroDto
{
    public class LivroQueryStringDto : IValidatableObject
    {
        public string? titulo { get; set; }


        [Range(1900, 2100, ErrorMessage = "O ano inicial deve ser um valor válido.")]
        public int? AnoInicial { get; set; }

        [Range(1900, 2100, ErrorMessage = "O ano final deve ser um valor válido.")]
        public int? AnoFinal { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O autorId deve ser um valor positivo.")]
        public int? AutorId { get; set; }
        public string? AutorNome { get; set; }

        //o order by precisa ser considerado apenas para campos Titulo, Ano e AutorNome

        [RegularExpression("^(titulo|autor|ano)$", ErrorMessage = "O orderBy deve ser 'titulo', 'autor' ou 'ano'.")]
        public string? OrderBy { get; set; }


        [RegularExpression("^(asc|desc)$",ErrorMessage = "Insira 'asc' para crescente ou 'desc' para decrescente.")]
        public string? Direcao { get; set; } = "asc";


        [Range(1, int.MaxValue, ErrorMessage = "O pageNumber deve ser um valor positivo.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 20, ErrorMessage = "O pageSize deve ser um valor positivo.")]
        public int PageSize { get; set; } = 10;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AnoInicial.HasValue &&
                AnoFinal.HasValue &&
                AnoInicial > AnoFinal)
            {
                yield return new ValidationResult(
                    "O ano inicial não pode ser maior que o ano final.",
                    new[] { nameof(AnoInicial), nameof(AnoFinal) });
            }
        }
    }
}
