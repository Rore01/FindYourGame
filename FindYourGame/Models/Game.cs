using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FindYourGame.Models
{
    public class Game
    {
        [PrimaryKey, AutoIncrement]  // Set this property as the primary key and auto-incremented
        public int Id { get; set; }

        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
    }
}

