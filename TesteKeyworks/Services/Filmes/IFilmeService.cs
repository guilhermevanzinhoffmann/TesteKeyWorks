using System.Linq.Expressions;
using TesteKeyworks.Models;

namespace TesteKeyworks.Services.Filmes
{
    public interface IFilmeService
    {
        Task<Filme> GetByIdAsync(Guid id);
        Task<IEnumerable<Filme>> GetByTituloAsync(string titulo);
        Task<IEnumerable<Filme>> GetAllAsync(string? genero, int? ano, string? titulo, string? streaming, int? avaliacao);
        Task AddAsync(Filme filme);
        Task UpdateAsync(Filme filme);
        Task DeleteAsync(Filme filme);
        Task<IEnumerable<string?>> GetGenerosAsync();
        Task<IEnumerable<int>> GetAnosLancamentoAsync();
        Task<int> GetTotalFilmesAsync();
    }
}
