namespace TesteKeyworks.Models
{
    public class FilmesStreamings
    {
        public Guid FilmeId { get; set; }
        public Filme Filme { get; set; }
        public Guid StreamingId { get; set; }
        public Streaming Streaming { get; set; }
    }
}
