using System.ComponentModel.DataAnnotations;

namespace TesteKeyworks.Models
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
