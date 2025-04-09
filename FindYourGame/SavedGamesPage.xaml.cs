using FindYourGame.Models;
using FindYourGame.Service;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace FindYourGame
{
    public partial class SavedGamesPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Game> SavedGames { get; set; }

        public SavedGamesPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            SavedGames = new ObservableCollection<Game>();
            BindingContext = this;
            LoadSavedGames();
        }

        private async void LoadSavedGames()
        {
            var games = await _databaseService.GetSavedGamesAsync();
            foreach (var game in games)
            {
                SavedGames.Add(game);
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            SingletonService.Instance.IsBackNavigation = true;

            // Clear the previous search text
            SingletonService.Instance.LastSearchText = string.Empty;

            // Navigate back to the MainPage
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            // Get the button that was clicked
            var button = sender as Button;

            var game = button?.BindingContext as Game;

            if (game != null)
            {
                // Remove from the ObservableCollection
                SavedGames.Remove(game);

                // Also delete from the database
                await _databaseService.DeleteGameAsync(game);
            }
        }
    }
}
