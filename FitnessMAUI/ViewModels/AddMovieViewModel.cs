using FitnessMAUI.db;
using FitnessMAUI.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitnessMAUI.ViewModels
{
    public class AddMovieViewModel : BaseViewModel
    {
        private readonly DB _database;
        private Movie _movie;
        private Studio _selectedStudio;
        private string _selectedType;
        private bool _isEditMode;

        public Movie Movie
        {
            get => _movie;
            set => SetProperty(ref _movie, value);
        }

        public Studio SelectedStudio
        {
            get => _selectedStudio;
            set
            {
                SetProperty(ref _selectedStudio, value);
                if (value != null && Movie != null)
                {
                    Movie.StudioId = value.Id;
                }
            }
        }

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                SetProperty(ref _selectedType, value);
                if (Movie != null)
                {
                    Movie.Type = value;
                }
            }
        }

        public string PageTitle => _isEditMode ? "Редактировать фильм" : "Добавить фильм";

        public ObservableCollection<Studio> Studios { get; } = new ObservableCollection<Studio>();
        public ObservableCollection<string> Types { get; } = new ObservableCollection<string>
        {
            "Фильм",
            "Сериал",
            "Мультфильм",
            "Документальный"
        };

        public bool CanTakePhoto => MediaPicker.Default.IsCaptureSupported;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand PickImageCommand { get; }
        public ICommand TakePhotoCommand { get; }

        public AddMovieViewModel()
        {
            _database = new DB();
            Movie = new Movie
            {
                ReleaseDate = DateTime.Now,
                Rating = 5.0m 
            };

            SaveCommand = new Command(async () => await ExecuteSave());
            CancelCommand = new Command(async () => await ExecuteCancel());
            PickImageCommand = new Command(async () => await ExecutePickImage());
            TakePhotoCommand = new Command(async () => await ExecuteTakePhoto());

           
            Task.Run(async () => await LoadStudios());
        }

        private async Task LoadStudios()
        {
            try
            {
                var studios = await _database.GetStudios();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Studios.Clear();
                    foreach (var studio in studios)
                    {
                        Studios.Add(studio);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки студий: {ex.Message}");
            }
        }

        public void SetMovieForEdit(Movie movie)
        {
            if (movie != null)
            {
                Movie = movie;
                _isEditMode = true;
                SelectedType = movie.Type;

                OnPropertyChanged(nameof(PageTitle));
            }
        }

        private async Task ExecuteSave()
        {
            if (string.IsNullOrWhiteSpace(Movie.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите название фильма", "OK");
                return;
            }

            try
            {
                if (_isEditMode)
                {
                    await _database.EditMovieAsync(Movie);
                    await Application.Current.MainPage.DisplayAlert("Успех", "Фильм обновлен", "OK");
                }
                else
                {
                    await _database.AddMovieAsync(Movie);
                    await Application.Current.MainPage.DisplayAlert("Успех", "Фильм добавлен", "OK");
                }

                // Возвращаемся назад
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось сохранить фильм: {ex.Message}", "OK");
            }
        }

        private async Task ExecuteCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task ExecutePickImage()
        {
            try
            {
                var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите постер фильма",
                    FileTypes = FilePickerFileType.Images
                });

                if (fileResult != null)
                {
                    Movie.ImageUrl = fileResult.FullPath;
                    OnPropertyChanged(nameof(Movie));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось выбрать изображение: {ex.Message}", "OK");
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
                        Movie.ImageUrl = photo.FullPath;
                        OnPropertyChanged(nameof(Movie));
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось сделать фото: {ex.Message}", "OK");
            }
        }
    }
}