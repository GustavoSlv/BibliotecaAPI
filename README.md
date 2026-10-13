# BibliotecaAPI

## Sobre o projeto

A BibliotecaAPI é uma API REST desenvolvida em C# com ASP.NET Core para gerenciamento de livros e autores. A aplicação permite cadastrar, consultar, atualizar e excluir registros, além de realizar consultas com filtros, ordenação e paginação.

O projeto utiliza uma estrutura organizada em controllers, serviços, modelos e DTOs, buscando separar as responsabilidades e facilitar a manutenção do código. A integração com o SQL Server é realizada por meio do Entity Framework Core, responsável pelo acesso e gerenciamento dos dados.

A API também contempla o relacionamento entre livros e autores, validação de dados e tratamento de situações como registros não encontrados, seguindo práticas comuns no desenvolvimento de aplicações backend.

## Tecnologias utilizadas

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server
- Entity Framework Core Tools

## Configuração

Crie o arquivo `appsettings.Development.json` na raiz do projeto e configure a string de conexão:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=BibliotecaAPI;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;"
  }
}
```

Substitua os valores pelos dados da sua instância do SQL Server.

## Migrations

Criar uma migration:

```bash
dotnet ef migrations add NomeDaMigration
```

Atualizar o banco de dados:

```bash
dotnet ef database update
```

## Objetivo

Projeto de estudo e portfólio voltado à aplicação prática de conceitos de desenvolvimento backend com C# e .NET.