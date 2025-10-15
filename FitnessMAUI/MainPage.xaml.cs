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
            
            dB = new DB();
            GetListsSort();



        }

        public async void GetListsSort()
        {
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
            await Navigation.PushAsync(new NewPage1(dB));
        }
    }
}