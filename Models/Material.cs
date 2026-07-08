using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Color { get; set; }
        public int AuthorId { get; set; }
        public User? Author { get; set; }
        public List<MaterialResource> Resources { get; set; } = new();
    }
}
