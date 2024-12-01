# FIAP Tech Challenge

## Descrição

Projeto de API do Tech Challenge da FIAP

## Requisitos do Sistema

Para execução do projeto é necessário a instalação do Docker Desktop

- Docker Desktop para [Windows](https://desktop.docker.com/win/main/amd64/Docker%20Desktop%20Installer.exe?utm_source=docker&utm_medium=webreferral&utm_campaign=dd-smartbutton&utm_location=module&_gl=1*tfs79*_ga*MTcxOTEyMTU0OS4xNjk4Njk3MTUw*_ga_XJWPQMJYHQ*MTY5ODY5NzE0OS4xLjEuMTY5ODY5NzE1NC41NS4wLjA.)

- Docker Desktop para [Mac](https://desktop.docker.com/mac/main/arm64/Docker.dmg?utm_source=docker&utm_medium=webreferral&utm_campaign=dd-smartbutton&utm_location=module&_gl=1*pcopo9*_ga*MTcxOTEyMTU0OS4xNjk4Njk3MTUw*_ga_XJWPQMJYHQ*MTY5ODY5NzE0OS4xLjEuMTY5ODY5NzE1NC41NS4wLjA.)

- Docker Desktop para [Linux](https://docs.docker.com/desktop/linux/install/?_gl=1*pcopo9*_ga*MTcxOTEyMTU0OS4xNjk4Njk3MTUw*_ga_XJWPQMJYHQ*MTY5ODY5NzE0OS4xLjEuMTY5ODY5NzE1NC41NS4wLjA.)

## Uso

Após o clone da brach [main](https://github.com/thiagolinardi/fiap_tech_challenge/tree/main), execute o comando abaixo apontado o terminal para a raiz do projeto.

```
docker-compose up -d
```

Após a inicialização dos containers, basta acessar o endereço abaixo para ter acesso a documentação.

```
http://localhost:8081/docs
```

## Tecnologias implementadas

- ASP.NET 7.0
- Entity Framework Core 7.0
- .NET Core Native DI
- AutoMapper
- FluentValidator
- MediatR
- Redoc UI with JWT support

## Arquitetura

- Arquitetura completa com preocupações de separação de responsabilidades, SOLID e Clean Code.
- Domain Driven Design
- Domain Events
- Domain Notification
- Domain Validations
- CQRS (Imediate Consistency)
- Event Sourcing
- Unit of Work
- Repository

## Licença

O projeto foi desenvolvido por [Thiago Linardi](https://www.linkedin.com/in/thiagolinardi/) e [Victor Araujo](https://www.linkedin.com/in/victor-araujo-32216253/).

[MIT license](LICENSE).
