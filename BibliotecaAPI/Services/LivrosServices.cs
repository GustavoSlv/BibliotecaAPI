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

        public async Task<RespostaGetLivrosDto> GetLivrosAsync(LivroQueryStringDto queryStringDto)
        {
            //Configuração da consulta

            var livros = _context.Livros.AsQueryable();






            // Filtros

            if (!string.IsNullOrEmpty(queryStringDto.titulo))
            {
                livros = livros.Where(x => x.Titulo.Contains(queryStringDto.titulo));
            }

            if (queryStringDto.AnoInicial.HasValue)
            {
                livros = livros.Where(x => x.AnoPublicacao >= queryStringDto.AnoInicial.Value);
            }

            if (queryStringDto.AnoFinal.HasValue)
            {
                livros = livros.Where(x => x.AnoPublicacao <= queryStringDto.AnoFinal.Value);
            }

            if (queryStringDto.AutorId.HasValue)
            {
                livros = livros.Where(x => x.AutorId == queryStringDto.AutorId.Value);
            }

            if (!string.IsNullOrEmpty(queryStringDto.AutorNome))
            {
                livros = livros.Where(x => x.Autor.Nome.Contains(queryStringDto.AutorNome));
            }


            //Configuração da resposta com informações de paginação
            var totalLivros = await livros.CountAsync();
            var paginaAtual = queryStringDto.PageNumber;
            var tamanhoPagina = queryStringDto.PageSize;
            var totalPaginas = (int)Math.Ceiling((double)totalLivros / queryStringDto.PageSize);




            //Ordenação
            if (queryStringDto.OrderBy == "titulo" && queryStringDto.Direcao == "asc")
            {
                livros = livros.OrderBy(x => x.Titulo);
            }

            else if (queryStringDto.OrderBy == "titulo" && queryStringDto.Direcao == "desc")
            {
                livros = livros.OrderByDescending(x => x.Titulo);
            }

            else if (queryStringDto.OrderBy == "ano" && queryStringDto.Direcao == "asc")
            {
                livros = livros.OrderBy(x => x.AnoPublicacao);
            }
            else if (queryStringDto.OrderBy == "ano" && queryStringDto.Direcao == "desc")
            {
                livros = livros.OrderByDescending(x => x.AnoPublicacao);
            }
            else if (queryStringDto.OrderBy == "autor" && queryStringDto.Direcao == "asc")
            {
                livros = livros.OrderBy(x => x.Autor.Nome);
            }
            else if (queryStringDto.OrderBy == "autor" && queryStringDto.Direcao == "desc")
            {
                livros = livros.OrderByDescending(x => x.Autor.Nome);
            }
            else
            {
                livros = livros.OrderBy(x => x.Id); // Ordenação padrão por Id
            }




            //Consulta os dados de acordo com a paginação, ordenação e filtros aplicados
            var livrosDto = await livros
                .Skip((queryStringDto.PageNumber - 1) * queryStringDto.PageSize)
                .Take(queryStringDto.PageSize)
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

            var autor = await GetAutorByIdAsync(dto.AutorId);

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


        //Esse método retorna uma tupla contendo o LivroReadDto atualizado e uma mensagem de erro, caso ocorra algum problema.
        //Se o livro for atualizado com sucesso, a tupla conterá o LivroReadDto e a mensagem será nula.
        public async Task<(LivroReadDto?, string?)> PutAsync(int id, LivroUpdateDto dto)
        {
            var livroPesquisado = await _context.Livros.Include(l => l.Autor).FirstOrDefaultAsync(l => l.Id == id);
            var autor = await GetAutorByIdAsync(dto.AutorId);

            if (livroPesquisado is null)
            {
                return (null, "Livro não encontrado.");
            }

            if (autor is null)
            {
                return (null, "Autor não encontrado.");
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

            return (livroDto, null);
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


        public async Task<AutorReadDto> GetAutorByIdAsync(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor is null)
            {
                return null;
            }

            return new AutorReadDto { Id = autor.Id, Nome = autor.Nome };
        }


        public async Task<bool> DeletaLivrosByAutor(int idAutor) 
        {
            var livros = await _context.Livros.Where(l => l.AutorId == idAutor).ToListAsync();
            if (livros.Count == 0)
            {
                return false;
            }
            _context.Livros.RemoveRange(livros);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
