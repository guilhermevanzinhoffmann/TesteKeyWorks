using System.Linq.Expressions;
using TesteKeyworks.Models;
using TesteKeyworks.Repositories;

namespace TesteKeyworks.Services.Filmes
{
    public class FilmeService : IFilmeService
    {
        private readonly IRepository<Filme> _repository;

        public FilmeService(IRepository<Filme> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Filme filme)
        {
            if (filme == null) throw new ArgumentNullException(nameof(filme));

            await _repository.AddAsync(filme);
        }

        public async Task DeleteAsync(Filme filme)
        {
            if (filme == null) throw new ArgumentNullException(nameof(filme));

            await _repository.DeleteAsync(filme);
        }

        public async Task<IEnumerable<Filme>> GetAllAsync(string? genero, int? ano, string? titulo, string? streaming, int? avaliacao)
        {
            Expression<Func<Filme, bool>>? query = null;

            if (!string.IsNullOrEmpty(genero))
            {
                query = x => x.Genero == genero;
            }

            if (!string.IsNullOrEmpty(titulo))
            {
                Expression<Func<Filme, bool>>? queryTitulo = x => x.Titulo!.Contains(titulo);

                if (query != null)
                {

                    var parameter = Expression.Parameter(typeof(Filme), "filme");

                    var combined = Expression.Lambda<Func<Filme, bool>>(
                        Expression.AndAlso(
                            Expression.Invoke(query, parameter),
                            Expression.Invoke(queryTitulo, parameter)
                        ),
                        parameter
                    );

                    query = combined;
                }
                else
                {
                    query = queryTitulo;
                }
            }

            if (ano.HasValue)
            {
                Expression<Func<Filme, bool>>? queryAno = x => x.DataLancamento.Year == ano.Value;

                if (query != null)
                {

                    var parameter = Expression.Parameter(typeof(Filme), "filme");

                    var combined = Expression.Lambda<Func<Filme, bool>>(
                        Expression.AndAlso(
                            Expression.Invoke(query, parameter),
                            Expression.Invoke(queryAno, parameter)
                        ),
                        parameter
                    );

                    query = combined;
                }
                else 
                {
                    query = queryAno;
                }
            }

            if (!string.IsNullOrEmpty(streaming))
            {
                Expression<Func<Filme, bool>>? queryStreaming = x => x.Streamings.Any(y => y.Nome == streaming);

                if (query != null)
                {

                    var parameter = Expression.Parameter(typeof(Filme), "filme");

                    var combined = Expression.Lambda<Func<Filme, bool>>(
                        Expression.AndAlso(
                            Expression.Invoke(query, parameter),
                            Expression.Invoke(queryStreaming, parameter)
                        ),
                        parameter
                    );

                    query = combined;
                }
                else
                {
                    query = queryStreaming;
                }
            }

            if (avaliacao.HasValue)
            {
                Expression<Func<Filme, bool>>? queryAvaliacao = x => Math.Floor((decimal)(x.Avaliacoes.Average(x => x.Nota))) == (decimal)avaliacao.Value  ;

                if (query != null)
                {

                    var parameter = Expression.Parameter(typeof(Filme), "filme");

                    var combined = Expression.Lambda<Func<Filme, bool>>(
                        Expression.AndAlso(
                            Expression.Invoke(query, parameter),
                            Expression.Invoke(queryAvaliacao, parameter)
                        ),
                        parameter
                    );

                    query = combined;
                }
                else
                {
                    query = queryAvaliacao;
                }
            }

            var filmes = query == null 
                ? await _repository.GetAllAsync([x => x.Streamings, x => x.Avaliacoes]) 
                : await _repository.GetAsync(query!, [x => x.Streamings, x => x.Avaliacoes]);
            
            var filmesAgrupados = filmes.GroupBy(x => x.Genero);
            
            var resultado = filmesAgrupados.SelectMany(x => x);
            
            return resultado;
        }
            
            
        public async Task<IEnumerable<Filme>> GetByTituloAsync(string titulo)
        {
            if(string.IsNullOrWhiteSpace(titulo)) return new List<Filme>();

            
            return await _repository.GetAsync(x => x.Titulo!.Contains(titulo), [x =>x.Streamings ,x => x.Avaliacoes]);
        }

        public async Task<Filme> GetByIdAsync(Guid id)
            => await _repository.GetByIdAsync(id, [x => x.Streamings, x => x.Avaliacoes]);

        public async Task UpdateAsync(Filme filme)
        {
            if(filme == null) throw new ArgumentNullException(nameof(filme));

            await _repository.UpdateAsync(filme);
        }

        public async Task<IEnumerable<string?>> GetGenerosAsync()
            => (await _repository.GetAllAsync())?.OrderBy(x => x.Genero)?.Select(x => x.Genero)?.Distinct();


        public async Task<IEnumerable<int>> GetAnosLancamentoAsync()
            => (await _repository.GetAllAsync())?.OrderBy(x => x.DataLancamento)?.Select(x => x.DataLancamento.Year)?.Distinct();

        public async Task<int> GetTotalFilmesAsync()
            => (await _repository.GetAllAsync()).Count();
    }
}
