using FitnessMAUI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FitnessMAUI.db
{
    public class DB
    {
        int movieAutoIncrement = 1;
        int studioAutoIncrement = 1;

        List<Movie> movies = new List<Movie>();
        List<Studio> studios = new List<Studio>();

        string filename = Path.Combine(FileSystem.Current.AppDataDirectory, "db2.bin");

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


                movieAutoIncrement = 1;
                studioAutoIncrement = 1;

                for (int i = 0; i < count; i++)
                {
                    Movie movie = new Movie();
                    movie.Id = br.ReadInt32();
                    movie.Title = br.ReadString();
                    movie.Rating = br.ReadDecimal();
                    movie.Genres = br.ReadString();
                    movie.ImageUrl = br.ReadString();
                    movie.ReleaseDate = new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                    movie.Type = br.ReadString();
                    movies.Add(movie);


                    if (movie.Id >= movieAutoIncrement)
                        movieAutoIncrement = movie.Id + 1;
                }

                if (fs.Position < fs.Length)
                {
                    int studioCount = br.ReadInt32();
                    for (int i = 0; i < studioCount; i++)
                    {
                        Studio studio = new Studio();
                        studio.Id = br.ReadInt32();
                        studio.Name = br.ReadString();
                        studio.DirectorName = br.ReadString();
                        studio.DirectorPatronymic = br.ReadString();
                        studio.DirectorSurname = br.ReadString();
                        studio.Rating = br.ReadInt32();
                        studios.Add(studio);

                        if (studio.Id >= studioAutoIncrement)
                            studioAutoIncrement = studio.Id + 1;
                    }
                }
            }
        }

        public async Task SaveDataAsync()
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
                        bw.Write(movie.Rating);
                        bw.Write(movie.Genres);
                        bw.Write(movie.ImageUrl);
                        bw.Write(movie.ReleaseDate.Year);
                        bw.Write(movie.ReleaseDate.Month);
                        bw.Write(movie.ReleaseDate.Day);
                        bw.Write(movie.Type);
                        
                    }

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
            movie.Id = movieAutoIncrement++;
            movies.Add(movie);
            await SaveDataAsync();
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
            await SaveDataAsync();
        }

        public async Task AddStudioAsync(Studio studio)
        {
            await Task.Delay(500);
            studio.Id = studioAutoIncrement++;
            studios.Add(studio);
            await SaveDataAsync();
        }

        public async Task<List<Studio>> GetStudios()
        {
            await Task.Delay(500);
            return new List<Studio>(studios);
        }

        public async Task DeleteStudioAsync(Studio studio)
        {
            await Task.Delay(500);
            studios.Remove(studio);
            await SaveDataAsync();
        }

        public int GetNextMovieId() => movieAutoIncrement;
        public int GetNextStudioId() => studioAutoIncrement;
    }
}