using FitnessMAUI.ViewModels;

namespace FitnessMAUI.Views
{
    public partial class MoviesPage : ContentPage
    {
        public MoviesPage()
        {
            InitializeComponent();
            BindingContext = new MoviesViewModel();
        }
    }
}