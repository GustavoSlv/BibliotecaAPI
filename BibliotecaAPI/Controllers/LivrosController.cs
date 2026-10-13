using BibliotecaAPI.Context;
using BibliotecaAPI.Dtos.AutorDto;
using BibliotecaAPI.Dtos.LivroDto;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
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
        public async Task<ActionResult<LivroReadDto>> PostAsync(LivroCreateDto dto)
        {

            var livro = await _livroService.PostAsync(dto);

            if (livro is null)
            {
                return NotFound();
            }




            return CreatedAtRoute("ObterLivroPorId",  new { id = livro.Id }, livro);
        }





        [HttpPut("{id}")]
        public async Task<ActionResult<LivroReadDto>> PutAsync(int id, LivroUpdateDto dto)
        {
            var livroPesquisado = await _livroService.PutAsync(id, dto);

            if (livroPesquisado is null)
            {
                return NotFound();
            }


            return Ok(livroPesquisado);
        }





        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var livroPesquisado = await _livroService.DeleteAsync(id);

            if (!livroPesquisado)
            {
                return NotFound("Livro não encontrado.");
            }
            return NoContent();
        }
    }
}
