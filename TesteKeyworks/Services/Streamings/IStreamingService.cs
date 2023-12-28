using TesteKeyworks.Models;

namespace TesteKeyworks.Services.Streamings
{
    public interface IStreamingService
    {
        Task<Streaming> GetByIdAsync(Guid id);
        Task<IEnumerable<Streaming>> GetByNomeAsync(string nome);
        Task<IEnumerable<Streaming>> GetAllAsync();
        Task AddAsync(Streaming streaming);
        Task UpdateAsync(Streaming streaming);
        Task DeleteAsync(Streaming streaming);
        Task<IEnumerable<string?>> GetNomeStreamingsAsync();
    }
}
