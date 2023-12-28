using Microsoft.AspNetCore.Mvc;
using Moq;
using TesteKeyworks.Controllers;
using TesteKeyworks.Models.Paginacao;
using TesteKeyworks.Models.ViewModels;
using TesteKeyworks.Services.Filmes;
using TesteKeyworks.Services.Streamings;
using Testes.Mocks.ModelMocks;

namespace Testes.Controllers.FilmesControllerTests
{
    public class FilmesControllerTest
    {
        [Fact]
        public async Task Index_SemParametrosParaFiltro_DeveRetornarViewComListaFilmeViewContendoTresElementos()
        {
            // Arrange
            var filmes = FilmeMock.GetListaFilmes();
            var serviceMock = new Mock<IFilmeService>();
            var streamingSeviceMock = new Mock<IStreamingService>();

            serviceMock.Setup(s => s.GetAllAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(filmes);

            serviceMock.Setup(s => s.GetAnosLancamentoAsync())
                .ReturnsAsync(new List<int>());

            serviceMock.Setup(s => s.GetGenerosAsync())
                .ReturnsAsync(new List<string>());

            streamingSeviceMock.Setup(s => s.GetNomeStreamingsAsync())
                .ReturnsAsync(new List<string>());

            var controller = new FilmesController(serviceMock.Object, streamingSeviceMock.Object);

            // Act 
            var result = await controller.Index(null, null, null, null, null, null) as ViewResult;

            // Assert 
            Assert.NotNull(result);
            Assert.IsType<PaginatedList<FilmeView>>(result.Model);

            var filmesResult = result.Model as PaginatedList<FilmeView>;
            Assert.Equal(filmesResult?.ToList().Count, 3); 
        }
    }
}