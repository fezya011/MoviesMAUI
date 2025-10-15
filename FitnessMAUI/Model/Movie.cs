using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessMAUI.Model
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Rating { get; set; } = "";
        public string Genres { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public DateTime ReleaseDate { get; set; }
        public Studio Studio { get; set; }
        public string Type { get; set; } = "";
    }
}
