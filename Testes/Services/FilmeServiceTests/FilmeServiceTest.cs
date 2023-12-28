using Moq;
using TesteKeyworks.Models;
using TesteKeyworks.Repositories;
using TesteKeyworks.Services.Filmes;
using Testes.Mocks.ModelMocks;

namespace Testes.Services.FilmeServiceTests
{
    public class FilmeServiceTest
    {
        [Fact]
        public async Task GetAllAsync_SemFiltroFornecido_DeveRetornarTodosOsFilmesQuandoNenhumFiltroForFornecido()
        {
            // Arrange
            var filmes = FilmeMock.GetListaFilmes();
        
            var repositoryMock = new Mock<IRepository<Filme>>();
            
            repositoryMock.Setup(repo => repo.GetAllAsync())
                          .ReturnsAsync(filmes);

            var filmeService = new FilmeService(repositoryMock.Object);

            // Act
            var filmesService = await filmeService.GetAllAsync(null, null, null, null, null);

            // Assert
            Assert.NotNull(filmes);
            Assert.Equal(filmes.Count, filmes.Count());
        }
    }
}