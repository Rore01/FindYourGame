using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using FindYourGame.Models;
using System.Collections;
using System.Globalization;


namespace FindYourGame.Service
{
    internal class WebScrapingService
    {
        private readonly HttpClient _httpClient;

        public WebScrapingService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Game> SearchGameOnGOGAsync(string gameName)
        {
            Game game = null;
            string formattedGameName = FormatGameName(gameName);
            string searchUrl = $"https://www.greenmangaming.com/games/{Uri.EscapeDataString(formattedGameName)}";

            try
            {
                var response = await _httpClient.GetStringAsync(searchUrl);
                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response);

                // XPath for game name
                var nameNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"pdp-title\"]/h1");

                // XPath for game price
                var priceNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"pdp-purchase\"]/div[8]/div[1]/div/div[2]/gmgprice");

                // XPath for game description
                var descriptionNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"pdp-game-description\"]/show-more");

                // XPath for game rating (updated with your provided XPath)
                var ratingNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"pdp-title\"]/ul/li[1]/span");

                if (nameNode != null && priceNode != null)
                {
                    game = new Game
                    {
                        Name = nameNode.InnerText.Trim(),
                        Price = priceNode.InnerText.Trim(),
                        Description = descriptionNode?.InnerText.Trim() ?? "No description available",
                        Rating = ratingNode?.InnerText.Trim() ?? "No rating available" // Use the rating XPath here
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle any errors during scraping
                Console.WriteLine($"Error: {ex.Message}");
            }

            return game;
        }
        private string FormatGameName(string gameName)
        {
            // replace spaces with underscores
            string formattedName = gameName.ToLower().Replace(" ", "-");

            // Capitalize the first letter of each word
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            return textInfo.ToTitleCase(formattedName);
        }
    }
}

