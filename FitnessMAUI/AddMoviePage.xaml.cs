using FitnessMAUI.db;
using FitnessMAUI.Model;
using FitnessMAUI.VMTools;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FitnessMAUI;

public partial class NewPage1 : ContentPage
{
    private Movie addMovie;

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


    public List<Studio> Studios { get; set; }

    public List<Movie> Studios2 { get; set; }


    public NewPage1()
	{
        DB.Instance.InitializeAsync();
        InitializeComponent();
        AddMovie = new Movie();
        BindingContext = this;

        Studios2 = DB.Instance.GetComingSoonMovies();
        
	}

    private void SaveClick(object sender, EventArgs e)
    {
        DB.Instance.AddComingSoonMovieAsync(AddMovie);
    }
}