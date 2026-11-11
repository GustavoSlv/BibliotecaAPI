using BibliotecaAPI.Dtos.AuthDto;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BibliotecaAPI.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        

        //Método responsável por cadastrar o usuário e associa-lo somente a permissão de usuário.
        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return result;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return roleResult;
            }

            return result;
        }


        // Validar login e gerar JWT
        public async Task<LoginResponseDto?> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var senhaValida = await _userManager.CheckPasswordAsync(user, password);

            if (!senhaValida)
            {
                return null;
            }

            return await GerarTokenAsync(user);
        }

        // Gerar token JWT
        private async Task<LoginResponseDto> GerarTokenAsync(ApplicationUser user)
        {
            var secretKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Chave JWT não configurada.");

            var key = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                             {
                                 new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                                 new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                             };

            // Buscar as roles do usuário no Identity
            var roles = await _userManager.GetRolesAsync(user);

            // Adicionar as roles ao token
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt
            };
        }
    }
}
