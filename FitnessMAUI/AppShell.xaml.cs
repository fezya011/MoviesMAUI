using FitnessMAUI.db;
using FitnessMAUI.Model;
using FitnessMAUI.ViewModels;
using FitnessMAUI.Views;
using FitnessMAUI.VMTools;
using System.Windows.Input;

namespace FitnessMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel();

            Routing.RegisterRoute(nameof(AddEditMoviePage), typeof(AddEditMoviePage));
            Routing.RegisterRoute(nameof(AddStudioPage), typeof(AddStudioPage));
            Routing.RegisterRoute(nameof(MoviesPage), typeof(MoviesPage));
            Routing.RegisterRoute(nameof(FavoritesPage), typeof(FavoritesPage));
            Routing.RegisterRoute(nameof(StudiosPage), typeof(StudiosPage));
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }
    }

    public class AppShellViewModel : BaseViewModel
    {
        private readonly DB _database;
        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                SetProperty(ref _currentUser, value);
                OnPropertyChanged(nameof(IsAuthenticated));

            }
        }


        public bool IsAuthenticated => CurrentUser != null;

        public CommandVM LogoutCommand { get; }
        public CommandVM OpenAllMovies { get; }
        public CommandVM OpenAllStudios { get; }
        public CommandVM OpenFavorites { get; }

        public AppShellViewModel()
        {
            _database = new DB();
            LogoutCommand = new CommandVM(async () => await ExecuteLogout());
            OpenAllMovies = new CommandVM(async () => await OpenAllMoviesAsync());
            OpenAllStudios = new CommandVM(async () => await OpenAllStudiosAsync());
            OpenFavorites = new CommandVM(async () => await OpenFavoritesAsync());
            LoadCurrentUser();
        }

        private async void LoadCurrentUser()
        {
            try
            {
                CurrentUser = _database.GetCurrentUser();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading user: {ex.Message}");
            }
        }

        private async Task ExecuteLogout()
        {
            try
            {
                _database.Logout();
                CurrentUser = null;           
                await Shell.Current.GoToAsync("//" + nameof(LoginPage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logout error: {ex.Message}");
            }
        }

        private async Task OpenAllMoviesAsync()
        {
            await Shell.Current.GoToAsync(nameof(MoviesPage));
        }
        private async Task OpenAllStudiosAsync()
        {
            await Shell.Current.GoToAsync(nameof(StudiosPage));
        }
        private async Task OpenFavoritesAsync()
        {
            await Shell.Current.GoToAsync(nameof(FavoritesPage));
        }
    }
}
