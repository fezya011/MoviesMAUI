using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessMAUI.Model
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string DirectorName { get; set; } = "";
        public string DirectorPatronymic { get; set; } = "";
        public string DirectorSurname { get; set; } = "";
        public int Rating { get; set; }
    }
}
