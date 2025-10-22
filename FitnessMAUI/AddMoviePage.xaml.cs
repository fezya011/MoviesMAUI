using FitnessMAUI.db;
using FitnessMAUI.Model;
using FitnessMAUI.VMTools;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FitnessMAUI;

public partial class NewPage1 : ContentPage
{
    private Movie addMovie;
    private List<Studio> studios = new List<Studio>();
    private List<string> types;
    private string selectedType;
    private DB dB;
    private Studio selectedStudio;

    public Movie AddMovie 
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

    public NewPage1(DB dB)
	{
        this.dB = dB;
        Types = new List<string> { "Популярные", "Топ рейтинга", "Скоро в прокате"};
        InitializeComponent();
        AddMovie = new Movie();
        BindingContext = this;
        LoadStudios();

    }

    private async void LoadStudios()
    {
        Studios = await dB.GetStudios();
    }

    private async void SaveClick(object sender, EventArgs e)
    {
        try
        {
            AddMovie.Rating = Math.Round(AddMovie.Rating, 1);
            AddMovie.Type = SelectedType;
            AddMovie.Studio = SelectedStudio;

            await dB.AddMovieAsync(AddMovie);

            await Navigation.PopToRootAsync();
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