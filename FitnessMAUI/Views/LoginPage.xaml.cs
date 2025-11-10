using FitnessMAUI.ViewModels;

namespace FitnessMAUI.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (BindingContext is LoginViewModel viewModel)
            {
                viewModel.ResetFields();
            }
        }
    }
}