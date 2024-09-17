using System.ComponentModel.DataAnnotations;

namespace Cafe_NET_API.Entities
{
    public class Cafe
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string Logo { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
