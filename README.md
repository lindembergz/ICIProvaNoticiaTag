<h1>ICIProvaNoticiaTag</h1>

Este repositório contém a implementação da prova .NET - Perfil Sênior, conforme especificações fornecidas. O projeto é uma aplicação ASP.NET Core MVC utilizando .NET 7, com foco na avaliação de habilidades em C#, Entity Framework, Migrations, Razor Views, JavaScript, jQuery, requisições Ajax, manipulação de JSON, rotinas assíncronas, validações e padrões de desenvolvimento como MVC e baixo acoplamento.

<h2>Estrutura do Projeto</h2>





Modelo de Dados: Entidades Tag e Noticia foram criadas utilizando a estratégia Model First com Entity Framework Migrations.



<h2>Funcionalidades:</h2>





CRUD de Tag implementado via formulários MVC.



CRUD de Noticia com cadastro e edição via requisições Ajax assíncronas, permitindo vínculo com múltiplas Tags.



Listagem e exclusão de Noticia processadas diretamente no back-end.



<h2>Tecnologias:</h2>





.NET 7



Entity Framework Core com Migrations



Razor Views com Tag Helpers



jQuery para manipulação de eventos e Ajax



JSON para troca de dados entre front-end e back-end



Validações: Implementadas tanto no front-end (via jQuery Validate) quanto no back-end (via ModelState).
