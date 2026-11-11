using BibliotecaAPI.Dtos.AuthDto;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous] //Permite executar sem autenticação
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(
                dto.Email, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Usuário cadastrado com sucesso!");
        }

        [HttpPost("login")]
        [AllowAnonymous] //Permite executar sem autenticação
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(
                dto.Email, dto.Password);

            if (token == null)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            return Ok(new { Token = token });
        }
    }
}
