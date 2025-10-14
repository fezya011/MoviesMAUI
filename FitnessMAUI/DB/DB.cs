using FitnessMAUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        List<Movie> movies { get; set; } = new List<Movie>();

        List<Studio> studios = new List<Studio>();


        string filename = Path.Combine(FileSystem.Current.AppDataDirectory, "db.bin");

        private static DB instance;

        public DB()
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
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
                    movies.Add(new Movie
                    {
                        Id = br.ReadInt32(),
                        Title = br.ReadString(),
                        Rating = br.ReadString(),
                        Genres = br.ReadString(),
                        ImageUrl = br.ReadString(),
                        ReleaseDate = new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),
                        Type = br.ReadString(),
                    });
                }

            }
            await Task.CompletedTask;
            Task.Delay(50);
        }

        public async Task SaveMovieAsync()
        {
            try
            {

                using (var fs = File.Create(filename))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(movies.Count);
                    foreach (var movie in movies)
                    {
                        bw.Write(movie.Id);
                        bw.Write(movie.Title);
                        bw.Write(movie.Genres);
                        bw.Write(movie.ImageUrl);
                        bw.Write(movie.Rating);
                        bw.Write(movie.ReleaseDate.Year);
                        bw.Write(movie.ReleaseDate.Month);
                        bw.Write(movie.ReleaseDate.Day);
                        bw.Write(movie.Type);
                    }
                }
                await Task.CompletedTask;
                Task.Delay(50);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                Task.Delay(50);
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
                    bw.Write(studios.Count);
                    foreach (var studio in studios)
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
                Task.Delay(50);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                Task.Delay(50);
                throw;
            }
        }

        public async Task AddMovieAsync(Movie movie)
        {
            movie.Id++;
            movies.Add(movie);
            await SaveMovieAsync();
            Task.Delay(50);
        }

       
        public List<Movie> GetMovies()
        {
            Task.Delay(50);
            return new List<Movie>(movies);
        }

        
        public async Task DeleteMovieAsync(Movie movie)
        {
            movies.Remove(movie);
            await SaveMovieAsync();
            Task.Delay(50);
        }

        public async Task AddStudioAsync(Studio studio)
        {
            studio.Id++;
            studios.Add(studio);
            await SaveStudioAsync();
            Task.Delay(50);
        }

        public List<Studio> GetStudios()
        {
            Task.Delay(50);
            return studios;
        }

        public async Task DeleteStudioAsync(Studio studio)
        {
            studios.Remove(studio);
            await SaveStudioAsync();
            Task.Delay(50);
        }

    }
}