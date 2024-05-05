# Squadra Challenge

## Summary

- [Descrição](#descrição)
- [Requisitos](#requisitos)
- [Tecnologias](#tecnologias)
- [Run the project locally](#run-the-project-locally)
- [Run the tests](#run-the-tests)

## Descrição

Este projeto é um desafio para a empresa Squadra. O projeto consiste em dois componentes, um backend (C#) e um frontend (angular).

O backend é uma API REST que expõe dois endpoints:
- Obter a previsão do tempo de 5 dias por cidade
- Obter auditoria da previsão do tempo por cidade

O frontend é uma aplicação web que expõe apenas uma página que permite ao usuário pesquisar a previsão do tempo informando o nome de uma cidade. Serão exibidos os dados da previsão do tempo para os próximos 5 dias.

## Requisitos Técnicos

- [x] Linguagem de programação C#- .NET e Angular.
- [x] Virtualização de container utilizando Docker.
- [x] Elaborar os endpoints utilizando padrão REST.
- [x] Poderá utilizar seu banco de dados a seu gosto, mas dê preferência para bancos relacionais, como SQL Server, Oracle.
- [x] Utilizar quaisquer bibliotecas e frameworks a seu gosto (Entity Framework, XUnit etc.), porém a chamada da API deve ser feita diretamente.
- [x] Elaborar um README com, no mínimo, as instruções referentes a subir o ambiente da sua aplicação;
- [x] Envie para o repositório quaisquer artefatos que tenha utilizado para implementar essa aplicação (collections do postman, testes unitários etc.);

## Tecnologias

* Visual studio Version 17.9.6
* Visual studio Code 1.89.0
* [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
* [Entity Framework Core 8](https://docs.microsoft.com/en-us/ef/core/)
* [Angular 17.3.6](https://angular.io/)
* [NodeJS v20.12.2](https://angular.io/)
* [Mapster](https://github.com/MapsterMapper/Mapster)
* [XUnit](https://nunit.org/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq)
* Swagger
* [Docker](https://www.docker.com/), Docker Compose

### Run The Project Locally

1. Clone the project:

```bash
$ git clone https://github.com/stefanini-applications/basic-api-csharp-template.git
```
2. Access the project directory:

```bash
$ cd basic-api-csharp-template
```

3. Adjust the environment variables to the `appsettings.json` file (for example)

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Basic": "Debug",
        "Microsoft.AspNetCore": "Warning",
        "System": "Warning"
      }
    }
  },
  "AllowedHosts": "*",
  "TmdbAdapterConfiguration": {
    "TmdbApiUrlBase": "https://api.themoviedb.org/3",
    "TmdbApiKey": "<API>",
    "TimeCacheInSeconds": 30,
    "Language": "pt-BR"
  },
  "HealthCheckPublisherOptions": {
    "Delay": "00:00:05",
    "Period": "00:00:50"
  }
}
```

4. Run the project:

```bash
$ dotnet run --project src/Basic.WebApi/Basic.WebApi.csproj
```

5. You can access the API documentation through the link: [Swagger](http://localhost:5000/swagger-ui.html)

## Run the tests

1. Run all unit tests:

```bash
$ dotnet test
```
