using FitnessMAUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessMAUI.db
{
    public class DB
    {
        public static DB Instance
        {
            get
            {
                if (instance == null)
                    instance = new DB();
                return instance;
            }
        }

        public int AutoIncrement { get; set; }

        List<Movie> PopularMovies { get; set; } = new List<Movie>();
        List<Movie> ComingSoonMovies { get; set; } = new List<Movie>();
        List<Movie> TopRatedMovies { get; set; } = new List<Movie>();

        List<Studio> Studios { get; set; } = new List<Studio>();

        string filename = Path.Combine(FileSystem.Current.AppDataDirectory, "db.bin");

        private static DB instance;

        public DB()
        {
            
        }

        public async Task InitializeAsync()
        {
            if (!File.Exists(filename))
                return;

            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private async Task LoadDataAsync()
        {
            using (var fs = File.OpenRead(filename))
            using (var br = new BinaryReader(fs))
            {
                if (fs.Length == 0)
                    return;

                int count = br.ReadInt32();

                
                for (int i = 0; i < count; i++)
                {
                    PopularMovies.Add(new Movie
                    {
                        Id = br.ReadInt32(),
                        Title = br.ReadString(),
                        Rating = br.ReadString(),
                        Genres = br.ReadString(),
                        ImageUrl = br.ReadString(),
                        ReleaseDate = new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),
                    });
                }

               
                for (int i = 0; i < count; i++)
                {
                    ComingSoonMovies.Add(new Movie
                    {
                        Id = br.ReadInt32(),
                        Title = br.ReadString(),
                        Rating = br.ReadString(),
                        Genres = br.ReadString(),
                        ImageUrl = br.ReadString(),
                        ReleaseDate = new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),
                    });
                }

               
                for (int i = 0; i < count; i++)
                {
                    TopRatedMovies.Add(new Movie
                    {
                        Id = br.ReadInt32(),
                        Title = br.ReadString(),
                        Rating = br.ReadString(),
                        Genres = br.ReadString(),
                        ImageUrl = br.ReadString(),
                        ReleaseDate = new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),
                    });
                }

               
                for (int i = 0; i < count; i++)
                {
                    Studios.Add(new Studio
                    {
                        Id = br.ReadInt32(),
                        Name = br.ReadString(),
                        DirectorName = br.ReadString(),
                        DirectorPatronymic = br.ReadString(),
                        DirectorSurname = br.ReadString(),
                        Rating = br.ReadInt32()
                    });
                }
            }

            await Task.CompletedTask;
        }

        public async Task SavePopularMovieAsync()
        {
            try
            {
                using (var fs = File.Create(filename))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(PopularMovies.Count);
                    foreach (var movie in PopularMovies)
                    {
                        bw.Write(movie.Id);
                        bw.Write(movie.Title);
                        bw.Write(movie.Genres);
                        bw.Write(movie.ImageUrl);
                        bw.Write(movie.Rating);
                        bw.Write(movie.ReleaseDate.Year);
                        bw.Write(movie.ReleaseDate.Month);
                        bw.Write(movie.ReleaseDate.Day);
                    }
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }
        }

        public async Task SaveComingSoonMovieAsync()
        {
            try
            {
                using (var fs = File.Create(filename))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(ComingSoonMovies.Count);
                    foreach (var movie in ComingSoonMovies)
                    {
                        bw.Write(movie.Id);
                        bw.Write(movie.Title);
                        bw.Write(movie.Genres);
                        bw.Write(movie.ImageUrl);
                        bw.Write(movie.Rating);
                        bw.Write(movie.ReleaseDate.Year);
                        bw.Write(movie.ReleaseDate.Month);
                        bw.Write(movie.ReleaseDate.Day);
                    }
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }
        }

        public async Task SaveTopRatedMovieAsync()
        {
            try
            {
                using (var fs = File.Create(filename))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(TopRatedMovies.Count);
                    foreach (var movie in TopRatedMovies)
                    {
                        bw.Write(movie.Id);
                        bw.Write(movie.Title);
                        bw.Write(movie.Genres);
                        bw.Write(movie.ImageUrl);
                        bw.Write(movie.Rating);
                        bw.Write(movie.ReleaseDate.Year);
                        bw.Write(movie.ReleaseDate.Month);
                        bw.Write(movie.ReleaseDate.Day);
                    }
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }
        }

        public async Task SaveStudioAsync()
        {
            try
            {
                using (var fs = File.Create(filename))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(Studios.Count);
                    foreach (var studio in Studios)
                    {
                        bw.Write(studio.Id);
                        bw.Write(studio.Name);
                        bw.Write(studio.DirectorName);
                        bw.Write(studio.DirectorPatronymic);
                        bw.Write(studio.DirectorSurname);
                        bw.Write(studio.Rating);
                    }
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }
        }

        public async Task AddPopularMovieAsync(Movie movie)
        {
            movie.Id++;
            PopularMovies.Add(movie);
            await SavePopularMovieAsync();
        }

        public async Task AddComingSoonMovieAsync(Movie movie)
        {
            movie.Id++;
            ComingSoonMovies.Add(movie);
            await SaveComingSoonMovieAsync();
        }

        public async Task AddTopRatedMovieAsync(Movie movie)
        {
            movie.Id++;
            TopRatedMovies.Add(movie);
            await SaveTopRatedMovieAsync();
        }

        public List<Movie> GetPopularMovies()
        {
            return PopularMovies;
        }

        public List<Movie> GetComingSoonMovies()
        {
            return ComingSoonMovies;
        }

        public List<Movie> GetTopRatedMovies()
        {
            return TopRatedMovies;
        }

        public async Task DeletePopularMovieAsync(Movie movie)
        {
            PopularMovies.Remove(movie);
            await SavePopularMovieAsync();
        }

        public async Task DeleteComingSoonMovieAsync(Movie movie)
        {
            ComingSoonMovies.Remove(movie);
            await SaveComingSoonMovieAsync();
        }

        public async Task DeleteTopRatedMoviesAsync(Movie movie)
        {
            TopRatedMovies.Remove(movie);
            await SaveTopRatedMovieAsync();
        }

        public async Task AddStudioAsync(Studio studio)
        {
            studio.Id++;
            Studios.Add(studio);
            await SaveStudioAsync();
        }

        public List<Studio> GetStudios()
        {
            return Studios;
        }

        public async Task DeleteStudioAsync(Studio studio)
        {
            Studios.Remove(studio);
            await SaveStudioAsync();
        }

       
        public async Task SaveAllAsync()
        {
            await SavePopularMovieAsync();
            await SaveComingSoonMovieAsync();
            await SaveTopRatedMovieAsync();
            //await SaveStudioAsync();
        }
    }
}