using FitnessMAUI.db;
using FitnessMAUI.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitnessMAUI.ViewModels
{
    public class StudiosViewModel : BaseViewModel
    {
        private readonly DB _database;
        private ObservableCollection<Studio> _studios;
        private Studio _selectedStudio;
        private bool _isRefreshing;

        public ObservableCollection<Studio> Studios
        {
            get => _studios;
            set => SetProperty(ref _studios, value);
        }

        public Studio SelectedStudio
        {
            get => _selectedStudio;
            set => SetProperty(ref _selectedStudio, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddStudioCommand { get; }
        public ICommand DeleteStudioCommand { get; }

        public StudiosViewModel()
        {
            _database = new DB();
            Studios = new ObservableCollection<Studio>();

            RefreshCommand = new Command(async () => await LoadStudios());
            AddStudioCommand = new Command(ExecuteAddStudio);
            DeleteStudioCommand = new Command<Studio>(ExecuteDeleteStudio);

            LoadStudios();
        }

        private async Task LoadStudios()
        {
            IsRefreshing = true;
            var studios = await _database.GetStudios();
            Studios.Clear();
            foreach (var studio in studios)
            {
                Studios.Add(studio);
            }
            IsRefreshing = false;
        }

        private async void ExecuteAddStudio()
        {
            await Shell.Current.GoToAsync(nameof(AddStudioPage));
        }

        private async void ExecuteDeleteStudio(Studio studio)
        {
            if (studio != null)
            {
                bool result = await Application.Current.MainPage.DisplayAlert(
                    "Удаление",
                    $"Вы уверены, что хотите удалить студию '{studio.Name}'?",
                    "Да", "Нет");

                if (result)
                {
                    await _database.DeleteStudioAsync(studio);
                    await LoadStudios();
                }
            }
        }
    }
}