using System;
using System.Collections.Generic;

namespace MoviesAPI.Models;

public partial class Studio
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? DirectorName { get; set; }

    public string? DirectorPatronymic { get; set; }

    public string? DirectorSurname { get; set; }

    public int? Rating { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
