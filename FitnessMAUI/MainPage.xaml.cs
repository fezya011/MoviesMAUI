using FitnessMAUI.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitnessMAUI
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Movie2> PopularMovies { get; set; }
        public ObservableCollection<Movie2> ComingSoonMovies { get; set; }
        public ObservableCollection<Movie2> TopRatedMovies { get; set; }

        public ICommand MovieTappedCommand { get; }

        public MainPage()
        {
            InitializeComponent();

            MovieTappedCommand = new Command<Movie2>(OnMovieTapped);

            LoadSampleData();
            BindingContext = this;
        }

        private void LoadSampleData()
        {
            // Популярные фильмы
            PopularMovies = new ObservableCollection<Movie2>
            {
                new Movie2
                {
                    Title = "Довод",
                    Rating = "8.1",
                    Genres = "Боевик, Фантастика",
                    ImageUrl = "movie1.jpg",
                    Year = "2020"
                },
                new Movie2
                {
                    Title = "Интерстеллар",
                    Rating = "8.6",
                    Genres = "Фантастика, Драма",
                    ImageUrl = "movie2.jpg",
                    Year = "2014"
                },
                new Movie2
                {
                    Title = "Начало",
                    Rating = "8.7",
                    Genres = "Фантастика, Боевик",
                    ImageUrl = "movie3.jpg",
                    Year = "2010"
                }
            };

            // Скоро выйдет
            ComingSoonMovies = new ObservableCollection<Movie2>
            {
                new Movie2 { Title = "Аватар 3", ReleaseDate = "15 дек", ImageUrl = "avatar3.jpg" },
                new Movie2 { Title = "Миссия невыполнима", ReleaseDate = "20 янв", ImageUrl = "mission.jpg" },
                new Movie2 { Title = "Дюна 2", ReleaseDate = "5 фев", ImageUrl = "dune2.jpg" }
            };

            // Лучшие фильмы
            TopRatedMovies = new ObservableCollection<Movie2>
            {
                new Movie2 { Title = "Побег из Шоушенка", Rating = "9.1", Year = "1994", Genres = "Драма", ImageUrl = "shawshank.jpg" },
                new Movie2 { Title = "Крестный отец", Rating = "9.0", Year = "1972", Genres = "Драма, Криминал", ImageUrl = "godfather.jpg" },
                new Movie2 { Title = "Темный рыцарь", Rating = "8.9", Year = "2008", Genres = "Боевик, Драма", ImageUrl = "darkknight.jpg" }
            };
        }

        private async void OnMovieTapped(Movie2 movie)
        {
            if (movie != null)
            {
                await DisplayAlert("Фильм выбран", $"Вы выбрали: {movie.Title}", "OK");
                // Здесь можно перейти на страницу деталей фильма
                // await Navigation.PushAsync(new MovieDetailPage(movie));
            }
        }
    }

    public class Movie2
    {
        public string Title { get; set; }
        public string Rating { get; set; }
        public string Genres { get; set; }
        public string ImageUrl { get; set; }
        public string Year { get; set; }
        public string ReleaseDate { get; set; }
    }
}
