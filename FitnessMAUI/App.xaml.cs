using FitnessMAUI.Views;

namespace FitnessMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            
            MainPage = new LoginPage();
        }

        public static void NavigateToMainApp()
        {
            
            Current.MainPage = new AppShell();
        }

        public static void NavigateToLogin()
        {
            
            Current.MainPage = new LoginPage();
        }
    }
}