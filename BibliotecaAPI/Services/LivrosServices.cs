using BibliotecaAPI.Context;
using BibliotecaAPI.Dtos.AutorDto;
using BibliotecaAPI.Dtos.LivroDto;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Services
{
    public class LivrosServices
    {
        private readonly AppDbContext _context;

        public LivrosServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RespostaGetLivrosDto> GetLivrosAsync(LivroQueryStringDto queryDto)
        {
            //Configuração da consulta

            var livros = _context.Livros.AsQueryable();






            // Filtros

            if (!string.IsNullOrEmpty(queryDto.titulo))
            {
                livros = livros.Where(x => x.Titulo.Contains(queryDto.titulo));
            }

            if (queryDto.AnoInicial.HasValue)
            {
                livros = livros.Where(x => x.AnoPublicacao >= queryDto.AnoInicial.Value);
            }

            if (queryDto.AnoFinal.HasValue)
            {
                livros = livros.Where(x => x.AnoPublicacao <= queryDto.AnoFinal.Value);
            }

            if (queryDto.AutorId.HasValue)
            {
                livros = livros.Where(x => x.AutorId == queryDto.AutorId.Value);
            }

            if (!string.IsNullOrEmpty(queryDto.AutorNome))
            {
                livros = livros.Where(x => x.Autor.Nome.Contains(queryDto.AutorNome));
            }


            //Configuração da resposta com informações de paginação
            var totalLivros = await livros.CountAsync();
            var paginaAtual = queryDto.PageNumber;
            var tamanhoPagina = queryDto.PageSize;
            var totalPaginas = (int)Math.Ceiling((double)totalLivros / queryDto.PageSize);




            //Ordenação
            if (queryDto.OrderBy == "titulo" && queryDto.Direcao == "asc")
            {
                livros = livros.OrderBy(x => x.Titulo);
            }

            else if (queryDto.OrderBy == "titulo" && queryDto.Direcao == "desc")
            {
                livros = livros.OrderByDescending(x => x.Titulo);
            }

            else if (queryDto.OrderBy == "ano" && queryDto.Direcao == "asc")
            {
                livros = livros.OrderBy(x => x.AnoPublicacao);
            }
            else if (queryDto.OrderBy == "ano" && queryDto.Direcao == "desc")
            {
                livros = livros.OrderByDescending(x => x.AnoPublicacao);
            }
            else if (queryDto.OrderBy == "autor" && queryDto.Direcao == "asc")
            {
                livros = livros.OrderBy(x => x.Autor.Nome);
            }
            else if (queryDto.OrderBy == "autor" && queryDto.Direcao == "desc")
            {
                livros = livros.OrderByDescending(x => x.Autor.Nome);
            }
            else
            {
                livros = livros.OrderBy(x => x.Id); // Ordenação padrão por Id
            }




            //Consulta os dados de acordo com a paginação, ordenação e filtros aplicados
            var livrosDto = await livros
                .Skip((queryDto.PageNumber - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .Select(x => new LivroReadDto
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    ISBN = x.ISBN,
                    AnoPublicacao = x.AnoPublicacao,
                    AutorId = x.AutorId,
                    Autor = new AutorReadDto { Id = x.Autor.Id, Nome = x.Autor.Nome }
                })
                .ToListAsync();







            RespostaGetLivrosDto respostaDto = new RespostaGetLivrosDto
            {
                TotalLivros = totalLivros,
                PaginaAtual = paginaAtual,
                TamanhoPagina = tamanhoPagina,
                TotalPaginas = totalPaginas,
                Livros = livrosDto
            };


            return respostaDto;
        }


        public async Task<LivroReadDto> GetLivroAsync(int id) {
            
            var livroPesquisado = await _context.Livros
                   .Include(l => l.Autor)
                   .FirstOrDefaultAsync(l => l.Id == id);

            if (livroPesquisado is null)
            {
                return null;
            }

            LivroReadDto livroDto = new LivroReadDto
            {
                Id = livroPesquisado.Id,
                Titulo = livroPesquisado.Titulo,
                ISBN = livroPesquisado.ISBN,
                AnoPublicacao = livroPesquisado.AnoPublicacao,
                AutorId = livroPesquisado.AutorId,
                Autor = new AutorReadDto { Id = livroPesquisado.Autor.Id, Nome = livroPesquisado.Autor.Nome }
            };

            return livroDto;
        }



        
        public async Task<LivroReadDto> PostAsync(LivroCreateDto dto)
        {

            var autor = await _context.Autores.FindAsync(dto.AutorId);

            if (autor is null)
            {
                return null;
            }



            Livro livro = new Livro
            {
                Titulo = dto.Titulo,
                ISBN = dto.ISBN,
                AnoPublicacao = dto.AnoPublicacao,
                AutorId = dto.AutorId
            };
            await _context.Livros.AddAsync(livro);
            await _context.SaveChangesAsync();




            LivroReadDto livroRead = new LivroReadDto
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                ISBN = livro.ISBN,
                AnoPublicacao = livro.AnoPublicacao,
                AutorId = livro.AutorId,
                Autor = new AutorReadDto { Id = autor.Id, Nome = autor.Nome }
            };

            return livroRead;

        }



        
        public async Task<LivroReadDto> PutAsync(int id, LivroUpdateDto dto)
        {
            var livroPesquisado = await _context.Livros.Include(l => l.Autor).FirstOrDefaultAsync(l => l.Id == id);
            var autor = await _context.Autores.FindAsync(dto.AutorId);

            if (livroPesquisado is null || autor is null)
            {
                return null;
            }

            livroPesquisado.Titulo = dto.Titulo;
            livroPesquisado.ISBN = dto.ISBN;
            livroPesquisado.AnoPublicacao = dto.AnoPublicacao;
            livroPesquisado.AutorId = dto.AutorId;

            await _context.SaveChangesAsync();

            LivroReadDto livroDto = new LivroReadDto
            {
                Id = livroPesquisado.Id,
                Titulo = livroPesquisado.Titulo,
                ISBN = livroPesquisado.ISBN,
                AnoPublicacao = livroPesquisado.AnoPublicacao,
                AutorId = livroPesquisado.AutorId,
                Autor = new AutorReadDto { Id = autor.Id, Nome = autor.Nome }
            };

            return livroDto;
        }






        public async Task<bool> DeleteAsync(int id)
        {
            var livroPesquisado = await _context.Livros.FindAsync(id);

            if (livroPesquisado is null)
            {
                return false;
            }

            _context.Livros.Remove(livroPesquisado);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
