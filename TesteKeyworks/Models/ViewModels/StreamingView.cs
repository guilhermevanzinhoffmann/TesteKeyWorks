namespace TesteKeyworks.Models.ViewModels
{
    public class StreamingView
    {
        public Guid? Id { get; set; }
        public string? Nome { get; set; }
        public bool Selecionado { get; set; } = false;

        public static implicit operator StreamingView(Streaming streaming)
            => new()
            {
                Id = streaming.Id,
                Nome = streaming.Nome
            };
    }
}
