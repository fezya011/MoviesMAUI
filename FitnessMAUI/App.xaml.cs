using FitnessMAUI.Views;

namespace FitnessMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();          
            MainPage = new AppShell();
        }
    }
}