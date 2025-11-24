using FitnessMAUI.db;
using FitnessMAUI.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FitnessMAUI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly DB _database;
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _hasError;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel()
        {
            _database = new DB();
            LoginCommand = new Command(async () => await ExecuteLogin());
            NavigateToRegisterCommand = new Command(async () => await ExecuteNavigateToRegister());
        }

        private async Task ExecuteLogin()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Заполните все поля";
                    HasError = true;
                    return;
                }

                var success = await _database.LoginAsync(Username, Password);
                if (success)
                {
                    HasError = false;
                    await Shell.Current.GoToAsync("//"+nameof(MainPage));
                }
                else
                {
                    ErrorMessage = "Неверное имя пользователя или пароль";
                    HasError = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка входа: {ex.Message}";
                HasError = true;
            }
        }

        private async Task ExecuteNavigateToRegister()
        {
            await Shell.Current.GoToAsync("//" + nameof(RegisterPage));
        }

        public void ResetFields()
        {
            Username = string.Empty;
            Password = string.Empty;
            HasError = false;
        }
    }
}