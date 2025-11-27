using MoviesAPI.CQRS.DTO;
using MoviesAPI.DB;
using MoviesAPI.Models;
using MyMediator.Interfaces;
using MyMediator.Types;

namespace MoviesAPI.CQRS.Commands
{
    public class AddMovieCommand : IRequest
    {
        public MovieDTO Movie { get; set; }

        public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, Unit>
        {
            private readonly MoviesMauiContext db;

            public AddMovieCommandHandler(MoviesMauiContext db)
            {
                this.db = db;
            }

            public async Task<Unit> HandleAsync(AddMovieCommand request, CancellationToken ct = default)
            {
                var movie = new Movie
                {
                    Title = request.Movie.Title,
                    Rating = request.Movie.Rating,
                    Genres = request.Movie.Genres,
                    ImageUrl = request.Movie.ImageUrl,
                    ReleaseDate = request.Movie.ReleaseDate,
                    StudioId = request.Movie.StudioId,
                    Type = request.Movie.Type,
                    UserId = request.Movie.UserId,
                };

                db.Movies.Add(movie);
                await db.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
