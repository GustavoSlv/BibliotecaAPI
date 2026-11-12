# BibliotecaAPI

O objetivo deste projeto é praticar o desenvolvimento de APIs REST com C# e .NET 8. A BibliotecaAPI permite gerenciar livros e autores com CRUD, filtros, ordenação, paginação e autenticação JWT. É um projeto de estudo e portfólio.

## Funcionalidades

- CRUD de livros e autores, com relacionamento entre eles.
- Consultas com filtros, ordenação e paginação.
- Organização em Controllers, Services e DTOs.
- Cadastro e login com ASP.NET Core Identity e JWT.
- Roles `User` e `Admin`: consultas GET liberadas; POST, PUT e DELETE restritos a `Admin`.

## Dependências

- **.NET 8 SDK**
- **Visual Studio 2026**
- **SQL Server**
- **Bibliotecas e ferramentas utilizadas:** Entity Framework Core, `Microsoft.EntityFrameworkCore.SqlServer`, ASP.NET Core Identity (`Microsoft.AspNetCore.Identity.EntityFrameworkCore`), JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`), Swagger/Swashbuckle e EF Core Tools.

## Conexão com Banco de Dados e Autenticação JWT

1. Na pasta do projeto (onde está `BibliotecaAPI.csproj`), crie o arquivo **`appsettings.Development.json`**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=\"\";Database=BibliotecaAPI;User Id=\"\";Password=\"\";TrustServerCertificate=True;"
  },
  "Jwt": {
    "SecretKey": "",
    "Issuer": "BibliotecaAPI",
    "Audience": "BibliotecaAPI",
    "ExpirationMinutes": 60
  }
}
```

Preencha o **servidor, usuário e senha** da conexão SQL Server nos campos `""`. 

2. **Crie a chave JWT**: coloque temporariamente este código no `Program.cs`, após a criação do `builder` (com o `using` no início do arquivo):

```csharp
using System.Security.Cryptography;

var chave = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
Console.WriteLine(chave);
```

Execute o projeto, copie a chave gerada e coloque-a em `Jwt:SecretKey` no `appsettings.Development.json`. **Depois remova esse código temporário.**

3. No **`Program.cs`**, mantenha a configuração de `AddAuthentication().AddJwtBearer(...)`, que lê a chave e valida assinatura, emissor, público e expiração. Use `app.UseAuthentication()` antes de `app.UseAuthorization()`.

4. Nos estudos, também foi utilizado o comando abaixo, executado no terminal na pasta do `.csproj`:

```bash
dotnet user-jwts create
```

Ele cria **um token de teste para desenvolvimento**, mas **não substitui** a chave `Jwt:SecretKey` usada no login da BibliotecaAPI.

> **Segurança:** mantenha `appsettings.Development.json` ignorado e não rastreado pelo Git, pois contém credenciais. Não publique sua chave ou senha.

## Migrations

No Visual Studio, abra **Ferramentas → Gerenciador de Pacotes NuGet → Console do Gerenciador de Pacotes** e selecione o projeto `BibliotecaAPI`.

**Criar uma nova migration** (quando houver mudanças nos modelos ou contexto):

```powershell
Add-Migration NomeDaMigration
```

**Criar/atualizar as tabelas no banco:**

```powershell
Update-Database
```


## Teste

Execute o projeto no Visual Studio e abra o **Swagger** (`/swagger`) para testar as rotas.

- `GET /api/Livros` e `GET /api/Autores`: consultas liberadas.
- `POST /api/Auth/register`: cadastro de usuário (recebe role `User`).
- `POST /api/Auth/login`: retorna o JWT e sua expiração.
- Clique em **Authorize** no Swagger, informe o token e teste POST, PUT e DELETE de livros e autores.
- Um usuário `User` recebe **403 Forbidden** nessas operações; um `Admin` autorizado pode executá-las. Sem token válido, o retorno é **401 Unauthorized**.

As roles são criadas pelo `RoleSeeder`. A atribuição de `Admin` deve ser feita de maneira controlada, não pelo cadastro público.
