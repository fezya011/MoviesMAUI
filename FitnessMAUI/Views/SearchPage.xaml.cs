using FitnessMAUI.ViewModels;

namespace FitnessMAUI.Views
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel();
        }
    }
}