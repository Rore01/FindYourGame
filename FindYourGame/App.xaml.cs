using FindYourGame.Service;
using System;
using System.IO;
using Microsoft.Maui.Controls;

namespace FindYourGame
{
    public partial class App : Application
    {
        // Declare a static DatabaseService instance
        public static DatabaseService Database;

        public App()
        {
            InitializeComponent();

            // Set up the database path
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "games.db3");

            // Initialize the DatabaseService with the database path
            Database = new DatabaseService(dbPath);

            // Set the main page of the app
            MainPage = new AppShell();
        }
    }
}
