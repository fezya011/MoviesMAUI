using FitnessMAUI.Model;
using FitnessMAUI.db;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace FitnessMAUI
{
    public partial class MainPage : ContentPage
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
       
       
        public MainPage()
        {
            InitializeComponent();
            dB = new DB();

            MovieTappedCommand = new Command<Movie>(OnMovieTapped);
            BindingContext = this;



        }


        public async void GetListsSort()
        {
            PopularMovies = new List<Movie>();
            ComingSoonMovies = new List<Movie>();
            TopRatedMovies = new List<Movie>();
            OnPropertyChanged(nameof(PopularMovies));
            OnPropertyChanged(nameof(ComingSoonMovies));
            OnPropertyChanged(nameof(TopRatedMovies));
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

        private async void OpenAddMoviePage(object sender, EventArgs e)
        {
            var addMoviePage = new AddEditMoviePage(dB, 0);
            await Navigation.PushAsync(addMoviePage);
            
        }

        private async void EditComingSoonMovieButton(object sender, EventArgs e)
        {
            var editMoviePage = new AddEditMoviePage(dB, SelectedMovie.Id);
            await Navigation.PushAsync(editMoviePage);

        }
        private async void EditTopRatedMovieButton(object sender, EventArgs e)
        {
            var editMoviePage = new AddEditMoviePage(dB, SelectedMovie.Id);
            await Navigation.PushAsync(editMoviePage);
            
        }

        protected override void OnAppearing()
        {
            GetListsSort();
        }

        private void DeleteComingSoonMovieButton(object sender, EventArgs e)
        {
            dB.DeleteMovieAsync(SelectedMovie);
            GetListsSort();
        }

        private void DeleteTopRatedMovieButton(object sender, EventArgs e)
        {
            dB.DeleteMovieAsync(SelectedMovie);
            GetListsSort();
        }

        private async void OpenAddStudioPage(object sender, EventArgs e)
        {
            var addStudioPage = new AddStudioPage(dB);
            await Navigation.PushAsync(addStudioPage);
        }

        
    }
}