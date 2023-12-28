using TesteKeyworks.Models;
using TesteKeyworks.Repositories;

namespace TesteKeyworks.Services.Streamings
{
    public class StreamingService : IStreamingService
    {
        private readonly IRepository<Streaming> _repository;

        public StreamingService(IRepository<Streaming> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Streaming streaming)
        {
            if (streaming == null) throw new ArgumentNullException(nameof(streaming));

            await _repository.AddAsync(streaming);
        }

        public async Task DeleteAsync(Streaming streaming)
        {
            if (streaming == null) throw new ArgumentNullException(nameof(streaming));

            await _repository.DeleteAsync(streaming);
        }

        public async Task<IEnumerable<Streaming>> GetAllAsync()
            => await _repository.GetAllAsync(x => x.Filmes);

        public async Task<IEnumerable<Streaming>> GetByNomeAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentNullException(nameof(nome));


            return await _repository.GetAsync(x => x.Nome!.Contains(nome));
        }

        public async Task<Streaming> GetByIdAsync(Guid id)
            => await _repository.GetByIdAsync(id, x => x.Filmes);

        public async Task UpdateAsync(Streaming streaming)
        {
            if (streaming == null) throw new ArgumentNullException(nameof(streaming));

            await _repository.UpdateAsync(streaming);
        }

        public async Task<IEnumerable<string?>> GetNomeStreamingsAsync()
            => (await _repository.GetAllAsync())?.OrderBy(x => x.Nome)?.Select(x => x.Nome)?.Distinct();
    }
}
