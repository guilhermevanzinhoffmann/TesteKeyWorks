using System.ComponentModel.DataAnnotations;

namespace TesteKeyworks.Models.ViewModels
{
    public class AvaliacaoView
    {
        public Guid? Id { get; set; }
        public Guid FilmeId {  get; set; }
        public string TituloFilme { get; set; }
        public int Nota { get; set; } = 1;
        public string Comentario { get; set; }

        public static implicit operator AvaliacaoView(Avaliacao avaliacao)
            => new()
            {
                Id = avaliacao.Id,
                FilmeId = avaliacao.Filme?.Id ?? Guid.Empty,
                TituloFilme = avaliacao.Filme?.Titulo ?? string.Empty,
                Comentario = avaliacao.Comentario,
                Nota = avaliacao.Nota
            };
    }
}
