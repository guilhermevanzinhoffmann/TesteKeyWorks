# TesteKeyWorks

Um projeto ASP.NET MVC com testes unitários usando xUnit e Moq, desenvolvido com .NET 8 e Entity Framework.

## Tecnologias Utilizadas

- ASP.NET MVC
- .NET 8
- Entity Framework
- xUnit
- Moq
- SQL Server Express LocalDB

## Clonando o Repositório

Para clonar este repositório e obter uma cópia local, use o seguinte comando:  
```git clone https://github.com/guilhermevanzinhoffmann/TesteKeyWorks.git```

## Instalação

Para iniciar o projeto localmente, você pode escolher entre duas opções:

### Usando o Visual Studio

1. Clone o repositório.
2. Abra o projeto no Visual Studio.
3. Execute o projeto pressionando `F5` ou clicando em "Iniciar".

### Usando a Linha de Comando

1. Navegue até o diretório do projeto.
2. Execute o projeto com os comandos necessários do .NET CLI.
3. Exemplo:
   ```
   cd TesteKeyWorks
   dotnet run
   ```

Certifique-se de ter todas as dependências instaladas e configuradas antes de executar o projeto.

## Testes Unitários

Os testes unitários foram implementados usando xUnit para estruturação e Moq para mocks. Eles estão localizados no projeto `Testes`.   
Importante saber que apenas tres testes foram criados (faltou tempo para realizar a cobertura completa).

### Executando os testes

É possível executar os testes unitários a partir do Visual Studio ou 
1. Navegando até o diretório do projeto.
2. Execute os testes com os comandos necessários do .NET CLI.
3. Exemplo:
   ```
   cd Testes
   dotnet test
   ```
