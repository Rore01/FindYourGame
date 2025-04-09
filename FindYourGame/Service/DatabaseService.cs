using SQLite;
using FindYourGame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindYourGame.Service
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;  // SQLite connection instance

        // Constructor: Initializes SQLiteAsyncConnection with a path to the database file
        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Game>().Wait();  // Ensures the 'Game' table is created if it doesn't exist
        }

        // Method to save a game to the database (inserts the game into the 'Game' table)
        public Task<int> SaveGameAsync(Game game)
        {
            return _database.InsertAsync(game);  // Inserts a game record asynchronously
        }

        // Method to get the list of saved games from the database (retrieves all games)
        public Task<List<Game>> GetSavedGamesAsync()
        {
            return _database.Table<Game>().ToListAsync();  // Retrieves all game records asynchronously
        }

        // Method to delete a game from the database (removes the specified game record)
        public Task<int> DeleteGameAsync(Game game)
        {
            return _database.DeleteAsync(game);  // Deletes the specified game record asynchronously
        }
    }
}
