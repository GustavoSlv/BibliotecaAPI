using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services
{
    public static class RoleSeeder
    {
        public static async Task CriarRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var resultado = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!resultado.Succeeded)
                    {
                        throw new InvalidOperationException($"Erro ao criar a role {role}.");
                    }
                }
            }
        }


        public static async Task CriarAdminAsync(IServiceProvider services,string email)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var usuario = await userManager.FindByEmailAsync(email);

            if (usuario is null)
            {
                throw new InvalidOperationException("Usuário administrador não encontrado.");
            }

            if (!await userManager.IsInRoleAsync(usuario, "Admin"))
            {
                var resultado = await userManager.AddToRoleAsync(usuario, "Admin");

                if (!resultado.Succeeded)
                {
                    throw new InvalidOperationException("Não foi possível atribuir a role Admin.");
                }
            }
        }
    }
}
