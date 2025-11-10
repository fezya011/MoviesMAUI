using FitnessMAUI.db;
using FitnessMAUI.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FitnessMAUI.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly DB _database;
        private string _searchQuery;
        private ObservableCollection<Movie> _searchResults;
        private bool _hasSearchResults;

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public ObservableCollection<Movie> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        public bool HasSearchResults
        {
            get => _hasSearchResults;
            set => SetProperty(ref _hasSearchResults, value);
        }

        public ICommand SearchCommand { get; }

        public SearchViewModel()
        {
            _database = new DB();
            SearchResults = new ObservableCollection<Movie>();
            SearchCommand = new Command(async () => await ExecuteSearch());
        }

        private async Task ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                SearchResults.Clear();
                HasSearchResults = false;
                return;
            }

            var allMovies = await _database.GetMovies();
            var results = allMovies.Where(m =>
                m.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                m.Genres.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                m.Type.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            SearchResults.Clear();
            foreach (var movie in results)
            {
                SearchResults.Add(movie);
            }

            HasSearchResults = SearchResults.Any();
        }
    }
}