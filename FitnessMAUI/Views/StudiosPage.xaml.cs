using FitnessMAUI.ViewModels;

namespace FitnessMAUI.Views
{
    public partial class StudiosPage : ContentPage
    {
        public StudiosPage()
        {
            InitializeComponent();
            BindingContext = new StudiosViewModel();
        }
    }
}