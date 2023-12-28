using System.ComponentModel.DataAnnotations;

namespace TesteKeyworks.Models.ViewModels
{
    public class FilmeView
    {
        public Guid? Id { get; set; }
        public string? Titulo { get; set; }
        public string? Genero { get; set; }
        [DataType(DataType.Date)]
        public string DataLancamento { get; set; } = DateTime.Now.ToString("yyyy/MM");
        public string? MediaAvaliacoes { get; set; }
        public List<StreamingView> Streamings { get; set; }

        public static implicit operator FilmeView(Filme filme)
            => new() 
            {
                Id = filme?.Id,
                DataLancamento = (filme?.DataLancamento ?? DateTime.Now).ToString("MM/yyyy"),
                Genero = filme?.Genero,
                Titulo = filme?.Titulo,
                MediaAvaliacoes = filme!.Avaliacoes.Any() ? filme!.Avaliacoes?.Average(x => x.Nota).ToString("F2") : "Sem avaliações",
                Streamings = filme!.Streamings?.Select(x => (StreamingView)x)?.ToList() ?? new List<StreamingView>()
            };

    }
}
