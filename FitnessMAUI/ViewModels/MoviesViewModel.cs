using FitnessMAUI.db;
using FitnessMAUI.Model;
using FitnessMAUI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitnessMAUI.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        private readonly DB _database;
        private ObservableCollection<Movie> _movies;
        private Movie _selectedMovie;
        private bool _isRefreshing;

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set => SetProperty(ref _movies, value);
        }

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set => SetProperty(ref _selectedMovie, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddMovieCommand { get; }
        public ICommand EditMovieCommand { get; }
        public ICommand DeleteMovieCommand { get; }

        public MoviesViewModel()
        {
            _database = new DB();
            Movies = new ObservableCollection<Movie>();

            RefreshCommand = new Command(async () => await LoadMovies());
            AddMovieCommand = new Command(async () => await ExecuteAddMovie());
            EditMovieCommand = new Command<Movie>(async (movie) => await ExecuteEditMovie(movie));
            DeleteMovieCommand = new Command<Movie>(async (movie) => await ExecuteDeleteMovie(movie));

            // Загружаем фильмы при создании
            Task.Run(async () => await LoadMovies());
        }

        private async Task LoadMovies()
        {
            try
            {
                IsRefreshing = true;
                var movies = await _database.GetMovies();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Movies.Clear();
                    foreach (var movie in movies)
                    {
                        Movies.Add(movie);
                    }
                    IsRefreshing = false;
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки фильмов: {ex.Message}");
                IsRefreshing = false;
            }
        }

        private async Task ExecuteAddMovie()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Нажата кнопка добавления фильма");             

                
                 await Shell.Current.GoToAsync(nameof(AddEditMoviePage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка перехода на AddMoviePage: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось открыть страницу добавления: {ex.Message}", "OK");
            }
        }

        private async Task ExecuteEditMovie(Movie movie)
        {
            if (movie != null)
            {
                try
                {
                    var parameters = new Dictionary<string, object>
                    {
                        ["Movie"] = movie
                    };
                    await Shell.Current.GoToAsync(nameof(AddEditMoviePage), parameters);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось открыть редактор: {ex.Message}", "OK");
                }
            }
        }

        private async Task ExecuteDeleteMovie(Movie movie)
        {
            if (movie != null)
            {
                bool result = await Application.Current.MainPage.DisplayAlert(
                    "Удаление",
                    $"Вы уверены, что хотите удалить фильм '{movie.Title}'?",
                    "Да", "Нет");

                if (result)
                {
                    try
                    {
                        await _database.DeleteMovieAsync(movie);
                        await LoadMovies();
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось удалить фильм: {ex.Message}", "OK");
                    }
                }
            }
        }
    }
}