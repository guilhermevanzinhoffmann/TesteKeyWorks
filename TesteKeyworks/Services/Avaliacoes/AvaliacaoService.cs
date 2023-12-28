using TesteKeyworks.Models;
using TesteKeyworks.Repositories;

namespace TesteKeyworks.Services.Avaliacoes
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IRepository<Avaliacao> _repository;

        public AvaliacaoService(IRepository<Avaliacao> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Avaliacao avaliacao)
        {
            if (avaliacao == null) throw new ArgumentNullException(nameof(avaliacao));

            await _repository.AddAsync(avaliacao);
        }

        public async Task DeleteAsync(Avaliacao avaliacao)
        {
            if (avaliacao == null) throw new ArgumentNullException(nameof(avaliacao));

            await _repository.DeleteAsync(avaliacao);
        }

        public async Task<IEnumerable<Avaliacao>> GetAllAsync()
            => await _repository.GetAllAsync(x => x.Filme);

        public async Task<Avaliacao> GetByIdAsync(Guid id)
            => await _repository.GetByIdAsync(id, x => x.Filme);

        public async Task UpdateAsync(Avaliacao avaliacao)
        {
            if (avaliacao == null) throw new ArgumentNullException(nameof(avaliacao));

            await _repository.UpdateAsync(avaliacao);
        }
    }
}
