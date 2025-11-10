using FitnessMAUI.db;
using FitnessMAUI.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FitnessMAUI.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly DB _database;
        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _firstName;
        private string _lastName;
        private string _errorMessage;
        private string _successMessage;
        private bool _hasError;
        private bool _hasSuccess;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set => SetProperty(ref _successMessage, value);
        }

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public bool HasSuccess
        {
            get => _hasSuccess;
            set => SetProperty(ref _hasSuccess, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public RegisterViewModel()
        {
            _database = new DB();
            RegisterCommand = new Command(async () => await ExecuteRegister());
            NavigateToLoginCommand = new Command(async () => await ExecuteNavigateToLogin());
        }

        private async Task ExecuteRegister()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
                    string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Заполните все обязательные поля";
                    HasError = true;
                    HasSuccess = false;
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Пароли не совпадают";
                    HasError = true;
                    HasSuccess = false;
                    return;
                }

                if (Password.Length < 6)
                {
                    ErrorMessage = "Пароль должен содержать минимум 6 символов";
                    HasError = true;
                    HasSuccess = false;
                    return;
                }

                var success = await _database.RegisterAsync(Username, Email, Password, FirstName, LastName);
                if (success)
                {
                    SuccessMessage = "Регистрация успешна! Вы можете войти в систему.";
                    HasSuccess = true;
                    HasError = false;


                    await Task.Delay(2000);
                    await ExecuteNavigateToLogin();
                }
                else
                {
                    ErrorMessage = "Пользователь с таким именем или email уже существует";
                    HasError = true;
                    HasSuccess = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка регистрации: {ex.Message}";
                HasError = true;
                HasSuccess = false;
            }
        }

        private async Task ExecuteNavigateToLogin()
        {
            try
            {

                if (Application.Current?.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.Navigation.PopAsync();
                }
                else
                {
                    
                    await Shell.Current.GoToAsync("//LoginPage");
                }
            }
            catch (Exception ex)
            {
                
                Application.Current.MainPage = new LoginPage();
            }
        }

        public void ResetFields()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            HasError = false;
            HasSuccess = false;
        }
    }
}