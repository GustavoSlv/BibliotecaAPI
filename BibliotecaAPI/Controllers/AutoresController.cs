using BibliotecaAPI.Context;
using BibliotecaAPI.Dtos.AutorDto;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    //crie a rota da api para o controller de autores

    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {

        private readonly AutoresServices _autoresServices;

        public AutoresController( AutoresServices autoresServices)
        {
            _autoresServices = autoresServices;
        }

        [HttpGet]
        public async Task<ActionResult<RespostaGetAutoresDto>> GetAutoresAsync([FromQuery] AutorQueryStringDto queryDto)
        {
            return await _autoresServices.GetAutoresAsync(queryDto);
        }





        [HttpGet("{id}", Name ="ObterAutorPorID")]
        public async Task<ActionResult<AutorReadDto>> GetAutorAsync(int id)
        {
            var autorDto = await _autoresServices.GetAutorAsync(id);

            if(autorDto is null)
            {
                return NotFound();
            }

            return Ok(autorDto);
        }





        [HttpPost]
        public async Task<ActionResult<AutorReadDto>> PostAutorAsync(AutorCreateDto dto)
        {
            var autorDto = await _autoresServices.PostAutorAsync(dto);

            return CreatedAtRoute(
                "ObterAutorPorId",
                new { id = autorDto.Id },
                autorDto);
        }


        [HttpPut("{id}")]

        public async Task<ActionResult<AutorReadDto>> PutAutorAsync(int id, AutorUpdateDto dto)
        {
            var autor = await _autoresServices.PutAutorAsync(id, dto);

            if(autor is null)
            {
                return NotFound();
            }

            return Ok(autor);
        }





        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>> DeleteAutorAsync(int id)

        {
            var autor = await _autoresServices.DeleteAutorAsync(id);
            if (autor == false)
            {
                return NotFound();
            }
            return NoContent();

        }
    }
}
