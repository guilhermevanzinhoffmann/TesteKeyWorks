using TesteKeyworks.Models;

namespace TesteKeyworks.Services.Avaliacoes
{
    public interface IAvaliacaoService
    {
        Task<Avaliacao> GetByIdAsync(Guid id);
        Task<IEnumerable<Avaliacao>> GetAllAsync();
        Task AddAsync(Avaliacao avaliacao);
        Task UpdateAsync(Avaliacao avaliacao);
        Task DeleteAsync(Avaliacao avaliacao);
    }
}
