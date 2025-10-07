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
        public ObservableCollection<Movie> PopularMovies { get; set; }
        public ObservableCollection<Movie> ComingSoonMovies { get; set; }
        public ObservableCollection<Movie> TopRatedMovies { get; set; }

        public ICommand MovieTappedCommand { get; }
       
        public MainPage()
        {
            InitializeComponent();
            MovieTappedCommand = new Command<Movie>(OnMovieTapped);
            
            DB.Instance.InitializeAsync();

            Load();

           
        }

        public void Load()
        {
            PopularMovies = new ObservableCollection<Movie>
            {
                new Movie
                {
                    Title = "Довод",
                    Rating = "8.1",
                    Genres = "Боевик, Фантастика",
                    ImageUrl = "123.jpg",
                },
                new Movie
                {
                    Title = "Интерстеллар",
                    Rating = "8.6",
                    Genres = "Фантастика, Драма",
                    ImageUrl = "123.jpg",
                },
                new Movie
                {
                    Title = "Начало",
                    Rating = "8.7",
                    Genres = "Фантастика, Боевик",
                    ImageUrl = "123.jpg",
                }
            };

            ComingSoonMovies = new ObservableCollection<Movie>
            {
                new Movie { Title = "Аватар 3", ImageUrl = "123.jpg", },
                new Movie { Title = "Миссия невыполнима", ImageUrl = "123.jpg",  },
                new Movie { Title = "Дюна 2", ImageUrl = "123.jpg", }
            };

            TopRatedMovies = new ObservableCollection<Movie>
            {
                new Movie { Title = "Побег из Шоушенка", Rating = "9.1", Genres = "Драма", ImageUrl = "123.jpg",  },
                new Movie { Title = "Крестный отец", Rating = "9.0",  Genres = "Драма, Криминал", ImageUrl = "123.jpg", },
                new Movie { Title = "Темный рыцарь", Rating = "8.9",  Genres = "Боевик, Драма", ImageUrl = "123.jpg", }
            };
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
            await Navigation.PushAsync(new NewPage1());
        }
    }
}