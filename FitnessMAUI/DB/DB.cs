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
        int userAutoIncrement = 1;

        List<Movie> movies = new List<Movie>();
        List<Studio> studios = new List<Studio>();
        List<User> users = new List<User>();

        string filename = Path.Combine(FileSystem.Current.AppDataDirectory, "db4.bin");
        string usersFilename = Path.Combine(FileSystem.Current.AppDataDirectory, "users_db.bin");

        public User CurrentUser { get; private set; }
        public bool IsAuthenticated => CurrentUser != null;

        public DB()
        {
            InitializeAsync();
            InitializeUsersAsync();
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

        private async void InitializeUsersAsync()
        {
            if (!File.Exists(usersFilename))
                return;

            try
            {
                await LoadUsersDataAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }


        public async Task<string> PickAndSaveFileAsync()
        {
            try
            {
                var fileResult = await FilePicker.Default.PickAsync();
                if (fileResult != null)
                {
                    return fileResult.FileName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка выбора файла: {ex.Message}");
            }

            return null;
        }

        public async Task<string> TakePhotoAsync()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        return photo.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка съемки фото: {ex.Message}");
            }

            return null;
        }

        
        public async Task<string> SaveFileAsync(FileResult fileResult)
        {
            try
            {
                if (fileResult != null)
                {
                    
                    var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "uploads");
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    
                    var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{fileResult.FileName}";
                    var targetFile = Path.Combine(filePath, fileName);

                    using (var stream = await fileResult.OpenReadAsync())
                    using (var fileStream = File.OpenWrite(targetFile))
                    {
                        await stream.CopyToAsync(fileStream);
                    }

                    return targetFile;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения файла: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> RegisterAsync(string username, string email, string password, string firstName, string lastName)
        {
            await Task.Delay(500);

            if (users.Any(u => u.Username == username || u.Email == email))
                return false;

            var user = new User
            {
                Id = userAutoIncrement++,
                Username = username,
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.Now
            };

            users.Add(user);
            await SaveUsersDataAsync();
            return true;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            await Task.Delay(500);

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }

            return false;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public async Task<bool> UpdateUserProfileAsync(User updatedUser)
        {
            await Task.Delay(500);

            var existingUser = users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (existingUser != null)
            {

                if (users.Any(u => u.Id != updatedUser.Id && (u.Username == updatedUser.Username || u.Email == updatedUser.Email)))
                    return false;

                existingUser.Username = updatedUser.Username;
                existingUser.Email = updatedUser.Email;
                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;


                if (CurrentUser?.Id == updatedUser.Id)
                {
                    CurrentUser = existingUser;
                }

                await SaveUsersDataAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            await Task.Delay(500);

            var user = users.FirstOrDefault(u => u.Id == userId && u.Password == currentPassword);
            if (user != null)
            {
                user.Password = newPassword;

                if (CurrentUser?.Id == userId)
                {
                    CurrentUser.Password = newPassword;
                }

                await SaveUsersDataAsync();
                return true;
            }

            return false;
        }

        public User GetCurrentUser() => CurrentUser;

        private async Task LoadDataAsync()
        {
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
                    movie.StudioId = br.ReadInt32();
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
                        studio.Rating = br.ReadDecimal();
                        studios.Add(studio);

                        if (studio.Id >= studioAutoIncrement)
                            studioAutoIncrement = studio.Id + 1;
                    }
                }
            }
        }

        private async Task LoadUsersDataAsync()
        {
            if (!File.Exists(usersFilename))
                return;

            using (var fs = File.OpenRead(usersFilename))
            using (var br = new BinaryReader(fs))
            {
                if (fs.Length == 0)
                    return;

                int count = br.ReadInt32();
                userAutoIncrement = 1;

                for (int i = 0; i < count; i++)
                {
                    User user = new User();
                    user.Id = br.ReadInt32();
                    user.Username = br.ReadString();
                    user.Email = br.ReadString();
                    user.Password = br.ReadString();
                    user.FirstName = br.ReadString();
                    user.LastName = br.ReadString();
                    user.CreatedAt = new DateTime(br.ReadInt64());
                    users.Add(user);

                    if (user.Id >= userAutoIncrement)
                        userAutoIncrement = user.Id + 1;
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
                        bw.Write(movie.StudioId);
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

        private async Task SaveUsersDataAsync()
        {
            await Task.Delay(500);
            try
            {
                using (var fs = File.Create(usersFilename))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(users.Count);
                    foreach (var user in users)
                    {
                        bw.Write(user.Id);
                        bw.Write(user.Username);
                        bw.Write(user.Email);
                        bw.Write(user.Password);
                        bw.Write(user.FirstName);
                        bw.Write(user.LastName);
                        bw.Write(user.CreatedAt.Ticks);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения пользователей: {ex.Message}");
                throw;
            }
        }

        public async Task AddMovieAsync(Movie tempMovie)
        {
            await Task.Delay(500);
            Movie movie = new Movie();
            movie.Id = movieAutoIncrement++;
            movie.Title = tempMovie.Title;
            movie.Rating = tempMovie.Rating;
            movie.Genres = tempMovie.Genres;
            movie.ImageUrl = tempMovie.ImageUrl;
            movie.ReleaseDate = tempMovie.ReleaseDate;
            movie.Type = tempMovie.Type;
            movies.Add(movie);
            await SaveDataAsync();
        }

        public async Task<Movie?> SearchMovieById(int tempMovie)
        {
            await Task.Delay(500);
            var existingMovie = movies.FirstOrDefault(m => m.Id == tempMovie);

            Movie movie = new Movie();
            if (existingMovie == null)
            {
                return movie;
            }

            movie.Id = existingMovie.Id;
            movie.Title = existingMovie.Title;
            movie.Rating = existingMovie.Rating;
            movie.Genres = existingMovie.Genres;
            movie.ImageUrl = existingMovie.ImageUrl;
            movie.ReleaseDate = existingMovie.ReleaseDate;
            movie.Type = existingMovie.Type;
            return movie;
        }

        public async Task EditMovieAsync(Movie tempMovie)
        {
            await Task.Delay(500);

            var existingMovie = movies.FirstOrDefault(m => m.Id == tempMovie.Id);

            if (existingMovie != null)
            {
                existingMovie.Title = tempMovie.Title;
                existingMovie.Rating = tempMovie.Rating;
                existingMovie.Genres = tempMovie.Genres;
                existingMovie.ImageUrl = tempMovie.ImageUrl;
                existingMovie.ReleaseDate = tempMovie.ReleaseDate;
                existingMovie.Type = tempMovie.Type;

                await SaveDataAsync();
            }
            else
            {
                await SaveDataAsync();
            }
        }

        public async Task<List<Movie>> GetMovies()
        {
            await Task.Delay(500);
            return movies.ToList();
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
        public int GetNextUserId() => userAutoIncrement;

        public async Task<List<User>> GetAllUsers()
        {
            await Task.Delay(500);
            return new List<User>(users);
        }

        public async Task<bool> UserExists(string username, string email)
        {
            await Task.Delay(100);
            return users.Any(u => u.Username == username || u.Email == email);
        }

        public async Task<bool> ValidateUserCredentials(string username, string password)
        {
            await Task.Delay(100);
            return users.Any(u => u.Username == username && u.Password == password);
        }
    }
}