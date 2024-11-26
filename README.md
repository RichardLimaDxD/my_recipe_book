## Sobre o projeto
Apresentando o **Meu Livro de Receitas** - uma aplicação para quem adora cozinhar e compartilhar receitas! O Meu Livro de Receitas foi projetado para tornar sua vida na cozinha mais fácil, ajudando você a se organizar, gerenciar suas receitas e tornar sua experiência culinária mais agradável.

Este projeto consiste em uma **API** desenvolvida em **.NET** para o gerenciamento de receitas culinárias. A **API** permite que os usuários se cadastrem fornecendo nome, e-mail e senha. Após o cadastro, os usuários podem criar, editar, filtrar e deletar receitas. Cada receita deve incluir um título, ingredientes e instruções. Adicionalmente, os usuários têm a opção de adicionar o tempo de preparo, nível de dificuldade e uma imagem ilustrativa à receita.

A **API** oferece suporte para **SQLServer** como opções de banco de dados, proporcionando flexibilidade na escolha do ambiente de armazenamento de dados.

Seguindo os princípios de **Domain-Driven Design (DDD)** e **SOLID**, a arquitetura do projeto busca manter um design modular e sustentável. A validação dos dados é realizada utilizando **FluentValidation**, assegurando que todas as entradas de dados atendam aos critérios estabelecidos.

Para garantir a qualidade do código, são implementados **testes de unidade e de integração**. A utilização de **injeção de dependências** promove uma melhor modularidade e testabilidade do código, facilitando a manutenção e evolução do projeto.

Outras tecnologias e práticas adotadas incluem o **Entity Framework** para o mapeamento objeto-relacional, a metodologia ágil **SCRUM** para o gerenciamento do projeto, e a implementação de **Tokens JWT & Refresh Token** para autenticação segura. As migrações do banco de dados são gerenciadas para assegurar uma evolução controlada do esquema de dados. Além disso, o uso de **Git** e a estratégia de ramificação **GitFlow** auxiliam na organização e controle das versões do código.
### Features

- **Gerenciamento de Receitas**: Criação, edição, exclusão e filtro de receitas. 🍲✏️🗑️🔍
- **Login com Google**: Integração para autenticação via conta Google. 🔑🔗🟦
- **Integração com ChatGPT**: Utilização de IA para melhorar a experiência dos usuários na geração de receitas a partir de ingredientes fornecidos. 🤖🍳
- **Mensageria**: Utilização de mensageria (Service Bus - Queue), para gerenciar a exclusão de contas. 📩🗂️🚫
- **Upload de Imagem**: Permite aos usuários enviar uma imagem para ilustrar suas receitas. 📸⬆️🖼️

### Construído com
[![My Skills](https://skillicons.dev/icons?i=azure,cs,docker,dotnet,postman,regex,visualstudio,windows&perline=4)](https://skillicons.dev)

## Getting Started

Para obter uma cópia local funcionando, siga estes passos simples.

### Requisitos

* Visual Studio versão 2022+ ou Visual Studio Code
* Windows 10+ ou Linux/MacOS com [.NET SDK][dot-net-sdk] instalado
* MySql Server ou SqlServer

### Instalação

1. Clone o repositório.
2. Preencha as informações no arquivo `appsettings.Development.json`.
3. Execute a API e aproveite o seu teste :)
