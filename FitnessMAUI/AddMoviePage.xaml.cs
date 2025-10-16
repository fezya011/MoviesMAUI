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
    private List<Studio> studios;
    private List<string> types;
    private string selectedType;
    private DB dB;


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

    public NewPage1(DB dB)
	{
        this.dB = dB;
        Types = new List<string> { "Популярные", "Топ рейтинга", "Скоро в прокате"};
        InitializeComponent();
        AddMovie = new Movie();
        BindingContext = this;
        
	}

    private async void SaveClick(object sender, EventArgs e)
    {
        try
        {
            AddMovie.Type = SelectedType;

            await dB.AddMovieAsync(AddMovie);

            await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось сохранить фильм: {ex.Message}", "OK");
        }
    }
}