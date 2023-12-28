using TesteKeyworks.Models;

namespace Testes.Mocks.ModelMocks
{
    public static class FilmeMock
    {
        public static Filme GetFilme()
        {
            return new Filme
            {
                Titulo = "Titulo 1",
                DataLancamento = DateTime.Now,
                Genero = "Gênero 1",
                Id = Guid.Empty,
                Avaliacoes = new List<Avaliacao>(),
                Streamings = new List<Streaming>()
            };
        }

        public static List<Filme> GetListaFilmes()
        {
            var lista = new List<Filme>
        {
            new Filme
            {
                Titulo = "Titulo 1",
                DataLancamento = DateTime.Now,
                Genero = "Gênero 1",
                Id = Guid.Empty,
                Avaliacoes = new List<Avaliacao>(),
                Streamings = new List<Streaming>()
            },
            new Filme
            {
                Titulo = "Titulo 2",
                DataLancamento = DateTime.Now,
                Genero = "Gênero 2",
                Id = Guid.Empty,
                Avaliacoes = new List<Avaliacao>(),
                Streamings = new List<Streaming>()
            },
            new Filme
            {
                Titulo = "Titulo 3",
                DataLancamento = DateTime.Now,
                Genero = "Gênero 3",
                Id = Guid.Empty,
                Avaliacoes = new List<Avaliacao>(),
                Streamings = new List<Streaming>()
            }
        };

            return lista;
        }

    }
}
