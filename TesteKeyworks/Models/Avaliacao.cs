﻿using TesteKeyworks.Models.ViewModels;

namespace TesteKeyworks.Models
{
    public class Avaliacao : Entity
    {
        public int Nota { get; set; }
        public string Comentario { get; set; }

        public Guid FilmeId { get; set; }
        public Filme Filme { get; set; }

        public static implicit operator Avaliacao(AvaliacaoView view)
            => new()
            {
                Id = view.Id ?? Guid.Empty,
                Nota = view.Nota,
                Comentario = view.Comentario,
                FilmeId = view.FilmeId  
            };
    }
}
