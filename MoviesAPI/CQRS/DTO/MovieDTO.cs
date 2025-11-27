namespace MoviesAPI.CQRS.DTO
{
    public class MovieDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int? Rating { get; set; }

        public string? Genres { get; set; }

        public byte[]? ImageUrl { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public int? StudioId { get; set; }

        public string? Type { get; set; }

        public int? UserId { get; set; }

    }
}
