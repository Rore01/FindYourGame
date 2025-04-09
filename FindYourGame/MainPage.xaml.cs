using FindYourGame.Models;
using FindYourGame.Service;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace FindYourGame
{
    public partial class MainPage : ContentPage
    {
        private readonly WebScrapingService _webScrapingService;
        private readonly DatabaseService _databaseService; 
        public ObservableCollection<Game> SearchResults { get; set; }

        public MainPage()
        {
            InitializeComponent();
            _webScrapingService = new WebScrapingService();
            _databaseService = App.Database; 

            SearchResults = new ObservableCollection<Game>();
            BindingContext = this; 
            UpdateSearchHistory();

        }

        private void UpdateSearchHistory()
        {
            SearchHistoryStack.Children.Clear();

            foreach (var searchTerm in SingletonService.Instance.PreviousSearches)
            {
                var button = new Button
                {
                    Text = searchTerm,
                    BackgroundColor = Color.FromArgb("#D3D3D3"),
                    TextColor = Color.FromArgb("#000000"),
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                button.Clicked += (sender, args) =>
                {
                    GameSearchEntry.Text = searchTerm;
                };

                SearchHistoryStack.Children.Add(button);
            }

            SearchHistoryStack.IsVisible = SingletonService.Instance.PreviousSearches.Any();
        }
        private void GameSearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(GameSearchEntry.Text))
            {
                SearchHistoryStack.IsVisible = false;
            }
            else
            {
                SearchHistoryStack.IsVisible = true;
                UpdateSearchHistory();
            }
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            var gameName = GameSearchEntry.Text;

            if (!string.IsNullOrWhiteSpace(gameName))
            {
                SingletonService.Instance.LastSearchText = gameName;
                SingletonService.Instance.AddSearchTerm(gameName);

                var game = await _webScrapingService.SearchGameOnGOGAsync(gameName);

                if (game != null)
                {
                    bool addToList = await DisplayAlert("Game Found",
                        $"Name: {game.Name}\nPrice: {game.Price}\nDescription: {game.Description}\nRating: {game.Rating}",
                        "Add To List", "OK");

                    if (addToList)
                    {
                        await _databaseService.SaveGameAsync(game);
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No game found matching your search.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please enter a game name to search.", "OK");
            }
            UpdateSearchHistory();
        }
        private async void OnListButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedGamesPage(_databaseService)); 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (SingletonService.Instance.IsBackNavigation)
            {
                GameSearchEntry.Text = SingletonService.Instance.LastSearchText;

                SingletonService.Instance.IsBackNavigation = false;
            }
            
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            var game = button?.BindingContext as Game;

            if (game != null)
            {
                SearchResults.Remove(game);

                await _databaseService.DeleteGameAsync(game);
            }
        }
    }
}
