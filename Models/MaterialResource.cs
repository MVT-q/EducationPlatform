using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Models
{
    public class MaterialResource
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? Type { get; set; }
        public int MaterialId { get; set; }
        public Material? Material { get; set; }
    }
}
