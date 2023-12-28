using TesteKeyworks.Models.ViewModels;

namespace TesteKeyworks.Models
{
    public class Filme : Entity
    {
        public string? Titulo {  get; set; }
        public string? Genero { get; set; }     
        public DateTime DataLancamento { get; set; }
        public ICollection<Avaliacao> Avaliacoes { get; set; }
        public ICollection<Streaming> Streamings { get; set; }

        public static implicit operator Filme(FilmeView view)
            => new()
            {
                Id = view?.Id ?? Guid.Empty,
                Genero = view?.Genero,
                Titulo = view?.Titulo,
                DataLancamento = DateTime.Parse((view?.DataLancamento ?? DateTime.Now.ToString("MM/yyyy"))),
                Streamings = []
            };
    }
}
