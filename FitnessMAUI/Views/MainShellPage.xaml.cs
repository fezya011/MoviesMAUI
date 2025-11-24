using FitnessMAUI.db;
using FitnessMAUI.Model;
using System.Windows.Input;

namespace FitnessMAUI.Views;

public partial class MainShellPage : ContentPage
{
    DB dB;
    private Movie selectedMovie;

    public List<Movie> PopularMovies { get; set; } = new List<Movie>();
    public List<Movie> ComingSoonMovies { get; set; } = new List<Movie>();
    public List<Movie> TopRatedMovies { get; set; } = new List<Movie>();

    public Movie SelectedMovie
    {
        get => selectedMovie;
        set
        {
            selectedMovie = value;
            OnPropertyChanged();
        }
    }

    public ICommand MovieTappedCommand { get; }
    public ICommand DeleteMovieCommand { get; }
    public ICommand EditMovieCommand { get; }

    public MainShellPage()
	{
		InitializeComponent();
        dB = new DB();

        MovieTappedCommand = new Command<Movie>(OnMovieTapped);
        DeleteMovieCommand = new Command<Movie>(DeleteMovie);
        EditMovieCommand = new Command<Movie>(EditMovie);
        BindingContext = this;
    }
    

    public async void GetListsSort()
    {
        PopularMovies = new List<Movie>();
        ComingSoonMovies = new List<Movie>();
        TopRatedMovies = new List<Movie>();

        var lists = await dB.GetMovies();
        foreach (var movie in lists)
        {

            if (movie.Type == "Популярные")
                PopularMovies.Add(movie);
            else if (movie.Type == "Топ рейтинга")
                TopRatedMovies.Add(movie);
            else if (movie.Type == "Скоро в прокате")
                ComingSoonMovies.Add(movie);
        }
        OnPropertyChanged(nameof(PopularMovies));
        OnPropertyChanged(nameof(ComingSoonMovies));
        OnPropertyChanged(nameof(TopRatedMovies));
    }

    private async void OnMovieTapped(Movie movie)
    {
        if (movie != null)
        {
            await DisplayAlert("Фильм выбран", $"Вы выбрали: {movie.Title}", "OK");
        }
    }

    private async void DeleteMovie(Movie movie)
    {
        if (movie != null)
        {
            bool result = await Application.Current.MainPage.DisplayAlert(
                "Удаление",
                $"Вы уверены, что хотите удалить фильм '{movie.Title}'?",
                "Да", "Нет");
            if (result)
            {
                try
                {
                    await dB.DeleteMovieAsync(SelectedMovie);
                    GetListsSort();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось удалить фильм: {ex.Message}", "OK");
                }
            }
        }    
    }
    private async void EditMovie(Movie movie)
    {
        if (movie != null)
        {
            bool result = await Application.Current.MainPage.DisplayAlert(
                "Редактирование",
                $"Вы уверены, что хотите отредактировать фильм '{movie.Title}'?",
                "Да", "Нет");
            if (result)
                try
                {
                    var editMoviePage = new AddEditMoviePage(dB, SelectedMovie.Id);
                    await Navigation.PushAsync(editMoviePage);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось удалить фильм: {ex.Message}", "OK");
                }
        }           
    }

    protected override void OnAppearing()
    {
        GetListsSort();
    }

    private async void OpenAddStudioPage(object sender, EventArgs e)
    {
        var addStudioPage = new AddStudioPage(dB);
        await Navigation.PushAsync(addStudioPage);
    }

    private async void AddEditMoviePage(object sender, EventArgs e)
    {
        var addMoviePage = new AddEditMoviePage(dB, 0);
        await Navigation.PushAsync(addMoviePage);
    }

    private void OnBurgerMenuClicked(object sender, EventArgs e)
    {
        if (App.Current.MainPage is Shell shell)
        {
            shell.FlyoutIsPresented = !shell.FlyoutIsPresented;
        }
    }
}