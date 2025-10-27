using FitnessMAUI.db;
using FitnessMAUI.Model;
using FitnessMAUI.VMTools;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FitnessMAUI;

public partial class AddEditMoviePage : ContentPage
{
    private Movie? addMovie;
    private List<Studio> studios = new List<Studio>();
    private List<string> types;
    private string selectedType;
    private DB dB;
    private Studio selectedStudio;
    private int movieId;

    public Movie? AddMovie 
    { 
        get => addMovie; 
        set
        {
            addMovie = value;
            OnPropertyChanged();
        }
    }


    public List<Studio> Studios 
    { 
        get => studios; 
        set
        {
            studios = value;
            OnPropertyChanged();
        }
    }

    public List<string> Types 
    { 
        get => types; 
        set
        {
            types = value;
            OnPropertyChanged();
        }
    }

    public string SelectedType 
    { 
        get => selectedType;
        set
        {
            selectedType = value;
            OnPropertyChanged();
        } 
    }

    public Studio SelectedStudio 
    { 
        get => selectedStudio; 
        set
        {
            selectedStudio = value;
            OnPropertyChanged();
        }
    }

    public AddEditMoviePage(DB dB, int movieId)
	{
         
        this.dB = dB;
        Types = new List<string> { "Популярные", "Топ рейтинга", "Скоро в прокате"};
        InitializeComponent();
        this.movieId = movieId;
        BindingContext = this;

        LoadData();
    }

    private async void LoadData()
    {
        Studios = await dB.GetStudios();

        SearchMovie(movieId);
    }

   
    private async void SearchMovie(int movieId)
    {
        AddMovie = await dB.SearchMovieById(movieId);
        if (AddMovie == null)
        {
            AddMovie = new Movie();
            return;
        }
        SelectedType = Types.FirstOrDefault(t => t == AddMovie.Type);
        SelectedStudio = Studios.FirstOrDefault(s => s.Id == AddMovie.StudioId);
    }

    private async void SaveClick(object sender, EventArgs e)
    {
        try
        {
            if (AddMovie.Id == 0)
            {
                AddMovie.Rating = Math.Round(AddMovie.Rating, 1);
                AddMovie.Type = SelectedType;
                AddMovie.Studio = SelectedStudio;
                AddMovie.StudioId = SelectedStudio.Id;

                await dB.AddMovieAsync(AddMovie);

                await Navigation.PopToRootAsync();
            }
            else
            {
                AddMovie.Rating = Math.Round(AddMovie.Rating, 1);
                AddMovie.Type = SelectedType;
                AddMovie.Studio = SelectedStudio;
                AddMovie.StudioId = SelectedStudio.Id;
                await dB.EditMovieAsync(AddMovie);
                OnPropertyChanged(nameof(AddMovie));
                await Navigation.PopToRootAsync();
            }
            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось сохранить фильм: {ex.Message}", "OK");
        }
    }

    void OnSliderValue(object sender, ValueChangedEventArgs e)
    {   
        header.Text = $"Выбрано: {e.NewValue:F1}";
    }
}