using BibliotecaAPI.Context;
using BibliotecaAPI.Dtos.AutorDto;
using BibliotecaAPI.Dtos.LivroDto;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LivrosServices _livroService;

        public LivrosController(LivrosServices livroService)
        {
            _livroService = livroService;
        }





        [HttpGet]
        public async Task<ActionResult<RespostaGetLivrosDto>> GetLivrosAsync([FromQuery] LivroQueryStringDto queryDto)
        {

            var respostaDto = await _livroService.GetLivrosAsync(queryDto);

            return Ok(respostaDto);
        }





        [HttpGet("{id}", Name = "ObterLivroPorId")]
        public async Task<ActionResult<LivroReadDto>> GetLivroAsync(int id)
        {

            var livroPesquisado = await _livroService.GetLivroAsync(id);
            
            if (livroPesquisado is null)
            {
                return NotFound("Livro não encontrado.");
            }

            

            return Ok(livroPesquisado);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LivroReadDto>> PostAsync(LivroCreateDto dto)
        {

            var livro = await _livroService.PostAsync(dto);

            if (livro is null)
            {
                return BadRequest("Autor informado não existe");
            }




            return CreatedAtRoute("ObterLivroPorId",  new { id = livro.Id }, livro);
        }





        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LivroReadDto>> PutAsync(int id, LivroUpdateDto dto)
        {

            //Esse método retorna uma tupla contendo o livro atualizado e uma mensagem de erro, caso haja algum problema.
            //A msgem de erro será nula se o livro for atualizado com sucesso.
            var (livro, errorMessage) = await _livroService.PutAsync(id, dto);

            if (errorMessage == "Livro não encontrado.")
            {
                return NotFound(errorMessage);
            }

            if (errorMessage == "Autor não encontrado.")
            {
                return BadRequest(errorMessage);
            }

            //Se não houver mensagem de erro, significa que o livro foi atualizado com sucesso, então retornamos Ok com o livro atualizado.
            return Ok(livro);
        }





        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var livroPesquisado = await _livroService.DeleteAsync(id);

            if (!livroPesquisado)
            {
                return NotFound("Livro não encontrado.");
            }
            return NoContent();
        }


        [HttpDelete("autor/{idAutor}", Name = "DeletarLivrosPorAutor")]
        [Authorize(Roles = "Admin")]
        
        public async Task<ActionResult> DeleteLivrosByAutorAsync(int idAutor)
        {
            var sucesso = await _livroService.DeletaLivrosByAutor(idAutor);

            if (!sucesso)
            {
                return NotFound("Nenhum livro encontrado para o autor especificado.");
            }

            return NoContent();
        }
    }
}
