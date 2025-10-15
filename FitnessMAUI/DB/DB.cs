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

        int autoIncrement;

        List<Movie> movies = new List<Movie>();

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
            await Task.Delay(500);
            using (var fs = File.OpenRead(filename))
            using (var br = new BinaryReader(fs))
            {
                if (fs.Length == 0)
                    return;

                int count = br.ReadInt32();

                
                for (int i = 0; i < count; i++)
                {
                    Movie movie = new Movie();
                    movie.Id = br.ReadInt32();
                    movie.Title = br.ReadString();
                    movie.Rating = br.ReadString();
                    movie.Genres = br.ReadString();
                    movie.ImageUrl = br.ReadString();
                    movie.ReleaseDate = new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                    movie.Type = br.ReadString();
                    movies.Add(movie);
                    ;
                }

            }

        }

        public async Task SaveMovieAsync()
        {
            await Task.Delay(500);
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
             
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                throw;
            }

        }


        public async Task SaveStudioAsync()
        {
            await Task.Delay(500);
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
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
               
                throw;
            }
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await Task.Delay(500);
            movie.Id++;
            movies.Add(movie);
            await SaveMovieAsync();
        }

       
        public async Task<List<Movie>> GetMovies()
        {
            await Task.Delay(500);
            return new List<Movie>(movies);
        }

        
        public async Task DeleteMovieAsync(Movie movie)
        {
            await Task.Delay(500);
            movies.Remove(movie);
            await SaveMovieAsync();
        }

        public async Task AddStudioAsync(Studio studio)
        {
            await Task.Delay(500);
            studio.Id++;
            studios.Add(studio);
            await SaveStudioAsync();
        }

        public async Task<List<Studio>> GetStudios()
        {
            await Task.Delay(500);
            return studios;
        }

        public async Task DeleteStudioAsync(Studio studio)
        {
            await Task.Delay(500);
            studios.Remove(studio);
            await SaveStudioAsync();
        }

    }
}