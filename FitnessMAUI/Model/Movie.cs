using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessMAUI.Model
{
    internal class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Studio Studio { get; set; }
        public int StudioId { get; set; }
        public DateTime Relise { get; set; } = DateTime.Now;
        public bool Status { get; set; }
    }
}
