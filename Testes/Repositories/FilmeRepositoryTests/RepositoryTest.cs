using Microsoft.EntityFrameworkCore;
using TesteKeyworks.Data;
using TesteKeyworks.Models;
using TesteKeyworks.Repositories;
using Testes.Mocks.ModelMocks;

namespace Testes.Repositories.FilmeRepositoryTests
{
    public class RepositoryTest
    {
        [Fact]
        public async Task GetAllAsync_SemNenhumParametro_DeveRetornarTodosOsFilmes()
        {
            // Arrange

            var filmes = FilmeMock.GetListaFilmes();

            var options = new DbContextOptionsBuilder<TesteKeyworksContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new TesteKeyworksContext(options))
            {
                var repository = new Repository<Filme>(context);
                await context.AddRangeAsync(filmes);
                await context.SaveChangesAsync();
            };

            using (var context = new TesteKeyworksContext(options))
            {
                var repository = new Repository<Filme>(context);

                // Act 
                var result = await repository.GetAllAsync();

                // Assert 
                Assert.NotNull(result);
                Assert.Equal(result.Count(), filmes.Count);
            }
        }

    }
}
