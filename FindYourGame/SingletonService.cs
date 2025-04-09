using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindYourGame
{
    public class SingletonService
    {
        private static SingletonService _instance;
        public static SingletonService Instance => _instance ??= new SingletonService();

        // Store the last search text
        public string LastSearchText { get; set; }

        // Store a list of previous searches
        public List<string> PreviousSearches { get; set; } = new List<string>();

        public bool IsBackNavigation { get; set; }

        private SingletonService() { }

        // Add a new search term to the list of previous searches
        public void AddSearchTerm(string searchTerm)
        {
            if (!PreviousSearches.Contains(searchTerm))
            {
                PreviousSearches.Insert(0, searchTerm);  // Insert at the start for most recent first
            }

            // Limit the number of saved searches (e.g., keep only the last 5)
            if (PreviousSearches.Count > 3)
            {
                PreviousSearches.RemoveAt(3);  // Remove the oldest search term if the list exceeds the limit
            }
        }
    }


}
