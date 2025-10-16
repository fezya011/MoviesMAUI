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

    public event PropertyChangedEventHandler? PropertyChanged;
    void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    public Movie AddMovie 
    { 
        get => addMovie; 
        set
        {
            addMovie = value;
            Signal();
        }
    }


    public List<Studio> Studios 
    { 
        get => studios; 
        set
        {
            studios = value;
            Signal();
        }
    }

    public List<string> Types 
    { 
        get => types; 
        set
        {
            types = value;
            Signal();
        }
    }

    public string SelectedType 
    { 
        get => selectedType;
        set
        {
            selectedType = value;
            Signal();
        } 
    }

    public NewPage1(DB dB)
	{
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

            await DB.Instance.AddMovieAsync(AddMovie);

            await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось сохранить фильм: {ex.Message}", "OK");
        }
    }
}