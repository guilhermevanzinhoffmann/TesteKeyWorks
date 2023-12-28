using TesteKeyworks.Models.ViewModels;

namespace TesteKeyworks.Models
{
    public class Streaming : Entity
    {
        public string? Nome {  get; set; }
        public ICollection<Filme> Filmes { get; set; }

        public static implicit operator Streaming(StreamingView view)
            => new()
            {
                Id = view.Id ?? Guid.Empty,
                Nome = view.Nome,
                Filmes = []
            };
    }
}
