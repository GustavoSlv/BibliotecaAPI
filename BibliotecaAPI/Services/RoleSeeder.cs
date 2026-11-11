using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services
{
    public static class RoleSeeder
    {
        public static async Task CriarRolesAsync(
            IServiceProvider services)
        {
            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var resultado = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!resultado.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Erro ao criar a role {role}.");
                    }
                }
            }
        }
    }
}
