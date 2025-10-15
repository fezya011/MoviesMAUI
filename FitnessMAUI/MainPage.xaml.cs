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
        public List<Movie> PopularMovies { get; set; }
        public ObservableCollection<Movie> ComingSoonMovies { get; set; }
        public ObservableCollection<Movie> TopRatedMovies { get; set; }


        
        public ICommand MovieTappedCommand { get; }
       
        public MainPage()
        {
            InitializeComponent();
            MovieTappedCommand = new Command<Movie>(OnMovieTapped);
            
            dB = new DB();

            PopularMovies.Add( DB.Instance.GetMovies());

           
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