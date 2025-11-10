namespace FitnessMAUI.Controls
{
    public partial class MovieCardView : ContentView
    {
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(MovieCardView), string.Empty);

        public static readonly BindableProperty RatingProperty =
            BindableProperty.Create(nameof(Rating), typeof(decimal), typeof(MovieCardView), 0m);

        public static readonly BindableProperty GenresProperty =
            BindableProperty.Create(nameof(Genres), typeof(string), typeof(MovieCardView), string.Empty);

        public static readonly BindableProperty ImageUrlProperty =
            BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(MovieCardView), string.Empty);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public decimal Rating
        {
            get => (decimal)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }

        public string Genres
        {
            get => (string)GetValue(GenresProperty);
            set => SetValue(GenresProperty, value);
        }

        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }

        public MovieCardView()
        {
            InitializeComponent();
        }
    }
}