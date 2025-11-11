using FitnessMAUI.db;
using FitnessMAUI.Model;
using FitnessMAUI.ViewModels;
using FitnessMAUI.Views;
using FitnessMAUI.VMTools;

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
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(MainShellPage), typeof(MainShellPage));
        }
         
        protected override void OnAppearing()
        {
            base.OnAppearing();

           
            if (!((AppShellViewModel)BindingContext).IsAuthenticated)
            {
                
                GoToAsync("//LoginPage");
            }
        }
    }

    public class AppShellViewModel : BaseViewModel
    {
        private readonly DB _database;
        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public bool IsAuthenticated => CurrentUser != null;

        public CommandVM LogoutCommand { get; }

        public AppShellViewModel()
        {
            _database = new DB();
            LogoutCommand = new CommandVM(ExecuteLogout);
            LoadCurrentUser();
        }

        private void LoadCurrentUser()
        {
            CurrentUser = _database.GetCurrentUser();
        }

        private void ExecuteLogout()
        {
            _database.Logout();
            CurrentUser = null;
            Shell.Current.GoToAsync("//LoginPage");
        }
    }
}