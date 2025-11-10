using FitnessMAUI.db;
using FitnessMAUI.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitnessMAUI.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
    {
        private readonly DB _database;
        private ObservableCollection<Movie> _favoriteMovies;
        private bool _isRefreshing;

        public ObservableCollection<Movie> FavoriteMovies
        {
            get => _favoriteMovies;
            set => SetProperty(ref _favoriteMovies, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand RefreshCommand { get; }

        public FavoritesViewModel()
        {
            _database = new DB();
            FavoriteMovies = new ObservableCollection<Movie>();
            RefreshCommand = new Command(async () => await LoadFavorites());
            LoadFavorites();
        }

        private async Task LoadFavorites()
        {
            IsRefreshing = true;
            var movies = await _database.GetMovies();          
            var favorites = movies.OrderByDescending(m => m.Rating).Take(6).ToList();

            FavoriteMovies.Clear();
            foreach (var movie in favorites)
            {
                FavoriteMovies.Add(movie);
            }
            IsRefreshing = false;
        }
    }
}