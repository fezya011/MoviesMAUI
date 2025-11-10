using System.Windows.Input;

namespace FitnessMAUI.Controls
{
    public partial class SearchHeaderView : ContentView
    {
        public static readonly BindableProperty AddMovieCommandProperty =
            BindableProperty.Create(nameof(AddMovieCommand), typeof(ICommand), typeof(SearchHeaderView));

        public static readonly BindableProperty AddStudioCommandProperty =
            BindableProperty.Create(nameof(AddStudioCommand), typeof(ICommand), typeof(SearchHeaderView));

        public ICommand AddMovieCommand
        {
            get => (ICommand)GetValue(AddMovieCommandProperty);
            set => SetValue(AddMovieCommandProperty, value);
        }

        public ICommand AddStudioCommand
        {
            get => (ICommand)GetValue(AddStudioCommandProperty);
            set => SetValue(AddStudioCommandProperty, value);
        }

        public SearchHeaderView()
        {
            InitializeComponent();
        }
    }
}