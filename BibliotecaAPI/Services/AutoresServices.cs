using BibliotecaAPI.Context;
using BibliotecaAPI.Dtos.AutorDto;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Services
{
    public class AutoresServices
    {
        private readonly AppDbContext _context;

        public AutoresServices(AppDbContext context)
        {
            _context = context;
        }


        public async Task<RespostaGetAutoresDto> GetAutoresAsync( AutorQueryStringDto queryDto)
        {
            var autores = _context.Autores.AsQueryable();


            //Filtro
            if (!string.IsNullOrEmpty(queryDto.nome))
            {
                autores = autores.Where(a => a.Nome.Contains(queryDto.nome));
            }


            //Ordenação
            if(queryDto.Direcao == "desc")
            {
                autores = autores.OrderByDescending(a => a.Nome);
            }
            else
            {
                autores = autores.OrderBy(a => a.Nome);
            }

            var listaAutorDto = await autores
                .Skip((queryDto.PageNumber - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .Select(a => new AutorReadDto
                {
                    Id = a.Id,
                    Nome = a.Nome
                }).ToListAsync();


            //Configuração da resposta com informações de paginação
            var totalAutores = await autores.CountAsync();
            var paginaAtual = queryDto.PageNumber;
            var tamanhoPagina = queryDto.PageSize;
            var totalPaginas = (int)Math.Ceiling((double)totalAutores / queryDto.PageSize);

            RespostaGetAutoresDto respota = new RespostaGetAutoresDto
            {
                TotalAutores = totalAutores,
                PaginaAtual = paginaAtual,
                TamanhoPagina = tamanhoPagina,
                TotalPaginas = totalPaginas,
                Autores = listaAutorDto
            };

            return respota;

        }




        public async Task<AutorReadDto> GetAutorAsync(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor is null)
            {
                return null;
            }

            AutorReadDto autorDto = new AutorReadDto { Id = autor.Id, Nome = autor.Nome };

            return autorDto;
        }


        public async Task<AutorReadDto> PostAutorAsync(AutorCreateDto dto)
        {
            Autor autor = new Autor { Nome = dto.Nome };

            await _context.Autores.AddAsync(autor);
            await _context.SaveChangesAsync();


            AutorReadDto autorDto = new AutorReadDto { Id = autor.Id, Nome = autor.Nome };

            return autorDto;

        }



        public async Task<AutorReadDto> PutAutorAsync(int id, AutorUpdateDto dto)
        {
            var autorExistente = await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);

            if (autorExistente is null)
            {
                return null;
            }

            autorExistente.Nome = dto.Nome;

            await _context.SaveChangesAsync();

            AutorReadDto autorDto = new AutorReadDto { Id = autorExistente.Id, Nome = autorExistente.Nome };


            return autorDto;
        }




        public async Task<bool> DeleteAutorAsync(int id)

        {
            var autorExistente = await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);


            if (autorExistente is null)
            {
                return false;
            }

            _context.Remove(autorExistente);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
