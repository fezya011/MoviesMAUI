using FitnessMAUI.db;
using FitnessMAUI.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FitnessMAUI.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly DB _database;
        private User _currentUser;
        private string _currentPassword;
        private string _newPassword;
        private string _confirmNewPassword;
        private string _selectedFileName;
        private FileResult _selectedFile;
        private bool _hasSelectedFile;

        public string FirstName
        {
            get => _currentUser?.FirstName ?? "";
            set
            {
                if (_currentUser != null)
                {
                    _currentUser.FirstName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FullName));
                    OnPropertyChanged(nameof(Initials));
                }
            }
        }

        public string LastName
        {
            get => _currentUser?.LastName ?? "";
            set
            {
                if (_currentUser != null)
                {
                    _currentUser.LastName = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FullName));
                    OnPropertyChanged(nameof(Initials));
                }
            }
        }

        public string Username
        {
            get => _currentUser?.Username ?? "";
            set
            {
                if (_currentUser != null)
                {
                    _currentUser.Username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _currentUser?.Email ?? "";
            set
            {
                if (_currentUser != null)
                {
                    _currentUser.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        public string Initials
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                    return "??";
                return $"{FirstName[0]}{LastName[0]}".ToUpper();
            }
        }

        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string ConfirmNewPassword
        {
            get => _confirmNewPassword;
            set => SetProperty(ref _confirmNewPassword, value);
        }

        public string SelectedFileName
        {
            get => _selectedFileName;
            set => SetProperty(ref _selectedFileName, value);
        }

        public bool HasSelectedFile
        {
            get => _hasSelectedFile;
            set => SetProperty(ref _hasSelectedFile, value);
        }

        public bool CanTakePhoto => MediaPicker.Default.IsCaptureSupported;

        public ICommand UpdateProfileCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand PickFileCommand { get; }
        public ICommand TakePhotoCommand { get; }
        public ICommand UploadFileCommand { get; }

        public ProfileViewModel()
        {
            _database = new DB();
            _currentUser = _database.GetCurrentUser();

            UpdateProfileCommand = new Command(async () => await ExecuteUpdateProfile());
            ChangePasswordCommand = new Command(async () => await ExecuteChangePassword());
            PickFileCommand = new Command(async () => await ExecutePickFile());
            TakePhotoCommand = new Command(async () => await ExecuteTakePhoto());
            UploadFileCommand = new Command(async () => await ExecuteUploadFile());
        }

        private async Task ExecuteUpdateProfile()
        {
            if (_currentUser == null) return;

            var success = await _database.UpdateUserProfileAsync(_currentUser);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Успех", "Профиль обновлен", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось обновить профиль", "OK");
            }
        }

        private async Task ExecuteChangePassword()
        {
            if (_currentUser == null) return;

            if (string.IsNullOrWhiteSpace(CurrentPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Заполните все поля", "OK");
                return;
            }

            if (NewPassword != ConfirmNewPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
                return;
            }

            var success = await _database.ChangePasswordAsync(_currentUser.Id, CurrentPassword, NewPassword);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Успех", "Пароль изменен", "OK");
                CurrentPassword = string.Empty;
                NewPassword = string.Empty;
                ConfirmNewPassword = string.Empty;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Неверный текущий пароль", "OK");
            }
        }

        private async Task ExecutePickFile()
        {
            try
            {
                var fileResult = await FilePicker.Default.PickAsync();
                if (fileResult != null)
                {
                    _selectedFile = fileResult;
                    SelectedFileName = $"Выбран файл: {fileResult.FileName}";
                    HasSelectedFile = true;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось выбрать файл: {ex.Message}", "OK");
            }
        }

        private async Task ExecuteTakePhoto()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        _selectedFile = photo;
                        SelectedFileName = $"Сделано фото: {photo.FileName}";
                        HasSelectedFile = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось сделать фото: {ex.Message}", "OK");
            }
        }

        private async Task ExecuteUploadFile()
        {
            if (_selectedFile == null) return;

            try
            {
                var savedPath = await _database.SaveFileAsync(_selectedFile);
                if (!string.IsNullOrEmpty(savedPath))
                {
                    await Application.Current.MainPage.DisplayAlert("Успех", $"Файл сохранен: {savedPath}", "OK");
                    SelectedFileName = string.Empty;
                    HasSelectedFile = false;
                    _selectedFile = null;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось сохранить файл", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка загрузки: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}