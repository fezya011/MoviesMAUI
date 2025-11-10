using FitnessMAUI.ViewModels;

namespace FitnessMAUI.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is RegisterViewModel viewModel)
            {
                viewModel.ResetFields();
            }
        }
    }
}